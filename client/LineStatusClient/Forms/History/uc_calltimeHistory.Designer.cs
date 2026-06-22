namespace LineStatusClient.Forms.History
{
    partial class uc_calltimeHistory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.dtpFrom = new DevExpress.XtraEditors.DateEdit();
            this.lblTo = new DevExpress.XtraEditors.LabelControl();
            this.dtpTo = new DevExpress.XtraEditors.DateEdit();
            this.btnToday = new DevExpress.XtraEditors.SimpleButton();
            this.btnYesterday = new DevExpress.XtraEditors.SimpleButton();
            this.btnThisWeek = new DevExpress.XtraEditors.SimpleButton();
            this.btnThisMonth = new DevExpress.XtraEditors.SimpleButton();
            this.lblLine = new DevExpress.XtraEditors.LabelControl();
            this.cb_Line = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.grvCb_Line = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colLineCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblPosition = new DevExpress.XtraEditors.LabelControl();
            this.txtPosition = new DevExpress.XtraEditors.TextEdit();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnReset = new DevExpress.XtraEditors.SimpleButton();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineCode2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLineName2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPosition = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Line.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvCb_Line)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.dtpFrom);
            this.groupControl1.Controls.Add(this.lblTo);
            this.groupControl1.Controls.Add(this.dtpTo);
            this.groupControl1.Controls.Add(this.btnToday);
            this.groupControl1.Controls.Add(this.btnYesterday);
            this.groupControl1.Controls.Add(this.btnThisWeek);
            this.groupControl1.Controls.Add(this.btnThisMonth);
            this.groupControl1.Controls.Add(this.lblLine);
            this.groupControl1.Controls.Add(this.cb_Line);
            this.groupControl1.Controls.Add(this.lblPosition);
            this.groupControl1.Controls.Add(this.txtPosition);
            this.groupControl1.Controls.Add(this.btnExport);
            this.groupControl1.Controls.Add(this.btnReset);
            this.groupControl1.Controls.Add(this.btnSearch);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1254, 58);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Bộ lọc";
            // 
            // dtpFrom
            // 
            this.dtpFrom.EditValue = new System.DateTime(2026, 6, 21, 0, 0, 0, 0);
            this.dtpFrom.Location = new System.Drawing.Point(6, 28);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Properties.AllowFocused = false;
            this.dtpFrom.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpFrom.Properties.Appearance.Options.UseFont = true;
            this.dtpFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFrom.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtpFrom.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpFrom.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtpFrom.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpFrom.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.dtpFrom.Properties.UseMaskAsDisplayFormat = true;
            this.dtpFrom.Size = new System.Drawing.Size(110, 22);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.EditValueChanged += new System.EventHandler(this.dtpFrom_EditValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.lblTo.Appearance.Options.UseFont = true;
            this.lblTo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTo.Location = new System.Drawing.Point(120, 30);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(14, 20);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "—";
            // 
            // dtpTo
            // 
            this.dtpTo.EditValue = new System.DateTime(2026, 6, 21, 0, 0, 0, 0);
            this.dtpTo.Location = new System.Drawing.Point(138, 28);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Properties.AllowFocused = false;
            this.dtpTo.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpTo.Properties.Appearance.Options.UseFont = true;
            this.dtpTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpTo.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtpTo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpTo.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtpTo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpTo.Properties.MaskSettings.Set("mask", "dd/MM/yyyy");
            this.dtpTo.Properties.UseMaskAsDisplayFormat = true;
            this.dtpTo.Size = new System.Drawing.Size(110, 22);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.EditValueChanged += new System.EventHandler(this.dtpTo_EditValueChanged);
            // 
            // btnToday
            // 
            this.btnToday.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnToday.Appearance.Options.UseFont = true;
            this.btnToday.Location = new System.Drawing.Point(258, 28);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(72, 24);
            this.btnToday.TabIndex = 4;
            this.btnToday.Text = "Hôm nay";
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnYesterday
            // 
            this.btnYesterday.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnYesterday.Appearance.Options.UseFont = true;
            this.btnYesterday.Location = new System.Drawing.Point(334, 28);
            this.btnYesterday.Name = "btnYesterday";
            this.btnYesterday.Size = new System.Drawing.Size(72, 24);
            this.btnYesterday.TabIndex = 5;
            this.btnYesterday.Text = "Hôm qua";
            this.btnYesterday.Click += new System.EventHandler(this.btnYesterday_Click);
            // 
            // btnThisWeek
            // 
            this.btnThisWeek.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnThisWeek.Appearance.Options.UseFont = true;
            this.btnThisWeek.Location = new System.Drawing.Point(410, 28);
            this.btnThisWeek.Name = "btnThisWeek";
            this.btnThisWeek.Size = new System.Drawing.Size(68, 24);
            this.btnThisWeek.TabIndex = 6;
            this.btnThisWeek.Text = "Tuần này";
            this.btnThisWeek.Click += new System.EventHandler(this.btnThisWeek_Click);
            // 
            // btnThisMonth
            // 
            this.btnThisMonth.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnThisMonth.Appearance.Options.UseFont = true;
            this.btnThisMonth.Location = new System.Drawing.Point(482, 28);
            this.btnThisMonth.Name = "btnThisMonth";
            this.btnThisMonth.Size = new System.Drawing.Size(72, 24);
            this.btnThisMonth.TabIndex = 7;
            this.btnThisMonth.Text = "Tháng này";
            this.btnThisMonth.Click += new System.EventHandler(this.btnThisMonth_Click);
            // 
            // lblLine
            // 
            this.lblLine.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblLine.Appearance.Options.UseFont = true;
            this.lblLine.Appearance.Options.UseTextOptions = true;
            this.lblLine.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblLine.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblLine.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblLine.Location = new System.Drawing.Point(564, 28);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(82, 24);
            this.lblLine.TabIndex = 8;
            this.lblLine.Text = "Dây chuyền:";
            // 
            // cb_Line
            // 
            this.cb_Line.Location = new System.Drawing.Point(650, 28);
            this.cb_Line.Name = "cb_Line";
            this.cb_Line.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cb_Line.Properties.Appearance.Options.UseFont = true;
            this.cb_Line.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cb_Line.Properties.NullText = "(Tất cả)";
            this.cb_Line.Properties.PopupView = this.grvCb_Line;
            this.cb_Line.Size = new System.Drawing.Size(120, 22);
            this.cb_Line.TabIndex = 9;
            // 
            // grvCb_Line
            // 
            this.grvCb_Line.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colLineCode,
            this.colLineName});
            this.grvCb_Line.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grvCb_Line.Name = "grvCb_Line";
            this.grvCb_Line.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grvCb_Line.OptionsView.ShowGroupPanel = false;
            // 
            // colLineCode
            // 
            this.colLineCode.Caption = "Mã chuyền";
            this.colLineCode.FieldName = "Line_c";
            this.colLineCode.Name = "colLineCode";
            this.colLineCode.Visible = true;
            this.colLineCode.VisibleIndex = 0;
            this.colLineCode.Width = 100;
            // 
            // colLineName
            // 
            this.colLineName.Caption = "Tên chuyền";
            this.colLineName.FieldName = "Line_nm";
            this.colLineName.Name = "colLineName";
            this.colLineName.Visible = true;
            this.colLineName.VisibleIndex = 1;
            this.colLineName.Width = 200;
            // 
            // lblPosition
            // 
            this.lblPosition.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblPosition.Appearance.Options.UseFont = true;
            this.lblPosition.Appearance.Options.UseTextOptions = true;
            this.lblPosition.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblPosition.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblPosition.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblPosition.Location = new System.Drawing.Point(776, 28);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(40, 24);
            this.lblPosition.TabIndex = 10;
            this.lblPosition.Text = "Vị trí:";
            // 
            // txtPosition
            // 
            this.txtPosition.Location = new System.Drawing.Point(820, 28);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtPosition.Properties.Appearance.Options.UseFont = true;
            this.txtPosition.Size = new System.Drawing.Size(90, 22);
            this.txtPosition.TabIndex = 11;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnExport.Appearance.Options.UseFont = true;
            this.btnExport.Location = new System.Drawing.Point(970, 28);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 24);
            this.btnExport.TabIndex = 14;
            this.btnExport.Text = "Xuất Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnReset.Appearance.Options.UseFont = true;
            this.btnReset.Location = new System.Drawing.Point(1063, 28);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(78, 24);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "Đặt lại";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Appearance.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSearch.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance.Options.UseBackColor = true;
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.Appearance.Options.UseForeColor = true;
            this.btnSearch.Location = new System.Drawing.Point(1144, 28);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(98, 24);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gridControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 58);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1254, 529);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "Dữ liệu";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 23);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1250, 504);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colNo,
            this.colCreatedDate,
            this.colLineCode2,
            this.colLineName2,
            this.colPosition});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridView1_CustomUnboundColumnData);
            // 
            // colNo
            // 
            this.colNo.AppearanceCell.Options.UseTextOptions = true;
            this.colNo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNo.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colNo.Caption = "STT";
            this.colNo.DisplayFormat.FormatString = "n0";
            this.colNo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colNo.FieldName = "No";
            this.colNo.MaxWidth = 50;
            this.colNo.MinWidth = 50;
            this.colNo.Name = "colNo";
            this.colNo.OptionsColumn.AllowEdit = false;
            this.colNo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "No", "{0}")});
            this.colNo.UnboundDataType = typeof(int);
            this.colNo.Visible = true;
            this.colNo.VisibleIndex = 0;
            this.colNo.Width = 50;
            // 
            // colCreatedDate
            // 
            this.colCreatedDate.AppearanceCell.Options.UseTextOptions = true;
            this.colCreatedDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCreatedDate.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colCreatedDate.Caption = "Thời gian";
            this.colCreatedDate.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
            this.colCreatedDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colCreatedDate.FieldName = "CreatedDate";
            this.colCreatedDate.Name = "colCreatedDate";
            this.colCreatedDate.OptionsColumn.AllowEdit = false;
            this.colCreatedDate.Visible = true;
            this.colCreatedDate.VisibleIndex = 1;
            this.colCreatedDate.Width = 180;
            // 
            // colLineCode2
            // 
            this.colLineCode2.AppearanceCell.Options.UseTextOptions = true;
            this.colLineCode2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colLineCode2.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLineCode2.Caption = "Mã chuyền";
            this.colLineCode2.FieldName = "LineCode";
            this.colLineCode2.Name = "colLineCode2";
            this.colLineCode2.OptionsColumn.AllowEdit = false;
            this.colLineCode2.Visible = true;
            this.colLineCode2.VisibleIndex = 2;
            this.colLineCode2.Width = 120;
            // 
            // colLineName2
            // 
            this.colLineName2.AppearanceCell.Options.UseTextOptions = true;
            this.colLineName2.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colLineName2.Caption = "Tên chuyền";
            this.colLineName2.FieldName = "LineName";
            this.colLineName2.Name = "colLineName2";
            this.colLineName2.OptionsColumn.AllowEdit = false;
            this.colLineName2.Visible = true;
            this.colLineName2.VisibleIndex = 3;
            this.colLineName2.Width = 220;
            // 
            // colPosition
            // 
            this.colPosition.AppearanceCell.Options.UseTextOptions = true;
            this.colPosition.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.colPosition.Caption = "Vị trí";
            this.colPosition.FieldName = "Position";
            this.colPosition.Name = "colPosition";
            this.colPosition.OptionsColumn.AllowEdit = false;
            this.colPosition.Visible = true;
            this.colPosition.VisibleIndex = 4;
            this.colPosition.Width = 220;
            // 
            // uc_calltimeHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "uc_calltimeHistory";
            this.Size = new System.Drawing.Size(1254, 587);
            this.Load += new System.EventHandler(this.uc_calltimeHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtpFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_Line.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvCb_Line)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.DateEdit dtpFrom;
        private DevExpress.XtraEditors.LabelControl lblTo;
        private DevExpress.XtraEditors.DateEdit dtpTo;
        private DevExpress.XtraEditors.SimpleButton btnToday;
        private DevExpress.XtraEditors.SimpleButton btnYesterday;
        private DevExpress.XtraEditors.SimpleButton btnThisWeek;
        private DevExpress.XtraEditors.SimpleButton btnThisMonth;
        private DevExpress.XtraEditors.LabelControl lblLine;
        private DevExpress.XtraEditors.SearchLookUpEdit cb_Line;
        private DevExpress.XtraGrid.Views.Grid.GridView grvCb_Line;
        private DevExpress.XtraGrid.Columns.GridColumn colLineCode;
        private DevExpress.XtraGrid.Columns.GridColumn colLineName;
        private DevExpress.XtraEditors.LabelControl lblPosition;
        private DevExpress.XtraEditors.TextEdit txtPosition;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.SimpleButton btnReset;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colNo;
        private DevExpress.XtraGrid.Columns.GridColumn colCreatedDate;
        private DevExpress.XtraGrid.Columns.GridColumn colLineCode2;
        private DevExpress.XtraGrid.Columns.GridColumn colLineName2;
        private DevExpress.XtraGrid.Columns.GridColumn colPosition;
    }
}
