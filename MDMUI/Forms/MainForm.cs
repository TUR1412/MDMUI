using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.DAL;
using MDMUI.Model;
using MenuItem = MDMUI.BLL.MenuItem;
using MDMUI.Utility;
using System.Data.SqlClient;

// 注意: 请在Visual Studio中重新加载项目并重新编译
// MDMUI.csproj已更新，包含FrmDepartment.cs和FrmDepartmentEdit.cs
// 确保这些文件存在于项目目录中并被正确引用

namespace MDMUI
{
    public partial class MainForm : Form
    {
        // 当前登录用户
        private User CurrentUser;
        
        // 业务逻辑层和数据访问层
        private MenuBLL menuBLL;
        private UserDAL userDAL;
        private PermissionChecker permissionChecker;
        
        // 日志类
        public static class LogHelper 
        {
            public static void Log(string message)
            {
                // 简单的日志实现
                Console.WriteLine($"[{DateTime.Now}] {message}");
            }
        }

        // 带参数的构造函数，用于从登录窗体接收用户信息
        public MainForm(User user)
        {
            InitializeComponent();
            
            // 设置用户信息
            CurrentUser = user;
            
            // 初始化业务逻辑层
            menuBLL = new MenuBLL();
            userDAL = new UserDAL();
            permissionChecker = new PermissionChecker();

            // 设置窗体标题
            this.Text = $"MDM系统 - {CurrentUser.Username} - 最后登录时间: {user.LastLoginTime}";
            
            // 初始化界面
            InitializeUI();
            
            // 窗体加载事件
            this.Load += MainForm_Load;
            
            // 窗体关闭事件
            this.FormClosed += (sender, e) => LogHelper.Log("应用程序关闭");
            
            // 添加FormClosing事件以确保应用程序能够正常关闭
            this.FormClosing += MainForm_FormClosing;
            
            // 立即刷新界面，确保控件可见
            this.Refresh();
        }

        // 默认构造函数
        public MainForm()
        {
            InitializeComponent();
            
            // 初始化业务逻辑层
            menuBLL = new MenuBLL();
            userDAL = new UserDAL();
            permissionChecker = new PermissionChecker();

            // 设置窗体标题
            this.Text = "MDM系统";
            
            // 初始化界面
            InitializeUI();
            
            // 窗体加载事件
            this.Load += MainForm_Load;
            
            // 窗体关闭事件
            this.FormClosed += (sender, e) => LogHelper.Log("应用程序关闭");
            
            // 添加FormClosing事件以确保应用程序能够正常关闭
            this.FormClosing += MainForm_FormClosing;
            
            // 立即刷新界面，确保控件可见
            this.Refresh();
        }

        // 窗体加载事件
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                LogHelper.Log("MainForm正在加载...");
                
                // 如果当前用户为 null，降级为访客态（不授予任何权限）
                if (CurrentUser == null)
                {
                    // 安全兜底：不允许在未登录状态下默认“变成管理员”
                    CurrentUser = new User
                    {
                        Id = -1,
                        Username = "未登录",
                        RoleName = "访客",
                        RealName = "访客",
                        LastLoginTime = DateTime.Now
                    };

                    // 更新窗体标题
                    this.Text = "MDM系统 - 未登录";
                }
                
                // 更新状态栏信息
                UpdateStatusInfo();
                
                // 确保欢迎页选中
                if (tabControl.TabPages.Count > 0)
                {
                    tabControl.SelectedIndex = 0;
                }
                
                // 强制刷新界面
                this.PerformLayout();
                this.Refresh();
                
                LogHelper.Log("MainForm加载完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"窗体加载时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Log($"MainForm加载出错: {ex.Message}");
            }
        }
        
        // 初始化用户界面
        private void InitializeUI()
        {
            try
            {
                // 不需要重复创建控件，通过InitializeComponent已经创建
                
                // 设置窗体基本属性
                this.Size = new Size(1280, 800);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Maximized;
                
                // 创建主菜单
                CreateMainMenu();
                
                // 更新状态栏显示
                UpdateStatusInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化界面时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Log($"初始化界面出错: {ex.Message}");
            }
        }
        
        // 创建主菜单 (修改权限检查逻辑)
        private void CreateMainMenu()
        { 
            try
            {
                // 清除现有菜单项
                mainMenu.Items.Clear();
                LogHelper.Log("开始创建主菜单...");

                // 检查当前用户是否为null，避免空引用
                if (CurrentUser == null)
                {
                     LogHelper.Log("警告: CurrentUser 为 null，无法应用权限。");
                     // 可以选择加载一个非常受限的菜单或直接返回
                     // return;
                     // 或者创建一个临时的默认用户用于显示（如果 MainForm() 构造函数不处理）
                      CurrentUser = new User { Id = -1, Username = "未知用户", RoleName = "无" };
                }

                // 超级管理员总是拥有所有权限
                bool isSuperAdmin = CurrentUser.RoleName == "超级管理员";
                LogHelper.Log($"用户: {CurrentUser.Username}, 角色: {CurrentUser.RoleName}, 超级管理员: {isSuperAdmin}");

                // --- 菜单项权限检查 --- 
                // 使用 permissionChecker 检查各模块的 "view" 权限来决定是否显示顶级菜单

                // 工艺管理菜单
                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "process", "view"))
                {
                    ToolStripMenuItem processMenu = new ToolStripMenuItem("工艺管理");
                    processMenu.Name = "processMenu";
                    
                    // 创建工艺包规范菜单项
                    ToolStripMenuItem processPackageItem = new ToolStripMenuItem("工艺包规范");
                    processPackageItem.Name = "processPackageItem";
                    processPackageItem.Tag = "process.package.view";
                    processPackageItem.Click += MenuItem_Click;
                    processMenu.DropDownItems.Add(processPackageItem);
                    
                    mainMenu.Items.Add(processMenu);
                    LogHelper.Log("已添加 工艺管理 菜单 (权限检查通过)");
                }
                else { LogHelper.Log("未添加 工艺管理 菜单 (无 view 权限)"); }

                // 产品管理 
                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "product", "view"))
                {
                    ToolStripMenuItem productMenu = new ToolStripMenuItem("产品管理");
                    productMenu.DropDownItems.Add(CreateMenuItem("产品信息", "产品管理_产品信息", "product_view"));
                    productMenu.DropDownItems.Add(CreateMenuItem("产品类别", "产品管理_产品类别", "product_category_view"));
                    mainMenu.Items.Add(productMenu);
                    LogHelper.Log("已添加 产品管理 菜单 (权限检查通过)");
                }
                else { LogHelper.Log("未添加 产品管理 菜单 (无 view 权限)"); }

                // 生产管理
                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "production", "view")) 
                {
                    ToolStripMenuItem productionMenu = new ToolStripMenuItem("生产管理");
                    productionMenu.DropDownItems.Add(CreateMenuItem("生产计划", "生产管理_生产计划", "production_plan_view"));
                    productionMenu.DropDownItems.Add(CreateMenuItem("生产记录", "生产管理_生产记录", "production_record_view"));
                    mainMenu.Items.Add(productionMenu);
                     LogHelper.Log("已添加 生产管理 菜单 (权限检查通过)");
                }
                else { LogHelper.Log("未添加 生产管理 菜单 (无 view 权限)"); }

                // 设备管理
                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "equipment", "view"))
                {
                    ToolStripMenuItem equipmentMenu = new ToolStripMenuItem("设备管理");
                    equipmentMenu.DropDownItems.Add(CreateMenuItem("设备组管理", "设备管理_设备组管理", "equipment_group_view"));
                    mainMenu.Items.Add(equipmentMenu);
                    LogHelper.Log("已添加 设备管理 菜单 (权限检查通过)");
                }
                 else { LogHelper.Log("未添加 设备管理 菜单 (无 view 权限)"); }

                // 库存管理
                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "inventory", "view"))
                {
                    ToolStripMenuItem inventoryMenu = new ToolStripMenuItem("库存管理");
                    inventoryMenu.DropDownItems.Add(CreateMenuItem("库存信息", "库存管理_库存信息", "inventory_info_view"));
                    inventoryMenu.DropDownItems.Add(CreateMenuItem("库存盘点", "库存管理_库存盘点", "inventory_count_view"));
                    mainMenu.Items.Add(inventoryMenu);
                    LogHelper.Log("已添加 库存管理 菜单 (权限检查通过)");
                }
                 else { LogHelper.Log("未添加 库存管理 菜单 (无 view 权限)"); }

                 // 工厂管理 (假设存在 'factory' 模块权限)
                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "factory", "view"))
                {
                    ToolStripMenuItem factoryMenu = new ToolStripMenuItem("基础信息"); // 或者叫"基础信息"
                    // 假设有这些菜单项和窗体
                    factoryMenu.DropDownItems.Add(CreateMenuItem("工厂管理", "基础信息_工厂管理", "factory_view")); 
                    factoryMenu.DropDownItems.Add(CreateMenuItem("部门管理", "基础信息_部门管理", "department_view")); 
                    factoryMenu.DropDownItems.Add(CreateMenuItem("区域管理", "基础信息_区域管理", "area_view")); 
                    mainMenu.Items.Add(factoryMenu);
                    LogHelper.Log("已添加 基础信息(工厂/区域/部门) 菜单 (权限检查通过)");
                }
                else { LogHelper.Log("未添加 基础信息(工厂/区域/部门) 菜单 (无 factory view 权限)"); }

                // 用户管理 - 需要 "user" 模块的 "view" 权限
                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "user", "view"))
                {
                    ToolStripMenuItem userMenu = new ToolStripMenuItem("用户管理");
                    userMenu.DropDownItems.Add(CreateMenuItem("用户信息", "用户管理_用户信息", "user_view"));
                    // 角色权限菜单项本身需要 system:view 权限才能打开其对应窗体 (在CheckPermission中检查)
                    userMenu.DropDownItems.Add(CreateMenuItem("角色权限", "用户管理_权限设置", "permission_view")); 
                    mainMenu.Items.Add(userMenu);
                    LogHelper.Log("已添加 用户管理 菜单 (权限检查通过)");
                }
                 else { LogHelper.Log("未添加 用户管理 菜单 (无 user view 权限)"); }

                // 系统设置 - 基础的修改密码对所有用户开放，其他功能需要 "system" 模块的 "view" 权限
                ToolStripMenuItem systemMenu = new ToolStripMenuItem("系统设置");
                bool addedSystemItem = false;

                if (isSuperAdmin || permissionChecker.HasPermission(CurrentUser.Id, "system", "view"))
                {
                    systemMenu.DropDownItems.Add(CreateMenuItem("系统参数", "系统设置_系统参数", "system_params_view"));
                    systemMenu.DropDownItems.Add(CreateMenuItem("数据备份", "系统设置_数据备份", "data_backup_view"));
                    systemMenu.DropDownItems.Add(CreateMenuItem("操作日志", "系统设置_操作日志", "log_view"));
                    systemMenu.DropDownItems.Add(new ToolStripSeparator());
                    addedSystemItem = true;
                     LogHelper.Log("已添加 系统设置 (高级功能) 子菜单 (权限检查通过)");
                }
                 else { LogHelper.Log("未添加 系统设置 (高级功能) 子菜单 (无 system view 权限)"); }

                // 修改密码功能对所有登录用户开放 (CurrentUser 不为 null)
                if (CurrentUser.Id != -1) // 排除上面创建的临时未知用户
                {
                   systemMenu.DropDownItems.Add(CreateMenuItem("修改密码", "系统设置_修改密码", null));
                   addedSystemItem = true;
                }

                // 只有添加了子项才显示系统菜单
                if (addedSystemItem)
                {
                   mainMenu.Items.Add(systemMenu);
                }
                
                // 为菜单项添加统一的内边距 (保持不变)
                foreach (ToolStripMenuItem item in mainMenu.Items)
                {
                   item.Padding = new Padding(10, 0, 10, 0);
                   item.Margin = new Padding(2, 0, 2, 0);
                }
                LogHelper.Log("主菜单创建完成。");
            }
            catch (Exception ex)
            {
                LogHelper.Log($"创建菜单项时出错: {ex.Message}");
                MessageBox.Show($"创建菜单时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 可以在这里添加一个默认的最小化菜单
            }
        }
        
        // 自定义菜单颜色
        private class MenuColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(120, 170, 220); }
            }
            
            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(90, 140, 190); }
            }
            
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(120, 170, 220); }
            }
            
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(110, 160, 210); }
            }
            
            public override Color MenuBorder
            {
                get { return Color.FromArgb(80, 130, 180); }
            }
            
            public override Color ToolStripDropDownBackground
            {
                get { return Color.FromArgb(240, 245, 250); }
            }
        }
        
        // 创建菜单项
        private ToolStripMenuItem CreateMenuItem(string text, string name, string permissionTag = null)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem(text);
            menuItem.Name = name;
            menuItem.Tag = permissionTag;
            menuItem.Click += MenuItem_Click;
            return menuItem;
        }
        
        // 菜单项点击事件
        private void MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                // 如果是直接处理的菜单项（如修改密码），则不调用 OpenFunctionForm
                if (menuItem.Name == "系统设置_修改密码") 
                {
                    ChangePasswordMenu_Click(sender, e);
                    return;
                }
                 // 其他菜单项通过 OpenFunctionForm 处理，传递 Name 和 Tag
                OpenFunctionForm(menuItem.Name, menuItem.Tag as string);
            }
        }
        
        // 创建状态栏
        private void CreateStatusStrip()
        {
            // 不重新创建状态栏，使用设计器中已经创建的statusStrip
            try 
            {
                // 清除现有状态栏项
                statusStrip.Items.Clear();
                
                // 添加状态栏项
                ToolStripStatusLabel userLabel = new ToolStripStatusLabel();
                userLabel.Name = "userLabel";
                userLabel.Text = CurrentUser != null ? $"当前用户: {CurrentUser.Username} ({CurrentUser.RoleName})" : "未登录";
                statusStrip.Items.Add(userLabel);
                
                statusStrip.Items.Add(new ToolStripSeparator());
                
                ToolStripStatusLabel timeLabel = new ToolStripStatusLabel();
                timeLabel.Name = "timeLabel";
                timeLabel.Text = $"登录时间: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}";
                statusStrip.Items.Add(timeLabel);
                
                statusStrip.Items.Add(new ToolStripSeparator());
                
                ToolStripStatusLabel versionLabel = new ToolStripStatusLabel("系统版本: v1.0.0");
                statusStrip.Items.Add(versionLabel);
                
                // 确保状态栏可见
                statusStrip.Visible = true;
            }
            catch (Exception ex)
            {
                LogHelper.Log($"创建状态栏出错: {ex.Message}");
            }
        }
        
        // 更新状态栏信息
        private void UpdateStatusInfo()
        {
            try
            {
                if (statusStrip == null)
                {
                    LogHelper.Log("状态栏未初始化，无法更新信息");
                    return;
                }
                
                // 检查并更新用户信息
                string userText = CurrentUser != null 
                    ? $"当前用户: {CurrentUser.Username} ({CurrentUser.RoleName})" 
                    : "未登录";
                    
                ToolStripStatusLabel userLabel = statusStrip.Items["userLabel"] as ToolStripStatusLabel;
                if (userLabel != null)
                {
                    userLabel.Text = userText;
                }
                else
                {
                    LogHelper.Log("未找到用户标签，重新创建");
                    userLabel = new ToolStripStatusLabel(userText);
                    userLabel.Name = "userLabel";
                    statusStrip.Items.Add(userLabel);
                }
                
                // 检查并更新时间信息
                ToolStripStatusLabel timeLabel = statusStrip.Items["timeLabel"] as ToolStripStatusLabel;
                if (timeLabel != null)
                {
                    timeLabel.Text = $"登录时间: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}";
                }
                else
                {
                    LogHelper.Log("未找到时间标签，重新创建");
                    statusStrip.Items.Add(new ToolStripSeparator());
                    timeLabel = new ToolStripStatusLabel($"登录时间: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}");
                    timeLabel.Name = "timeLabel";
                    statusStrip.Items.Add(timeLabel);
                }
                
                // 更新版本信息为统一版本号
                ToolStripStatusLabel versionLabel = null;
                foreach (ToolStripItem item in statusStrip.Items)
                {
                    if (item is ToolStripStatusLabel label && label.Text.Contains("系统版本"))
                    {
                        versionLabel = label;
                        break;
                    }
                }
                
                if (versionLabel != null)
                {
                    versionLabel.Text = "版本: V9.9.9 © 2025 轩天帝";
                }
                else
                {
                    statusStrip.Items.Add(new ToolStripSeparator());
                    versionLabel = new ToolStripStatusLabel("版本: V9.9.9 © 2025 轩天帝");
                    statusStrip.Items.Add(versionLabel);
                }
                
                // 确保状态栏可见
                statusStrip.Visible = true;
            }
            catch (Exception ex)
            {
                LogHelper.Log($"更新状态栏信息出错: {ex.Message}");
            }
        }
        
        // 创建内容区域
        private void CreateContentArea()
        {
            // 不重新创建容器，使用设计器中已经创建的tabContainer和tabControl
            try
            {
                // 确保TabControl和容器可见
                tabContainer.Visible = true;
                tabControl.Visible = true;
                
                // 添加欢迎页
                ShowWelcomePage();
            }
            catch (Exception ex)
            {
                LogHelper.Log($"创建内容区域出错: {ex.Message}");
            }
        }
        
        // 恢复欢迎页
        private void ShowWelcomePage()
        {
            // 如果已有欢迎页则显示，否则不操作（由InitializeComponent创建）
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.Text == "欢迎页")
                {
                    tabControl.SelectedTab = page;
                    return;
                }
            }
        }
        
        // 绘制TabControl的选项卡
        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabControl tabControl = (TabControl)sender;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabRect = tabControl.GetTabRect(e.Index);

            // 扩展TabControl的背景使它更高，不被菜单遮挡
            Rectangle extendedBounds = new Rectangle(tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height);
            extendedBounds.Inflate(0, 8); // 增大垂直方向的扩展，确保标签背景足够高

            // 确定选项卡背景颜色
            using (Brush brushTabBackground = new SolidBrush(Color.FromArgb(e.State == DrawItemState.Selected ? 200 : 160, 220, 250)))
            {
                // 绘制选项卡背景，用圆角矩形
                using (GraphicsPath path = new GraphicsPath())
                {
                    int radius = 8; // 圆角半径
                    path.AddArc(extendedBounds.X, extendedBounds.Y, radius * 2, radius * 2, 180, 90);
                    path.AddArc(extendedBounds.Right - radius * 2, extendedBounds.Y, radius * 2, radius * 2, 270, 90);
                    path.AddArc(extendedBounds.Right - radius * 2, extendedBounds.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                    path.AddArc(extendedBounds.X, extendedBounds.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                    path.CloseAllFigures();

                    g.FillPath(brushTabBackground, path);

                    // 为选中标签添加底部边框
                    if (e.State == DrawItemState.Selected)
                    {
                        using (Pen borderPen = new Pen(Color.FromArgb(100, 140, 190), 3))
                        {
                            g.DrawLine(borderPen, extendedBounds.X, extendedBounds.Bottom - 2,
                                extendedBounds.Right, extendedBounds.Bottom - 2);
                        }
                    }
                }
            }

            // 绘制选项卡文本
            Rectangle textRect = tabRect;
            textRect.X += 5;
            textRect.Width -= 25; // 留出关闭按钮的空间
            textRect.Y += 5; // 调整文本垂直居中

            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                
                using (Brush textBrush = new SolidBrush(e.State == DrawItemState.Selected ? 
                    Color.FromArgb(30, 60, 110) : Color.FromArgb(60, 90, 140)))
                {
                    g.DrawString(tabPage.Text, tabControl.Font, textBrush, textRect, sf);
                }
            }

            // 绘制关闭按钮
            if (e.Index > 0) // 第一个选项卡不显示关闭按钮
            {
                Rectangle closeButtonRect = new Rectangle(
                    tabRect.Right - 20, 
                    tabRect.Y + tabRect.Height / 2 - 6, 
                    16, 
                    16);

                using (Brush closeBrush = new SolidBrush(Color.FromArgb(100, 130, 170)))
                using (Pen closePen = new Pen(closeBrush, 2))
                {
                    // 绘制 X 形状
                    g.DrawLine(closePen, closeButtonRect.X + 4, closeButtonRect.Y + 4,
                        closeButtonRect.X + closeButtonRect.Width - 4, closeButtonRect.Y + closeButtonRect.Height - 4);
                    g.DrawLine(closePen, closeButtonRect.X + closeButtonRect.Width - 4, closeButtonRect.Y + 4,
                        closeButtonRect.X + 4, closeButtonRect.Y + closeButtonRect.Height - 4);
                }
            }
        }
        
        // TabControl点击事件
        private void TabControl_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (tabControl == null || tabControl.TabPages.Count == 0)
                    return;
                
                // 获取点击的标签页索引
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    Rectangle tabRect = tabControl.GetTabRect(i);
                    
                    // 检查是否点击了关闭按钮区域
                    Rectangle closeButtonRect = new Rectangle(
                        tabRect.Right - 20, tabRect.Top + (tabRect.Height - 16) / 2, 16, 16);
                        
                    if (closeButtonRect.Contains(e.Location) && tabControl.TabPages[i].Text != "欢迎页")
                    {
                        TabPage tabPage = tabControl.TabPages[i];
                        
                        // 查找子窗体并关闭
                        foreach (Control control in tabPage.Controls)
                        {
                            if (control is Form childForm)
                            {
                                childForm.Close();
                                childForm.Dispose();
                                break;
                            }
                        }
                        
                        // 移除标签页
                        tabControl.TabPages.RemoveAt(i);
                        
                        // 如果没有标签页了，显示欢迎页
                        if (tabControl.TabPages.Count == 0)
                        {
                            ShowWelcomePage();
                        }
                        
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"关闭标签页时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Log($"关闭标签页出错: {ex.Message}");
            }
        }
        
        // 打开功能窗体
        private void OpenFunctionForm(string formName, string permissionTag)
        {
            // 权限检查放在首位
            if (!CheckPermission(permissionTag))
            {
                MessageBox.Show("您没有足够的权限执行此操作。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogHelper.Log($"用户 {CurrentUser.Username} 尝试打开 {formName} (Tag: {permissionTag}) 但权限不足。");
                return;
            }

            try
            {
                LogHelper.Log($"尝试打开窗体: {formName} (Tag: {permissionTag})");
                // 检查TabControl是否已初始化
                if (tabControl == null)
                {
                    LogHelper.Log("错误: tabControl 未初始化。");
                    MessageBox.Show("界面控件尚未完全加载，请稍后再试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
                
                // 检查窗体是否已打开
                foreach (TabPage tp in tabControl.TabPages)
                {
                    if (tp.Name == formName) // Use unique formName (menu item Name)
                    {
                        tabControl.SelectedTab = tp; // 切换到已存在的标签页
                        LogHelper.Log($"窗体 {formName} 已存在，切换到标签页。");
                        return;
                }
                }

                // 如果窗体未打开，创建新实例
                Form form = null;
                string displayName = GetDisplayName(formName);

                // 使用 formName (menu item Name) 来决定创建哪个窗体
                switch (formName)
                {
                    // 产品管理
                    case "产品管理_产品信息":
                        form = new FrmProduct(CurrentUser);
                        break;
                    case "产品管理_产品类别":
                        form = new FrmProductCategory(CurrentUser);
                        break;

                    // 生产管理
                    case "生产管理_生产计划":
                        form = new FrmProductionPlan(CurrentUser);
                        break;
                    case "生产管理_生产记录":
                        form = new FrmProductionRecord(CurrentUser);
                        break;

                    // 设备管理
                    case "设备管理_设备组管理": // 添加新的 case
                        form = new FrmEqpGroup(CurrentUser); // 使用新的 FrmEqpGroup
                        break;

                    // 库存管理
                    case "库存管理_库存信息":
                        form = new FrmInventoryInfo(CurrentUser);
                        break;
                    case "库存管理_库存盘点":
                        form = new FrmInventoryCount(CurrentUser);
                        break;

                    // 基础信息
                    case "基础信息_工厂管理":
                        form = new FrmFactory(CurrentUser);
                        break;
                    case "基础信息_部门管理":
                        form = new FrmDepartment(CurrentUser);
                        break;
                    case "基础信息_区域管理": // Added Area
                        form = new FrmArea(CurrentUser);
                        break;
                        
                    // 工艺管理
                    case "processPackageItem":
                        form = new FrmProcessManagement(CurrentUser);
                        displayName = "工艺管理"; // 明确设置中文显示名称
                        break;

                    // 系统管理 (原 User Management in CreateMainMenu)
                    case "用户管理_用户信息": // Corrected Case Label
                        form = new FrmUser(CurrentUser);
                         break;
                    case "用户管理_权限设置": // Corrected Case Label
                        form = new FrmPermission(CurrentUser);
                         break;
                    // The following belong to the "系统设置" top-level menu in CreateMainMenu
                    case "系统设置_操作日志": // Corrected Case Prefix
                        form = new FrmOperationLog(CurrentUser);
                         break;
                    case "系统设置_系统参数": // Corrected Case Prefix
                        form = new FrmSystemParameters(CurrentUser);
                        break;
                    
                    // 数据管理 (Corrected Case Prefix based on CreateMainMenu)
                    case "系统设置_数据备份": // Corrected Case Prefix
                        form = new FrmDataBackup(CurrentUser);
                        break;

                    // 帮助菜单项 (Assuming Help_About is still handled)
                    case "Help_About":
                        MessageBox.Show("关于信息待添加。", "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         break;

                    default:
                        // Check if it's a known category but not handled (e.g., a sub-item under a handled top-level)
                        if (formName.StartsWith("产品管理_") || formName.StartsWith("生产管理_") || 
                            formName.StartsWith("设备管理_") || formName.StartsWith("库存管理_") || 
                            formName.StartsWith("基础信息_") || formName.StartsWith("process"))
                         {
                             MessageBox.Show($"功能 '{displayName}' 尚未实现或配置错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         LogHelper.Log($"尝试打开未实现的窗体: {formName}");
                         }
                         else 
                         {
                            MessageBox.Show($"未知的菜单项: {formName}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogHelper.Log($"错误: 未能识别的菜单项名称 {formName}");
                         }
                        break;
                }

                // 如果窗体创建成功，则添加到TabControl
                if (form != null)
                {
                    LogHelper.Log($"窗体 {formName} 创建成功，准备添加到 TabControl。");
                    TabPage newTabPage = new TabPage(displayName);
                    newTabPage.Name = formName; // Use unique formName for identification
                    newTabPage.BackColor = Color.White; // 设置背景色
                    
                    form.TopLevel = false;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;

                    // 调整子窗体 UI 样式
                    AdjustChildFormUI(form);
                    
                    newTabPage.Controls.Add(form);
                    tabControl.TabPages.Add(newTabPage);
                    tabControl.SelectedTab = newTabPage; // 选中新创建的标签页
                    
                    // 确保窗体显示
                    form.Show();
                    
                    // 尝试确保所有控件可见
                    EnsureFormControlsVisible(form);
                    LogHelper.Log($"窗体 {formName} 已添加到 TabControl 并显示。");
                }
                else if (formName != "Help_About") // Don't log error for handled 'About' case
                {
                   LogHelper.Log($"错误: 窗体实例未能为 {formName} 创建。");
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show($"打开功能窗体 '{formName}' 时出错: {ex.Message}\n{ex.StackTrace}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 LogHelper.Log($"打开窗体 {formName} 时异常: {ex.Message}");
            }
        }
        
        // 获取功能的显示名称
        private string GetDisplayName(string formName)
        {
            if (formName.Contains("_"))
            {
                string[] parts = formName.Split('_');
                return parts[parts.Length - 1]; // 使用最后一部分作为显示名称
            }
            return formName;
        }
        
        // 调整子窗体UI
        private void AdjustChildFormUI(Form childForm)
        {
            try
            {
                // 为子窗体添加顶部边距，避免内容被菜单遮挡
                childForm.Padding = new Padding(0, 0, 0, 0);
                
                // 确保所有窗体的控件都正确显示
                EnsureFormControlsVisible(childForm);
                
                // 处理特殊窗体类型的额外调整
                if (childForm is FrmFactory || childForm is FrmArea)
                {
                    // 这些窗体通常有工具栏面板，确保它们有足够的上边距
                    foreach (Control control in childForm.Controls)
                    {
                        if (control is Panel panel && panel.Dock == DockStyle.Top)
                        {
                            panel.Padding = new Padding(panel.Padding.Left, 2, panel.Padding.Right, panel.Padding.Bottom);
                        }
                    }
                }
                
                // 对于FrmProductionRecord窗体，保留其顶部面板内的标签
                if (childForm is FrmProductionRecord)
                {
                    return; // 不对其标签进行特殊处理
                }
                
                // 遍历子窗体中的顶部标签，隐藏可能与标签页标题重复的标签
                foreach (Control control in childForm.Controls)
                {
                    if (control is Label lbl && lbl.Text.Contains("管理") && lbl.Font.Size >= 12)
                    {
                        lbl.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log($"调整子窗体UI时出错: {ex.Message}");
            }
        }
        
        // 确保窗体中所有控件都可见和正确显示的通用方法
        private void EnsureFormControlsVisible(Form form)
        {
            try
            {
                // 挂起布局以提高性能
                form.SuspendLayout();
                
                // 考虑窗体已经在构造函数中调用了InitializeControls的情况
                // 如果窗体是FrmProductionRecord类，它直接在构造函数中调用InitializeControls
                if (!(form is FrmProductionRecord)) 
                {
                    // 调用InitializeControls方法（如果存在）
                    System.Reflection.MethodInfo initMethod = form.GetType().GetMethod("InitializeControls", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        
                    if (initMethod != null)
                    {
                        try
                        {
                            initMethod.Invoke(form, null);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log($"调用窗体 {form.GetType().Name} 的InitializeControls方法时出错: {ex.Message}");
                        }
                    }
                }
                
                // 调用CreateControls方法（如果存在）
                System.Reflection.MethodInfo createMethod = form.GetType().GetMethod("CreateControls", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
                if (createMethod != null)
                {
                    try
                    {
                        createMethod.Invoke(form, null);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log($"调用窗体 {form.GetType().Name} 的CreateControls方法时出错: {ex.Message}");
                    }
                }
                
                // 确保所有控件可见
                MakeAllControlsVisible(form);
                
                // 恢复布局并刷新界面
                form.ResumeLayout(true);
                form.PerformLayout();
                form.Refresh();
            }
            catch (Exception ex)
            {
                LogHelper.Log($"确保窗体 {form.GetType().Name} 的控件可见时出错: {ex.Message}");
            }
        }
        
        // 递归使所有控件可见
        private void MakeAllControlsVisible(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                control.Visible = true;
                
                // 递归处理子控件
                if (control.Controls.Count > 0)
                {
                    MakeAllControlsVisible(control);
                }
            }
        }
        
        // 检查用户是否有权限访问特定模块 (重构逻辑)
        private bool CheckPermission(string permissionTag)
        {
            // 如果 Tag 为空或 null，则认为不需要权限检查 (或者根据业务规则决定是否拒绝)
            if (string.IsNullOrEmpty(permissionTag))
            {   
                LogHelper.Log("权限检查: permissionTag 为空，跳过检查。");
                return true; // Or false if empty tag means forbidden
            }
            
            // 超级管理员跳过检查
            if (CurrentUser?.RoleName == "超级管理员")
            {
                LogHelper.Log($"权限检查 ({permissionTag}): 用户是超级管理员，授权访问。");
                return true;
            }

            // 检查 CurrentUser 是否有效
             if (CurrentUser == null)
            {
                 LogHelper.Log($"权限检查 ({permissionTag}): CurrentUser 为 null，拒绝访问。");
                return false;
            }

            // 解析 Tag, 假设格式为 "module_action"
            string[] parts = permissionTag.Split('_');
            if (parts.Length < 2) // 需要至少两部分，或者根据你的 Tag 设计调整
            {
                LogHelper.Log($"权限检查 ({permissionTag}): Tag 格式无效，无法解析模块和操作。");
                // 决定如何处理无效 Tag，是允许还是拒绝？暂时允许以保持原行为
                // return false; 
                return true; // 暂时允许，但应修复 Tag 或处理逻辑
            }

            // 通常最后一部分是操作，前面的部分组合起来是模块
            string action = parts[parts.Length - 1];
            string module = string.Join("_", parts, 0, parts.Length - 1);
            
             // 特殊处理简化模块名 (保持不变)
             if (permissionTag.StartsWith("product_")) module = "product";
             else if (permissionTag.StartsWith("production_")) module = "production";
             else if (permissionTag.StartsWith("equipment_")) module = "equipment";
             else if (permissionTag.StartsWith("inventory_")) module = "inventory";
             else if (permissionTag.StartsWith("factory_") || permissionTag.StartsWith("department_") || permissionTag.StartsWith("area_")) module = "factory"; // Group base info under factory?
             else if (permissionTag.StartsWith("user_") || permissionTag.StartsWith("permission_") || permissionTag.StartsWith("log_") || permissionTag.StartsWith("system_")) module = "system";
             else if (permissionTag.StartsWith("data_")) module = "data";
             else module = "unknown"; // 未知模块
            
            // --- 去除 Action 简化逻辑 ---
            // 不再将所有包含 "view" 的 Tag 都视为 "view" 操作
            // if (action.Contains("view")) action = "view"; 
            // 现在使用从 Tag 中精确提取的 action，例如 "view", "category_view", "plan_view" 等
            // --- End of removal ---

            LogHelper.Log($"权限检查 ({permissionTag}): 解析为 Module='{module}', Action='{action}' for UserID={CurrentUser.Id}");

            // 调用实际的权限检查器 (使用精确的 action)
            bool hasPermission = permissionChecker.HasPermission(CurrentUser.Id, module, action);

            LogHelper.Log($"权限检查 ({permissionTag}): 结果 = {hasPermission}");
            return hasPermission;
        }
        
        // 修改密码功能
        private void ChangePasswordMenu_Click(object sender, EventArgs e)
        {
            using (Form passwordForm = new Form())
            {
                passwordForm.Text = "修改密码";
                passwordForm.Size = new Size(400, 250);
                passwordForm.StartPosition = FormStartPosition.CenterParent;
                passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                passwordForm.MaximizeBox = false;
                passwordForm.MinimizeBox = false;

                // 创建控件
                Label lblOldPassword = new Label();
                lblOldPassword.Text = "原密码:";
                lblOldPassword.AutoSize = true;
                lblOldPassword.Location = new Point(50, 30);

                TextBox txtOldPassword = new TextBox();
                txtOldPassword.Location = new Point(150, 27);
                txtOldPassword.Size = new Size(180, 25);
                txtOldPassword.PasswordChar = '*';

                Label lblNewPassword = new Label();
                lblNewPassword.Text = "新密码:";
                lblNewPassword.AutoSize = true;
                lblNewPassword.Location = new Point(50, 70);

                TextBox txtNewPassword = new TextBox();
                txtNewPassword.Location = new Point(150, 67);
                txtNewPassword.Size = new Size(180, 25);
                txtNewPassword.PasswordChar = '*';

                Label lblConfirmPassword = new Label();
                lblConfirmPassword.Text = "确认新密码:";
                lblConfirmPassword.AutoSize = true;
                lblConfirmPassword.Location = new Point(50, 110);

                TextBox txtConfirmPassword = new TextBox();
                txtConfirmPassword.Location = new Point(150, 107);
                txtConfirmPassword.Size = new Size(180, 25);
                txtConfirmPassword.PasswordChar = '*';

                Button btnSave = new Button();
                btnSave.Text = "保存";
                btnSave.Location = new Point(100, 160);
                btnSave.Size = new Size(80, 30);

                Button btnCancel = new Button();
                btnCancel.Text = "取消";
                btnCancel.Location = new Point(200, 160);
                btnCancel.Size = new Size(80, 30);
                btnCancel.DialogResult = DialogResult.Cancel;

                // 添加控件到窗体
                passwordForm.Controls.Add(lblOldPassword);
                passwordForm.Controls.Add(txtOldPassword);
                passwordForm.Controls.Add(lblNewPassword);
                passwordForm.Controls.Add(txtNewPassword);
                passwordForm.Controls.Add(lblConfirmPassword);
                passwordForm.Controls.Add(txtConfirmPassword);
                passwordForm.Controls.Add(btnSave);
                passwordForm.Controls.Add(btnCancel);

                // 设置保存按钮点击事件
                btnSave.Click += (s, args) =>
                {
                    try
                    {
                        // 验证输入
                        if (string.IsNullOrEmpty(txtOldPassword.Text) ||
                            string.IsNullOrEmpty(txtNewPassword.Text) ||
                            string.IsNullOrEmpty(txtConfirmPassword.Text))
                        {
                            MessageBox.Show("所有字段不能为空！", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // 验证原密码 - 使用加密后的比较
                        bool isOldPasswordValid = MDMUI.Utility.PasswordEncryptor.VerifyPassword(
                            txtOldPassword.Text, CurrentUser.Password);

                        if (!isOldPasswordValid)
                        {
                            MessageBox.Show("原密码不正确！", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (txtNewPassword.Text != txtConfirmPassword.Text)
                        {
                            MessageBox.Show("新密码和确认密码不一致！", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // 修改密码 - 使用加密
                        if (userDAL.ChangePassword(CurrentUser.Id, txtNewPassword.Text))
                        {
                            // 更新当前用户密码 - 使用加密后的密码
                            CurrentUser.Password = MDMUI.Utility.PasswordEncryptor.EncryptPassword(txtNewPassword.Text);

                            MessageBox.Show("密码修改成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            passwordForm.DialogResult = DialogResult.OK;
                            passwordForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("密码修改失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("密码修改失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                passwordForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// 初始化标签控件
        /// </summary>
        private void InitTabControl()
        {
            // 基本设置
            tabControl.Multiline = false;
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(130, 40); // 调整高度为40
            tabControl.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold); // 使用粗体字
            tabControl.Padding = new Point(15, 8); // 增加顶部Padding确保标签不被菜单栏遮挡
            
            // 设置背景色和边框样式
            tabControl.BackColor = Color.FromArgb(240, 245, 250);
            tabControl.Appearance = TabAppearance.Normal;
            
            // 添加事件处理程序
            tabControl.DrawItem += new DrawItemEventHandler(TabControl_DrawItem);
            tabControl.MouseDown += new MouseEventHandler(TabControl_MouseDown);
            tabControl.SelectedIndexChanged += new EventHandler(TabControl_SelectedIndexChanged);

            // 为TabControl添加欢迎页
            AddWelcomeTab();
        }

        // 添加欢迎标签页
        private void AddWelcomeTab()
        {
            // 不需要重复代码，使用InitializeComponent中的代码
        }

        // TabControl鼠标按下事件处理
        private void TabControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                // 右键点击处理
                if (e.Button == MouseButtons.Right)
                {
                    // 查找点击的标签页
                    for (int i = 0; i < tabControl.TabPages.Count; i++)
                    {
                        if (tabControl.GetTabRect(i).Contains(e.Location))
                        {
                            // 可以在这里添加右键菜单
                            // 例如：关闭所有标签，关闭其他标签等功能
                            LogHelper.Log($"右键点击了标签页: {tabControl.TabPages[i].Text}");
                            break;
                        }
                    }
                }
                
                // 中键点击处理（关闭标签页）
                if (e.Button == MouseButtons.Middle)
                {
                    for (int i = 0; i < tabControl.TabPages.Count; i++)
                    {
                        if (tabControl.GetTabRect(i).Contains(e.Location) && tabControl.TabPages[i].Text != "欢迎页")
                        {
                            TabPage tabPage = tabControl.TabPages[i];
                            
                            // 查找子窗体并关闭
                            foreach (Control control in tabPage.Controls)
                            {
                                if (control is Form childForm)
                                {
                                    childForm.Close();
                                    childForm.Dispose();
                                    break;
                                }
                            }
                            
                            // 移除标签页
                            tabControl.TabPages.RemoveAt(i);
                            
                            // 如果没有标签页了，显示欢迎页
                            if (tabControl.TabPages.Count == 0)
                            {
                                ShowWelcomePage();
                            }
                            
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"鼠标操作标签页时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Log($"鼠标操作标签页出错: {ex.Message}");
            }
        }
        
        // TabControl标签切换事件
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl.SelectedTab != null)
                {
                    LogHelper.Log($"切换到标签页: {tabControl.SelectedTab.Text}");
                    
                    // 激活子窗体（如果存在）
                    foreach (Control control in tabControl.SelectedTab.Controls)
                    {
                        Panel panel = control as Panel;
                        if (panel != null)
                        {
                            foreach (Control panelControl in panel.Controls)
                            {
                                if (panelControl is Form childForm)
                                {
                                    if (!childForm.ContainsFocus)
                                    {
                                        childForm.Activate();
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log($"标签页切换出错: {ex.Message}");
            }
        }

        // 调整TabContainer的边距
        private void InitTabContainer()
        {
            tabContainer.Dock = DockStyle.Fill;
            tabContainer.Padding = new Padding(20, 40, 20, 20); // 增加顶部距离到40像素
            tabContainer.BackColor = Color.FromArgb(240, 245, 250);
            tabContainer.Controls.Add(tabControl);
        }

        // 添加窗体关闭事件处理函数
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // MessageBox.Show($"MainForm_FormClosing 事件触发，关闭原因: {e.CloseReason}, Cancel State: {e.Cancel}"); // 移除 MessageBox
            Console.WriteLine($"MainForm_FormClosing 事件触发，关闭原因: {e.CloseReason}"); 
            LogHelper.Log($"MainForm_FormClosing 事件触发，关闭原因: {e.CloseReason}"); // 使用 LogHelper
            
            // !! 恢复关闭子窗体的逻辑 !!
            try 
            {
                LogHelper.Log("应用程序正在关闭... 正在关闭子窗体。");
                
                List<TabPage> pagesToClose = tabControl.TabPages.Cast<TabPage>().ToList();
                foreach (TabPage tabPage in pagesToClose)
                {
                    if (tabPage.Text == "欢迎页") continue; 

                    Form childForm = null;
                    Panel contentPanel = tabPage.Controls.OfType<Panel>().FirstOrDefault();
                    if(contentPanel != null)
                    {
                       childForm = contentPanel.Controls.OfType<Form>().FirstOrDefault();
                    }
                    else
                    {   
                        childForm = tabPage.Controls.OfType<Form>().FirstOrDefault();
                    }

                    if (childForm != null)
                    {
                        LogHelper.Log($"尝试关闭 TabPage '{tabPage.Text}' 中的子窗体: {childForm.GetType().Name}"); // 更详细日志
                        try 
                        {
                            childForm.Close();
                            if (!childForm.IsDisposed)
                            {
                                childForm.Dispose();
                                LogHelper.Log($"子窗体 {childForm.GetType().Name} 已 Dispose。"); // 确认 Dispose
                            }
                        }
                        catch (Exception ex)
                        {   
                            LogHelper.Log($"关闭子窗体 {childForm.GetType().Name} 时出错: {ex.Message}");
                        }
                    }
                    else 
                    {
                        LogHelper.Log($"在 TabPage '{tabPage.Text}' 中未找到子窗体进行关闭。"); // 记录未找到的情况
                    }
                }
                LogHelper.Log("所有子窗体处理完毕。"); // 标记循环结束
            }
            catch (Exception ex)
            {
                LogHelper.Log($"关闭子窗体时发生意外错误: {ex.Message}");
            }
            // !! 结束恢复的逻辑 !!

            Console.WriteLine($"MainForm_FormClosing: e.Cancel = {e.Cancel}"); 
            LogHelper.Log($"MainForm_FormClosing: e.Cancel = {e.Cancel}"); // 使用 LogHelper

            if (e.Cancel)
            {
                LogHelper.Log("MainForm 关闭被取消");
            }
            else
            {
                LogHelper.Log("MainForm 正在关闭，准备调用 Application.Exit()。");
                // 可以添加必要的清理代码

                // !! 恢复 Application.Exit() 调用 !!
                Application.Exit(); 
                LogHelper.Log("Application.Exit() 已调用。"); // 确认调用
            }
        }
    }
}
