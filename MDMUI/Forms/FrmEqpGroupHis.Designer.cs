using System.Drawing;
using System.Windows.Forms;

namespace MDMUI
{
    partial class FrmEqpGroupHis
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
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.lblHistory = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            
            // 新增布局容器控件
            this.mainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.footerPanel = new System.Windows.Forms.Panel();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.mainTableLayout.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.SuspendLayout();
            
            // mainTableLayout
            //
            this.mainTableLayout.ColumnCount = 1;
            this.mainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayout.Controls.Add(this.headerPanel, 0, 0);
            this.mainTableLayout.Controls.Add(this.dgvHistory, 0, 1);
            this.mainTableLayout.Controls.Add(this.footerPanel, 0, 2);
            this.mainTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayout.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayout.Name = "mainTableLayout";
            this.mainTableLayout.Padding = new System.Windows.Forms.Padding(15);
            this.mainTableLayout.RowCount = 3;
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.mainTableLayout.Size = new System.Drawing.Size(850, 500);
            this.mainTableLayout.TabIndex = 0;
            
            // headerPanel
            //
            this.headerPanel.Controls.Add(this.lblHistory);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerPanel.Location = new System.Drawing.Point(15, 15);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(820, 40);
            this.headerPanel.TabIndex = 0;
            
            // footerPanel
            //
            this.footerPanel.Controls.Add(this.btnClose);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footerPanel.Location = new System.Drawing.Point(15, 435);
            this.footerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Size = new System.Drawing.Size(820, 50);
            this.footerPanel.TabIndex = 2;
            
            // lblHistory
            // 
            this.lblHistory.AutoSize = true;
            this.lblHistory.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblHistory.Location = new System.Drawing.Point(0, 10);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(146, 27);
            this.lblHistory.TabIndex = 0;
            this.lblHistory.Text = "设备组历史记录:";
            // 
            // dgvHistory
            // 
            this.dgvHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHistory.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(18, 58);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.RowHeadersVisible = false;
            this.dgvHistory.RowTemplate.Height = 30;
            this.dgvHistory.Size = new System.Drawing.Size(814, 374);
            this.dgvHistory.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(720, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmEqpGroupHis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 500);
            this.Controls.Add(this.mainTableLayout);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmEqpGroupHis";
            this.Text = "设备组历史记录";
            this.Load += new System.EventHandler(this.FrmEqpGroupHis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.mainTableLayout.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.Label lblHistory;
        private System.Windows.Forms.Button btnClose;
        
        // 新增布局容器控件
        private System.Windows.Forms.TableLayoutPanel mainTableLayout;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Panel footerPanel;
    }
} 