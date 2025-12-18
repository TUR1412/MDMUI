using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI
{
    /// <summary>
    /// 产品编辑窗体
    /// </summary>
    public partial class FrmProductEdit : Form
    {
        private bool isNew = true;
        private string productId = string.Empty;
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        // UI Controls
        private Label lblProductId;
        private TextBox txtProductId;
        private Label lblProductName;
        private TextBox txtProductName;
        private Label lblCategory;
        private ComboBox cboCategory;
        private Label lblSpecification;
        private TextBox txtSpecification;
        private Label lblUnit;
        private TextBox txtUnit;
        private Label lblPrice;
        private NumericUpDown nudPrice;
        private Label lblCost;
        private NumericUpDown nudCost;
        private Label lblStatus;
        private ComboBox cboStatus;
        private Label lblDescription;
        private TextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;

        public FrmProductEdit()
        {
            isNew = true;
            InitializeComponent();
            this.Text = "添加产品";
            this.Load += FrmProductEdit_Load;
        }

        public FrmProductEdit(string productId)
        {
            isNew = false;
            this.productId = productId;
            InitializeComponent();
            this.Text = "编辑产品";
            this.Load += FrmProductEdit_Load;
        }

        private void FrmProductEdit_Load(object sender, EventArgs e)
        {
            SetupControls();
            try
            {
                LoadCategories();
                LoadStatuses();
                if (!isNew)
                {
                    LoadProductData();
                }
                 else
                {
                    txtProductId.ReadOnly = false;
                    // Set default status for new product
                    cboStatus.SelectedItem = "正常";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupControls()
        {
            int labelWidth = 100;
            int controlWidth = 350; // Wider controls
            int margin = 30;
            int controlHeight = 25;
            int startY = 30;
            int lineHeight = 35; // Slightly smaller line height
            int currentY = startY;

            // Product ID
            lblProductId = new Label { Text = "产品编号:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            txtProductId = new TextBox { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), MaxLength = 20, ReadOnly = !isNew };
            currentY += lineHeight;

            // Product Name
            lblProductName = new Label { Text = "产品名称:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            txtProductName = new TextBox { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), MaxLength = 100 };
            currentY += lineHeight;

            // Category
            lblCategory = new Label { Text = "产品类别:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            cboCategory = new ComboBox { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), DropDownStyle = ComboBoxStyle.DropDownList };
            currentY += lineHeight;

            // Specification
            lblSpecification = new Label { Text = "规格:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            txtSpecification = new TextBox { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), MaxLength = 100 };
            currentY += lineHeight;

            // Unit
            lblUnit = new Label { Text = "单位:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            txtUnit = new TextBox { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), MaxLength = 20 };
            currentY += lineHeight;

            // Price
            lblPrice = new Label { Text = "价格:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            nudPrice = new NumericUpDown { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), DecimalPlaces = 2, Maximum = 9999999999999999.99m, Minimum = 0 };
            currentY += lineHeight;

            // Cost
            lblCost = new Label { Text = "成本:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            nudCost = new NumericUpDown { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), DecimalPlaces = 2, Maximum = 9999999999999999.99m, Minimum = 0 };
            currentY += lineHeight;

            // Status
            lblStatus = new Label { Text = "状态:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            cboStatus = new ComboBox { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight), DropDownStyle = ComboBoxStyle.DropDownList };
            currentY += lineHeight;

            // Description
            lblDescription = new Label { Text = "描述:", Location = new Point(margin, currentY + 4), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.TopRight }; // Align top for multiline
            txtDescription = new TextBox { Location = new Point(margin + labelWidth + 10, currentY), Size = new Size(controlWidth, controlHeight * 4), MaxLength = 500, Multiline = true, ScrollBars = ScrollBars.Vertical };
            currentY += controlHeight * 4 + lineHeight; // Adjust Y after multiline textbox

            // Buttons
            int buttonWidth = 100;
            int buttonMargin = 20;
            int buttonY = currentY; // Place buttons below description
            btnSave = new Button { Text = "保存", Location = new Point((this.ClientSize.Width - buttonWidth * 2 - buttonMargin) / 2, buttonY), Size = new Size(buttonWidth, controlHeight + 5) };
            btnCancel = new Button { Text = "取消", Location = new Point(btnSave.Location.X + buttonWidth + buttonMargin, buttonY), Size = new Size(buttonWidth, controlHeight + 5) };
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // Adjust form size
            this.ClientSize = new Size(this.ClientSize.Width, buttonY + controlHeight + 5 + margin);

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblProductId, txtProductId, lblProductName, txtProductName, lblCategory, cboCategory,
                lblSpecification, txtSpecification, lblUnit, txtUnit, lblPrice, nudPrice, lblCost, nudCost,
                lblStatus, cboStatus, lblDescription, txtDescription, btnSave, btnCancel
            });
        }

        private void LoadCategories()
        {
            try
            {
                cboCategory.Items.Clear();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT CategoryId, CategoryName FROM ProductCategory ORDER BY CategoryName";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cboCategory.Items.Add(new ComboboxItem(reader["CategoryName"].ToString(), reader["CategoryId"].ToString()));
                    }
                }
                if (cboCategory.Items.Count > 0)
                {
                    cboCategory.SelectedIndex = 0; // Select first category by default
                }
                 else
                {
                      MessageBox.Show("系统中没有产品类别，请先添加类别。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                      // Consider disabling the form or closing it if categories are mandatory
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品类别失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStatuses()
        {
            cboStatus.Items.Clear();
            // Load statuses - could come from DB or config in a real app
            cboStatus.Items.Add("正常");
            cboStatus.Items.Add("停用");
            // Add other relevant statuses if needed
            cboStatus.SelectedIndex = 0; // Default to '正常'
        }

        private void LoadProductData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT ProductId, ProductName, CategoryId, Specification, Unit, Price, Cost, Description, Status
                                 FROM Product WHERE ProductId = @ProductId";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ProductId", this.productId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtProductId.Text = reader["ProductId"].ToString();
                        txtProductName.Text = reader["ProductName"].ToString();
                        txtSpecification.Text = reader["Specification"] != DBNull.Value ? reader["Specification"].ToString() : "";
                        txtUnit.Text = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : "";
                        txtDescription.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";

                        // Handle nullable decimals for NumericUpDown
                        nudPrice.Value = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"]) : 0m;
                        nudCost.Value = reader["Cost"] != DBNull.Value ? Convert.ToDecimal(reader["Cost"]) : 0m;

                        // Select category and status
                        SelectCategoryById(reader["CategoryId"].ToString());
                        cboStatus.SelectedItem = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "正常"; // Default if status is null
                    }
                    else
                    {
                        MessageBox.Show("未找到指定的产品数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void SelectCategoryById(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId)) return;
            for (int i = 0; i < cboCategory.Items.Count; i++)
            {
                if (cboCategory.Items[i] is ComboboxItem item && item.Value.ToString() == categoryId)
                {
                    cboCategory.SelectedIndex = i;
                    return;
                }
            }
             // If category not found (maybe deleted?), keep first selected or show warning
             if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtProductId.Text))
            {
                MessageBox.Show("产品编号不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductId.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("产品名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus(); return;
            }
            if (cboCategory.SelectedItem == null)
            {
                 MessageBox.Show("必须选择一个产品类别。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 cboCategory.Focus(); return;
            }
             if (cboStatus.SelectedItem == null)
            {
                 MessageBox.Show("必须选择一个产品状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 cboStatus.Focus(); return;
            }

            string selectedCategoryId = (cboCategory.SelectedItem as ComboboxItem)?.Value?.ToString();
            string selectedStatus = cboStatus.SelectedItem.ToString();

            // Handle nullable decimals - treat 0 as null/not set? Or require explicit input?
            // For now, save the value directly. Consider if 0 is a valid price/cost.
             object priceValue = nudPrice.Value == 0m ? DBNull.Value : (object)nudPrice.Value;
             object costValue = nudCost.Value == 0m ? DBNull.Value : (object)nudCost.Value;


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand { Connection = conn };

                    cmd.Parameters.AddWithValue("@ProductId", txtProductId.Text.Trim());
                    cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CategoryId", selectedCategoryId);
                    cmd.Parameters.AddWithValue("@Specification", string.IsNullOrWhiteSpace(txtSpecification.Text) ? DBNull.Value : (object)txtSpecification.Text.Trim());
                    cmd.Parameters.AddWithValue("@Unit", string.IsNullOrWhiteSpace(txtUnit.Text) ? DBNull.Value : (object)txtUnit.Text.Trim());
                    cmd.Parameters.AddWithValue("@Price", priceValue); // Use prepared value
                    cmd.Parameters.AddWithValue("@Cost", costValue);   // Use prepared value
                    cmd.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(txtDescription.Text) ? DBNull.Value : (object)txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Status", selectedStatus);
                    // CreateTime is handled by DB default

                    if (isNew)
                    {
                        string checkSql = "SELECT COUNT(*) FROM Product WHERE ProductId = @ProductId";
                        SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                        checkCmd.Parameters.AddWithValue("@ProductId", txtProductId.Text.Trim());
                        if ((int)checkCmd.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("产品编号已存在，请使用其他编号。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtProductId.Focus();
                            return;
                        }

                        cmd.CommandText = @"INSERT INTO Product (ProductId, ProductName, CategoryId, Specification, Unit, Price, Cost, Description, Status)
                                            VALUES (@ProductId, @ProductName, @CategoryId, @Specification, @Unit, @Price, @Cost, @Description, @Status)";
                    }
                    else
                    {
                        cmd.CommandText = @"UPDATE Product SET
                                                ProductName = @ProductName,
                                                CategoryId = @CategoryId,
                                                Specification = @Specification,
                                                Unit = @Unit,
                                                Price = @Price,
                                                Cost = @Cost,
                                                Description = @Description,
                                                Status = @Status
                                            WHERE ProductId = @ProductId";
                    }

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("保存失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("数据库操作失败：\n" + sqlEx.Message, "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存产品数据时发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 
