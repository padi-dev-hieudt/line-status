namespace LineStatusClient.Forms.Main
{
    partial class uc_main_downtime
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopLiveUpdates();
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalRunningTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotalDowntime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatusText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShiftText = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            //
            // gridControl1
            //
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1000, 600);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            //
            // gridView1
            //
            this.gridView1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridView1.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colNo,
            this.colLineCode,
            this.colLineName,
            this.colTotalRunningTime,
            this.colTotalDowntime,
            this.colStatusText,
            this.colProductCount,
            this.colShiftText,
            this.colStatus});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoPopulateColumns = false;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
            //
            // colNo
            //
            this.colNo.AppearanceCell.Options.UseTextOptions = true;
            this.colNo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNo.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colNo.Caption = "STT";
            this.colNo.FieldName = "No";
            this.colNo.MaxWidth = 60;
            this.colNo.MinWidth = 60;
            this.colNo.Name = "colNo";
            this.colNo.OptionsColumn.AllowEdit = false;
            this.colNo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Max, "No", "{0}")});
            this.colNo.UnboundDataType = typeof(int);
            this.colNo.Visible = true;
            this.colNo.VisibleIndex = 0;
            this.colNo.Width = 60;
            //
            // colLineCode
            //
            this.colLineCode.AppearanceCell.Options.UseTextOptions = true;
            this.colLineCode.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLineCode.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.colLineCode.Caption = "Mã chuyền";
            this.colLineCode.FieldName = "line_code";
            this.colLineCode.Name = "colLineCode";
            this.colLineCode.OptionsColumn.ReadOnly = true;
            this.colLineCode.Visible = true;
            this.colLineCode.VisibleIndex = 1;
            this.colLineCode.Width = 157;
            //
            // colLineName
            //
            this.colLineName.AppearanceCell.Options.UseTextOptions = true;
            this.colLineName.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLineName.Caption = "Tên chuyền";
            this.colLineName.FieldName = "line_nm";
            this.colLineName.Name = "colLineName";
            this.colLineName.OptionsColumn.ReadOnly = true;
            this.colLineName.Visible = true;
            this.colLineName.VisibleIndex = 2;
            this.colLineName.Width = 299;
            //
            // colTotalRunningTime
            //
            this.colTotalRunningTime.AppearanceCell.Options.UseTextOptions = true;
            this.colTotalRunningTime.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTotalRunningTime.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colTotalRunningTime.Caption = "Thời gian chạy";
            this.colTotalRunningTime.FieldName = "TotalRunningTime";
            this.colTotalRunningTime.Name = "colTotalRunningTime";
            this.colTotalRunningTime.OptionsColumn.ReadOnly = true;
            this.colTotalRunningTime.Visible = true;
            this.colTotalRunningTime.VisibleIndex = 3;
            this.colTotalRunningTime.Width = 217;
            //
            // colTotalDowntime
            //
            this.colTotalDowntime.AppearanceCell.Options.UseTextOptions = true;
            this.colTotalDowntime.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTotalDowntime.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colTotalDowntime.Caption = "Thời gian dừng";
            this.colTotalDowntime.FieldName = "TotalDowntime";
            this.colTotalDowntime.Name = "colTotalDowntime";
            this.colTotalDowntime.OptionsColumn.ReadOnly = true;
            this.colTotalDowntime.Visible = true;
            this.colTotalDowntime.VisibleIndex = 4;
            this.colTotalDowntime.Width = 217;
            //
            // colStatusText
            //
            this.colStatusText.AppearanceCell.Options.UseTextOptions = true;
            this.colStatusText.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatusText.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colStatusText.Caption = "Trạng thái";
            this.colStatusText.FieldName = "status_text";
            this.colStatusText.Name = "colStatusText";
            this.colStatusText.OptionsColumn.ReadOnly = true;
            this.colStatusText.Visible = true;
            this.colStatusText.VisibleIndex = 5;
            this.colStatusText.Width = 244;
            //
            // colProductCount
            //
            this.colProductCount.AppearanceCell.Options.UseTextOptions = true;
            this.colProductCount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colProductCount.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colProductCount.Caption = "Số lượng sản phẩm";
            this.colProductCount.FieldName = "product_count";
            this.colProductCount.Name = "colProductCount";
            this.colProductCount.OptionsColumn.ReadOnly = true;
            this.colProductCount.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "product_count", "Tổng = {0:0.##}")});
            this.colProductCount.Visible = true;
            this.colProductCount.VisibleIndex = 6;
            this.colProductCount.Width = 256;
            //
            // colShiftText
            //
            this.colShiftText.AppearanceCell.Options.UseTextOptions = true;
            this.colShiftText.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colShiftText.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colShiftText.Caption = "Ca làm";
            this.colShiftText.FieldName = "shift_text";
            this.colShiftText.Name = "colShiftText";
            this.colShiftText.OptionsColumn.ReadOnly = true;
            this.colShiftText.Visible = true;
            this.colShiftText.VisibleIndex = 7;
            this.colShiftText.Width = 165;
            //
            // colStatus
            //
            // Cột ẩn chứa mã trạng thái (int) dùng cho timer và tô màu hàng.
            this.colStatus.Caption = "status";
            this.colStatus.FieldName = "status";
            this.colStatus.Name = "colStatus";
            //
            // uc_main_downtime
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Name = "uc_main_downtime";
            this.Size = new System.Drawing.Size(1000, 600);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colNo;
        private DevExpress.XtraGrid.Columns.GridColumn colLineCode;
        private DevExpress.XtraGrid.Columns.GridColumn colLineName;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalRunningTime;
        private DevExpress.XtraGrid.Columns.GridColumn colTotalDowntime;
        private DevExpress.XtraGrid.Columns.GridColumn colStatusText;
        private DevExpress.XtraGrid.Columns.GridColumn colProductCount;
        private DevExpress.XtraGrid.Columns.GridColumn colShiftText;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
    }
}
