using System.Drawing;
using System.Windows.Forms;

namespace MDMUI.Forms
{
    partial class FrmDeviceInfo
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
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftSplitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvEquipment = new System.Windows.Forms.DataGridView(); 
            this.detailsPanel = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtEventRemark = new System.Windows.Forms.TextBox();
            this.lblEventRemark = new System.Windows.Forms.Label();
            this.txtEventUser = new System.Windows.Forms.TextBox();
            this.lblEventUser = new System.Windows.Forms.Label();
            this.lblFactory = new System.Windows.Forms.Label();
            this.txtEquipmentLayer = new System.Windows.Forms.TextBox();
            this.lblEquipmentLayer = new System.Windows.Forms.Label();
            this.txtEquipmentSubType = new System.Windows.Forms.TextBox();
            this.lblEquipmentSubType = new System.Windows.Forms.Label();
            this.txtEquipmentDescription = new System.Windows.Forms.TextBox();
            this.lblEquipmentDescription = new System.Windows.Forms.Label();
            this.txtEquipmentType = new System.Windows.Forms.TextBox();
            this.lblEquipmentType = new System.Windows.Forms.Label();
            this.txtEquipmentId = new System.Windows.Forms.TextBox();
            this.lblEquipmentId = new System.Windows.Forms.Label();
            this.txtEquipmentName = new System.Windows.Forms.TextBox();
            this.lblEquipmentName = new System.Windows.Forms.Label();
            this.cmbFactory = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftSplitContainer)).BeginInit();
            this.leftSplitContainer.Panel1.SuspendLayout();
            this.leftSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).BeginInit();
            this.detailsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Vertical;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.leftSplitContainer);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.detailsPanel);
            this.mainSplitContainer.Size = new System.Drawing.Size(1184, 561);
            this.mainSplitContainer.SplitterDistance = 650;
            this.mainSplitContainer.TabIndex = 1;
            // 
            // leftSplitContainer
            // 
            this.leftSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.leftSplitContainer.Name = "leftSplitContainer";
            this.leftSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // leftSplitContainer.Panel1
            // 
            this.leftSplitContainer.Panel1.Controls.Add(this.dgvEquipment);
            // Hide or remove Panel2 if nothing is in it
            this.leftSplitContainer.Panel2Collapsed = true; 
            this.leftSplitContainer.Size = new System.Drawing.Size(650, 501);
            this.leftSplitContainer.TabIndex = 0;
            // 
            // dgvEquipment
            // 
            this.dgvEquipment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEquipment.Location = new System.Drawing.Point(0, 0);
            this.dgvEquipment.Name = "dgvEquipment";
            this.dgvEquipment.RowTemplate.Height = 23;
            this.dgvEquipment.Size = new System.Drawing.Size(650, 501); // Should fill Panel1 of leftSplitContainer
            this.dgvEquipment.TabIndex = 0;
            this.dgvEquipment.SelectionChanged += new System.EventHandler(this.dgvEquipment_SelectionChanged);
            // 
            // detailsPanel
            // 
            this.detailsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.detailsPanel.Controls.Add(this.btnCancel);
            this.detailsPanel.Controls.Add(this.btnSave);
            this.detailsPanel.Controls.Add(this.btnAdd);
            this.detailsPanel.Controls.Add(this.btnEdit);
            this.detailsPanel.Controls.Add(this.btnDelete);
            this.detailsPanel.Controls.Add(this.txtEventRemark);
            this.detailsPanel.Controls.Add(this.lblEventRemark);
            this.detailsPanel.Controls.Add(this.txtEventUser);
            this.detailsPanel.Controls.Add(this.lblEventUser);
            this.detailsPanel.Controls.Add(this.lblFactory);
            this.detailsPanel.Controls.Add(this.txtEquipmentLayer);
            this.detailsPanel.Controls.Add(this.lblEquipmentLayer);
            this.detailsPanel.Controls.Add(this.txtEquipmentSubType);
            this.detailsPanel.Controls.Add(this.lblEquipmentSubType);
            this.detailsPanel.Controls.Add(this.txtEquipmentDescription);
            this.detailsPanel.Controls.Add(this.lblEquipmentDescription);
            this.detailsPanel.Controls.Add(this.txtEquipmentType);
            this.detailsPanel.Controls.Add(this.lblEquipmentType);
            this.detailsPanel.Controls.Add(this.txtEquipmentName);
            this.detailsPanel.Controls.Add(this.lblEquipmentName);
            this.detailsPanel.Controls.Add(this.txtEquipmentId);
            this.detailsPanel.Controls.Add(this.lblEquipmentId);
            this.detailsPanel.Controls.Add(this.cmbFactory);
            this.detailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsPanel.Location = new System.Drawing.Point(0, 0);
            this.detailsPanel.Name = "detailsPanel";
            this.detailsPanel.Size = new System.Drawing.Size(530, 501); // Adjust based on mainSplitContainer.SplitterDistance
            this.detailsPanel.TabIndex = 0;
            this.detailsPanel.AutoScroll = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(290, 480);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(180, 480);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "确定";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtEventRemark
            // 
            this.txtEventRemark.Location = new System.Drawing.Point(130, 420);
            this.txtEventRemark.Multiline = true;
            this.txtEventRemark.Name = "txtEventRemark";
            this.txtEventRemark.ReadOnly = true;
            this.txtEventRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEventRemark.Size = new System.Drawing.Size(250, 50);
            this.txtEventRemark.TabIndex = 17;
            // 
            // lblEventRemark
            // 
            this.lblEventRemark.AutoSize = true;
            this.lblEventRemark.Location = new System.Drawing.Point(20, 423);
            this.lblEventRemark.Name = "lblEventRemark";
            this.lblEventRemark.Size = new System.Drawing.Size(70, 17);
            this.lblEventRemark.TabIndex = 16;
            this.lblEventRemark.Text = "事件备注:";
            // 
            // txtEventUser
            // 
            this.txtEventUser.Location = new System.Drawing.Point(130, 380);
            this.txtEventUser.Name = "txtEventUser";
            this.txtEventUser.ReadOnly = true;
            this.txtEventUser.Size = new System.Drawing.Size(250, 25);
            this.txtEventUser.TabIndex = 15;
            // 
            // lblEventUser
            // 
            this.lblEventUser.AutoSize = true;
            this.lblEventUser.Location = new System.Drawing.Point(20, 383);
            this.lblEventUser.Name = "lblEventUser";
            this.lblEventUser.Size = new System.Drawing.Size(70, 17);
            this.lblEventUser.TabIndex = 14;
            this.lblEventUser.Text = "事件用户:";
            // 
            // lblFactory
            // 
            this.lblFactory.AutoSize = true;
            this.lblFactory.Location = new System.Drawing.Point(20, 343);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(40, 17);
            this.lblFactory.TabIndex = 12;
            this.lblFactory.Text = "工厂:";
            // 
            // txtEquipmentLayer
            // 
            this.txtEquipmentLayer.Location = new System.Drawing.Point(130, 300);
            this.txtEquipmentLayer.Name = "txtEquipmentLayer";
            this.txtEquipmentLayer.Size = new System.Drawing.Size(250, 25);
            this.txtEquipmentLayer.TabIndex = 11;
            // 
            // lblEquipmentLayer
            // 
            this.lblEquipmentLayer.AutoSize = true;
            this.lblEquipmentLayer.Location = new System.Drawing.Point(20, 303);
            this.lblEquipmentLayer.Name = "lblEquipmentLayer";
            this.lblEquipmentLayer.Size = new System.Drawing.Size(70, 17);
            this.lblEquipmentLayer.TabIndex = 10;
            this.lblEquipmentLayer.Text = "设备层次:";
            // 
            // txtEquipmentSubType
            // 
            this.txtEquipmentSubType.Location = new System.Drawing.Point(130, 220);
            this.txtEquipmentSubType.Name = "txtEquipmentSubType";
            this.txtEquipmentSubType.Size = new System.Drawing.Size(250, 25);
            this.txtEquipmentSubType.TabIndex = 7;
            // 
            // lblEquipmentSubType
            // 
            this.lblEquipmentSubType.AutoSize = true;
            this.lblEquipmentSubType.Location = new System.Drawing.Point(20, 223);
            this.lblEquipmentSubType.Name = "lblEquipmentSubType";
            this.lblEquipmentSubType.Size = new System.Drawing.Size(95, 17);
            this.lblEquipmentSubType.TabIndex = 6;
            this.lblEquipmentSubType.Text = "设备详细类型:";
            // 
            // txtEquipmentDescription
            // 
            this.txtEquipmentDescription.Location = new System.Drawing.Point(130, 100);
            this.txtEquipmentDescription.Multiline = true;
            this.txtEquipmentDescription.Name = "txtEquipmentDescription";
            this.txtEquipmentDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEquipmentDescription.Size = new System.Drawing.Size(250, 100);
            this.txtEquipmentDescription.TabIndex = 5;
            // 
            // lblEquipmentDescription
            // 
            this.lblEquipmentDescription.AutoSize = true;
            this.lblEquipmentDescription.Location = new System.Drawing.Point(20, 103);
            this.lblEquipmentDescription.Name = "lblEquipmentDescription";
            this.lblEquipmentDescription.Size = new System.Drawing.Size(70, 17);
            this.lblEquipmentDescription.TabIndex = 4;
            this.lblEquipmentDescription.Text = "设备说明:";
            // 
            // txtEquipmentType
            // 
            this.txtEquipmentType.Location = new System.Drawing.Point(130, 60);
            this.txtEquipmentType.Name = "txtEquipmentType";
            this.txtEquipmentType.Size = new System.Drawing.Size(250, 25);
            this.txtEquipmentType.TabIndex = 3;
            // 
            // lblEquipmentType
            // 
            this.lblEquipmentType.AutoSize = true;
            this.lblEquipmentType.Location = new System.Drawing.Point(20, 63);
            this.lblEquipmentType.Name = "lblEquipmentType";
            this.lblEquipmentType.Size = new System.Drawing.Size(70, 17);
            this.lblEquipmentType.TabIndex = 2;
            this.lblEquipmentType.Text = "设备类型:";
            // 
            // txtEquipmentId
            // 
            this.txtEquipmentId.Location = new System.Drawing.Point(130, 20);
            this.txtEquipmentId.Name = "txtEquipmentId";
            this.txtEquipmentId.Size = new System.Drawing.Size(250, 25);
            this.txtEquipmentId.TabIndex = 1;
            // 
            // lblEquipmentId
            // 
            this.lblEquipmentId.AutoSize = true;
            this.lblEquipmentId.Location = new System.Drawing.Point(20, 23);
            this.lblEquipmentId.Name = "lblEquipmentId";
            this.lblEquipmentId.Size = new System.Drawing.Size(58, 17);
            this.lblEquipmentId.TabIndex = 0;
            this.lblEquipmentId.Text = "设备号:";
            // 
            // txtEquipmentName
            // 
            this.txtEquipmentName.Location = new System.Drawing.Point(130, 40);
            this.txtEquipmentName.Name = "txtEquipmentName";
            this.txtEquipmentName.Size = new System.Drawing.Size(250, 25);
            this.txtEquipmentName.TabIndex = 3;
            // 
            // lblEquipmentName
            // 
            this.lblEquipmentName.AutoSize = true;
            this.lblEquipmentName.Location = new System.Drawing.Point(20, 43);
            this.lblEquipmentName.Name = "lblEquipmentName";
            this.lblEquipmentName.Size = new System.Drawing.Size(70, 17);
            this.lblEquipmentName.TabIndex = 2;
            this.lblEquipmentName.Text = "设备名称:";
            // 
            // cmbFactory
            // 
            this.cmbFactory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.Location = new System.Drawing.Point(130, 340);
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(250, 25);
            this.cmbFactory.TabIndex = 13;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(20, 440);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 30);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(130, 440);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 30);
            this.btnEdit.TabIndex = 21;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(240, 440);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 30);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // FrmDeviceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.mainSplitContainer);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmDeviceInfo";
            this.Text = "设备信息管理";
            this.Load += new System.EventHandler(this.FrmDeviceInfo_Load);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.leftSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftSplitContainer)).EndInit();
            this.leftSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).EndInit();
            this.detailsPanel.ResumeLayout(false);
            this.detailsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer leftSplitContainer;
        private System.Windows.Forms.DataGridView dgvEquipment;
        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtEventRemark;
        private System.Windows.Forms.Label lblEventRemark;
        private System.Windows.Forms.TextBox txtEventUser;
        private System.Windows.Forms.Label lblEventUser;
        private System.Windows.Forms.Label lblFactory;
        private System.Windows.Forms.TextBox txtEquipmentLayer;
        private System.Windows.Forms.Label lblEquipmentLayer;
        private System.Windows.Forms.TextBox txtEquipmentSubType;
        private System.Windows.Forms.Label lblEquipmentSubType;
        private System.Windows.Forms.TextBox txtEquipmentDescription;
        private System.Windows.Forms.Label lblEquipmentDescription;
        private System.Windows.Forms.TextBox txtEquipmentType;
        private System.Windows.Forms.Label lblEquipmentType;
        private System.Windows.Forms.TextBox txtEquipmentId;
        private System.Windows.Forms.Label lblEquipmentId;
        private System.Windows.Forms.ComboBox cmbFactory;
        private System.Windows.Forms.TextBox txtEquipmentName;
        private System.Windows.Forms.Label lblEquipmentName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
    }
} 