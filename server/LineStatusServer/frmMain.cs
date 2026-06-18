using LineStatusServer.Common;
using LineStatusServer.DTOs;
using LineStatusServer.Models;
using Microsoft.Win32;
using NB_TestTruyenThong.Uc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineStatusServer
{
    public partial class frmMain : Form
    {
        public BindingList<LineData> listLineData = new BindingList<LineData>();
        private (string, int, int) lastLineKey;
        private TCPServer tcpServer = null;
        private readonly object listLock = new object();
        private bool isRun;
        public bool IsRun
        {
            get { return isRun; }
            set { isRun = value; }
        }
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tcpServer != null)
                tcpServer.OnReceiveDataEvents_Server -= GetDataFromServer;
            DisconnectServer();
        }

        private void SafeInvoke(Action action)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try { this.BeginInvoke(action); }
                catch (ObjectDisposedException) { }
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            InitializeCheckStartUp();
            var (serverName, dbName, userName, password, extra) = Settings.ReadSQLConnectionString();
            var (ip, port) = Settings.ReadTCPAddress();
            txtServerName.Text = serverName;
            txtDBName.Text = dbName;
            txtUserName.Text = userName;
            txtPassword.Text = password;
            txtOptional.Text = extra;
            txtIPAddress.Text = ip;
            txtPort.Text = port;
            grvMain.DataSource = listLineData;
            chkConnectWhenStart.Checked = Properties.Settings.Default.ConnectWhenStart;
            if (chkConnectWhenStart.Checked) btnConnect_Click(null, null);
        }

        #region Minimize to tray

        private void btnMinimize_Click(object sender, EventArgs e)
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

        private void chkRunOnStartUp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRunOnStartUp.Checked) AddToStartup();
            else RemoveFromStartup();
        }

        public void InitializeCheckStartUp()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
                if (key != null)
                {
                    object value = key.GetValue("LineStatusServer");
                    chkRunOnStartUp.Checked = value != null;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error checking startup registry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.Write("Error checking startup registry:\n" + ex.ToString());
            }
        }

        public void AddToStartup()
        {
            try
            {
                string appName = "LineStatusServer";
                string appPath = Application.ExecutablePath;

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                key.SetValue(appName, "\"" + appPath + "\"");
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error adding to startup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.Write("Error adding to startup:\n" + ex.ToString());
            }
        }

        public void RemoveFromStartup()
        {
            try
            {
                string appName = "LineStatusServer";

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                key.DeleteValue(appName, false);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error removing from startup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.Write("Error removing from startup:\n" + ex.ToString());
            }
        }

        #endregion Run on start up

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.WriteSQLConnectionString(
                txtServerName.Text,
                txtDBName.Text,
                txtUserName.Text,
                txtPassword.Text,
                txtOptional.Text
            );
            Settings.WriteTCPAddress(
                txtIPAddress.Text,
                txtPort.Text
            );
            btnSave.BeginInvoke(new Action(async () =>
            {
                btnSave.Text = "Đã lưu";
                btnSave.Padding = new Padding(45, 0, 35, 0);
                btnSave.Enabled = false;
                btnSave.FlatStyle = FlatStyle.Flat;
                await Task.Delay(2000);
                btnSave.Text = "Lưu";
                btnSave.Padding = new Padding(50, 0, 50, 0);
                btnSave.Enabled = true;
                btnSave.FlatStyle = FlatStyle.Standard;
            }));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsRun)
                {
                    DisconnectServer();
                    tcpServer = new TCPServer(Settings.TCPAddress.Port);
                    tcpServer.Start();
                    if (!tcpServer.IsConnected)
                    {
                        UpdateUIConnected(false);
                        IsRun = false;
                        return;
                    }
                    else
                    {
                        UpdateUIConnected(true);
                        IsRun = true;
                    }
                    tcpServer.OnReceiveDataEvents_Server -= GetDataFromServer;
                    tcpServer.OnReceiveDataEvents_Server += GetDataFromServer;
                }
                else
                {
                    UpdateUIConnected(false);
                    tcpServer.OnReceiveDataEvents_Server -= GetDataFromServer;
                    DisconnectServer();
                    IsRun = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Write(ex.ToString());
                IsRun = false;
            }
        }
        private void DisconnectServer()
        {
            if (tcpServer != null)
            {
                tcpServer.Stop();
                tcpServer = null;
            }
        }

        private async void GetDataFromServer(string receive_Data)
        {
            try
            {
                using (var sr = new StringReader(receive_Data))
                using (var reader = new JsonTextReader(sr))
                {
                    reader.SupportMultipleContent = true;
                    var serializer = new JsonSerializer();

                    while (await reader.ReadAsync())
                    {
                        if (reader.TokenType != JsonToken.StartObject) continue;

                        try
                        {
                            JObject jObject = await JObject.LoadAsync(reader);
                            var lineData = jObject.ToObject<LineData>(serializer) ?? new LineData();

                            if (lineData == null || string.IsNullOrWhiteSpace(lineData.LineCode))
                            {
                                ErrorLogger.Write($"Invalid lineData: {jObject}");
                                continue;
                            }

                            var currentKey = (lineData.LineCode, lineData.Status, lineData.ProductCount);
                            if (currentKey.Equals(lastLineKey)) continue;
                            lastLineKey = currentKey;

                            lineData.Timestamp = SQLUtilities.GetDate();
                            lineData.shift = getShiftBasedOnLineShift(lineData.LineCode, lineData.Timestamp);

                            // Cập nhật UI
                            SafeInvoke(() =>
                            {
                                lock (listLock)
                                {
                                    listLineData.Insert(0, lineData);
                                    if (listLineData.Count > 100)
                                        listLineData.RemoveAt(listLineData.Count - 1);
                                }
                            });

                            // Ghi dữ liệu vào DB
                            _ = Task.Run(() =>
                            {
                                try
                                {
                                    SQLUtilities.ExcuteProcedure(
                                        "sp_UpdateLineStatus",
                                        new[] { "@LineCode", "@Status", "@ProductCount", "@Timestamp", "@Shift" },
                                        new object[] { lineData.LineCode, lineData.Status, lineData.ProductCount, lineData.Timestamp, lineData.shift }
                                    );
                                }
                                catch (Exception ex)
                                {
                                    ErrorLogger.Write($"DB error: {ex}\nData: {JsonConvert.SerializeObject(lineData)}");
                                }
                            });
                        }
                        catch (JsonReaderException ex)
                        {
                            ErrorLogger.Write($"JSON parse error: {ex}\nRaw: {receive_Data}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger.Write($"Unhandled error: {ex}");
            }
        }

        private void UpdateUIConnected(bool connected)
        {
            if (connected)
            {
                lblCurrentStatus.Text = "Đã kết nối";
                lblCurrentStatus.ForeColor = Color.Green;
                btnConnect.Text = "Ngắt kết nối";
                btnConnect.Image = Properties.Resources.disconnect;
                btnConnect.Padding = new Padding(25, 0, 25, 0);
                btnConnect.ForeColor = Color.Red;
                btnConnect.Tag = "connected";
            }
            else
            {
                lblCurrentStatus.Text = "Đã ngắt kết nối";
                lblCurrentStatus.ForeColor = Color.Red;
                btnConnect.Text = "Kết nối";
                btnConnect.Image = Properties.Resources.connect;
                btnConnect.Padding = new Padding(40, 0, 40, 0);
                btnConnect.ForeColor = Color.Green;
                btnConnect.Tag = "disconnected";
            }
        }

        //private void UpdateStatusDownTime(string lineCode, int status)
        //{
        //    if (string.IsNullOrWhiteSpace(lineCode))
        //    {
        //        ErrorLogger.Write($"UpdateStatusDownTime failed: LineCode is null or empty. Status={status}");
        //        return;
        //    }
        //    try
        //    {
        //        SQLUtilities.ExcuteProcedure(
        //            "sp_UpdateLineStatus",
        //            new[] { "@LineCode", "@Status" },
        //            new object[] { lineCode, status }
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.Write($"UpdateStatusDownTime failed for LineCode={lineCode}, Status={status}. Exception: \n{ex}");
        //    }
        //}

        private int getShiftBasedOnLineShift(string lineCode, DateTime time)
        {
            if (string.IsNullOrWhiteSpace(lineCode))
            {
                ErrorLogger.Write("getShiftBasedOnLineShift \n Line code is empty");
                return 0;
            }
            try
            {
                var query = "select ws.ShiftCode, ws.StartTime, ws.EndTime " +
                    "from [LineShift] ls " +
                    "inner join WorkShift ws on ws.ID = ls.WorkShiftID " +
                    $"where ls.LineCode = '{lineCode}'";

                var list_LineShift = SQLHelper<LineShiftDTO>.SqlToList(query);
                if (list_LineShift.Count == 0)
                {
                    ErrorLogger.Write("getShiftBasedOnLineShift \n Bạn chưa khai báo ca làm việc theo line");
                    return 0;
                }

                TimeSpan currentTime = time.TimeOfDay;

                foreach (var shiftItem in list_LineShift)
                {
                    TimeSpan startTime = SQLUtilities.ToTimeSpan(shiftItem.StartTime);
                    TimeSpan endTime = SQLUtilities.ToTimeSpan(shiftItem.EndTime);

                    if (startTime < endTime)
                    {
                        // Ca làm việc trong cùng một ngày (08:00 - 22:00)
                        if (currentTime >= startTime && currentTime < endTime)
                            return shiftItem.ShiftCode;
                    }
                    else
                    {
                        // Ca làm việc qua đêm (22:00 - 08:00)
                        if (currentTime >= startTime || currentTime < endTime)
                            return shiftItem.ShiftCode;
                    }
                }
                ErrorLogger.Write($"getShiftBasedOnLineShift \n Không tìm thấy ca làm việc của dây chuyền {lineCode} at {currentTime}");
            }
            catch (Exception ex)
            {
                ErrorLogger.Write(ex.ToString());
            }
            return 0;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            int frmHeight = this.Height;
            grvMain.Height = frmHeight > 1000 ? (int)(frmHeight * 0.75) :
                            frmHeight > 850 ? (int)(frmHeight * 0.70) :
                            frmHeight > 700 ? (int)(frmHeight * 0.65) :
                            frmHeight > 600 ? (int)(frmHeight * 0.60) :
                            (int)(frmHeight * 0.50);
        }

        private void mnitemDelete_Click(object sender, EventArgs e)
        {
            listLineData.Clear();
        }

        private void chkConnectWhenStart_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConnectWhenStart = chkConnectWhenStart.Checked;
            Properties.Settings.Default.Save();
        }


    }
}