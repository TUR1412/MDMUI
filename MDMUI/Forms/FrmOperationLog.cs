using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MDMUI.Model;
using MDMUI.BLL;

namespace MDMUI
{
    public partial class FrmOperationLog : Form
    {
        private User CurrentUser;
        private SystemLogBLL logBll;

        public FrmOperationLog(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            logBll = new SystemLogBLL();
        }

        private void FrmOperationLog_Load(object sender, EventArgs e)
        {
            // Comment out the reference to the removed/non-existent userLabel
            // if (this.userLabel != null) 
            // {
            //     this.userLabel.Text = $"当前用户: {CurrentUser?.Username ?? "未知用户"}";
            // }
            Console.WriteLine("操作日志窗体已加载");

            // 设置初始日期范围为最近30天
            dtpStartDate.Value = DateTime.Now.Date.AddDays(-30);
            dtpStartDate.Checked = true;
            dtpEndDate.Value = DateTime.Now.Date;
            dtpEndDate.Checked = true;

            ConfigureDataGridView();
            LoadFilterOptions();
            
            // 添加日志
            Console.WriteLine($"正在加载操作日志，开始日期: {dtpStartDate.Value:yyyy-MM-dd}，结束日期: {dtpEndDate.Value:yyyy-MM-dd}");
            
            // 立即加载数据
            LoadData();

            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
        }

        private void ConfigureDataGridView()
        {
            dgvLogs.AutoGenerateColumns = false;
            dgvLogs.Columns.Clear();
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.MultiSelect = false;
            dgvLogs.AllowUserToAddRows = false;
            dgvLogs.AllowUserToDeleteRows = false;
            dgvLogs.ReadOnly = true;
            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.EnableHeadersVisualStyles = false;

            dgvLogs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLogs.ColumnHeadersDefaultCellStyle.Font = new Font(dgvLogs.Font, FontStyle.Bold);
            dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlLight;
            dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;

            dgvLogs.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            AddLogColumn("LogTime", "时间", 150, "yyyy-MM-dd HH:mm:ss", true, DataGridViewContentAlignment.MiddleCenter);
            AddLogColumn("UserName", "操作用户", 100, null, true, DataGridViewContentAlignment.MiddleCenter);
            AddLogColumn("OperationModule", "操作模块", 120);
            AddLogColumn("OperationType", "操作类型", 100);
            AddLogColumn("Description", "详细描述", 300);
            AddLogColumn("IPAddress", "IP地址", 120);
            AddLogColumn("LogId", "日志ID", 80, null, true, DataGridViewContentAlignment.MiddleCenter);
            AddLogColumn("UserId", "用户ID", 0, null, false);

            if (dgvLogs.Columns.Contains("colDescription"))
            {
                dgvLogs.Columns["colDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void AddLogColumn(string dataPropertyName, string headerText, int width, string format = null, bool isVisible = true, DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = dataPropertyName;
            column.HeaderText = headerText;
            column.Name = "col" + dataPropertyName;
            column.Width = width;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.DefaultCellStyle.Alignment = align;
            if (!string.IsNullOrEmpty(format))
            {
                column.DefaultCellStyle.Format = format;
            }
            column.Visible = isVisible;
            dgvLogs.Columns.Add(column);
        }

        private void LoadFilterOptions()
        {
            try
            {
                var users = logBll.GetDistinctLogUsers();
                var userList = users.Select(kvp => new { Display = kvp.Value, Value = kvp.Key }).ToList();
                userList.Insert(0, new { Display = "[所有用户]", Value = 0 });
                cmbUser.DataSource = userList;
                cmbUser.DisplayMember = "Display";
                cmbUser.ValueMember = "Value";
                cmbUser.SelectedIndex = 0;

                var modules = logBll.GetDistinctLogModules();
                modules.Insert(0, "[所有模块]");
                cmbModule.DataSource = modules;
                cmbModule.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载过滤选项失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                DateTime? startDate = dtpStartDate.Checked ? dtpStartDate.Value : (DateTime?)null;
                DateTime? endDate = dtpEndDate.Checked ? dtpEndDate.Value : (DateTime?)null;
                int? userId = (cmbUser.SelectedValue != null && (int)cmbUser.SelectedValue > 0) ? (int?)cmbUser.SelectedValue : null;
                string operationType = txtOperationType.Text.Trim();
                string operationModule = (cmbModule.SelectedIndex > 0) ? cmbModule.SelectedItem.ToString() : null;

                Console.WriteLine($"查询参数 - 开始日期: {(startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : "无")}，" +
                                 $"结束日期: {(endDate.HasValue ? endDate.Value.ToString("yyyy-MM-dd") : "无")}，" +
                                 $"用户ID: {(userId.HasValue ? userId.Value.ToString() : "所有用户")}，" +
                                 $"操作类型: {(string.IsNullOrEmpty(operationType) ? "所有" : operationType)}，" +
                                 $"操作模块: {(string.IsNullOrEmpty(operationModule) ? "所有" : operationModule)}");

                DataTable logData = logBll.GetSystemLogs(startDate, endDate, userId, operationType, operationModule);
                
                Console.WriteLine($"查询返回记录数: {logData.Rows.Count}");
                
                dgvLogs.DataSource = logData;
                
                // 如果没有数据，显示提示信息
                if (logData.Rows.Count == 0)
                {
                    MessageBox.Show("未查询到符合条件的日志记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载操作日志失败: {ex.Message}");
                MessageBox.Show("加载操作日志失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
} 