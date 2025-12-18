namespace MDMUI
{
    partial class FrmProductCategory
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.panelLeftTitle = new System.Windows.Forms.Panel();
            this.lblLeftTitle = new System.Windows.Forms.Label();
            this.toolStripLeft = new System.Windows.Forms.ToolStrip();
            this.btnAddCategory = new System.Windows.Forms.ToolStripButton();
            this.btnEditCategory = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteCategory = new System.Windows.Forms.ToolStripButton();
            this.btnRefreshTree = new System.Windows.Forms.ToolStripButton();
            this.tvCategory = new System.Windows.Forms.TreeView();
            this.panelRightTitle = new System.Windows.Forms.Panel();
            this.lblRightTitle = new System.Windows.Forms.Label();
            this.dgvSubCategory = new System.Windows.Forms.DataGridView();
            this.statusStripBottom = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.panelLeftTitle.SuspendLayout();
            this.toolStripLeft.SuspendLayout();
            this.panelRightTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubCategory)).BeginInit();
            this.statusStripBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tvCategory);
            this.splitContainerMain.Panel1.Controls.Add(this.toolStripLeft);
            this.splitContainerMain.Panel1.Controls.Add(this.panelLeftTitle);
            this.splitContainerMain.Panel1MinSize = 150; // Adjusted MinSize
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.dgvSubCategory);
            this.splitContainerMain.Panel2.Controls.Add(this.panelRightTitle);
            this.splitContainerMain.Panel2MinSize = 200; // Adjusted MinSize
            this.splitContainerMain.Size = new System.Drawing.Size(884, 539); // Adjusted size based on default form size minus status bar
            this.splitContainerMain.SplitterDistance = 294; // Default distance (approx 1/3)
            this.splitContainerMain.SplitterWidth = 5;
            this.splitContainerMain.TabIndex = 0;
            // 
            // panelLeftTitle
            // 
            this.panelLeftTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.panelLeftTitle.Controls.Add(this.lblLeftTitle);
            this.panelLeftTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLeftTitle.Location = new System.Drawing.Point(0, 0);
            this.panelLeftTitle.Name = "panelLeftTitle";
            this.panelLeftTitle.Size = new System.Drawing.Size(294, 40);
            this.panelLeftTitle.TabIndex = 0;
            // 
            // lblLeftTitle
            // 
            this.lblLeftTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLeftTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLeftTitle.ForeColor = System.Drawing.Color.White;
            this.lblLeftTitle.Location = new System.Drawing.Point(0, 0);
            this.lblLeftTitle.Name = "lblLeftTitle";
            this.lblLeftTitle.Size = new System.Drawing.Size(294, 40);
            this.lblLeftTitle.TabIndex = 0;
            this.lblLeftTitle.Text = "产品类别";
            this.lblLeftTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStripLeft
            // 
            this.toolStripLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStripLeft.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripLeft.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddCategory,
            this.btnEditCategory,
            this.btnDeleteCategory,
            this.btnRefreshTree});
            this.toolStripLeft.Location = new System.Drawing.Point(0, 40);
            this.toolStripLeft.Name = "toolStripLeft";
            this.toolStripLeft.Size = new System.Drawing.Size(294, 25);
            this.toolStripLeft.TabIndex = 1;
            this.toolStripLeft.Text = "toolStrip1";
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(60, 22);
            this.btnAddCategory.Text = "添加类别";
            this.btnAddCategory.Click += new System.EventHandler(this.BtnAddCategory_Click);
            // 
            // btnEditCategory
            // 
            this.btnEditCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnEditCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditCategory.Name = "btnEditCategory";
            this.btnEditCategory.Size = new System.Drawing.Size(60, 22);
            this.btnEditCategory.Text = "编辑类别";
            this.btnEditCategory.Click += new System.EventHandler(this.BtnEditCategory_Click);
            // 
            // btnDeleteCategory
            // 
            this.btnDeleteCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDeleteCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteCategory.Name = "btnDeleteCategory";
            this.btnDeleteCategory.Size = new System.Drawing.Size(60, 22);
            this.btnDeleteCategory.Text = "删除类别";
            this.btnDeleteCategory.Click += new System.EventHandler(this.BtnDeleteCategory_Click);
            // 
            // btnRefreshTree
            // 
            this.btnRefreshTree.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRefreshTree.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshTree.Name = "btnRefreshTree";
            this.btnRefreshTree.Size = new System.Drawing.Size(36, 22);
            this.btnRefreshTree.Text = "刷新";
            this.btnRefreshTree.Click += new System.EventHandler(this.BtnRefreshTree_Click);
            // 
            // tvCategory
            // 
            this.tvCategory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCategory.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.tvCategory.FullRowSelect = true;
            this.tvCategory.HideSelection = false;
            this.tvCategory.ItemHeight = 25;
            this.tvCategory.Location = new System.Drawing.Point(0, 65); // Adjusted location based on Title Panel and ToolStrip
            this.tvCategory.Name = "tvCategory";
            this.tvCategory.ShowLines = true;
            this.tvCategory.Size = new System.Drawing.Size(294, 474); // Adjusted size
            this.tvCategory.TabIndex = 2;
            this.tvCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvCategory_AfterSelect);
            // 
            // panelRightTitle
            // 
            this.panelRightTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.panelRightTitle.Controls.Add(this.lblRightTitle);
            this.panelRightTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRightTitle.Location = new System.Drawing.Point(0, 0);
            this.panelRightTitle.Name = "panelRightTitle";
            this.panelRightTitle.Size = new System.Drawing.Size(585, 40);
            this.panelRightTitle.TabIndex = 0;
            // 
            // lblRightTitle
            // 
            this.lblRightTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRightTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblRightTitle.ForeColor = System.Drawing.Color.White;
            this.lblRightTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(585, 40);
            this.lblRightTitle.TabIndex = 0;
            this.lblRightTitle.Text = "子类别列表";
            this.lblRightTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvSubCategory
            // 
            this.dgvSubCategory.AllowUserToAddRows = false;
            this.dgvSubCategory.AllowUserToDeleteRows = false;
            this.dgvSubCategory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSubCategory.BackgroundColor = System.Drawing.Color.White;
            this.dgvSubCategory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSubCategory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSubCategory.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.dgvSubCategory.Location = new System.Drawing.Point(0, 40); // Adjusted location based on Title Panel
            this.dgvSubCategory.Name = "dgvSubCategory";
            this.dgvSubCategory.ReadOnly = true;
            this.dgvSubCategory.RowHeadersVisible = false;
            this.dgvSubCategory.RowTemplate.Height = 23;
            this.dgvSubCategory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubCategory.Size = new System.Drawing.Size(585, 499); // Adjusted size
            this.dgvSubCategory.TabIndex = 1;
            this.dgvSubCategory.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSubCategory_DataBindingComplete);
            // 
            // statusStripBottom
            // 
            this.statusStripBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.statusStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStripBottom.Location = new System.Drawing.Point(0, 539);
            this.statusStripBottom.Name = "statusStripBottom";
            this.statusStripBottom.Size = new System.Drawing.Size(884, 22);
            this.statusStripBottom.TabIndex = 1;
            this.statusStripBottom.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(869, 17); // Adjusted size calculation
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "就绪";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmProductCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(884, 561); // Default size
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStripBottom);
            this.Name = "FrmProductCategory";
            this.Text = "产品类别管理";
            this.Load += new System.EventHandler(this.FrmProductCategory_Load);
            this.Shown += new System.EventHandler(this.FrmProductCategory_Shown);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelLeftTitle.ResumeLayout(false);
            this.toolStripLeft.ResumeLayout(false);
            this.toolStripLeft.PerformLayout();
            this.panelRightTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubCategory)).EndInit();
            this.statusStripBottom.ResumeLayout(false);
            this.statusStripBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelLeftTitle;
        private System.Windows.Forms.Label lblLeftTitle;
        private System.Windows.Forms.ToolStrip toolStripLeft;
        private System.Windows.Forms.ToolStripButton btnAddCategory;
        private System.Windows.Forms.ToolStripButton btnEditCategory;
        private System.Windows.Forms.ToolStripButton btnDeleteCategory;
        private System.Windows.Forms.TreeView tvCategory;
        private System.Windows.Forms.Panel panelRightTitle;
        private System.Windows.Forms.Label lblRightTitle;
        private System.Windows.Forms.DataGridView dgvSubCategory;
        private System.Windows.Forms.StatusStrip statusStripBottom;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripButton btnRefreshTree;
    }
} 