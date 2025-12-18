using System;
using System.Windows.Forms;
using System.Drawing;

namespace MDMUI
{
    partial class FrmUpdateOpSeq
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvLeftOpers = new System.Windows.Forms.DataGridView();
            this.lblLeftCount = new System.Windows.Forms.Label();
            this.dgvRightOpers = new System.Windows.Forms.DataGridView();
            this.lblRightCount = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAutoSort = new System.Windows.Forms.Button();
            this.btnBatchAdd = new System.Windows.Forms.Button();
            this.btnBatchRemove = new System.Windows.Forms.Button();
            this.chkEnableDrag = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeftOpers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRightOpers)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(12, 62);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgvLeftOpers);
            this.splitContainer.Panel1.Controls.Add(this.lblLeftCount);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dgvRightOpers);
            this.splitContainer.Panel2.Controls.Add(this.lblRightCount);
            this.splitContainer.Size = new System.Drawing.Size(1056, 468);
            this.splitContainer.SplitterDistance = 526;
            this.splitContainer.TabIndex = 0;
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(150)))), ((int)(((byte)(240)))));
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1080, 50);
            this.panelTitle.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(107, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "修改工艺路线";
            // 
            // dgvLeftOpers
            // 
            this.dgvLeftOpers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLeftOpers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvLeftOpers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLeftOpers.Location = new System.Drawing.Point(3, 35);
            this.dgvLeftOpers.Name = "dgvLeftOpers";
            this.dgvLeftOpers.Size = new System.Drawing.Size(520, 430);
            this.dgvLeftOpers.TabIndex = 1;
            // 
            // lblLeftCount
            // 
            this.lblLeftCount.AutoSize = true;
            this.lblLeftCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeftCount.Location = new System.Drawing.Point(3, 5);
            this.lblLeftCount.Name = "lblLeftCount";
            this.lblLeftCount.Size = new System.Drawing.Size(248, 15);
            this.lblLeftCount.TabIndex = 0;
            this.lblLeftCount.Text = "不在当前工艺流程中的工艺站 (0)";
            // 
            // dgvRightOpers
            // 
            this.dgvRightOpers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRightOpers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvRightOpers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRightOpers.Location = new System.Drawing.Point(3, 35);
            this.dgvRightOpers.Name = "dgvRightOpers";
            this.dgvRightOpers.Size = new System.Drawing.Size(520, 430);
            this.dgvRightOpers.TabIndex = 1;
            // 
            // lblRightCount
            // 
            this.lblRightCount.AutoSize = true;
            this.lblRightCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightCount.Location = new System.Drawing.Point(3, 5);
            this.lblRightCount.Name = "lblRightCount";
            this.lblRightCount.Size = new System.Drawing.Size(188, 15);
            this.lblRightCount.TabIndex = 0;
            this.lblRightCount.Text = "当前工艺流程中的工艺站 (0)";
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.btnDown);
            this.panelButtons.Controls.Add(this.btnUp);
            this.panelButtons.Controls.Add(this.btnRemove);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnAutoSort);
            this.panelButtons.Controls.Add(this.btnBatchAdd);
            this.panelButtons.Controls.Add(this.btnBatchRemove);
            this.panelButtons.Controls.Add(this.chkEnableDrag);
            this.panelButtons.Location = new System.Drawing.Point(12, 536);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1056, 53);
            this.panelButtons.TabIndex = 1;
            // 
            // btnAutoSort
            // 
            this.btnAutoSort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
            this.btnAutoSort.FlatAppearance.BorderSize = 0;
            this.btnAutoSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSort.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAutoSort.ForeColor = System.Drawing.Color.White;
            this.btnAutoSort.Location = new System.Drawing.Point(450, 15);
            this.btnAutoSort.Name = "btnAutoSort";
            this.btnAutoSort.Size = new System.Drawing.Size(120, 36);
            this.btnAutoSort.TabIndex = 6;
            this.btnAutoSort.Text = "自动排序";
            this.toolTip.SetToolTip(this.btnAutoSort, "根据工站类型和依赖关系自动优化工艺流程顺序");
            this.btnAutoSort.UseVisualStyleBackColor = false;
            // 
            // btnBatchAdd
            // 
            this.btnBatchAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(39)))), ((int)(((byte)(176)))));
            this.btnBatchAdd.FlatAppearance.BorderSize = 0;
            this.btnBatchAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchAdd.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBatchAdd.ForeColor = System.Drawing.Color.White;
            this.btnBatchAdd.Location = new System.Drawing.Point(160, 15);
            this.btnBatchAdd.Name = "btnBatchAdd";
            this.btnBatchAdd.Size = new System.Drawing.Size(120, 36);
            this.btnBatchAdd.TabIndex = 7;
            this.btnBatchAdd.Text = "批量添加";
            this.toolTip.SetToolTip(this.btnBatchAdd, "批量添加选中的工艺站");
            this.btnBatchAdd.UseVisualStyleBackColor = false;
            // 
            // btnBatchRemove
            // 
            this.btnBatchRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.btnBatchRemove.FlatAppearance.BorderSize = 0;
            this.btnBatchRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchRemove.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBatchRemove.ForeColor = System.Drawing.Color.White;
            this.btnBatchRemove.Location = new System.Drawing.Point(290, 15);
            this.btnBatchRemove.Name = "btnBatchRemove";
            this.btnBatchRemove.Size = new System.Drawing.Size(120, 36);
            this.btnBatchRemove.TabIndex = 8;
            this.btnBatchRemove.Text = "批量移除";
            this.toolTip.SetToolTip(this.btnBatchRemove, "批量移除选中的工艺站");
            this.btnBatchRemove.UseVisualStyleBackColor = false;
            // 
            // chkEnableDrag
            // 
            this.chkEnableDrag.AutoSize = true;
            this.chkEnableDrag.Checked = true;
            this.chkEnableDrag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableDrag.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkEnableDrag.Location = new System.Drawing.Point(580, 24);
            this.chkEnableDrag.Name = "chkEnableDrag";
            this.chkEnableDrag.Size = new System.Drawing.Size(87, 24);
            this.chkEnableDrag.TabIndex = 9;
            this.chkEnableDrag.Text = "启用拖拽";
            this.toolTip.SetToolTip(this.chkEnableDrag, "启用或禁用拖拽重排序功能");
            this.chkEnableDrag.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(166)))), ((int)(((byte)(245)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(200, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "添加 >";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(166)))), ((int)(((byte)(245)))));
            this.btnRemove.FlatAppearance.BorderSize = 0;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.Color.White;
            this.btnRemove.Location = new System.Drawing.Point(346, 13);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 30);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "< 移除";
            this.btnRemove.UseVisualStyleBackColor = false;
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(166)))), ((int)(((byte)(245)))));
            this.btnUp.FlatAppearance.BorderSize = 0;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUp.ForeColor = System.Drawing.Color.White;
            this.btnUp.Location = new System.Drawing.Point(580, 13);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(100, 30);
            this.btnUp.TabIndex = 2;
            this.btnUp.Text = "向上 ↑";
            this.btnUp.UseVisualStyleBackColor = false;
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(166)))), ((int)(((byte)(245)))));
            this.btnDown.FlatAppearance.BorderSize = 0;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDown.ForeColor = System.Drawing.Color.White;
            this.btnDown.Location = new System.Drawing.Point(726, 13);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(100, 30);
            this.btnDown.TabIndex = 3;
            this.btnDown.Text = "向下 ↓";
            this.btnDown.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(87)))), ((int)(((byte)(166)))), ((int)(((byte)(245)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(953, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // FrmUpdateOpSeq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 601);
            this.Controls.Add(this.panelTitle);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.splitContainer);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmUpdateOpSeq";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改工艺路线";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLeftOpers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRightOpers)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvLeftOpers;
        private System.Windows.Forms.Label lblLeftCount;
        private System.Windows.Forms.DataGridView dgvRightOpers;
        private System.Windows.Forms.Label lblRightCount;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAutoSort;
        private System.Windows.Forms.Button btnBatchAdd;
        private System.Windows.Forms.Button btnBatchRemove;
        private System.Windows.Forms.CheckBox chkEnableDrag;
        private System.Windows.Forms.ToolTip toolTip;
    }
} 