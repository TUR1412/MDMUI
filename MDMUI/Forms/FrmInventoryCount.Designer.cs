using System.Drawing;
using System.Windows.Forms;

namespace MDMUI
{
    partial class FrmInventoryCount
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
            this.components = new System.ComponentModel.Container();
            
            // 初始化控件
            this.panelList = new System.Windows.Forms.Panel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            
            this.dgvInventory = new System.Windows.Forms.DataGridView();
            
            this.panelListButtons = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            
            this.panelDetail = new System.Windows.Forms.Panel();
            this.grpInventoryDetails = new System.Windows.Forms.GroupBox();
            
            this.lblInventoryId = new System.Windows.Forms.Label();
            this.txtInventoryId = new System.Windows.Forms.TextBox();
            
            this.lblProductId = new System.Windows.Forms.Label();
            this.txtProductId = new System.Windows.Forms.TextBox();
            
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            
            this.lblCountDate = new System.Windows.Forms.Label();
            this.dtpCountDate = new System.Windows.Forms.DateTimePicker();
            
            this.lblBookQuantity = new System.Windows.Forms.Label();
            this.txtBookQuantity = new System.Windows.Forms.TextBox();
            
            this.lblActualQuantity = new System.Windows.Forms.Label();
            this.txtActualQuantity = new System.Windows.Forms.TextBox();
            
            this.lblDifference = new System.Windows.Forms.Label();
            this.txtDifference = new System.Windows.Forms.TextBox();
            
            this.lblResponsiblePerson = new System.Windows.Forms.Label();
            this.txtResponsiblePerson = new System.Windows.Forms.TextBox();
            
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            
            this.panelDetailButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).BeginInit();
            this.panelList.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelListButtons.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.grpInventoryDetails.SuspendLayout();
            this.panelDetailButtons.SuspendLayout();
            this.SuspendLayout();

            // panelList
            this.panelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelList.Location = new System.Drawing.Point(0, 0);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(800, 450);
            this.panelList.TabIndex = 0;
            this.panelList.Controls.Add(this.dgvInventory);
            this.panelList.Controls.Add(this.panelSearch);
            this.panelList.Controls.Add(this.panelListButtons);

            // panelSearch
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Height = 60;
            this.panelSearch.Padding = new System.Windows.Forms.Padding(10);
            this.panelSearch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.btnRefresh);

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(15, 22);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(65, 16);
            this.lblSearch.Text = "关键字：";

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(85, 19);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 22);

            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(300, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(385, 18);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 25);
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // dgvInventory
            this.dgvInventory.AllowUserToAddRows = false;
            this.dgvInventory.AllowUserToDeleteRows = false;
            this.dgvInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInventory.BackgroundColor = System.Drawing.Color.White;
            this.dgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventory.Location = new System.Drawing.Point(10, 70);
            this.dgvInventory.MultiSelect = false;
            this.dgvInventory.Name = "dgvInventory";
            this.dgvInventory.ReadOnly = true;
            this.dgvInventory.RowHeadersWidth = 51;
            this.dgvInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInventory.Size = new System.Drawing.Size(780, 310);
            this.dgvInventory.TabIndex = 4;
            this.dgvInventory.SelectionChanged += new System.EventHandler(this.dgvInventory_SelectionChanged);

            // panelListButtons
            this.panelListButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelListButtons.Controls.Add(this.btnNew);
            this.panelListButtons.Controls.Add(this.btnEdit);
            this.panelListButtons.Controls.Add(this.btnDelete);
            this.panelListButtons.Controls.Add(this.btnExport);
            this.panelListButtons.Location = new System.Drawing.Point(10, 390);
            this.panelListButtons.Name = "panelListButtons";
            this.panelListButtons.Size = new System.Drawing.Size(780, 50);
            this.panelListButtons.TabIndex = 5;

            // btnNew
            this.btnNew.Location = new System.Drawing.Point(10, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(90, 28);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "新增盘点";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(110, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 28);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(210, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 28);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // btnExport
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(680, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 28);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);

            // panelDetail
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(0, 0);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(800, 450);
            this.panelDetail.TabIndex = 1;
            this.panelDetail.Visible = false;
            this.panelDetail.Controls.Add(this.grpInventoryDetails);
            this.panelDetail.Controls.Add(this.panelDetailButtons);

            // grpInventoryDetails
            this.grpInventoryDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInventoryDetails.Location = new System.Drawing.Point(10, 10);
            this.grpInventoryDetails.Name = "grpInventoryDetails";
            this.grpInventoryDetails.Size = new System.Drawing.Size(780, 370);
            this.grpInventoryDetails.TabIndex = 0;
            this.grpInventoryDetails.Text = "盘点信息";
            this.grpInventoryDetails.Controls.Add(this.lblInventoryId);
            this.grpInventoryDetails.Controls.Add(this.txtInventoryId);
            this.grpInventoryDetails.Controls.Add(this.lblProductId);
            this.grpInventoryDetails.Controls.Add(this.txtProductId);
            this.grpInventoryDetails.Controls.Add(this.lblProductName);
            this.grpInventoryDetails.Controls.Add(this.txtProductName);
            this.grpInventoryDetails.Controls.Add(this.lblCountDate);
            this.grpInventoryDetails.Controls.Add(this.dtpCountDate);
            this.grpInventoryDetails.Controls.Add(this.lblBookQuantity);
            this.grpInventoryDetails.Controls.Add(this.txtBookQuantity);
            this.grpInventoryDetails.Controls.Add(this.lblActualQuantity);
            this.grpInventoryDetails.Controls.Add(this.txtActualQuantity);
            this.grpInventoryDetails.Controls.Add(this.lblDifference);
            this.grpInventoryDetails.Controls.Add(this.txtDifference);
            this.grpInventoryDetails.Controls.Add(this.lblResponsiblePerson);
            this.grpInventoryDetails.Controls.Add(this.txtResponsiblePerson);
            this.grpInventoryDetails.Controls.Add(this.lblStatus);
            this.grpInventoryDetails.Controls.Add(this.cmbStatus);
            this.grpInventoryDetails.Controls.Add(this.lblRemarks);
            this.grpInventoryDetails.Controls.Add(this.txtRemarks);

            // lblInventoryId
            this.lblInventoryId.AutoSize = true;
            this.lblInventoryId.Location = new System.Drawing.Point(20, 30);
            this.lblInventoryId.Name = "lblInventoryId";
            this.lblInventoryId.Size = new System.Drawing.Size(80, 16);
            this.lblInventoryId.TabIndex = 0;
            this.lblInventoryId.Text = "盘点单号：";

            // txtInventoryId
            this.txtInventoryId.Location = new System.Drawing.Point(100, 27);
            this.txtInventoryId.Name = "txtInventoryId";
            this.txtInventoryId.Size = new System.Drawing.Size(200, 22);
            this.txtInventoryId.TabIndex = 1;

            // lblProductId
            this.lblProductId.AutoSize = true;
            this.lblProductId.Location = new System.Drawing.Point(320, 30);
            this.lblProductId.Name = "lblProductId";
            this.lblProductId.Size = new System.Drawing.Size(80, 16);
            this.lblProductId.TabIndex = 2;
            this.lblProductId.Text = "产品编号：";

            // txtProductId
            this.txtProductId.Location = new System.Drawing.Point(400, 27);
            this.txtProductId.Name = "txtProductId";
            this.txtProductId.Size = new System.Drawing.Size(150, 22);
            this.txtProductId.TabIndex = 3;

            // lblProductName
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(20, 60);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(80, 16);
            this.lblProductName.TabIndex = 4;
            this.lblProductName.Text = "产品名称：";

            // txtProductName
            this.txtProductName.Location = new System.Drawing.Point(100, 57);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 22);
            this.txtProductName.TabIndex = 5;

            // lblCountDate
            this.lblCountDate.AutoSize = true;
            this.lblCountDate.Location = new System.Drawing.Point(320, 60);
            this.lblCountDate.Name = "lblCountDate";
            this.lblCountDate.Size = new System.Drawing.Size(80, 16);
            this.lblCountDate.TabIndex = 6;
            this.lblCountDate.Text = "盘点日期：";

            // dtpCountDate
            this.dtpCountDate.Location = new System.Drawing.Point(400, 57);
            this.dtpCountDate.Name = "dtpCountDate";
            this.dtpCountDate.Size = new System.Drawing.Size(150, 22);
            this.dtpCountDate.TabIndex = 7;
            this.dtpCountDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // lblBookQuantity
            this.lblBookQuantity.AutoSize = true;
            this.lblBookQuantity.Location = new System.Drawing.Point(20, 90);
            this.lblBookQuantity.Name = "lblBookQuantity";
            this.lblBookQuantity.Size = new System.Drawing.Size(80, 16);
            this.lblBookQuantity.TabIndex = 8;
            this.lblBookQuantity.Text = "账面数量：";

            // txtBookQuantity
            this.txtBookQuantity.Location = new System.Drawing.Point(100, 87);
            this.txtBookQuantity.Name = "txtBookQuantity";
            this.txtBookQuantity.Size = new System.Drawing.Size(100, 22);
            this.txtBookQuantity.TabIndex = 9;
            this.txtBookQuantity.TextChanged += new System.EventHandler(this.txtBookQuantity_TextChanged);

            // lblActualQuantity
            this.lblActualQuantity.AutoSize = true;
            this.lblActualQuantity.Location = new System.Drawing.Point(205, 90);
            this.lblActualQuantity.Name = "lblActualQuantity";
            this.lblActualQuantity.Size = new System.Drawing.Size(80, 16);
            this.lblActualQuantity.TabIndex = 10;
            this.lblActualQuantity.Text = "实际数量：";

            // txtActualQuantity
            this.txtActualQuantity.Location = new System.Drawing.Point(285, 87);
            this.txtActualQuantity.Name = "txtActualQuantity";
            this.txtActualQuantity.Size = new System.Drawing.Size(100, 22);
            this.txtActualQuantity.TabIndex = 11;
            this.txtActualQuantity.TextChanged += new System.EventHandler(this.txtActualQuantity_TextChanged);

            // lblDifference
            this.lblDifference.AutoSize = true;
            this.lblDifference.Location = new System.Drawing.Point(390, 90);
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.Size = new System.Drawing.Size(80, 16);
            this.lblDifference.TabIndex = 12;
            this.lblDifference.Text = "差异数量：";

            // txtDifference
            this.txtDifference.Location = new System.Drawing.Point(470, 87);
            this.txtDifference.Name = "txtDifference";
            this.txtDifference.Size = new System.Drawing.Size(100, 22);
            this.txtDifference.TabIndex = 13;
            this.txtDifference.ReadOnly = true;

            // lblResponsiblePerson
            this.lblResponsiblePerson.AutoSize = true;
            this.lblResponsiblePerson.Location = new System.Drawing.Point(20, 120);
            this.lblResponsiblePerson.Name = "lblResponsiblePerson";
            this.lblResponsiblePerson.Size = new System.Drawing.Size(80, 16);
            this.lblResponsiblePerson.TabIndex = 14;
            this.lblResponsiblePerson.Text = "责任人：";

            // txtResponsiblePerson
            this.txtResponsiblePerson.Location = new System.Drawing.Point(100, 117);
            this.txtResponsiblePerson.Name = "txtResponsiblePerson";
            this.txtResponsiblePerson.Size = new System.Drawing.Size(200, 22);
            this.txtResponsiblePerson.TabIndex = 15;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(320, 120);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(80, 16);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "状态：";

            // cmbStatus
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "待确认",
            "已确认",
            "已处理"});
            this.cmbStatus.Location = new System.Drawing.Point(400, 117);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(150, 24);
            this.cmbStatus.TabIndex = 17;

            // lblRemarks
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(20, 150);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(80, 16);
            this.lblRemarks.TabIndex = 18;
            this.lblRemarks.Text = "备注：";

            // txtRemarks
            this.txtRemarks.Location = new System.Drawing.Point(100, 147);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(450, 70);
            this.txtRemarks.TabIndex = 19;

            // panelDetailButtons
            this.panelDetailButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetailButtons.Controls.Add(this.btnSave);
            this.panelDetailButtons.Controls.Add(this.btnCancel);
            this.panelDetailButtons.Location = new System.Drawing.Point(10, 390);
            this.panelDetailButtons.Name = "panelDetailButtons";
            this.panelDetailButtons.Size = new System.Drawing.Size(780, 50);
            this.panelDetailButtons.TabIndex = 1;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(100, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(200, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // FrmInventoryCount
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelList);
            this.Controls.Add(this.panelDetail);
            this.Name = "FrmInventoryCount";
            this.Text = "库存盘点";
            this.Load += new System.EventHandler(this.FrmInventoryCount_Load);
            
            this.panelList.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelListButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventory)).EndInit();
            this.panelDetail.ResumeLayout(false);
            this.grpInventoryDetails.ResumeLayout(false);
            this.grpInventoryDetails.PerformLayout();
            this.panelDetailButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvInventory;
        private System.Windows.Forms.Panel panelListButtons;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExport;

        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.GroupBox grpInventoryDetails;
        private System.Windows.Forms.Label lblInventoryId;
        private System.Windows.Forms.TextBox txtInventoryId;
        private System.Windows.Forms.Label lblProductId;
        private System.Windows.Forms.TextBox txtProductId;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblCountDate;
        private System.Windows.Forms.DateTimePicker dtpCountDate;
        private System.Windows.Forms.Label lblBookQuantity;
        private System.Windows.Forms.TextBox txtBookQuantity;
        private System.Windows.Forms.Label lblActualQuantity;
        private System.Windows.Forms.TextBox txtActualQuantity;
        private System.Windows.Forms.Label lblDifference;
        private System.Windows.Forms.TextBox txtDifference;
        private System.Windows.Forms.Label lblResponsiblePerson;
        private System.Windows.Forms.TextBox txtResponsiblePerson;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Panel panelDetailButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
} 