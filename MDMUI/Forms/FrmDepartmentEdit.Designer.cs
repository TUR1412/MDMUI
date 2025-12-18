namespace MDMUI
{
    partial class FrmDepartmentEdit
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
            this.lblDepartmentId = new System.Windows.Forms.Label();
            this.txtDepartmentId = new System.Windows.Forms.TextBox();
            this.lblDepartmentName = new System.Windows.Forms.Label();
            this.txtDepartmentName = new System.Windows.Forms.TextBox();
            this.lblFactory = new System.Windows.Forms.Label();
            this.cboFactory = new System.Windows.Forms.ComboBox();
            this.lblParentDept = new System.Windows.Forms.Label();
            this.cboParentDept = new System.Windows.Forms.ComboBox();
            this.lblManager = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDepartmentId
            // 
            this.lblDepartmentId.Location = new System.Drawing.Point(30, 30);
            this.lblDepartmentId.Name = "lblDepartmentId";
            this.lblDepartmentId.Size = new System.Drawing.Size(100, 25);
            this.lblDepartmentId.TabIndex = 0;
            this.lblDepartmentId.Text = "部门编号:";
            this.lblDepartmentId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDepartmentId
            // 
            this.txtDepartmentId.Location = new System.Drawing.Point(140, 30);
            this.txtDepartmentId.Name = "txtDepartmentId";
            this.txtDepartmentId.Size = new System.Drawing.Size(320, 22); // Adjusted standard height
            this.txtDepartmentId.TabIndex = 1;
            // txtDepartmentId.ReadOnly will be set in code based on isNew
            // 
            // lblDepartmentName
            // 
            this.lblDepartmentName.Location = new System.Drawing.Point(30, 70); // 30 + 40
            this.lblDepartmentName.Name = "lblDepartmentName";
            this.lblDepartmentName.Size = new System.Drawing.Size(100, 25);
            this.lblDepartmentName.TabIndex = 2;
            this.lblDepartmentName.Text = "部门名称:";
            this.lblDepartmentName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDepartmentName
            // 
            this.txtDepartmentName.Location = new System.Drawing.Point(140, 70);
            this.txtDepartmentName.Name = "txtDepartmentName";
            this.txtDepartmentName.Size = new System.Drawing.Size(320, 22);
            this.txtDepartmentName.TabIndex = 3;
            // 
            // lblFactory
            // 
            this.lblFactory.Location = new System.Drawing.Point(30, 110); // 70 + 40
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(100, 25);
            this.lblFactory.TabIndex = 4;
            this.lblFactory.Text = "所属工厂:";
            this.lblFactory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboFactory
            // 
            this.cboFactory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFactory.FormattingEnabled = true;
            this.cboFactory.Location = new System.Drawing.Point(140, 110);
            this.cboFactory.Name = "cboFactory";
            this.cboFactory.Size = new System.Drawing.Size(320, 24); // Adjusted standard height
            this.cboFactory.TabIndex = 5;
            this.cboFactory.SelectedIndexChanged += new System.EventHandler(this.CboFactory_SelectedIndexChanged);
            // 
            // lblParentDept
            // 
            this.lblParentDept.Location = new System.Drawing.Point(30, 150); // 110 + 40
            this.lblParentDept.Name = "lblParentDept";
            this.lblParentDept.Size = new System.Drawing.Size(100, 25);
            this.lblParentDept.TabIndex = 6;
            this.lblParentDept.Text = "上级部门:";
            this.lblParentDept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboParentDept
            // 
            this.cboParentDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParentDept.FormattingEnabled = true;
            this.cboParentDept.Location = new System.Drawing.Point(140, 150);
            this.cboParentDept.Name = "cboParentDept";
            this.cboParentDept.Size = new System.Drawing.Size(320, 24);
            this.cboParentDept.TabIndex = 7;
            // 
            // lblManager
            // 
            this.lblManager.Location = new System.Drawing.Point(30, 190); // 150 + 40
            this.lblManager.Name = "lblManager";
            this.lblManager.Size = new System.Drawing.Size(100, 25);
            this.lblManager.TabIndex = 8;
            this.lblManager.Text = "部门负责人:";
            this.lblManager.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(140, 190);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(320, 22);
            this.txtManager.TabIndex = 9;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(30, 230); // 190 + 40
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 25);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "描述:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(140, 230);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(320, 50); // 25 * 2
            this.txtDescription.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(140, 310); // Approx Y: 30 + 7*40 = 310
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30); // 25 + 5
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(260, 310); // btnSave.X + 100 + 20
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmDepartmentEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 370); // Adjusted height
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtManager);
            this.Controls.Add(this.lblManager);
            this.Controls.Add(this.cboParentDept);
            this.Controls.Add(this.lblParentDept);
            this.Controls.Add(this.cboFactory);
            this.Controls.Add(this.lblFactory);
            this.Controls.Add(this.txtDepartmentName);
            this.Controls.Add(this.lblDepartmentName);
            this.Controls.Add(this.txtDepartmentId);
            this.Controls.Add(this.lblDepartmentId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDepartmentEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑部门"; // Default text
            this.Load += new System.EventHandler(this.FrmDepartmentEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblDepartmentId;
        private System.Windows.Forms.TextBox txtDepartmentId;
        private System.Windows.Forms.Label lblDepartmentName;
        private System.Windows.Forms.TextBox txtDepartmentName;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.ComboBox cboFactory;
        private System.Windows.Forms.Label lblParentDept;
        private System.Windows.Forms.ComboBox cboParentDept;
        private System.Windows.Forms.Label lblManager;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
} 