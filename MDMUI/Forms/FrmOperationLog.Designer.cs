using System.Drawing;
using System.Windows.Forms;

namespace MDMUI
{
    partial class FrmOperationLog // Make partial
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.filterPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblUser = new System.Windows.Forms.Label();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.lblOperationType = new System.Windows.Forms.Label();
            this.txtOperationType = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.demoPanel = new System.Windows.Forms.Panel();
            this.demoLabel = new System.Windows.Forms.Label();
            this.placeholderLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.filterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.demoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.Controls.Add(this.dgvLogs);
            this.mainPanel.Controls.Add(this.filterPanel);
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(900, 500);
            this.mainPanel.TabIndex = 0;
            // 
            // filterPanel
            // 
            this.filterPanel.ColumnCount = 11;
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.filterPanel.Controls.Add(this.lblStartDate, 0, 0);
            this.filterPanel.Controls.Add(this.dtpStartDate, 1, 0);
            this.filterPanel.Controls.Add(this.lblEndDate, 3, 0);
            this.filterPanel.Controls.Add(this.dtpEndDate, 4, 0);
            this.filterPanel.Controls.Add(this.lblUser, 6, 0);
            this.filterPanel.Controls.Add(this.cmbUser, 7, 0);
            this.filterPanel.Controls.Add(this.lblModule, 9, 0);
            this.filterPanel.Controls.Add(this.cmbModule, 10, 0);
            this.filterPanel.Controls.Add(this.lblOperationType, 0, 1);
            this.filterPanel.Controls.Add(this.txtOperationType, 1, 1);
            this.filterPanel.Controls.Add(this.btnSearch, 10, 1);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(10, 46);
            this.filterPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.RowCount = 2;
            this.filterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.filterPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.filterPanel.Size = new System.Drawing.Size(880, 65);
            this.filterPanel.TabIndex = 1;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(3, 8);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(72, 16);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "开始日期:";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpStartDate.Checked = false;
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(81, 5);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(130, 22);
            this.dtpStartDate.TabIndex = 1;
            // 
            // lblEndDate
            // 
            this.lblEndDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(227, 8);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(72, 16);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "结束日期:";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtpEndDate.Checked = false;
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(305, 5);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(130, 22);
            this.dtpEndDate.TabIndex = 2;
            // 
            // lblUser
            // 
            this.lblUser.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(451, 8);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(47, 16);
            this.lblUser.TabIndex = 4;
            this.lblUser.Text = "用户:";
            // 
            // cmbUser
            // 
            this.cmbUser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(504, 4);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(140, 24);
            this.cmbUser.TabIndex = 3;
            // 
            // lblModule
            // 
            this.lblModule.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(660, 8);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(47, 16);
            this.lblModule.TabIndex = 6;
            this.lblModule.Text = "模块:";
            // 
            // cmbModule
            // 
            this.cmbModule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModule.FormattingEnabled = true;
            this.cmbModule.Location = new System.Drawing.Point(713, 4);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(140, 24);
            this.cmbModule.TabIndex = 4;
            // 
            // lblOperationType
            // 
            this.lblOperationType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOperationType.AutoSize = true;
            this.lblOperationType.Location = new System.Drawing.Point(3, 40);
            this.lblOperationType.Name = "lblOperationType";
            this.lblOperationType.Size = new System.Drawing.Size(72, 16);
            this.lblOperationType.TabIndex = 8;
            this.lblOperationType.Text = "操作类型:";
            // 
            // txtOperationType
            // 
            this.txtOperationType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.filterPanel.SetColumnSpan(this.txtOperationType, 3);
            this.txtOperationType.Location = new System.Drawing.Point(81, 37);
            this.txtOperationType.Name = "txtOperationType";
            this.txtOperationType.Size = new System.Drawing.Size(218, 22);
            this.txtOperationType.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSearch.Location = new System.Drawing.Point(713, 36);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 25);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogs.Location = new System.Drawing.Point(10, 121);
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.ReadOnly = true;
            this.dgvLogs.RowHeadersWidth = 51;
            this.dgvLogs.RowTemplate.Height = 24;
            this.dgvLogs.Size = new System.Drawing.Size(880, 369);
            this.dgvLogs.TabIndex = 2;
            // 
            // demoPanel
            // 
            this.demoPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.demoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.demoPanel.Controls.Add(this.demoLabel);
            this.demoPanel.Location = new System.Drawing.Point(23, 183);
            this.demoPanel.Name = "demoPanel";
            this.demoPanel.Size = new System.Drawing.Size(500, 200);
            this.demoPanel.TabIndex = 3;
            // 
            // demoLabel
            // 
            this.demoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.demoLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F);
            this.demoLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.demoLabel.Location = new System.Drawing.Point(0, 0);
            this.demoLabel.Name = "demoLabel";
            this.demoLabel.Size = new System.Drawing.Size(498, 198);
            this.demoLabel.TabIndex = 0;
            this.demoLabel.Text = "功能开发中";
            this.demoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // placeholderLabel
            // 
            this.placeholderLabel.AutoSize = true;
            this.placeholderLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.placeholderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.placeholderLabel.Location = new System.Drawing.Point(23, 83);
            this.placeholderLabel.Name = "placeholderLabel";
            this.placeholderLabel.Size = new System.Drawing.Size(352, 27);
            this.placeholderLabel.TabIndex = 1;
            this.placeholderLabel.Text = "操作日志查询功能正在开发中，敬请期待...";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.titleLabel.Location = new System.Drawing.Point(23, 23);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(177, 36);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "操作日志查询";
            // 
            // FrmOperationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.mainPanel);
            this.Name = "FrmOperationLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "操作日志";
            this.Load += new System.EventHandler(this.FrmOperationLog_Load);
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.demoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel mainPanel;
        private Panel demoPanel;
        private Label demoLabel;
        private Label placeholderLabel;
        private Label titleLabel;
        private System.Windows.Forms.TableLayoutPanel filterPanel;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox cmbUser;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.ComboBox cmbModule;
        private System.Windows.Forms.Label lblOperationType;
        private System.Windows.Forms.TextBox txtOperationType;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvLogs;
    }
} 