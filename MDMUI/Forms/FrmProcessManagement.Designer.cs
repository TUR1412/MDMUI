namespace MDMUI
{
    partial class FrmProcessManagement
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
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.packagePanel = new System.Windows.Forms.Panel();
            this.middleContainer = new System.Windows.Forms.TableLayoutPanel();
            this.processPanel = new System.Windows.Forms.Panel();
            this.routePanel = new System.Windows.Forms.Panel();
            this.detailPanel = new System.Windows.Forms.Panel();
            this.lblPackage = new System.Windows.Forms.Label();
            this.lblProcess = new System.Windows.Forms.Label();
            this.lblRoute = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.dgvPackage = new System.Windows.Forms.DataGridView();
            this.dgvProcess = new System.Windows.Forms.DataGridView();
            this.dgvRoute = new System.Windows.Forms.DataGridView();
            this.detailContainer = new System.Windows.Forms.TableLayoutPanel();
            this.detailContent = new System.Windows.Forms.RichTextBox();
            this.packageShadowPanel = new System.Windows.Forms.Panel();
            this.middleShadowPanel = new System.Windows.Forms.Panel();
            this.processShadowPanel = new System.Windows.Forms.Panel();
            this.routeShadowPanel = new System.Windows.Forms.Panel();
            this.detailShadowPanel = new System.Windows.Forms.Panel();
            this.btnUpdateOpSeq = new System.Windows.Forms.Button();
            
            this.mainContainer.SuspendLayout();
            this.packageShadowPanel.SuspendLayout();
            this.packagePanel.SuspendLayout();
            this.middleShadowPanel.SuspendLayout();
            this.middleContainer.SuspendLayout();
            this.processShadowPanel.SuspendLayout();
            this.processPanel.SuspendLayout();
            this.routeShadowPanel.SuspendLayout();
            this.routePanel.SuspendLayout();
            this.detailShadowPanel.SuspendLayout();
            this.detailPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).BeginInit();
            this.detailContainer.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // mainContainer - 主容器改为3列布局，优化宽度分配
            // 
            this.mainContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.mainContainer.ColumnCount = 3;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 520F));
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.mainContainer.Controls.Add(this.packageShadowPanel, 0, 0);
            this.mainContainer.Controls.Add(this.middleShadowPanel, 1, 0);
            this.mainContainer.Controls.Add(this.detailShadowPanel, 2, 0);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 0);
            this.mainContainer.Margin = new System.Windows.Forms.Padding(0);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.Padding = new System.Windows.Forms.Padding(15, 5, 15, 20);
            this.mainContainer.RowCount = 1;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Size = new System.Drawing.Size(1400, 800);
            this.mainContainer.TabIndex = 0;
            
            // 
            // packageShadowPanel - 工艺包阴影容器
            // 
            this.packageShadowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.packageShadowPanel.Controls.Add(this.packagePanel);
            this.packageShadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packageShadowPanel.Location = new System.Drawing.Point(18, 23);
            this.packageShadowPanel.Margin = new System.Windows.Forms.Padding(3, 3, 8, 3);
            this.packageShadowPanel.Name = "packageShadowPanel";
            this.packageShadowPanel.Padding = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.packageShadowPanel.Size = new System.Drawing.Size(389, 754);
            this.packageShadowPanel.TabIndex = 0;
            
            // 
            // packagePanel - 工艺包面板
            // 
            this.packagePanel.BackColor = System.Drawing.Color.White;
            this.packagePanel.Controls.Add(this.dgvPackage);
            this.packagePanel.Controls.Add(this.lblPackage);
            this.packagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packagePanel.Location = new System.Drawing.Point(3, 3);
            this.packagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.packagePanel.Name = "packagePanel";
            this.packagePanel.Size = new System.Drawing.Size(380, 745);
            this.packagePanel.TabIndex = 0;
            
            // 
            // middleShadowPanel - 中间区域阴影容器
            // 
            this.middleShadowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.middleShadowPanel.Controls.Add(this.middleContainer);
            this.middleShadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middleShadowPanel.Location = new System.Drawing.Point(426, 23);
            this.middleShadowPanel.Margin = new System.Windows.Forms.Padding(3);
            this.middleShadowPanel.Name = "middleShadowPanel";
            this.middleShadowPanel.Padding = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.middleShadowPanel.Size = new System.Drawing.Size(576, 754);
            this.middleShadowPanel.TabIndex = 1;
            
            // 
            // middleContainer - 中间区域容器（上下分割）
            // 
            this.middleContainer.BackColor = System.Drawing.Color.Transparent;
            this.middleContainer.ColumnCount = 1;
            this.middleContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.middleContainer.Controls.Add(this.processShadowPanel, 0, 0);
            this.middleContainer.Controls.Add(this.routeShadowPanel, 0, 1);
            this.middleContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middleContainer.Location = new System.Drawing.Point(3, 3);
            this.middleContainer.Margin = new System.Windows.Forms.Padding(0);
            this.middleContainer.Name = "middleContainer";
            this.middleContainer.RowCount = 2;
            this.middleContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.middleContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.middleContainer.Size = new System.Drawing.Size(567, 745);
            this.middleContainer.TabIndex = 0;
            
            // 
            // processShadowPanel - 工艺流程阴影容器
            // 
            this.processShadowPanel.BackColor = System.Drawing.Color.Transparent;
            this.processShadowPanel.Controls.Add(this.processPanel);
            this.processShadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processShadowPanel.Location = new System.Drawing.Point(3, 3);
            this.processShadowPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.processShadowPanel.Name = "processShadowPanel";
            this.processShadowPanel.Padding = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.processShadowPanel.Size = new System.Drawing.Size(561, 324);
            this.processShadowPanel.TabIndex = 0;
            
            // 
            // processPanel - 工艺流程面板
            // 
            this.processPanel.BackColor = System.Drawing.Color.White;
            this.processPanel.Controls.Add(this.dgvProcess);
            this.processPanel.Controls.Add(this.lblProcess);
            this.processPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processPanel.Location = new System.Drawing.Point(3, 3);
            this.processPanel.Margin = new System.Windows.Forms.Padding(0);
            this.processPanel.Name = "processPanel";
            this.processPanel.Size = new System.Drawing.Size(552, 315);
            this.processPanel.TabIndex = 0;
            
            // 
            // routeShadowPanel - 工艺路线阴影容器
            // 
            this.routeShadowPanel.BackColor = System.Drawing.Color.Transparent;
            this.routeShadowPanel.Controls.Add(this.routePanel);
            this.routeShadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routeShadowPanel.Location = new System.Drawing.Point(3, 338);
            this.routeShadowPanel.Margin = new System.Windows.Forms.Padding(3);
            this.routeShadowPanel.Name = "routeShadowPanel";
            this.routeShadowPanel.Padding = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.routeShadowPanel.Size = new System.Drawing.Size(561, 404);
            this.routeShadowPanel.TabIndex = 1;
            
            // 
            // routePanel - 工艺路线面板
            // 
            this.routePanel.BackColor = System.Drawing.Color.White;
            this.routePanel.Controls.Add(this.dgvRoute);
            this.routePanel.Controls.Add(this.lblRoute);
            this.routePanel.Controls.Add(this.btnUpdateOpSeq);
            this.routePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routePanel.Location = new System.Drawing.Point(3, 3);
            this.routePanel.Margin = new System.Windows.Forms.Padding(0);
            this.routePanel.Name = "routePanel";
            this.routePanel.Size = new System.Drawing.Size(552, 395);
            this.routePanel.TabIndex = 0;
            
            // 
            // detailShadowPanel - 详细信息阴影容器
            // 
            this.detailShadowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.detailShadowPanel.Controls.Add(this.detailPanel);
            this.detailShadowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailShadowPanel.Location = new System.Drawing.Point(1011, 23);
            this.detailShadowPanel.Margin = new System.Windows.Forms.Padding(3);
            this.detailShadowPanel.Name = "detailShadowPanel";
            this.detailShadowPanel.Padding = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.detailShadowPanel.Size = new System.Drawing.Size(371, 754);
            this.detailShadowPanel.TabIndex = 2;
            
            // 
            // detailPanel - 详细信息面板
            // 
            this.detailPanel.BackColor = System.Drawing.Color.White;
            this.detailPanel.Controls.Add(this.detailContainer);
            this.detailPanel.Controls.Add(this.lblDetail);
            this.detailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailPanel.Location = new System.Drawing.Point(3, 3);
            this.detailPanel.Margin = new System.Windows.Forms.Padding(0);
            this.detailPanel.Name = "detailPanel";
            this.detailPanel.Size = new System.Drawing.Size(362, 745);
            this.detailPanel.TabIndex = 0;
            
            // 
            // lblPackage - 工艺包标题
            // 
            this.lblPackage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblPackage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPackage.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPackage.ForeColor = System.Drawing.Color.White;
            this.lblPackage.Location = new System.Drawing.Point(0, 0);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblPackage.Size = new System.Drawing.Size(380, 40);
            this.lblPackage.TabIndex = 0;
            this.lblPackage.Text = "📦 工艺包产品 (5)";
            this.lblPackage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // lblProcess - 工艺流程标题
            // 
            this.lblProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lblProcess.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProcess.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProcess.ForeColor = System.Drawing.Color.White;
            this.lblProcess.Location = new System.Drawing.Point(0, 0);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblProcess.Size = new System.Drawing.Size(552, 40);
            this.lblProcess.TabIndex = 0;
            this.lblProcess.Text = "⚙️ 工艺流程绑定工艺包 (3)";
            this.lblProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // lblRoute - 工艺路线标题
            // 
            this.lblRoute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblRoute.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRoute.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRoute.ForeColor = System.Drawing.Color.White;
            this.lblRoute.Location = new System.Drawing.Point(0, 0);
            this.lblRoute.Name = "lblRoute";
            this.lblRoute.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblRoute.Size = new System.Drawing.Size(552, 40);
            this.lblRoute.TabIndex = 0;
            this.lblRoute.Text = "🛣️ 工艺路线 (3)";
            this.lblRoute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // lblDetail - 详细信息标题
            // 
            this.lblDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.lblDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetail.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDetail.ForeColor = System.Drawing.Color.White;
            this.lblDetail.Location = new System.Drawing.Point(0, 0);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblDetail.Size = new System.Drawing.Size(362, 40);
            this.lblDetail.TabIndex = 0;
            this.lblDetail.Text = "📋 详细信息";
            this.lblDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // dgvPackage - 工艺包数据表格
            // 
            this.dgvPackage.AllowUserToAddRows = false;
            this.dgvPackage.AllowUserToDeleteRows = false;
            this.dgvPackage.AllowUserToResizeRows = false;
            this.dgvPackage.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.dgvPackage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPackage.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPackage.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPackage.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvPackage.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.dgvPackage.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvPackage.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvPackage.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.dgvPackage.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.dgvPackage.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvPackage.ColumnHeadersHeight = 35;
            this.dgvPackage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPackage.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvPackage.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dgvPackage.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.dgvPackage.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.dgvPackage.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(214)))), ((int)(((byte)(241)))));
            this.dgvPackage.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvPackage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackage.EnableHeadersVisualStyles = false;
            this.dgvPackage.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(234)))));
            this.dgvPackage.Location = new System.Drawing.Point(0, 40);
            this.dgvPackage.MultiSelect = false;
            this.dgvPackage.Name = "dgvPackage";
            this.dgvPackage.ReadOnly = true;
            this.dgvPackage.RowHeadersVisible = false;
            this.dgvPackage.RowTemplate.Height = 32;
            this.dgvPackage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPackage.Size = new System.Drawing.Size(380, 705);
            this.dgvPackage.TabIndex = 1;
            this.dgvPackage.SelectionChanged += new System.EventHandler(this.DgvPackage_SelectionChanged);
            
            // 
            // dgvProcess - 工艺流程数据表格
            // 
            this.dgvProcess.AllowUserToAddRows = false;
            this.dgvProcess.AllowUserToDeleteRows = false;
            this.dgvProcess.AllowUserToResizeRows = false;
            this.dgvProcess.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.dgvProcess.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProcess.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProcess.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvProcess.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvProcess.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.dgvProcess.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvProcess.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvProcess.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.dgvProcess.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.dgvProcess.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvProcess.ColumnHeadersHeight = 35;
            this.dgvProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProcess.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvProcess.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dgvProcess.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.dgvProcess.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.dgvProcess.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(214)))), ((int)(((byte)(241)))));
            this.dgvProcess.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProcess.EnableHeadersVisualStyles = false;
            this.dgvProcess.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(234)))));
            this.dgvProcess.Location = new System.Drawing.Point(0, 40);
            this.dgvProcess.MultiSelect = false;
            this.dgvProcess.Name = "dgvProcess";
            this.dgvProcess.ReadOnly = true;
            this.dgvProcess.RowHeadersVisible = false;
            this.dgvProcess.RowTemplate.Height = 32;
            this.dgvProcess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProcess.Size = new System.Drawing.Size(552, 315);
            this.dgvProcess.TabIndex = 1;
            this.dgvProcess.SelectionChanged += new System.EventHandler(this.DgvProcess_SelectionChanged);
            
            // 
            // dgvRoute - 工艺路线数据表格
            // 
            this.dgvRoute.AllowUserToAddRows = false;
            this.dgvRoute.AllowUserToDeleteRows = false;
            this.dgvRoute.AllowUserToResizeRows = false;
            this.dgvRoute.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.dgvRoute.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRoute.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRoute.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRoute.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvRoute.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.dgvRoute.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvRoute.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvRoute.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.dgvRoute.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.dgvRoute.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvRoute.ColumnHeadersHeight = 35;
            this.dgvRoute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRoute.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvRoute.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dgvRoute.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.dgvRoute.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.dgvRoute.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(214)))), ((int)(((byte)(241)))));
            this.dgvRoute.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.dgvRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoute.EnableHeadersVisualStyles = false;
            this.dgvRoute.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(228)))), ((int)(((byte)(234)))));
            this.dgvRoute.Location = new System.Drawing.Point(0, 40);
            this.dgvRoute.MultiSelect = false;
            this.dgvRoute.Name = "dgvRoute";
            this.dgvRoute.ReadOnly = true;
            this.dgvRoute.RowHeadersVisible = false;
            this.dgvRoute.RowTemplate.Height = 32;
            this.dgvRoute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoute.Size = new System.Drawing.Size(552, 355);
            this.dgvRoute.TabIndex = 1;
            
            // 
            // detailContainer - 详细信息容器
            // 
            this.detailContainer.BackColor = System.Drawing.Color.Transparent;
            this.detailContainer.ColumnCount = 1;
            this.detailContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.detailContainer.Controls.Add(this.detailContent, 0, 0);
            this.detailContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailContainer.Location = new System.Drawing.Point(0, 40);
            this.detailContainer.Name = "detailContainer";
            this.detailContainer.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.detailContainer.RowCount = 1;
            this.detailContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.detailContainer.Size = new System.Drawing.Size(362, 705);
            this.detailContainer.TabIndex = 1;
            
            // 
            // detailContent - 详细信息内容（直接放置，无GroupBox）
            // 
            this.detailContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(251)))), ((int)(((byte)(253)))));
            this.detailContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detailContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailContent.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.detailContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.detailContent.Location = new System.Drawing.Point(15, 15);
            this.detailContent.Margin = new System.Windows.Forms.Padding(3);
            this.detailContent.Name = "detailContent";
            this.detailContent.Padding = new System.Windows.Forms.Padding(10);
            this.detailContent.ReadOnly = true;
            this.detailContent.Size = new System.Drawing.Size(332, 675);
            this.detailContent.TabIndex = 0;
            this.detailContent.Text = "🎯 请选择一个项目查看详细信息";
            
            // 
            // btnUpdateOpSeq - 修改工艺路线按钮
            // 
            this.btnUpdateOpSeq = new System.Windows.Forms.Button();
            this.btnUpdateOpSeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(166)))), ((int)(((byte)(245)))));
            this.btnUpdateOpSeq.FlatAppearance.BorderSize = 0;
            this.btnUpdateOpSeq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateOpSeq.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdateOpSeq.ForeColor = System.Drawing.Color.White;
            this.btnUpdateOpSeq.Name = "btnUpdateOpSeq";
            this.btnUpdateOpSeq.Size = new System.Drawing.Size(120, 30);
            this.btnUpdateOpSeq.TabIndex = 4;
            this.btnUpdateOpSeq.Text = "修改工艺路线";
            this.btnUpdateOpSeq.UseVisualStyleBackColor = false;
            this.btnUpdateOpSeq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateOpSeq.Click += new System.EventHandler(this.btnUpdateOpSeq_Click);
            
            // 
            // FrmProcessManagement - 主窗体
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.mainContainer);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.Name = "FrmProcessManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🏭 工艺管理 - 专业版 v2.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainContainer.ResumeLayout(false);
            this.packageShadowPanel.ResumeLayout(false);
            this.packagePanel.ResumeLayout(false);
            this.middleShadowPanel.ResumeLayout(false);
            this.middleContainer.ResumeLayout(false);
            this.processShadowPanel.ResumeLayout(false);
            this.processPanel.ResumeLayout(false);
            this.routeShadowPanel.ResumeLayout(false);
            this.routePanel.ResumeLayout(false);
            this.detailShadowPanel.ResumeLayout(false);
            this.detailPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).EndInit();
            this.detailContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainContainer;
        private System.Windows.Forms.Panel packageShadowPanel;
        private System.Windows.Forms.Panel packagePanel;
        private System.Windows.Forms.Panel middleShadowPanel;
        private System.Windows.Forms.TableLayoutPanel middleContainer;
        private System.Windows.Forms.Panel processShadowPanel;
        private System.Windows.Forms.Panel processPanel;
        private System.Windows.Forms.Panel routeShadowPanel;
        private System.Windows.Forms.Panel routePanel;
        private System.Windows.Forms.Panel detailShadowPanel;
        private System.Windows.Forms.Panel detailPanel;
        private System.Windows.Forms.DataGridView dgvPackage;
        private System.Windows.Forms.DataGridView dgvProcess;
        private System.Windows.Forms.DataGridView dgvRoute;
        private System.Windows.Forms.Label lblPackage;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.Label lblRoute;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.TableLayoutPanel detailContainer;
        private System.Windows.Forms.RichTextBox detailContent;
        private System.Windows.Forms.Button btnUpdateOpSeq;
    }
} 