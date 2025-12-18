namespace MDMUI
{
    partial class FrmProcessDefinition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProcessDefinition));
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.cboProductType = new System.Windows.Forms.ComboBox();
            this.lblStationTypeLabel = new System.Windows.Forms.Label();
            this.cboActive = new System.Windows.Forms.ComboBox();
            this.lblActive = new System.Windows.Forms.Label();
            this.cboReleaseStatus = new System.Windows.Forms.ComboBox();
            this.lblReleaseStatus = new System.Windows.Forms.Label();
            this.lblProductType = new System.Windows.Forms.Label();
            this.splitContainerContent = new System.Windows.Forms.SplitContainer();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.lblPackage = new System.Windows.Forms.Label();
            this.dgvPackage = new System.Windows.Forms.DataGridView();
            this.lblProcessCount = new System.Windows.Forms.Label();
            this.dgvProcess = new System.Windows.Forms.DataGridView();
            this.lblRouteCount = new System.Windows.Forms.Label();
            this.dgvRoute = new System.Windows.Forms.DataGridView();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProperties = new System.Windows.Forms.Label();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.cboDetailedType = new System.Windows.Forms.ComboBox();
            this.lblDetailedType = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerContent)).BeginInit();
            this.splitContainerContent.Panel1.SuspendLayout();
            this.splitContainerContent.Panel2.SuspendLayout();
            this.splitContainerContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).BeginInit();
            this.panelProperties.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.SteelBlue;
            this.panelTop.Controls.Add(this.btnClose);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 35);
            this.panelTop.TabIndex = 0;
            
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1165, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(200, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "工艺包规范";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelFilter.Controls.Add(this.cboDetailedType);
            this.panelFilter.Controls.Add(this.lblDetailedType);
            this.panelFilter.Controls.Add(this.toolStrip1);
            this.panelFilter.Controls.Add(this.btnSearch);
            this.panelFilter.Controls.Add(this.txtProductCode);
            this.panelFilter.Controls.Add(this.lblProductCode);
            this.panelFilter.Controls.Add(this.cboProductType);
            this.panelFilter.Controls.Add(this.lblStationTypeLabel);
            this.panelFilter.Controls.Add(this.cboActive);
            this.panelFilter.Controls.Add(this.lblActive);
            this.panelFilter.Controls.Add(this.cboReleaseStatus);
            this.panelFilter.Controls.Add(this.lblReleaseStatus);
            this.panelFilter.Controls.Add(this.lblProductType);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 35);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(1200, 80);
            this.panelFilter.TabIndex = 1;
            
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(875, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(364, 65);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(170, 20);
            this.txtProductCode.TabIndex = 8;
            
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Location = new System.Drawing.Point(255, 69);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(73, 13);
            this.lblProductCode.TabIndex = 7;
            this.lblProductCode.Text = "产品编号";
            
            // 
            // cboProductType
            // 
            this.cboProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProductType.FormattingEnabled = true;
            this.cboProductType.Location = new System.Drawing.Point(125, 40);
            this.cboProductType.Name = "cboProductType";
            this.cboProductType.Size = new System.Drawing.Size(121, 21);
            this.cboProductType.TabIndex = 2;
            
            // 
            // lblStationTypeLabel
            // 
            this.lblStationTypeLabel.AutoSize = true;
            this.lblStationTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStationTypeLabel.Location = new System.Drawing.Point(18, 14);
            this.lblStationTypeLabel.Name = "lblStationTypeLabel";
            this.lblStationTypeLabel.Size = new System.Drawing.Size(11, 13);
            this.lblStationTypeLabel.TabIndex = 6;
            this.lblStationTypeLabel.Text = "*";
            
            // 
            // cboActive
            // 
            this.cboActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboActive.FormattingEnabled = true;
            this.cboActive.Location = new System.Drawing.Point(605, 40);
            this.cboActive.Name = "cboActive";
            this.cboActive.Size = new System.Drawing.Size(121, 21);
            this.cboActive.TabIndex = 4;
            
            // 
            // lblActive
            // 
            this.lblActive.AutoSize = true;
            this.lblActive.Location = new System.Drawing.Point(525, 44);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(61, 13);
            this.lblActive.TabIndex = 5;
            this.lblActive.Text = "是否激活";
            
            // 
            // cboReleaseStatus
            // 
            this.cboReleaseStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReleaseStatus.FormattingEnabled = true;
            this.cboReleaseStatus.Location = new System.Drawing.Point(364, 40);
            this.cboReleaseStatus.Name = "cboReleaseStatus";
            this.cboReleaseStatus.Size = new System.Drawing.Size(121, 21);
            this.cboReleaseStatus.TabIndex = 3;
            
            // 
            // lblReleaseStatus
            // 
            this.lblReleaseStatus.AutoSize = true;
            this.lblReleaseStatus.Location = new System.Drawing.Point(255, 44);
            this.lblReleaseStatus.Name = "lblReleaseStatus";
            this.lblReleaseStatus.Size = new System.Drawing.Size(73, 13);
            this.lblReleaseStatus.TabIndex = 3;
            this.lblReleaseStatus.Text = "发行状态";
            
            // 
            // lblProductType
            // 
            this.lblProductType.AutoSize = true;
            this.lblProductType.Location = new System.Drawing.Point(35, 44);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(73, 13);
            this.lblProductType.TabIndex = 1;
            this.lblProductType.Text = "产品类型";
            
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSearch,
            this.toolStripButtonExport,
            this.toolStripButtonPrint,
            this.toolStripButtonHelp,
            this.toolStripButtonRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(964, 40);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(184, 25);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonSearch.Text = "搜索";
            
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonExport.Text = "导出";
            
            // 
            // toolStripButtonPrint
            // 
            this.toolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPrint.Name = "toolStripButtonPrint";
            this.toolStripButtonPrint.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonPrint.Text = "打印";
            
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonHelp.Text = "帮助";
            
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(36, 22);
            this.toolStripButtonRefresh.Text = "刷新";
            
            // 
            // splitContainerContent
            // 
            this.splitContainerContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerContent.Location = new System.Drawing.Point(0, 0);
            this.splitContainerContent.Name = "splitContainerContent";
            // 
            // splitContainerContent.Panel1
            // 
            this.splitContainerContent.Panel1.Controls.Add(this.splitContainerMain);
            // 
            // splitContainerContent.Panel2
            // 
            this.splitContainerContent.Panel2.Controls.Add(this.panelProperties);
            this.splitContainerContent.Size = new System.Drawing.Size(1200, 585);
            this.splitContainerContent.SplitterDistance = 800;
            this.splitContainerContent.TabIndex = 2;
            
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.dgvRoute);
            this.splitContainerMain.Panel2.Controls.Add(this.lblRouteCount);
            this.splitContainerMain.Size = new System.Drawing.Size(900, 585);
            this.splitContainerMain.SplitterDistance = 280;
            this.splitContainerMain.TabIndex = 2;
            
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.dgvPackage);
            this.splitContainerLeft.Panel1.Controls.Add(this.lblPackage);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.dgvProcess);
            this.splitContainerLeft.Panel2.Controls.Add(this.lblProcessCount);
            this.splitContainerLeft.Size = new System.Drawing.Size(900, 280);
            this.splitContainerLeft.SplitterDistance = 450;
            this.splitContainerLeft.TabIndex = 0;
            
            // 
            // lblPackage
            // 
            this.lblPackage.BackColor = System.Drawing.Color.LightGray;
            this.lblPackage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPackage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPackage.Location = new System.Drawing.Point(0, 0);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblPackage.Size = new System.Drawing.Size(446, 30);
            this.lblPackage.TabIndex = 0;
            this.lblPackage.Text = "工艺包";
            this.lblPackage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // dgvPackage
            // 
            this.dgvPackage.AllowUserToAddRows = false;
            this.dgvPackage.AllowUserToDeleteRows = false;
            this.dgvPackage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPackage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPackage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackage.Location = new System.Drawing.Point(0, 30);
            this.dgvPackage.MultiSelect = false;
            this.dgvPackage.Name = "dgvPackage";
            this.dgvPackage.ReadOnly = true;
            this.dgvPackage.RowTemplate.Height = 25;
            this.dgvPackage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPackage.Size = new System.Drawing.Size(446, 246);
            this.dgvPackage.TabIndex = 1;
            
            // 
            // lblProcessCount
            // 
            this.lblProcessCount.BackColor = System.Drawing.Color.LightGray;
            this.lblProcessCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProcessCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessCount.Location = new System.Drawing.Point(0, 0);
            this.lblProcessCount.Name = "lblProcessCount";
            this.lblProcessCount.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblProcessCount.Size = new System.Drawing.Size(442, 30);
            this.lblProcessCount.TabIndex = 0;
            this.lblProcessCount.Text = "工艺流程绑定工艺包";
            this.lblProcessCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // dgvProcess
            // 
            this.dgvProcess.AllowUserToAddRows = false;
            this.dgvProcess.AllowUserToDeleteRows = false;
            this.dgvProcess.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProcess.Location = new System.Drawing.Point(0, 30);
            this.dgvProcess.MultiSelect = false;
            this.dgvProcess.Name = "dgvProcess";
            this.dgvProcess.ReadOnly = true;
            this.dgvProcess.RowTemplate.Height = 25;
            this.dgvProcess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProcess.Size = new System.Drawing.Size(442, 246);
            this.dgvProcess.TabIndex = 1;
            
            // 
            // lblRouteCount
            // 
            this.lblRouteCount.BackColor = System.Drawing.Color.LightGray;
            this.lblRouteCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRouteCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRouteCount.Location = new System.Drawing.Point(0, 0);
            this.lblRouteCount.Name = "lblRouteCount";
            this.lblRouteCount.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblRouteCount.Size = new System.Drawing.Size(896, 30);
            this.lblRouteCount.TabIndex = 0;
            this.lblRouteCount.Text = "工艺路线";
            this.lblRouteCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // dgvRoute
            // 
            this.dgvRoute.AllowUserToAddRows = false;
            this.dgvRoute.AllowUserToDeleteRows = false;
            this.dgvRoute.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRoute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoute.Location = new System.Drawing.Point(0, 30);
            this.dgvRoute.MultiSelect = false;
            this.dgvRoute.Name = "dgvRoute";
            this.dgvRoute.ReadOnly = true;
            this.dgvRoute.RowTemplate.Height = 25;
            this.dgvRoute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoute.Size = new System.Drawing.Size(896, 267);
            this.dgvRoute.TabIndex = 1;
            
            // 
            // cboDetailedType
            // 
            this.cboDetailedType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDetailedType.FormattingEnabled = true;
            this.cboDetailedType.Location = new System.Drawing.Point(125, 67);
            this.cboDetailedType.Name = "cboDetailedType";
            this.cboDetailedType.Size = new System.Drawing.Size(121, 21);
            this.cboDetailedType.TabIndex = 12;
            
            // 
            // lblDetailedType
            // 
            this.lblDetailedType.AutoSize = true;
            this.lblDetailedType.Location = new System.Drawing.Point(35, 70);
            this.lblDetailedType.Name = "lblDetailedType";
            this.lblDetailedType.Size = new System.Drawing.Size(73, 13);
            this.lblDetailedType.TabIndex = 11;
            this.lblDetailedType.Text = "详细类型";
            
            // 
            // panelProperties
            // 
            this.panelProperties.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelProperties.Controls.Add(this.label2);
            this.panelProperties.Controls.Add(this.label1);
            this.panelProperties.Controls.Add(this.propertyGrid);
            this.panelProperties.Controls.Add(this.lblProperties);
            this.panelProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProperties.Location = new System.Drawing.Point(0, 0);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(396, 585);
            this.panelProperties.TabIndex = 0;
            
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 547);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(392, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "版本";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightGray;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 30);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(392, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "属性: 工艺规范产品";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // lblProperties
            // 
            this.lblProperties.BackColor = System.Drawing.Color.LightGray;
            this.lblProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProperties.Location = new System.Drawing.Point(0, 0);
            this.lblProperties.Name = "lblProperties";
            this.lblProperties.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblProperties.Size = new System.Drawing.Size(392, 30);
            this.lblProperties.TabIndex = 0;
            this.lblProperties.Text = "工艺规范配件";
            this.lblProperties.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid.Location = new System.Drawing.Point(7, 79);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid.Size = new System.Drawing.Size(380, 460);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            
            // 
            // FrmProcessDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.splitContainerContent);
            this.Controls.Add(this.panelFilter);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmProcessDefinition";
            this.Text = "工艺包规范";
            this.panelTop.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.splitContainerContent.Panel1.ResumeLayout(false);
            this.splitContainerContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerContent)).EndInit();
            this.splitContainerContent.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).EndInit();
            this.panelProperties.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.ComboBox cboProductType;
        private System.Windows.Forms.Label lblStationTypeLabel;
        private System.Windows.Forms.ComboBox cboActive;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.ComboBox cboReleaseStatus;
        private System.Windows.Forms.Label lblReleaseStatus;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.DataGridView dgvPackage;
        private System.Windows.Forms.Label lblPackage;
        private System.Windows.Forms.DataGridView dgvProcess;
        private System.Windows.Forms.Label lblProcessCount;
        private System.Windows.Forms.DataGridView dgvRoute;
        private System.Windows.Forms.Label lblRouteCount;
        
        // 添加新控件
        private System.Windows.Forms.SplitContainer splitContainerContent;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.Label lblProperties;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonHelp;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ComboBox cboDetailedType;
        private System.Windows.Forms.Label lblDetailedType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
} 