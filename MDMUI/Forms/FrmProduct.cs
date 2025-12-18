using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.DAL;
using MDMUI.Model;
using MDMUI.Utility;
using System.Data.SqlClient;

namespace MDMUI
{
    public partial class FrmProduct : Form
    {
        private User CurrentUser;
        private DataTable productTable;
        
        // 构造函数
        public FrmProduct(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            
            // 绑定事件
            this.Load += FrmProduct_Load;
        }
        
        // 窗体加载事件
        private void FrmProduct_Load(object sender, EventArgs e)
        {
            InitUI();
            LoadProductData();
        }
        
        // 初始化界面
        private void InitUI()
        {
            this.Text = "产品信息管理";
            this.BackColor = Color.White;
            this.ClientSize = new Size(1000, 600);
            
            // 移除控件动态创建调用
            // CreateToolBar();
            // CreateSearchPanel();
            // CreateStatusStrip(); 
        }
        
        // 创建工具栏 (方法移除)
        /*
        private void CreateToolBar()
        {
           // ... (removed code) ...
        }
        */
        
        // 创建搜索面板 (方法移除)
        /*
        private void CreateSearchPanel()
        {
            // ... (removed code) ...
        }
        */

        // 创建状态栏 (方法移除)
        /*
        private void CreateStatusStrip()
        {
           // ... (removed code) ...
        }
        */
        
        // 加载产品数据
        private void LoadProductData()
        {
            try
            {
                // 使用ProductBLL从数据库获取真实产品数据
                ProductBLL productBLL = new ProductBLL();
                productTable = productBLL.GetAllProducts();
                Console.WriteLine($"成功获取产品数据，共 {productTable.Rows.Count} 条记录");
                
                // 绑定数据
                if (this.dgvProduct != null)
                {
                    this.dgvProduct.DataSource = productTable;
                    
                    // 设置列标题和宽度
                    if (this.dgvProduct.Columns.Contains("ProductId"))
                    {
                        this.dgvProduct.Columns["ProductId"].HeaderText = "产品编码";
                        this.dgvProduct.Columns["ProductId"].Width = 120;
                    }
                    
                    if (this.dgvProduct.Columns.Contains("ProductName"))
                    {
                        this.dgvProduct.Columns["ProductName"].HeaderText = "产品名称";
                        this.dgvProduct.Columns["ProductName"].Width = 160;
                    }
                    
                    if (this.dgvProduct.Columns.Contains("CategoryName"))
                    {
                        this.dgvProduct.Columns["CategoryName"].HeaderText = "类别";
                        this.dgvProduct.Columns["CategoryName"].Width = 90;
                    }
                    
                    if (this.dgvProduct.Columns.Contains("Specification"))
                    {
                        this.dgvProduct.Columns["Specification"].HeaderText = "规格";
                        this.dgvProduct.Columns["Specification"].Width = 120;
                    }
                    
                    if (this.dgvProduct.Columns.Contains("Unit"))
                    {
                        this.dgvProduct.Columns["Unit"].HeaderText = "单位";
                        this.dgvProduct.Columns["Unit"].Width = 80;
                    }
                    
                    if (this.dgvProduct.Columns.Contains("Price"))
                    {
                        this.dgvProduct.Columns["Price"].HeaderText = "价格";
                        this.dgvProduct.Columns["Price"].Width = 80;
                        this.dgvProduct.Columns["Price"].DefaultCellStyle.Format = "N2";
                    }
                    
                    if (this.dgvProduct.Columns.Contains("Cost"))
                    {
                        this.dgvProduct.Columns["Cost"].HeaderText = "成本";
                        this.dgvProduct.Columns["Cost"].Width = 80;
                        this.dgvProduct.Columns["Cost"].DefaultCellStyle.Format = "N2";
                        this.dgvProduct.Columns["Cost"].Visible = false; // 隐藏成本列，通常仅管理员可见
                    }
                    
                    if (this.dgvProduct.Columns.Contains("Status"))
                    {
                        this.dgvProduct.Columns["Status"].HeaderText = "状态";
                        this.dgvProduct.Columns["Status"].Width = 100;
                    }
                    
                    if (this.dgvProduct.Columns.Contains("Description"))
                    {
                        this.dgvProduct.Columns["Description"].HeaderText = "描述";
                        this.dgvProduct.Columns["Description"].Width = 220;
                    }
                    
                    if (this.dgvProduct.Columns.Contains("CreateTime"))
                    {
                        this.dgvProduct.Columns["CreateTime"].HeaderText = "创建时间";
                        this.dgvProduct.Columns["CreateTime"].Width = 150;
                        this.dgvProduct.Columns["CreateTime"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm";
                    }
                    
                    // 隐藏CategoryId列
                    if (this.dgvProduct.Columns.Contains("CategoryId"))
                    {
                        this.dgvProduct.Columns["CategoryId"].Visible = false;
                    }

                    // 在绑定数据后添加以下代码
                    this.dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    this.dgvProduct.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    this.dgvProduct.AllowUserToResizeColumns = true; // 允许用户调整列宽

                    // 确保状态列显示完整
                    if (this.dgvProduct.Columns.Contains("Status")) 
                    {
                        this.dgvProduct.Columns["Status"].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    }
                }
                
                // 加载产品类别数据到下拉框
                LoadCategoryComboBox();
                
                // 更新状态栏
                UpdateStatus($"共加载 {productTable.Rows.Count} 条产品记录");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载产品数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.LogHelper.Log($"加载产品数据失败: {ex.Message}");
            }
        }
        
        // 加载产品类别数据到下拉框
        private void LoadCategoryComboBox()
        {
            try
            {
                if (this.cmbCategory != null)
                {
                    this.cmbCategory.Items.Clear();
                    this.cmbCategory.Items.Add("");  // 空选项
                    
                    // 从数据库获取产品类别
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string sql = "SELECT DISTINCT CategoryName FROM ProductCategory ORDER BY CategoryName";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string categoryName = reader["CategoryName"].ToString();
                                    if (!string.IsNullOrEmpty(categoryName))
                                    {
                                        this.cmbCategory.Items.Add(categoryName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载产品类别数据失败: {ex.Message}");
            }
        }
        
        // 更新状态栏信息
        private void UpdateStatus(string message)
        {
            if (this.statusStrip1 != null) // 直接访问成员变量
            {
                if (this.lblStatus != null) // 直接访问成员变量
                {
                    this.lblStatus.Text = message;
                }
                
                if (this.lblTime != null) // 直接访问成员变量
                {
                    this.lblTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                }

                // 更新用户信息 (如果需要动态更新)
                if (this.lblUser != null) 
                {
                     this.lblUser.Text = $"当前用户: {CurrentUser?.Username ?? "未登录"}";
                }
            }
        }
        
        #region 事件处理
        
        // 新增按钮点击
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("新增产品功能将在后续实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // 编辑按钮点击
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvProduct != null && this.dgvProduct.SelectedRows.Count > 0)
            {
                string productId = this.dgvProduct.SelectedRows[0].Cells["ProductId"].Value.ToString();
                string productName = this.dgvProduct.SelectedRows[0].Cells["ProductName"].Value.ToString();
                MessageBox.Show($"编辑产品: {productName}，ID: {productId}\n此功能将在后续实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请先选择要编辑的产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        // 删除按钮点击
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvProduct != null && this.dgvProduct.SelectedRows.Count > 0)
            {
                string productId = this.dgvProduct.SelectedRows[0].Cells["ProductId"].Value.ToString();
                string productName = this.dgvProduct.SelectedRows[0].Cells["ProductName"].Value.ToString();

                bool isAdmin = CurrentUser != null && CurrentUser.RoleName == "超级管理员";
                if (!isAdmin)
                {
                    if (CurrentUser == null)
                    {
                        MessageBox.Show("未登录用户无权删除产品。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    PermissionChecker permissionChecker = new PermissionChecker();
                    if (!permissionChecker.HasPermission(CurrentUser.Id, "product", "delete"))
                    {
                        MessageBox.Show("您没有删除产品的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                if (MessageBox.Show($"确定要删除产品: {productName}吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        ProductBLL productBLL = new ProductBLL();
                        bool success = productBLL.DeleteProduct(productId);
                        if (success)
                        {
                            UpdateStatus($"已删除产品：{productName} ({productId})");
                        }
                        else
                        {
                            MessageBox.Show("删除失败：未找到该产品记录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        LoadProductData(); // 重新加载数据
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除产品失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的产品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        // 刷新按钮点击
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadProductData();
        }
        
        // 导出按钮点击
        private void BtnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导出功能将在后续实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // 搜索按钮点击
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取搜索条件
                string productName = this.txtProductName?.Text.Trim() ?? ""; // 直接访问
                string productCode = this.txtProductCode?.Text.Trim() ?? ""; // 直接访问
                string categoryName = this.cmbCategory?.SelectedItem?.ToString() ?? ""; // 直接访问

                // 从数据库中搜索产品
                ProductBLL productBLL = new ProductBLL();
                string categoryId = null;
                
                // 如果选择了类别，获取类别ID
                if (!string.IsNullOrEmpty(categoryName))
                {
                    string connectionString = DbConnectionHelper.GetConnectionString();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = "SELECT CategoryId FROM ProductCategory WHERE CategoryName = @CategoryName";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                            object result = cmd.ExecuteScalar();
                            categoryId = result == DBNull.Value ? null : result?.ToString();
                        }
                    }
                }

                // 执行搜索
                productTable = productBLL.SearchProducts(
                    !string.IsNullOrEmpty(productName) ? productName : null,
                    !string.IsNullOrEmpty(productCode) ? productCode : null,
                    categoryId
                );
                
                // 更新数据绑定
                this.dgvProduct.DataSource = productTable;
                
                UpdateStatus($"共找到 {productTable.Rows.Count} 条产品记录");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搜索失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"搜索失败: {ex.Message}");
            }
        }
        
        // 重置按钮点击
        private void BtnReset_Click(object sender, EventArgs e)
        {
            // 清空搜索条件
            if (this.txtProductName != null) // 直接访问
                this.txtProductName.Text = "";
                
            if (this.txtProductCode != null) // 直接访问
                this.txtProductCode.Text = "";
                
            if (this.cmbCategory != null) // 直接访问
                this.cmbCategory.SelectedIndex = -1;
            
            // 重新加载数据
            LoadProductData();
        }
        
        // 数据网格双击事件
        private void DgvProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string productId = this.dgvProduct.Rows[e.RowIndex].Cells["ProductId"].Value.ToString();
                string productName = this.dgvProduct.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                MessageBox.Show($"查看产品详情: {productName}, ID: {productId}\n此功能将在后续实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        #endregion
    }
} 
