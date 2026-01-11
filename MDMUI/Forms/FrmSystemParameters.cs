using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;
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
        private AppButton btnSave;
        private AppButton btnReload;
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
                this.userLabel.Text = $"当前用户: {CurrentUser?.Username ?? "未知用户"}";
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
            demoPanel.BackColor = ThemeManager.Palette.Background;
            demoPanel.BorderStyle = BorderStyle.None;

            CardPanel contentCard = new CardPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = ThemeManager.Palette.Surface
            };

            ActionToolbar toolbar = BuildToolbar();

            parameterGrid = BuildGrid();

            statusLabel = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Palette.TextSecondary
            };

            contentCard.Controls.Add(parameterGrid);
            contentCard.Controls.Add(statusLabel);
            contentCard.Controls.Add(toolbar);
            demoPanel.Controls.Add(contentCard);
        }

        private ActionToolbar BuildToolbar()
        {
            ActionToolbar toolbar = new ActionToolbar
            {
                Dock = DockStyle.Top,
                Height = 46,
                Padding = new Padding(0, 6, 0, 10)
            };

            Label filterLabel = new Label
            {
                AutoSize = true,
                Text = "筛选：",
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = ThemeManager.Palette.TextSecondary,
                Padding = new Padding(0, 6, 0, 0),
                Margin = new Padding(0)
            };

            filterBox = new TextBox
            {
                Width = 260,
                Height = 28,
                Margin = new Padding(8, 0, 0, 0)
            };
            filterBox.TextChanged += (s, e) => ApplyFilter();
            filterBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    filterBox.Text = string.Empty;
                    e.Handled = true;
                }
            };

            btnReload = new AppButton
            {
                Text = "刷新",
                Variant = AppButtonVariant.Secondary,
                MinimumSize = new Size(90, 32)
            };
            btnReload.Click += (s, e) => LoadParameters();

            btnSave = new AppButton
            {
                Text = "保存",
                Variant = AppButtonVariant.Primary,
                MinimumSize = new Size(90, 32)
            };
            btnSave.Click += (s, e) => SaveParameters();

            toolbar.LeftPanel.Controls.Add(filterLabel);
            toolbar.LeftPanel.Controls.Add(filterBox);
            toolbar.RightPanel.Controls.Add(btnReload);
            toolbar.RightPanel.Controls.Add(btnSave);

            return toolbar;
        }

        private DataGridView BuildGrid()
        {
            DataGridView grid = new DataGridView
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

            grid.Columns.Add(CreateColumn("ParamKey", "参数键", 220, true));
            grid.Columns.Add(CreateColumn("ParamValue", "参数值", 240, false));
            grid.Columns.Add(CreateColumn("Description", "说明", 280, true));
            grid.Columns.Add(CreateColumn("UpdatedAt", "更新时间", 160, true));

            grid.RowPrePaint += ParameterGrid_RowPrePaint;
            grid.CellEndEdit += (s, e) => UpdateStatusSummary();
            grid.DataBindingComplete += (s, e) => UpdateStatusSummary();

            GridStyler.Apply(grid);

            return grid;
        }

        private void LoadParameters()
        {
            try
            {
                using (AppTelemetry.Measure("SystemParameters.Load"))
                {
                    allParameters = parameterService.GetAllParameters().ToList();
                    originalValues.Clear();
                    foreach (SystemParameter parameter in allParameters)
                    {
                        if (parameter == null || string.IsNullOrWhiteSpace(parameter.ParamKey)) continue;
                        originalValues[parameter.ParamKey] = parameter.ParamValue ?? string.Empty;
                    }

                    ApplyFilter();
                    UpdateStatusSummary("已加载");
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "加载系统参数失败");
                UpdateStatus("加载失败: " + ex.Message);
            }
        }

        private void ApplyFilter()
        {
            if (parameterGrid == null) return;
            string keyword = filterBox?.Text?.Trim();

            IEnumerable<SystemParameter> data = allParameters ?? new List<SystemParameter>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                data = data.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.ParamKey) && p.ParamKey.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrWhiteSpace(p.Description) && p.Description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            parameterGrid.DataSource = data.ToList();
            UpdateStatusSummary();
        }

        private void SaveParameters()
        {
            if (parameterGrid == null) return;

            int updated = 0;
            try
            {
                using (AppTelemetry.Measure("SystemParameters.Save"))
                {
                    foreach (SystemParameter parameter in allParameters ?? new List<SystemParameter>())
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
                }

                LoadParameters();
                UpdateStatusSummary(updated > 0 ? $"已保存 {updated} 项" : "无变更需要保存");
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "保存系统参数失败");
                UpdateStatus("保存失败: " + ex.Message);
            }
        }

        private void UpdateStatus(string message)
        {
            if (statusLabel == null) return;
            statusLabel.Text = message ?? string.Empty;
        }

        private void UpdateStatusSummary(string prefix = null)
        {
            int total = allParameters?.Count ?? 0;
            int filtered = (parameterGrid?.DataSource as IList<SystemParameter>)?.Count ?? total;
            int dirty = allParameters?.Count(IsDirty) ?? 0;

            string message = $"总计 {total} 项";
            if (filtered != total) message += $"，筛选后 {filtered} 项";
            if (dirty > 0) message += $"，未保存 {dirty} 项";
            if (!string.IsNullOrWhiteSpace(prefix)) message = prefix + " · " + message;

            UpdateStatus(message);
        }

        private bool IsDirty(SystemParameter parameter)
        {
            if (parameter == null) return false;
            if (string.IsNullOrWhiteSpace(parameter.ParamKey)) return false;

            string current = parameter.ParamValue ?? string.Empty;
            string original = originalValues.ContainsKey(parameter.ParamKey) ? originalValues[parameter.ParamKey] : string.Empty;
            return !string.Equals(current, original, StringComparison.Ordinal);
        }

        private void ParameterGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (parameterGrid == null) return;
            if (e.RowIndex < 0 || e.RowIndex >= parameterGrid.Rows.Count) return;

            DataGridViewRow row = parameterGrid.Rows[e.RowIndex];
            if (!(row?.DataBoundItem is SystemParameter parameter)) return;

            ThemePalette palette = ThemeManager.Palette;
            Color baseColor = (e.RowIndex % 2 == 1) ? palette.SurfaceAlt : palette.Surface;
            Color dirtyColor = Color.FromArgb(255, 252, 240);

            row.DefaultCellStyle.BackColor = IsDirty(parameter) ? dirtyColor : baseColor;
            row.DefaultCellStyle.ForeColor = palette.TextPrimary;
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
