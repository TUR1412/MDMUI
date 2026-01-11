using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.Model;
using MDMUI.Utility;
using MDMUI.BLL;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;

namespace MDMUI
{
    /// <summary>
    /// 部门管理窗体
    /// </summary>
    public partial class FrmDepartment : Form
    {
        private User currentUser;
        private List<Department> currentDepartments;
        private PermissionChecker permissionChecker;
        private DepartmentService departmentService;

        // Modern UI (Atomic Design) - runtime enhancement (OCP: do not rewrite Designer)
        private bool modernLayoutInitialized;
        private CardPanel modernHeaderCard;
        private Panel gridHostPanel;
        private Label emptyStateLabel;
        private ComboBox modernFactoryComboBox;
        private TextBox modernSearchTextBox;
        private AppButton modernAddButton;
        private AppButton modernEditButton;
        private AppButton modernDeleteButton;
        private AppButton modernRefreshButton;
        private AppButton modernSearchButton;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmDepartment(User user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.permissionChecker = new PermissionChecker();
            this.departmentService = new DepartmentService();
            this.Load += FrmDepartment_Load;

            InitializeModernLayout();
        }

        private void InitializeModernLayout()
        {
            if (modernLayoutInitialized) return;
            modernLayoutInitialized = true;

            try
            {
                using (AppTelemetry.Measure("FrmDepartment.ModernLayout"))
                {
                    EnsureModernHeader();
                    EnsureGridHostAndEmptyState();
                    EnsureStatusStrip();

                    try { GridStyler.Apply(dataGridViewDepartments); } catch { }
                    TryEnableGridDoubleBuffering(dataGridViewDepartments);

                    // UiThemingBootstrapper 只对窗体执行一次；这里确保运行时新 
                    // 增控件也能吃到统一风格
                    try { ThemeManager.ApplyTo(this); } catch { }
                    try { ModernTheme.EnableMicroInteractions(this); } catch { }

                    UpdateDepartmentListIndicators(GetSearchTerm(), dataGridViewDepartments?.Rows?.Count ?? 0);
                }
            }
            catch (Exception ex)
            {
                try { AppLog.Error(ex, "初始化部门管理现代化布局失败"); } catch { }
            }
        }

        private void EnsureModernHeader()
        {
            if (modernHeaderCard != null) return;

            ToolStrip oldToolStrip = toolStrip1;

            ToolStripButton oldAdd = toolStripButtonAdd;
            ToolStripButton oldEdit = toolStripButtonEdit;
            ToolStripButton oldDelete = toolStripButtonDelete;
            ToolStripButton oldRefresh = toolStripButtonRefresh;
            ToolStripButton oldSearch = toolStripButtonSearch;

            CardPanel header = new CardPanel
            {
                Dock = DockStyle.Top,
                Padding = new Padding(12),
                Height = 132
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
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
            header.Controls.Add(layout);

            ActionToolbar actionsToolbar = new ActionToolbar { Dock = DockStyle.Fill };
            ActionToolbar searchToolbar = new ActionToolbar { Dock = DockStyle.Fill };

            Label title = new Label
            {
                AutoSize = true,
                Text = string.IsNullOrWhiteSpace(Text) ? "部门管理" : Text,
                ForeColor = ThemeManager.Palette.TextPrimary,
                Margin = new Padding(0, 10, 12, 0)
            };
            try { title.Font = ThemeManager.CreateTitleFont(11f); } catch { }
            actionsToolbar.LeftPanel.Controls.Add(title);

            modernAddButton = CreateAppButton(oldAdd?.Text ?? "新增", AppButtonVariant.Primary, BtnAdd_Click, name: "btnAdd");
            modernEditButton = CreateAppButton(oldEdit?.Text ?? "编辑", AppButtonVariant.Secondary, BtnEdit_Click, name: "btnEdit");
            modernDeleteButton = CreateAppButton(oldDelete?.Text ?? "删除", AppButtonVariant.Danger, BtnDelete_Click, name: "btnDelete");
            modernRefreshButton = CreateAppButton(oldRefresh?.Text ?? "刷新", AppButtonVariant.Secondary, BtnRefresh_Click, name: "btnRefresh");

            CopyToolStripState(oldAdd, modernAddButton);
            CopyToolStripState(oldEdit, modernEditButton);
            CopyToolStripState(oldDelete, modernDeleteButton);
            CopyToolStripState(oldRefresh, modernRefreshButton);

            actionsToolbar.RightPanel.Controls.Add(modernAddButton);
            actionsToolbar.RightPanel.Controls.Add(modernEditButton);
            actionsToolbar.RightPanel.Controls.Add(modernDeleteButton);
            actionsToolbar.RightPanel.Controls.Add(modernRefreshButton);

            Label factoryLabel = CreateFieldLabel("工厂");
            modernFactoryComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 160
            };
            modernFactoryComboBox.SelectedIndexChanged += CboFactory_SelectedIndexChanged;

            Label searchLabel = CreateFieldLabel("部门名称");
            modernSearchTextBox = new TextBox
            {
                Width = 180
            };
            modernSearchTextBox.KeyDown += (s, e) =>
            {
                try
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        btnSearch_Click(this, EventArgs.Empty);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                }
                catch { }
            };

            modernSearchButton = CreateAppButton(oldSearch?.Text ?? "查询", AppButtonVariant.Primary, btnSearch_Click, name: "btnSearch");
            CopyToolStripState(oldSearch, modernSearchButton);

            searchToolbar.LeftPanel.Controls.Add(factoryLabel);
            searchToolbar.LeftPanel.Controls.Add(modernFactoryComboBox);
            searchToolbar.LeftPanel.Controls.Add(searchLabel);
            searchToolbar.LeftPanel.Controls.Add(modernSearchTextBox);
            searchToolbar.RightPanel.Controls.Add(modernSearchButton);

            try { AcceptButton = modernSearchButton; } catch { }

            layout.Controls.Add(actionsToolbar, 0, 0);
            layout.Controls.Add(searchToolbar, 0, 1);

            Controls.Add(header);
            header.BringToFront();
            modernHeaderCard = header;

            // 旧 ToolStrip 移除并释放（开闭原则：运行时替换，不动 Designer）
            try { if (oldToolStrip != null) Controls.Remove(oldToolStrip); } catch { }
            try { oldToolStrip?.Dispose(); } catch { }

            toolStrip1 = null;
            toolStripButtonAdd = null;
            toolStripButtonEdit = null;
            toolStripButtonDelete = null;
            toolStripButtonRefresh = null;
            toolStripLabelFactory = null;
            toolStripComboBoxFactory = null;
            toolStripLabelSearch = null;
            toolStripTextBoxSearchName = null;
            toolStripButtonSearch = null;
        }

        private void EnsureGridHostAndEmptyState()
        {
            if (gridHostPanel != null) return;
            if (dataGridViewDepartments == null) return;

            int gridIndex = 0;
            try { gridIndex = Controls.GetChildIndex(dataGridViewDepartments); } catch { }

            try { Controls.Remove(dataGridViewDepartments); } catch { }

            Panel host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            dataGridViewDepartments.Dock = DockStyle.Fill;
            dataGridViewDepartments.BorderStyle = BorderStyle.None;
            host.Controls.Add(dataGridViewDepartments);

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

        private static void CopyToolStripState(ToolStripItem source, Control target)
        {
            if (source == null || target == null) return;

            try { target.Visible = source.Visible; } catch { }
            try { target.Enabled = source.Enabled; } catch { }
        }

        private ComboBox GetFactoryComboBox()
        {
            if (modernFactoryComboBox != null) return modernFactoryComboBox;
            try { return toolStripComboBoxFactory?.ComboBox; } catch { return null; }
        }

        private string GetSearchTerm()
        {
            try
            {
                if (modernSearchTextBox != null) return modernSearchTextBox.Text?.Trim() ?? "";
                if (toolStripTextBoxSearchName != null) return toolStripTextBoxSearchName.Text?.Trim() ?? "";
            }
            catch { }

            return "";
        }

        private ComboboxItem GetSelectedFactoryItem()
        {
            ComboBox combo = GetFactoryComboBox();
            if (combo == null) return null;
            return combo.SelectedItem as ComboboxItem;
        }

        private void UpdateDepartmentListIndicators(string searchTerm, int count)
        {
            string normalized = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim();

            ComboboxItem selectedFactoryItem = GetSelectedFactoryItem();
            string factoryText = selectedFactoryItem?.Text;
            bool hasFactory = !string.IsNullOrWhiteSpace(factoryText) && factoryText != "所有工厂";

            if (emptyStateLabel != null)
            {
                if (count <= 0)
                {
                    emptyStateLabel.Text = string.IsNullOrWhiteSpace(normalized) ? "暂无部门数据" : "未找到匹配的部门";
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
                UpdateStatus(string.IsNullOrWhiteSpace(normalized) ? "暂无部门数据" : $"未找到匹配部门：{normalized}");
            }
            else
            {
                UpdateStatus(hasFactory ? $"工厂[{factoryText}] 共 {count} 条部门" : $"共 {count} 条部门");
            }
        }

        private void FrmDepartment_Load(object sender, EventArgs e)
        {
            try
            {
                LoadFactories();
                LoadDepartments();
                SetButtonPermissions();
                ConfigureDataGridView();
                UpdateStatus("窗体加载完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show("窗体加载失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("窗体加载失败");
            }
        }

        /// <summary>
        /// 根据用户权限设置按钮可用性
        /// </summary>
        private void SetButtonPermissions()
        {
            bool isAdmin = currentUser != null && currentUser.RoleName == "超级管理员";

            ToolStripItem btnAdd = toolStripButtonAdd;
            ToolStripItem btnEdit = toolStripButtonEdit;
            ToolStripItem btnDelete = toolStripButtonDelete;

            bool hasAnyButtonTarget =
                btnAdd != null ||
                btnEdit != null ||
                btnDelete != null ||
                modernAddButton != null ||
                modernEditButton != null ||
                modernDeleteButton != null;

            if (!hasAnyButtonTarget)
            {
                try { AppLog.Warn("警告: FrmDepartment 工具栏按钮未全部找到。"); } catch { }
            }

            bool canAdd = isAdmin || permissionChecker.HasPermission(currentUser.Id, "department", "add");
            bool canEdit = isAdmin || permissionChecker.HasPermission(currentUser.Id, "department", "edit");
            bool canDelete = isAdmin || permissionChecker.HasPermission(currentUser.Id, "department", "delete");

            if (btnAdd != null) btnAdd.Enabled = canAdd;
            if (btnEdit != null) btnEdit.Enabled = canEdit;
            if (btnDelete != null) btnDelete.Enabled = canDelete;

            if (modernAddButton != null) modernAddButton.Enabled = canAdd;
            if (modernEditButton != null) modernEditButton.Enabled = canEdit;
            if (modernDeleteButton != null) modernDeleteButton.Enabled = canDelete;
        }

        /// <summary>
        /// 加载工厂数据到下拉列表
        /// </summary>
        private void LoadFactories()
        {
            try
            {
                List<ComboboxItem> factoryItems = departmentService.GetFactoriesForComboBox(currentUser);

                ComboBox combo = GetFactoryComboBox();
                if (combo == null) return;

                combo.DataSource = null;
                combo.Items.Clear();

                if (currentUser.RoleName == "超级管理员")
                {
                    combo.Items.Add(new ComboboxItem("所有工厂", ""));
                }

                combo.Items.AddRange(factoryItems.ToArray());
                combo.DisplayMember = "Text";
                combo.ValueMember = "Value";

                if (currentUser.RoleName != "超级管理员" && !string.IsNullOrEmpty(currentUser.FactoryId))
                {
                    SelectComboBoxItemByValue(combo, currentUser.FactoryId);
                    combo.Enabled = false;
                }
                else
                {
                    combo.Enabled = true;
                    if (combo.Items.Count > 0) combo.SelectedIndex = 0;
                }
                combo.Width = 160;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载工厂列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 辅助方法：根据值选中 ComboBox 项
        /// </summary>
        private void SelectComboBoxItemByValue(ComboBox comboBox, object value)
        {
            if (value == null) return;
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i] is ComboboxItem item && item.Value != null && item.Value.Equals(value))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
                else if (comboBox.Items[i] is DataRowView rowView && rowView.Row[comboBox.ValueMember].Equals(value))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
            if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0;
            else comboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// 加载部门数据
        /// </summary>
        private void LoadDepartments()
        {
            try
            {
                using (AppTelemetry.Measure("department.load_list"))
                {
                    ComboBox factoryCombo = GetFactoryComboBox();
                    string searchTerm = GetSearchTerm();
                    ComboboxItem selectedFactoryItem = GetSelectedFactoryItem();
                    string selectedFactoryId = selectedFactoryItem?.Value?.ToString();

                    if (factoryCombo == null || (factoryCombo.SelectedItem == null && currentUser.RoleName != "超级管理员"))
                    {
                        this.dataGridViewDepartments.DataSource = null;
                        UpdateStatus("请先确保工厂列表已加载");
                        UpdateDepartmentListIndicators(searchTerm, 0);
                        return;
                    }

                    if (currentUser.RoleName != "超级管理员" && string.IsNullOrEmpty(selectedFactoryId))
                    {
                        if (selectedFactoryItem != null && (selectedFactoryItem.Value?.ToString() ?? "") == "")
                        {
                            this.dataGridViewDepartments.DataSource = null;
                            UpdateStatus("请选择一个具体的工厂进行查看");
                            UpdateDepartmentListIndicators(searchTerm, 0);
                            return;
                        }

                        if (string.IsNullOrEmpty(currentUser.FactoryId))
                        {
                            this.dataGridViewDepartments.DataSource = null;
                            UpdateStatus("当前用户未关联工厂，无法加载部门");
                            UpdateDepartmentListIndicators(searchTerm, 0);
                            return;
                        }
                    }

                    if (string.IsNullOrEmpty(selectedFactoryId))
                    {
                        currentDepartments = departmentService.GetAllDepartments();
                    }
                    else
                    {
                        currentDepartments = departmentService.GetDepartmentsByFactoryId(selectedFactoryId);
                    }

                    List<Department> filteredDepartments = currentDepartments;
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        filteredDepartments = currentDepartments
                            .Where(d => d.DeptName != null && d.DeptName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                            .ToList();
                    }

                    var bindingList = new BindingList<Department>(filteredDepartments);
                    var source = new BindingSource(bindingList, null);
                    this.dataGridViewDepartments.DataSource = source;

                    UpdateDepartmentListIndicators(searchTerm, filteredDepartments?.Count ?? 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载部门数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { AppLog.Error(ex, "加载部门数据失败"); } catch { }
                UpdateStatus("加载部门数据失败");
                this.dataGridViewDepartments.DataSource = null;
                currentDepartments = new List<Department>();
                UpdateDepartmentListIndicators(GetSearchTerm(), 0);
            }
        }

        /// <summary>
        /// 配置 DataGridView (最好在 Load 事件或设计器中完成一次性设置)
        /// </summary>
        private void ConfigureDataGridView()
        {
            TryEnableGridDoubleBuffering(dataGridViewDepartments);
            try { GridStyler.Apply(dataGridViewDepartments); } catch { }
        }

        private void UpdateStatus(string message)
        {
            if (this.toolStripStatusLabelStatus != null)
            {
                this.toolStripStatusLabelStatus.Text = message;
            }
        }

        private bool TryGetSelectedDepartment(out Department selected)
        {
            selected = null;

            try
            {
                if (dataGridViewDepartments?.SelectedRows == null || dataGridViewDepartments.SelectedRows.Count <= 0)
                {
                    return false;
                }

                DataGridViewRow row = dataGridViewDepartments.SelectedRows[0];
                if (row?.DataBoundItem is Department dept)
                {
                    selected = dept;
                    return true;
                }
            }
            catch
            {
                // ignore
            }

            return false;
        }

        /// <summary>
        /// 添加按钮点击事件 (调用 Service)
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // 权限已在 SetButtonPermissions 中检查和禁用按钮
            // string selectedFactoryId = this.toolStripComboBoxFactory.ComboBox.SelectedValue?.ToString(); // Original problematic line

            // Safer way to get FactoryId
            ComboboxItem selectedFactoryItem = GetSelectedFactoryItem();
            string selectedFactoryId = selectedFactoryItem?.Value?.ToString();

            if (string.IsNullOrEmpty(selectedFactoryId))
            {
                MessageBox.Show("请先选择一个具体的工厂。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); // Updated message
                return;
            }

            // 使用 using 确保 FrmDepartmentEdit 被正确释放
            using (FrmDepartmentEdit frmEdit = new FrmDepartmentEdit(null, selectedFactoryId, currentUser))
            {
                if (frmEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadDepartments();
                }
            }
        }

        /// <summary>
        /// 编辑按钮点击事件
        /// </summary>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            EditSelectedDepartment();
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // 权限已在 SetButtonPermissions 中检查和禁用按钮
            if (this.dataGridViewDepartments.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要删除的部门。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!TryGetSelectedDepartment(out Department selectedDept) || string.IsNullOrWhiteSpace(selectedDept?.DeptId))
            {
                MessageBox.Show("无法获取选定部门的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string deptId = selectedDept.DeptId;
            string deptName = string.IsNullOrWhiteSpace(selectedDept.DeptName) ? deptId : selectedDept.DeptName;

            if (MessageBox.Show($"确定要删除部门 [{deptName}] 及其所有子部门吗？\n此操作不可恢复！", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                UpdateStatus($"正在删除部门 [{deptName}]...");
                try
                {
                    // Call the service layer to delete the department
                    bool success = departmentService.DeleteDepartment(deptId, currentUser);

                    if (success)
                    {
                        MessageBox.Show("部门删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDepartments(); // Refresh the list
                        UpdateStatus($"部门 [{deptName}] 已删除");
                    }
                    // If the service returns false, it means a business rule prevented deletion (handled by the service throwing an exception now)
                }
                catch (InvalidOperationException opEx) // Catch specific business logic errors from the service
                {
                    MessageBox.Show(opEx.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateStatus($"删除部门 [{deptName}] 失败");
                }
                catch (Exception ex) // Catch other potential errors (database connection, etc.)
                {
                    MessageBox.Show("删除部门时发生错误: \n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus($"删除部门 [{deptName}] 时出错");
                }
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            UpdateStatus("正在刷新部门列表...");
            LoadDepartments();
        }

        /// <summary>
        /// 查询按钮点击事件
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            UpdateStatus("正在按条件查询部门...");
            LoadDepartments(); // 查询逻辑已包含在 LoadDepartments 中，它会读取搜索框内容
        }

        /// <summary>
        /// 工厂下拉列表选择变化事件
        /// </summary>
        private void CboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 仅当用户手动更改选择时才重新加载，避免 Load 事件触发时不必要的加载
            ComboBox combo = sender as ComboBox;
            if (combo == null && sender is ToolStripComboBox toolStripCombo) combo = toolStripCombo.ComboBox;
            if (combo == null) combo = GetFactoryComboBox();

            bool isUserAction = false;
            try { isUserAction = (combo != null && combo.Focused) || MouseButtons == MouseButtons.Left; } catch { isUserAction = MouseButtons == MouseButtons.Left; }
            if (!isUserAction) return;

            UpdateStatus("工厂已更改，正在重新加载部门列表...");
            LoadDepartments();
        }

        /// <summary>
        /// 数据网格双击事件
        /// </summary>
        private void DgvDepartment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedDepartment();
            }
        }

        /// <summary>
        /// 编辑选中的部门
        /// </summary>
        private void EditSelectedDepartment()
        {
            if (this.dataGridViewDepartments.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要编辑的部门。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                 // string deptId = this.dataGridViewDepartments.SelectedRows[0].Cells["DeptId"].Value.ToString(); 
                 // string factoryId = this.toolStripComboBoxFactory.ComboBox.SelectedValue.ToString(); // Original problematic line

                if (!TryGetSelectedDepartment(out Department selectedDept) || string.IsNullOrWhiteSpace(selectedDept?.DeptId))
                {
                    MessageBox.Show("无法获取选定部门的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string deptId = selectedDept.DeptId;
                string factoryId = selectedDept.FactoryId;

                if (string.IsNullOrWhiteSpace(factoryId))
                {
                    factoryId = GetSelectedFactoryItem()?.Value?.ToString();
                }

                if (string.IsNullOrWhiteSpace(factoryId))
                {
                    MessageBox.Show("无法确定所选部门关联的工厂，请在下拉列表中选择一个具体的工厂。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                 using (FrmDepartmentEdit frmEdit = new FrmDepartmentEdit(deptId, factoryId, currentUser))
                 {
                    if (frmEdit.ShowDialog() == DialogResult.OK)
                    {
                        LoadDepartments();
                    }
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开编辑窗口失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
