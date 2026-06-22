using ClosedXML.Excel;
using DevExpress.Utils.Extensions;
using DevExpress.Utils.Text;
using DevExpress.Utils.Win;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using LineStatusClient.Common;
using LineStatusClient.DTOs;
using LineStatusClient.Forms;
using LineStatusClient.Forms.Email;
using LineStatusClient.Froms;
using LineStatusClient.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineStatusClient
{
    public partial class FormMain : XtraForm
    {
        private System.Windows.Forms.Timer timerRunAndDown;
        private System.Windows.Forms.Timer reconnectTimer;
        private bool isMonitoringReconnect = false;
        private SqlDependencyEx dependency; // Khai báo ở class
        private bool isRefresh = false;
        private readonly object reconnectLock = new object();
        private DisplayDataType _displayDataType = DisplayDataType.Downtime;

        private readonly List<Control> _downtimeControl;
        private readonly List<Control> _calltimeControl;

        public FormMain()
        {
            InitializeComponent();
            InitializeCheckStartUp();
            InitializeTimer();

            _downtimeControl = new List<Control>()
            {
                eldowntime_01,
                eldowntime_02,
                eldowntime_03,
                eldowntime_04,
                eldowntime_05,
                eldowntime_06,
                eldowntime_07,
                eldowntime_08,
            };
            _calltimeControl = new List<Control>()
            {
                elcalltime_01,
                elcalltime_02
            };

            cboDisplayDataType.DataSource = Enum.GetValues(typeof(DisplayDataType))
            .Cast<DisplayDataType>()
            .Select(x => new
            {
                Value = x,
                Text = x.GetDescription()
            })
            .ToList();

            cboDisplayDataType.DisplayMember = "Text";
            cboDisplayDataType.ValueMember = "Value";
            cboDisplayDataType.SelectedIndexChanged += cboDisplayDataType_SelectedIndexChanged;
        }

        private async void FormMain_Shown(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                try
                {
                    grvMain.ShowLoadingPanel();

                    Settings.ReadSQLConnectionString();
                    Settings.LoadConfig();
                    await LoadDataAsync();
                }
                finally
                {
                    grvMain.HideLoadingPanel();
                }
               
                InitializeSqlDependency(); // Không cần tạo Thread thủ công
            });

           
        }

        private async void cboDisplayDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDisplayDataType.SelectedValue is DisplayDataType selected)
            {
                _displayDataType = selected;

                // show hide control theo type 
                foreach (var item in _downtimeControl)
                {
                    item.Visible = selected == DisplayDataType.Downtime;
                }
                foreach (var item in _calltimeControl)
                {
                    item.Visible = selected == DisplayDataType.CallTime;
                }

                try
                {
                    grvMain.ShowLoadingPanel();

                    // 1. Stop old sql dependency
                    await Task.Run(() =>
                    {
                        StopSqlDependency();
                    });

                    // 2. Clear data and columns
                    grdMain.DataSource = null;
                    grvMain.Columns.Clear();
                    timerRunAndDown.Stop();

                    // 3. Create columns
                    switch (selected)
                    {
                        case DisplayDataType.Downtime:
                            timerRunAndDown.Start();

                            CreateDowntimeColumns();
                            break;

                        case DisplayDataType.CallTime:
                            CreateCallTimeColumns();
                            break;
                    }

                    // 4. Load data
                    await LoadDataFollowTypeAsync();
                }
                finally
                {
                    grvMain.HideLoadingPanel();
                }

                await Task.Run(() =>
                {
                    InitializeSqlDependency();
                });
            }
        }
        private async Task LoadDataFollowTypeAsync()
        {
            switch (_displayDataType)
            {
                case DisplayDataType.Downtime:
                    await LoadDataAsync();
                    break;
                case DisplayDataType.CallTime:
                    await LoadData_CallTimeAsync();
                    break;
            }
        }

        private void CreateDowntimeColumns()
        {
            var colSTT = grvMain.Columns.AddVisible("No", "STT");
            HAlignment(colSTT);
            colSTT.UnboundDataType = typeof(int);
            // STT: hiển thị giá trị lớn nhất (= tổng số dòng vì STT chạy tuần tự)
            colSTT.Summary.Add(DevExpress.Data.SummaryItemType.Max, "No", "{0}");

            grvMain.Columns.AddVisible("line_code", "Mã chuyền");
            grvMain.Columns.AddVisible("line_nm", "Tên chuyền");

            var colTimeRun = grvMain.Columns.AddVisible("TotalRunningTime", "Thời gian chạy");
            HAlignment(colTimeRun);

            var colTimeStop = grvMain.Columns.AddVisible("TotalDowntime", "Thời gian dừng");
            HAlignment(colTimeStop);

            var Downtime_col6 = grvMain.Columns.AddVisible("status_text", "Trạng thái");
            HAlignment(Downtime_col6);

            var Downtime_col7 = grvMain.Columns.AddVisible("product_count", "Số lượng sản phẩm");
            HAlignment(Downtime_col7);
            // Số lượng sản phẩm: tính tổng
            Downtime_col7.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "product_count", "Tổng = {0:0.##}");

            var Downtime_col8 = grvMain.Columns.AddVisible("shift_text", "Ca làm");
            HAlignment(Downtime_col8);

            // Bật hàng footer để hiển thị summary
            grvMain.OptionsView.ShowFooter = true;
        }

        private void CreateCallTimeColumns()
        {
            var colSTT = grvMain.Columns.AddVisible("No", "STT");
            HAlignment(colSTT);
            colSTT.UnboundDataType = typeof(int);

            grvMain.Columns.AddVisible("LineCode", "Mã chuyền");
            grvMain.Columns.AddVisible("LineName", "Tên chuyền");
            grvMain.Columns.AddVisible("Position", "Vị trí");
            var colCallCount = grvMain.Columns.AddVisible("CallCount", "Số lần gọi");
            HAlignment(colCallCount);

            var colCallTime = grvMain.Columns.AddVisible("CallTime", "T/G gọi gần nhất");
            DisplayAsDateTime(colCallTime);
        }

        private void DisplayAsDateTime(GridColumn column)
        {
            column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            column.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
        }

        private void HAlignment(GridColumn column, DevExpress.Utils.HorzAlignment alignment = DevExpress.Utils.HorzAlignment.Center)
        {
            column.AppearanceHeader.TextOptions.HAlignment = alignment;
            column.AppearanceCell.TextOptions.HAlignment = alignment;
        }

        private async void btnRefesh_Click(object sender, EventArgs e)
        {
            try
            {
                grvMain.ShowLoadingPanel();

                if (isRefresh) return;
                isRefresh = true;
                await Task.Delay(500);
                await LoadDataFollowTypeAsync();
                isRefresh = false;
            }
            finally
            {
                grvMain.HideLoadingPanel();
            }
        }

        #region LOAD DATA
        private async Task LoadDataAsync()
        {
            try
            {
                var data = await SQLHelper<Line_downtime_history_DTO>.ProcedureToListAsync("spGetLineStatus",
                      new string[] { },
                      new object[] { });

                grdMain.BeginInvoke(new Action(() =>
                {
                  

                    grdMain.DataSource = data;
                }));
            }
            catch (Exception ex)
            {
                ErrorLogger.Write(ex);
            }
        }

        #region NEW
        public class LineStatusResult
        {
            public long STT { get; set; }
            public string LineCode { get; set; }
            public string LineName { get; set; }
            public string Position { get; set; }
            public int CallCount { get; set; }
            public DateTime CallTime { get; set; }
        }
        private async Task LoadData_CallTimeAsync()
        {
            try
            {
                var data = await SQLHelper<LineStatusResult>.ProcedureToListAsync("sp_CallSubleaderHistory_Search",
                       new string[] { },
                       new object[] { });

                grdMain.BeginInvoke(new Action(() =>
                {
                    grdMain.DataSource = data;
                }));
            }
            catch (Exception ex)
            {
                ErrorLogger.Write(ex);
            }
        }
        #endregion

        /// <summary>
        /// Khởi tạo SqlDependencyEx để theo dõi thay đổi dữ liệu
        /// </summary>
        private void InitializeSqlDependency()
        {
            try
            {
                StopSqlDependency(); // Dừng dependency cũ nếu có

                var builder = new SqlConnectionStringBuilder(Settings.connectionString);
                string dbName = builder.InitialCatalog;
                int identity = SQLUtilities.GetUniqueIdentity();

                // Check type
                switch (_displayDataType)
                {
                    case DisplayDataType.Downtime:
                        dependency = new SqlDependencyEx(Settings.connectionString, dbName, "Line_downtime_history", identity: identity);
                        dependency.TableChanged += async (s, e) => await LoadDataAsync();
                        break;
                    case DisplayDataType.CallTime:
                        dependency = new SqlDependencyEx(Settings.connectionString, dbName, "CallSubleaderHistory", identity: identity);
                        dependency.TableChanged += async (s, e) => await LoadData_CallTimeAsync();
                        break;
                    default:
                        throw new Exception("Unsupported DisplayDataType for SqlDependencyEx");
                }

                //dependency = new SqlDependencyEx(Settings.connectionString, dbName, "Line_downtime_history", identity: identity);
                //dependency.TableChanged += (s, e) => LoadData();

                dependency.NotificationProcessStopped += (s, e) =>
                {
                    Invoke((MethodInvoker)StartReconnectMonitor);
                };

                dependency.Start();
                Invoke((MethodInvoker)StopReconnectMonitor);
            }
            catch (Exception ex)
            {
                ErrorLogger.SaveLog("Error in InitializeSqlDependency: ", ex.ToString());
                Invoke((MethodInvoker)StartReconnectMonitor);
            }
        }

        /// <summary>
        /// Dừng SqlDependencyEx nếu đang chạy
        /// </summary>
        private void StopSqlDependency()
        {
            if (dependency == null) return;

            try { dependency.Stop(); dependency.Dispose(); }
            catch (Exception ex)
            {
            }
            dependency = null;
        }

        /// <summary>
        /// Bắt đầu kiểm tra kết nối SQL định kỳ
        /// </summary>
        private void StartReconnectMonitor()
        {
            lock (reconnectLock)
            {
                if (isMonitoringReconnect) return;

                StopReconnectMonitor();

                reconnectTimer = new System.Windows.Forms.Timer
                {
                    Interval = 10000 // 10 seconds
                };
                reconnectTimer.Tick += ReconnectTimer_Tick;
                reconnectTimer.Start();

                isMonitoringReconnect = true;
            }
        }

        /// <summary>
        /// Dừng kiểm tra kết nối SQL
        /// </summary>
        private void StopReconnectMonitor()
        {
            try
            {
                reconnectTimer?.Stop();
                reconnectTimer?.Dispose();
            }
            catch (Exception)
            {
            }
            reconnectTimer = null;
            isMonitoringReconnect = false;
        }

        /// <summary>
        /// Sự kiện kiểm tra kết nối SQL định kỳ
        /// </summary>
        private void ReconnectTimer_Tick(object sender, EventArgs e)
        {
            if (SQLUtilities.CheckSQLConnection())
            {
                InitializeSqlDependency();
                StopReconnectMonitor();
            }
        }

        #endregion

        #region Count running and stopping time

        private void InitializeTimer()
        {
            timerRunAndDown = new System.Windows.Forms.Timer();
            timerRunAndDown.Interval = 1000; // Mỗi giây
            timerRunAndDown.Tick += Timer_Tick;
            timerRunAndDown.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                GridView gridView = grvMain;
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    int status = SQLUtilities.ToInt(gridView.GetRowCellValue(i, "status"));

                    if (status == 1) // Cộng vào TotalRunningTime
                    {
                        string time = gridView.GetRowCellValue(i, "TotalRunningTime").ToString();
                        gridView.SetRowCellValue(i, "TotalRunningTime", AddOneSecond(time));
                    }
                    else if (status == 3) // Cộng vào TotalDowntime
                    {
                        string time = gridView.GetRowCellValue(i, "TotalDowntime").ToString();
                        gridView.SetRowCellValue(i, "TotalDowntime", AddOneSecond(time));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Write(ex);
            }
        }

        private string AddOneSecond(string time)
        {
            try
            {
                string[] parts = time.Split(':');
                int hours = int.Parse(parts[0]);
                int minutes = int.Parse(parts[1]);
                int seconds = int.Parse(parts[2]);

                seconds++;
                if (seconds >= 60)
                {
                    seconds = 0;
                    minutes++;
                }
                if (minutes >= 60)
                {
                    minutes = 0;
                    hours++;
                }

                return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            }
            catch (Exception ex)
            {
                ErrorLogger.Write(ex);
                return "00:00:00";
            }
        }

        #endregion

        #region Minimize to tray
        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon.Visible = true;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void mnitemShow_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void mnitemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowForm()
        {
            notifyIcon.Visible = false;
            this.Show();
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        #endregion Minimize to tray

        #region Run on start up
        private void btnRunAtStartup_Click(object sender, EventArgs e)
        {

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            if (key != null)
            {
                object value = key.GetValue("LineStatusClient");
                if (value != null) RemoveFromStartup();
                else AddToStartup();
            }
            else AddToStartup();
        }

        public void InitializeCheckStartUp()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
                if (key != null)
                {
                    object value = key.GetValue("LineStatusClient");
                    if (value != null)
                        btnRunAtStartup.Appearance.BackColor = System.Drawing.Color.LimeGreen;
                    else
                        btnRunAtStartup.Appearance.BackColor = System.Drawing.Color.Silver;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking startup registry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddToStartup()
        {
            try
            {
                string appName = "LineStatusClient";
                string appPath = Application.ExecutablePath;

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                key.SetValue(appName, "\"" + appPath + "\"");
                btnRunAtStartup.Appearance.BackColor = System.Drawing.Color.LimeGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding to startup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RemoveFromStartup()
        {
            try
            {
                string appName = "LineStatusClient";

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                key.DeleteValue(appName, false);
                btnRunAtStartup.Appearance.BackColor = System.Drawing.Color.Silver;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing from startup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Run on start up

        #region OTHERS

        //tự sinh STT
        private void grvData_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                e.Value = grvMain.GetRowHandle(e.ListSourceRowIndex) + 1;
            }
        }

        //Đổ màu row theo status
        private void grvMain_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0) // Kiểm tra có phải là hàng hợp lệ không
            {
                if(_displayDataType == DisplayDataType.Downtime)
                {
                    int status = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "status"));
                    switch (status)
                    {
                        case 0: // Không chạy
                            e.Appearance.BackColor = System.Drawing.Color.White;
                            e.Appearance.ForeColor = System.Drawing.Color.Black;
                            break;
                        case 1: // Chạy
                            e.Appearance.BackColor = System.Drawing.Color.ForestGreen;
                            e.Appearance.ForeColor = System.Drawing.Color.White;
                            break;
                        case 2: // Nghỉ trưa
                            e.Appearance.BackColor = System.Drawing.Color.Yellow;
                            e.Appearance.ForeColor = System.Drawing.Color.Black;
                            break;
                        case 3:  // Dừng
                            e.Appearance.BackColor = System.Drawing.Color.OrangeRed;
                            e.Appearance.ForeColor = System.Drawing.Color.White;
                            break;
                        default:
                            e.Appearance.BackColor = System.Drawing.Color.White;
                            e.Appearance.ForeColor = System.Drawing.Color.Black;
                            break;
                    }

                    return;
                }

                if (_displayDataType == DisplayDataType.CallTime)
                {
                    int callCount = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "CallCount"));
                    if(callCount > 5)
                    {
                        e.Appearance.BackColor = System.Drawing.Color.Orange;
                        e.Appearance.ForeColor = System.Drawing.Color.Black;
                    }

                    return;
                }

            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            flyoutPanel1.ShowBeakForm();
        }
        frmDowntimeHistory frmHistoryShow = null;
        private void btnHistory_Click(object sender, EventArgs e)
        {
            flyoutPanel1.HideBeakForm();


            var form = new LineStatusClient.Forms.History.frmHistoryMaster();
            form.Show();


            return;
            if (frmHistoryShow == null || frmHistoryShow.IsDisposed)
            {
                frmHistoryShow = new frmDowntimeHistory();
                frmHistoryShow.Show();
            }
            else
            {
                if (frmHistoryShow.WindowState == FormWindowState.Minimized)
                    frmHistoryShow.WindowState = FormWindowState.Normal;
                frmHistoryShow.TopMost = true;  // Đảm bảo form hiển thị trên cùng
                frmHistoryShow.TopMost = false; // Trả về trạng thái bình thường
                frmHistoryShow.BringToFront();
                frmHistoryShow.Activate();
            }
        }
        frmEnterPassword frmPasswordShow = null;
        #region SHOW EMAIL FORM
        frmEmail frmEmailShow = null;
        private void btnEmail_Click(object sender, EventArgs e)
        {
            flyoutPanel1.HideBeakForm();
            if (frmEmailShow == null || frmEmailShow.IsDisposed)
            {
                if (frmPasswordShow == null || frmPasswordShow.IsDisposed)
                {
                    frmPasswordShow = new frmEnterPassword();
                    frmPasswordShow.FormClosed += (s, args) =>
                    {
                        if (frmPasswordShow.DialogResult == DialogResult.OK)
                        {
                            frmEmailShow = new frmEmail();
                            frmEmailShow.Show();
                        }
                    };
                    frmPasswordShow.Show();
                }
                else
                {
                    if (frmPasswordShow.WindowState == FormWindowState.Minimized)
                        frmPasswordShow.WindowState = FormWindowState.Normal;
                    frmPasswordShow.TopMost = true;  // Đảm bảo form hiển thị trên cùng
                    frmPasswordShow.TopMost = false; // Trả về trạng thái bình thường
                    frmPasswordShow.BringToFront();
                    frmPasswordShow.Activate();
                }
            }
            else
            {
                if (frmEmailShow.WindowState == FormWindowState.Minimized)
                    frmEmailShow.WindowState = FormWindowState.Normal;
                frmEmailShow.TopMost = true;  // Đảm bảo form hiển thị trên cùng
                frmEmailShow.TopMost = false; // Trả về trạng thái bình thường
                frmEmailShow.BringToFront();
                frmEmailShow.Activate();
            }
        }
        #endregion

        #region SHOW SHIFT FORM
        frmShift frmShiftShow = null;
        private void btnShift_Click(object sender, EventArgs e)
        {
            flyoutPanel1.HideBeakForm();
            if (frmShiftShow == null || frmShiftShow.IsDisposed)
            {
                if (frmPasswordShow == null || frmPasswordShow.IsDisposed)
                {
                    frmPasswordShow = new frmEnterPassword();
                    frmPasswordShow.FormClosed += (s, args) =>
                    {
                        if (frmPasswordShow.DialogResult == DialogResult.OK)
                        {
                            frmShiftShow = new frmShift();
                            frmShiftShow.Show();
                        }
                    };
                    frmPasswordShow.Show();
                }
                else
                {
                    if (frmPasswordShow.WindowState == FormWindowState.Minimized)
                        frmPasswordShow.WindowState = FormWindowState.Normal;
                    frmPasswordShow.TopMost = true;  // Đảm bảo form hiển thị trên cùng
                    frmPasswordShow.TopMost = false; // Trả về trạng thái bình thường
                    frmPasswordShow.BringToFront();
                    frmPasswordShow.Activate();
                }
            }
            else
            {
                if (frmShiftShow.WindowState == FormWindowState.Minimized)
                    frmShiftShow.WindowState = FormWindowState.Normal;
                frmShiftShow.TopMost = true;  // Đảm bảo form hiển thị trên cùng
                frmShiftShow.TopMost = false; // Trả về trạng thái bình thường
                frmShiftShow.BringToFront();
                frmShiftShow.Activate();
            }
        }
        #endregion

        #endregion
    }

    public enum DisplayDataType
    {
        [Description("Thời gian dừng")]
        Downtime,

        [Description("Thời gian gọi")]
        CallTime
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            return field?
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description
                ?? value.ToString();
        }
    }
}