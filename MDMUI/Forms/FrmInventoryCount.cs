using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI
{
    public partial class FrmInventoryCount : Form
    {
        private User CurrentUser;
        private DataTable inventoryData;

        public FrmInventoryCount(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            this.Text = "库存盘点";
        }

        private void FrmInventoryCount_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            LoadInventoryData();
        }
        
        private void ConfigureDataGridView()
        {
            dgvInventory.AutoGenerateColumns = false;
            dgvInventory.Columns.Clear();

            // 设置列
            AddColumn("InventoryId", "盘点ID", 80);
            AddColumn("ProductId", "产品ID", 80);
            AddColumn("ProductName", "产品名称", 150);
            AddColumn("CountDate", "盘点日期", 120, "yyyy-MM-dd");
            AddColumn("BookQuantity", "账面数量", 100);
            AddColumn("ActualQuantity", "实际数量", 100);
            AddColumn("Difference", "差异数量", 100);
            AddColumn("ResponsiblePerson", "责任人", 100);
            AddColumn("Status", "状态", 80);
            AddColumn("Remarks", "备注", 150);
        }

        private void AddColumn(string dataPropertyName, string headerText, int width, string format = null)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = dataPropertyName;
            column.HeaderText = headerText;
            column.Name = "col" + dataPropertyName;
            column.Width = width;
            if (!string.IsNullOrEmpty(format))
            {
                column.DefaultCellStyle.Format = format;
            }
            dgvInventory.Columns.Add(column);
        }

        private void LoadInventoryData(string searchKeyword = null)
        {
            try
            {
                // 这里应该调用数据访问层获取实际数据
                // 暂时使用模拟数据
                inventoryData = CreateSampleData();
                
                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    // 根据关键字筛选
                    DataView dv = inventoryData.DefaultView;
                    dv.RowFilter = string.Format("ProductName LIKE '%{0}%' OR InventoryId LIKE '%{0}%'", 
                        searchKeyword.Replace("'", "''"));
                    dgvInventory.DataSource = dv.ToTable();
                }
                else
                {
                    dgvInventory.DataSource = inventoryData;
                }

                // 清空详情区
                ClearDetailArea();
                
                // 如果有数据，选中第一行
                if (dgvInventory.Rows.Count > 0)
                {
                    dgvInventory.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载库存盘点数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private DataTable CreateSampleData()
        {
            DataTable dt = new DataTable();
            
            // 添加列
            dt.Columns.Add("InventoryId", typeof(string));
            dt.Columns.Add("ProductId", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("CountDate", typeof(DateTime));
            dt.Columns.Add("BookQuantity", typeof(int));
            dt.Columns.Add("ActualQuantity", typeof(int));
            dt.Columns.Add("Difference", typeof(int));
            dt.Columns.Add("ResponsiblePerson", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));
            
            // 添加样本数据
            dt.Rows.Add("INV001", "P001", "笔记本电脑", DateTime.Today.AddDays(-5), 100, 98, -2, "张三", "已确认", "库位A001");
            dt.Rows.Add("INV002", "P002", "显示器", DateTime.Today.AddDays(-4), 50, 48, -2, "李四", "已确认", "库位A002");
            dt.Rows.Add("INV003", "P003", "键盘", DateTime.Today.AddDays(-3), 200, 195, -5, "王五", "待确认", "库位A003");
            dt.Rows.Add("INV004", "P004", "鼠标", DateTime.Today.AddDays(-2), 300, 302, 2, "赵六", "待确认", "库位A004");
            dt.Rows.Add("INV005", "P005", "耳机", DateTime.Today.AddDays(-1), 150, 145, -5, "钱七", "已确认", "库位A005");
            
            return dt;
        }

        private void ClearDetailArea()
        {
            txtInventoryId.Text = string.Empty;
            txtProductId.Text = string.Empty;
            txtProductName.Text = string.Empty;
            dtpCountDate.Value = DateTime.Today;
            txtBookQuantity.Text = "0";
            txtActualQuantity.Text = "0";
            txtDifference.Text = "0";
            txtResponsiblePerson.Text = string.Empty;
            cmbStatus.SelectedIndex = -1;
            txtRemarks.Text = string.Empty;
        }

        private void dgvInventory_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvInventory.SelectedRows[0];
                DisplayInventoryDetails(row);
            }
            else
            {
                ClearDetailArea();
            }
        }

        private void DisplayInventoryDetails(DataGridViewRow row)
        {
            try
            {
                txtInventoryId.Text = GetCellValue(row, "InventoryId");
                txtProductId.Text = GetCellValue(row, "ProductId");
                txtProductName.Text = GetCellValue(row, "ProductName");
                
                object dateValue = row.Cells["colCountDate"].Value;
                if (dateValue != null && dateValue != DBNull.Value)
                {
                    dtpCountDate.Value = (DateTime)dateValue;
                }
                
                txtBookQuantity.Text = GetCellValue(row, "BookQuantity");
                txtActualQuantity.Text = GetCellValue(row, "ActualQuantity");
                txtDifference.Text = GetCellValue(row, "Difference");
                txtResponsiblePerson.Text = GetCellValue(row, "ResponsiblePerson");
                
                string status = GetCellValue(row, "Status");
                for (int i = 0; i < cmbStatus.Items.Count; i++)
                {
                    if (cmbStatus.Items[i].ToString() == status)
                    {
                        cmbStatus.SelectedIndex = i;
                        break;
                    }
                }
                
                txtRemarks.Text = GetCellValue(row, "Remarks");
            }
            catch (Exception ex)
            {
                MessageBox.Show("显示库存盘点详情失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            object value = row.Cells["col" + columnName].Value;
            return value == null || value == DBNull.Value ? string.Empty : value.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            LoadInventoryData(keyword);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadInventoryData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // 切换到详情面板
            panelDetail.BringToFront();
            ClearDetailArea();
            txtInventoryId.Text = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss");
            txtResponsiblePerson.Text = CurrentUser.RealName;
            txtInventoryId.ReadOnly = true;
            btnSave.Tag = "New";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要编辑的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 切换到详情面板
            panelDetail.BringToFront();
            btnSave.Tag = "Edit";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string inventoryId = dgvInventory.SelectedRows[0].Cells["colInventoryId"].Value.ToString();
            string productName = dgvInventory.SelectedRows[0].Cells["colProductName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"确定要删除产品[{productName}]的盘点记录吗？",
                "确认删除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // 实际应用中，这里应调用数据访问层删除数据
                    // 模拟删除
                    DataRow[] rowsToDelete = inventoryData.Select($"InventoryId = '{inventoryId}'");
                    if (rowsToDelete.Length > 0)
                    {
                        rowsToDelete[0].Delete();
                        inventoryData.AcceptChanges();
                        MessageBox.Show("删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadInventoryData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 数据验证
            if (string.IsNullOrEmpty(txtProductId.Text))
            {
                MessageBox.Show("请输入产品ID", "验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductId.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                MessageBox.Show("请输入产品名称", "验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return;
            }

            try
            {
                // 计算差异
                int bookQty = int.Parse(txtBookQuantity.Text);
                int actualQty = int.Parse(txtActualQuantity.Text);
                int difference = actualQty - bookQty;
                txtDifference.Text = difference.ToString();

                // 实际应用中，这里应调用数据访问层保存数据
                if (btnSave.Tag.ToString() == "New")
                {
                    // 新增记录
                    DataRow newRow = inventoryData.NewRow();
                    newRow["InventoryId"] = txtInventoryId.Text;
                    newRow["ProductId"] = txtProductId.Text;
                    newRow["ProductName"] = txtProductName.Text;
                    newRow["CountDate"] = dtpCountDate.Value;
                    newRow["BookQuantity"] = bookQty;
                    newRow["ActualQuantity"] = actualQty;
                    newRow["Difference"] = difference;
                    newRow["ResponsiblePerson"] = txtResponsiblePerson.Text;
                    newRow["Status"] = cmbStatus.SelectedItem?.ToString() ?? "待确认";
                    newRow["Remarks"] = txtRemarks.Text;
                    
                    inventoryData.Rows.Add(newRow);
                    MessageBox.Show("新增成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // 编辑记录
                    string inventoryId = txtInventoryId.Text;
                    DataRow[] rowsToUpdate = inventoryData.Select($"InventoryId = '{inventoryId}'");
                    if (rowsToUpdate.Length > 0)
                    {
                        rowsToUpdate[0]["ProductId"] = txtProductId.Text;
                        rowsToUpdate[0]["ProductName"] = txtProductName.Text;
                        rowsToUpdate[0]["CountDate"] = dtpCountDate.Value;
                        rowsToUpdate[0]["BookQuantity"] = bookQty;
                        rowsToUpdate[0]["ActualQuantity"] = actualQty;
                        rowsToUpdate[0]["Difference"] = difference;
                        rowsToUpdate[0]["ResponsiblePerson"] = txtResponsiblePerson.Text;
                        rowsToUpdate[0]["Status"] = cmbStatus.SelectedItem?.ToString() ?? "待确认";
                        rowsToUpdate[0]["Remarks"] = txtRemarks.Text;
                        
                        MessageBox.Show("更新成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                inventoryData.AcceptChanges();
                LoadInventoryData();
                panelList.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 返回列表面板
            panelList.BringToFront();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导出功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtActualQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }

        private void txtBookQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }

        private void CalculateDifference()
        {
            try
            {
                if (int.TryParse(txtBookQuantity.Text, out int bookQty) && 
                    int.TryParse(txtActualQuantity.Text, out int actualQty))
                {
                    int difference = actualQty - bookQty;
                    txtDifference.Text = difference.ToString();
                }
            }
            catch
            {
                txtDifference.Text = "0";
            }
        }
    }
} 