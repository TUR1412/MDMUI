using System;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model;
using System.Data;

namespace MDMUI
{
    public partial class FrmProductionRecord : Form
    {
        private User CurrentUser;
        private DataTable dtProduction;

        public FrmProductionRecord(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            this.Text = "生产记录";
            InitializeControls();
            LoadData();
        }

        private void InitializeControls()
        {
            // 此方法在InitializeComponent之后调用
            try
            {
                // 设置日期控件默认值
                if (dtpStartDate != null)
                    dtpStartDate.Value = DateTime.Now.AddDays(-30);
                    
                if (dtpEndDate != null)
                    dtpEndDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化控件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                // 模拟数据，实际应用中应从数据库获取
                dtProduction = new DataTable();
                dtProduction.Columns.Add("ID", typeof(int));
                dtProduction.Columns.Add("ProductName", typeof(string));
                dtProduction.Columns.Add("BatchNo", typeof(string));
                dtProduction.Columns.Add("ProductionDate", typeof(DateTime));
                dtProduction.Columns.Add("Quantity", typeof(int));
                dtProduction.Columns.Add("Person", typeof(string));
                dtProduction.Columns.Add("Status", typeof(string));
                
                // 添加示例数据
                dtProduction.Rows.Add(1, "产品A", "BATCH001", DateTime.Now.AddDays(-5), 100, "张三", "已完成");
                dtProduction.Rows.Add(2, "产品B", "BATCH002", DateTime.Now.AddDays(-4), 150, "李四", "已完成");
                dtProduction.Rows.Add(3, "产品C", "BATCH003", DateTime.Now.AddDays(-2), 200, "王五", "生产中");
                dtProduction.Rows.Add(4, "产品A", "BATCH004", DateTime.Now.AddDays(-1), 120, "赵六", "生产中");
                
                if (dgvProductionRecords != null)
                {
                    dgvProductionRecords.DataSource = dtProduction;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProductionRecords_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                if (dgv != null && dgv.Columns.Count > 0)
                {
                    // Move column configuration logic here
                    // 配置 ID 列
                    if (dgv.Columns["ID"] != null)
                    {
                        dgv.Columns["ID"].HeaderText = "ID";
                        dgv.Columns["ID"].Width = 60;
                    }

                    // 配置 ProductName 列
                    if (dgv.Columns["ProductName"] != null)
                    {
                        dgv.Columns["ProductName"].HeaderText = "产品名称";
                        dgv.Columns["ProductName"].Width = 120;
                    }

                    // 配置 BatchNo 列
                    if (dgv.Columns["BatchNo"] != null)
                    {
                        dgv.Columns["BatchNo"].HeaderText = "批次号";
                        dgv.Columns["BatchNo"].Width = 100;
                    }

                    // 配置 ProductionDate 列
                    if (dgv.Columns["ProductionDate"] != null)
                    {
                        dgv.Columns["ProductionDate"].HeaderText = "生产日期";
                        dgv.Columns["ProductionDate"].Width = 120;
                        dgv.Columns["ProductionDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    }

                    // 配置 Quantity 列
                    if (dgv.Columns["Quantity"] != null)
                    {
                        dgv.Columns["Quantity"].HeaderText = "生产数量";
                        dgv.Columns["Quantity"].Width = 100;
                    }

                    // 配置 Person 列
                    if (dgv.Columns["Person"] != null)
                    {
                        dgv.Columns["Person"].HeaderText = "负责人";
                        dgv.Columns["Person"].Width = 100;
                    }

                    // 配置 Status 列
                    if (dgv.Columns["Status"] != null)
                    {
                        dgv.Columns["Status"].HeaderText = "状态";
                        dgv.Columns["Status"].Width = 100;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置列属性时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmProductionRecord_Load(object sender, EventArgs e)
        {
            try
            {
                // 窗体加载时的初始化逻辑
                Console.WriteLine("生产记录窗体已加载");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"窗体加载时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // 实现查询逻辑
                if (txtProductName != null && dtpStartDate != null && dtpEndDate != null)
                {
                    MessageBox.Show($"查询条件：产品名称={txtProductName.Text}，开始日期={dtpStartDate.Value.ToShortDateString()}，结束日期={dtpEndDate.Value.ToShortDateString()}", 
                        "查询", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // 实际应用中应根据条件筛选数据
                    // 此处仅为示例，重新加载所有数据
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                // 重置查询条件
                if (txtProductName != null && dtpStartDate != null && dtpEndDate != null)
                {
                    txtProductName.Text = "";
                    dtpStartDate.Value = DateTime.Now.AddDays(-30);
                    dtpEndDate.Value = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重置条件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开新增记录窗口
                MessageBox.Show("打开新增生产记录窗口", "新增", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // 编辑选中记录
                if (dgvProductionRecords != null && dgvProductionRecords.SelectedRows.Count > 0)
                {
                    int selectedRowIndex = dgvProductionRecords.SelectedRows[0].Index;
                    int selectedID = Convert.ToInt32(dgvProductionRecords.Rows[selectedRowIndex].Cells[0].Value);
                    MessageBox.Show($"编辑ID为{selectedID}的记录", "编辑", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("请先选择要编辑的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"编辑记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // 删除选中记录
                if (dgvProductionRecords != null && dgvProductionRecords.SelectedRows.Count > 0)
                {
                    int selectedRowIndex = dgvProductionRecords.SelectedRows[0].Index;
                    int selectedID = Convert.ToInt32(dgvProductionRecords.Rows[selectedRowIndex].Cells[0].Value);
                    
                    if (MessageBox.Show($"确定要删除ID为{selectedID}的记录吗？", "确认删除", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // 实际应用中应该从数据库删除记录
                        if (dtProduction != null)
                        {
                            // 从数据源中删除
                            dtProduction.Rows.RemoveAt(selectedRowIndex);
                            MessageBox.Show("记录已删除", "删除成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // 导出数据功能
                MessageBox.Show("数据导出功能", "导出", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void dgvProductionRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 双击查看详情
                if (e.RowIndex >= 0 && dgvProductionRecords != null)
                {
                    int selectedID = Convert.ToInt32(dgvProductionRecords.Rows[e.RowIndex].Cells[0].Value);
                    MessageBox.Show($"查看ID为{selectedID}的记录详情", "记录详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查看详情时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 