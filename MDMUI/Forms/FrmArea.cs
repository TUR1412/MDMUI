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

        public FrmArea(User user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.permissionChecker = new PermissionChecker();
            this.areaService = new AreaService(); // 初始化 AreaService
        }

        private void FrmArea_Load(object sender, EventArgs e)
        {
            try
            {
                // Update lblInfo text if needed, assuming lblInfo is created in Designer
                if (this.lblInfo != null) // Check if lblInfo exists (created by Designer)
                {
                   this.lblInfo.Text = $"当前用户: {currentUser.Username}, 工厂ID: {currentUser.FactoryId}";
                }

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
                // 不再需要 EnsureAreaTableExists()
                treeView.Nodes.Clear();
                // 从 AreaService 获取数据
                List<Area> areas = areaService.GetAllAreas();

                // 使用 Area 对象构建树
                BuildAreaTree(areas, null, null);
                treeView.ExpandAll();

                // 清空详细信息或加载根节点（如果需要）
                LoadAreaDetails(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载区域数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
            else
            {
                LoadAreaDetails(null); // 清空详细信息
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
                return;
            }

            try
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
                         } else {
                             parentName = $"(ID: {area.ParentAreaId} 未找到)";
                         }
                    }
                    lblParent.Text = parentName;
                    lblPostal.Text = area.PostalCode ?? "";
                    lblRemark.Text = area.Remark ?? "";
                }
                else
                {
                     MessageBox.Show("无法加载所选区域的详细信息。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     // 清空显示
                    lblId.Text = "";
                    lblName.Text = "";
                    lblParent.Text = "";
                    lblPostal.Text = "";
                    lblRemark.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载区域详情失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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