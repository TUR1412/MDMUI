using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI
{
    public partial class FrmProductCategory : Form
    {
        private User CurrentUser;
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();
        private PermissionChecker permissionChecker;

        // Modern UI (Atomic Design) - runtime enhancement (OCP: do not rewrite Designer)
        private bool modernLayoutInitialized;
        private CardPanel modernHeaderCard;
        private Label treeEmptyStateLabel;
        private Label gridEmptyStateLabel;
        private AppButton modernAddButton;
        private AppButton modernEditButton;
        private AppButton modernDeleteButton;
        private AppButton modernRefreshButton;

        public FrmProductCategory(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            permissionChecker = new PermissionChecker();
        }

        private void FrmProductCategory_Load(object sender, EventArgs e)        
        {
            InitializeModernLayout();
            this.dgvSubCategory.AutoGenerateColumns = true;
            LoadCategoryData();
            ApplyPermissions();
        }

        private void FrmProductCategory_Shown(object sender, EventArgs e)
        {
            if (this.splitContainerMain != null)
            {
                if (this.splitContainerMain.Width >= this.splitContainerMain.Panel1MinSize + this.splitContainerMain.Panel2MinSize)
                {
                    int defaultDistance = this.splitContainerMain.Width / 3;
                    int validDistance = Math.Max(this.splitContainerMain.Panel1MinSize, Math.Min(defaultDistance, this.splitContainerMain.Width - this.splitContainerMain.Panel2MinSize));
                    
                    try 
                    {
                         this.splitContainerMain.SplitterDistance = validDistance;
                         this.splitContainerMain.IsSplitterFixed = false;
                    } 
                    catch (Exception ex)
                    {
                         Console.WriteLine($"无法设置分隔条位置: {ex.Message}");
                         if (this.splitContainerMain.Width >= 40)
                         {
                             try { 
                                 this.splitContainerMain.SplitterDistance = Math.Max(20, Math.Min(150, this.splitContainerMain.Width - 20));
                                 this.splitContainerMain.IsSplitterFixed = false;
                             } catch { /* 忽略后备错误 */ }
                         }
                    }
                }
            }
        }

        private void ApplyPermissions()
        {
            if (CurrentUser == null) return;
            bool isAdmin = CurrentUser.RoleName == "超级管理员";

            bool canAdd = isAdmin || permissionChecker.HasPermission(CurrentUser.Id, "product_category", "add");
            bool canEdit = isAdmin || permissionChecker.HasPermission(CurrentUser.Id, "product_category", "edit");
            bool canDelete = isAdmin || permissionChecker.HasPermission(CurrentUser.Id, "product_category", "delete");

            if (btnAddCategory != null) btnAddCategory.Visible = canAdd;
            if (btnEditCategory != null) btnEditCategory.Visible = canEdit;
            if (btnDeleteCategory != null) btnDeleteCategory.Visible = canDelete;

            if (modernAddButton != null) modernAddButton.Visible = canAdd;
            if (modernEditButton != null) modernEditButton.Visible = canEdit;
            if (modernDeleteButton != null) modernDeleteButton.Visible = canDelete;
        }

        private void InitializeModernLayout()
        {
            if (modernLayoutInitialized) return;
            modernLayoutInitialized = true;

            try
            {
                using (AppTelemetry.Measure("FrmProductCategory.ModernLayout"))
                {
                    EnsureModernHeader();
                    EnsureEmptyStates();
                    EnsureStatusStrip();

                    try { GridStyler.Apply(dgvSubCategory); } catch { }
                    TryEnableGridDoubleBuffering(dgvSubCategory);

                    try { ThemeManager.ApplyTo(this); } catch { }
                    try { ModernTheme.EnableMicroInteractions(this); } catch { }

                    TryStyleTreeView(tvCategory);
                    UpdateEmptyStates();
                }
            }
            catch (Exception ex)
            {
                try { AppLog.Error(ex, "初始化产品类别管理现代化布局失败"); } catch { }
            }
        }

        private void EnsureModernHeader()
        {
            if (modernHeaderCard != null) return;

            ToolStrip oldToolStrip = toolStripLeft;
            ToolStripButton oldAdd = btnAddCategory;
            ToolStripButton oldEdit = btnEditCategory;
            ToolStripButton oldDelete = btnDeleteCategory;
            ToolStripButton oldRefresh = btnRefreshTree;

            CardPanel header = new CardPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(12),
                Height = 72
            };

            ActionToolbar toolbar = new ActionToolbar { Dock = DockStyle.Fill };

            Label title = new Label
            {
                AutoSize = true,
                Text = string.IsNullOrWhiteSpace(Text) ? "产品类别管理" : Text,
                ForeColor = ThemeManager.Palette.TextPrimary,
                Margin = new Padding(0, 10, 12, 0)
            };
            try { title.Font = ThemeManager.CreateTitleFont(11f); } catch { }
            toolbar.LeftPanel.Controls.Add(title);

            modernAddButton = CreateAppButton(oldAdd?.Text ?? "添加类别", AppButtonVariant.Primary, UiSafe.Wrap("product_category.add", BtnAddCategory_Click), name: "btnAddCategory");
            modernEditButton = CreateAppButton(oldEdit?.Text ?? "编辑类别", AppButtonVariant.Secondary, UiSafe.Wrap("product_category.edit", BtnEditCategory_Click), name: "btnEditCategory");
            modernDeleteButton = CreateAppButton(oldDelete?.Text ?? "删除类别", AppButtonVariant.Danger, UiSafe.Wrap("product_category.delete", BtnDeleteCategory_Click), name: "btnDeleteCategory");
            modernRefreshButton = CreateAppButton(oldRefresh?.Text ?? "刷新", AppButtonVariant.Secondary, UiSafe.Wrap("product_category.refresh", BtnRefreshTree_Click), name: "btnRefreshTree");

            CopyToolStripState(oldAdd, modernAddButton);
            CopyToolStripState(oldEdit, modernEditButton);
            CopyToolStripState(oldDelete, modernDeleteButton);
            CopyToolStripState(oldRefresh, modernRefreshButton);

            toolbar.RightPanel.Controls.Add(modernAddButton);
            toolbar.RightPanel.Controls.Add(modernEditButton);
            toolbar.RightPanel.Controls.Add(modernDeleteButton);
            toolbar.RightPanel.Controls.Add(modernRefreshButton);

            header.Controls.Add(toolbar);

            Controls.Add(header);
            header.BringToFront();
            modernHeaderCard = header;

            // 旧标题栏/工具栏移除并释放（开闭原则：运行时替换，不动 Designer）
            try
            {
                splitContainerMain?.Panel1?.Controls.Remove(panelLeftTitle);
                splitContainerMain?.Panel1?.Controls.Remove(oldToolStrip);
                splitContainerMain?.Panel2?.Controls.Remove(panelRightTitle);
            }
            catch
            {
                // ignore
            }

            try { panelLeftTitle?.Dispose(); } catch { }
            try { panelRightTitle?.Dispose(); } catch { }
            try { oldToolStrip?.Dispose(); } catch { }

            panelLeftTitle = null;
            lblLeftTitle = null;
            panelRightTitle = null;
            lblRightTitle = null;
            toolStripLeft = null;

            btnAddCategory = null;
            btnEditCategory = null;
            btnDeleteCategory = null;
            btnRefreshTree = null;
        }

        private void EnsureEmptyStates()
        {
            if (treeEmptyStateLabel == null && splitContainerMain?.Panel1 != null)
            {
                treeEmptyStateLabel = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = ThemeManager.Palette.TextSecondary,
                    BackColor = Color.Transparent,
                    Visible = false
                };
                try { treeEmptyStateLabel.Font = ThemeManager.CreateBodyFont(9f); } catch { }

                splitContainerMain.Panel1.Controls.Add(treeEmptyStateLabel);
                try { treeEmptyStateLabel.BringToFront(); } catch { }
            }

            if (gridEmptyStateLabel == null && splitContainerMain?.Panel2 != null)
            {
                gridEmptyStateLabel = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = ThemeManager.Palette.TextSecondary,
                    BackColor = Color.Transparent,
                    Visible = false
                };
                try { gridEmptyStateLabel.Font = ThemeManager.CreateBodyFont(9f); } catch { }

                splitContainerMain.Panel2.Controls.Add(gridEmptyStateLabel);
                try { gridEmptyStateLabel.BringToFront(); } catch { }
            }
        }

        private void EnsureStatusStrip()
        {
            if (statusStripBottom == null) return;
            try { statusStripBottom.SizingGrip = false; } catch { }
        }

        private static void TryEnableGridDoubleBuffering(DataGridView grid)
        {
            if (grid == null) return;

            try
            {
                typeof(DataGridView).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(grid, true, null);
            }
            catch
            {
                // ignore
            }
        }

        private static void TryStyleTreeView(TreeView tree)
        {
            if (tree == null) return;

            try { tree.BorderStyle = BorderStyle.None; } catch { }
            try { tree.BackColor = ThemeManager.Palette.Surface; } catch { }
            try { tree.ForeColor = ThemeManager.Palette.TextPrimary; } catch { }
        }

        private void UpdateEmptyStates(int? gridCountOverride = null)
        {
            int treeCount = 0;
            try { treeCount = tvCategory?.GetNodeCount(includeSubTrees: true) ?? 0; } catch { }

            bool treeEmpty = treeCount <= 0;
            if (treeEmptyStateLabel != null)
            {
                treeEmptyStateLabel.Text = "暂无产品类别";
                treeEmptyStateLabel.Visible = treeEmpty;
                if (treeEmpty) { try { treeEmptyStateLabel.BringToFront(); } catch { } }
            }

            int gridCount = 0;
            if (gridCountOverride.HasValue)
            {
                gridCount = gridCountOverride.Value;
            }
            else
            {
                try
                {
                    gridCount = dgvSubCategory?.Rows?.Count ?? 0;
                    if (dgvSubCategory != null && dgvSubCategory.AllowUserToAddRows) gridCount = Math.Max(0, gridCount - 1);
                }
                catch
                {
                    gridCount = 0;
                }
            }

            bool gridEmpty = gridCount <= 0;
            if (gridEmptyStateLabel != null)
            {
                gridEmptyStateLabel.Text = treeEmpty ? "请先添加产品类别" : "暂无子类别";
                gridEmptyStateLabel.Visible = gridEmpty;
                if (gridEmpty) { try { gridEmptyStateLabel.BringToFront(); } catch { } }
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

        private static void CopyToolStripState(ToolStripItem source, Control target)
        {
            if (source == null || target == null) return;
            try { target.Visible = source.Visible; } catch { }
            try { target.Enabled = source.Enabled; } catch { }
        }

        private void LoadCategoryData()
        {
            try
            {
                using (AppTelemetry.Measure("product_category.load_tree"))
                {
                    UpdateStatus("正在加载产品类别...");
                    this.tvCategory.Nodes.Clear();
                    List<ProductCategory> categories = GetAllCategories();
                    var categoryMap = categories.ToDictionary(c => c.CategoryId);

                    BuildCategoryTree(categories, null, null, categoryMap);

                    this.tvCategory.ExpandAll();
                    if (this.tvCategory.SelectedNode == null)
                    {
                        LoadSubCategoriesGridView(null);
                    }
                    UpdateStatus("产品类别加载完成。");
                    UpdateEmptyStates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品类别数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { AppLog.Error(ex, "加载产品类别数据失败"); } catch { }
                UpdateStatus("加载失败。");
                UpdateEmptyStates(0);
            }
        }

        private List<ProductCategory> GetAllCategories()
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT CategoryId, CategoryName, ParentCategoryId, Description, CreateTime FROM ProductCategory ORDER BY CategoryName";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new ProductCategory
                    {
                        CategoryId = reader["CategoryId"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                        ParentCategoryId = reader["ParentCategoryId"] != DBNull.Value ? reader["ParentCategoryId"].ToString() : null,
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                        CreateTime = Convert.ToDateTime(reader["CreateTime"])
                    });
                }
            }
            return categories;
        }

        private void BuildCategoryTree(List<ProductCategory> allCategories, string parentId, TreeNode parentNode, Dictionary<string, ProductCategory> categoryMap)
        {
            var children = allCategories.Where(c => c.ParentCategoryId == parentId).ToList();
            foreach (var category in children)
            {
                TreeNode node = new TreeNode(category.CategoryName);
                node.Tag = category.CategoryId;

                if (parentNode == null)
                {
                    this.tvCategory.Nodes.Add(node);
                }
                else
                {
                    parentNode.Nodes.Add(node);
                }
                BuildCategoryTree(allCategories, category.CategoryId, node, categoryMap);
            }
        }

        private void LoadSubCategoriesGridView(string parentCategoryId)
        {
            try
            {
                using (AppTelemetry.Measure("product_category.load_subcategories"))
                {
                    List<ProductCategory> subCategories = new List<ProductCategory>();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = "SELECT CategoryId, CategoryName, ParentCategoryId, Description, CreateTime FROM ProductCategory WHERE ParentCategoryId = @ParentId ORDER BY CategoryName";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add("@ParentId", SqlDbType.VarChar, 36).Value = parentCategoryId ?? (object)DBNull.Value;

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string categoryNameFromReader = reader["CategoryName"].ToString();
                            string categoryIdFromReader = reader["CategoryId"].ToString();

                            subCategories.Add(new ProductCategory
                            {
                                CategoryId = categoryIdFromReader,
                                CategoryName = categoryNameFromReader,
                                ParentCategoryId = reader["ParentCategoryId"] != DBNull.Value ? reader["ParentCategoryId"].ToString() : null,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                CreateTime = Convert.ToDateTime(reader["CreateTime"])
                            });
                        }
                    }
                    this.dgvSubCategory.DataSource = subCategories;

                    UpdateStatus($"显示 {(parentCategoryId == null ? "顶级" : $"类别 {parentCategoryId} 的")} 子类别 ({subCategories.Count} 项)");
                    UpdateEmptyStates(subCategories.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载子类别数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { AppLog.Error(ex, $"加载子类别失败: parent={parentCategoryId ?? "(null)"}"); } catch { }
                UpdateStatus("子类别加载失败。");
                this.dgvSubCategory.DataSource = null;
                UpdateEmptyStates(0);
            }
        }

        private void dgvSubCategory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.dgvSubCategory.Columns.Count > 0 && e.ListChangedType != ListChangedType.ItemDeleted)
            {
                try
                {
                    if (this.dgvSubCategory.Columns["CategoryId"] != null)
                    {
                        this.dgvSubCategory.Columns["CategoryId"].HeaderText = "类别ID";
                        this.dgvSubCategory.Columns["CategoryId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    if (this.dgvSubCategory.Columns["CategoryName"] != null)
                    {
                        this.dgvSubCategory.Columns["CategoryName"].HeaderText = "子类别名称";
                        this.dgvSubCategory.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    if (this.dgvSubCategory.Columns["ParentCategoryId"] != null)
                    {
                        this.dgvSubCategory.Columns["ParentCategoryId"].HeaderText = "父类别ID";
                        this.dgvSubCategory.Columns["ParentCategoryId"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        this.dgvSubCategory.Columns["ParentCategoryId"].Visible = true;
                    }
                    if (this.dgvSubCategory.Columns["Description"] != null)
                    {
                        this.dgvSubCategory.Columns["Description"].HeaderText = "描述";
                        this.dgvSubCategory.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    if (this.dgvSubCategory.Columns["CreateTime"] != null)
                    {
                        this.dgvSubCategory.Columns["CreateTime"].HeaderText = "创建时间";
                        this.dgvSubCategory.Columns["CreateTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                        this.dgvSubCategory.Columns["CreateTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error configuring dgvSubCategory columns: {ex.Message}");
                }
            }
        }

        private void UpdateStatus(string message)
        {
            if (this.statusLabel != null)
            {
                this.statusLabel.Text = message;
            }
        }

        private void TvCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                string selectedCategoryId = e.Node.Tag.ToString();
                LoadSubCategoriesGridView(selectedCategoryId);
            }
            else
            {
                LoadSubCategoriesGridView(null);
            }
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            string parentId = null;
            string parentName = "无";
            if (this.tvCategory.SelectedNode != null)
            {
                parentId = this.tvCategory.SelectedNode.Tag.ToString();
                parentName = this.tvCategory.SelectedNode.Text;
            }

            using (FrmProductCategoryEdit addForm = new FrmProductCategoryEdit(parentId, parentName))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCategoryData();
                    TreeNode newNode = FindNodeById(this.tvCategory.Nodes, addForm.SavedCategoryId);
                    if (newNode != null)
                    {
                        this.tvCategory.SelectedNode = newNode;
                        newNode.EnsureVisible();
                    }
                }
            }
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            if (this.tvCategory.SelectedNode == null)
            {
                MessageBox.Show("请先选择一个要编辑的产品类别。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string categoryIdToEdit = this.tvCategory.SelectedNode.Tag.ToString();
            string parentId = null;
            string parentName = "无";
            if (this.tvCategory.SelectedNode.Parent != null)
            {
                 parentId = this.tvCategory.SelectedNode.Parent.Tag.ToString();
                 parentName = this.tvCategory.SelectedNode.Parent.Text;
            }

            using (FrmProductCategoryEdit editForm = new FrmProductCategoryEdit(parentId, parentName, categoryIdToEdit))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    TreeNode selectedNodeParent = this.tvCategory.SelectedNode.Parent;
                    string selectedNodeId = this.tvCategory.SelectedNode.Tag.ToString();
                    LoadCategoryData();
                    TreeNode reselectedNode = FindNodeById(this.tvCategory.Nodes, selectedNodeId);
                     if (reselectedNode != null)
                    {
                        this.tvCategory.SelectedNode = reselectedNode;
                        reselectedNode.EnsureVisible();
                    }
                    else if (selectedNodeParent != null) {
                        this.tvCategory.SelectedNode = selectedNodeParent;
                         selectedNodeParent.EnsureVisible();
                    }
                }
            }
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (this.tvCategory.SelectedNode == null)
            {
                MessageBox.Show("请先选择一个要删除的产品类别。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string categoryIdToDelete = this.tvCategory.SelectedNode.Tag.ToString();
            string categoryNameToDelete = this.tvCategory.SelectedNode.Text;
            TreeNode parentNode = this.tvCategory.SelectedNode.Parent;

            bool hasSubcategories = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlCheck = "SELECT COUNT(*) FROM ProductCategory WHERE ParentCategoryId = @CategoryId";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                cmdCheck.Parameters.AddWithValue("@CategoryId", categoryIdToDelete);
                int subCategoryCount = (int)cmdCheck.ExecuteScalar();
                hasSubcategories = subCategoryCount > 0;
            }

            bool hasProducts = false;
             using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sqlCheck = "SELECT COUNT(*) FROM Product WHERE CategoryId = @CategoryId";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                cmdCheck.Parameters.AddWithValue("@CategoryId", categoryIdToDelete);
                int productCount = (int)cmdCheck.ExecuteScalar();
                hasProducts = productCount > 0;
            }

            if (hasProducts)
            {
                MessageBox.Show($"类别 '{categoryNameToDelete}' 已关联产品，无法删除。请先将产品移至其他类别或删除产品。", "无法删除", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"确定要删除产品类别 '{categoryNameToDelete}' 吗？此操作不可恢复。", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string sqlDelete = "DELETE FROM ProductCategory WHERE CategoryId = @CategoryId";
                        SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn);
                        cmdDelete.Parameters.AddWithValue("@CategoryId", categoryIdToDelete);
                        int rowsAffected = cmdDelete.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            UpdateStatus($"产品类别 '{categoryNameToDelete}' 删除成功。");
                            LoadCategoryData();
                            if (parentNode != null)
                            {
                                this.tvCategory.SelectedNode = parentNode;
                                parentNode.EnsureVisible();
                            }
                        }
                        else
                        {
                            MessageBox.Show("删除产品类别失败，可能已被其他用户删除。", "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除产品类别时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("删除失败。");
                }
            }
        }

        private void BtnRefreshTree_Click(object sender, EventArgs e)
        {
            LoadCategoryData();
        }

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
            return null;
        }
    }
} 
