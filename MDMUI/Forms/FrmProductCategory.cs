using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI
{
    public partial class FrmProductCategory : Form
    {
        private User CurrentUser;
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();
        private PermissionChecker permissionChecker;

        public FrmProductCategory(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            permissionChecker = new PermissionChecker();
        }

        private void FrmProductCategory_Load(object sender, EventArgs e)
        {
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

            if (this.toolStripLeft != null)
            {
                var btnAdd = this.btnAddCategory;
                var btnEdit = this.btnEditCategory;
                var btnDelete = this.btnDeleteCategory;

                if (btnAdd != null) btnAdd.Visible = isAdmin || permissionChecker.HasPermission(CurrentUser.Id, "product_category", "add");
                if (btnEdit != null) btnEdit.Visible = isAdmin || permissionChecker.HasPermission(CurrentUser.Id, "product_category", "edit");
                if (btnDelete != null) btnDelete.Visible = isAdmin || permissionChecker.HasPermission(CurrentUser.Id, "product_category", "delete");
            }
        }

        private void LoadCategoryData()
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品类别数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("加载失败。");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载子类别数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("子类别加载失败。");
                this.dgvSubCategory.DataSource = null;
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
