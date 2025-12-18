namespace MDMUI
{
    partial class FrmUserEdit
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblRealName = new System.Windows.Forms.Label();
            this.txtRealName = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblUsername
            //
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(30, 33);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(64, 24); // Adjusted size for font
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "用户名:";
            //
            // txtUsername
            //
            this.txtUsername.Location = new System.Drawing.Point(120, 30);
            this.txtUsername.MaxLength = 50;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(250, 30); // Adjusted size for font
            this.txtUsername.TabIndex = 1;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(30, 73);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(50, 24); // Adjusted size for font
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "密码:";
            //
            // txtPassword
            //
            this.txtPassword.Location = new System.Drawing.Point(120, 70);
            this.txtPassword.MaxLength = 50; // Consider password policy
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 30); // Adjusted size for font
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // lblRealName
            //
            this.lblRealName.AutoSize = true;
            this.lblRealName.Location = new System.Drawing.Point(30, 113);
            this.lblRealName.Name = "lblRealName";
            this.lblRealName.Size = new System.Drawing.Size(82, 24); // Adjusted size for font
            this.lblRealName.TabIndex = 4;
            this.lblRealName.Text = "真实姓名:";
            //
            // txtRealName
            //
            this.txtRealName.Location = new System.Drawing.Point(120, 110);
            this.txtRealName.MaxLength = 50;
            this.txtRealName.Name = "txtRealName";
            this.txtRealName.Size = new System.Drawing.Size(250, 30); // Adjusted size for font
            this.txtRealName.TabIndex = 5;
            //
            // lblRole
            //
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(30, 153);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(50, 24); // Adjusted size for font
            this.lblRole.TabIndex = 6;
            this.lblRole.Text = "角色:";
            //
            // cmbRole
            //
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(120, 150);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(250, 32); // Adjusted size for font
            this.cmbRole.TabIndex = 7;
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(120, 210);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(270, 210); // Adjusted position
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // FrmUserEdit
            //
            this.AcceptButton = this.btnSave; // Pressing Enter triggers Save
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F); // Match FrmUser font
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel; // Pressing Esc triggers Cancel
            this.ClientSize = new System.Drawing.Size(404, 271); // Adjusted size
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.txtRealName);
            this.Controls.Add(this.lblRealName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))); // Match FrmUser font
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4); // Match FrmUser font settings
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUserEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑用户"; // Default text, will be changed in constructor
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblRealName;
        private System.Windows.Forms.TextBox txtRealName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
} 