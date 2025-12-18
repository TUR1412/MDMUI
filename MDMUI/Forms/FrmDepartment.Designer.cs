namespace MDMUI
{
    partial class FrmDepartment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelFactory = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxFactory = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelSearch = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxSearchName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridViewDepartments = new System.Windows.Forms.DataGridView();
            this.colDeptId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParentDeptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFactoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDepartments)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAdd,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonRefresh,
            this.toolStripSeparator1,
            this.toolStripLabelFactory,
            this.toolStripComboBoxFactory,
            this.toolStripSeparator2,
            this.toolStripLabelSearch,
            this.toolStripTextBoxSearchName,
            this.toolStripButtonSearch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1000, 27); // Adjusted size for text buttons/combo box
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(43, 24);
            this.toolStripButtonAdd.Text = "新增";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(43, 24);
            this.toolStripButtonEdit.Text = "编辑";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(43, 24);
            this.toolStripButtonDelete.Text = "删除";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(43, 24);
            this.toolStripButtonRefresh.Text = "刷新";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabelFactory
            // 
            this.toolStripLabelFactory.Name = "toolStripLabelFactory";
            this.toolStripLabelFactory.Size = new System.Drawing.Size(73, 24);
            this.toolStripLabelFactory.Text = "所属工厂:";
            // 
            // toolStripComboBoxFactory
            // 
            this.toolStripComboBoxFactory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxFactory.Name = "toolStripComboBoxFactory";
            this.toolStripComboBoxFactory.Size = new System.Drawing.Size(150, 27);
            this.toolStripComboBoxFactory.SelectedIndexChanged += new System.EventHandler(this.CboFactory_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabelSearch
            // 
            this.toolStripLabelSearch.Name = "toolStripLabelSearch";
            this.toolStripLabelSearch.Size = new System.Drawing.Size(73, 24);
            this.toolStripLabelSearch.Text = "部门名称:";
            // 
            // toolStripTextBoxSearchName
            // 
            this.toolStripTextBoxSearchName.Name = "textBoxSearchName"; // Corresponds to FrmDepartment.cs usage
            this.toolStripTextBoxSearchName.Size = new System.Drawing.Size(150, 27);
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(43, 24);
            this.toolStripButtonSearch.Text = "查询";
            this.toolStripButtonSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 578);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1000, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelStatus
            // 
            this.toolStripStatusLabelStatus.Name = "toolStripStatusLabelStatus";
            this.toolStripStatusLabelStatus.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabelStatus.Text = "就绪";
            // 
            // dataGridViewDepartments
            // 
            this.dataGridViewDepartments.AllowUserToAddRows = false;
            this.dataGridViewDepartments.AllowUserToDeleteRows = false;
            this.dataGridViewDepartments.AllowUserToResizeRows = false;
            this.dataGridViewDepartments.AutoGenerateColumns = false; // We defined columns manually
            this.dataGridViewDepartments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDepartments.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewDepartments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewDepartments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDepartments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDeptId,
            this.colDeptName,
            this.colParentDeptName,
            this.colFactoryName,
            this.colManager,
            this.colDescription,
            this.colCreateTime});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewDepartments.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewDepartments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDepartments.Location = new System.Drawing.Point(0, 27); // Below toolStrip1
            this.dataGridViewDepartments.Name = "dataGridViewDepartments";
            this.dataGridViewDepartments.ReadOnly = true;
            this.dataGridViewDepartments.RowHeadersVisible = false;
            this.dataGridViewDepartments.RowTemplate.Height = 24;
            this.dataGridViewDepartments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDepartments.Size = new System.Drawing.Size(1000, 551); // 600 - 27 (toolStrip) - 22 (statusStrip)
            this.dataGridViewDepartments.TabIndex = 2;
            this.dataGridViewDepartments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvDepartment_CellDoubleClick);
            // 
            // colDeptId
            // 
            this.colDeptId.DataPropertyName = "DeptId";
            this.colDeptId.HeaderText = "部门编号";
            this.colDeptId.Name = "colDeptId";
            this.colDeptId.ReadOnly = true;
            this.colDeptId.Width = 100;
            // 
            // colDeptName
            // 
            this.colDeptName.DataPropertyName = "DeptName";
            this.colDeptName.HeaderText = "部门名称";
            this.colDeptName.Name = "colDeptName";
            this.colDeptName.ReadOnly = true;
            this.colDeptName.Width = 150;
            // 
            // colParentDeptName
            // 
            this.colParentDeptName.DataPropertyName = "ParentDeptName";
            this.colParentDeptName.HeaderText = "上级部门";
            this.colParentDeptName.Name = "colParentDeptName";
            this.colParentDeptName.ReadOnly = true;
            this.colParentDeptName.Width = 150;
            // 
            // colFactoryName
            // 
            this.colFactoryName.DataPropertyName = "FactoryName";
            this.colFactoryName.HeaderText = "所属工厂";
            this.colFactoryName.Name = "colFactoryName";
            this.colFactoryName.ReadOnly = true;
            this.colFactoryName.Width = 150;
            // 
            // colManager
            // 
            this.colManager.DataPropertyName = "Manager";
            this.colManager.HeaderText = "部门负责人";
            this.colManager.Name = "colManager";
            this.colManager.ReadOnly = true;
            this.colManager.Width = 100;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.HeaderText = "描述";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.Width = 200;
            // 
            // colCreateTime
            // 
            this.colCreateTime.DataPropertyName = "CreateTime";
            this.colCreateTime.HeaderText = "创建时间";
            this.colCreateTime.Name = "colCreateTime";
            this.colCreateTime.ReadOnly = true;
            this.colCreateTime.Width = 150;
            // 
            // FrmDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.dataGridViewDepartments);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmDepartment";
            this.Text = "部门管理";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDepartments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelFactory;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxFactory;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStatus;
        private System.Windows.Forms.DataGridView dataGridViewDepartments;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSearch;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSearchName;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParentDeptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFactoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateTime;
    }
} 