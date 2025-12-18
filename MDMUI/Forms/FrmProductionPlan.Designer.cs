using System.Drawing; // 添加 Font 的 using
using System.Windows.Forms; // 添加 Label 和 Point 的 using

namespace MDMUI
{
    partial class FrmProductionPlan
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelList = new System.Windows.Forms.Panel();
            this.dgvPlan = new System.Windows.Forms.DataGridView();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.lblFilterDateRange = new System.Windows.Forms.Label();
            this.dtpFilterStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFilterEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblFilterSeparator = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.panelDetailButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.trkProgress = new System.Windows.Forms.TrackBar();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.cmbPriority = new System.Windows.Forms.ComboBox();
            this.lblPriority = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtResponsible = new System.Windows.Forms.TextBox();
            this.lblResponsible = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.txtPlanQuantity = new System.Windows.Forms.TextBox();
            this.lblPlanQuantity = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtPlanId = new System.Windows.Forms.TextBox();
            this.lblPlanId = new System.Windows.Forms.Label();

            this.panelTop.SuspendLayout();
            this.panelList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlan)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelDetailButtons.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkProgress)).BeginInit();
            this.SuspendLayout();
            
            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 40);
            this.panelTop.TabIndex = 0;
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(10, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(183, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "生产计划管理";
            
            // panelList
            this.panelList.BackColor = System.Drawing.Color.White;
            this.panelList.Controls.Add(this.dgvPlan);
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(0, 85);
            this.panelList.Name = "panelList";
            this.panelList.Padding = new System.Windows.Forms.Padding(10);
            this.panelList.Size = new System.Drawing.Size(800, 315);
            this.panelList.TabIndex = 2;
            
            // dgvPlan
            this.dgvPlan.AllowUserToAddRows = false;
            this.dgvPlan.AllowUserToDeleteRows = false;
            this.dgvPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPlan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlan.Location = new System.Drawing.Point(10, 10);
            this.dgvPlan.MultiSelect = false;
            this.dgvPlan.Name = "dgvPlan";
            this.dgvPlan.ReadOnly = true;
            this.dgvPlan.RowHeadersWidth = 51;
            this.dgvPlan.RowTemplate.Height = 24;
            this.dgvPlan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlan.Size = new System.Drawing.Size(780, 295);
            this.dgvPlan.TabIndex = 0;
            this.dgvPlan.SelectionChanged += new System.EventHandler(this.dgvPlan_SelectionChanged);
            
            // panelButtons
            this.panelButtons.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelButtons.Controls.Add(this.lblFilterDateRange);
            this.panelButtons.Controls.Add(this.dtpFilterStartDate);
            this.panelButtons.Controls.Add(this.lblFilterSeparator);
            this.panelButtons.Controls.Add(this.dtpFilterEndDate);
            this.panelButtons.Controls.Add(this.lblSearch);
            this.panelButtons.Controls.Add(this.txtSearch);
            this.panelButtons.Controls.Add(this.btnSearch);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 40);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.panelButtons.Size = new System.Drawing.Size(800, 45);
            this.panelButtons.TabIndex = 1;
            
            // btnNew
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.Location = new System.Drawing.Point(13, 10);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(80, 30);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "新建";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            
            // btnEdit
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.Location = new System.Drawing.Point(100, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            
            // btnDelete
            this.btnDelete.BackColor = System.Drawing.Color.IndianRed;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Location = new System.Drawing.Point(187, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            // btnExport
            this.btnExport.BackColor = System.Drawing.Color.Gray;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.Location = new System.Drawing.Point(274, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 30);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            
            // btnReport
            this.btnReport.BackColor = System.Drawing.Color.Gray;
            this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReport.ForeColor = System.Drawing.Color.White;
            this.btnReport.FlatAppearance.BorderSize = 0;
            this.btnReport.Location = new System.Drawing.Point(361, 10);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(80, 30);
            this.btnReport.TabIndex = 4;
            this.btnReport.Text = "报表";
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            
            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(370, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(52, 15);
            this.lblSearch.TabIndex = 5;
            this.lblSearch.Text = "搜索：";
            
            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(420, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 23);
            this.txtSearch.TabIndex = 6;
            
            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(580, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 28);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            
            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(660, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 28);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            
            // lblFilterDateRange
            this.lblFilterDateRange.AutoSize = true;
            this.lblFilterDateRange.Location = new System.Drawing.Point(13, 10);
            this.lblFilterDateRange.Name = "lblFilterDateRange";
            this.lblFilterDateRange.Size = new System.Drawing.Size(72, 15);
            this.lblFilterDateRange.TabIndex = 9;
            this.lblFilterDateRange.Text = "日期范围:";
            
            // dtpFilterStartDate
            this.dtpFilterStartDate.Checked = false;
            this.dtpFilterStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilterStartDate.Location = new System.Drawing.Point(90, 6);
            this.dtpFilterStartDate.Name = "dtpFilterStartDate";
            this.dtpFilterStartDate.ShowCheckBox = true;
            this.dtpFilterStartDate.Size = new System.Drawing.Size(120, 23);
            this.dtpFilterStartDate.TabIndex = 10;
            
            // lblFilterSeparator
            this.lblFilterSeparator.AutoSize = true;
            this.lblFilterSeparator.Location = new System.Drawing.Point(215, 10);
            this.lblFilterSeparator.Name = "lblFilterSeparator";
            this.lblFilterSeparator.Size = new System.Drawing.Size(15, 15);
            this.lblFilterSeparator.TabIndex = 11;
            this.lblFilterSeparator.Text = "-";
            
            // dtpFilterEndDate
            this.dtpFilterEndDate.Checked = false;
            this.dtpFilterEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilterEndDate.Location = new System.Drawing.Point(235, 6);
            this.dtpFilterEndDate.Name = "dtpFilterEndDate";
            this.dtpFilterEndDate.ShowCheckBox = true;
            this.dtpFilterEndDate.Size = new System.Drawing.Size(120, 23);
            this.dtpFilterEndDate.TabIndex = 12;
            
            // panelBottom
            this.panelBottom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelBottom.Controls.Add(this.btnNew);
            this.panelBottom.Controls.Add(this.btnEdit);
            this.panelBottom.Controls.Add(this.btnDelete);
            this.panelBottom.Controls.Add(this.btnExport);
            this.panelBottom.Controls.Add(this.btnReport);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 400);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.panelBottom.Size = new System.Drawing.Size(800, 50);
            this.panelBottom.TabIndex = 3;
            
            // panelDetail
            this.panelDetail.Location = new System.Drawing.Point(0, 0);
            this.panelDetail.Size = new System.Drawing.Size(800, 450);
            this.panelDetail.Visible = false;
            this.panelDetail.Controls.Add(this.groupBox1);
            this.panelDetail.Controls.Add(this.panelDetailButtons);
            
            // groupBox1
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 390);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生产计划详情";
            this.groupBox1.Controls.Add(this.lblPlanId);
            this.groupBox1.Controls.Add(this.txtPlanId);
            this.groupBox1.Controls.Add(this.lblProductName);
            this.groupBox1.Controls.Add(this.txtProductName);
            this.groupBox1.Controls.Add(this.lblPlanQuantity);
            this.groupBox1.Controls.Add(this.txtPlanQuantity);
            this.groupBox1.Controls.Add(this.lblStartDate);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.lblEndDate);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.lblResponsible);
            this.groupBox1.Controls.Add(this.txtResponsible);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.cmbStatus);
            this.groupBox1.Controls.Add(this.lblPriority);
            this.groupBox1.Controls.Add(this.cmbPriority);
            this.groupBox1.Controls.Add(this.lblProgress);
            this.groupBox1.Controls.Add(this.txtProgress);
            this.groupBox1.Controls.Add(this.trkProgress);
            this.groupBox1.Controls.Add(this.lblNotes);
            this.groupBox1.Controls.Add(this.txtNotes);
            
            // lblPlanId
            this.lblPlanId.AutoSize = true;
            this.lblPlanId.Location = new System.Drawing.Point(20, 30);
            this.lblPlanId.Name = "lblPlanId";
            this.lblPlanId.Size = new System.Drawing.Size(82, 15);
            this.lblPlanId.TabIndex = 0;
            this.lblPlanId.Text = "计划编号：";
            
            // txtPlanId
            this.txtPlanId.Location = new System.Drawing.Point(108, 27);
            this.txtPlanId.Name = "txtPlanId";
            this.txtPlanId.ReadOnly = true;
            this.txtPlanId.Size = new System.Drawing.Size(150, 23);
            this.txtPlanId.TabIndex = 1;
            
            // lblProductName
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(20, 60);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(82, 15);
            this.lblProductName.TabIndex = 2;
            this.lblProductName.Text = "产品名称：";
            
            // txtProductName
            this.txtProductName.Location = new System.Drawing.Point(108, 57);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(250, 23);
            this.txtProductName.TabIndex = 3;
            
            // lblPlanQuantity
            this.lblPlanQuantity.AutoSize = true;
            this.lblPlanQuantity.Location = new System.Drawing.Point(20, 90);
            this.lblPlanQuantity.Name = "lblPlanQuantity";
            this.lblPlanQuantity.Size = new System.Drawing.Size(82, 15);
            this.lblPlanQuantity.TabIndex = 4;
            this.lblPlanQuantity.Text = "计划数量：";
            
            // txtPlanQuantity
            this.txtPlanQuantity.Location = new System.Drawing.Point(108, 87);
            this.txtPlanQuantity.Name = "txtPlanQuantity";
            this.txtPlanQuantity.Size = new System.Drawing.Size(150, 23);
            this.txtPlanQuantity.TabIndex = 5;
            
            // lblStartDate
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(20, 120);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(82, 15);
            this.lblStartDate.TabIndex = 6;
            this.lblStartDate.Text = "开始日期：";
            
            // dtpStartDate
            this.dtpStartDate.Location = new System.Drawing.Point(108, 117);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(150, 23);
            this.dtpStartDate.TabIndex = 7;
            
            // lblEndDate
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(20, 150);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(82, 15);
            this.lblEndDate.TabIndex = 8;
            this.lblEndDate.Text = "结束日期：";
            
            // dtpEndDate
            this.dtpEndDate.Location = new System.Drawing.Point(108, 147);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(150, 23);
            this.dtpEndDate.TabIndex = 9;
            
            // lblResponsible
            this.lblResponsible.AutoSize = true;
            this.lblResponsible.Location = new System.Drawing.Point(20, 180);
            this.lblResponsible.Name = "lblResponsible";
            this.lblResponsible.Size = new System.Drawing.Size(82, 15);
            this.lblResponsible.TabIndex = 10;
            this.lblResponsible.Text = "负责人：";
            
            // txtResponsible
            this.txtResponsible.Location = new System.Drawing.Point(108, 177);
            this.txtResponsible.Name = "txtResponsible";
            this.txtResponsible.Size = new System.Drawing.Size(150, 23);
            this.txtResponsible.TabIndex = 11;
            
            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 210);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(82, 15);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "状态：";
            
            // cmbStatus
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "未开始",
            "进行中",
            "暂停",
            "完成"});
            this.cmbStatus.Location = new System.Drawing.Point(108, 207);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(150, 23);
            this.cmbStatus.TabIndex = 13;
            
            // lblPriority
            this.lblPriority.AutoSize = true;
            this.lblPriority.Location = new System.Drawing.Point(20, 240);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(82, 15);
            this.lblPriority.TabIndex = 14;
            this.lblPriority.Text = "优先级：";
            
            // cmbPriority
            this.cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriority.FormattingEnabled = true;
            this.cmbPriority.Items.AddRange(new object[] {
            "高",
            "中",
            "低"});
            this.cmbPriority.Location = new System.Drawing.Point(108, 237);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(150, 23);
            this.cmbPriority.TabIndex = 15;
            
            // lblProgress
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(20, 270);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(82, 15);
            this.lblProgress.TabIndex = 16;
            this.lblProgress.Text = "进度：";
            
            // txtProgress
            this.txtProgress.Location = new System.Drawing.Point(108, 267);
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.Size = new System.Drawing.Size(60, 23);
            this.txtProgress.TabIndex = 17;
            this.txtProgress.Text = "0";
            this.txtProgress.TextChanged += new System.EventHandler(this.txtProgress_TextChanged);
            
            // trkProgress
            this.trkProgress.Location = new System.Drawing.Point(174, 267);
            this.trkProgress.Maximum = 100;
            this.trkProgress.Name = "trkProgress";
            this.trkProgress.Size = new System.Drawing.Size(184, 45);
            this.trkProgress.TabIndex = 18;
            this.trkProgress.TickFrequency = 10;
            this.trkProgress.ValueChanged += new System.EventHandler(this.trkProgress_ValueChanged);
            
            // lblNotes
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(20, 310);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(82, 15);
            this.lblNotes.TabIndex = 19;
            this.lblNotes.Text = "备注：";
            
            // txtNotes
            this.txtNotes.Location = new System.Drawing.Point(108, 307);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(650, 70);
            this.txtNotes.TabIndex = 20;
            
            // panelDetailButtons
            this.panelDetailButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetailButtons.Location = new System.Drawing.Point(12, 408);
            this.panelDetailButtons.Name = "panelDetailButtons";
            this.panelDetailButtons.Size = new System.Drawing.Size(776, 30);
            this.panelDetailButtons.TabIndex = 1;
            this.panelDetailButtons.Controls.Add(this.btnSave);
            this.panelDetailButtons.Controls.Add(this.btnCancel);
            
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(300, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(400, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // FrmProductionPlan
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelList);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelDetail);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmProductionPlan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生产计划管理";
            this.Load += new System.EventHandler(this.FrmProductionPlan_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlan)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkProgress)).EndInit();
            this.panelDetailButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.DataGridView dgvPlan;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblFilterDateRange;
        private System.Windows.Forms.DateTimePicker dtpFilterStartDate;
        private System.Windows.Forms.DateTimePicker dtpFilterEndDate;
        private System.Windows.Forms.Label lblFilterSeparator;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TrackBar trkProgress;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ComboBox cmbPriority;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtResponsible;
        private System.Windows.Forms.Label lblResponsible;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.TextBox txtPlanQuantity;
        private System.Windows.Forms.Label lblPlanQuantity;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtPlanId;
        private System.Windows.Forms.Label lblPlanId;
        private System.Windows.Forms.Panel panelDetailButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
} 