using System.Drawing;
using System.Windows.Forms;

namespace MDMUI
{
    partial class FrmSystemParameters
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
            this.demoPanel = new System.Windows.Forms.Panel();
            this.demoLabel = new System.Windows.Forms.Label();
            this.userLabel = new System.Windows.Forms.Label();
            this.placeholderLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.demoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.Controls.Add(this.demoPanel);
            this.mainPanel.Controls.Add(this.userLabel);
            this.mainPanel.Controls.Add(this.placeholderLabel);
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(20);
            this.mainPanel.Size = new System.Drawing.Size(800, 450);
            this.mainPanel.TabIndex = 0;
            // 
            // demoPanel
            // 
            this.demoPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.demoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.demoPanel.Controls.Add(this.demoLabel);
            this.demoPanel.Location = new System.Drawing.Point(23, 183); // Adjusted Y slightly
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
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.userLabel.ForeColor = System.Drawing.Color.Gray;
            this.userLabel.Location = new System.Drawing.Point(23, 133); // Adjusted Y slightly
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(200, 23); // Example size
            this.userLabel.TabIndex = 2;
            this.userLabel.Text = "当前用户: {Username}"; // Placeholder, updated in Load
            // 
            // placeholderLabel
            // 
            this.placeholderLabel.AutoSize = true;
            this.placeholderLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.placeholderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.placeholderLabel.Location = new System.Drawing.Point(23, 83); // Adjusted Y slightly
            this.placeholderLabel.Name = "placeholderLabel";
            this.placeholderLabel.Size = new System.Drawing.Size(352, 27);
            this.placeholderLabel.TabIndex = 1;
            this.placeholderLabel.Text = "系统参数设置功能正在开发中，敬请期待...";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(100)))), ((int)(((byte)(180)))));
            this.titleLabel.Location = new System.Drawing.Point(23, 23); // Adjusted X/Y slightly
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(177, 36);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "系统参数设置";
            // 
            // FrmSystemParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainPanel);
            this.Name = "FrmSystemParameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统参数设置";
            this.Load += new System.EventHandler(this.FrmSystemParameters_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.demoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel mainPanel;
        private Panel demoPanel;
        private Label demoLabel;
        private Label userLabel;
        private Label placeholderLabel;
        private Label titleLabel;
    }
} 