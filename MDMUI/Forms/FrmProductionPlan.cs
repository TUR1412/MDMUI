using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDMUI.Model;

namespace MDMUI
{
    public partial class FrmProductionPlan : Form
    {
        private User CurrentUser;
        private List<ProductionPlan> planData;
        private bool isEditing = false;
        private int currentPlanId = 1000;

        public FrmProductionPlan(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            this.Text = "生产计划";
        }

        private void FrmProductionPlan_Load(object sender, EventArgs e)
        {
            // 初始化界面
            InitializeUI();
            
            // 加载数据
            LoadData();
            
            // 更新界面显示
            UpdateUI();
        }

        private void InitializeUI()
        {
            // 设置DataGridView列
            dgvPlan.AutoGenerateColumns = false;
            dgvPlan.Columns.Clear(); 
            dgvPlan.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 
            dgvPlan.MultiSelect = false;
            dgvPlan.AllowUserToAddRows = false;
            dgvPlan.AllowUserToDeleteRows = false;
            dgvPlan.ReadOnly = true;
            
            // Apply styles from FrmProductionRecord.Designer.cs
            dgvPlan.BackgroundColor = System.Drawing.Color.White;
            dgvPlan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvPlan.RowHeadersVisible = false; // Hide row headers like in FrmProductionRecord
            dgvPlan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill; // Use Fill mode
            dgvPlan.EnableHeadersVisualStyles = true; // Use default header styles

            // Ensure no custom header styles are applied
            // dgvPlan.ColumnHeadersDefaultCellStyle... (Remove/Comment out any custom header styles)

            // Ensure no alternating row styles are applied
            // dgvPlan.AlternatingRowsDefaultCellStyle... (Remove/Comment out)

            // Reset Default Cell Style (Keep necessary formats)
            dgvPlan.DefaultCellStyle.BackColor = SystemColors.Window; // Default background
            dgvPlan.DefaultCellStyle.ForeColor = SystemColors.ControlText; // Default text color
            dgvPlan.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight; // Default selection background
            dgvPlan.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText; // Default selection text color

            // Re-add columns with widths (Fill mode might override some)
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PlanId",
                DataPropertyName = "PlanId",
                HeaderText = "计划编号",
                Width = 80, 
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None // Set explicitly if Fill is overriding needed width
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                DataPropertyName = "ProductName",
                HeaderText = "产品名称",
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PlanQuantity",
                DataPropertyName = "PlanQuantity",
                HeaderText = "计划数量",
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None 
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartDate",
                DataPropertyName = "StartDate",
                HeaderText = "开始日期",
                Width = 100, 
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" }, // Keep format
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EndDate",
                DataPropertyName = "EndDate",
                HeaderText = "结束日期",
                Width = 100, 
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" }, // Keep format
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Responsible",
                DataPropertyName = "Responsible",
                HeaderText = "负责人",
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None 
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                DataPropertyName = "Status",
                HeaderText = "状态",
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None 
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Priority",
                DataPropertyName = "Priority",
                HeaderText = "优先级",
                Width = 70,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None 
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Progress",
                DataPropertyName = "Progress",
                HeaderText = "进度(%)", 
                Width = 70, 
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }, // Keep format
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });
            
            dgvPlan.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Notes",
                DataPropertyName = "Notes",
                HeaderText = "备注",
                // Width will be determined by Fill mode primarily
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Let Notes fill remaining space
            });

            cmbStatus.SelectedIndex = 0;
            cmbPriority.SelectedIndex = 1;
            panelDetail.Visible = false;
            panelList.Visible = true;
            panelList.Dock = DockStyle.Fill;
        }

        private void LoadData()
        {
            // 模拟从数据库加载数据
            planData = new List<ProductionPlan>();
            
            // 添加一些示例数据
            planData.Add(new ProductionPlan
            {
                PlanId = 1001,
                ProductName = "工业齿轮A型",
                PlanQuantity = 500,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(20),
                Responsible = "张工",
                Status = "进行中",
                Priority = "高",
                Progress = 35,
                Notes = "按客户要求优先处理"
            });
            
            planData.Add(new ProductionPlan
            {
                PlanId = 1002,
                ProductName = "精密轴承B系列",
                PlanQuantity = 2000,
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(10),
                Responsible = "李工",
                Status = "进行中",
                Priority = "中",
                Progress = 50,
                Notes = "常规生产"
            });
            
            planData.Add(new ProductionPlan
            {
                PlanId = 1003,
                ProductName = "液压阀门C型",
                PlanQuantity = 300,
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(15),
                Responsible = "王工",
                Status = "未开始",
                Priority = "中",
                Progress = 0,
                Notes = "等待原材料到货"
            });
            
            planData.Add(new ProductionPlan
            {
                PlanId = 1004,
                ProductName = "电子控制板D系列",
                PlanQuantity = 1000,
                StartDate = DateTime.Now.AddDays(-15),
                EndDate = DateTime.Now.AddDays(-2),
                Responsible = "赵工",
                Status = "完成",
                Priority = "高",
                Progress = 100,
                Notes = "已完成质检"
            });
            
            // 更新当前最大ID
            currentPlanId = planData.Max(p => p.PlanId);
            
            // 绑定数据到DataGridView
            dgvPlan.DataSource = new BindingList<ProductionPlan>(planData);
        }

        private void UpdateUI()
        {
            // 根据是否有选中行，启用或禁用按钮
            bool hasSelection = dgvPlan.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void ShowPlanDetail(ProductionPlan plan)
        {
            // 显示计划详情
            txtPlanId.Text = plan.PlanId.ToString();
            txtProductName.Text = plan.ProductName;
            txtPlanQuantity.Text = plan.PlanQuantity.ToString();
            dtpStartDate.Value = plan.StartDate;
            dtpEndDate.Value = plan.EndDate;
            txtResponsible.Text = plan.Responsible;
            
            // 设置状态
            switch (plan.Status)
            {
                case "未开始":
                    cmbStatus.SelectedIndex = 0;
                    break;
                case "进行中":
                    cmbStatus.SelectedIndex = 1;
                    break;
                case "暂停":
                    cmbStatus.SelectedIndex = 2;
                    break;
                case "完成":
                    cmbStatus.SelectedIndex = 3;
                    break;
                default:
                    cmbStatus.SelectedIndex = 0;
                    break;
            }
            
            // 设置优先级
            switch (plan.Priority)
            {
                case "高":
                    cmbPriority.SelectedIndex = 0;
                    break;
                case "中":
                    cmbPriority.SelectedIndex = 1;
                    break;
                case "低":
                    cmbPriority.SelectedIndex = 2;
                    break;
                default:
                    cmbPriority.SelectedIndex = 1;
                    break;
            }
            
            // 设置进度
            trkProgress.Value = plan.Progress;
            txtProgress.Text = plan.Progress.ToString();
            
            txtNotes.Text = plan.Notes;
        }

        private ProductionPlan GetPlanFromUI()
        {
            // 从UI获取计划数据
            ProductionPlan plan = new ProductionPlan();
            
            if (!string.IsNullOrEmpty(txtPlanId.Text))
            {
                plan.PlanId = int.Parse(txtPlanId.Text);
            }
            else
            {
                plan.PlanId = ++currentPlanId;
            }
            
            plan.ProductName = txtProductName.Text;
            
            if (int.TryParse(txtPlanQuantity.Text, out int quantity))
            {
                plan.PlanQuantity = quantity;
            }
            
            plan.StartDate = dtpStartDate.Value;
            plan.EndDate = dtpEndDate.Value;
            plan.Responsible = txtResponsible.Text;
            
            // 获取状态
            switch (cmbStatus.SelectedIndex)
            {
                case 0:
                    plan.Status = "未开始";
                    break;
                case 1:
                    plan.Status = "进行中";
                    break;
                case 2:
                    plan.Status = "暂停";
                    break;
                case 3:
                    plan.Status = "完成";
                    break;
                default:
                    plan.Status = "未开始";
                    break;
            }
            
            // 获取优先级
            switch (cmbPriority.SelectedIndex)
            {
                case 0:
                    plan.Priority = "高";
                    break;
                case 1:
                    plan.Priority = "中";
                    break;
                case 2:
                    plan.Priority = "低";
                    break;
                default:
                    plan.Priority = "中";
                    break;
            }
            
            plan.Progress = trkProgress.Value;
            plan.Notes = txtNotes.Text;
            
            return plan;
        }

        private bool ValidatePlanData()
        {
            // 验证输入数据
            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                MessageBox.Show("请输入产品名称", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }
            
            if (string.IsNullOrEmpty(txtPlanQuantity.Text) || !int.TryParse(txtPlanQuantity.Text, out _))
            {
                MessageBox.Show("请输入有效的计划数量", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlanQuantity.Focus();
                return false;
            }
            
            if (dtpEndDate.Value < dtpStartDate.Value)
            {
                MessageBox.Show("结束日期不能早于开始日期", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDate.Focus();
                return false;
            }
            
            if (string.IsNullOrEmpty(txtResponsible.Text))
            {
                MessageBox.Show("请输入负责人", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtResponsible.Focus();
                return false;
            }
            
            return true;
        }

        private void ShowDetailPanel(bool isEdit)
        {
            // 显示详情面板
            isEditing = isEdit;
            panelList.Visible = false;
            panelDetail.Visible = true;
            panelDetail.Dock = DockStyle.Fill;
            
            // 如果是新建，则清空输入框
            if (!isEdit)
            {
                txtPlanId.Text = (currentPlanId + 1).ToString();
                txtProductName.Text = string.Empty;
                txtPlanQuantity.Text = string.Empty;
                dtpStartDate.Value = DateTime.Now;
                dtpEndDate.Value = DateTime.Now.AddDays(7);
                txtResponsible.Text = string.Empty;
                cmbStatus.SelectedIndex = 0;
                cmbPriority.SelectedIndex = 1;
                trkProgress.Value = 0;
                txtProgress.Text = "0";
                txtNotes.Text = string.Empty;
            }
        }

        private void ShowListPanel()
        {
            // 显示列表面板
            panelDetail.Visible = false;
            panelList.Visible = true;
            panelList.Dock = DockStyle.Fill;
        }

        #region 事件处理

        private void btnNew_Click(object sender, EventArgs e)
        {
            ShowDetailPanel(false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPlan.SelectedRows.Count > 0)
            {
                // 获取选中的行
                DataGridViewRow row = dgvPlan.SelectedRows[0];
                
                // 获取对应的计划对象
                ProductionPlan plan = planData.FirstOrDefault(p => p.PlanId.ToString() == row.Cells["PlanId"].Value.ToString());
                
                if (plan != null)
                {
                    // 显示计划详情
                    ShowPlanDetail(plan);
                    
                    // 显示详情面板
                    ShowDetailPanel(true);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPlan.SelectedRows.Count > 0)
            {
                // 获取选中的行
                DataGridViewRow row = dgvPlan.SelectedRows[0];
                
                // 获取计划ID
                int planId = Convert.ToInt32(row.Cells["PlanId"].Value);
                
                // 确认删除
                DialogResult result = MessageBox.Show("确定要删除此计划吗？", "确认删除", 
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    // 从数据源中删除
                    ProductionPlan planToRemove = planData.FirstOrDefault(p => p.PlanId == planId);
                    if (planToRemove != null)
                    {
                        planData.Remove(planToRemove);
                        
                        // 刷新DataGridView
                        dgvPlan.DataSource = null;
                        dgvPlan.DataSource = new BindingList<ProductionPlan>(planData);
                        
                        // 更新UI状态
                        UpdateUI();
                        
                        MessageBox.Show("删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导出功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("报表功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            // Get date filter values
            DateTime? filterStartDate = dtpFilterStartDate.Checked ? dtpFilterStartDate.Value.Date : (DateTime?)null;
            DateTime? filterEndDate = dtpFilterEndDate.Checked ? dtpFilterEndDate.Value.Date.AddDays(1).AddTicks(-1) : (DateTime?)null; // End of selected day
            
            // Filter data based on keyword and date range
            var filteredData = planData.Where(p => 
                // Date filtering
                (!filterStartDate.HasValue || p.StartDate >= filterStartDate.Value) &&
                (!filterEndDate.HasValue || p.EndDate <= filterEndDate.Value) &&
                // Keyword filtering (optional)
                (string.IsNullOrEmpty(keyword) || 
                 p.PlanId.ToString().Contains(keyword) ||
                 p.ProductName.ToLower().Contains(keyword) ||
                 p.Responsible.ToLower().Contains(keyword) ||
                 p.Status.ToLower().Contains(keyword) ||
                 (p.Notes != null && p.Notes.ToLower().Contains(keyword)) // Check Notes for null
                )
            ).ToList();
                
            dgvPlan.DataSource = new BindingList<ProductionPlan>(filteredData);
            UpdateUI(); // Update button states based on filtered selection
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // 清空搜索框和日期选择
            txtSearch.Text = string.Empty;
            dtpFilterStartDate.Checked = false;
            dtpFilterEndDate.Checked = false;
            
            // 重新加载所有数据 (or re-apply default filters if any)
            dgvPlan.DataSource = new BindingList<ProductionPlan>(planData); 
            UpdateUI();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (!ValidatePlanData())
            {
                return;
            }
            
            // 获取计划数据
            ProductionPlan plan = GetPlanFromUI();
            
            if (isEditing)
            {
                // 更新现有计划
                ProductionPlan existingPlan = planData.FirstOrDefault(p => p.PlanId == plan.PlanId);
                if (existingPlan != null)
                {
                    // 更新属性
                    existingPlan.ProductName = plan.ProductName;
                    existingPlan.PlanQuantity = plan.PlanQuantity;
                    existingPlan.StartDate = plan.StartDate;
                    existingPlan.EndDate = plan.EndDate;
                    existingPlan.Responsible = plan.Responsible;
                    existingPlan.Status = plan.Status;
                    existingPlan.Priority = plan.Priority;
                    existingPlan.Progress = plan.Progress;
                    existingPlan.Notes = plan.Notes;
                }
            }
            else
            {
                // 添加新计划
                planData.Add(plan);
                currentPlanId = plan.PlanId;
            }
            
            // 刷新数据显示
            dgvPlan.DataSource = null;
            dgvPlan.DataSource = new BindingList<ProductionPlan>(planData);
            
            // 返回列表视图
            ShowListPanel();
            
            // 显示成功消息
            MessageBox.Show(isEditing ? "计划更新成功！" : "新计划添加成功！", 
                           "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 返回列表视图
            ShowListPanel();
        }

        private void dgvPlan_SelectionChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void trkProgress_ValueChanged(object sender, EventArgs e)
        {
            // 同步进度值到文本框
            txtProgress.Text = trkProgress.Value.ToString();
        }

        private void txtProgress_TextChanged(object sender, EventArgs e)
        {
            // 同步文本框的值到滑块
            if (int.TryParse(txtProgress.Text, out int progress))
            {
                if (progress >= 0 && progress <= 100)
                {
                    trkProgress.Value = progress;
                }
            }
        }

        #endregion
    }

    // 生产计划实体类
    public class ProductionPlan
    {
        public int PlanId { get; set; }
        public string ProductName { get; set; }
        public int PlanQuantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Responsible { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public int Progress { get; set; }
        public string Notes { get; set; }
    }
} 