using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using LineStatusClient.Common;
using LineStatusClient.Forms;
using LineStatusClient.Forms.Email;
using LineStatusClient.Forms.Main;
using LineStatusClient.Froms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineStatusClient
{
    public partial class FormMain : XtraForm
    {
        private bool isRefresh = false;
        private bool _switching = false;
        private DisplayDataType _displayDataType = DisplayDataType.Downtime;

        // UC đang hiển thị trong pnlContent (để nút Làm mới yêu cầu nạp lại).
        private IMainContentControl _activeContent;

        private readonly List<Control> _downtimeControl;
        private readonly List<Control> _calltimeControl;

        public FormMain()
        {
            InitializeComponent();
            InitializeCheckStartUp();

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
            Settings.ReadSQLConnectionString();
            Settings.LoadConfig();

            // Mở mặc định màn hình Thời gian dừng.
            await SwitchContentAsync(DisplayDataType.Downtime);
        }

        private async void cboDisplayDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDisplayDataType.SelectedValue is DisplayDataType selected)
                await SwitchContentAsync(selected);
        }

        /// <summary>
        /// Chuyển chế độ hiển thị: hiện overlay loading trên pnlContent, dừng UC cũ
        /// (phần chặn UI được đẩy ra luồng nền), thay UC mới rồi nạp dữ liệu + theo dõi realtime.
        /// </summary>
        private async Task SwitchContentAsync(DisplayDataType selected)
        {
            if (_switching) return;
            _switching = true;
            _displayDataType = selected;
            cboDisplayDataType.Enabled = false;

            IOverlaySplashScreenHandle overlay = SplashScreenManager.ShowOverlayForm(pnlContent);
            try
            {
                // 1. Dừng & giải phóng UC cũ (dừng SqlDependencyEx chạy ở luồng nền).
                if (_activeContent != null)
                    await _activeContent.DeactivateAsync();
                ClearContent();

                // 2. Đổi chú thích footer + thêm UC mới.
                UpdateFooterLegend(selected);

                UserControl uc = (selected == DisplayDataType.CallTime)
                    ? (UserControl)new uc_main_calltime()
                    : new uc_main_downtime();
                uc.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(uc);
                _activeContent = (IMainContentControl)uc;

                // 3. Nạp dữ liệu + bắt đầu theo dõi realtime.
                await _activeContent.ActivateAsync();
            }
            finally
            {
                SplashScreenManager.CloseOverlayForm(overlay);
                cboDisplayDataType.Enabled = true;
                _switching = false;
            }
        }

        // Ẩn/hiện chú thích màu ở footer theo loại dữ liệu đang xem.
        private void UpdateFooterLegend(DisplayDataType selected)
        {
            foreach (var item in _downtimeControl)
            {
                item.Visible = selected == DisplayDataType.Downtime;
            }
            foreach (var item in _calltimeControl)
            {
                item.Visible = selected == DisplayDataType.CallTime;
            }
        }

        // Giải phóng UC đang nằm trong pnlContent.
        private void ClearContent()
        {
            foreach (Control ctl in pnlContent.Controls)
            {
                ctl.Dispose();
            }
            pnlContent.Controls.Clear();
            _activeContent = null;
        }

        private async void btnRefesh_Click(object sender, EventArgs e)
        {
            if (isRefresh || _switching || _activeContent == null) return;
            isRefresh = true;

            IOverlaySplashScreenHandle overlay = SplashScreenManager.ShowOverlayForm(pnlContent);
            try
            {
                await Task.Delay(500);
                await _activeContent.ReloadAsync();
            }
            finally
            {
                SplashScreenManager.CloseOverlayForm(overlay);
                isRefresh = false;
            }
        }

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
}