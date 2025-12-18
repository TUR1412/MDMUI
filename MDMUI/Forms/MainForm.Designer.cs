using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDMUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        // 主菜单
        private MenuStrip mainMenu;
        private StatusStrip statusStrip;
        private TabControl tabControl;
        private Panel tabContainer;
        
        // 产品管理菜单
        private ToolStripMenuItem productMenu;
        private ToolStripMenuItem productInfoItem;
        private ToolStripMenuItem productCategoryItem;
        
        // 生产管理菜单
        private ToolStripMenuItem productionMenu;
        private ToolStripMenuItem productionPlanItem;
        private ToolStripMenuItem productionRecordItem;
        
        // 设备管理菜单
        private ToolStripMenuItem equipmentMenu;
        private ToolStripMenuItem equipmentInfoItem;
        private ToolStripMenuItem equipmentMaintenanceItem;
        
        // 系统设置菜单
        private ToolStripMenuItem systemMenu;
        private ToolStripMenuItem systemParamsItem;
        private ToolStripMenuItem changePasswordItem;
        
        // 状态栏项
        private ToolStripStatusLabel userLabel;
        private ToolStripStatusLabel timeLabel;
        private ToolStripStatusLabel versionLabel;
        
        // 欢迎页
        private TabPage welcomePage;
        private Panel welcomePanel;
        private Label welcomeLabel;

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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Name = "MainForm";
            this.Text = "MDM系统";
            
            // 创建菜单
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainMenu.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.mainMenu.BackColor = System.Drawing.Color.FromArgb(100, 140, 190);
            this.mainMenu.ForeColor = System.Drawing.Color.White;
            this.mainMenu.Height = 35;
            this.mainMenu.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            
            // 产品管理
            this.productMenu = new ToolStripMenuItem("产品管理");
            this.productInfoItem = new ToolStripMenuItem("产品信息");
            this.productInfoItem.Name = "产品管理_产品信息";
            this.productInfoItem.Tag = "产品管理_产品信息";
            this.productInfoItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.productCategoryItem = new ToolStripMenuItem("产品类别");
            this.productCategoryItem.Name = "产品管理_产品类别";
            this.productCategoryItem.Tag = "产品管理_产品类别";
            this.productCategoryItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.productMenu.DropDownItems.Add(this.productInfoItem);
            this.productMenu.DropDownItems.Add(this.productCategoryItem);
            
            // 生产管理
            this.productionMenu = new ToolStripMenuItem("生产管理");
            this.productionPlanItem = new ToolStripMenuItem("生产计划");
            this.productionPlanItem.Name = "生产管理_生产计划";
            this.productionPlanItem.Tag = "生产管理_生产计划";
            this.productionPlanItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.productionRecordItem = new ToolStripMenuItem("生产记录");
            this.productionRecordItem.Name = "生产管理_生产记录";
            this.productionRecordItem.Tag = "生产管理_生产记录";
            this.productionRecordItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.productionMenu.DropDownItems.Add(this.productionPlanItem);
            this.productionMenu.DropDownItems.Add(this.productionRecordItem);
            
            // 设备管理
            this.equipmentMenu = new ToolStripMenuItem("设备管理");
            this.equipmentInfoItem = new ToolStripMenuItem("设备信息");
            this.equipmentInfoItem.Name = "设备管理_设备信息";
            this.equipmentInfoItem.Tag = "设备管理_设备信息";
            this.equipmentInfoItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.equipmentMaintenanceItem = new ToolStripMenuItem("设备维护");
            this.equipmentMaintenanceItem.Name = "设备管理_设备维护";
            this.equipmentMaintenanceItem.Tag = "设备管理_设备维护";
            this.equipmentMaintenanceItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.equipmentMenu.DropDownItems.Add(this.equipmentInfoItem);
            this.equipmentMenu.DropDownItems.Add(this.equipmentMaintenanceItem);
            
            // 系统设置
            this.systemMenu = new ToolStripMenuItem("系统设置");
            this.systemParamsItem = new ToolStripMenuItem("系统参数");
            this.systemParamsItem.Name = "系统设置_系统参数";
            this.systemParamsItem.Tag = "系统设置_系统参数";
            this.systemParamsItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.changePasswordItem = new ToolStripMenuItem("修改密码");
            this.changePasswordItem.Name = "系统设置_修改密码";
            this.changePasswordItem.Tag = "系统设置_修改密码";
            this.changePasswordItem.Click += new System.EventHandler(this.MenuItem_Click);
            
            this.systemMenu.DropDownItems.Add(this.systemParamsItem);
            this.systemMenu.DropDownItems.Add(this.changePasswordItem);
            
            // 添加菜单项到主菜单
            this.mainMenu.Items.Add(this.productMenu);
            this.mainMenu.Items.Add(this.productionMenu);
            this.mainMenu.Items.Add(this.equipmentMenu);
            this.mainMenu.Items.Add(this.systemMenu);
            
            // 创建状态栏
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(240, 245, 250);
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            
            // 添加状态栏项
            this.userLabel = new ToolStripStatusLabel("当前用户: 未登录");
            this.userLabel.Name = "userLabel";
            this.statusStrip.Items.Add(this.userLabel);
            
            this.statusStrip.Items.Add(new ToolStripSeparator());
            
            this.timeLabel = new ToolStripStatusLabel("登录时间: " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            this.timeLabel.Name = "timeLabel";
            this.statusStrip.Items.Add(this.timeLabel);
            
            this.statusStrip.Items.Add(new ToolStripSeparator());
            
            this.versionLabel = new ToolStripStatusLabel("系统版本: v1.0.0");
            this.statusStrip.Items.Add(this.versionLabel);
            
            // 创建TabControl容器
            this.tabContainer = new System.Windows.Forms.Panel();
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabContainer.Padding = new System.Windows.Forms.Padding(5, 20, 5, 5);
            this.tabContainer.BackColor = System.Drawing.Color.White;
            
            // 创建TabControl
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabControl.Name = "tabControl";
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Padding = new System.Drawing.Point(10, 8);
            this.tabControl.ItemSize = new System.Drawing.Size(150, 40);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TabControl_DrawItem);
            this.tabControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TabControl_MouseClick);
            
            // 添加欢迎页
            this.welcomePage = new System.Windows.Forms.TabPage("欢迎页");
            this.welcomePage.BackColor = System.Drawing.Color.White;
            
            this.welcomePanel = new System.Windows.Forms.Panel();
            this.welcomePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.welcomePanel.BackColor = System.Drawing.Color.White;
            this.welcomePanel.Padding = new System.Windows.Forms.Padding(20);
            
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.welcomeLabel.Text = "欢迎使用MDM管理系统";
            this.welcomeLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold);
            this.welcomeLabel.ForeColor = System.Drawing.Color.FromArgb(60, 100, 180);
            this.welcomeLabel.AutoSize = false;
            this.welcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.welcomeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            
            this.welcomePanel.Controls.Add(this.welcomeLabel);
            this.welcomePage.Controls.Add(this.welcomePanel);
            this.tabControl.TabPages.Add(this.welcomePage);
            
            // 添加控件到容器
            this.tabContainer.Controls.Add(this.tabControl);
            
            // 添加所有控件到窗体
            this.Controls.Add(this.tabContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.mainMenu);
            
            // 设置MainMenu为主菜单
            this.MainMenuStrip = this.mainMenu;
            
            // Resume
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}