namespace MDMUI
{
    partial class FrmDatabaseBrowser
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstTables = new System.Windows.Forms.ListBox();
            this.lblTables = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabData = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.tabStructure = new System.Windows.Forms.TabPage();
            this.dgvStructure = new System.Windows.Forms.DataGridView();
            this.tabQuery = new System.Windows.Forms.TabPage();
            this.btnExecuteQuery = new System.Windows.Forms.Button();
            this.txtSqlQuery = new System.Windows.Forms.TextBox();
            this.lblQuery = new System.Windows.Forms.Label();
            
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblTables);
            this.splitContainer1.Panel1.Controls.Add(this.lstTables);
            this.splitContainer1.Panel1MinSize = 200;
            
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            
            // 
            // lstTables
            // 
            this.lstTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTables.FormattingEnabled = true;
            this.lstTables.ItemHeight = 12;
            this.lstTables.Location = new System.Drawing.Point(12, 25);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(176, 412);
            this.lstTables.TabIndex = 0;
            this.lstTables.SelectedIndexChanged += new System.EventHandler(this.lstTables_SelectedIndexChanged);
            
            // 
            // lblTables
            // 
            this.lblTables.AutoSize = true;
            this.lblTables.Location = new System.Drawing.Point(12, 9);
            this.lblTables.Name = "lblTables";
            this.lblTables.Size = new System.Drawing.Size(53, 12);
            this.lblTables.TabIndex = 1;
            this.lblTables.Text = "表列表：";
            
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabData);
            this.tabControl.Controls.Add(this.tabStructure);
            this.tabControl.Controls.Add(this.tabQuery);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(596, 450);
            this.tabControl.TabIndex = 0;
            
            // 
            // tabData
            // 
            this.tabData.Controls.Add(this.dataGridView);
            this.tabData.Location = new System.Drawing.Point(4, 22);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new System.Windows.Forms.Padding(3);
            this.tabData.Size = new System.Drawing.Size(588, 424);
            this.tabData.TabIndex = 0;
            this.tabData.Text = "数据";
            this.tabData.UseVisualStyleBackColor = true;
            
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(582, 418);
            this.dataGridView.TabIndex = 0;
            
            // 
            // tabStructure
            // 
            this.tabStructure.Controls.Add(this.dgvStructure);
            this.tabStructure.Location = new System.Drawing.Point(4, 22);
            this.tabStructure.Name = "tabStructure";
            this.tabStructure.Padding = new System.Windows.Forms.Padding(3);
            this.tabStructure.Size = new System.Drawing.Size(588, 424);
            this.tabStructure.TabIndex = 1;
            this.tabStructure.Text = "表结构";
            this.tabStructure.UseVisualStyleBackColor = true;
            
            // 
            // dgvStructure
            // 
            this.dgvStructure.AllowUserToAddRows = false;
            this.dgvStructure.AllowUserToDeleteRows = false;
            this.dgvStructure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStructure.Location = new System.Drawing.Point(3, 3);
            this.dgvStructure.Name = "dgvStructure";
            this.dgvStructure.ReadOnly = true;
            this.dgvStructure.Size = new System.Drawing.Size(582, 418);
            this.dgvStructure.TabIndex = 0;
            
            // 
            // tabQuery
            // 
            this.tabQuery.Controls.Add(this.btnExecuteQuery);
            this.tabQuery.Controls.Add(this.txtSqlQuery);
            this.tabQuery.Controls.Add(this.lblQuery);
            this.tabQuery.Location = new System.Drawing.Point(4, 22);
            this.tabQuery.Name = "tabQuery";
            this.tabQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tabQuery.Size = new System.Drawing.Size(588, 424);
            this.tabQuery.TabIndex = 2;
            this.tabQuery.Text = "SQL查询";
            this.tabQuery.UseVisualStyleBackColor = true;
            
            // 
            // btnExecuteQuery
            // 
            this.btnExecuteQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecuteQuery.Location = new System.Drawing.Point(507, 393);
            this.btnExecuteQuery.Name = "btnExecuteQuery";
            this.btnExecuteQuery.Size = new System.Drawing.Size(75, 23);
            this.btnExecuteQuery.TabIndex = 2;
            this.btnExecuteQuery.Text = "执行";
            this.btnExecuteQuery.UseVisualStyleBackColor = true;
            this.btnExecuteQuery.Click += new System.EventHandler(this.btnExecuteQuery_Click);
            
            // 
            // txtSqlQuery
            // 
            this.txtSqlQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlQuery.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSqlQuery.Location = new System.Drawing.Point(6, 25);
            this.txtSqlQuery.Multiline = true;
            this.txtSqlQuery.Name = "txtSqlQuery";
            this.txtSqlQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSqlQuery.Size = new System.Drawing.Size(576, 362);
            this.txtSqlQuery.TabIndex = 1;
            this.txtSqlQuery.Text = "SELECT * FROM Users";
            
            // 
            // lblQuery
            // 
            this.lblQuery.AutoSize = true;
            this.lblQuery.Location = new System.Drawing.Point(6, 10);
            this.lblQuery.Name = "lblQuery";
            this.lblQuery.Size = new System.Drawing.Size(83, 12);
            this.lblQuery.TabIndex = 0;
            this.lblQuery.Text = "输入SQL查询：";
            
            // 
            // FrmDatabaseBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmDatabaseBrowser";
            this.Text = "数据库浏览器";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabStructure.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStructure)).EndInit();
            this.tabQuery.ResumeLayout(false);
            this.tabQuery.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstTables;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TabPage tabStructure;
        private System.Windows.Forms.DataGridView dgvStructure;
        private System.Windows.Forms.TabPage tabQuery;
        private System.Windows.Forms.Button btnExecuteQuery;
        private System.Windows.Forms.TextBox txtSqlQuery;
        private System.Windows.Forms.Label lblQuery;
    }
} 