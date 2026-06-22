namespace LineStatusClient
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.pnlHeader = new DevExpress.XtraEditors.PanelControl();
            this.cboDisplayDataType = new System.Windows.Forms.ComboBox();
            this.btnRefesh = new DevExpress.XtraEditors.SimpleButton();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlContent = new DevExpress.XtraEditors.PanelControl();
            this.menuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnitemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnitemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlFooter = new DevExpress.XtraEditors.PanelControl();
            this.eldowntime_08 = new System.Windows.Forms.Label();
            this.eldowntime_07 = new System.Windows.Forms.TextBox();
            this.eldowntime_06 = new System.Windows.Forms.Label();
            this.eldowntime_05 = new System.Windows.Forms.TextBox();
            this.eldowntime_04 = new System.Windows.Forms.Label();
            this.eldowntime_03 = new System.Windows.Forms.TextBox();
            this.eldowntime_02 = new System.Windows.Forms.Label();
            this.eldowntime_01 = new System.Windows.Forms.TextBox();
            this.btnSetting = new DevExpress.XtraEditors.SimpleButton();
            this.flyoutPanel1 = new DevExpress.Utils.FlyoutPanel();
            this.btnShift = new DevExpress.XtraEditors.SimpleButton();
            this.btnEmail = new DevExpress.XtraEditors.SimpleButton();
            this.btnHistory = new DevExpress.XtraEditors.SimpleButton();
            this.btnRunAtStartup = new DevExpress.XtraEditors.SimpleButton();
            this.btnHide = new DevExpress.XtraEditors.SimpleButton();
            this.elcalltime_02 = new System.Windows.Forms.Label();
            this.elcalltime_01 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pnlHeader)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).BeginInit();
            this.menuNotify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFooter)).BeginInit();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanel1)).BeginInit();
            this.flyoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Appearance.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlHeader.Appearance.Options.UseBackColor = true;
            this.pnlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlHeader.Controls.Add(this.cboDisplayDataType);
            this.pnlHeader.Controls.Add(this.btnRefesh);
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1430, 60);
            this.pnlHeader.TabIndex = 0;
            // 
            // cboDisplayDataType
            // 
            this.cboDisplayDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDisplayDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDisplayDataType.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDisplayDataType.FormattingEnabled = true;
            this.cboDisplayDataType.Location = new System.Drawing.Point(1144, 12);
            this.cboDisplayDataType.Name = "cboDisplayDataType";
            this.cboDisplayDataType.Size = new System.Drawing.Size(224, 27);
            this.cboDisplayDataType.TabIndex = 2;
            // 
            // btnRefesh
            // 
            this.btnRefesh.Appearance.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnRefesh.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnRefesh.Appearance.Options.UseBackColor = true;
            this.btnRefesh.Appearance.Options.UseBorderColor = true;
            this.btnRefesh.AutoSize = true;
            this.btnRefesh.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefesh.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnRefesh.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRefesh.ImageOptions.SvgImage")));
            this.btnRefesh.ImageOptions.SvgImageSize = new System.Drawing.Size(50, 50);
            this.btnRefesh.Location = new System.Drawing.Point(1374, 0);
            this.btnRefesh.Name = "btnRefesh";
            this.btnRefesh.Size = new System.Drawing.Size(56, 60);
            this.btnRefesh.TabIndex = 1;
            this.btnRefesh.ToolTip = "Làm mới dữ liệu";
            this.btnRefesh.Click += new System.EventHandler(this.btnRefesh_Click);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1430, 60);
            this.label5.TabIndex = 0;
            this.label5.Text = "HỆ THÔNG QUẢN LÝ DÂY CHUYỀN (SEB)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlContent
            //
            this.pnlContent.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 60);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1430, 575);
            this.pnlContent.TabIndex = 1;
            //
            // menuNotify
            //
            this.menuNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnitemShow,
            this.mnitemExit});
            this.menuNotify.Name = "notifyIconMenu";
            this.menuNotify.Size = new System.Drawing.Size(117, 48);
            // 
            // mnitemShow
            // 
            this.mnitemShow.Name = "mnitemShow";
            this.mnitemShow.Size = new System.Drawing.Size(116, 22);
            this.mnitemShow.Text = "Hiển thị";
            this.mnitemShow.Click += new System.EventHandler(this.mnitemShow_Click);
            // 
            // mnitemExit
            // 
            this.mnitemExit.Name = "mnitemExit";
            this.mnitemExit.Size = new System.Drawing.Size(116, 22);
            this.mnitemExit.Text = "Thoát";
            this.mnitemExit.Click += new System.EventHandler(this.mnitemExit_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.menuNotify;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Trạng thái dây chuyền";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Appearance.BackColor = System.Drawing.Color.White;
            this.pnlFooter.Appearance.Options.UseBackColor = true;
            this.pnlFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlFooter.Controls.Add(this.elcalltime_02);
            this.pnlFooter.Controls.Add(this.elcalltime_01);
            this.pnlFooter.Controls.Add(this.eldowntime_08);
            this.pnlFooter.Controls.Add(this.eldowntime_07);
            this.pnlFooter.Controls.Add(this.eldowntime_06);
            this.pnlFooter.Controls.Add(this.eldowntime_05);
            this.pnlFooter.Controls.Add(this.eldowntime_04);
            this.pnlFooter.Controls.Add(this.eldowntime_03);
            this.pnlFooter.Controls.Add(this.eldowntime_02);
            this.pnlFooter.Controls.Add(this.eldowntime_01);
            this.pnlFooter.Controls.Add(this.btnSetting);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 635);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1430, 45);
            this.pnlFooter.TabIndex = 2;
            // 
            // eldowntime_08
            // 
            this.eldowntime_08.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_08.AutoSize = true;
            this.eldowntime_08.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eldowntime_08.Location = new System.Drawing.Point(1363, 13);
            this.eldowntime_08.Name = "eldowntime_08";
            this.eldowntime_08.Size = new System.Drawing.Size(52, 20);
            this.eldowntime_08.TabIndex = 2;
            this.eldowntime_08.Text = "Dừng";
            // 
            // eldowntime_07
            // 
            this.eldowntime_07.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_07.BackColor = System.Drawing.Color.OrangeRed;
            this.eldowntime_07.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eldowntime_07.Location = new System.Drawing.Point(1324, 8);
            this.eldowntime_07.Multiline = true;
            this.eldowntime_07.Name = "eldowntime_07";
            this.eldowntime_07.ReadOnly = true;
            this.eldowntime_07.Size = new System.Drawing.Size(35, 30);
            this.eldowntime_07.TabIndex = 1;
            // 
            // eldowntime_06
            // 
            this.eldowntime_06.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_06.AutoSize = true;
            this.eldowntime_06.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.eldowntime_06.Location = new System.Drawing.Point(1257, 13);
            this.eldowntime_06.Name = "eldowntime_06";
            this.eldowntime_06.Size = new System.Drawing.Size(45, 20);
            this.eldowntime_06.TabIndex = 2;
            this.eldowntime_06.Text = "Nghỉ";
            // 
            // eldowntime_05
            // 
            this.eldowntime_05.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_05.BackColor = System.Drawing.Color.Yellow;
            this.eldowntime_05.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eldowntime_05.Location = new System.Drawing.Point(1218, 8);
            this.eldowntime_05.Multiline = true;
            this.eldowntime_05.Name = "eldowntime_05";
            this.eldowntime_05.ReadOnly = true;
            this.eldowntime_05.Size = new System.Drawing.Size(35, 30);
            this.eldowntime_05.TabIndex = 1;
            // 
            // eldowntime_04
            // 
            this.eldowntime_04.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_04.AutoSize = true;
            this.eldowntime_04.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.eldowntime_04.Location = new System.Drawing.Point(1145, 13);
            this.eldowntime_04.Name = "eldowntime_04";
            this.eldowntime_04.Size = new System.Drawing.Size(49, 20);
            this.eldowntime_04.TabIndex = 2;
            this.eldowntime_04.Text = "Chạy";
            // 
            // eldowntime_03
            // 
            this.eldowntime_03.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_03.BackColor = System.Drawing.Color.SpringGreen;
            this.eldowntime_03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eldowntime_03.Location = new System.Drawing.Point(1106, 8);
            this.eldowntime_03.Multiline = true;
            this.eldowntime_03.Name = "eldowntime_03";
            this.eldowntime_03.ReadOnly = true;
            this.eldowntime_03.Size = new System.Drawing.Size(35, 30);
            this.eldowntime_03.TabIndex = 1;
            // 
            // eldowntime_02
            // 
            this.eldowntime_02.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_02.AutoSize = true;
            this.eldowntime_02.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.eldowntime_02.Location = new System.Drawing.Point(934, 13);
            this.eldowntime_02.Name = "eldowntime_02";
            this.eldowntime_02.Size = new System.Drawing.Size(146, 20);
            this.eldowntime_02.TabIndex = 2;
            this.eldowntime_02.Text = "Không hoạt động";
            // 
            // eldowntime_01
            // 
            this.eldowntime_01.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.eldowntime_01.BackColor = System.Drawing.Color.White;
            this.eldowntime_01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.eldowntime_01.Location = new System.Drawing.Point(895, 8);
            this.eldowntime_01.Multiline = true;
            this.eldowntime_01.Name = "eldowntime_01";
            this.eldowntime_01.ReadOnly = true;
            this.eldowntime_01.Size = new System.Drawing.Size(35, 30);
            this.eldowntime_01.TabIndex = 1;
            // 
            // btnSetting
            // 
            this.btnSetting.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnSetting.Appearance.Options.UseForeColor = true;
            this.btnSetting.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.ImageOptions.Image")));
            this.btnSetting.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSetting.Location = new System.Drawing.Point(12, 4);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(38, 36);
            this.btnSetting.TabIndex = 0;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // flyoutPanel1
            // 
            this.flyoutPanel1.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.flyoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flyoutPanel1.Controls.Add(this.btnShift);
            this.flyoutPanel1.Controls.Add(this.btnEmail);
            this.flyoutPanel1.Controls.Add(this.btnHistory);
            this.flyoutPanel1.Controls.Add(this.btnRunAtStartup);
            this.flyoutPanel1.Controls.Add(this.btnHide);
            this.flyoutPanel1.Location = new System.Drawing.Point(56, 415);
            this.flyoutPanel1.Name = "flyoutPanel1";
            this.flyoutPanel1.OptionsButtonPanel.ButtonPanelContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.flyoutPanel1.OptionsButtonPanel.ButtonPanelHeight = 100;
            this.flyoutPanel1.OwnerControl = this.btnSetting;
            this.flyoutPanel1.Size = new System.Drawing.Size(221, 233);
            this.flyoutPanel1.TabIndex = 3;
            // 
            // btnShift
            // 
            this.btnShift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShift.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShift.Appearance.Options.UseFont = true;
            this.btnShift.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnShift.ImageOptions.Image")));
            this.btnShift.Location = new System.Drawing.Point(4, 187);
            this.btnShift.Name = "btnShift";
            this.btnShift.Size = new System.Drawing.Size(211, 41);
            this.btnShift.TabIndex = 3;
            this.btnShift.Text = "Shift";
            this.btnShift.Click += new System.EventHandler(this.btnShift_Click);
            // 
            // btnEmail
            // 
            this.btnEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEmail.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmail.Appearance.Options.UseFont = true;
            this.btnEmail.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEmail.ImageOptions.Image")));
            this.btnEmail.Location = new System.Drawing.Point(4, 145);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(211, 41);
            this.btnEmail.TabIndex = 3;
            this.btnEmail.Text = "Email";
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistory.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistory.Appearance.Options.UseFont = true;
            this.btnHistory.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHistory.ImageOptions.Image")));
            this.btnHistory.Location = new System.Drawing.Point(4, 98);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(211, 41);
            this.btnHistory.TabIndex = 3;
            this.btnHistory.Text = "Lịch sử";
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnRunAtStartup
            // 
            this.btnRunAtStartup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunAtStartup.Appearance.BackColor = System.Drawing.Color.Silver;
            this.btnRunAtStartup.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunAtStartup.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnRunAtStartup.Appearance.Options.UseBackColor = true;
            this.btnRunAtStartup.Appearance.Options.UseFont = true;
            this.btnRunAtStartup.Appearance.Options.UseForeColor = true;
            this.btnRunAtStartup.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRunAtStartup.ImageOptions.Image")));
            this.btnRunAtStartup.Location = new System.Drawing.Point(4, 51);
            this.btnRunAtStartup.Name = "btnRunAtStartup";
            this.btnRunAtStartup.Size = new System.Drawing.Size(211, 41);
            this.btnRunAtStartup.TabIndex = 3;
            this.btnRunAtStartup.Tag = "RunStartup";
            this.btnRunAtStartup.Text = "Chạy cùng hệ thống";
            this.btnRunAtStartup.Click += new System.EventHandler(this.btnRunAtStartup_Click);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHide.Appearance.Options.UseFont = true;
            this.btnHide.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnHide.ImageOptions.Image")));
            this.btnHide.Location = new System.Drawing.Point(4, 4);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(211, 41);
            this.btnHide.TabIndex = 3;
            this.btnHide.Text = "Ẩn";
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // elcalltime_02
            // 
            this.elcalltime_02.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.elcalltime_02.AutoSize = true;
            this.elcalltime_02.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.elcalltime_02.Location = new System.Drawing.Point(1201, 13);
            this.elcalltime_02.Name = "elcalltime_02";
            this.elcalltime_02.Size = new System.Drawing.Size(217, 20);
            this.elcalltime_02.TabIndex = 4;
            this.elcalltime_02.Text = "Số lần gọi nhiều hơn 5 lần";
            this.elcalltime_02.Visible = false;
            // 
            // elcalltime_01
            // 
            this.elcalltime_01.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.elcalltime_01.BackColor = System.Drawing.Color.Orange;
            this.elcalltime_01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.elcalltime_01.Location = new System.Drawing.Point(1162, 8);
            this.elcalltime_01.Multiline = true;
            this.elcalltime_01.Name = "elcalltime_01";
            this.elcalltime_01.ReadOnly = true;
            this.elcalltime_01.Size = new System.Drawing.Size(35, 30);
            this.elcalltime_01.TabIndex = 3;
            this.elcalltime_01.Visible = false;
            // 
            // FormMain
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1430, 680);
            this.Controls.Add(this.flyoutPanel1);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.IconOptions.Image = global::LineStatusClient.Properties.Resources.line;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trạng thái dây chuyền";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pnlHeader)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContent)).EndInit();
            this.menuNotify.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlFooter)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanel1)).EndInit();
            this.flyoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlHeader;
        private DevExpress.XtraEditors.PanelControl pnlContent;
        private System.Windows.Forms.ContextMenuStrip menuNotify;
        private System.Windows.Forms.ToolStripMenuItem mnitemShow;
        private System.Windows.Forms.ToolStripMenuItem mnitemExit;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private DevExpress.XtraEditors.PanelControl pnlFooter;
        private DevExpress.XtraEditors.SimpleButton btnSetting;
        private System.Windows.Forms.Label eldowntime_02;
        private System.Windows.Forms.TextBox eldowntime_01;
        private System.Windows.Forms.Label eldowntime_08;
        private System.Windows.Forms.TextBox eldowntime_07;
        private System.Windows.Forms.Label eldowntime_06;
        private System.Windows.Forms.TextBox eldowntime_05;
        private System.Windows.Forms.Label eldowntime_04;
        private System.Windows.Forms.TextBox eldowntime_03;
        private DevExpress.Utils.FlyoutPanel flyoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnHistory;
        private DevExpress.XtraEditors.SimpleButton btnRunAtStartup;
        private DevExpress.XtraEditors.SimpleButton btnHide;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnEmail;
        private DevExpress.XtraEditors.SimpleButton btnShift;
        private DevExpress.XtraEditors.SimpleButton btnRefesh;
        private System.Windows.Forms.ComboBox cboDisplayDataType;
        private System.Windows.Forms.Label elcalltime_02;
        private System.Windows.Forms.TextBox elcalltime_01;
    }
}

