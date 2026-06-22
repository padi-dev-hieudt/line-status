using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using LineStatusClient.Common;
using LineStatusClient.DTOs;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineStatusClient.Forms.Main
{
    /// <summary>
    /// Màn hình chính - Thời gian dừng. Tự quản lý vòng đời grid:
    /// cột, nạp dữ liệu, tô màu theo trạng thái, timer chạy/dừng và SqlDependencyEx.
    /// </summary>
    public partial class uc_main_downtime : UserControl, IMainContentControl
    {
        private System.Windows.Forms.Timer timerRunAndDown;
        private System.Windows.Forms.Timer reconnectTimer;
        private bool isMonitoringReconnect = false;
        private SqlDependencyEx dependency;
        private readonly object reconnectLock = new object();

        public uc_main_downtime()
        {
            InitializeComponent();

            gridView1.DataSourceChanged += gridView1_DataSourceChanged;
            InitializeTimer();
        }

        private void gridView1_DataSourceChanged(object sender, EventArgs e)
        {
            gridView1.BestFitColumns();
        }

        public async Task ActivateAsync()
        {
            timerRunAndDown.Start();
            await LoadDataAsync();
            await Task.Run(() => InitializeSqlDependency());
        }

        public async Task DeactivateAsync()
        {
            timerRunAndDown.Stop();
            StopReconnectMonitor();
            // Dừng SqlDependencyEx là thao tác chặn (ExecuteNonQuery) -> đẩy ra luồng nền.
            await Task.Run(() => StopSqlDependency());
        }

        public async Task ReloadAsync()
        {
            await LoadDataAsync();
        }

        #region LOAD DATA

        private async Task LoadDataAsync()
        {
            try
            {
                var data = await SQLHelper<Line_downtime_history_DTO>.ProcedureToListAsync("spGetLineStatus",
                      new string[] { },
                      new object[] { });

                if (IsDisposed) return;

                gridControl1.BeginInvoke(new Action(() =>
                {
                    gridControl1.DataSource = data;
                }));
            }
            catch (Exception ex)
            {
                ErrorLogger.Write(ex);
            }
        }

        #endregion

        #region SQL DEPENDENCY

        private void InitializeSqlDependency()
        {
            try
            {
                StopSqlDependency(); // Dừng dependency cũ nếu có

                var builder = new SqlConnectionStringBuilder(Settings.connectionString);
                string dbName = builder.InitialCatalog;
                int identity = SQLUtilities.GetUniqueIdentity();

                dependency = new SqlDependencyEx(Settings.connectionString, dbName, "Line_downtime_history", identity: identity);
                dependency.TableChanged += async (s, e) => await LoadDataAsync();

                dependency.NotificationProcessStopped += (s, e) =>
                {
                    if (IsDisposed || Disposing) return;
                    Invoke((MethodInvoker)StartReconnectMonitor);
                };

                dependency.Start();

                if (!IsDisposed && !Disposing)
                    Invoke((MethodInvoker)StopReconnectMonitor);
            }
            catch (Exception ex)
            {
                ErrorLogger.SaveLog("Error in InitializeSqlDependency: ", ex.ToString());
                if (!IsDisposed && !Disposing)
                    Invoke((MethodInvoker)StartReconnectMonitor);
            }
        }

        private void StopSqlDependency()
        {
            if (dependency == null) return;

            try { dependency.Stop(); dependency.Dispose(); }
            catch (Exception) { }
            dependency = null;
        }

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

        private void StopReconnectMonitor()
        {
            try
            {
                reconnectTimer?.Stop();
                reconnectTimer?.Dispose();
            }
            catch (Exception) { }
            reconnectTimer = null;
            isMonitoringReconnect = false;
        }

        private void ReconnectTimer_Tick(object sender, EventArgs e)
        {
            if (SQLUtilities.CheckSQLConnection())
            {
                InitializeSqlDependency();
                StopReconnectMonitor();
            }
        }

        /// <summary>
        /// Giải phóng timer và SqlDependencyEx khi UC bị đóng/đổi (gọi từ Dispose).
        /// </summary>
        private void StopLiveUpdates()
        {
            try { timerRunAndDown?.Stop(); timerRunAndDown?.Dispose(); } catch (Exception) { }
            StopReconnectMonitor();
            StopSqlDependency();
        }

        #endregion

        #region COUNT RUNNING AND STOPPING TIME

        private void InitializeTimer()
        {
            timerRunAndDown = new System.Windows.Forms.Timer();
            timerRunAndDown.Interval = 1000; // Mỗi giây
            timerRunAndDown.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                GridView gridView = gridView1;
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

        #region GRID STYLE

        // Tự sinh STT
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                e.Value = gridView1.GetRowHandle(e.ListSourceRowIndex) + 1;
            }
        }

        // Đổ màu row theo status
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle < 0) return;

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
                case 3: // Dừng
                    e.Appearance.BackColor = System.Drawing.Color.OrangeRed;
                    e.Appearance.ForeColor = System.Drawing.Color.White;
                    break;
                default:
                    e.Appearance.BackColor = System.Drawing.Color.White;
                    e.Appearance.ForeColor = System.Drawing.Color.Black;
                    break;
            }
        }

        #endregion
    }
}
