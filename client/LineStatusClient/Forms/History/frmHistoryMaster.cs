using LineStatusClient.Common;
using LineStatusClient.Froms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineStatusClient.Forms.History
{
    public enum DisplayDataType
    {
        [Description("Thời gian dừng")]
        Downtime,

        [Description("Thời gian gọi")]
        CallTime
    }

    public partial class frmHistoryMaster : Form
    {
        private DisplayDataType _displayDataType = DisplayDataType.Downtime;

        public frmHistoryMaster()
        {
            InitializeComponent();

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
            cboDisplayDataType.SelectedIndex = 0;
        }

        private void cboDisplayDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDisplayDataType.SelectedValue is DisplayDataType selected)
            {
                _displayDataType = selected;

                switch (selected)
                {
                    case DisplayDataType.Downtime:
                        OpenChildForm(new frmDowntimeHistory());
                        break;

                    case DisplayDataType.CallTime:
                        OpenUserControl(new uc_calltimeHistory());
                        break;
                }

                LoadTitle();
            }
        }

        private void frmHistory_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            // Open default mode view
            OpenChildForm(new frmDowntimeHistory());
            LoadTitle();
        }

        private void LoadTitle()
        {
            var title = "HISTORY";
            switch (_displayDataType)
            {
                case DisplayDataType.Downtime:
                    title = "Lịch sử dừng";
                    break;
                case DisplayDataType.CallTime:
                    title = "Lịch sử gọi";
                    break;
            }

            lbTitle.BeginInvoke((Action)(() =>
            {
                Text = title;
                lbTitle.Text = title;
            }));
        }

        private void OpenChildForm(Form childForm)
        {
            foreach (Control ctl in pnlContent.Controls)
            {
                ctl.Dispose();
            }

            pnlContent.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlContent.Controls.Add(childForm);
            childForm.Show();
        }

        private void OpenUserControl(UserControl uc)
        {
            foreach (Control ctl in pnlContent.Controls)
            {
                ctl.Dispose();
            }

            pnlContent.Controls.Clear();

            uc.Dock = DockStyle.Fill;

            pnlContent.Controls.Add(uc);
            
        }
    }
}
