namespace MDMUI
{
    partial class FrmAreaEdit
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
            this.lblAreaId = new System.Windows.Forms.Label();
            this.txtAreaId = new System.Windows.Forms.TextBox();
            this.lblAreaName = new System.Windows.Forms.Label();
            this.txtAreaName = new System.Windows.Forms.TextBox();
            this.lblParentArea = new System.Windows.Forms.Label();
            this.cboParentArea = new System.Windows.Forms.ComboBox();
            this.lblPostalCode = new System.Windows.Forms.Label();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.lblRemark = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAreaId
            // 
            this.lblAreaId.Location = new System.Drawing.Point(30, 30);
            this.lblAreaId.Name = "lblAreaId";
            this.lblAreaId.Size = new System.Drawing.Size(100, 25);
            this.lblAreaId.TabIndex = 0;
            this.lblAreaId.Text = "区域编号:";
            this.lblAreaId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAreaId
            // 
            this.txtAreaId.Location = new System.Drawing.Point(140, 30);
            this.txtAreaId.MaxLength = 20;
            this.txtAreaId.Name = "txtAreaId";
            this.txtAreaId.Size = new System.Drawing.Size(320, 22);
            this.txtAreaId.TabIndex = 1;
            // ReadOnly property set in code
            // 
            // lblAreaName
            // 
            this.lblAreaName.Location = new System.Drawing.Point(30, 70); // 30 + 40
            this.lblAreaName.Name = "lblAreaName";
            this.lblAreaName.Size = new System.Drawing.Size(100, 25);
            this.lblAreaName.TabIndex = 2;
            this.lblAreaName.Text = "区域名称:";
            this.lblAreaName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAreaName
            // 
            this.txtAreaName.Location = new System.Drawing.Point(140, 70);
            this.txtAreaName.MaxLength = 100;
            this.txtAreaName.Name = "txtAreaName";
            this.txtAreaName.Size = new System.Drawing.Size(320, 22);
            this.txtAreaName.TabIndex = 3;
            // 
            // lblParentArea
            // 
            this.lblParentArea.Location = new System.Drawing.Point(30, 110); // 70 + 40
            this.lblParentArea.Name = "lblParentArea";
            this.lblParentArea.Size = new System.Drawing.Size(100, 25);
            this.lblParentArea.TabIndex = 4;
            this.lblParentArea.Text = "上级区域:";
            this.lblParentArea.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboParentArea
            // 
            this.cboParentArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParentArea.FormattingEnabled = true;
            this.cboParentArea.Location = new System.Drawing.Point(140, 110);
            this.cboParentArea.Name = "cboParentArea";
            this.cboParentArea.Size = new System.Drawing.Size(320, 24);
            this.cboParentArea.TabIndex = 5;
            // 
            // lblPostalCode
            // 
            this.lblPostalCode.Location = new System.Drawing.Point(30, 150); // 110 + 40
            this.lblPostalCode.Name = "lblPostalCode";
            this.lblPostalCode.Size = new System.Drawing.Size(100, 25);
            this.lblPostalCode.TabIndex = 6;
            this.lblPostalCode.Text = "邮政编码:";
            this.lblPostalCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.Location = new System.Drawing.Point(140, 150);
            this.txtPostalCode.MaxLength = 20;
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(320, 22);
            this.txtPostalCode.TabIndex = 7;
            // 
            // lblRemark
            // 
            this.lblRemark.Location = new System.Drawing.Point(30, 190); // 150 + 40
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(100, 25);
            this.lblRemark.TabIndex = 8;
            this.lblRemark.Text = "备注:";
            this.lblRemark.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(140, 190);
            this.txtRemark.MaxLength = 500;
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemark.Size = new System.Drawing.Size(320, 75); // 25 * 3
            this.txtRemark.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(140, 285); // Y = 190 + 75 + 20
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30); // 25 + 5
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(260, 285); // btnSave.X + 100 + 20
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmAreaEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 345); // Adjusted height: 285 + 30 + 30
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.lblRemark);
            this.Controls.Add(this.txtPostalCode);
            this.Controls.Add(this.lblPostalCode);
            this.Controls.Add(this.cboParentArea);
            this.Controls.Add(this.lblParentArea);
            this.Controls.Add(this.txtAreaName);
            this.Controls.Add(this.lblAreaName);
            this.Controls.Add(this.txtAreaId);
            this.Controls.Add(this.lblAreaId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAreaEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑区域"; // Default text, will be changed in code
            this.Load += new System.EventHandler(this.FrmAreaEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAreaId;
        private System.Windows.Forms.TextBox txtAreaId;
        private System.Windows.Forms.Label lblAreaName;
        private System.Windows.Forms.TextBox txtAreaName;
        private System.Windows.Forms.Label lblParentArea;
        private System.Windows.Forms.ComboBox cboParentArea;
        private System.Windows.Forms.Label lblPostalCode;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.Label lblRemark;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
} 