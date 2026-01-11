using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.DAL;
using MDMUI.Model;
using MDMUI.Utility;
using System.Data.SqlClient;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;

namespace MDMUI
{
    public partial class FrmProduct : Form
    {
        private User CurrentUser;
        private DataTable productTable;

        // Modern UI (Atomic Design) - runtime enhancement (OCP: do not rewrite Designer)
        private bool modernLayoutInitialized;
        private CardPanel modernHeaderCard;
        private Panel gridHostPanel;
        private Label emptyStateLabel;
        
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
            InitializeModernLayout();
            LoadProductData();
        }

        private void InitializeModernLayout()
        {
            if (modernLayoutInitialized) return;
            modernLayoutInitialized = true;

            try
            {
                using (AppTelemetry.Measure("FrmProduct.ModernLayout"))
                {
                    EnsureModernHeader();
                    EnsureGridHostAndEmptyState();
                    EnsureStatusStrip();

                    try { GridStyler.Apply(dgvProduct); } catch { }
                    TryEnableGridDoubleBuffering(dgvProduct);

                    // UiThemingBootstrapper 只对窗体执行一次；这里确保运行时新
                    // 增控件也能吃到统一风格
                    try { ThemeManager.ApplyTo(this); } catch { }
                    try { ModernTheme.EnableMicroInteractions(this); } catch { }

                    UpdateProductListIndicators(productTable?.Rows?.Count ?? 0);
                }
            }
            catch (Exception ex)
            {
                try { AppLog.Error(ex, "初始化产品管理现代化布局失败"); } catch { }
            }
        }

        private void EnsureModernHeader()
        {
            if (modernHeaderCard != null) return;

            ToolStrip oldToolStrip = toolStrip1;
            Panel oldSearchPanel = searchPanel;

            ToolStripButton oldAdd = btnAdd;
            ToolStripButton oldEdit = btnEdit;
            ToolStripButton oldDelete = btnDelete;
            ToolStripButton oldRefresh = btnRefresh;
            ToolStripButton oldExport = btnExport;

            Button oldSearch = btnSearch;
            Button oldReset = btnReset;

            // 先把需要保留的输入控件迁移出来，避免旧 Panel Dispose 时连带释放
            try
            {
                if (txtProductCode?.Parent != null) txtProductCode.Parent.Controls.Remove(txtProductCode);
                if (txtProductName?.Parent != null) txtProductName.Parent.Controls.Remove(txtProductName);
                if (cmbCategory?.Parent != null) cmbCategory.Parent.Controls.Remove(cmbCategory);
            }
            catch
            {
                // ignore
            }

            CardPanel header = new CardPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(12),
                Height = 140
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
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
            header.Controls.Add(layout);

            ActionToolbar actionsToolbar = new ActionToolbar { Dock = DockStyle.Fill };
            ActionToolbar searchToolbar = new ActionToolbar { Dock = DockStyle.Fill, Height = 54 };

            Label title = new Label
            {
                AutoSize = true,
                Text = string.IsNullOrWhiteSpace(Text) ? "产品信息管理" : Text,
                ForeColor = ThemeManager.Palette.TextPrimary,
                Margin = new Padding(0, 10, 12, 0)
            };
            try { title.Font = ThemeManager.CreateTitleFont(11f); } catch { }
            actionsToolbar.LeftPanel.Controls.Add(title);

            AppButton add = CreateAppButton("新增", AppButtonVariant.Primary, BtnAdd_Click, name: "btnAdd");
            AppButton edit = CreateAppButton("编辑", AppButtonVariant.Secondary, BtnEdit_Click, name: "btnEdit");
            AppButton delete = CreateAppButton("删除", AppButtonVariant.Danger, BtnDelete_Click, name: "btnDelete");
            AppButton refresh = CreateAppButton("刷新", AppButtonVariant.Secondary, BtnRefresh_Click, name: "btnRefresh");
            AppButton export = CreateAppButton("导出", AppButtonVariant.Secondary, BtnExport_Click, name: "btnExport");

            CopyToolStripState(oldAdd, add);
            CopyToolStripState(oldEdit, edit);
            CopyToolStripState(oldDelete, delete);
            CopyToolStripState(oldRefresh, refresh);
            CopyToolStripState(oldExport, export);

            actionsToolbar.RightPanel.Controls.Add(add);
            actionsToolbar.RightPanel.Controls.Add(edit);
            actionsToolbar.RightPanel.Controls.Add(delete);
            actionsToolbar.RightPanel.Controls.Add(refresh);
            actionsToolbar.RightPanel.Controls.Add(export);

            Label codeLabel = CreateFieldLabel("产品编码");
            Label nameLabel = CreateFieldLabel("产品名称");
            Label categoryLabel = CreateFieldLabel("类别");

            searchToolbar.LeftPanel.Controls.Add(codeLabel);
            if (txtProductCode != null)
            {
                txtProductCode.Width = Math.Max(120, txtProductCode.Width);
                txtProductCode.Margin = new Padding(0, 8, 10, 0);
                searchToolbar.LeftPanel.Controls.Add(txtProductCode);
            }

            searchToolbar.LeftPanel.Controls.Add(nameLabel);
            if (txtProductName != null)
            {
                txtProductName.Width = Math.Max(160, txtProductName.Width);
                txtProductName.Margin = new Padding(0, 8, 10, 0);
                searchToolbar.LeftPanel.Controls.Add(txtProductName);
            }

            searchToolbar.LeftPanel.Controls.Add(categoryLabel);
            if (cmbCategory != null)
            {
                cmbCategory.Width = Math.Max(140, cmbCategory.Width);
                cmbCategory.Margin = new Padding(0, 8, 10, 0);
                searchToolbar.LeftPanel.Controls.Add(cmbCategory);
            }

            AppButton reset = CreateAppButton("重置", AppButtonVariant.Secondary, BtnReset_Click, name: "btnReset");
            AppButton search = CreateAppButton("搜索", AppButtonVariant.Primary, BtnSearch_Click, name: "btnSearch");

            CopyButtonState(oldReset, reset);
            CopyButtonState(oldSearch, search);

            btnReset = reset;
            btnSearch = search;

            searchToolbar.RightPanel.Controls.Add(reset);
            searchToolbar.RightPanel.Controls.Add(search);

            try { AcceptButton = search; } catch { }

            layout.Controls.Add(actionsToolbar, 0, 0);
            layout.Controls.Add(searchToolbar, 0, 1);

            Controls.Add(header);
            header.BringToFront();
            modernHeaderCard = header;

            // 旧控件移除并释放（开闭原则：运行时替换，不动 Designer）
            try { if (oldToolStrip != null) Controls.Remove(oldToolStrip); } catch { }
            try { if (oldSearchPanel != null) Controls.Remove(oldSearchPanel); } catch { }

            try { oldToolStrip?.Dispose(); } catch { }
            try { oldSearchPanel?.Dispose(); } catch { }

            toolStrip1 = null;
            searchPanel = null;

            // 避免后续误用已释放按钮引用（ToolStripButton）
            btnAdd = null;
            btnEdit = null;
            btnDelete = null;
            btnRefresh = null;
            btnExport = null;
        }

        private void EnsureGridHostAndEmptyState()
        {
            if (gridHostPanel != null) return;
            if (dgvProduct == null) return;

            int gridIndex = 0;
            try { gridIndex = Controls.GetChildIndex(dgvProduct); } catch { }

            try { Controls.Remove(dgvProduct); } catch { }

            Panel host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            dgvProduct.Dock = DockStyle.Fill;
            dgvProduct.BorderStyle = BorderStyle.None;
            host.Controls.Add(dgvProduct);

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

            Controls.Add(host);
            try { Controls.SetChildIndex(host, gridIndex); } catch { }

            gridHostPanel = host;
            emptyStateLabel = empty;
        }

        private void EnsureStatusStrip()
        {
            if (statusStrip1 == null) return;
            try { statusStrip1.SizingGrip = false; } catch { }
        }

        private static Label CreateFieldLabel(string text)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                ForeColor = ThemeManager.Palette.TextSecondary,
                Margin = new Padding(0, 10, 8, 0)
            };
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

        private static void CopyToolStripState(ToolStripItem source, Control target)
        {
            if (source == null || target == null) return;

            try { target.Visible = source.Visible; } catch { }
            try { target.Enabled = source.Enabled; } catch { }
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

        private void UpdateProductListIndicators(int count)
        {
            bool hasFilter =
                !string.IsNullOrWhiteSpace(txtProductCode?.Text) ||
                !string.IsNullOrWhiteSpace(txtProductName?.Text) ||
                !string.IsNullOrWhiteSpace(cmbCategory?.Text);

            if (emptyStateLabel != null)
            {
                if (count <= 0)
                {
                    emptyStateLabel.Text = hasFilter ? "未找到匹配的产品" : "暂无产品数据";
                    emptyStateLabel.Visible = true;
                    try { emptyStateLabel.BringToFront(); } catch { }
                }
                else
                {
                    emptyStateLabel.Visible = false;
                }
            }

            if (count <= 0)
            {
                UpdateStatus(hasFilter ? "未找到匹配的产品" : "暂无产品数据");
            }
            else
            {
                UpdateStatus($"共 {count} 条产品记录");
            }
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
                using (AppTelemetry.Measure("product.load_list"))
                {
                    // 使用 ProductBLL 从数据库获取真实产品数据
                ProductBLL productBLL = new ProductBLL();
                productTable = productBLL.GetAllProducts();
                AppLog.Info($"成功获取产品数据，共 {productTable.Rows.Count} 条记录");
                
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
                
                // 更新状态栏 / 空态
                UpdateProductListIndicators(productTable?.Rows?.Count ?? 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载产品数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppLog.Error(ex, "加载产品数据失败");
                UpdateProductListIndicators(0);
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
                AppLog.Error(ex, "加载产品类别数据失败");
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
            try
            {
                if (productTable == null)
                {
                    MessageBox.Show("当前没有可导出的产品数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "导出产品数据";
                    dialog.Filter = "CSV 文件 (*.csv)|*.csv";
                    dialog.FileName = $"products_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    WriteDataTableToCsv(productTable, dialog.FileName);
                    UpdateStatus($"已导出产品数据：{dialog.FileName}");
                    MessageBox.Show("导出成功。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void WriteDataTableToCsv(DataTable table, string filePath)
        {
            if (table == null) { throw new ArgumentNullException(nameof(table)); }
            if (string.IsNullOrWhiteSpace(filePath)) { throw new ArgumentException("导出路径不能为空", nameof(filePath)); }

            // UTF-8 BOM：提升在 Excel 中直接打开的兼容性
            using (StreamWriter writer = new StreamWriter(filePath, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true)))
            {
                // Header
                string header = string.Join(",", table.Columns.Cast<DataColumn>().Select(c => EscapeCsv(c.ColumnName)));
                writer.WriteLine(header);

                foreach (DataRow row in table.Rows)
                {
                    string line = string.Join(",", table.Columns.Cast<DataColumn>().Select(c => EscapeCsv(row[c])));
                    writer.WriteLine(line);
                }
            }
        }

        private static string EscapeCsv(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            string text = value.ToString();
            bool mustQuote = text.Contains(",") || text.Contains("\"") || text.Contains("\r") || text.Contains("\n");
            if (!mustQuote)
            {
                return text;
            }

            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
        
        // 搜索按钮点击
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (AppTelemetry.Measure("product.search"))
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

                    UpdateProductListIndicators(productTable?.Rows?.Count ?? 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搜索失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppLog.Error(ex, "搜索失败");
                UpdateProductListIndicators(0);
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
