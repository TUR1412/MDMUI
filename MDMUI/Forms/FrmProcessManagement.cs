using MDMUI.BLL;
using MDMUI.Model;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

namespace MDMUI
{
    public partial class FrmProcessManagement : Form
    {
        private User CurrentUser;
        private ProcessPackageBLL packageBLL;
        private ProcessBLL processBLL;
        private ProcessRouteBLL routeBLL;
        
        // 当前选中的工艺包和工艺流程ID
        private string selectedPackageId;
        private string selectedProcessId;

        public FrmProcessManagement(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            packageBLL = new ProcessPackageBLL();
            processBLL = new ProcessBLL();
            routeBLL = new ProcessRouteBLL();
            
            // 绑定事件
            this.Load += FrmProcessManagement_Load;
        }

        private void FrmProcessManagement_Load(object sender, EventArgs e)
        {
            InitializeUI();
            LoadProcessPackages();
        }

        private void InitializeUI()
        {
            // 设置窗体属性
            this.Text = "🏭 工艺管理 - 专业版 v2.0";
            this.WindowState = FormWindowState.Maximized;
            
            // 初始化详细信息显示
            detailContent.Text = "🎯 请选择一个项目查看详细信息\n\n✨ 点击工艺包、工艺流程或工艺路线的任意行，\n📋 在此处查看详细信息。\n\n💡 提示：每个标题栏都有不同的颜色主题！";
            
            // 设置数据表格样式
            ConfigureDataGridView(dgvPackage);
            ConfigureDataGridView(dgvProcess);
            ConfigureDataGridView(dgvRoute);
            
            // 添加悬停效果
            AddHoverEffects();
            
            // 添加行点击事件
            dgvRoute.SelectionChanged += DgvRoute_SelectionChanged;
            
            // 设置修改工艺路线按钮
            btnUpdateOpSeq.BackColor = Color.FromArgb(87, 166, 245);
            btnUpdateOpSeq.ForeColor = Color.White;
            btnUpdateOpSeq.FlatStyle = FlatStyle.Flat;
            btnUpdateOpSeq.FlatAppearance.BorderSize = 0;
            btnUpdateOpSeq.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnUpdateOpSeq.Text = "修改工艺路线";
            btnUpdateOpSeq.Visible = true;
            btnUpdateOpSeq.Parent = routePanel;
            btnUpdateOpSeq.Location = new Point(routePanel.Width - btnUpdateOpSeq.Width - 20, 5);
            btnUpdateOpSeq.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdateOpSeq.Cursor = Cursors.Hand;
            btnUpdateOpSeq.BringToFront();
        }

        private void AddHoverEffects()
        {
            // 给面板添加鼠标悬停效果
            AddPanelHoverEffect(packagePanel, Color.FromArgb(248, 251, 255));
            AddPanelHoverEffect(processPanel, Color.FromArgb(248, 251, 255));
            AddPanelHoverEffect(routePanel, Color.FromArgb(248, 251, 255));
            AddPanelHoverEffect(detailPanel, Color.FromArgb(248, 251, 255));
            
            // 给标题标签添加悬停效果
            AddLabelHoverEffect(lblPackage, Color.FromArgb(41, 128, 185), Color.FromArgb(52, 152, 219));
            AddLabelHoverEffect(lblProcess, Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185));
            AddLabelHoverEffect(lblRoute, Color.FromArgb(46, 204, 113), Color.FromArgb(39, 174, 96));
            AddLabelHoverEffect(lblDetail, Color.FromArgb(155, 89, 182), Color.FromArgb(142, 68, 173));
        }

        private void AddPanelHoverEffect(Panel panel, Color hoverColor)
        {
            Color originalColor = panel.BackColor;
            
            panel.MouseEnter += (s, e) => 
            {
                panel.BackColor = hoverColor;
                panel.Cursor = Cursors.Hand;
            };
            
            panel.MouseLeave += (s, e) => 
            {
                panel.BackColor = originalColor;
                panel.Cursor = Cursors.Default;
            };
        }

        private void AddLabelHoverEffect(Label label, Color originalColor, Color hoverColor)
        {
            label.MouseEnter += (s, e) => 
            {
                label.BackColor = hoverColor;
                label.Cursor = Cursors.Hand;
            };
            
            label.MouseLeave += (s, e) => 
            {
                label.BackColor = originalColor;
                label.Cursor = Cursors.Default;
            };
        }

        private void ConfigureDataGridView(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.BackgroundColor = Color.FromArgb(252, 253, 254);
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.ScrollBars = ScrollBars.Both;
            
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(174, 214, 241);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(44, 62, 80);
            dgv.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9.5F);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
            dgv.DefaultCellStyle.Padding = new Padding(12, 6, 12, 6);
            dgv.GridColor = Color.FromArgb(223, 228, 234);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            
            dgv.CellMouseEnter += (sender, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 248, 250);
                }
            };
            
            dgv.CellMouseLeave += (sender, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            };
        }

        private void LoadProcessPackages()
        {
            try
            {
                DataTable packageData = packageBLL.GetAllProcessPackages();
                dgvPackage.DataSource = packageData;
                
                // 更新标签计数
                lblPackage.Text = $"📦 工艺包产品 ({packageData.Rows.Count})";
                
                // 设置列属性 - 使用固定宽度而非FillWeight
                if (dgvPackage.Columns.Contains("PackageId"))
                {
                    dgvPackage.Columns["PackageId"].HeaderText = "📋 工艺包ID";
                    dgvPackage.Columns["PackageId"].Width = 150;
                }
                
                if (dgvPackage.Columns.Contains("Version"))
                {
                    dgvPackage.Columns["Version"].HeaderText = "🔖 版本";
                    dgvPackage.Columns["Version"].Width = 80;
                }
                
                if (dgvPackage.Columns.Contains("Description"))
                {
                    dgvPackage.Columns["Description"].HeaderText = "📝 描述";
                    dgvPackage.Columns["Description"].Width = 200;
                }
                
                if (dgvPackage.Columns.Contains("ProductId"))
                {
                    dgvPackage.Columns["ProductId"].HeaderText = "🎯 产品ID";
                    dgvPackage.Columns["ProductId"].Width = 120;
                }
                
                if (dgvPackage.Columns.Contains("ProductName"))
                {
                    dgvPackage.Columns["ProductName"].HeaderText = "🏷️ 产品名称";
                    dgvPackage.Columns["ProductName"].Width = 150;
                }
                
                if (dgvPackage.Columns.Contains("CreateTime"))
                {
                    dgvPackage.Columns["CreateTime"].HeaderText = "📅 创建时间";
                    dgvPackage.Columns["CreateTime"].Width = 120;
                    dgvPackage.Columns["CreateTime"].DefaultCellStyle.Format = "yyyy/MM/dd";
                }
                
                if (dgvPackage.Columns.Contains("Status"))
                {
                    dgvPackage.Columns["Status"].HeaderText = "✅ 状态";
                    dgvPackage.Columns["Status"].Width = 80;
                }
                
                // 如果有数据，选择第一行
                if (dgvPackage.Rows.Count > 0)
                {
                    dgvPackage.Rows[0].Selected = true;
                    selectedPackageId = dgvPackage.Rows[0].Cells["PackageId"].Value.ToString();
                    LoadProcessesByPackageId(selectedPackageId);
                    ShowPackageDetail(dgvPackage.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 加载工艺包数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProcessesByPackageId(string packageId)
        {
            try
            {
                DataTable processData = processBLL.GetProcessesByPackageId(packageId);
                dgvProcess.DataSource = processData;
                
                // 更新标签计数
                lblProcess.Text = $"⚙️ 工艺流程绑定工艺包 ({processData.Rows.Count})";
                
                // 设置列属性
                if (dgvProcess.Columns.Contains("ProcessId"))
                {
                    dgvProcess.Columns["ProcessId"].HeaderText = "🔧 工艺流程ID";
                    dgvProcess.Columns["ProcessId"].Width = 150;
                }
                
                if (dgvProcess.Columns.Contains("Version"))
                {
                    dgvProcess.Columns["Version"].HeaderText = "🔖 版本";
                    dgvProcess.Columns["Version"].Width = 80;
                }
                
                if (dgvProcess.Columns.Contains("PackageId"))
                {
                    dgvProcess.Columns["PackageId"].HeaderText = "📦 工艺包ID";
                    dgvProcess.Columns["PackageId"].Width = 150;
                }
                
                if (dgvProcess.Columns.Contains("Description"))
                {
                    dgvProcess.Columns["Description"].HeaderText = "📄 工艺流程描述";
                    dgvProcess.Columns["Description"].Width = 200;
                }
                
                if (dgvProcess.Columns.Contains("ProductionType"))
                {
                    dgvProcess.Columns["ProductionType"].HeaderText = "🏭 生产类型";
                    dgvProcess.Columns["ProductionType"].Width = 120;
                }
                
                if (dgvProcess.Columns.Contains("Sequence"))
                {
                    dgvProcess.Columns["Sequence"].HeaderText = "🔢 顺序";
                    dgvProcess.Columns["Sequence"].Width = 80;
                }
                
                if (dgvProcess.Columns.Contains("CreateTime"))
                {
                    dgvProcess.Columns["CreateTime"].HeaderText = "📅 创建时间";
                    dgvProcess.Columns["CreateTime"].Width = 120;
                    dgvProcess.Columns["CreateTime"].DefaultCellStyle.Format = "yyyy/MM/dd";
                }
                
                if (dgvProcess.Columns.Contains("Status"))
                {
                    dgvProcess.Columns["Status"].HeaderText = "✅ 状态";
                    dgvProcess.Columns["Status"].Width = 80;
                }
                
                // 如果有数据，选择第一行
                if (dgvProcess.Rows.Count > 0)
                {
                    dgvProcess.Rows[0].Selected = true;
                    selectedProcessId = dgvProcess.Rows[0].Cells["ProcessId"].Value.ToString();
                    LoadRoutesByProcessId(selectedProcessId);
                    ShowProcessDetail(dgvProcess.Rows[0]);
                }
                else
                {
                    // 如果没有数据，清空路线表和选中ID
                    selectedProcessId = null;
                    dgvRoute.DataSource = null;
                    lblRoute.Text = "🛣️ 工艺路线 (0)";
                    detailContent.Text = "🔍 当前选中的工艺包没有相关联的工艺流程。\n\n🔄 请选择其他工艺包，或考虑添加工艺流程到此工艺包。";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 加载工艺流程数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRoutesByProcessId(string processId)
        {
            try
            {
                DataTable routeData = routeBLL.GetRoutesByProcessId(processId);
                dgvRoute.DataSource = routeData;
                
                // 更新标签计数
                lblRoute.Text = $"🛣️ 工艺路线 ({routeData.Rows.Count})";
                
                // 设置列属性
                if (dgvRoute.Columns.Contains("RouteId"))
                {
                    dgvRoute.Columns["RouteId"].HeaderText = "🛣️ 工艺路线ID";
                    dgvRoute.Columns["RouteId"].Width = 150;
                }
                
                if (dgvRoute.Columns.Contains("StationId"))
                {
                    dgvRoute.Columns["StationId"].HeaderText = "🏢 工位ID";
                    dgvRoute.Columns["StationId"].Width = 120;
                }
                
                if (dgvRoute.Columns.Contains("Version"))
                {
                    dgvRoute.Columns["Version"].HeaderText = "🔖 版本";
                    dgvRoute.Columns["Version"].Width = 80;
                }
                
                if (dgvRoute.Columns.Contains("ProcessId"))
                {
                    dgvRoute.Columns["ProcessId"].HeaderText = "🔧 工艺流程ID";
                    dgvRoute.Columns["ProcessId"].Width = 150;
                }
                
                if (dgvRoute.Columns.Contains("Description"))
                {
                    dgvRoute.Columns["Description"].HeaderText = "📄 工艺描述";
                    dgvRoute.Columns["Description"].Width = 200;
                }
                
                if (dgvRoute.Columns.Contains("Sequence"))
                {
                    dgvRoute.Columns["Sequence"].HeaderText = "🔢 顺序";
                    dgvRoute.Columns["Sequence"].Width = 80;
                }
                
                if (dgvRoute.Columns.Contains("StationType"))
                {
                    dgvRoute.Columns["StationType"].HeaderText = "🏭 工位类型";
                    dgvRoute.Columns["StationType"].Width = 120;
                }
                
                if (dgvRoute.Columns.Contains("CreateTime"))
                {
                    dgvRoute.Columns["CreateTime"].HeaderText = "📅 创建时间";
                    dgvRoute.Columns["CreateTime"].Width = 120;
                    dgvRoute.Columns["CreateTime"].DefaultCellStyle.Format = "yyyy/MM/dd";
                }
                
                if (dgvRoute.Columns.Contains("Status"))
                {
                    dgvRoute.Columns["Status"].HeaderText = "✅ 状态";
                    dgvRoute.Columns["Status"].Width = 80;
                }
                
                // 如果有数据，选择第一行
                if (dgvRoute.Rows.Count > 0)
                {
                    dgvRoute.Rows[0].Selected = true;
                    ShowRouteDetail(dgvRoute.Rows[0]);
                }
                else
                {
                    // 如果没有数据，显示提示信息
                    detailContent.Text = "🔍 当前选中的工艺流程没有相关联的工艺路线。\n\n🔄 请选择其他工艺流程，或考虑添加工艺路线到此流程。";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ 加载工艺路线数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 显示工艺包详细信息
        private void ShowPackageDetail(DataGridViewRow row)
        {
            if (row == null || row.DataBoundItem == null) return;
            
            var sb = new StringBuilder();
            sb.AppendLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            sb.AppendLine("┃ 📦 工艺包产品详细信息                    ┃");
            sb.AppendLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            sb.AppendLine();
            
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.OwningColumn.Visible && cell.Value != null)
                {
                    sb.AppendLine($"🔹 {cell.OwningColumn.HeaderText}：");
                    sb.AppendLine($"   {cell.Value}");
                    sb.AppendLine();
                }
            }
            
            sb.AppendLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            sb.AppendLine("┃ 💡 提示：选择工艺流程查看更多详情    ┃");
            sb.AppendLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            
            detailContent.Text = sb.ToString();
        }

        // 显示工艺流程详细信息
        private void ShowProcessDetail(DataGridViewRow row)
        {
            if (row == null || row.DataBoundItem == null) return;
            
            var sb = new StringBuilder();
            sb.AppendLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            sb.AppendLine("┃ ⚙️ 工艺流程详细信息                    ┃");
            sb.AppendLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            sb.AppendLine();
            
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.OwningColumn.Visible && cell.Value != null)
                {
                    sb.AppendLine($"🔹 {cell.OwningColumn.HeaderText}：");
                    sb.AppendLine($"   {cell.Value}");
                    sb.AppendLine();
                }
            }
            
            sb.AppendLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            sb.AppendLine("┃ 💡 提示：选择工艺路线查看站点详情    ┃");
            sb.AppendLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            
            detailContent.Text = sb.ToString();
        }

        // 显示工艺路线详细信息
        private void ShowRouteDetail(DataGridViewRow row)
        {
            if (row == null || row.DataBoundItem == null) return;
            
            var sb = new StringBuilder();
            sb.AppendLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            sb.AppendLine("┃ 🛣️ 工艺路线详细信息                    ┃");
            sb.AppendLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            sb.AppendLine();
            
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.OwningColumn.Visible && cell.Value != null)
                {
                    sb.AppendLine($"🔹 {cell.OwningColumn.HeaderText}：");
                    sb.AppendLine($"   {cell.Value}");
                    sb.AppendLine();
                }
            }
            
            sb.AppendLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            sb.AppendLine("┃ 💡 提示：该工站的具体配置和参数      ┃");
            sb.AppendLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            
            detailContent.Text = sb.ToString();
        }

        private void DgvPackage_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPackage.SelectedRows.Count > 0)
            {
                selectedPackageId = dgvPackage.SelectedRows[0].Cells["PackageId"].Value.ToString();
                LoadProcessesByPackageId(selectedPackageId);
                ShowPackageDetail(dgvPackage.SelectedRows[0]);
            }
        }

        private void DgvProcess_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProcess.SelectedRows.Count > 0)
            {
                selectedProcessId = dgvProcess.SelectedRows[0].Cells["ProcessId"].Value.ToString();
                LoadRoutesByProcessId(selectedProcessId);
                ShowProcessDetail(dgvProcess.SelectedRows[0]);
            }
        }

        private void DgvRoute_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRoute.SelectedRows.Count > 0)
            {
                ShowRouteDetail(dgvRoute.SelectedRows[0]);
            }
        }

        // 添加一个按钮点击事件，用于打开工艺路线修改窗体
        private void btnUpdateOpSeq_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否选中了工艺流程
                if (string.IsNullOrEmpty(selectedProcessId))
                {
                    MessageBox.Show("请先选择一个工艺流程", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 打开工艺路线修改窗体
                FrmUpdateOpSeq frmUpdateOpSeq = new FrmUpdateOpSeq(selectedProcessId);
                DialogResult result = frmUpdateOpSeq.ShowDialog();
                
                // 如果窗体返回OK，则刷新工艺路线数据
                if (result == DialogResult.OK)
                {
                    // 刷新工艺路线数据
                    if (!string.IsNullOrEmpty(selectedProcessId))
                    {
                        LoadRoutesByProcessId(selectedProcessId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开工艺路线修改窗体时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 