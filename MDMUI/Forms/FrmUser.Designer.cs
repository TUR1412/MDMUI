namespace MDMUI
{
    partial class FrmUser
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
            this.components = new System.ComponentModel.Container(); // Initialize components container FIRST
            // Define Colors (Revert to Light Scheme Defaults + Accent)
            System.Drawing.Color backColorControl = System.Drawing.SystemColors.Control;
            System.Drawing.Color backColorWindow = System.Drawing.SystemColors.Window;
            System.Drawing.Color foreColorDefault = System.Drawing.SystemColors.ControlText;
            System.Drawing.Color foreColorGray = System.Drawing.SystemColors.GrayText;
            System.Drawing.Color accentColor = System.Drawing.SystemColors.Highlight; // Use system highlight color
            System.Drawing.Color accentForeColor = System.Drawing.SystemColors.HighlightText;
            System.Drawing.Color hoverColorButton = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(243)))), ((int)(((byte)(255))))); // Light blue hover for buttons
            System.Drawing.Color gridAltRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250))))); // Very light alternating row

            System.Windows.Forms.Padding panelPadding = new System.Windows.Forms.Padding(10);
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle(); // Alternating Rows
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle(); // Header Style
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle(); // Default Cell Style
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnResetPwd = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RealName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RoleNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastLoginTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolPanel = new System.Windows.Forms.Panel();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.pnlUserDetails = new System.Windows.Forms.Panel();
            this.lblDetailHeader = new System.Windows.Forms.Label();
            this.lblDetailId = new System.Windows.Forms.Label();
            this.txtDetailId = new System.Windows.Forms.TextBox();
            this.lblDetailUsername = new System.Windows.Forms.Label();
            this.txtDetailUsername = new System.Windows.Forms.TextBox();
            this.lblDetailRealName = new System.Windows.Forms.Label();
            this.txtDetailRealName = new System.Windows.Forms.TextBox();
            this.lblDetailRoleName = new System.Windows.Forms.Label();
            this.txtDetailRoleName = new System.Windows.Forms.TextBox();
            this.lblDetailLastLogin = new System.Windows.Forms.Label();
            this.txtDetailLastLogin = new System.Windows.Forms.TextBox();
            this.lblDetailPlaceholder = new System.Windows.Forms.Label();
            this.searchDebounceTimer = new System.Windows.Forms.Timer(this.components);
            this.separatorLabel = new System.Windows.Forms.Label(); // Add Separator Label
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.toolPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.pnlUserDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = backColorControl; // Default button color
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatAppearance.MouseDownBackColor = accentColor; // Use highlight for press
            this.btnAdd.FlatAppearance.MouseOverBackColor = hoverColorButton; // Light hover
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = foreColorDefault; // Default text color
            this.btnAdd.Location = new System.Drawing.Point(10, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = backColorControl;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatAppearance.MouseDownBackColor = accentColor;
            this.btnEdit.FlatAppearance.MouseOverBackColor = hoverColorButton;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.ForeColor = foreColorDefault;
            this.btnEdit.Location = new System.Drawing.Point(100, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = backColorControl;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkRed;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192))))); // Light red hover
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.Red; // Red text for delete
            this.btnDelete.Location = new System.Drawing.Point(190, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = backColorControl;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = accentColor;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = hoverColorButton;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = foreColorDefault;
            this.btnRefresh.Location = new System.Drawing.Point(280, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = backColorControl;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = accentColor;
            this.btnSearch.FlatAppearance.MouseOverBackColor = hoverColorButton;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = foreColorDefault;
            this.btnSearch.Location = new System.Drawing.Point(290, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // btnResetPwd
            // 
            this.btnResetPwd.BackColor = backColorControl;
            this.btnResetPwd.FlatAppearance.BorderSize = 0;
            this.btnResetPwd.FlatAppearance.MouseDownBackColor = accentColor;
            this.btnResetPwd.FlatAppearance.MouseOverBackColor = hoverColorButton;
            this.btnResetPwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetPwd.ForeColor = foreColorDefault;
            this.btnResetPwd.Location = new System.Drawing.Point(370, 12);
            this.btnResetPwd.Name = "btnResetPwd";
            this.btnResetPwd.Size = new System.Drawing.Size(98, 30);
            this.btnResetPwd.TabIndex = 4;
            this.btnResetPwd.Text = "重置密码";
            this.btnResetPwd.UseVisualStyleBackColor = false;
            this.btnResetPwd.Click += new System.EventHandler(this.BtnResetPwd_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BackColor = backColorWindow; // Use Window background
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.ForeColor = foreColorDefault;
            this.txtSearch.Location = new System.Drawing.Point(80, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 26);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.ForeColor = foreColorDefault; // Default text color
            this.lblSearch.Location = new System.Drawing.Point(10, 18);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(54, 19);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "搜索：";
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = gridAltRowColor; // Light alternating row
            dataGridViewCellStyle1.ForeColor = foreColorDefault;
            this.dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = backColorWindow; // Window background for grid
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D; // Use 3D border for contrast
            this.dgvUsers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvUsers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single; // Default border
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = backColorControl; // Default header background
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = foreColorDefault; // Default header text color
            dataGridViewCellStyle2.SelectionBackColor = accentColor;
            dataGridViewCellStyle2.SelectionForeColor = accentForeColor;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUsers.ColumnHeadersHeight = 34;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id, this.Username, this.RealName, this.RoleNameColumn, this.LastLoginTime});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = backColorWindow; // Default cell background
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = foreColorDefault; // Default cell text color
            dataGridViewCellStyle3.SelectionBackColor = accentColor; // Selection background
            dataGridViewCellStyle3.SelectionForeColor = accentForeColor; // Selection text color
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.EnableHeadersVisualStyles = false; // Keep this false to allow custom styles
            this.dgvUsers.GridColor = System.Drawing.SystemColors.ControlLight; // Lighter grid lines
            this.dgvUsers.Location = new System.Drawing.Point(0, 112);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.RowTemplate.Height = 28;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(528, 432);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellMouseEnter);
            this.dgvUsers.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellMouseLeave);
            this.dgvUsers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvUsers_CellDoubleClick);
            this.dgvUsers.SelectionChanged += new System.EventHandler(this.dgvUsers_SelectionChanged);
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "用户ID";
            this.Id.MinimumWidth = 8;
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 80;
            // 
            // Username
            // 
            this.Username.DataPropertyName = "Username";
            this.Username.HeaderText = "用户名";
            this.Username.MinimumWidth = 120;
            this.Username.Name = "Username";
            this.Username.ReadOnly = true;
            this.Username.FillWeight = 100;
            // 
            // RealName
            // 
            this.RealName.DataPropertyName = "RealName";
            this.RealName.HeaderText = "真实姓名";
            this.RealName.MinimumWidth = 100;
            this.RealName.Name = "RealName";
            this.RealName.ReadOnly = true;
            this.RealName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RealName.FillWeight = 80;
            // 
            // RoleNameColumn
            // 
            this.RoleNameColumn.DataPropertyName = "RoleName";
            this.RoleNameColumn.HeaderText = "角色";
            this.RoleNameColumn.MinimumWidth = 100;
            this.RoleNameColumn.Name = "RoleNameColumn";
            this.RoleNameColumn.ReadOnly = true;
            this.RoleNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RoleNameColumn.FillWeight = 80;
            // 
            // LastLoginTime
            // 
            this.LastLoginTime.DataPropertyName = "LastLoginTime";
            this.LastLoginTime.HeaderText = "最后登录时间";
            this.LastLoginTime.MinimumWidth = 150;
            this.LastLoginTime.Name = "LastLoginTime";
            this.LastLoginTime.ReadOnly = true;
            this.LastLoginTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LastLoginTime.Width = 180;
            this.LastLoginTime.FillWeight = 90;
            // 
            // toolPanel
            // 
            this.toolPanel.BackColor = backColorControl; // Default panel color
            this.toolPanel.Controls.Add(this.btnAdd);
            this.toolPanel.Controls.Add(this.btnEdit);
            this.toolPanel.Controls.Add(this.btnDelete);
            this.toolPanel.Controls.Add(this.btnRefresh);
            this.toolPanel.Controls.Add(this.btnResetPwd);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolPanel.Location = new System.Drawing.Point(0, 0);
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Padding = panelPadding;
            this.toolPanel.Size = new System.Drawing.Size(778, 55);
            this.toolPanel.TabIndex = 2;
            // 
            // searchPanel
            // 
            this.searchPanel.BackColor = backColorControl; // Default panel color
            this.searchPanel.Controls.Add(this.lblSearch);
            this.searchPanel.Controls.Add(this.txtSearch);
            this.searchPanel.Controls.Add(this.btnSearch);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(0, 57);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = panelPadding;
            this.searchPanel.Size = new System.Drawing.Size(778, 55);
            this.searchPanel.TabIndex = 1;
            // 
            // pnlUserDetails
            // 
            this.pnlUserDetails.BackColor = backColorControl; // Use Control color for card distinction
            this.pnlUserDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUserDetails.Controls.Add(this.lblDetailPlaceholder);
            this.pnlUserDetails.Controls.Add(this.lblDetailHeader);
            this.pnlUserDetails.Controls.Add(this.txtDetailLastLogin);
            this.pnlUserDetails.Controls.Add(this.lblDetailLastLogin);
            this.pnlUserDetails.Controls.Add(this.txtDetailRoleName);
            this.pnlUserDetails.Controls.Add(this.lblDetailRoleName);
            this.pnlUserDetails.Controls.Add(this.txtDetailRealName);
            this.pnlUserDetails.Controls.Add(this.lblDetailRealName);
            this.pnlUserDetails.Controls.Add(this.txtDetailUsername);
            this.pnlUserDetails.Controls.Add(this.lblDetailUsername);
            this.pnlUserDetails.Controls.Add(this.txtDetailId);
            this.pnlUserDetails.Controls.Add(this.lblDetailId);
            this.pnlUserDetails.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlUserDetails.ForeColor = foreColorDefault; // Default text color
            this.pnlUserDetails.Location = new System.Drawing.Point(528, 112);
            this.pnlUserDetails.Name = "pnlUserDetails";
            this.pnlUserDetails.Padding = new System.Windows.Forms.Padding(15);
            this.pnlUserDetails.Size = new System.Drawing.Size(250, 432);
            this.pnlUserDetails.TabIndex = 3;
            // 
            // lblDetailHeader
            // 
            this.lblDetailHeader.AutoSize = true;
            this.lblDetailHeader.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetailHeader.ForeColor = foreColorDefault;
            this.lblDetailHeader.Location = new System.Drawing.Point(15, 15);
            this.lblDetailHeader.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.lblDetailHeader.Name = "lblDetailHeader";
            this.lblDetailHeader.Size = new System.Drawing.Size(95, 24);
            this.lblDetailHeader.TabIndex = 0;
            this.lblDetailHeader.Text = "用户详情";
            // 
            // lblDetailId
            // 
            this.lblDetailId.AutoSize = false;
            this.lblDetailId.Location = new System.Drawing.Point(15, 58);
            this.lblDetailId.Name = "lblDetailId";
            this.lblDetailId.Size = new System.Drawing.Size(80, 24);
            this.lblDetailId.TabIndex = 1;
            this.lblDetailId.Text = "ID:";
            this.lblDetailId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDetailId
            // 
            this.txtDetailId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailId.BackColor = backColorControl; // Match panel background
            this.txtDetailId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailId.ForeColor = foreColorDefault;
            this.txtDetailId.Location = new System.Drawing.Point(100, 55);
            this.txtDetailId.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtDetailId.Name = "txtDetailId";
            this.txtDetailId.ReadOnly = true;
            this.txtDetailId.Size = new System.Drawing.Size(130, 26);
            this.txtDetailId.TabIndex = 2;
            // 
            // lblDetailUsername
            // 
            this.lblDetailUsername.AutoSize = false;
            this.lblDetailUsername.Location = new System.Drawing.Point(15, 98);
            this.lblDetailUsername.Name = "lblDetailUsername";
            this.lblDetailUsername.Size = new System.Drawing.Size(80, 24);
            this.lblDetailUsername.TabIndex = 3;
            this.lblDetailUsername.Text = "用户名:";
            this.lblDetailUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDetailUsername
            // 
            this.txtDetailUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailUsername.BackColor = backColorControl;
            this.txtDetailUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailUsername.ForeColor = foreColorDefault;
            this.txtDetailUsername.Location = new System.Drawing.Point(100, 95);
            this.txtDetailUsername.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtDetailUsername.Name = "txtDetailUsername";
            this.txtDetailUsername.ReadOnly = true;
            this.txtDetailUsername.Size = new System.Drawing.Size(130, 26);
            this.txtDetailUsername.TabIndex = 4;
            // 
            // lblDetailRealName
            // 
            this.lblDetailRealName.AutoSize = false;
            this.lblDetailRealName.Location = new System.Drawing.Point(15, 138);
            this.lblDetailRealName.Name = "lblDetailRealName";
            this.lblDetailRealName.Size = new System.Drawing.Size(80, 24);
            this.lblDetailRealName.TabIndex = 5;
            this.lblDetailRealName.Text = "真实姓名:";
            this.lblDetailRealName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDetailRealName
            // 
            this.txtDetailRealName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailRealName.BackColor = backColorControl;
            this.txtDetailRealName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailRealName.ForeColor = foreColorDefault;
            this.txtDetailRealName.Location = new System.Drawing.Point(100, 135);
            this.txtDetailRealName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtDetailRealName.Name = "txtDetailRealName";
            this.txtDetailRealName.ReadOnly = true;
            this.txtDetailRealName.Size = new System.Drawing.Size(130, 26);
            this.txtDetailRealName.TabIndex = 6;
            // 
            // lblDetailRoleName
            // 
            this.lblDetailRoleName.AutoSize = false;
            this.lblDetailRoleName.Location = new System.Drawing.Point(15, 178);
            this.lblDetailRoleName.Name = "lblDetailRoleName";
            this.lblDetailRoleName.Size = new System.Drawing.Size(80, 24);
            this.lblDetailRoleName.TabIndex = 7;
            this.lblDetailRoleName.Text = "角色:";
            this.lblDetailRoleName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDetailRoleName
            // 
            this.txtDetailRoleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailRoleName.BackColor = backColorControl;
            this.txtDetailRoleName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailRoleName.ForeColor = foreColorDefault;
            this.txtDetailRoleName.Location = new System.Drawing.Point(100, 175);
            this.txtDetailRoleName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtDetailRoleName.Name = "txtDetailRoleName";
            this.txtDetailRoleName.ReadOnly = true;
            this.txtDetailRoleName.Size = new System.Drawing.Size(130, 26);
            this.txtDetailRoleName.TabIndex = 8;
            // 
            // lblDetailLastLogin
            // 
            this.lblDetailLastLogin.AutoSize = false;
            this.lblDetailLastLogin.Location = new System.Drawing.Point(15, 218);
            this.lblDetailLastLogin.Name = "lblDetailLastLogin";
            this.lblDetailLastLogin.Size = new System.Drawing.Size(80, 24);
            this.lblDetailLastLogin.TabIndex = 9;
            this.lblDetailLastLogin.Text = "上次登录:";
            this.lblDetailLastLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDetailLastLogin
            // 
            this.txtDetailLastLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailLastLogin.BackColor = backColorControl;
            this.txtDetailLastLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetailLastLogin.ForeColor = foreColorDefault;
            this.txtDetailLastLogin.Location = new System.Drawing.Point(100, 215);
            this.txtDetailLastLogin.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtDetailLastLogin.Name = "txtDetailLastLogin";
            this.txtDetailLastLogin.ReadOnly = true;
            this.txtDetailLastLogin.Size = new System.Drawing.Size(130, 26);
            this.txtDetailLastLogin.TabIndex = 10;
            // 
            // lblDetailPlaceholder
            // 
            this.lblDetailPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailPlaceholder.ForeColor = foreColorGray; // Use GrayText for placeholder
            this.lblDetailPlaceholder.Location = new System.Drawing.Point(15, 15);
            this.lblDetailPlaceholder.Name = "lblDetailPlaceholder";
            this.lblDetailPlaceholder.Size = new System.Drawing.Size(218, 400);
            this.lblDetailPlaceholder.TabIndex = 11;
            this.lblDetailPlaceholder.Text = "\r\n\r\n请在左侧选择用户\r\n以查看详细信息";
            this.lblDetailPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // searchDebounceTimer
            // 
            this.searchDebounceTimer.Interval = 500;
            this.searchDebounceTimer.Tick += new System.EventHandler(this.searchDebounceTimer_Tick);
            // 
            // separatorLabel
            // 
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.separatorLabel.Location = new System.Drawing.Point(0, 55);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(778, 2);
            this.separatorLabel.TabIndex = 4;
            // 
            // FrmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = backColorWindow; // Use Window background for form
            this.ClientSize = new System.Drawing.Size(778, 544);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.pnlUserDetails);
            this.Controls.Add(this.searchPanel);
            this.Controls.Add(this.separatorLabel);
            this.Controls.Add(this.toolPanel);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = foreColorDefault; // Default text color
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.FrmUser_Load);
            this.Shown += new System.EventHandler(this.FrmUser_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.toolPanel.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.pnlUserDetails.ResumeLayout(false);
            this.pnlUserDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer searchDebounceTimer;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnResetPwd;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewTextBoxColumn RealName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RoleNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastLoginTime;
        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Panel pnlUserDetails;
        private System.Windows.Forms.Label lblDetailHeader;
        private System.Windows.Forms.Label lblDetailId;
        private System.Windows.Forms.TextBox txtDetailId;
        private System.Windows.Forms.Label lblDetailUsername;
        private System.Windows.Forms.TextBox txtDetailUsername;
        private System.Windows.Forms.Label lblDetailRealName;
        private System.Windows.Forms.TextBox txtDetailRealName;
        private System.Windows.Forms.Label lblDetailRoleName;
        private System.Windows.Forms.TextBox txtDetailRoleName;
        private System.Windows.Forms.Label lblDetailLastLogin;
        private System.Windows.Forms.TextBox txtDetailLastLogin;
        private System.Windows.Forms.Label lblDetailPlaceholder;
        private System.Windows.Forms.Label separatorLabel; // Add Separator Label
    }
} 