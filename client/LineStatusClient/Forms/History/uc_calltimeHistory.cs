using ClosedXML.Excel;
using DevExpress.XtraEditors;
using LineStatusClient.Common;
using LineStatusClient.DTOs;
using LineStatusClient.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineStatusClient.Forms.History
{
    public partial class uc_calltimeHistory : UserControl
    {
        private List<CallSubleaderHistoryDTO> _currentData = new List<CallSubleaderHistoryDTO>();
        private SimpleButton _activeChip = null;
        private bool _suppressChipReset = false;

        public uc_calltimeHistory()
        {
            InitializeComponent();

            gridView1.DataSourceChanged += gridControl1_DataSourceChanged;
        }

        private void gridControl1_DataSourceChanged(object sender, EventArgs e)
        {
            gridView1.BestFitColumns();
        }

        private async void uc_calltimeHistory_Load(object sender, EventArgs e)
        {
            LoadLine();
            btnToday_Click(null, null);
            if (this.FindForm() is Form parentForm)
                parentForm.AcceptButton = btnSearch;
            await LoadData();
        }

        #region LOAD DATA

        private void LoadLine()
        {
            try
            {
                var listLines = SQLHelper<Line_mst>.FindAll().OrderBy(x => x.Sort);
                cb_Line.Properties.DataSource = listLines.ToList();
                cb_Line.Properties.DisplayMember = "Line_nm";
                cb_Line.Properties.ValueMember = "Line_c";
            }
            catch (Exception ex)
            {
                ErrorLogger.SaveLog("LoadLine", ex.Message);
            }
        }

        private async Task LoadData()
        {
            if (dtpFrom.EditValue == null || dtpTo.EditValue == null)
            {
                MessageBox.Show("Không để trống Ngày bắt đầu - Ngày kết thúc", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            gridView1.ShowLoadingPanel();
            try
            {
                await Task.Delay(500);

                DateTime from = Convert.ToDateTime(dtpFrom.EditValue).Date;
                DateTime to = Convert.ToDateTime(dtpTo.EditValue).Date.AddDays(1).AddSeconds(-1);
                string lineCode = SQLUtilities.ToString(cb_Line.EditValue);
                string position = txtPosition.Text.Trim();

                var data = await SQLHelper<CallSubleaderHistoryDTO>.ProcedureToListAsync(
                    "sp_CallSubleaderHistory_SearchByFilter",
                    new[] { "@DateFrom", "@DateTo", "@LineCode", "@Position" },
                    new object[] { from, to, lineCode, position });

                _currentData = data ?? new List<CallSubleaderHistoryDTO>();
                gridControl1.DataSource = null;
                gridControl1.DataSource = _currentData;
            }
            catch (Exception ex)
            {
                ErrorLogger.SaveLog("LoadData", ex.Message);
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                gridView1.HideLoadingPanel();
            }
        }

        #endregion

        #region CHIP BUTTONS

        private void SetChipActive(SimpleButton btn)
        {
            var chips = new[] { btnToday, btnYesterday, btnThisWeek, btnThisMonth };
            foreach (var chip in chips)
            {
                chip.Appearance.Options.UseBackColor = false;
                chip.Appearance.Options.UseForeColor = false;
            }

            _activeChip = btn;
            if (btn == null) return;

            btn.Appearance.BackColor = Color.SteelBlue;
            btn.Appearance.ForeColor = Color.White;
            btn.Appearance.Options.UseBackColor = true;
            btn.Appearance.Options.UseForeColor = true;
        }

        private void SetDateRange(DateTime from, DateTime to)
        {
            _suppressChipReset = true;
            dtpFrom.EditValue = from;
            dtpTo.EditValue = to;
            _suppressChipReset = false;
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            SetDateRange(DateTime.Today, DateTime.Today);
            SetChipActive(btnToday);
        }

        private void btnYesterday_Click(object sender, EventArgs e)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            SetDateRange(yesterday, yesterday);
            SetChipActive(btnYesterday);
        }

        private void btnThisWeek_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = today.AddDays(-diff);
            SetDateRange(monday, today);
            SetChipActive(btnThisWeek);
        }

        private void btnThisMonth_Click(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            var firstDay = new DateTime(today.Year, today.Month, 1);
            SetDateRange(firstDay, today);
            SetChipActive(btnThisMonth);
        }

        private void dtpFrom_EditValueChanged(object sender, EventArgs e)
        {
            if (!_suppressChipReset) SetChipActive(null);
        }

        private void dtpTo_EditValueChanged(object sender, EventArgs e)
        {
            if (!_suppressChipReset) SetChipActive(null);
        }

        #endregion

        #region GRID

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.IsGetData && e.Column.FieldName == "No")
            {
                e.Value = gridView1.GetRowHandle(e.ListSourceRowIndex) + 1;
            }
        }

        #endregion

        #region SEARCH & EXPORT

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadData();
        }

        private string OpenSaveFileDialog(string fileName)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "Excel Files|*.xlsx";
                dlg.Title = "Lưu file Excel";
                dlg.FileName = fileName;
                return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : string.Empty;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_currentData == null || _currentData.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string savePath = OpenSaveFileDialog($"LichSuGoi_{DateTime.Now:ddMMyyyy_HHmm}");
            if (string.IsNullOrEmpty(savePath)) return;

            var data = _currentData.ToList();

            btnExport.Enabled = false;
            var waitForm = new Form
            {
                Text = "Vui lòng chờ",
                Size = new Size(300, 110),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false,
                TopMost = true
            };
            waitForm.Controls.Add(new Label { Text = "Đang xuất dữ liệu...", Dock = DockStyle.Top, Height = 45, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 11F) });
            waitForm.Controls.Add(new System.Windows.Forms.ProgressBar { Style = ProgressBarStyle.Marquee, Dock = DockStyle.Bottom, Height = 28, MarqueeAnimationSpeed = 30 });
            waitForm.Show(this);

            Task.Run(() =>
            {
                try
                {
                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Lịch sử gọi");

                        ws.Cell(1, 1).Value = "STT";
                        ws.Cell(1, 2).Value = "Thời gian";
                        ws.Cell(1, 3).Value = "Mã chuyền";
                        ws.Cell(1, 4).Value = "Tên chuyền";
                        ws.Cell(1, 5).Value = "Vị trí";
                        ws.Cell(1, 6).Value = "Ca làm";
                        ws.Cell(1, 7).Value = "Tổng số lần gọi";

                        var header = ws.Range(1, 1, 1, 7);
                        header.Style.Font.Bold = true;
                        header.Style.Fill.BackgroundColor = XLColor.SteelBlue;
                        header.Style.Font.FontColor = XLColor.White;
                        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < data.Count; i++)
                        {
                            int row = i + 2;
                            ws.Cell(row, 1).Value = i + 1;
                            ws.Cell(row, 2).Value = data[i].CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");
                            ws.Cell(row, 3).Value = data[i].LineCode;
                            ws.Cell(row, 4).Value = data[i].LineName;
                            ws.Cell(row, 5).Value = data[i].Position;
                            ws.Cell(row, 6).Value = data[i].ShiftName;
                            ws.Cell(row, 7).Value = data[i].TotalCount;
                        }

                        ws.Columns().AdjustToContents();
                        wb.SaveAs(savePath);
                    }

                    this.BeginInvoke(new Action(() =>
                    {
                        waitForm.Close();
                        waitForm.Dispose();
                        btnExport.Enabled = true;

                        var ans = MessageBox.Show("Bạn có muốn mở file đã export không?",
                            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (ans == DialogResult.Yes)
                            System.Diagnostics.Process.Start(savePath);
                    }));
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        waitForm.Close();
                        waitForm.Dispose();
                        btnExport.Enabled = true;
                        MessageBox.Show(ex.Message, "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            txtPosition.Text = string.Empty;
            cb_Line.EditValue = null;
            btnToday_Click(null, null);
            await LoadData();
        }

        #endregion
    }
}
