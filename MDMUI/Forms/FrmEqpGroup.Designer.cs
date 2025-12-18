using System.Drawing;
using System.Windows.Forms;

namespace MDMUI
{
    partial class FrmEqpGroup
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
            this.dgvEqpGroup = new System.Windows.Forms.DataGridView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtGroupIdSearch = new System.Windows.Forms.TextBox();
            this.lblGroupIdSearch = new System.Windows.Forms.Label();
            this.cmbGroupTypeFilter = new System.Windows.Forms.ComboBox();
            this.lblGroupTypeFilter = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelSubDeviceArea = new System.Windows.Forms.Panel();
            this.dgvSubEquipment = new System.Windows.Forms.DataGridView();
            this.panelSubDeviceTop = new System.Windows.Forms.Panel();
            this.lblSubDevice = new System.Windows.Forms.Label();
            this.btnDeleteSubDevice = new System.Windows.Forms.Button();
            this.btnEditSubDevice = new System.Windows.Forms.Button();
            this.btnAddSubDevice = new System.Windows.Forms.Button();
            this.panelPortsArea = new System.Windows.Forms.Panel();
            this.dgvPorts = new System.Windows.Forms.DataGridView();
            this.panelPortsTop = new System.Windows.Forms.Panel();
            this.lblPorts = new System.Windows.Forms.Label();
            this.btnDeletePort = new System.Windows.Forms.Button();
            this.btnEditPort = new System.Windows.Forms.Button();
            this.btnAddPort = new System.Windows.Forms.Button();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.filterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.devicesPanel = new System.Windows.Forms.Panel();
            this.bottomSplitContainer = new System.Windows.Forms.SplitContainer();
            this.subDevicesPanel = new System.Windows.Forms.Panel();
            this.subDevicesTitlePanel = new System.Windows.Forms.TableLayoutPanel();
            this.portsPanel = new System.Windows.Forms.Panel();
            this.portsTitlePanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEqpGroup)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelSubDeviceArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubEquipment)).BeginInit();
            this.panelSubDeviceTop.SuspendLayout();
            this.panelPortsArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorts)).BeginInit();
            this.panelPortsTop.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.devicesPanel.SuspendLayout();
            this.subDevicesPanel.SuspendLayout();
            this.subDevicesTitlePanel.SuspendLayout();
            this.portsPanel.SuspendLayout();
            this.portsTitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvEqpGroup
            // 
            this.dgvEqpGroup.AllowUserToAddRows = false;
            this.dgvEqpGroup.AllowUserToDeleteRows = false;
            this.dgvEqpGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEqpGroup.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEqpGroup.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvEqpGroup.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvEqpGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEqpGroup.Location = new System.Drawing.Point(12, 68);
            this.dgvEqpGroup.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.dgvEqpGroup.MultiSelect = false;
            this.dgvEqpGroup.Name = "dgvEqpGroup";
            this.dgvEqpGroup.ReadOnly = true;
            this.dgvEqpGroup.RowHeadersVisible = false;
            this.dgvEqpGroup.RowHeadersWidth = 51;
            this.dgvEqpGroup.RowTemplate.Height = 27;
            this.dgvEqpGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEqpGroup.Size = new System.Drawing.Size(776, 210);
            this.dgvEqpGroup.TabIndex = 1;
            this.dgvEqpGroup.SelectionChanged += new System.EventHandler(this.dgvEqpGroup_SelectionChanged);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Controls.Add(this.btnDelete);
            this.panelTop.Controls.Add(this.btnEdit);
            this.panelTop.Controls.Add(this.btnAdd);
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.txtGroupIdSearch);
            this.panelTop.Controls.Add(this.lblGroupIdSearch);
            this.panelTop.Controls.Add(this.cmbGroupTypeFilter);
            this.panelTop.Controls.Add(this.lblGroupTypeFilter);
            this.panelTop.Location = new System.Drawing.Point(12, 12);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(5);
            this.panelTop.Size = new System.Drawing.Size(776, 50);
            this.panelTop.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(693, 14);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 36);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(630, 14);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 36);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(567, 14);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(69, 36);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(497, 14);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(73, 36);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(420, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(71, 36);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtGroupIdSearch
            // 
            this.txtGroupIdSearch.Location = new System.Drawing.Point(317, 15);
            this.txtGroupIdSearch.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.txtGroupIdSearch.Name = "txtGroupIdSearch";
            this.txtGroupIdSearch.Size = new System.Drawing.Size(100, 31);
            this.txtGroupIdSearch.TabIndex = 3;
            this.txtGroupIdSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGroupIdSearch_KeyDown);
            // 
            // lblGroupIdSearch
            // 
            this.lblGroupIdSearch.AutoSize = true;
            this.lblGroupIdSearch.Location = new System.Drawing.Point(242, 18);
            this.lblGroupIdSearch.Name = "lblGroupIdSearch";
            this.lblGroupIdSearch.Size = new System.Drawing.Size(104, 24);
            this.lblGroupIdSearch.TabIndex = 2;
            this.lblGroupIdSearch.Text = "设备组编号:";
            // 
            // cmbGroupTypeFilter
            // 
            this.cmbGroupTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupTypeFilter.FormattingEnabled = true;
            this.cmbGroupTypeFilter.Location = new System.Drawing.Point(88, 15);
            this.cmbGroupTypeFilter.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.cmbGroupTypeFilter.Name = "cmbGroupTypeFilter";
            this.cmbGroupTypeFilter.Size = new System.Drawing.Size(140, 32);
            this.cmbGroupTypeFilter.TabIndex = 1;
            this.cmbGroupTypeFilter.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            // 
            // lblGroupTypeFilter
            // 
            this.lblGroupTypeFilter.AutoSize = true;
            this.lblGroupTypeFilter.Location = new System.Drawing.Point(10, 18);
            this.lblGroupTypeFilter.Name = "lblGroupTypeFilter";
            this.lblGroupTypeFilter.Size = new System.Drawing.Size(104, 24);
            this.lblGroupTypeFilter.TabIndex = 0;
            this.lblGroupTypeFilter.Text = "设备组类型:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 288);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelSubDeviceArea);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelPortsArea);
            this.splitContainer1.Panel2MinSize = 50;
            this.splitContainer1.Size = new System.Drawing.Size(776, 250);
            this.splitContainer1.TabIndex = 2;
            // 
            // panelSubDeviceArea
            // 
            this.panelSubDeviceArea.Controls.Add(this.dgvSubEquipment);
            this.panelSubDeviceArea.Controls.Add(this.panelSubDeviceTop);
            this.panelSubDeviceArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSubDeviceArea.Location = new System.Drawing.Point(0, 0);
            this.panelSubDeviceArea.Name = "panelSubDeviceArea";
            this.panelSubDeviceArea.Size = new System.Drawing.Size(350, 250);
            this.panelSubDeviceArea.TabIndex = 0;
            // 
            // dgvSubEquipment
            // 
            this.dgvSubEquipment.AllowUserToAddRows = false;
            this.dgvSubEquipment.AllowUserToDeleteRows = false;
            this.dgvSubEquipment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSubEquipment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSubEquipment.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvSubEquipment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSubEquipment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubEquipment.Location = new System.Drawing.Point(0, 40);
            this.dgvSubEquipment.MultiSelect = false;
            this.dgvSubEquipment.Name = "dgvSubEquipment";
            this.dgvSubEquipment.ReadOnly = true;
            this.dgvSubEquipment.RowHeadersVisible = false;
            this.dgvSubEquipment.RowHeadersWidth = 51;
            this.dgvSubEquipment.RowTemplate.Height = 27;
            this.dgvSubEquipment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubEquipment.Size = new System.Drawing.Size(350, 210);
            this.dgvSubEquipment.TabIndex = 1;
            // 
            // panelSubDeviceTop
            // 
            this.panelSubDeviceTop.Controls.Add(this.lblSubDevice);
            this.panelSubDeviceTop.Controls.Add(this.btnDeleteSubDevice);
            this.panelSubDeviceTop.Controls.Add(this.btnEditSubDevice);
            this.panelSubDeviceTop.Controls.Add(this.btnAddSubDevice);
            this.panelSubDeviceTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSubDeviceTop.Location = new System.Drawing.Point(0, 0);
            this.panelSubDeviceTop.Name = "panelSubDeviceTop";
            this.panelSubDeviceTop.Size = new System.Drawing.Size(350, 40);
            this.panelSubDeviceTop.TabIndex = 0;
            // 
            // lblSubDevice
            // 
            this.lblSubDevice.AutoSize = true;
            this.lblSubDevice.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSubDevice.Location = new System.Drawing.Point(3, 10);
            this.lblSubDevice.Name = "lblSubDevice";
            this.lblSubDevice.Size = new System.Drawing.Size(84, 25);
            this.lblSubDevice.TabIndex = 3;
            this.lblSubDevice.Text = "子设备：";
            // 
            // btnDeleteSubDevice
            // 
            this.btnDeleteSubDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSubDevice.Location = new System.Drawing.Point(315, 5);
            this.btnDeleteSubDevice.Name = "btnDeleteSubDevice";
            this.btnDeleteSubDevice.Size = new System.Drawing.Size(30, 30);
            this.btnDeleteSubDevice.TabIndex = 2;
            this.btnDeleteSubDevice.Text = "-";
            this.btnDeleteSubDevice.UseVisualStyleBackColor = true;
            this.btnDeleteSubDevice.Click += new System.EventHandler(this.btnDeleteSubDevice_Click);
            // 
            // btnEditSubDevice
            // 
            this.btnEditSubDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditSubDevice.Location = new System.Drawing.Point(285, 5);
            this.btnEditSubDevice.Name = "btnEditSubDevice";
            this.btnEditSubDevice.Size = new System.Drawing.Size(30, 30);
            this.btnEditSubDevice.TabIndex = 1;
            this.btnEditSubDevice.Text = "✎";
            this.btnEditSubDevice.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnEditSubDevice.Click += new System.EventHandler(this.btnEditSubDevice_Click);
            // 
            // btnAddSubDevice
            // 
            this.btnAddSubDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSubDevice.Location = new System.Drawing.Point(255, 5);
            this.btnAddSubDevice.Name = "btnAddSubDevice";
            this.btnAddSubDevice.Size = new System.Drawing.Size(30, 30);
            this.btnAddSubDevice.TabIndex = 0;
            this.btnAddSubDevice.Text = "+";
            this.btnAddSubDevice.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnAddSubDevice.Click += new System.EventHandler(this.btnAddSubDevice_Click);
            // 
            // panelPortsArea
            // 
            this.panelPortsArea.Controls.Add(this.dgvPorts);
            this.panelPortsArea.Controls.Add(this.panelPortsTop);
            this.panelPortsArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPortsArea.Location = new System.Drawing.Point(0, 0);
            this.panelPortsArea.Name = "panelPortsArea";
            this.panelPortsArea.Size = new System.Drawing.Size(272, 250);
            this.panelPortsArea.TabIndex = 0;
            // 
            // dgvPorts
            // 
            this.dgvPorts.AllowUserToAddRows = false;
            this.dgvPorts.AllowUserToDeleteRows = false;
            this.dgvPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPorts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPorts.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPorts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPorts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPorts.Location = new System.Drawing.Point(0, 40);
            this.dgvPorts.MultiSelect = false;
            this.dgvPorts.Name = "dgvPorts";
            this.dgvPorts.ReadOnly = true;
            this.dgvPorts.RowHeadersVisible = false;
            this.dgvPorts.RowHeadersWidth = 51;
            this.dgvPorts.RowTemplate.Height = 27;
            this.dgvPorts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPorts.Size = new System.Drawing.Size(272, 210);
            this.dgvPorts.TabIndex = 1;
            // 
            // panelPortsTop
            // 
            this.panelPortsTop.Controls.Add(this.lblPorts);
            this.panelPortsTop.Controls.Add(this.btnDeletePort);
            this.panelPortsTop.Controls.Add(this.btnEditPort);
            this.panelPortsTop.Controls.Add(this.btnAddPort);
            this.panelPortsTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPortsTop.Location = new System.Drawing.Point(0, 0);
            this.panelPortsTop.Name = "panelPortsTop";
            this.panelPortsTop.Size = new System.Drawing.Size(272, 40);
            this.panelPortsTop.TabIndex = 0;
            // 
            // lblPorts
            // 
            this.lblPorts.AutoSize = true;
            this.lblPorts.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPorts.Location = new System.Drawing.Point(3, 10);
            this.lblPorts.Name = "lblPorts";
            this.lblPorts.Size = new System.Drawing.Size(84, 25);
            this.lblPorts.TabIndex = 3;
            this.lblPorts.Text = "端口：";
            // 
            // btnDeletePort
            // 
            this.btnDeletePort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePort.Location = new System.Drawing.Point(237, 5);
            this.btnDeletePort.Name = "btnDeletePort";
            this.btnDeletePort.Size = new System.Drawing.Size(30, 30);
            this.btnDeletePort.TabIndex = 2;
            this.btnDeletePort.Text = "-";
            this.btnDeletePort.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnDeletePort.Click += new System.EventHandler(this.btnDeletePort_Click);
            // 
            // btnEditPort
            // 
            this.btnEditPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditPort.Location = new System.Drawing.Point(207, 5);
            this.btnEditPort.Name = "btnEditPort";
            this.btnEditPort.Size = new System.Drawing.Size(30, 30);
            this.btnEditPort.TabIndex = 1;
            this.btnEditPort.Text = "✎";
            this.btnEditPort.UseVisualStyleBackColor = true;
            this.btnEditPort.Click += new System.EventHandler(this.btnEditPort_Click);
            // 
            // btnAddPort
            // 
            this.btnAddPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPort.Location = new System.Drawing.Point(177, 5);
            this.btnAddPort.Name = "btnAddPort";
            this.btnAddPort.Size = new System.Drawing.Size(30, 30);
            this.btnAddPort.TabIndex = 0;
            this.btnAddPort.Text = "+";
            this.btnAddPort.UseVisualStyleBackColor = true;
            this.btnAddPort.Click += new System.EventHandler(this.btnAddPort_Click);
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.filterPanel, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.devicesPanel, 0, 1);
            this.mainTableLayoutPanel.Controls.Add(this.bottomSplitContainer, 0, 2);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainTableLayoutPanel.RowCount = 3;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(1000, 700);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // filterPanel
            // 
            this.filterPanel.ColumnCount = 9;
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.filterPanel.Controls.Add(this.lblGroupTypeFilter, 0, 0);
            this.filterPanel.Controls.Add(this.cmbGroupTypeFilter, 1, 0);
            this.filterPanel.Controls.Add(this.lblGroupIdSearch, 3, 0);
            this.filterPanel.Controls.Add(this.txtGroupIdSearch, 4, 0);
            this.filterPanel.Controls.Add(this.btnSearch, 5, 0);
            this.filterPanel.Controls.Add(this.btnAdd, 7, 0);
            this.filterPanel.Controls.Add(this.btnEdit, 8, 0);
            this.filterPanel.Controls.Add(this.btnDelete, 9, 0);
            this.filterPanel.Controls.Add(this.btnRefresh, 10, 0);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterPanel.Location = new System.Drawing.Point(13, 13);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.filterPanel.RowCount = 1;
            this.filterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.filterPanel.Size = new System.Drawing.Size(974, 54);
            this.filterPanel.TabIndex = 0;
            // 
            // devicesPanel
            // 
            this.devicesPanel.Controls.Add(this.dgvEqpGroup);
            this.devicesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.devicesPanel.Location = new System.Drawing.Point(13, 73);
            this.devicesPanel.Name = "devicesPanel";
            this.devicesPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.devicesPanel.Size = new System.Drawing.Size(974, 236);
            this.devicesPanel.TabIndex = 1;
            // 
            // bottomSplitContainer
            // 
            this.bottomSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomSplitContainer.Location = new System.Drawing.Point(13, 315);
            this.bottomSplitContainer.Name = "bottomSplitContainer";
            // 
            // bottomSplitContainer.Panel1
            // 
            this.bottomSplitContainer.Panel1.Controls.Add(this.subDevicesPanel);
            this.bottomSplitContainer.Panel1MinSize = 50;
            // 
            // bottomSplitContainer.Panel2
            // 
            this.bottomSplitContainer.Panel2.Controls.Add(this.portsPanel);
            this.bottomSplitContainer.Panel2MinSize = 50;
            this.bottomSplitContainer.Size = new System.Drawing.Size(974, 372);
            this.bottomSplitContainer.TabIndex = 2;
            // 
            // subDevicesPanel
            // 
            this.subDevicesPanel.Controls.Add(this.dgvSubEquipment);
            this.subDevicesPanel.Controls.Add(this.subDevicesTitlePanel);
            this.subDevicesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subDevicesPanel.Location = new System.Drawing.Point(0, 0);
            this.subDevicesPanel.Name = "subDevicesPanel";
            this.subDevicesPanel.Size = new System.Drawing.Size(400, 372);
            this.subDevicesPanel.TabIndex = 0;
            // 
            // subDevicesTitlePanel
            // 
            this.subDevicesTitlePanel.ColumnCount = 5;
            this.subDevicesTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.subDevicesTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.subDevicesTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.subDevicesTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.subDevicesTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.subDevicesTitlePanel.Controls.Add(this.lblSubDevice, 0, 0);
            this.subDevicesTitlePanel.Controls.Add(this.btnAddSubDevice, 1, 0);
            this.subDevicesTitlePanel.Controls.Add(this.btnEditSubDevice, 2, 0);
            this.subDevicesTitlePanel.Controls.Add(this.btnDeleteSubDevice, 3, 0);
            this.subDevicesTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.subDevicesTitlePanel.Location = new System.Drawing.Point(0, 0);
            this.subDevicesTitlePanel.Name = "subDevicesTitlePanel";
            this.subDevicesTitlePanel.RowCount = 1;
            this.subDevicesTitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.subDevicesTitlePanel.Size = new System.Drawing.Size(400, 40);
            this.subDevicesTitlePanel.TabIndex = 0;
            // 
            // lblSubDevice
            // 
            this.lblSubDevice.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSubDevice.AutoSize = true;
            this.lblSubDevice.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubDevice.Location = new System.Drawing.Point(3, 8);
            this.lblSubDevice.Name = "lblSubDevice";
            this.lblSubDevice.Size = new System.Drawing.Size(61, 24);
            this.lblSubDevice.TabIndex = 0;
            this.lblSubDevice.Text = "子设备:";
            // 
            // btnAddSubDevice
            // 
            this.btnAddSubDevice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddSubDevice.Location = new System.Drawing.Point(83, 4);
            this.btnAddSubDevice.Name = "btnAddSubDevice";
            this.btnAddSubDevice.Size = new System.Drawing.Size(64, 32);
            this.btnAddSubDevice.TabIndex = 1;
            this.btnAddSubDevice.Text = "添加";
            this.btnAddSubDevice.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnAddSubDevice.Click += new System.EventHandler(this.btnAddSubDevice_Click);
            // 
            // btnEditSubDevice
            // 
            this.btnEditSubDevice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEditSubDevice.Location = new System.Drawing.Point(153, 4);
            this.btnEditSubDevice.Name = "btnEditSubDevice";
            this.btnEditSubDevice.Size = new System.Drawing.Size(64, 32);
            this.btnEditSubDevice.TabIndex = 2;
            this.btnEditSubDevice.Text = "编辑";
            this.btnEditSubDevice.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnEditSubDevice.Click += new System.EventHandler(this.btnEditSubDevice_Click);
            // 
            // btnDeleteSubDevice
            // 
            this.btnDeleteSubDevice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteSubDevice.Location = new System.Drawing.Point(223, 4);
            this.btnDeleteSubDevice.Name = "btnDeleteSubDevice";
            this.btnDeleteSubDevice.Size = new System.Drawing.Size(64, 32);
            this.btnDeleteSubDevice.TabIndex = 3;
            this.btnDeleteSubDevice.Text = "删除";
            this.btnDeleteSubDevice.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnDeleteSubDevice.Click += new System.EventHandler(this.btnDeleteSubDevice_Click);
            // 
            // dgvSubEquipment
            // 
            this.dgvSubEquipment.AllowUserToAddRows = false;
            this.dgvSubEquipment.AllowUserToDeleteRows = false;
            this.dgvSubEquipment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSubEquipment.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvSubEquipment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSubEquipment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSubEquipment.Location = new System.Drawing.Point(0, 40);
            this.dgvSubEquipment.MultiSelect = false;
            this.dgvSubEquipment.Name = "dgvSubEquipment";
            this.dgvSubEquipment.ReadOnly = true;
            this.dgvSubEquipment.RowHeadersVisible = false;
            this.dgvSubEquipment.RowHeadersWidth = 51;
            this.dgvSubEquipment.RowTemplate.Height = 27;
            this.dgvSubEquipment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubEquipment.Size = new System.Drawing.Size(400, 332);
            this.dgvSubEquipment.TabIndex = 1;
            // 
            // portsPanel
            // 
            this.portsPanel.Controls.Add(this.dgvPorts);
            this.portsPanel.Controls.Add(this.portsTitlePanel);
            this.portsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.portsPanel.Location = new System.Drawing.Point(0, 0);
            this.portsPanel.Name = "portsPanel";
            this.portsPanel.Size = new System.Drawing.Size(370, 372);
            this.portsPanel.TabIndex = 0;
            // 
            // portsTitlePanel
            // 
            this.portsTitlePanel.ColumnCount = 5;
            this.portsTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.portsTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.portsTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.portsTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.portsTitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.portsTitlePanel.Controls.Add(this.lblPorts, 0, 0);
            this.portsTitlePanel.Controls.Add(this.btnAddPort, 1, 0);
            this.portsTitlePanel.Controls.Add(this.btnEditPort, 2, 0);
            this.portsTitlePanel.Controls.Add(this.btnDeletePort, 3, 0);
            this.portsTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.portsTitlePanel.Location = new System.Drawing.Point(0, 0);
            this.portsTitlePanel.Name = "portsTitlePanel";
            this.portsTitlePanel.RowCount = 1;
            this.portsTitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.portsTitlePanel.Size = new System.Drawing.Size(370, 40);
            this.portsTitlePanel.TabIndex = 0;
            // 
            // lblPorts
            // 
            this.lblPorts.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPorts.AutoSize = true;
            this.lblPorts.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPorts.Location = new System.Drawing.Point(3, 8);
            this.lblPorts.Name = "lblPorts";
            this.lblPorts.Size = new System.Drawing.Size(47, 24);
            this.lblPorts.TabIndex = 0;
            this.lblPorts.Text = "端口:";
            // 
            // btnAddPort
            // 
            this.btnAddPort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddPort.Location = new System.Drawing.Point(83, 4);
            this.btnAddPort.Name = "btnAddPort";
            this.btnAddPort.Size = new System.Drawing.Size(64, 32);
            this.btnAddPort.TabIndex = 1;
            this.btnAddPort.Text = "添加";
            this.btnAddPort.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnAddPort.Click += new System.EventHandler(this.btnAddPort_Click);
            // 
            // btnEditPort
            // 
            this.btnEditPort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEditPort.Location = new System.Drawing.Point(153, 4);
            this.btnEditPort.Name = "btnEditPort";
            this.btnEditPort.Size = new System.Drawing.Size(64, 32);
            this.btnEditPort.TabIndex = 2;
            this.btnEditPort.Text = "编辑";
            this.btnEditPort.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnEditPort.Click += new System.EventHandler(this.btnEditPort_Click);
            // 
            // btnDeletePort
            // 
            this.btnDeletePort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeletePort.Location = new System.Drawing.Point(223, 4);
            this.btnDeletePort.Name = "btnDeletePort";
            this.btnDeletePort.Size = new System.Drawing.Size(64, 32);
            this.btnDeletePort.TabIndex = 3;
            this.btnDeletePort.Text = "删除";
            this.btnDeletePort.UseVisualStyleBackColor = true;
            // 设计器中的事件绑定被移除，在代码中手动绑定
            // this.btnDeletePort.Click += new System.EventHandler(this.btnDeletePort_Click);
            // 
            // dgvPorts
            // 
            this.dgvPorts.AllowUserToAddRows = false;
            this.dgvPorts.AllowUserToDeleteRows = false;
            this.dgvPorts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPorts.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvPorts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPorts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPorts.Location = new System.Drawing.Point(0, 40);
            this.dgvPorts.MultiSelect = false;
            this.dgvPorts.Name = "dgvPorts";
            this.dgvPorts.ReadOnly = true;
            this.dgvPorts.RowHeadersVisible = false;
            this.dgvPorts.RowHeadersWidth = 51;
            this.dgvPorts.RowTemplate.Height = 27;
            this.dgvPorts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPorts.Size = new System.Drawing.Size(370, 332);
            this.dgvPorts.TabIndex = 1;
            // 
            // FrmEqpGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmEqpGroup";
            this.Text = "设备组管理";
            this.Load += new System.EventHandler(this.FrmEqpGroup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEqpGroup)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelSubDeviceArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubEquipment)).EndInit();
            this.panelSubDeviceTop.ResumeLayout(false);
            this.panelSubDeviceTop.PerformLayout();
            this.panelPortsArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorts)).EndInit();
            this.panelPortsTop.ResumeLayout(false);
            this.panelPortsTop.PerformLayout();
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            this.devicesPanel.ResumeLayout(false);
            this.subDevicesPanel.ResumeLayout(false);
            this.subDevicesTitlePanel.ResumeLayout(false);
            this.subDevicesTitlePanel.PerformLayout();
            this.portsPanel.ResumeLayout(false);
            this.portsTitlePanel.ResumeLayout(false);
            this.portsTitlePanel.PerformLayout();
            this.bottomSplitContainer.Panel1.ResumeLayout(false);
            this.bottomSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bottomSplitContainer)).EndInit();
            this.bottomSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.DataGridView dgvEqpGroup;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox txtGroupIdSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblGroupIdSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cmbGroupTypeFilter;
        private System.Windows.Forms.Label lblGroupTypeFilter;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelSubDeviceArea;
        private System.Windows.Forms.Panel panelSubDeviceTop;
        private System.Windows.Forms.Button btnDeleteSubDevice;
        private System.Windows.Forms.Button btnEditSubDevice;
        private System.Windows.Forms.Button btnAddSubDevice;
        private System.Windows.Forms.Panel panelPortsArea;
        private System.Windows.Forms.Panel panelPortsTop;
        private System.Windows.Forms.Button btnDeletePort;
        private System.Windows.Forms.Button btnEditPort;
        private System.Windows.Forms.Button btnAddPort;
        private System.Windows.Forms.DataGridView dgvSubEquipment;
        private System.Windows.Forms.DataGridView dgvPorts;
        private System.Windows.Forms.Label lblSubDevice;
        private System.Windows.Forms.Label lblPorts;
        
        // 新增的布局控件
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel filterPanel;
        private System.Windows.Forms.Panel devicesPanel;
        private System.Windows.Forms.SplitContainer bottomSplitContainer;
        private System.Windows.Forms.Panel subDevicesPanel;
        private System.Windows.Forms.TableLayoutPanel subDevicesTitlePanel;
        private System.Windows.Forms.Panel portsPanel;
        private System.Windows.Forms.TableLayoutPanel portsTitlePanel;
    }
} 