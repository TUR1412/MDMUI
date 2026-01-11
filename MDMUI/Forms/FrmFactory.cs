using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;
using System.Linq;
using System.Collections.Generic;
using MDMUI.Controls.Atoms;
using MDMUI.Controls.Molecules;

namespace MDMUI
{
    public partial class FrmFactory : Form
    {
        // 当前用户信息
        private User currentUser;
        // 权限检查器
        private PermissionChecker permissionChecker;
        // 工厂服务
        private FactoryService factoryService;

        // Modern UI (Atomic Design) - runtime enhancement (OCP: do not rewrite Designer)
        private bool modernLayoutInitialized;
        private CardPanel modernHeaderCard;
        private Panel gridHostPanel;
        private Label emptyStateLabel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        public FrmFactory(User user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.permissionChecker = new PermissionChecker();
            this.factoryService = new FactoryService(); // 初始化 FactoryService
            this.Load += FrmFactory_Load;

            // Configure DataGridView columns - 最好在这里或Designer中配置一次  
            ConfigureDataGridView();

            InitializeModernLayout();
            SetButtonVisibility();
        }

        private void InitializeModernLayout()
        {
            if (modernLayoutInitialized) return;
            modernLayoutInitialized = true;

            try
            {
                using (AppTelemetry.Measure("FrmFactory.ModernLayout"))
                {
                    EnsureModernHeader();
                    EnsureGridHostAndEmptyState();
                    EnsureStatusStrip();

                    // UiThemingBootstrapper 只对窗体执行一次；这里确保运行时新
                    // 增控件也能吃到统一风格
                    try { ThemeManager.ApplyTo(this); } catch { }
                    try { ModernTheme.EnableMicroInteractions(this); } catch { }

                    UpdateFactoryListIndicators(textBoxSearch?.Text, dataGridView1?.Rows?.Count ?? 0);
                }
            }
            catch (Exception ex)
            {
                try { AppLog.Error(ex, "初始化工厂管理现代化布局失败"); } catch { }
            }
        }

        private void EnsureModernHeader()
        {
            if (modernHeaderCard != null) return;

            Panel oldToolPanel = panelTool;
            Panel oldSearchPanel = panelSearch;

            Button oldAdd = buttonAdd;
            Button oldEdit = buttonEdit;
            Button oldDelete = buttonDelete;
            Button oldRefresh = buttonRefresh;
            Button oldSearch = buttonSearch;

            // 先把需要保留的输入框迁移出来，避免旧 Panel Dispose 时连带释放
            try
            {
                if (textBoxSearch?.Parent != null)
                {
                    textBoxSearch.Parent.Controls.Remove(textBoxSearch);
                }
            }
            catch
            {
                // ignore
            }

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
                Text = string.IsNullOrWhiteSpace(Text) ? "工厂管理" : Text,
                ForeColor = ThemeManager.Palette.TextPrimary,
                Margin = new Padding(0, 10, 12, 0)
            };
            try { title.Font = ThemeManager.CreateTitleFont(11f); } catch { }
            actionsToolbar.LeftPanel.Controls.Add(title);

            AppButton add = CreateAppButton(oldAdd?.Text ?? "添加", AppButtonVariant.Primary, BtnAdd_Click, name: "buttonAdd");
            AppButton edit = CreateAppButton(oldEdit?.Text ?? "编辑", AppButtonVariant.Secondary, BtnEdit_Click, name: "buttonEdit");
            AppButton delete = CreateAppButton(oldDelete?.Text ?? "删除", AppButtonVariant.Danger, BtnDelete_Click, name: "buttonDelete");
            AppButton refresh = CreateAppButton(oldRefresh?.Text ?? "刷新", AppButtonVariant.Secondary, BtnRefresh_Click, name: "buttonRefresh");
            AppButton search = CreateAppButton(oldSearch?.Text ?? "搜索", AppButtonVariant.Primary, BtnSearch_Click, name: "buttonSearch");

            CopyButtonState(oldAdd, add);
            CopyButtonState(oldEdit, edit);
            CopyButtonState(oldDelete, delete);
            CopyButtonState(oldRefresh, refresh);
            CopyButtonState(oldSearch, search);

            buttonAdd = add;
            buttonEdit = edit;
            buttonDelete = delete;
            buttonRefresh = refresh;
            buttonSearch = search;

            actionsToolbar.RightPanel.Controls.Add(add);
            actionsToolbar.RightPanel.Controls.Add(edit);
            actionsToolbar.RightPanel.Controls.Add(delete);
            actionsToolbar.RightPanel.Controls.Add(refresh);

            Label searchLabel = new Label
            {
                AutoSize = true,
                Text = "工厂名称",
                ForeColor = ThemeManager.Palette.TextSecondary,
                Margin = new Padding(0, 10, 8, 0)
            };
            searchToolbar.LeftPanel.Controls.Add(searchLabel);

            if (textBoxSearch != null)
            {
                textBoxSearch.Width = Math.Max(220, textBoxSearch.Width);
                textBoxSearch.Margin = new Padding(0, 8, 8, 0);
                searchToolbar.LeftPanel.Controls.Add(textBoxSearch);
            }

            searchToolbar.RightPanel.Controls.Add(search);

            layout.Controls.Add(actionsToolbar, 0, 0);
            layout.Controls.Add(searchToolbar, 0, 1);

            // 维持 Dock 计算顺序：Fill 控件需保持较低 z-order
            Controls.Add(header);
            header.BringToFront();
            modernHeaderCard = header;

            // 旧控件移除并释放（开闭原则：运行时替换，不动 Designer）
            try { if (oldToolPanel != null) Controls.Remove(oldToolPanel); } catch { }
            try { if (oldSearchPanel != null) Controls.Remove(oldSearchPanel); } catch { }

            try { oldToolPanel?.Dispose(); } catch { }
            try { oldSearchPanel?.Dispose(); } catch { }
        }

        private void EnsureGridHostAndEmptyState()
        {
            if (gridHostPanel != null) return;
            if (dataGridView1 == null) return;

            int gridIndex = 0;
            try { gridIndex = Controls.GetChildIndex(dataGridView1); } catch { }

            try { Controls.Remove(dataGridView1); } catch { }

            Panel host = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            dataGridView1.Dock = DockStyle.Fill;
            host.Controls.Add(dataGridView1);

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
            if (statusStrip != null) return;

            StatusStrip strip = new StatusStrip
            {
                Dock = DockStyle.Bottom,
                SizingGrip = false
            };

            ToolStripStatusLabel label = new ToolStripStatusLabel { Text = "就绪" };
            strip.Items.Add(label);

            Controls.Add(strip);

            statusStrip = strip;
            statusLabel = label;
        }

        private void UpdateFactoryListIndicators(string searchTerm, int count)
        {
            string normalized = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim();

            if (statusLabel != null)
            {
                if (count <= 0)
                {
                    statusLabel.Text = string.IsNullOrWhiteSpace(normalized) ? "暂无工厂数据" : $"未找到匹配工厂：{normalized}";
                }
                else
                {
                    statusLabel.Text = string.IsNullOrWhiteSpace(normalized) ? $"共 {count} 个工厂" : $"匹配 {count} 个工厂";
                }
            }

            if (emptyStateLabel != null)
            {
                if (count <= 0)
                {
                    emptyStateLabel.Text = string.IsNullOrWhiteSpace(normalized) ? "暂无工厂数据" : "未找到匹配的工厂";
                    emptyStateLabel.Visible = true;
                    try { emptyStateLabel.BringToFront(); } catch { }
                }
                else
                {
                    emptyStateLabel.Visible = false;
                }
            }
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

            // 默认 AutoSize=true，FlowLayoutPanel 下表现更稳定
            try { button.AutoSize = true; } catch { }
            try { button.MinimumSize = new Size(86, 34); } catch { }

            if (onClick != null)
            {
                button.Click += onClick;
            }

            return button;
        }

        private static void CopyButtonState(Button source, Button target)
        {
            if (source == null || target == null) return;

            try { target.Visible = source.Visible; } catch { }
            try { target.Enabled = source.Enabled; } catch { }
            try { target.TabIndex = source.TabIndex; } catch { }
        }

        private void FrmFactory_Load(object sender, EventArgs e)
        {
            try
            {
                AppLog.Info($"工厂管理窗体加载 - 用户: {currentUser?.Username ?? "-"}, 工厂: {currentUser?.FactoryId ?? "-"}");
                LoadData();
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "工厂管理窗体加载错误");
                MessageBox.Show($"窗体加载时发生错误: {ex.Message}", "加载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 根据用户权限设置按钮可见性
        private void SetButtonVisibility()
        {
            if (currentUser == null)
            {
                // 如果没有用户信息，默认显示所有按钮
                return;
            }

            // 检查用户是否为超级管理员
            bool isAdmin = currentUser.RoleName == "超级管理员";

            // 如果是超级管理员，显示所有按钮
            if (isAdmin)
            {
                return;
            }

            // 根据权限检查结果设置按钮可见性
            this.buttonAdd.Visible = permissionChecker.HasPermission(currentUser.Id, "factory", "add");
            this.buttonEdit.Visible = permissionChecker.HasPermission(currentUser.Id, "factory", "edit");
            this.buttonDelete.Visible = permissionChecker.HasPermission(currentUser.Id, "factory", "delete");
        }

        // 配置 DataGridView 列
        private void ConfigureDataGridView()
        {
             // 提高 DGV 性能
            typeof(DataGridView).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(dataGridView1, true, null);

            this.dataGridView1.AutoGenerateColumns = false; // 禁用自动生成列
            this.dataGridView1.Columns.Clear(); // 清除任何设计时可能存在的列

            // 手动添加列并设置 DataPropertyName
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFactoryId", // 列名
                DataPropertyName = "FactoryId", // 绑定到 Factory 模型的 FactoryId 属性
                HeaderText = "工厂编号", // 显示的标题
                Width = 100
            });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colFactoryName",
                DataPropertyName = "FactoryName",
                HeaderText = "工厂名称",
                Width = 150
            });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAddress",
                DataPropertyName = "Address",
                HeaderText = "地址",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // 填充剩余空间
            });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colManager",
                DataPropertyName = "ManagerName",
                HeaderText = "负责人",
                Width = 100
            });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPhone",
                DataPropertyName = "Phone",
                HeaderText = "联系电话",
                Width = 120
            });
        }

        // 重构 LoadData 以使用 FactoryService
        private void LoadData(string searchTerm = null)
        {
            try
            {
                using (AppTelemetry.Measure("factory.load_list"))
                {
                    List<Factory> factories = factoryService.GetFactories(currentUser, searchTerm);

                    // 使用 BindingSource 以获得更好的数据绑定体验
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = factories;
                    this.dataGridView1.DataSource = bindingSource;

                    int count = factories?.Count ?? 0;
                    UpdateFactoryListIndicators(searchTerm, count);

                    AppLog.Info($"已加载 {count} 个工厂数据。");
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex, "加载工厂数据失败");
                MessageBox.Show("加载数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.dataGridView1.DataSource = null; // 出错时清空 DGV
                UpdateFactoryListIndicators(searchTerm, 0);
            }
        }

        // 按钮点击事件处理
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // 检查添加权限
            if (!permissionChecker.HasPermission(currentUser.Id, "factory", "add") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有添加工厂的权限", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 打开编辑窗体进行添加
            using (FrmFactoryEdit frmEdit = new FrmFactoryEdit(null, currentUser)) // 传递 null 表示新增
            {
                if (frmEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadData(); // 刷新数据
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // 检查编辑权限
            if (!permissionChecker.HasPermission(currentUser.Id, "factory", "edit") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有编辑工厂的权限", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    // 获取选定行的 FactoryId (作为字符串)
                    string factoryId = this.dataGridView1.SelectedRows[0].Cells["colFactoryId"]?.Value?.ToString(); // 使用配置的列名
                    
                    if (string.IsNullOrEmpty(factoryId))
                    {
                         MessageBox.Show("无法获取选定工厂的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return;
                    }

                    // 打开编辑窗体进行编辑，传递字符串 ID
                    using (FrmFactoryEdit frmEdit = new FrmFactoryEdit(factoryId, currentUser))
                    {
                        if (frmEdit.ShowDialog() == DialogResult.OK)
                        {
                            LoadData(this.textBoxSearch.Text.Trim()); // 编辑后按当前搜索条件刷新
                        }
                    }
                }
                 catch (Exception ex)
                 {
                     MessageBox.Show("编辑操作失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
            }
            else
            {
                MessageBox.Show("请先选择要编辑的工厂。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // 检查删除权限
            if (!permissionChecker.HasPermission(currentUser.Id, "factory", "delete") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有删除工厂的权限", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                string factoryId = this.dataGridView1.SelectedRows[0].Cells["colFactoryId"]?.Value?.ToString(); // 使用配置的列名
                string factoryName = this.dataGridView1.SelectedRows[0].Cells["colFactoryName"]?.Value?.ToString() ?? "选定工厂";

                if (string.IsNullOrEmpty(factoryId))
                {
                     MessageBox.Show("无法获取选定工厂的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                }

                if (MessageBox.Show($"确定要删除工厂 [{factoryName}] 吗？请确保该工厂下没有关联的用户或部门。", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        bool success = factoryService.DeleteFactory(factoryId);

                        if (success)
                        {
                            MessageBox.Show("工厂删除成功。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData(this.textBoxSearch.Text.Trim()); // 删除后按当前搜索条件刷新
                        }
                        // 如果 BLL 返回 false 或抛出异常，会被下面的 catch 处理
                    }
                    catch (InvalidOperationException opEx) // 捕获 BLL 抛出的关联错误
                    {
                        MessageBox.Show(opEx.Message, "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的工厂。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.textBoxSearch.Text = ""; // 清空搜索框
            LoadData(); // 刷新数据，不带搜索条件
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData(this.textBoxSearch.Text.Trim());
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 检查编辑权限
                if (!permissionChecker.HasPermission(currentUser.Id, "factory", "edit") && currentUser.RoleName != "超级管理员")
                {
                    // 可以选择不提示，因为按钮通常已被禁用
                    // MessageBox.Show("您没有编辑工厂的权限", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.dataGridView1.Rows[e.RowIndex].Selected = true;
                try
                {
                    // 获取字符串 FactoryId
                    string factoryId = this.dataGridView1.Rows[e.RowIndex].Cells["colFactoryId"]?.Value?.ToString(); // 使用配置的列名

                    if (!string.IsNullOrEmpty(factoryId))
                    {
                        // 选中被双击的行，确保 BtnEdit_Click 如果被调用时能正确获取
                        this.dataGridView1.Rows[e.RowIndex].Selected = true;

                        using (FrmFactoryEdit frmEdit = new FrmFactoryEdit(factoryId, currentUser)) // 传递字符串 ID
                        {
                            DialogResult result = frmEdit.ShowDialog(this);
                            if (result == DialogResult.OK)
                            {
                                LoadData(this.textBoxSearch.Text.Trim()); // 编辑后按当前搜索条件刷新
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("无法获取选中行的工厂ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("处理双击事件时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
