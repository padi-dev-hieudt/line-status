using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using LineStatusClient.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineStatusClient.Forms.Main
{
    /// <summary>
    /// Màn hình chính - Thời gian gọi. Tự quản lý vòng đời grid:
    /// cột, nạp dữ liệu, tô màu khi số lần gọi nhiều và SqlDependencyEx.
    /// </summary>
    public partial class uc_main_calltime : UserControl, IMainContentControl
    {
        private System.Windows.Forms.Timer reconnectTimer;
        private bool isMonitoringReconnect = false;
        private SqlDependencyEx dependency;
        private readonly object reconnectLock = new object();

        public uc_main_calltime()
        {
            InitializeComponent();

            gridView1.DataSourceChanged += gridView1_DataSourceChanged;
        }

        private void gridView1_DataSourceChanged(object sender, EventArgs e)
        {
            gridView1.BestFitColumns();
        }

        public async Task ActivateAsync()
        {
            await LoadDataAsync();
            await Task.Run(() => InitializeSqlDependency());
        }

        public async Task DeactivateAsync()
        {
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
                var data = await SQLHelper<LineStatusResult>.ProcedureToListAsync("sp_CallSubleaderHistory_Search",
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

                dependency = new SqlDependencyEx(Settings.connectionString, dbName, "CallSubleaderHistory", identity: identity);
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
        /// Giải phóng SqlDependencyEx khi UC bị đóng/đổi (gọi từ Dispose).
        /// </summary>
        private void StopLiveUpdates()
        {
            StopReconnectMonitor();
            StopSqlDependency();
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

        // Tô màu khi số lần gọi nhiều hơn 5 lần
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle < 0) return;

            int callCount = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "CallCount"));
            if (callCount > 5)
            {
                e.Appearance.BackColor = System.Drawing.Color.Orange;
                e.Appearance.ForeColor = System.Drawing.Color.Black;
            }
        }

        #endregion
    }

    /// <summary>
    /// Kết quả tổng hợp số lần gọi tổ trưởng theo từng chuyền (dùng cho sp_CallSubleaderHistory_Search).
    /// </summary>
    public class LineStatusResult
    {
        public long STT { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public string Position { get; set; }
        public int CallCount { get; set; }
        public DateTime CallTime { get; set; }
        public string ShiftName { get; set; }
    }
}
