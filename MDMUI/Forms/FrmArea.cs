using System;
using System.Collections.Generic;
using System.Data;
// using System.Data.SqlClient; // 移除直接的 SQL Client 引用
using System.Drawing;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;
using System.Linq;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;

namespace MDMUI
{
    public partial class FrmArea : Form
    {
        // 移除数据库连接字符串
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True";

        // 当前登录用户
        private User currentUser;
        // 权限检查器
        private PermissionChecker permissionChecker;
        // 区域服务
        private AreaService areaService; // 添加 AreaService 引用

        // Modern UI (Atomic Design) - runtime enhancement (OCP: do not rewrite Designer)
        private bool modernLayoutInitialized;
        private CardPanel headerCard;
        private Label headerInfoLabel;
        private Panel treeHostPanel;
        private Label treeEmptyStateLabel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        public FrmArea(User user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.permissionChecker = new PermissionChecker();
            this.areaService = new AreaService(); // 初始化 AreaService

            InitializeModernLayout();
        }

        private void InitializeModernLayout()
        {
            if (modernLayoutInitialized) return;
            modernLayoutInitialized = true;

            try
            {
                using (AppTelemetry.Measure("FrmArea.ModernLayout"))
                {
                    EnsureHeaderCard();
                    EnsureTreeEmptyState();
                    EnsureStatusStrip();

                    UpdateHeaderInfo();
                    UpdateTreeIndicators(0);

                    // UiThemingBootstrapper 只对窗体执行一次；这里确保运行时新增控件也能吃到统一风格
                    try { ThemeManager.ApplyTo(this); } catch { }
                    try { ModernTheme.EnableMicroInteractions(this); } catch { }
                }
            }
            catch (Exception ex)
            {
                try { AppLog.Error(ex, "初始化区域管理现代化布局失败"); } catch { }
            }
        }

        private void EnsureHeaderCard()
        {
            if (headerCard != null) return;

            Label oldTitle = lblTitle;
            Label oldInfo = lblInfo;
            Panel oldToolPanel = toolPanel;

            Button oldAdd = btnAdd;
            Button oldEdit = btnEdit;
            Button oldDelete = btnDelete;
            Button oldRefresh = btnRefresh;

            CardPanel header = new CardPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(12),
                Height = 128
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            header.Controls.Add(layout);

            ActionToolbar actionsToolbar = new ActionToolbar { Dock = DockStyle.Fill };

            Label title = new Label
            {
                AutoSize = true,
                Text = string.IsNullOrWhiteSpace(Text) ? (oldTitle?.Text ?? "区域管理") : Text,
                ForeColor = ThemeManager.Palette.TextPrimary,
                Margin = new Padding(0, 12, 12, 0)
            };
            try { title.Font = ThemeManager.CreateTitleFont(12f); } catch { }
            actionsToolbar.LeftPanel.Controls.Add(title);

            AppButton add = CreateAppButton(oldAdd?.Text ?? "添加", AppButtonVariant.Primary, BtnAdd_Click, name: "btnAdd");
            AppButton edit = CreateAppButton(oldEdit?.Text ?? "编辑", AppButtonVariant.Secondary, BtnEdit_Click, name: "btnEdit");
            AppButton delete = CreateAppButton(oldDelete?.Text ?? "删除", AppButtonVariant.Danger, BtnDelete_Click, name: "btnDelete");
            AppButton refresh = CreateAppButton(oldRefresh?.Text ?? "刷新", AppButtonVariant.Secondary, BtnRefresh_Click, name: "btnRefresh");

            CopyButtonState(oldAdd, add);
            CopyButtonState(oldEdit, edit);
            CopyButtonState(oldDelete, delete);
            CopyButtonState(oldRefresh, refresh);

            btnAdd = add;
            btnEdit = edit;
            btnDelete = delete;
            btnRefresh = refresh;

            actionsToolbar.RightPanel.Controls.Add(add);
            actionsToolbar.RightPanel.Controls.Add(edit);
            actionsToolbar.RightPanel.Controls.Add(delete);
            actionsToolbar.RightPanel.Controls.Add(refresh);

            Label info = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Palette.TextSecondary,
                BackColor = Color.Transparent,
                AutoEllipsis = true,
                Margin = new Padding(0)
            };
            try { info.Font = ThemeManager.CreateBodyFont(9f); } catch { }
            headerInfoLabel = info;

            layout.Controls.Add(actionsToolbar, 0, 0);
            layout.Controls.Add(info, 0, 1);

            Controls.Add(header);
            header.BringToFront();
            headerCard = header;

            // 移除并释放旧控件（运行时替换，保持 Designer 不动）
            try { if (oldTitle != null) Controls.Remove(oldTitle); } catch { }
            try { if (oldInfo != null) Controls.Remove(oldInfo); } catch { }
            try { if (oldToolPanel != null) Controls.Remove(oldToolPanel); } catch { }

            try { oldTitle?.Dispose(); } catch { }
            try { oldInfo?.Dispose(); } catch { }
            try { oldToolPanel?.Dispose(); } catch { }

            // 避免后续误用已释放控件引用
            lblTitle = null;
            lblInfo = null;
            toolPanel = null;
        }

        private void EnsureTreeEmptyState()
        {
            if (treeHostPanel != null) return;
            if (splitContainer == null || treeView == null) return;

            Control panel1 = splitContainer.Panel1;
            if (panel1 == null) return;

            try { panel1.Controls.Remove(treeView); } catch { }

            Panel host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            try
            {
                treeView.Dock = DockStyle.Fill;
                treeView.BorderStyle = BorderStyle.None;
            }
            catch { }

            host.Controls.Add(treeView);

            Label empty = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = ThemeManager.Palette.TextSecondary,
                BackColor = Color.Transparent,
                Visible = false
            };
            try { empty.Font = ThemeManager.CreateBodyFont(9f); } catch { }

            host.Controls.Add(empty);
            try { empty.BringToFront(); } catch { }

            panel1.Controls.Add(host);

            treeHostPanel = host;
            treeEmptyStateLabel = empty;
        }

        private void EnsureStatusStrip()
        {
            if (statusStrip != null) return;

            StatusStrip strip = new StatusStrip
            {
                Dock = DockStyle.Bottom,
                SizingGrip = false
            };

            ToolStripStatusLabel label = new ToolStripStatusLabel { Text = "就绪" };
            strip.Items.Add(label);

            Controls.Add(strip);

            statusStrip = strip;
            statusLabel = label;
        }

        private void UpdateHeaderInfo()
        {
            string username = currentUser?.Username ?? "-";
            string factoryId = currentUser?.FactoryId ?? "-";

            string text = $"当前用户: {username}，工厂ID: {factoryId}";

            if (headerInfoLabel != null)
            {
                headerInfoLabel.Text = text;
            }
            else if (lblInfo != null)
            {
                lblInfo.Text = text;
            }
        }

        private void UpdateStatus(string message)
        {
            if (statusLabel != null)
            {
                statusLabel.Text = string.IsNullOrWhiteSpace(message) ? "就绪" : message.Trim();
            }
        }

        private void UpdateTreeIndicators(int areaCount)
        {
            if (treeEmptyStateLabel != null)
            {
                if (areaCount <= 0)
                {
                    treeEmptyStateLabel.Text = "暂无区域数据";
                    treeEmptyStateLabel.Visible = true;
                    try { treeEmptyStateLabel.BringToFront(); } catch { }
                }
                else
                {
                    treeEmptyStateLabel.Visible = false;
                }
            }

            if (areaCount <= 0)
            {
                UpdateStatus("暂无区域数据");
            }
            else
            {
                UpdateStatus($"共 {areaCount} 个区域");
            }
        }

        private static AppButton CreateAppButton(string text, AppButtonVariant variant, EventHandler onClick, string name)
        {
            AppButton button = new AppButton
            {
                Text = text,
                Variant = variant
            };

            if (!string.IsNullOrWhiteSpace(name))
            {
                button.Name = name.Trim();
            }

            try { button.AutoSize = true; } catch { }
            try { button.MinimumSize = new Size(86, 34); } catch { }

            if (onClick != null)
            {
                button.Click += onClick;
            }

            return button;
        }

        private static void CopyButtonState(Button source, Button target)
        {
            if (source == null || target == null) return;

            try { target.Visible = source.Visible; } catch { }
            try { target.Enabled = source.Enabled; } catch { }
            try { target.TabIndex = source.TabIndex; } catch { }
        }

        private void FrmArea_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateHeaderInfo();

                // Load data and apply permissions
                LoadData();
                SetButtonVisibility();

                Console.WriteLine("区域管理窗体已加载"); // 修改日志文本
            }
            catch (Exception ex)
            {
                MessageBox.Show($"区域管理窗体加载错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"区域管理窗体加载错误: {ex.Message}");
            }
        }

        private void SetButtonVisibility()
        {
            if (currentUser == null) return;
            bool isAdmin = currentUser.RoleName == "超级管理员";
            // Assuming buttons are now declared in Designer.cs
            // 检查设计器中是否声明了这些按钮
            Control btnAddCtrl = this.Controls.Find("btnAdd", true).FirstOrDefault();
            Control btnEditCtrl = this.Controls.Find("btnEdit", true).FirstOrDefault();
            Control btnDeleteCtrl = this.Controls.Find("btnDelete", true).FirstOrDefault();

            if (btnAddCtrl == null || btnEditCtrl == null || btnDeleteCtrl == null)
            {
                 Console.WriteLine("警告: FrmArea 中的一个或多个按钮（btnAdd, btnEdit, btnDelete）未在设计器中找到。");
                 return;
            }

            if (isAdmin)
            {
                 btnAddCtrl.Visible = true;
                 btnEditCtrl.Visible = true;
                 btnDeleteCtrl.Visible = true;
                 return;
            }

            btnAddCtrl.Visible = permissionChecker.HasPermission(currentUser.Id, "area", "add");
            btnEditCtrl.Visible = permissionChecker.HasPermission(currentUser.Id, "area", "edit");
            btnDeleteCtrl.Visible = permissionChecker.HasPermission(currentUser.Id, "area", "delete");
        }

        private void LoadData()
        {
            try
            {
                using (AppTelemetry.Measure("area.load_tree"))
                {
                    // 不再需要 EnsureAreaTableExists()
                    treeView.Nodes.Clear();

                    // 从 AreaService 获取数据
                    List<Area> areas = areaService.GetAllAreas();
                    int count = areas?.Count ?? 0;

                    // 使用 Area 对象构建树
                    if (count > 0)
                    {
                        BuildAreaTree(areas, null, null);
                        treeView.ExpandAll();
                    }

                    // 清空详细信息或加载根节点（如果需要）
                    LoadAreaDetails(null);

                    UpdateTreeIndicators(count);
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "加载区域数据失败");
                MessageBox.Show("加载区域数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateTreeIndicators(0);
            }
        }

        private void BuildAreaTree(List<Area> allAreas, string parentAreaId, TreeNode parentNode)
        {
            // 查找当前级别的子区域
            List<Area> children;
            if (string.IsNullOrEmpty(parentAreaId))
            {
                children = allAreas.Where(a => string.IsNullOrEmpty(a.ParentAreaId)).OrderBy(a => a.AreaName).ToList();
            }
            else
            {
                children = allAreas.Where(a => a.ParentAreaId == parentAreaId).OrderBy(a => a.AreaName).ToList();
            }

            foreach (Area area in children)
            {
                TreeNode node = new TreeNode(area.AreaName);
                node.Tag = area.AreaId; // 将 AreaId 存储在 Tag 中

                if (parentNode == null)
                {
                    treeView.Nodes.Add(node);
                }
                else
                {
                    parentNode.Nodes.Add(node);
                }

                // 递归构建子树
                BuildAreaTree(allAreas, area.AreaId, node);
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)   
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                string selectedAreaId = e.Node.Tag.ToString();
                LoadAreaDetails(selectedAreaId);
                UpdateStatus($"已选择: {e.Node.Text}");
            }
            else
            {
                LoadAreaDetails(null); // 清空详细信息
                UpdateStatus("请选择区域查看详情");
            }
        }

        private void LoadAreaDetails(string areaId)
        {
             // 假设详细信息标签在设计器中已创建 (lblDetailAreaIdValue, ...)
             Control lblId = this.Controls.Find("lblDetailAreaIdValue", true).FirstOrDefault();
             Control lblName = this.Controls.Find("lblDetailAreaNameValue", true).FirstOrDefault();
             Control lblParent = this.Controls.Find("lblDetailParentAreaValue", true).FirstOrDefault();
             Control lblPostal = this.Controls.Find("lblDetailPostalCodeValue", true).FirstOrDefault();
             Control lblRemark = this.Controls.Find("lblDetailRemarkValue", true).FirstOrDefault();

             if (lblId == null || lblName == null || lblParent == null || lblPostal == null || lblRemark == null)
             {
                 Console.WriteLine("警告: FrmArea 中的一个或多个详细信息标签未找到。");
                 return; // 如果标签不存在，则不继续执行
             }


            if (string.IsNullOrEmpty(areaId))
            {
                // 清空详细信息
                lblId.Text = "";
                lblName.Text = "";
                lblParent.Text = "";
                lblPostal.Text = "";
                lblRemark.Text = "";
                UpdateStatus("请选择区域查看详情");
                return;
            }

            try
            {
                using (AppTelemetry.Measure("area.load_details"))
                {
                    Area area = areaService.GetAreaById(areaId);

                    if (area != null)
                    {
                        lblId.Text = area.AreaId;
                        lblName.Text = area.AreaName;

                        // 获取父区域名称 (需要额外调用或优化查询)
                        string parentName = "(无)";
                        if (!string.IsNullOrEmpty(area.ParentAreaId))
                        {
                            Area parentArea = areaService.GetAreaById(area.ParentAreaId);
                            if (parentArea != null)
                            {
                                parentName = parentArea.AreaName;
                            }
                            else
                            {
                                parentName = $"(ID: {area.ParentAreaId} 未找到)";
                            }
                        }

                        lblParent.Text = parentName;
                        lblPostal.Text = area.PostalCode ?? "";
                        lblRemark.Text = area.Remark ?? "";
                    }
                    else
                    {
                        AppLog.Warn($"无法加载所选区域详情: areaId={areaId}");
                        UpdateStatus("无法加载所选区域详情");

                        // 清空显示
                        lblId.Text = "";
                        lblName.Text = "";
                        lblParent.Text = "";
                        lblPostal.Text = "";
                        lblRemark.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, $"加载区域详情失败: areaId={areaId}");
                UpdateStatus("加载区域详情失败");
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // 权限检查 (可选，如果顶级菜单已控制，但加上更保险)
            if (!permissionChecker.HasPermission(currentUser.Id, "area", "add") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有添加区域的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 打开编辑窗体进行添加 (不传递 ID)
                using (FrmAreaEdit addForm = new FrmAreaEdit())
                {
                    DialogResult result = addForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        LoadData(); // 刷新数据
                        // 可选：定位到新添加的节点
                    }
                }
            }
             catch (Exception ex)
            {
                 MessageBox.Show("打开添加窗口时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
            {
                MessageBox.Show("请先在左侧树中选择要编辑的区域。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedAreaId = treeView.SelectedNode.Tag.ToString();

            // 权限检查
            if (!permissionChecker.HasPermission(currentUser.Id, "area", "edit") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有编辑该区域的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                 // 打开编辑窗体进行编辑
                using (FrmAreaEdit editForm = new FrmAreaEdit(selectedAreaId))
                {
                    DialogResult result = editForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        LoadData(); // 刷新数据
                        // 可选：保持编辑前的节点选中状态
                        FindAndSelectNode(selectedAreaId); // 尝试重新选中节点
                    }
                }
            }
             catch (Exception ex)
            {
                 MessageBox.Show("打开编辑窗口时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
             if (treeView.SelectedNode == null || treeView.SelectedNode.Tag == null)
            {
                MessageBox.Show("请先在左侧树中选择要删除的区域。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedAreaId = treeView.SelectedNode.Tag.ToString();
            string selectedAreaName = treeView.SelectedNode.Text;

             // 权限检查
            if (!permissionChecker.HasPermission(currentUser.Id, "area", "delete") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有删除该区域的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

             if (MessageBox.Show($"确定要删除选定的区域 [{selectedAreaName}] ({selectedAreaId}) 吗？\n注意：如果该区域下有子区域，将无法删除。", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bool success = areaService.DeleteArea(selectedAreaId, currentUser); // 传递 currentUser 用于可能的日志记录

                    if (success)
                    {
                        MessageBox.Show("区域删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // 刷新数据
                    }
                    // 如果 BLL/DAL 抛出异常 (如子区域存在或外键约束)，会被下面的 catch 捕获
                }
                catch (InvalidOperationException bizEx) // 捕获业务逻辑异常 (如子区域存在)
                {
                    MessageBox.Show(bizEx.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                     MessageBox.Show("删除区域时发生错误: " + ex.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 在 TreeView 中查找并选中指定 AreaId 的节点
        /// </summary>
        private void FindAndSelectNode(string areaId)
        {
            TreeNode nodeToSelect = FindNodeById(treeView.Nodes, areaId);
            if (nodeToSelect != null)
            {
                treeView.SelectedNode = nodeToSelect;
                nodeToSelect.EnsureVisible(); // 确保节点可见
            }
        }

        /// <summary>
        /// 递归查找具有指定 Tag (AreaId) 的 TreeNode
        /// </summary>
        private TreeNode FindNodeById(TreeNodeCollection nodes, string id)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag != null && node.Tag.ToString() == id)
                {
                    return node;
                }
                TreeNode foundNode = FindNodeById(node.Nodes, id);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null; // 未找到
        }
    }
}
