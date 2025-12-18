namespace MDMUI
{
    partial class FrmPermission
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userPanel = new System.Windows.Forms.Panel();
            this.userListBox = new System.Windows.Forms.ListBox();
            this.labelUserListTitle = new System.Windows.Forms.Label();
            this.permissionPanel = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.systemGroup = new System.Windows.Forms.GroupBox();
            this.systemLogCheckBox = new System.Windows.Forms.CheckBox();
            this.systemRestoreCheckBox = new System.Windows.Forms.CheckBox();
            this.systemBackupCheckBox = new System.Windows.Forms.CheckBox();
            this.systemViewCheckBox = new System.Windows.Forms.CheckBox();
            this.areaGroup = new System.Windows.Forms.GroupBox();
            this.areaPrintCheckBox = new System.Windows.Forms.CheckBox();
            this.areaDeleteCheckBox = new System.Windows.Forms.CheckBox();
            this.areaEditCheckBox = new System.Windows.Forms.CheckBox();
            this.areaAddCheckBox = new System.Windows.Forms.CheckBox();
            this.areaViewCheckBox = new System.Windows.Forms.CheckBox();
            this.userGroup = new System.Windows.Forms.GroupBox();
            this.userResetPwdCheckBox = new System.Windows.Forms.CheckBox();
            this.userDeleteCheckBox = new System.Windows.Forms.CheckBox();
            this.userEditCheckBox = new System.Windows.Forms.CheckBox();
            this.userAddCheckBox = new System.Windows.Forms.CheckBox();
            this.userViewCheckBox = new System.Windows.Forms.CheckBox();
            this.factoryGroup = new System.Windows.Forms.GroupBox();
            this.factoryExportCheckBox = new System.Windows.Forms.CheckBox();
            this.factoryPrintCheckBox = new System.Windows.Forms.CheckBox();
            this.factoryDeleteCheckBox = new System.Windows.Forms.CheckBox();
            this.factoryEditCheckBox = new System.Windows.Forms.CheckBox();
            this.factoryAddCheckBox = new System.Windows.Forms.CheckBox();
            this.factoryViewCheckBox = new System.Windows.Forms.CheckBox();
            this.labelPermissionTitle = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.userPanel.SuspendLayout();
            this.permissionPanel.SuspendLayout();
            this.systemGroup.SuspendLayout();
            this.areaGroup.SuspendLayout();
            this.userGroup.SuspendLayout();
            this.factoryGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 647);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1200, 31);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(82, 24);
            this.statusLabel.Text = "准备就绪";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.userPanel);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.permissionPanel);
            this.splitContainer1.Panel2MinSize = 500;
            this.splitContainer1.Size = new System.Drawing.Size(1200, 647);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 1;
            // 
            // userPanel
            // 
            this.userPanel.Controls.Add(this.userListBox);
            this.userPanel.Controls.Add(this.labelUserListTitle);
            this.userPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userPanel.Location = new System.Drawing.Point(0, 0);
            this.userPanel.Margin = new System.Windows.Forms.Padding(4);
            this.userPanel.Name = "userPanel";
            this.userPanel.Padding = new System.Windows.Forms.Padding(15, 14, 15, 14);
            this.userPanel.Size = new System.Drawing.Size(260, 647);
            this.userPanel.TabIndex = 0;
            // 
            // userListBox
            // 
            this.userListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userListBox.DisplayMember = "DisplayText";
            this.userListBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.userListBox.FormattingEnabled = true;
            this.userListBox.ItemHeight = 24;
            this.userListBox.Location = new System.Drawing.Point(19, 55);
            this.userListBox.Margin = new System.Windows.Forms.Padding(4);
            this.userListBox.Name = "userListBox";
            this.userListBox.Size = new System.Drawing.Size(221, 532);
            this.userListBox.TabIndex = 1;
            this.userListBox.ValueMember = "UserId";
            this.userListBox.SelectedIndexChanged += new System.EventHandler(this.UserListBox_SelectedIndexChanged);
            // 
            // labelUserListTitle
            // 
            this.labelUserListTitle.AutoSize = true;
            this.labelUserListTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelUserListTitle.Location = new System.Drawing.Point(15, 14);
            this.labelUserListTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUserListTitle.Name = "labelUserListTitle";
            this.labelUserListTitle.Size = new System.Drawing.Size(110, 31);
            this.labelUserListTitle.TabIndex = 0;
            this.labelUserListTitle.Text = "用户列表";
            // 
            // permissionPanel
            // 
            this.permissionPanel.AutoScroll = true;
            this.permissionPanel.BackColor = System.Drawing.Color.White;
            this.permissionPanel.Controls.Add(this.btnSave);
            this.permissionPanel.Controls.Add(this.systemGroup);
            this.permissionPanel.Controls.Add(this.areaGroup);
            this.permissionPanel.Controls.Add(this.userGroup);
            this.permissionPanel.Controls.Add(this.factoryGroup);
            this.permissionPanel.Controls.Add(this.labelPermissionTitle);
            this.permissionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permissionPanel.Location = new System.Drawing.Point(0, 0);
            this.permissionPanel.Margin = new System.Windows.Forms.Padding(4);
            this.permissionPanel.Name = "permissionPanel";
            this.permissionPanel.Padding = new System.Windows.Forms.Padding(30, 28, 30, 28);
            this.permissionPanel.Size = new System.Drawing.Size(934, 647);
            this.permissionPanel.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.BackColor = System.Drawing.Color.LightGreen;
            this.btnSave.Enabled = false;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(377, 394);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 43);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "保存权限设置";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // systemGroup
            // 
            this.systemGroup.Controls.Add(this.systemLogCheckBox);
            this.systemGroup.Controls.Add(this.systemRestoreCheckBox);
            this.systemGroup.Controls.Add(this.systemBackupCheckBox);
            this.systemGroup.Controls.Add(this.systemViewCheckBox);
            this.systemGroup.Enabled = false;
            this.systemGroup.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.systemGroup.Location = new System.Drawing.Point(465, 224);
            this.systemGroup.Margin = new System.Windows.Forms.Padding(4);
            this.systemGroup.Name = "systemGroup";
            this.systemGroup.Padding = new System.Windows.Forms.Padding(4);
            this.systemGroup.Size = new System.Drawing.Size(345, 146);
            this.systemGroup.TabIndex = 4;
            this.systemGroup.TabStop = false;
            this.systemGroup.Text = "系统设置";
            // 
            // systemLogCheckBox
            // 
            this.systemLogCheckBox.AutoSize = true;
            this.systemLogCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.systemLogCheckBox.Location = new System.Drawing.Point(172, 80);
            this.systemLogCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.systemLogCheckBox.Name = "systemLogCheckBox";
            this.systemLogCheckBox.Size = new System.Drawing.Size(108, 28);
            this.systemLogCheckBox.TabIndex = 3;
            this.systemLogCheckBox.Text = "日志查看";
            this.systemLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // systemRestoreCheckBox
            // 
            this.systemRestoreCheckBox.AutoSize = true;
            this.systemRestoreCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.systemRestoreCheckBox.Location = new System.Drawing.Point(30, 80);
            this.systemRestoreCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.systemRestoreCheckBox.Name = "systemRestoreCheckBox";
            this.systemRestoreCheckBox.Size = new System.Drawing.Size(72, 28);
            this.systemRestoreCheckBox.TabIndex = 2;
            this.systemRestoreCheckBox.Text = "恢复";
            this.systemRestoreCheckBox.UseVisualStyleBackColor = true;
            // 
            // systemBackupCheckBox
            // 
            this.systemBackupCheckBox.AutoSize = true;
            this.systemBackupCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.systemBackupCheckBox.Location = new System.Drawing.Point(172, 43);
            this.systemBackupCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.systemBackupCheckBox.Name = "systemBackupCheckBox";
            this.systemBackupCheckBox.Size = new System.Drawing.Size(72, 28);
            this.systemBackupCheckBox.TabIndex = 1;
            this.systemBackupCheckBox.Text = "备份";
            this.systemBackupCheckBox.UseVisualStyleBackColor = true;
            // 
            // systemViewCheckBox
            // 
            this.systemViewCheckBox.AutoSize = true;
            this.systemViewCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.systemViewCheckBox.Location = new System.Drawing.Point(30, 43);
            this.systemViewCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.systemViewCheckBox.Name = "systemViewCheckBox";
            this.systemViewCheckBox.Size = new System.Drawing.Size(72, 28);
            this.systemViewCheckBox.TabIndex = 0;
            this.systemViewCheckBox.Text = "查看";
            this.systemViewCheckBox.UseVisualStyleBackColor = true;
            // 
            // areaGroup
            // 
            this.areaGroup.Controls.Add(this.areaPrintCheckBox);
            this.areaGroup.Controls.Add(this.areaDeleteCheckBox);
            this.areaGroup.Controls.Add(this.areaEditCheckBox);
            this.areaGroup.Controls.Add(this.areaAddCheckBox);
            this.areaGroup.Controls.Add(this.areaViewCheckBox);
            this.areaGroup.Enabled = false;
            this.areaGroup.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.areaGroup.Location = new System.Drawing.Point(35, 224);
            this.areaGroup.Margin = new System.Windows.Forms.Padding(4);
            this.areaGroup.Name = "areaGroup";
            this.areaGroup.Padding = new System.Windows.Forms.Padding(4);
            this.areaGroup.Size = new System.Drawing.Size(345, 146);
            this.areaGroup.TabIndex = 3;
            this.areaGroup.TabStop = false;
            this.areaGroup.Text = "生产地信息";
            // 
            // areaPrintCheckBox
            // 
            this.areaPrintCheckBox.AutoSize = true;
            this.areaPrintCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.areaPrintCheckBox.Location = new System.Drawing.Point(30, 117);
            this.areaPrintCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.areaPrintCheckBox.Name = "areaPrintCheckBox";
            this.areaPrintCheckBox.Size = new System.Drawing.Size(72, 28);
            this.areaPrintCheckBox.TabIndex = 4;
            this.areaPrintCheckBox.Text = "打印";
            this.areaPrintCheckBox.UseVisualStyleBackColor = true;
            // 
            // areaDeleteCheckBox
            // 
            this.areaDeleteCheckBox.AutoSize = true;
            this.areaDeleteCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.areaDeleteCheckBox.Location = new System.Drawing.Point(172, 80);
            this.areaDeleteCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.areaDeleteCheckBox.Name = "areaDeleteCheckBox";
            this.areaDeleteCheckBox.Size = new System.Drawing.Size(72, 28);
            this.areaDeleteCheckBox.TabIndex = 3;
            this.areaDeleteCheckBox.Text = "删除";
            this.areaDeleteCheckBox.UseVisualStyleBackColor = true;
            // 
            // areaEditCheckBox
            // 
            this.areaEditCheckBox.AutoSize = true;
            this.areaEditCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.areaEditCheckBox.Location = new System.Drawing.Point(30, 80);
            this.areaEditCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.areaEditCheckBox.Name = "areaEditCheckBox";
            this.areaEditCheckBox.Size = new System.Drawing.Size(72, 28);
            this.areaEditCheckBox.TabIndex = 2;
            this.areaEditCheckBox.Text = "编辑";
            this.areaEditCheckBox.UseVisualStyleBackColor = true;
            // 
            // areaAddCheckBox
            // 
            this.areaAddCheckBox.AutoSize = true;
            this.areaAddCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.areaAddCheckBox.Location = new System.Drawing.Point(172, 43);
            this.areaAddCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.areaAddCheckBox.Name = "areaAddCheckBox";
            this.areaAddCheckBox.Size = new System.Drawing.Size(72, 28);
            this.areaAddCheckBox.TabIndex = 1;
            this.areaAddCheckBox.Text = "添加";
            this.areaAddCheckBox.UseVisualStyleBackColor = true;
            // 
            // areaViewCheckBox
            // 
            this.areaViewCheckBox.AutoSize = true;
            this.areaViewCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.areaViewCheckBox.Location = new System.Drawing.Point(30, 43);
            this.areaViewCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.areaViewCheckBox.Name = "areaViewCheckBox";
            this.areaViewCheckBox.Size = new System.Drawing.Size(72, 28);
            this.areaViewCheckBox.TabIndex = 0;
            this.areaViewCheckBox.Text = "查看";
            this.areaViewCheckBox.UseVisualStyleBackColor = true;
            // 
            // userGroup
            // 
            this.userGroup.Controls.Add(this.userResetPwdCheckBox);
            this.userGroup.Controls.Add(this.userDeleteCheckBox);
            this.userGroup.Controls.Add(this.userEditCheckBox);
            this.userGroup.Controls.Add(this.userAddCheckBox);
            this.userGroup.Controls.Add(this.userViewCheckBox);
            this.userGroup.Enabled = false;
            this.userGroup.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.userGroup.Location = new System.Drawing.Point(465, 70);
            this.userGroup.Margin = new System.Windows.Forms.Padding(4);
            this.userGroup.Name = "userGroup";
            this.userGroup.Padding = new System.Windows.Forms.Padding(4);
            this.userGroup.Size = new System.Drawing.Size(345, 146);
            this.userGroup.TabIndex = 2;
            this.userGroup.TabStop = false;
            this.userGroup.Text = "用户管理";
            // 
            // userResetPwdCheckBox
            // 
            this.userResetPwdCheckBox.AutoSize = true;
            this.userResetPwdCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.userResetPwdCheckBox.Location = new System.Drawing.Point(30, 117);
            this.userResetPwdCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.userResetPwdCheckBox.Name = "userResetPwdCheckBox";
            this.userResetPwdCheckBox.Size = new System.Drawing.Size(108, 28);
            this.userResetPwdCheckBox.TabIndex = 4;
            this.userResetPwdCheckBox.Text = "重置密码";
            this.userResetPwdCheckBox.UseVisualStyleBackColor = true;
            // 
            // userDeleteCheckBox
            // 
            this.userDeleteCheckBox.AutoSize = true;
            this.userDeleteCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.userDeleteCheckBox.Location = new System.Drawing.Point(172, 80);
            this.userDeleteCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.userDeleteCheckBox.Name = "userDeleteCheckBox";
            this.userDeleteCheckBox.Size = new System.Drawing.Size(72, 28);
            this.userDeleteCheckBox.TabIndex = 3;
            this.userDeleteCheckBox.Text = "删除";
            this.userDeleteCheckBox.UseVisualStyleBackColor = true;
            // 
            // userEditCheckBox
            // 
            this.userEditCheckBox.AutoSize = true;
            this.userEditCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.userEditCheckBox.Location = new System.Drawing.Point(30, 80);
            this.userEditCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.userEditCheckBox.Name = "userEditCheckBox";
            this.userEditCheckBox.Size = new System.Drawing.Size(72, 28);
            this.userEditCheckBox.TabIndex = 2;
            this.userEditCheckBox.Text = "编辑";
            this.userEditCheckBox.UseVisualStyleBackColor = true;
            // 
            // userAddCheckBox
            // 
            this.userAddCheckBox.AutoSize = true;
            this.userAddCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.userAddCheckBox.Location = new System.Drawing.Point(172, 43);
            this.userAddCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.userAddCheckBox.Name = "userAddCheckBox";
            this.userAddCheckBox.Size = new System.Drawing.Size(72, 28);
            this.userAddCheckBox.TabIndex = 1;
            this.userAddCheckBox.Text = "添加";
            this.userAddCheckBox.UseVisualStyleBackColor = true;
            // 
            // userViewCheckBox
            // 
            this.userViewCheckBox.AutoSize = true;
            this.userViewCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.userViewCheckBox.Location = new System.Drawing.Point(30, 43);
            this.userViewCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.userViewCheckBox.Name = "userViewCheckBox";
            this.userViewCheckBox.Size = new System.Drawing.Size(72, 28);
            this.userViewCheckBox.TabIndex = 0;
            this.userViewCheckBox.Text = "查看";
            this.userViewCheckBox.UseVisualStyleBackColor = true;
            // 
            // factoryGroup
            // 
            this.factoryGroup.Controls.Add(this.factoryExportCheckBox);
            this.factoryGroup.Controls.Add(this.factoryPrintCheckBox);
            this.factoryGroup.Controls.Add(this.factoryDeleteCheckBox);
            this.factoryGroup.Controls.Add(this.factoryEditCheckBox);
            this.factoryGroup.Controls.Add(this.factoryAddCheckBox);
            this.factoryGroup.Controls.Add(this.factoryViewCheckBox);
            this.factoryGroup.Enabled = false;
            this.factoryGroup.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.factoryGroup.Location = new System.Drawing.Point(35, 70);
            this.factoryGroup.Margin = new System.Windows.Forms.Padding(4);
            this.factoryGroup.Name = "factoryGroup";
            this.factoryGroup.Padding = new System.Windows.Forms.Padding(4);
            this.factoryGroup.Size = new System.Drawing.Size(345, 146);
            this.factoryGroup.TabIndex = 1;
            this.factoryGroup.TabStop = false;
            this.factoryGroup.Text = "工厂管理";
            // 
            // factoryExportCheckBox
            // 
            this.factoryExportCheckBox.AutoSize = true;
            this.factoryExportCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.factoryExportCheckBox.Location = new System.Drawing.Point(172, 117);
            this.factoryExportCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.factoryExportCheckBox.Name = "factoryExportCheckBox";
            this.factoryExportCheckBox.Size = new System.Drawing.Size(72, 28);
            this.factoryExportCheckBox.TabIndex = 5;
            this.factoryExportCheckBox.Text = "导出";
            this.factoryExportCheckBox.UseVisualStyleBackColor = true;
            // 
            // factoryPrintCheckBox
            // 
            this.factoryPrintCheckBox.AutoSize = true;
            this.factoryPrintCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.factoryPrintCheckBox.Location = new System.Drawing.Point(30, 117);
            this.factoryPrintCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.factoryPrintCheckBox.Name = "factoryPrintCheckBox";
            this.factoryPrintCheckBox.Size = new System.Drawing.Size(72, 28);
            this.factoryPrintCheckBox.TabIndex = 4;
            this.factoryPrintCheckBox.Text = "打印";
            this.factoryPrintCheckBox.UseVisualStyleBackColor = true;
            // 
            // factoryDeleteCheckBox
            // 
            this.factoryDeleteCheckBox.AutoSize = true;
            this.factoryDeleteCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.factoryDeleteCheckBox.Location = new System.Drawing.Point(172, 80);
            this.factoryDeleteCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.factoryDeleteCheckBox.Name = "factoryDeleteCheckBox";
            this.factoryDeleteCheckBox.Size = new System.Drawing.Size(72, 28);
            this.factoryDeleteCheckBox.TabIndex = 3;
            this.factoryDeleteCheckBox.Text = "删除";
            this.factoryDeleteCheckBox.UseVisualStyleBackColor = true;
            // 
            // factoryEditCheckBox
            // 
            this.factoryEditCheckBox.AutoSize = true;
            this.factoryEditCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.factoryEditCheckBox.Location = new System.Drawing.Point(30, 80);
            this.factoryEditCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.factoryEditCheckBox.Name = "factoryEditCheckBox";
            this.factoryEditCheckBox.Size = new System.Drawing.Size(72, 28);
            this.factoryEditCheckBox.TabIndex = 2;
            this.factoryEditCheckBox.Text = "编辑";
            this.factoryEditCheckBox.UseVisualStyleBackColor = true;
            // 
            // factoryAddCheckBox
            // 
            this.factoryAddCheckBox.AutoSize = true;
            this.factoryAddCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.factoryAddCheckBox.Location = new System.Drawing.Point(172, 43);
            this.factoryAddCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.factoryAddCheckBox.Name = "factoryAddCheckBox";
            this.factoryAddCheckBox.Size = new System.Drawing.Size(72, 28);
            this.factoryAddCheckBox.TabIndex = 1;
            this.factoryAddCheckBox.Text = "添加";
            this.factoryAddCheckBox.UseVisualStyleBackColor = true;
            // 
            // factoryViewCheckBox
            // 
            this.factoryViewCheckBox.AutoSize = true;
            this.factoryViewCheckBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.factoryViewCheckBox.Location = new System.Drawing.Point(30, 43);
            this.factoryViewCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.factoryViewCheckBox.Name = "factoryViewCheckBox";
            this.factoryViewCheckBox.Size = new System.Drawing.Size(72, 28);
            this.factoryViewCheckBox.TabIndex = 0;
            this.factoryViewCheckBox.Text = "查看";
            this.factoryViewCheckBox.UseVisualStyleBackColor = true;
            // 
            // labelPermissionTitle
            // 
            this.labelPermissionTitle.AutoSize = true;
            this.labelPermissionTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelPermissionTitle.Location = new System.Drawing.Point(30, 28);
            this.labelPermissionTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPermissionTitle.Name = "labelPermissionTitle";
            this.labelPermissionTitle.Size = new System.Drawing.Size(110, 31);
            this.labelPermissionTitle.TabIndex = 0;
            this.labelPermissionTitle.Text = "权限设置";
            // 
            // FrmPermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 678);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmPermission";
            this.Text = "权限管理";
            this.Load += new System.EventHandler(this.FrmPermission_Load);
            this.Shown += new System.EventHandler(this.FrmPermission_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.userPanel.ResumeLayout(false);
            this.userPanel.PerformLayout();
            this.permissionPanel.ResumeLayout(false);
            this.permissionPanel.PerformLayout();
            this.systemGroup.ResumeLayout(false);
            this.systemGroup.PerformLayout();
            this.areaGroup.ResumeLayout(false);
            this.areaGroup.PerformLayout();
            this.userGroup.ResumeLayout(false);
            this.userGroup.PerformLayout();
            this.factoryGroup.ResumeLayout(false);
            this.factoryGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel userPanel;
        private System.Windows.Forms.ListBox userListBox;
        private System.Windows.Forms.Label labelUserListTitle;
        private System.Windows.Forms.Panel permissionPanel;
        private System.Windows.Forms.Label labelPermissionTitle;
        private System.Windows.Forms.GroupBox factoryGroup;
        private System.Windows.Forms.CheckBox factoryViewCheckBox;
        private System.Windows.Forms.CheckBox factoryAddCheckBox;
        private System.Windows.Forms.CheckBox factoryEditCheckBox;
        private System.Windows.Forms.CheckBox factoryDeleteCheckBox;
        private System.Windows.Forms.CheckBox factoryPrintCheckBox;
        private System.Windows.Forms.CheckBox factoryExportCheckBox;
        private System.Windows.Forms.GroupBox userGroup;
        private System.Windows.Forms.CheckBox userResetPwdCheckBox;
        private System.Windows.Forms.CheckBox userDeleteCheckBox;
        private System.Windows.Forms.CheckBox userEditCheckBox;
        private System.Windows.Forms.CheckBox userAddCheckBox;
        private System.Windows.Forms.CheckBox userViewCheckBox;
        private System.Windows.Forms.GroupBox areaGroup;
        private System.Windows.Forms.CheckBox areaPrintCheckBox;
        private System.Windows.Forms.CheckBox areaDeleteCheckBox;
        private System.Windows.Forms.CheckBox areaEditCheckBox;
        private System.Windows.Forms.CheckBox areaAddCheckBox;
        private System.Windows.Forms.CheckBox areaViewCheckBox;
        private System.Windows.Forms.GroupBox systemGroup;
        private System.Windows.Forms.CheckBox systemLogCheckBox;
        private System.Windows.Forms.CheckBox systemRestoreCheckBox;
        private System.Windows.Forms.CheckBox systemBackupCheckBox;
        private System.Windows.Forms.CheckBox systemViewCheckBox;
        private System.Windows.Forms.Button btnSave;
    }
} 