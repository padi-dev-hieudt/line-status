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
    public partial class frmHistoryMaster : Form
    {
        public frmHistoryMaster()
        {
            InitializeComponent();
        }

        private void frmHistory_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            OpenUserControl(new uc_calltimeHistory());
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
