using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;
using SystemParameter = MDMUI.Model.SystemParameter;

namespace MDMUI
{
    // Note: This class is now partial to allow separation of Designer code
    public partial class FrmSystemParameters : Form
    {
        // Keep CurrentUser as a field accessible by the form logic
        private User CurrentUser;
        private readonly SystemParameterService parameterService = new SystemParameterService();
        private DataGridView parameterGrid;
        private TextBox filterBox;
        private Button btnSave;
        private Button btnReload;
        private Label statusLabel;
        private readonly Dictionary<string, string> originalValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private List<SystemParameter> allParameters = new List<SystemParameter>();

        public FrmSystemParameters(User user)
        {
            InitializeComponent();
            CurrentUser = user;
        }

        private void FrmSystemParameters_Load(object sender, EventArgs e)
        {
            if (this.userLabel != null)
            {
                this.userLabel.Text = $"当前用户: {CurrentUser?.Username ?? "未 知用户"}";
            }

            InitializeParameterPanel();
            LoadParameters();
            ThemeManager.ApplyTo(this);
            ModernTheme.EnableMicroInteractions(this);
        }

        private void InitializeParameterPanel()
        {
            if (demoPanel == null) return;

            titleLabel.Text = "系统参数中心";
            placeholderLabel.Visible = false;
            demoLabel.Visible = false;

            demoPanel.Controls.Clear();
            demoPanel.Dock = DockStyle.Fill;
            demoPanel.Padding = new Padding(16);
            demoPanel.BackColor = ThemeManager.Palette.Surface;

            Panel toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                Padding = new Padding(0, 0, 0, 8)
            };

            filterBox = new TextBox
            {
                Width = 220,
                Height = 28,
                Location = new Point(0, 8)
            };
            filterBox.TextChanged += (s, e) => ApplyFilter();

            btnReload = new Button
            {
                Text = "刷新",
                Width = 80,
                Height = 30,
                Location = new Point(filterBox.Right + 12, 6)
            };
            btnReload.Click += (s, e) => LoadParameters();

            btnSave = new Button
            {
                Text = "保存",
                Width = 80,
                Height = 30,
                Location = new Point(btnReload.Right + 10, 6)
            };
            btnSave.Click += (s, e) => SaveParameters();

            toolbar.Controls.Add(filterBox);
            toolbar.Controls.Add(btnReload);
            toolbar.Controls.Add(btnSave);

            parameterGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
            };

            parameterGrid.Columns.Add(CreateColumn("ParamKey", "参数键", 220, true));
            parameterGrid.Columns.Add(CreateColumn("ParamValue", "参数值", 240, false));
            parameterGrid.Columns.Add(CreateColumn("Description", "说明", 280, true));
            parameterGrid.Columns.Add(CreateColumn("UpdatedAt", "更新时间", 160, true));

            statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Palette.TextSecondary
            };

            demoPanel.Controls.Add(parameterGrid);
            demoPanel.Controls.Add(statusLabel);
            demoPanel.Controls.Add(toolbar);
        }

        private void LoadParameters()
        {
            try
            {
                allParameters = parameterService.GetAllParameters().ToList();
                originalValues.Clear();
                foreach (SystemParameter parameter in allParameters)
                {
                    if (parameter == null || string.IsNullOrWhiteSpace(parameter.ParamKey)) continue;
                    originalValues[parameter.ParamKey] = parameter.ParamValue ?? string.Empty;
                }

                ApplyFilter();
                UpdateStatus($"已加载 {allParameters.Count} 项参数");
            }
            catch (Exception ex)
            {
                UpdateStatus("加载失败: " + ex.Message);
            }
        }

        private void ApplyFilter()
        {
            if (parameterGrid == null) return;
            string keyword = filterBox?.Text?.Trim();

            IEnumerable<SystemParameter> data = allParameters;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                data = data.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.ParamKey) && p.ParamKey.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrWhiteSpace(p.Description) && p.Description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            parameterGrid.DataSource = data.ToList();
        }

        private void SaveParameters()
        {
            if (parameterGrid == null) return;

            int updated = 0;
            foreach (SystemParameter parameter in parameterGrid.DataSource as List<SystemParameter> ?? new List<SystemParameter>())
            {
                if (parameter == null || string.IsNullOrWhiteSpace(parameter.ParamKey)) continue;
                string value = parameter.ParamValue ?? string.Empty;
                string original = originalValues.ContainsKey(parameter.ParamKey) ? originalValues[parameter.ParamKey] : string.Empty;
                if (string.Equals(value, original, StringComparison.Ordinal)) continue;

                parameterService.SetValue(parameter.ParamKey, value, parameter.Description);
                originalValues[parameter.ParamKey] = value;
                updated++;
            }

            if (updated > 0)
            {
                AuditTrail.Log(CurrentUser, "Update", "SystemParameters", $"更新系统参数 {updated} 项");
            }

            LoadParameters();
            UpdateStatus(updated > 0 ? $"已保存 {updated} 项" : "无变更需要保存");
        }

        private void UpdateStatus(string message)
        {
            if (statusLabel == null) return;
            statusLabel.Text = message ?? string.Empty;
        }

        private static DataGridViewTextBoxColumn CreateColumn(string dataProperty, string header, int width, bool readOnly)
        {
            return new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataProperty,
                HeaderText = header,
                Width = width,
                ReadOnly = readOnly
            };
        }
    }
}
