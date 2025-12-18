namespace MDMUI.Forms
{
    partial class NewDeviceForm
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
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbDeviceGroup = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.dgvDevices = new System.Windows.Forms.DataGridView();
            this.splitContainerSub = new System.Windows.Forms.SplitContainer();
            this.dgvSubDevices = new System.Windows.Forms.DataGridView();
            this.dgvPorts = new System.Windows.Forms.DataGridView();
            this.tableMain.SuspendLayout();
            this.panelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSub)).BeginInit();
            this.splitContainerSub.Panel1.SuspendLayout();
            this.splitContainerSub.Panel2.SuspendLayout();
            this.splitContainerSub.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorts)).BeginInit();
            this.SuspendLayout();
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 1;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Controls.Add(this.panelFilter, 0, 0);
            this.tableMain.Controls.Add(this.splitContainerMain, 0, 1);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 2;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.Size = new System.Drawing.Size(984, 761);
            this.tableMain.TabIndex = 0;
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.btnSearch);
            this.panelFilter.Controls.Add(this.cmbDeviceGroup);
            this.panelFilter.Controls.Add(this.label2);
            this.panelFilter.Controls.Add(this.cmbDeviceType);
            this.panelFilter.Controls.Add(this.label1);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFilter.Location = new System.Drawing.Point(3, 3);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(978, 54);
            this.panelFilter.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(439, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbDeviceGroup
            // 
            this.cmbDeviceGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeviceGroup.FormattingEnabled = true;
            this.cmbDeviceGroup.Location = new System.Drawing.Point(301, 15);
            this.cmbDeviceGroup.Name = "cmbDeviceGroup";
            this.cmbDeviceGroup.Size = new System.Drawing.Size(121, 20);
            this.cmbDeviceGroup.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(229, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "设备组：";
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeviceType.FormattingEnabled = true;
            this.cmbDeviceType.Location = new System.Drawing.Point(87, 15);
            this.cmbDeviceType.Name = "cmbDeviceType";
            this.cmbDeviceType.Size = new System.Drawing.Size(121, 20);
            this.cmbDeviceType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备类型：";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(3, 63);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.dgvDevices);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerSub);
            this.splitContainerMain.Size = new System.Drawing.Size(978, 695);
            this.splitContainerMain.SplitterDistance = 400;
            this.splitContainerMain.TabIndex = 1;
            // 
            // dgvDevices
            // 
            this.dgvDevices.AllowUserToAddRows = false;
            this.dgvDevices.AllowUserToDeleteRows = false;
            this.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevices.Location = new System.Drawing.Point(0, 0);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.ReadOnly = true;
            this.dgvDevices.RowTemplate.Height = 23;
            this.dgvDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevices.Size = new System.Drawing.Size(978, 400);
            this.dgvDevices.TabIndex = 0;
            this.dgvDevices.SelectionChanged += new System.EventHandler(this.dgvDevices_SelectionChanged);
            // 
            // splitContainerSub
            // 
            this.splitContainerSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSub.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSub.Name = "splitContainerSub";
            // 
            // splitContainerSub.Panel1
            // 
            this.splitContainerSub.Panel1.Controls.Add(this.dgvSubDevices);
            // 
            // splitContainerSub.Panel2
            // 
            this.splitContainerSub.Panel2.Controls.Add(this.dgvPorts);
            this.splitContainerSub.Size = new System.Drawing.Size(978, 291);
            this.splitContainerSub.SplitterDistance = 489;
            this.splitContainerSub.TabIndex = 0;
            // 
            // dgvSubDevices
            // 
            this.dgvSubDevices.AllowUserToAddRows = false;
            this.dgvSubDevices.AllowUserToDeleteRows = false;
            this.dgvSubDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSubDevices.Location = new System.Drawing.Point(0, 0);
            this.dgvSubDevices.Name = "dgvSubDevices";
            this.dgvSubDevices.ReadOnly = true;
            this.dgvSubDevices.RowTemplate.Height = 23;
            this.dgvSubDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubDevices.Size = new System.Drawing.Size(489, 291);
            this.dgvSubDevices.TabIndex = 0;
            this.dgvSubDevices.SelectionChanged += new System.EventHandler(this.dgvSubDevices_SelectionChanged);
            // 
            // dgvPorts
            // 
            this.dgvPorts.AllowUserToAddRows = false;
            this.dgvPorts.AllowUserToDeleteRows = false;
            this.dgvPorts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPorts.Location = new System.Drawing.Point(0, 0);
            this.dgvPorts.Name = "dgvPorts";
            this.dgvPorts.ReadOnly = true;
            this.dgvPorts.RowTemplate.Height = 23;
            this.dgvPorts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPorts.Size = new System.Drawing.Size(485, 291);
            this.dgvPorts.TabIndex = 0;
            // 
            // NewDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tableMain);
            this.Name = "NewDeviceForm";
            this.Text = "设备信息(简化版)";
            this.Load += new System.EventHandler(this.NewDeviceForm_Load);
            this.tableMain.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            this.splitContainerSub.Panel1.ResumeLayout(false);
            this.splitContainerSub.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSub)).EndInit();
            this.splitContainerSub.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.DataGridView dgvDevices;
        private System.Windows.Forms.SplitContainer splitContainerSub;
        private System.Windows.Forms.DataGridView dgvSubDevices;
        private System.Windows.Forms.DataGridView dgvPorts;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbDeviceGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDeviceType;
        private System.Windows.Forms.Label label1;
    }
} 