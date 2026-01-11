using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Controls.Atoms;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI
{
    public partial class FrmOperationLog : Form
    {
        private readonly SystemLogBLL logBll;
        private User CurrentUser;

        private Label statusLabel;
        private Label emptyStateLabel;

        public FrmOperationLog(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            logBll = new SystemLogBLL();
        }

        private void FrmOperationLog_Load(object sender, EventArgs e)
        {
            InitializeModernLayout();
            HookInteractions();

            dtpStartDate.Value = DateTime.Now.Date.AddDays(-30);
            dtpStartDate.Checked = true;
            dtpEndDate.Value = DateTime.Now.Date;
            dtpEndDate.Checked = true;

            ConfigureDataGridView();
            LoadFilterOptions();
            LoadData();

            ThemeManager.ApplyTo(this);
            ModernTheme.EnableMicroInteractions(this);
        }

        private void InitializeModernLayout()
        {
            if (mainPanel == null || filterPanel == null || dgvLogs == null || titleLabel == null) return;

            ThemePalette palette = ThemeManager.Palette;

            mainPanel.SuspendLayout();
            try
            {
                mainPanel.BackColor = palette.Background;
                mainPanel.Padding = new Padding(16);

                titleLabel.Text = "操作日志查询";
                titleLabel.Dock = DockStyle.Top;
                titleLabel.Padding = new Padding(4, 0, 0, 10);
                titleLabel.ForeColor = palette.TextPrimary;

                filterPanel.Dock = DockStyle.Top;
                filterPanel.Padding = new Padding(0, 0, 0, 10);
                filterPanel.BackColor = palette.Surface;

                CardPanel card = new CardPanel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(12),
                    BackColor = palette.Surface
                };

                Panel gridHost = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = palette.Surface
                };

                dgvLogs.Dock = DockStyle.Fill;
                gridHost.Controls.Add(dgvLogs);

                emptyStateLabel = new Label
                {
                    Dock = DockStyle.Fill,
                    Text = "未查询到符合条件的日志记录",
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = palette.TextSecondary,
                    BackColor = palette.Surface,
                    Visible = false
                };
                gridHost.Controls.Add(emptyStateLabel);
                emptyStateLabel.BringToFront();

                statusLabel = new Label
                {
                    Dock = DockStyle.Bottom,
                    Height = 24,
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = palette.TextSecondary
                };

                mainPanel.Controls.Clear();
                card.Controls.Add(gridHost);
                card.Controls.Add(statusLabel);
                card.Controls.Add(filterPanel);
                mainPanel.Controls.Add(card);
                mainPanel.Controls.Add(titleLabel);
            }
            finally
            {
                mainPanel.ResumeLayout(performLayout: true);
            }
        }

        private void HookInteractions()
        {
            if (btnSearch != null)
            {
                btnSearch.Click -= btnSearch_Click;
                btnSearch.Click += btnSearch_Click;
            }

            if (txtOperationType != null)
            {
                txtOperationType.KeyDown -= TxtOperationType_KeyDown;
                txtOperationType.KeyDown += TxtOperationType_KeyDown;
            }
        }

        private void TxtOperationType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                LoadData();
            }
        }

        private void ConfigureDataGridView()
        {
            if (dgvLogs == null) return;

            dgvLogs.AutoGenerateColumns = false;
            dgvLogs.Columns.Clear();
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.MultiSelect = false;
            dgvLogs.AllowUserToAddRows = false;
            dgvLogs.AllowUserToDeleteRows = false;
            dgvLogs.AllowUserToResizeRows = false;
            dgvLogs.ReadOnly = true;
            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvLogs.RowHeadersVisible = false;
            dgvLogs.EnableHeadersVisualStyles = false;

            AddLogColumn("LogTime", "时间", 170, "yyyy-MM-dd HH:mm:ss", true, DataGridViewContentAlignment.MiddleCenter);
            AddLogColumn("UserName", "操作用户", 110, null, true, DataGridViewContentAlignment.MiddleCenter);
            AddLogColumn("OperationModule", "操作模块", 140);
            AddLogColumn("OperationType", "操作类型", 120);
            AddLogColumn("Description", "详细描述", 320);
            AddLogColumn("IPAddress", "IP地址", 140);
            AddLogColumn("LogId", "日志ID", 90, null, true, DataGridViewContentAlignment.MiddleCenter);
            AddLogColumn("UserId", "用户ID", 0, null, false);

            if (dgvLogs.Columns.Contains("colDescription"))
            {
                dgvLogs.Columns["colDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            GridStyler.Apply(dgvLogs);
        }

        private void AddLogColumn(
            string dataPropertyName,
            string headerText,
            int width,
            string format = null,
            bool isVisible = true,
            DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataPropertyName,
                HeaderText = headerText,
                Name = "col" + dataPropertyName,
                Width = width,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Visible = isVisible
            };

            column.DefaultCellStyle.Alignment = align;
            if (!string.IsNullOrEmpty(format))
            {
                column.DefaultCellStyle.Format = format;
            }

            dgvLogs.Columns.Add(column);
        }

        private void LoadFilterOptions()
        {
            try
            {
                using (AppTelemetry.Measure("OperationLog.LoadFilters"))
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
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "加载操作日志过滤选项失败");
                UpdateStatus("加载过滤选项失败: " + ex.Message);
                MessageBox.Show("加载过滤选项失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            if (dgvLogs == null) return;

            try
            {
                using (AppTelemetry.Measure("OperationLog.Query"))
                {
                    DateTime? startDate = dtpStartDate.Checked ? dtpStartDate.Value : (DateTime?)null;
                    DateTime? endDate = dtpEndDate.Checked ? dtpEndDate.Value : (DateTime?)null;

                    int? userId = null;
                    if (cmbUser.SelectedValue != null && int.TryParse(cmbUser.SelectedValue.ToString(), out int parsedUserId) && parsedUserId > 0)
                    {
                        userId = parsedUserId;
                    }

                    string operationType = txtOperationType?.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(operationType)) operationType = null;

                    string operationModule = null;
                    if (cmbModule.SelectedIndex > 0 && cmbModule.SelectedItem != null)
                    {
                        operationModule = cmbModule.SelectedItem.ToString();
                    }

                    DataTable logData = logBll.GetSystemLogs(startDate, endDate, userId, operationType, operationModule);
                    dgvLogs.DataSource = logData;

                    int count = logData?.Rows?.Count ?? 0;
                    SetEmptyStateVisible(count == 0);
                    UpdateStatus($"共 {count} 条记录");
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "加载操作日志失败");
                SetEmptyStateVisible(true);
                UpdateStatus("加载失败: " + ex.Message);
                MessageBox.Show("加载操作日志失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEmptyStateVisible(bool visible)
        {
            if (emptyStateLabel == null) return;
            emptyStateLabel.Visible = visible;
        }

        private void UpdateStatus(string message)
        {
            if (statusLabel == null) return;
            statusLabel.Text = message ?? string.Empty;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

