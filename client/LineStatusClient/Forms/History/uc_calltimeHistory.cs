using ClosedXML.Excel;
using DevExpress.XtraEditors;
using LineStatusClient.Common;
using LineStatusClient.DTOs;
using LineStatusClient.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
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
            var waitForm = CreateWaitForm();
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

        private Form CreateWaitForm()
        {
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
            return waitForm;
        }

        private DateTime? ShowMonthPickerDialog()
        {
            using (var dlg = new XtraForm())
            {
                dlg.Text = "Chọn tháng";
                dlg.Size = new Size(300, 150);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;

                var lbl = new LabelControl { Text = "Tháng xuất dữ liệu:", Location = new Point(15, 18), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(120, 22) };
                lbl.Appearance.Font = new Font("Microsoft Sans Serif", 10F);
                lbl.Appearance.Options.UseFont = true;

                var dtpMonth = new DateEdit { Location = new Point(140, 16), Size = new Size(125, 22) };
                dtpMonth.Properties.Appearance.Font = new Font("Microsoft Sans Serif", 10F);
                dtpMonth.Properties.Appearance.Options.UseFont = true;
                dtpMonth.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
                dtpMonth.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
                dtpMonth.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.YearView;
                dtpMonth.Properties.DisplayFormat.FormatString = "MM/yyyy";
                dtpMonth.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                dtpMonth.Properties.EditFormat.FormatString = "MM/yyyy";
                dtpMonth.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                dtpMonth.Properties.MaskSettings.Set("mask", "MM/yyyy");
                dtpMonth.Properties.UseMaskAsDisplayFormat = true;
                dtpMonth.EditValue = DateTime.Today;

                var btnOk = new SimpleButton { Text = "Xuất Excel", Location = new Point(60, 60), Size = new Size(100, 26), DialogResult = DialogResult.OK };
                btnOk.Appearance.BackColor = Color.SteelBlue;
                btnOk.Appearance.ForeColor = Color.White;
                btnOk.Appearance.Options.UseBackColor = true;
                btnOk.Appearance.Options.UseForeColor = true;
                var btnCancel = new SimpleButton { Text = "Hủy", Location = new Point(170, 60), Size = new Size(80, 26), DialogResult = DialogResult.Cancel };

                dlg.Controls.AddRange(new Control[] { lbl, dtpMonth, btnOk, btnCancel });
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) != DialogResult.OK || dtpMonth.EditValue == null)
                    return null;
                return Convert.ToDateTime(dtpMonth.EditValue);
            }
        }

        private async void btnExportMonth_Click(object sender, EventArgs e)
        {
            DateTime? selected = ShowMonthPickerDialog();
            if (selected == null) return;

            int year = selected.Value.Year;
            int month = selected.Value.Month;

            List<CallSubleaderHistoryDTO> data;
            try
            {
                data = await SQLHelper<CallSubleaderHistoryDTO>.ProcedureToListAsync(
                    "sp_CallSubleaderHistory_ExportByMonth",
                    new[] { "@Year", "@Month" },
                    new object[] { year, month });
            }
            catch (Exception ex)
            {
                ErrorLogger.SaveLog("btnExportMonth_Click", ex.Message);
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (data == null || data.Count == 0)
            {
                MessageBox.Show($"Không có dữ liệu tháng {month:00}/{year} để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string savePath = OpenSaveFileDialog($"LichSuGoi_Thang{month:00}_{year}");
            if (string.IsNullOrEmpty(savePath)) return;

            btnExportMonth.Enabled = false;
            var waitForm = CreateWaitForm();
            waitForm.Show(this);

            await Task.Run(() =>
            {
                try
                {
                    ExportMonthlyReport(data, year, month, savePath);

                    this.BeginInvoke(new Action(() =>
                    {
                        waitForm.Close();
                        waitForm.Dispose();
                        btnExportMonth.Enabled = true;

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
                        btnExportMonth.Enabled = true;
                        MessageBox.Show(ex.Message, "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private static void ExportMonthlyReport(List<CallSubleaderHistoryDTO> data, int year, int month, string savePath)
        {
            var monthSummary = data
                .GroupBy(x => x.Position)
                .Select(g => new { Position = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ThenBy(x => x.Position)
                .ToList();

            var days = data.Select(x => x.CreatedDate.Date).Distinct().OrderBy(d => d).ToList();
            var dayCounts = data
                .GroupBy(x => new { x.Position, Day = x.CreatedDate.Date })
                .ToDictionary(g => g.Key, g => g.Count());

            using (var pkg = new ExcelPackage())
            {
                var ws = pkg.Workbook.Worksheets.Add($"Báo cáo tháng {month:00}-{year}");

                // ===== Bảng chi tiết (cột A-G) =====
                ws.Cells[1, 1].Value = "STT";
                ws.Cells[1, 2].Value = "Thời gian";
                ws.Cells[1, 3].Value = "Mã chuyền";
                ws.Cells[1, 4].Value = "Tên chuyền";
                ws.Cells[1, 5].Value = "Vị trí";
                ws.Cells[1, 6].Value = "Ca làm";
                ws.Cells[1, 7].Value = "Tổng số lần gọi";
                StyleHeader(ws.Cells[1, 1, 1, 7]);

                for (int i = 0; i < data.Count; i++)
                {
                    int row = i + 2;
                    ws.Cells[row, 1].Value = i + 1;
                    ws.Cells[row, 2].Value = data[i].CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[row, 3].Value = data[i].LineCode;
                    ws.Cells[row, 4].Value = data[i].LineName;
                    ws.Cells[row, 5].Value = data[i].Position;
                    ws.Cells[row, 6].Value = data[i].ShiftName;
                    ws.Cells[row, 7].Value = data[i].TotalCount;
                }

                // ===== Bảng tổng hợp theo tháng (cột I-J) + biểu đồ =====
                const int sumCol = 9;
                ws.Cells[1, sumCol].Value = "VỊ TRÍ GỌI NHIỀU TRONG THÁNG";
                ws.Cells[1, sumCol, 1, sumCol + 2].Merge = true;
                ws.Cells[1, sumCol].Style.Font.Bold = true;

                ws.Cells[2, sumCol].Value = "Vị trí";
                ws.Cells[2, sumCol + 1].Value = "Số lần gọi";
                StyleHeader(ws.Cells[2, sumCol, 2, sumCol + 1]);

                for (int i = 0; i < monthSummary.Count; i++)
                {
                    ws.Cells[3 + i, sumCol].Value = monthSummary[i].Position;
                    ws.Cells[3 + i, sumCol + 1].Value = monthSummary[i].Count;
                }
                int sumLastRow = 2 + monthSummary.Count;

                var chartMonth = (ExcelBarChart)ws.Drawings.AddChart("chartMonth", eChartType.ColumnClustered);
                chartMonth.Title.Text = "Vị trí gọi nhiều trong tháng";
                chartMonth.SetPosition(1, 0, sumCol + 2, 0);
                chartMonth.SetSize(Math.Max(600, monthSummary.Count * 22), 420);
                chartMonth.GapWidth = 10;
                chartMonth.DataLabel.ShowValue = true;
                var monthSerie = chartMonth.Series.Add(
                    ws.Cells[3, sumCol + 1, sumLastRow, sumCol + 1],
                    ws.Cells[3, sumCol, sumLastRow, sumCol]);
                monthSerie.Header = "Số lần gọi";
                chartMonth.Legend.Remove();

                // ===== Bảng theo ngày (pivot: vị trí x ngày, heatmap) + biểu đồ =====
                int pivotStartCol = sumCol + 3;          // thẳng mép trái biểu đồ tháng
                int pivotTitleRow = 24;                  // dưới hình biểu đồ tháng (cao 420px ≈ 21 dòng)
                int pivotHeaderRow = pivotTitleRow + 1;
                int firstDayCol = pivotStartCol + 1;
                int lastDayCol = pivotStartCol + days.Count;
                int totalCol = lastDayCol + 1;

                ws.Cells[pivotTitleRow, pivotStartCol].Value = "VỊ TRÍ GỌI NHIỀU TRONG NGÀY";
                ws.Cells[pivotTitleRow, pivotStartCol, pivotTitleRow, lastDayCol].Merge = true;
                ws.Cells[pivotTitleRow, pivotStartCol].Style.Font.Bold = true;

                ws.Cells[pivotHeaderRow, pivotStartCol].Value = "Vị trí";
                for (int d = 0; d < days.Count; d++)
                    ws.Cells[pivotHeaderRow, firstDayCol + d].Value = days[d].ToString("dd/MM");
                ws.Cells[pivotHeaderRow, totalCol].Value = "Total";
                StyleHeader(ws.Cells[pivotHeaderRow, pivotStartCol, pivotHeaderRow, totalCol]);

                var colTotals = new int[days.Count];
                for (int i = 0; i < monthSummary.Count; i++)
                {
                    int row = pivotHeaderRow + 1 + i;
                    string pos = monthSummary[i].Position;
                    ws.Cells[row, pivotStartCol].Value = pos;
                    int rowTotal = 0;
                    for (int d = 0; d < days.Count; d++)
                    {
                        int count;
                        dayCounts.TryGetValue(new { Position = pos, Day = days[d] }, out count);
                        ws.Cells[row, firstDayCol + d].Value = count;
                        rowTotal += count;
                        colTotals[d] += count;
                    }
                    ws.Cells[row, totalCol].Value = rowTotal;
                    ws.Cells[row, totalCol].Style.Font.Bold = true;
                }

                // Dòng Total dưới cùng
                int totalRow = pivotHeaderRow + monthSummary.Count + 1;
                ws.Cells[totalRow, pivotStartCol].Value = "Total";
                int grandTotal = 0;
                for (int d = 0; d < days.Count; d++)
                {
                    ws.Cells[totalRow, firstDayCol + d].Value = colTotals[d];
                    grandTotal += colTotals[d];
                }
                ws.Cells[totalRow, totalCol].Value = grandTotal;
                StyleHeader(ws.Cells[totalRow, pivotStartCol, totalRow, totalCol]);

                // Heatmap: thang màu xanh trên vùng dữ liệu theo ngày (không gồm Total)
                var heatRange = ws.Cells[pivotHeaderRow + 1, firstDayCol, pivotHeaderRow + monthSummary.Count, lastDayCol];
                var scale = ws.ConditionalFormatting.AddTwoColorScale(heatRange);
                scale.LowValue.Color = Color.White;
                scale.HighValue.Color = Color.FromArgb(0, 128, 0);

                var pivotRange = ws.Cells[pivotHeaderRow, pivotStartCol, totalRow, totalCol];
                pivotRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                pivotRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                pivotRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                pivotRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                pivotRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                /* Remove chart follow day
                var chartDay = (ExcelBarChart)ws.Drawings.AddChart("chartDay", eChartType.ColumnClustered);
                chartDay.Title.Text = "Vị trí gọi nhiều trong ngày";
                chartDay.SetPosition(pivotTitleRow, 0, totalCol + 1, 0);
                chartDay.SetSize(Math.Max(600, days.Count * 60), 420);
                chartDay.GapWidth = 10;
                var xRange = ws.Cells[pivotHeaderRow, firstDayCol, pivotHeaderRow, lastDayCol];
                for (int i = 0; i < monthSummary.Count; i++)
                {
                    int row = pivotHeaderRow + 1 + i;
                    var serie = chartDay.Series.Add(ws.Cells[row, firstDayCol, row, lastDayCol], xRange);
                    serie.Header = monthSummary[i].Position;
                }
                */

                ws.Cells[1, 1, Math.Max(data.Count + 1, totalRow), totalCol].AutoFitColumns();
                pkg.SaveAs(new FileInfo(savePath));
            }
        }

        private static void StyleHeader(ExcelRange range)
        {
            range.Style.Font.Bold = true;
            range.Style.Font.Color.SetColor(Color.White);
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
