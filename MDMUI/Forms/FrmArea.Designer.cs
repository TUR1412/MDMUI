using System.Drawing;
using System.Windows.Forms;

namespace MDMUI
{
    partial class FrmArea
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
            this.toolPanel = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.detailPanel = new System.Windows.Forms.Panel();
            this.lblDetailRemarkValue = new System.Windows.Forms.Label();
            this.lblDetailRemarkTitle = new System.Windows.Forms.Label();
            this.lblDetailPostalCodeValue = new System.Windows.Forms.Label();
            this.lblDetailPostalCodeTitle = new System.Windows.Forms.Label();
            this.lblDetailParentAreaValue = new System.Windows.Forms.Label();
            this.lblDetailParentAreaTitle = new System.Windows.Forms.Label();
            this.lblDetailAreaNameValue = new System.Windows.Forms.Label();
            this.lblDetailAreaNameTitle = new System.Windows.Forms.Label();
            this.lblDetailAreaIdValue = new System.Windows.Forms.Label();
            this.lblDetailAreaIdTitle = new System.Windows.Forms.Label();
            this.labelDetailTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.toolPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.detailPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolPanel
            // 
            this.toolPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolPanel.Controls.Add(this.btnRefresh);
            this.toolPanel.Controls.Add(this.btnDelete);
            this.toolPanel.Controls.Add(this.btnEdit);
            this.toolPanel.Controls.Add(this.btnAdd);
            this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolPanel.Location = new System.Drawing.Point(0, 80); // Adjusted Y based on original Padding
            this.toolPanel.Name = "toolPanel";
            this.toolPanel.Padding = new System.Windows.Forms.Padding(5);
            this.toolPanel.Size = new System.Drawing.Size(800, 50);
            this.toolPanel.TabIndex = 2; // Assign TabIndex
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(340, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(230, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(120, 10);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(10, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 130); // Below toolPanel
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            this.splitContainer.Panel1MinSize = 150;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.detailPanel);
            this.splitContainer.Panel2MinSize = 200;
            this.splitContainer.Size = new System.Drawing.Size(800, 320); // Fill remaining space
            this.splitContainer.SplitterDistance = 250;
            this.splitContainer.TabIndex = 3; // Assign TabIndex
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.treeView.FullRowSelect = true;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.ShowLines = true;
            this.treeView.Size = new System.Drawing.Size(250, 320);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            // 
            // detailPanel
            // 
            this.detailPanel.BackColor = System.Drawing.Color.White;
            this.detailPanel.Controls.Add(this.lblDetailRemarkValue);
            this.detailPanel.Controls.Add(this.lblDetailRemarkTitle);
            this.detailPanel.Controls.Add(this.lblDetailPostalCodeValue);
            this.detailPanel.Controls.Add(this.lblDetailPostalCodeTitle);
            this.detailPanel.Controls.Add(this.lblDetailParentAreaValue);
            this.detailPanel.Controls.Add(this.lblDetailParentAreaTitle);
            this.detailPanel.Controls.Add(this.lblDetailAreaNameValue);
            this.detailPanel.Controls.Add(this.lblDetailAreaNameTitle);
            this.detailPanel.Controls.Add(this.lblDetailAreaIdValue);
            this.detailPanel.Controls.Add(this.lblDetailAreaIdTitle);
            this.detailPanel.Controls.Add(this.labelDetailTitle);
            this.detailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailPanel.Location = new System.Drawing.Point(0, 0);
            this.detailPanel.Name = "detailPanel";
            this.detailPanel.Padding = new System.Windows.Forms.Padding(10);
            this.detailPanel.Size = new System.Drawing.Size(546, 320);
            this.detailPanel.TabIndex = 0;
            // 
            // lblDetailRemarkValue
            // 
            this.lblDetailRemarkValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailRemarkValue.Location = new System.Drawing.Point(120, 170); // 50 + 30*4
            this.lblDetailRemarkValue.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblDetailRemarkValue.Name = "lblDetailRemarkValue";
            this.lblDetailRemarkValue.Size = new System.Drawing.Size(350, 80); // Height = 80
            this.lblDetailRemarkValue.TabIndex = 11;
            this.lblDetailRemarkValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailRemarkTitle
            // 
            this.lblDetailRemarkTitle.AutoSize = true;
            this.lblDetailRemarkTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailRemarkTitle.Location = new System.Drawing.Point(15, 170); // 50 + 30*4
            this.lblDetailRemarkTitle.Name = "lblDetailRemarkTitle";
            this.lblDetailRemarkTitle.Size = new System.Drawing.Size(43, 20);
            this.lblDetailRemarkTitle.TabIndex = 10;
            this.lblDetailRemarkTitle.Text = "备注:";
            this.lblDetailRemarkTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
             // 
            // lblDetailPostalCodeValue
            // 
            this.lblDetailPostalCodeValue.AutoSize = true;
            this.lblDetailPostalCodeValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailPostalCodeValue.Location = new System.Drawing.Point(120, 140); // 50 + 30*3
            this.lblDetailPostalCodeValue.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblDetailPostalCodeValue.Name = "lblDetailPostalCodeValue";
            this.lblDetailPostalCodeValue.Size = new System.Drawing.Size(0, 20);
            this.lblDetailPostalCodeValue.TabIndex = 9;
            this.lblDetailPostalCodeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailPostalCodeTitle
            // 
            this.lblDetailPostalCodeTitle.AutoSize = true;
            this.lblDetailPostalCodeTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailPostalCodeTitle.Location = new System.Drawing.Point(15, 140); // 50 + 30*3
            this.lblDetailPostalCodeTitle.Name = "lblDetailPostalCodeTitle";
            this.lblDetailPostalCodeTitle.Size = new System.Drawing.Size(73, 20);
            this.lblDetailPostalCodeTitle.TabIndex = 8;
            this.lblDetailPostalCodeTitle.Text = "邮政编码:";
            this.lblDetailPostalCodeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailParentAreaValue
            // 
            this.lblDetailParentAreaValue.AutoSize = true;
            this.lblDetailParentAreaValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailParentAreaValue.Location = new System.Drawing.Point(120, 110); // 50 + 30*2
            this.lblDetailParentAreaValue.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblDetailParentAreaValue.Name = "lblDetailParentAreaValue";
            this.lblDetailParentAreaValue.Size = new System.Drawing.Size(0, 20);
            this.lblDetailParentAreaValue.TabIndex = 7;
            this.lblDetailParentAreaValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailParentAreaTitle
            // 
            this.lblDetailParentAreaTitle.AutoSize = true;
            this.lblDetailParentAreaTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailParentAreaTitle.Location = new System.Drawing.Point(15, 110); // 50 + 30*2
            this.lblDetailParentAreaTitle.Name = "lblDetailParentAreaTitle";
            this.lblDetailParentAreaTitle.Size = new System.Drawing.Size(73, 20);
            this.lblDetailParentAreaTitle.TabIndex = 6;
            this.lblDetailParentAreaTitle.Text = "上级区域:";
            this.lblDetailParentAreaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailAreaNameValue
            // 
            this.lblDetailAreaNameValue.AutoSize = true;
            this.lblDetailAreaNameValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailAreaNameValue.Location = new System.Drawing.Point(120, 80); // 50 + 30
            this.lblDetailAreaNameValue.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblDetailAreaNameValue.Name = "lblDetailAreaNameValue";
            this.lblDetailAreaNameValue.Size = new System.Drawing.Size(0, 20);
            this.lblDetailAreaNameValue.TabIndex = 5;
            this.lblDetailAreaNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailAreaNameTitle
            // 
            this.lblDetailAreaNameTitle.AutoSize = true;
            this.lblDetailAreaNameTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailAreaNameTitle.Location = new System.Drawing.Point(15, 80); // 50 + 30
            this.lblDetailAreaNameTitle.Name = "lblDetailAreaNameTitle";
            this.lblDetailAreaNameTitle.Size = new System.Drawing.Size(73, 20);
            this.lblDetailAreaNameTitle.TabIndex = 4;
            this.lblDetailAreaNameTitle.Text = "区域名称:";
            this.lblDetailAreaNameTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetailAreaIdValue
            // 
            this.lblDetailAreaIdValue.AutoSize = true;
            this.lblDetailAreaIdValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailAreaIdValue.Location = new System.Drawing.Point(120, 50);
            this.lblDetailAreaIdValue.MaximumSize = new System.Drawing.Size(350, 0);
            this.lblDetailAreaIdValue.Name = "lblDetailAreaIdValue";
            this.lblDetailAreaIdValue.Size = new System.Drawing.Size(0, 20);
            this.lblDetailAreaIdValue.TabIndex = 3;
            this.lblDetailAreaIdValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetailAreaIdTitle
            // 
            this.lblDetailAreaIdTitle.AutoSize = true;
            this.lblDetailAreaIdTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblDetailAreaIdTitle.Location = new System.Drawing.Point(15, 50);
            this.lblDetailAreaIdTitle.Name = "lblDetailAreaIdTitle";
            this.lblDetailAreaIdTitle.Size = new System.Drawing.Size(73, 20);
            this.lblDetailAreaIdTitle.TabIndex = 2;
            this.lblDetailAreaIdTitle.Text = "区域编号:";
            this.lblDetailAreaIdTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDetailTitle
            // 
            this.labelDetailTitle.AutoSize = true;
            this.labelDetailTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelDetailTitle.Location = new System.Drawing.Point(15, 15);
            this.labelDetailTitle.Name = "labelDetailTitle";
            this.labelDetailTitle.Size = new System.Drawing.Size(92, 27);
            this.labelDetailTitle.TabIndex = 1;
            this.labelDetailTitle.Text = "区域详情";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(134, 31);
            this.lblTitle.TabIndex = 0; // Assign TabIndex
            this.lblTitle.Text = "生产地信息";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.lblInfo.Location = new System.Drawing.Point(22, 60); // Below lblTitle
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(200, 23); // Example size, AutoSize handles it
            this.lblInfo.TabIndex = 1; // Assign TabIndex
            this.lblInfo.Text = "当前用户: {Username}, 工厂ID: {FactoryId}"; // Placeholder text
            // 
            // FrmArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolPanel);
            this.Controls.Add(this.lblInfo); // Add to form controls
            this.Controls.Add(this.lblTitle); // Add to form controls
            this.Name = "FrmArea";
            this.Text = "生产地信息";
            this.Load += new System.EventHandler(this.FrmArea_Load);
            this.toolPanel.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.detailPanel.ResumeLayout(false);
            this.detailPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel toolPanel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Panel detailPanel;
        private System.Windows.Forms.Label lblDetailRemarkValue;
        private System.Windows.Forms.Label lblDetailRemarkTitle;
        private System.Windows.Forms.Label lblDetailPostalCodeValue;
        private System.Windows.Forms.Label lblDetailPostalCodeTitle;
        private System.Windows.Forms.Label lblDetailParentAreaValue;
        private System.Windows.Forms.Label lblDetailParentAreaTitle;
        private System.Windows.Forms.Label lblDetailAreaNameValue;
        private System.Windows.Forms.Label lblDetailAreaNameTitle;
        private System.Windows.Forms.Label lblDetailAreaIdValue;
        private System.Windows.Forms.Label lblDetailAreaIdTitle;
        private System.Windows.Forms.Label labelDetailTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInfo;
    }
} 