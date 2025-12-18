namespace MDMUI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.picPassword = new System.Windows.Forms.PictureBox();
            this.txtNewUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.picUsername = new System.Windows.Forms.PictureBox();
            this.cboNewLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.picLanguage = new System.Windows.Forms.PictureBox();
            this.cboNewFactory = new System.Windows.Forms.ComboBox();
            this.lblFactory = new System.Windows.Forms.Label();
            this.picFactory = new System.Windows.Forms.PictureBox();
            this.lblLoginHeader = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFactory)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(52)))), ((int)(((byte)(212)))));
            this.pnlHeader.Controls.Add(this.btnMinimize);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(900, 40);
            this.pnlHeader.TabIndex = 9;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(820, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(40, 40);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.Text = "—";
            this.btnMinimize.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(860, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "×";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(234, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "MDM 智能制造系统";
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(65)))), ((int)(((byte)(220)))));
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(450, 560);
            this.pnlLeft.TabIndex = 10;
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.Color.White;
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Controls.Add(this.chkRemember);
            this.pnlLogin.Controls.Add(this.txtNewPassword);
            this.pnlLogin.Controls.Add(this.lblPassword);
            this.pnlLogin.Controls.Add(this.picPassword);
            this.pnlLogin.Controls.Add(this.txtNewUsername);
            this.pnlLogin.Controls.Add(this.lblUsername);
            this.pnlLogin.Controls.Add(this.picUsername);
            this.pnlLogin.Controls.Add(this.cboNewLanguage);
            this.pnlLogin.Controls.Add(this.lblLanguage);
            this.pnlLogin.Controls.Add(this.picLanguage);
            this.pnlLogin.Controls.Add(this.cboNewFactory);
            this.pnlLogin.Controls.Add(this.lblFactory);
            this.pnlLogin.Controls.Add(this.picFactory);
            this.pnlLogin.Controls.Add(this.lblLoginHeader);
            this.pnlLogin.Location = new System.Drawing.Point(480, 70);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(380, 480);
            this.pnlLogin.TabIndex = 11;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(52)))), ((int)(((byte)(212)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(50, 400);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(300, 45);
            this.btnLogin.TabIndex = 14;
            this.btnLogin.Text = "登 录";
            this.btnLogin.UseVisualStyleBackColor = false;
            // 
            // chkRemember
            // 
            this.chkRemember.AutoSize = true;
            this.chkRemember.Checked = true;
            this.chkRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRemember.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkRemember.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.chkRemember.Location = new System.Drawing.Point(50, 370);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(103, 28);
            this.chkRemember.TabIndex = 13;
            this.chkRemember.Text = "记住密码";
            this.chkRemember.UseVisualStyleBackColor = true;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNewPassword.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNewPassword.Location = new System.Drawing.Point(50, 330);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '●';
            this.txtNewPassword.Size = new System.Drawing.Size(300, 30);
            this.txtNewPassword.TabIndex = 12;
            this.txtNewPassword.Text = "1";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.lblPassword.Location = new System.Drawing.Point(88, 300);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 30);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "密码";
            // 
            // picPassword
            // 
            this.picPassword.BackColor = System.Drawing.Color.Transparent;
            this.picPassword.Location = new System.Drawing.Point(50, 298);
            this.picPassword.Name = "picPassword";
            this.picPassword.Size = new System.Drawing.Size(32, 32);
            this.picPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picPassword.TabIndex = 10;
            this.picPassword.TabStop = false;
            // 
            // txtNewUsername
            // 
            this.txtNewUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNewUsername.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNewUsername.Location = new System.Drawing.Point(50, 260);
            this.txtNewUsername.Name = "txtNewUsername";
            this.txtNewUsername.Size = new System.Drawing.Size(300, 30);
            this.txtNewUsername.TabIndex = 9;
            this.txtNewUsername.Text = "admin";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.lblUsername.Location = new System.Drawing.Point(88, 230);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(79, 30);
            this.lblUsername.TabIndex = 8;
            this.lblUsername.Text = "用户名";
            // 
            // picUsername
            // 
            this.picUsername.BackColor = System.Drawing.Color.Transparent;
            this.picUsername.Location = new System.Drawing.Point(50, 228);
            this.picUsername.Name = "picUsername";
            this.picUsername.Size = new System.Drawing.Size(32, 32);
            this.picUsername.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picUsername.TabIndex = 7;
            this.picUsername.TabStop = false;
            // 
            // cboNewLanguage
            // 
            this.cboNewLanguage.BackColor = System.Drawing.Color.White;
            this.cboNewLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboNewLanguage.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboNewLanguage.FormattingEnabled = true;
            this.cboNewLanguage.Items.AddRange(new object[] {
            "中文",
            "English"});
            this.cboNewLanguage.Location = new System.Drawing.Point(50, 190);
            this.cboNewLanguage.Name = "cboNewLanguage";
            this.cboNewLanguage.Size = new System.Drawing.Size(300, 35);
            this.cboNewLanguage.TabIndex = 6;
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.lblLanguage.Location = new System.Drawing.Point(88, 160);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(57, 30);
            this.lblLanguage.TabIndex = 5;
            this.lblLanguage.Text = "语言";
            // 
            // picLanguage
            // 
            this.picLanguage.BackColor = System.Drawing.Color.Transparent;
            this.picLanguage.Location = new System.Drawing.Point(50, 158);
            this.picLanguage.Name = "picLanguage";
            this.picLanguage.Size = new System.Drawing.Size(32, 32);
            this.picLanguage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLanguage.TabIndex = 4;
            this.picLanguage.TabStop = false;
            // 
            // cboNewFactory
            // 
            this.cboNewFactory.BackColor = System.Drawing.Color.White;
            this.cboNewFactory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewFactory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboNewFactory.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboNewFactory.FormattingEnabled = true;
            this.cboNewFactory.Location = new System.Drawing.Point(50, 120);
            this.cboNewFactory.Name = "cboNewFactory";
            this.cboNewFactory.Size = new System.Drawing.Size(300, 35);
            this.cboNewFactory.TabIndex = 3;
            // 
            // lblFactory
            // 
            this.lblFactory.AutoSize = true;
            this.lblFactory.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.lblFactory.Location = new System.Drawing.Point(88, 90);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(57, 30);
            this.lblFactory.TabIndex = 2;
            this.lblFactory.Text = "工厂";
            // 
            // picFactory
            // 
            this.picFactory.BackColor = System.Drawing.Color.Transparent;
            this.picFactory.Location = new System.Drawing.Point(50, 88);
            this.picFactory.Name = "picFactory";
            this.picFactory.Size = new System.Drawing.Size(32, 32);
            this.picFactory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picFactory.TabIndex = 1;
            this.picFactory.TabStop = false;
            // 
            // lblLoginHeader
            // 
            this.lblLoginHeader.AutoSize = true;
            this.lblLoginHeader.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoginHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
            this.lblLoginHeader.Location = new System.Drawing.Point(150, 30);
            this.lblLoginHeader.Name = "lblLoginHeader";
            this.lblLoginHeader.Size = new System.Drawing.Size(164, 47);
            this.lblLoginHeader.TabIndex = 0;
            this.lblLoginHeader.Text = "用户登录";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersion.ForeColor = System.Drawing.Color.Gray;
            this.lblVersion.Location = new System.Drawing.Point(598, 566);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(262, 25);
            this.lblVersion.TabIndex = 12;
            this.lblVersion.Text = "版本 V9.9.9 © 2025 轩天帝";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVersion.Click += new System.EventHandler(this.lblVersion_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.ControlBox = false;
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pnlLogin);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MDM系统登录";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFactory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.PictureBox picPassword;
        private System.Windows.Forms.TextBox txtNewUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.PictureBox picUsername;
        private System.Windows.Forms.ComboBox cboNewLanguage;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.PictureBox picLanguage;
        private System.Windows.Forms.ComboBox cboNewFactory;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.PictureBox picFactory;
        private System.Windows.Forms.Label lblLoginHeader;
        private System.Windows.Forms.Label lblVersion;
    }
}

