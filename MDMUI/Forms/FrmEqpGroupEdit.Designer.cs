using System.Drawing;
using System.Windows.Forms;

namespace MDMUI
{
    partial class FrmEqpGroupEdit
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
            this.lblEqpGroupId = new System.Windows.Forms.Label();
            this.txtEqpGroupId = new System.Windows.Forms.TextBox();
            this.lblEqpGroupType = new System.Windows.Forms.Label();
            this.txtEqpGroupType = new System.Windows.Forms.TextBox();
            this.lblFactory = new System.Windows.Forms.Label();
            this.cmbFactory = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            
            // 添加新的布局控件
            this.mainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            
            // 初始化容器控件
            this.mainTableLayout.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            
            // mainTableLayout - 主布局容器
            // 
            this.mainTableLayout.ColumnCount = 2;
            this.mainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.mainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            
            this.mainTableLayout.Controls.Add(this.lblEqpGroupId, 0, 0);
            this.mainTableLayout.Controls.Add(this.txtEqpGroupId, 1, 0);
            this.mainTableLayout.Controls.Add(this.lblEqpGroupType, 0, 1);
            this.mainTableLayout.Controls.Add(this.txtEqpGroupType, 1, 1);
            this.mainTableLayout.Controls.Add(this.lblFactory, 0, 2);
            this.mainTableLayout.Controls.Add(this.cmbFactory, 1, 2);
            this.mainTableLayout.Controls.Add(this.lblDescription, 0, 3);
            this.mainTableLayout.Controls.Add(this.txtDescription, 1, 3);
            this.mainTableLayout.Controls.Add(this.buttonPanel, 0, 4);
            
            this.mainTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayout.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayout.Margin = new System.Windows.Forms.Padding(0);
            this.mainTableLayout.Name = "mainTableLayout";
            this.mainTableLayout.Padding = new System.Windows.Forms.Padding(20);
            this.mainTableLayout.RowCount = 5;
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.mainTableLayout.Size = new System.Drawing.Size(480, 350);
            this.mainTableLayout.TabIndex = 0;
            
            // buttonPanel - 按钮区域
            // 
            this.buttonPanel.Controls.Add(this.btnSave);
            this.buttonPanel.Controls.Add(this.btnCancel);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.Location = new System.Drawing.Point(20, 270);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(440, 60);
            this.buttonPanel.TabIndex = 8;
            
            // lblEqpGroupId
            // 
            this.lblEqpGroupId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEqpGroupId.AutoSize = true;
            this.lblEqpGroupId.Location = new System.Drawing.Point(23, 31);
            this.lblEqpGroupId.Name = "lblEqpGroupId";
            this.lblEqpGroupId.Size = new System.Drawing.Size(104, 24);
            this.lblEqpGroupId.TabIndex = 0;
            this.lblEqpGroupId.Text = "设备组编号:";
            // 
            // txtEqpGroupId
            // 
            this.txtEqpGroupId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEqpGroupId.Location = new System.Drawing.Point(143, 27);
            this.txtEqpGroupId.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txtEqpGroupId.MaxLength = 100;
            this.txtEqpGroupId.Name = "txtEqpGroupId";
            this.txtEqpGroupId.Size = new System.Drawing.Size(302, 31);
            this.txtEqpGroupId.TabIndex = 1;
            // 
            // lblEqpGroupType
            // 
            this.lblEqpGroupType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEqpGroupType.AutoSize = true;
            this.lblEqpGroupType.Location = new System.Drawing.Point(23, 71);
            this.lblEqpGroupType.Name = "lblEqpGroupType";
            this.lblEqpGroupType.Size = new System.Drawing.Size(104, 24);
            this.lblEqpGroupType.TabIndex = 2;
            this.lblEqpGroupType.Text = "设备组类型:";
            // 
            // txtEqpGroupType
            // 
            this.txtEqpGroupType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEqpGroupType.Location = new System.Drawing.Point(143, 67);
            this.txtEqpGroupType.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.txtEqpGroupType.MaxLength = 100;
            this.txtEqpGroupType.Name = "txtEqpGroupType";
            this.txtEqpGroupType.Size = new System.Drawing.Size(302, 31);
            this.txtEqpGroupType.TabIndex = 3;
            // 
            // lblFactory
            // 
            this.lblFactory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(23, 111);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(90, 24);
            this.lblFactory.TabIndex = 4;
            this.lblFactory.Text = "所属工厂:";
            // 
            // cmbFactory
            // 
            this.cmbFactory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFactory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.Location = new System.Drawing.Point(143, 107);
            this.cmbFactory.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(302, 32);
            this.cmbFactory.TabIndex = 5;
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top)));
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(23, 150);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(104, 24);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "设备组说明:";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(143, 143);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(3, 3, 15, 10);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(302, 117);
            this.txtDescription.TabIndex = 7;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Location = new System.Drawing.Point(128, 14);
            this.btnSave.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 36);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(248, 14);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 36);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmEqpGroupEdit
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(480, 350);
            this.Controls.Add(this.mainTableLayout);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEqpGroupEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "编辑设备组";
            this.Load += new System.EventHandler(this.FrmEqpGroupEdit_Load);
            this.mainTableLayout.ResumeLayout(false);
            this.mainTableLayout.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Label lblEqpGroupId;
        private TextBox txtEqpGroupId;
        private Label lblEqpGroupType;
        private TextBox txtEqpGroupType;
        private Label lblFactory;
        private ComboBox cmbFactory;
        private Label lblDescription;
        private TextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;
        
        // 新增布局控件
        private TableLayoutPanel mainTableLayout;
        private Panel buttonPanel;
    }
} 