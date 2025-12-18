namespace MDMUI
{
    partial class FrmFactoryEdit
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
            this.lblFactoryId = new System.Windows.Forms.Label();
            this.txtFactoryId = new System.Windows.Forms.TextBox();
            this.lblFactoryName = new System.Windows.Forms.Label();
            this.txtFactoryName = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblManager = new System.Windows.Forms.Label();
            this.comboBoxManager = new System.Windows.Forms.ComboBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFactoryId
            // 
            this.lblFactoryId.Location = new System.Drawing.Point(30, 30);
            this.lblFactoryId.Name = "lblFactoryId";
            this.lblFactoryId.Size = new System.Drawing.Size(100, 25);
            this.lblFactoryId.TabIndex = 0;
            this.lblFactoryId.Text = "工厂编号:";
            this.lblFactoryId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFactoryId
            // 
            this.txtFactoryId.Location = new System.Drawing.Point(140, 30);
            this.txtFactoryId.MaxLength = 10;
            this.txtFactoryId.Name = "txtFactoryId";
            this.txtFactoryId.Size = new System.Drawing.Size(320, 22); // Standard TextBox height adjustment
            this.txtFactoryId.TabIndex = 1;
            // 
            // lblFactoryName
            // 
            this.lblFactoryName.Location = new System.Drawing.Point(30, 70); // 30 + 40
            this.lblFactoryName.Name = "lblFactoryName";
            this.lblFactoryName.Size = new System.Drawing.Size(100, 25);
            this.lblFactoryName.TabIndex = 2;
            this.lblFactoryName.Text = "工厂名称:";
            this.lblFactoryName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFactoryName
            // 
            this.txtFactoryName.Location = new System.Drawing.Point(140, 70);
            this.txtFactoryName.MaxLength = 50;
            this.txtFactoryName.Name = "txtFactoryName";
            this.txtFactoryName.Size = new System.Drawing.Size(320, 22);
            this.txtFactoryName.TabIndex = 3;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(30, 110); // 70 + 40
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(100, 25);
            this.lblAddress.TabIndex = 4;
            this.lblAddress.Text = "地址:";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(140, 110);
            this.txtAddress.MaxLength = 100;
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAddress.Size = new System.Drawing.Size(320, 50); // 25 * 2
            this.txtAddress.TabIndex = 5;
            // 
            // lblManager
            // 
            this.lblManager.Location = new System.Drawing.Point(30, 175); // 110 + 40 + 25
            this.lblManager.Name = "lblManager";
            this.lblManager.Size = new System.Drawing.Size(100, 25);
            this.lblManager.TabIndex = 6;
            this.lblManager.Text = "负责人:";
            this.lblManager.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxManager
            // 
            this.comboBoxManager = new System.Windows.Forms.ComboBox();
            this.comboBoxManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxManager.FormattingEnabled = true;
            this.comboBoxManager.Location = new System.Drawing.Point(140, 175);
            this.comboBoxManager.Name = "comboBoxManager";
            this.comboBoxManager.Size = new System.Drawing.Size(320, 24);
            this.comboBoxManager.TabIndex = 7;
            // 
            // lblPhone
            // 
            this.lblPhone.Location = new System.Drawing.Point(30, 215); // 175 + 40 
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(100, 25);
            this.lblPhone.TabIndex = 8;
            this.lblPhone.Text = "联系电话:";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(140, 215);
            this.txtPhone.MaxLength = 20;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(320, 22);
            this.txtPhone.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(140, 260); // Y approx: 30 + 5*40 + 25 + 20 = 275, adjusted for text box height changes
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30); // 25 + 5
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(260, 260); // btnSave.X + 100 + 20
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmFactoryEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 320); // Adjusted height based on controls
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.comboBoxManager);
            this.Controls.Add(this.lblManager);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtFactoryName);
            this.Controls.Add(this.lblFactoryName);
            this.Controls.Add(this.txtFactoryId);
            this.Controls.Add(this.lblFactoryId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog; // Prevent resizing
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFactoryEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑工厂"; // Default text, will be overridden in constructor
            this.Load += new System.EventHandler(this.FrmFactoryEdit_Load); // Add Load event handler
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFactoryId;
        private System.Windows.Forms.TextBox txtFactoryId;
        private System.Windows.Forms.Label lblFactoryName;
        private System.Windows.Forms.TextBox txtFactoryName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblManager;
        private System.Windows.Forms.ComboBox comboBoxManager;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
} 