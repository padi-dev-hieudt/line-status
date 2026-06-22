//using LineStatusClient.Common;
//using LineStatusClient.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace LineStatusClient.Models
//{
//    public interface IDisplayMode
//    {
//        void Start();
//        void Stop();
//    }

//    public class DowntimeMode : IDisplayMode
//    {
//        private SqlDependencyEx dependency;
//        private DevExpress.XtraGrid.GridControl container;
//        private readonly object reconnectLock = new object();
//        private bool isMonitoringReconnect = false;
//        private System.Windows.Forms.Timer reconnectTimer;

//        public DowntimeMode(DevExpress.XtraGrid.GridControl container)
//        {
//            this.container = container;
//        }

//        public void Start()
//        {
//            // Start timer
//            // Start dependency
//            // Load data
//        }

//        public void Stop()
//        {
//            // Stop timer
//            // Stop dependency
//        }

//        private void LoadData()
//        {
//            try
//            {
//                container.BeginInvoke(new Action(() =>
//                {
//                    var data = SQLHelper<Line_downtime_history_DTO>.ProcedureToList("spGetLineStatus",
//                        new string[] { },
//                        new object[] { });

//                    container.DataSource = data;
//                }));
//            }
//            catch (Exception ex)
//            {
//                ErrorLogger.Write(ex);
//            }
//        }

//        /// <summary>
//        /// Khởi tạo SqlDependencyEx để theo dõi thay đổi dữ liệu
//        /// </summary>
//        private void InitializeSqlDependency()
//        {
//            try
//            {
//                StopSqlDependency(); // Dừng dependency cũ nếu có

//                var builder = new SqlConnectionStringBuilder(Settings.connectionString);
//                string dbName = builder.InitialCatalog;
//                int identity = SQLUtilities.GetUniqueIdentity();

//                dependency = new SqlDependencyEx(Settings.connectionString, dbName, "Line_downtime_history", identity: identity);
//                dependency.TableChanged += (s, e) => LoadData();

//                dependency.NotificationProcessStopped += (s, e) =>
//                {
//                    Invoke((MethodInvoker)StartReconnectMonitor);
//                };

//                dependency.Start();
//                Invoke((MethodInvoker)StopReconnectMonitor);
//            }
//            catch (Exception ex)
//            {
//                ErrorLogger.SaveLog("Error in InitializeSqlDependency: ", ex.ToString());
//                Invoke((MethodInvoker)StartReconnectMonitor);
//            }
//        }

//        /// <summary>
//        /// Dừng SqlDependencyEx nếu đang chạy
//        /// </summary>
//        private void StopSqlDependency()
//        {
//            if (dependency == null) return;

//            try { dependency.Stop(); dependency.Dispose(); }
//            catch (Exception ex)
//            {
//            }
//            dependency = null;
//        }

//        /// <summary>
//        /// Bắt đầu kiểm tra kết nối SQL định kỳ
//        /// </summary>
//        private void StartReconnectMonitor()
//        {
//            lock (reconnectLock)
//            {
//                if (isMonitoringReconnect) return;

//                StopReconnectMonitor();

//                reconnectTimer = new System.Windows.Forms.Timer
//                {
//                    Interval = 10000 // 10 seconds
//                };
//                reconnectTimer.Tick += ReconnectTimer_Tick;
//                reconnectTimer.Start();

//                isMonitoringReconnect = true;
//            }
//        }

//        /// <summary>
//        /// Dừng kiểm tra kết nối SQL
//        /// </summary>
//        private void StopReconnectMonitor()
//        {
//            try
//            {
//                reconnectTimer?.Stop();
//                reconnectTimer?.Dispose();
//            }
//            catch (Exception)
//            {
//            }
//            reconnectTimer = null;
//            isMonitoringReconnect = false;
//        }

//        /// <summary>
//        /// Sự kiện kiểm tra kết nối SQL định kỳ
//        /// </summary>
//        private void ReconnectTimer_Tick(object sender, EventArgs e)
//        {
//            if (SQLUtilities.CheckSQLConnection())
//            {
//                InitializeSqlDependency();
//                StopReconnectMonitor();
//            }
//        }
//    }
//}
