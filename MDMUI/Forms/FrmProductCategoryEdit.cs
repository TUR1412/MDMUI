using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model; // For ProductCategory and ComboboxItem
using MDMUI.Utility;

namespace MDMUI
{
    /// <summary>
    /// 产品类别编辑窗体
    /// </summary>
    public partial class FrmProductCategoryEdit : Form
    {
        private bool isNew = true;
        private string categoryId = string.Empty;
        private string initialParentId = null; // To store parentId passed from constructor
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        // UI Controls
        private Label lblCategoryId;
        private TextBox txtCategoryId;
        private Label lblCategoryName;
        private TextBox txtCategoryName;
        private Label lblParentCategory;
        private ComboBox cboParentCategory;
        private Label lblDescription;
        private TextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;

        /// <summary>
        /// Public property to retrieve the saved category ID.
        /// </summary>
        public string SavedCategoryId { get; private set; }

        /// <summary>
        /// Constructor - Add Category
        /// </summary>
        public FrmProductCategoryEdit()
        {
            isNew = true;
            InitializeComponent();
            this.Text = "添加产品类别";
            this.Load += FrmProductCategoryEdit_Load;
        }

        /// <summary>
        /// Constructor - Add Category with specified parent
        /// </summary>
        public FrmProductCategoryEdit(string parentId, string parentName)
        {
            isNew = true;
            this.initialParentId = parentId;
            InitializeComponent();
            this.Text = $"添加子类别到 '{parentName}'";
            this.Load += FrmProductCategoryEdit_Load;
        }

        /// <summary>
        /// Constructor - Edit Category
        /// </summary>
        /// <param name="categoryId">Category ID to edit</param>
        public FrmProductCategoryEdit(string categoryId)
        {
            isNew = false;
            this.categoryId = categoryId;
            InitializeComponent();
            this.Text = "编辑产品类别";
            this.Load += FrmProductCategoryEdit_Load;
        }

        /// <summary>
        /// Constructor - Edit Category with specified parent context
        /// </summary>
        public FrmProductCategoryEdit(string parentId, string parentName, string categoryIdToEdit)
        {
            isNew = false;
            this.categoryId = categoryIdToEdit;
            this.initialParentId = parentId; // Store parent for context, even if loading actual parent later
            InitializeComponent();
            this.Text = "编辑产品类别";
            this.Load += FrmProductCategoryEdit_Load;
        }

        private void FrmProductCategoryEdit_Load(object sender, EventArgs e)
        {
            SetupControls();
            try
            {
                LoadParentCategories();
                if (!isNew)
                {
                    LoadCategoryData();
                }
                else
                {
                    txtCategoryId.ReadOnly = false; // Allow ID input for new category
                    SelectParentCategoryById(this.initialParentId); // Select the specified parent for new category
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
            int controlWidth = 320;
            int margin = 30;
            int controlHeight = 25;
            int startY = 30;
            int lineHeight = 40;

            // Category ID
            lblCategoryId = new Label { Text = "类别编号:", Location = new Point(margin, startY), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            txtCategoryId = new TextBox { Location = new Point(margin + labelWidth + 10, startY), Size = new Size(controlWidth, controlHeight), MaxLength = 20, ReadOnly = !isNew };

            // Category Name
            lblCategoryName = new Label { Text = "类别名称:", Location = new Point(margin, startY + lineHeight), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            txtCategoryName = new TextBox { Location = new Point(margin + labelWidth + 10, startY + lineHeight), Size = new Size(controlWidth, controlHeight), MaxLength = 50 };

            // Parent Category
            lblParentCategory = new Label { Text = "上级类别:", Location = new Point(margin, startY + lineHeight * 2), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            cboParentCategory = new ComboBox { Location = new Point(margin + labelWidth + 10, startY + lineHeight * 2), Size = new Size(controlWidth, controlHeight), DropDownStyle = ComboBoxStyle.DropDownList };

            // Description
            lblDescription = new Label { Text = "描述:", Location = new Point(margin, startY + lineHeight * 3), Size = new Size(labelWidth, controlHeight), TextAlign = ContentAlignment.MiddleRight };
            txtDescription = new TextBox { Location = new Point(margin + labelWidth + 10, startY + lineHeight * 3), Size = new Size(controlWidth, controlHeight * 3), MaxLength = 200, Multiline = true, ScrollBars = ScrollBars.Vertical };

            // Buttons
            int buttonWidth = 100;
            int buttonMargin = 20;
            int buttonY = startY + lineHeight * 3 + controlHeight * 3 + 20;
            btnSave = new Button { Text = "保存", Location = new Point((this.ClientSize.Width - buttonWidth * 2 - buttonMargin) / 2, buttonY), Size = new Size(buttonWidth, controlHeight + 5) };
            btnCancel = new Button { Text = "取消", Location = new Point(btnSave.Location.X + buttonWidth + buttonMargin, buttonY), Size = new Size(buttonWidth, controlHeight + 5) };
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // Adjust form size
            this.ClientSize = new Size(this.ClientSize.Width, buttonY + controlHeight + 5 + margin);

            // Add controls
            this.Controls.AddRange(new Control[] {
                lblCategoryId, txtCategoryId, lblCategoryName, txtCategoryName, lblParentCategory,
                cboParentCategory, lblDescription, txtDescription, btnSave, btnCancel
            });
        }

        private void LoadParentCategories()
        {
            try
            {
                cboParentCategory.Items.Clear();
                cboParentCategory.Items.Add(new ComboboxItem("(无)", "")); // Top-level category

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT CategoryId, CategoryName FROM ProductCategory";
                    if (!isNew && !string.IsNullOrEmpty(this.categoryId))
                    {
                        sql += " WHERE CategoryId NOT IN (SELECT CategoryId FROM dbo.GetCategoryDescendants(@CurrentCategoryId))";
                    }
                    sql += " ORDER BY CategoryName";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (!isNew && !string.IsNullOrEmpty(this.categoryId))
                    {
                        cmd.Parameters.AddWithValue("@CurrentCategoryId", this.categoryId);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cboParentCategory.Items.Add(new ComboboxItem(reader["CategoryName"].ToString(), reader["CategoryId"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载上级类别数据失败：" + ex.Message + "\n\n请确保数据库中存在 GetCategoryDescendants 函数。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategoryData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT CategoryId, CategoryName, ParentCategoryId, Description FROM ProductCategory WHERE CategoryId = @CategoryId";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@CategoryId", this.categoryId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtCategoryId.Text = reader["CategoryId"].ToString();
                        txtCategoryName.Text = reader["CategoryName"].ToString();
                        txtDescription.Text = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "";
                        string parentId = reader["ParentCategoryId"] != DBNull.Value ? reader["ParentCategoryId"].ToString() : "";
                        SelectParentCategoryById(parentId);
                    }
                    else
                    {
                        MessageBox.Show("未找到指定的产品类别数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品类别数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void SelectParentCategoryById(string parentId)
        {
            if (string.IsNullOrEmpty(parentId))
            {
                cboParentCategory.SelectedIndex = 0; // Select '(None)'
                return;
            }
            for (int i = 0; i < cboParentCategory.Items.Count; i++)
            {
                if (cboParentCategory.Items[i] is ComboboxItem item && item.Value.ToString() == parentId)
                {
                    cboParentCategory.SelectedIndex = i;
                    return;
                }
            }
            cboParentCategory.SelectedIndex = 0; // Default to '(None)' if not found
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryId.Text))
            {
                MessageBox.Show("类别编号不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryId.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("类别名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return;
            }

            string currentCategoryId = txtCategoryId.Text.Trim();
            string currentCategoryName = txtCategoryName.Text.Trim();
            string parentId = (cboParentCategory.SelectedItem as ComboboxItem)?.Value?.ToString();
            object parentIdValue = string.IsNullOrEmpty(parentId) ? DBNull.Value : (object)parentId;
            string description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(); // Use null for DB
            object descriptionValue = description == null ? DBNull.Value : (object)description;

            // Prevent setting parent to self
            if (!isNew && currentCategoryId == parentId)
            {
                 MessageBox.Show("上级类别不能是自身。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand { Connection = conn };

                    if (isNew)
                    {
                        // Check if ID already exists
                        string checkSql = "SELECT COUNT(*) FROM ProductCategory WHERE CategoryId = @CategoryId";
                        cmd.CommandText = checkSql;
                        cmd.Parameters.AddWithValue("@CategoryId", currentCategoryId);
                        int existingCount = (int)cmd.ExecuteScalar();
                        if (existingCount > 0)
                        {
                            MessageBox.Show("类别编号已存在，请使用其他编号。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtCategoryId.Focus();
                            return;
                        }
                        cmd.Parameters.Clear(); // Clear parameters before setting for INSERT

                        // Insert new category
                        string insertSql = "INSERT INTO ProductCategory (CategoryId, CategoryName, ParentCategoryId, Description) VALUES (@CategoryId, @CategoryName, @ParentCategoryId, @Description)";
                        cmd.CommandText = insertSql;
                        cmd.Parameters.AddWithValue("@CategoryId", currentCategoryId);
                        cmd.Parameters.AddWithValue("@CategoryName", currentCategoryName);
                        cmd.Parameters.AddWithValue("@ParentCategoryId", parentIdValue);
                        cmd.Parameters.AddWithValue("@Description", descriptionValue);
                    }
                    else
                    {
                        // Update existing category
                        // Check for potential circular dependency before update (already partially handled in LoadParentCategories)
                        if (parentId == this.categoryId) // Double check parent not set to self
                        {
                             MessageBox.Show("上级类别不能是自身。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                             return;
                        }

                        string updateSql = "UPDATE ProductCategory SET CategoryName = @CategoryName, ParentCategoryId = @ParentCategoryId, Description = @Description WHERE CategoryId = @CategoryId";
                        cmd.CommandText = updateSql;
                        cmd.Parameters.AddWithValue("@CategoryName", currentCategoryName);
                        cmd.Parameters.AddWithValue("@ParentCategoryId", parentIdValue);
                        cmd.Parameters.AddWithValue("@Description", descriptionValue);
                        cmd.Parameters.AddWithValue("@CategoryId", this.categoryId); // Use original ID for WHERE clause
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        this.SavedCategoryId = currentCategoryId; // Set the public property
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("保存产品类别失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                 MessageBox.Show("数据库操作失败：\n" + sqlEx.Message, "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存产品类别时发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 
