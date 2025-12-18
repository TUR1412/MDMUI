using MDMUI.BLL;
using MDMUI.Model;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;

namespace MDMUI
{
    public partial class FrmProcessDefinition : Form
    {
        private User CurrentUser;
        private ProcessPackageBLL packageBLL;
        private ProcessBLL processBLL;
        private ProcessRouteBLL routeBLL;
        
        // 当前选中的工艺包和工艺流程ID
        private string selectedPackageId;
        private string selectedProcessId;

        public FrmProcessDefinition(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            packageBLL = new ProcessPackageBLL();
            processBLL = new ProcessBLL();
            routeBLL = new ProcessRouteBLL();
            
            // 绑定事件
            this.Load += FrmProcessDefinition_Load;
            btnSearch.Click += BtnSearch_Click;
            btnClose.Click += BtnClose_Click;
            
            // 表格选择变更绑定属性显示
            dgvPackage.SelectionChanged += DgvPackage_SelectionChanged;
            dgvProcess.SelectionChanged += DgvProcess_SelectionChanged;
            dgvRoute.SelectionChanged += DgvRoute_SelectionChanged;
            
            // 工具栏按钮事件
            toolStripButtonSearch.Click += ToolStripButtonSearch_Click;
            toolStripButtonExport.Click += ToolStripButtonExport_Click;
            toolStripButtonPrint.Click += ToolStripButtonPrint_Click;
            toolStripButtonRefresh.Click += ToolStripButtonRefresh_Click;
            toolStripButtonHelp.Click += ToolStripButtonHelp_Click;
        }

        private void FrmProcessDefinition_Load(object sender, EventArgs e)
        {
            InitializeFilters();
            LoadProcessPackages();
            
            // 设置默认标题
            label2.Text = "属性: 工艺规范产品";
            
            // 设置DataGridView样式
            ConfigureDataGridViewStyles();
        }

        private void ConfigureDataGridViewStyles()
        {
            // 设置DataGridView的统一样式
            ConfigurePackageGridColumns();
            ConfigureProcessGridColumns();
            ConfigureRouteGridColumns();
            
            // 设置DataGridView的替代行背景色和行高
            foreach (DataGridView dgv in new[] { dgvPackage, dgvProcess, dgvRoute })
            {
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                dgv.RowTemplate.Height = 25;
                dgv.BorderStyle = BorderStyle.None;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.GridColor = Color.FromArgb(230, 230, 230);
                dgv.RowHeadersVisible = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToResizeRows = false;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeFilters()
        {
            // 初始化过滤条件
            cboProductType.Items.Add("All");
            cboProductType.SelectedIndex = 0;
            
            cboReleaseStatus.Items.Add("All");
            cboReleaseStatus.SelectedIndex = 0;
            
            cboActive.Items.Add("All");
            cboActive.SelectedIndex = 0;
            
            cboDetailedType.Items.Add("All");
            cboDetailedType.SelectedIndex = 0;
            
            cboProductType.SelectedIndexChanged += FilterChanged;
            cboReleaseStatus.SelectedIndexChanged += FilterChanged;
            cboActive.SelectedIndexChanged += FilterChanged;
            cboDetailedType.SelectedIndexChanged += FilterChanged;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            // 当过滤条件改变时重新加载数据
            LoadProcessPackages();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadProcessPackages();
        }
        
        #region 工具栏按钮事件
        
        private void ToolStripButtonSearch_Click(object sender, EventArgs e)
        {
            LoadProcessPackages();
        }
        
        private void ToolStripButtonExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导出功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ToolStripButtonPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("打印功能尚未实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ToolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            LoadProcessPackages();
        }
        
        private void ToolStripButtonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("工艺包管理帮助\n\n用于管理工艺包、工艺流程和工艺路线。", "帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        #endregion

        private void LoadProcessPackages()
        {
            try
            {
                // 获取筛选数据
                string productType = cboProductType.SelectedItem?.ToString() ?? "All";
                string releaseStatus = cboReleaseStatus.SelectedItem?.ToString() ?? "All";
                string active = cboActive.SelectedItem?.ToString() ?? "All";
                string detailedType = cboDetailedType.SelectedItem?.ToString() ?? "All";
                string productCode = txtProductCode.Text.Trim();

                // 获取所有工艺包数据
                DataTable dt = packageBLL.GetAllProcessPackages();
                
                // 创建视图（用于筛选）
                DataView dv = new DataView(dt);
                
                // 应用筛选条件
                if (productType != "All")
                {
                    dv.RowFilter += dv.RowFilter.Length > 0 ? " AND " : "";
                    dv.RowFilter += $"ProductType = '{productType}'";
                }
                
                if (releaseStatus != "All")
                {
                    dv.RowFilter += dv.RowFilter.Length > 0 ? " AND " : "";
                    dv.RowFilter += $"Status = '{releaseStatus}'";
                }
                
                if (active != "All")
                {
                    dv.RowFilter += dv.RowFilter.Length > 0 ? " AND " : "";
                    dv.RowFilter += $"Active = '{active}'";
                }
                
                if (detailedType != "All")
                {
                    dv.RowFilter += dv.RowFilter.Length > 0 ? " AND " : "";
                    dv.RowFilter += $"DetailedType = '{detailedType}'";
                }
                
                if (!string.IsNullOrEmpty(productCode))
                {
                    dv.RowFilter += dv.RowFilter.Length > 0 ? " AND " : "";
                    dv.RowFilter += $"ProductId LIKE '%{productCode}%' OR Description LIKE '%{productCode}%'";
                }
                
                // 应用数据到DataGridView
                dgvPackage.DataSource = dv;
                
                // 清除其他相关数据
                dgvProcess.DataSource = null;
                dgvRoute.DataSource = null;
                
                // 更新选中项
                selectedPackageId = null;
                selectedProcessId = null;
                
                // 清除属性显示
                propertyGrid.SelectedObject = null;
                lblProperties.Text = "工艺规范配件";
                label2.Text = "属性: 工艺规范产品";
                label1.Text = "版本";
                
                // 如果有数据，则选中第一行
                if (dgvPackage.Rows.Count > 0)
                {
                    dgvPackage.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载工艺包数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurePackageGridColumns()
        {
            // 设置列标题和格式
            if (dgvPackage.Columns.Count > 0)
            {
                // 清除所有列
                dgvPackage.Columns.Clear();
            }

            // 依次添加列并设置属性
            DataGridViewTextBoxColumn packageIdColumn = new DataGridViewTextBoxColumn();
            packageIdColumn.Name = "PackageId";
            packageIdColumn.HeaderText = "工艺包ID";
            packageIdColumn.DataPropertyName = "PackageId";
            packageIdColumn.Width = 130;
            dgvPackage.Columns.Add(packageIdColumn);

            DataGridViewTextBoxColumn versionColumn = new DataGridViewTextBoxColumn();
            versionColumn.Name = "Version";
            versionColumn.HeaderText = "版本";
            versionColumn.DataPropertyName = "Version";
            versionColumn.Width = 60;
            dgvPackage.Columns.Add(versionColumn);

            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.Name = "Description";
            descriptionColumn.HeaderText = "描述";
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.Width = 200;
            dgvPackage.Columns.Add(descriptionColumn);

            DataGridViewTextBoxColumn productIdColumn = new DataGridViewTextBoxColumn();
            productIdColumn.Name = "ProductId";
            productIdColumn.HeaderText = "产品ID";
            productIdColumn.DataPropertyName = "ProductId";
            productIdColumn.Width = 100;
            dgvPackage.Columns.Add(productIdColumn);

            // 设置样式
            dgvPackage.EnableHeadersVisualStyles = false;
            dgvPackage.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
            dgvPackage.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvPackage.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 9F, FontStyle.Bold);
            dgvPackage.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPackage.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPackage.ColumnHeadersHeight = 28;

            // 行样式
            dgvPackage.RowsDefaultCellStyle.Font = new Font("宋体", 9F, FontStyle.Regular);
            dgvPackage.RowsDefaultCellStyle.BackColor = Color.White;
            dgvPackage.RowsDefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvPackage.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            // 交替行颜色
            dgvPackage.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            
            // 选择整行
            dgvPackage.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPackage.MultiSelect = false;
            dgvPackage.ReadOnly = true;
        }

        private void LoadProcessesByPackageId(string packageId)
        {
            try
            {
                DataTable processData = processBLL.GetProcessesByPackageId(packageId);
                dgvProcess.DataSource = processData;
                
                // 设置右上标签显示选中的工艺包
                string packageCount = processData.Rows.Count.ToString();
                lblProcessCount.Text = $"工艺流程产品 ({packageCount})";
                
                // 设置列属性
                ConfigureProcessGridColumns();
                
                // 如果有数据，选择第一行
                if (dgvProcess.Rows.Count > 0)
                {
                    dgvProcess.Rows[0].Selected = true;
                    selectedProcessId = dgvProcess.Rows[0].Cells["ProcessId"].Value.ToString();
                    LoadRoutesByProcessId(selectedProcessId);
                }
                else
                {
                    // 如果没有数据，清空路线表格
                    dgvRoute.DataSource = null;
                    lblRouteCount.Text = "工艺路线 (0)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载工艺流程数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureProcessGridColumns()
        {
            if (dgvProcess.Columns.Count > 0) dgvProcess.Columns.Clear();
            dgvProcess.AutoGenerateColumns = false;

            dgvProcess.Columns.Add(new DataGridViewTextBoxColumn { Name = "ProcessId", HeaderText = "工艺流程ID", DataPropertyName = "ProcessId", Width = 130 });
            dgvProcess.Columns.Add(new DataGridViewTextBoxColumn { Name = "Version", HeaderText = "版本", DataPropertyName = "Version", Width = 60 });
            
            DataGridViewTextBoxColumn typeColumn = new DataGridViewTextBoxColumn();
            typeColumn.Name = "ProductionType";
            typeColumn.HeaderText = "工艺类型";
            typeColumn.DataPropertyName = "ProductionType";
            typeColumn.Width = 100;
            typeColumn.ValueType = typeof(string);
            dgvProcess.Columns.Add(typeColumn);
            
            dgvProcess.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "描述", DataPropertyName = "Description", Width = 200 });
            dgvProcess.Columns.Add(new DataGridViewTextBoxColumn { Name = "Sequence", HeaderText = "顺序", DataPropertyName = "Sequence", Width = 50 });
            
            DataGridViewCheckBoxColumn usedColumn = new DataGridViewCheckBoxColumn();
            usedColumn.Name = "IsCurrentlyUsed";
            usedColumn.HeaderText = "当前使用";
            usedColumn.DataPropertyName = "IsCurrentlyUsed";
            usedColumn.Width = 80;
            dgvProcess.Columns.Add(usedColumn);
        }

        private void LoadRoutesByProcessId(string processId)
        {
            try
            {
                DataTable routeData = routeBLL.GetRoutesByProcessId(processId);
                dgvRoute.DataSource = routeData;
                
                // 更新底部标签显示选中的工艺路线数量
                string routeCount = routeData.Rows.Count.ToString();
                lblRouteCount.Text = $"工艺路线 ({routeCount})";
                
                // 设置列属性
                ConfigureRouteGridColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载工艺路线数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureRouteGridColumns()
        {
            if (dgvRoute.Columns.Count > 0) dgvRoute.Columns.Clear();
            dgvRoute.AutoGenerateColumns = false;

            dgvRoute.Columns.Add(new DataGridViewTextBoxColumn { Name = "RouteId", HeaderText = "工艺路线ID", DataPropertyName = "RouteId", Width = 130 });
            dgvRoute.Columns.Add(new DataGridViewTextBoxColumn { Name = "StationId", HeaderText = "工位编号", DataPropertyName = "StationId", Width = 100 });
            dgvRoute.Columns.Add(new DataGridViewTextBoxColumn { Name = "Version", HeaderText = "版本", DataPropertyName = "Version", Width = 60 });
            dgvRoute.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "工艺说明", DataPropertyName = "Description", Width = 200 });
            dgvRoute.Columns.Add(new DataGridViewTextBoxColumn { Name = "Sequence", HeaderText = "顺序", DataPropertyName = "Sequence", Width = 50 });
            
            DataGridViewTextBoxColumn stationTypeColumn = new DataGridViewTextBoxColumn();
            stationTypeColumn.Name = "StationType";
            stationTypeColumn.HeaderText = "工位类型";
            stationTypeColumn.DataPropertyName = "StationType";
            stationTypeColumn.Width = 100;
            stationTypeColumn.ValueType = typeof(string);
            dgvRoute.Columns.Add(stationTypeColumn);
        }

        private void DgvPackage_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvPackage.SelectedRows.Count > 0)
                {
                    // 获取选中行
                    DataRowView row = dgvPackage.SelectedRows[0].DataBoundItem as DataRowView;
                    if (row != null)
                    {
                        // 获取选中工艺包的ID
                        selectedPackageId = row["PackageId"].ToString();
                        
                        // 加载相关的工艺流程
                        LoadProcessesByPackageId(selectedPackageId);
                        
                        // 显示属性
                        DisplaySelectedPackageProperties(row);
                        
                        // 更新标签
                        lblProperties.Text = "工艺包";
                        label2.Text = $"属性: {row["Description"]}";
                        label1.Text = $"版本: {row["Version"]}";
                    }
                }
                else
                {
                    // 清除选择
                    selectedPackageId = null;
                    dgvProcess.DataSource = null;
                    dgvRoute.DataSource = null;
                    propertyGrid.SelectedObject = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理工艺包选择变更时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvProcess_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProcess.SelectedRows.Count > 0)
            {
                selectedProcessId = dgvProcess.SelectedRows[0].Cells["ProcessId"].Value.ToString();
                LoadRoutesByProcessId(selectedProcessId);
                
                // 将选中的行数据转为属性显示对象
                DisplaySelectedProcessProperties(dgvProcess.SelectedRows[0].DataBoundItem as DataRowView);
            }
        }
        
        private void DgvRoute_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRoute.SelectedRows.Count > 0)
            {
                // 将选中的行数据转为属性显示对象
                DisplaySelectedRouteProperties(dgvRoute.SelectedRows[0].DataBoundItem as DataRowView);
            }
        }
        
        #region 属性显示相关方法
        
        private void DisplaySelectedPackageProperties(DataRowView row)
        {
            if (row == null) { propertyGrid.SelectedObject = null; return; }
            PackageDisplay packageDisplay = new PackageDisplay
            {
                PackageId = row["PackageId"].ToString(),
                Version = row["Version"].ToString(),
                Description = row["Description"].ToString(),
                ProductId = row["ProductId"].ToString(),
                ProductName = row.Row.Table.Columns.Contains("ProductName") && row["ProductName"] != DBNull.Value ? row["ProductName"].ToString() : string.Empty,
                CreateTime = row.Row.Table.Columns.Contains("CreateTime") && row["CreateTime"] != DBNull.Value ? Convert.ToDateTime(row["CreateTime"]) : DateTime.MinValue,
                Status = row.Row.Table.Columns.Contains("Status") && row["Status"] != DBNull.Value ? row["Status"].ToString() : string.Empty
            };
            propertyGrid.SelectedObject = packageDisplay;
            lblProperties.Text = "工艺包";
            label2.Text = $"属性: {packageDisplay.Description}";
            label1.Text = $"版本: {packageDisplay.Version}";
        }
        
        private void DisplaySelectedProcessProperties(DataRowView row)
        {
            if (row == null) { propertyGrid.SelectedObject = null; return; }
            ProcessDisplay processDisplay = new ProcessDisplay
            {
                ProcessId = row["ProcessId"].ToString(),
                Version = row["Version"].ToString(),
                PackageId = row["PackageId"].ToString(),
                Description = row["Description"].ToString(),
                Sequence = row["Sequence"] != DBNull.Value ? Convert.ToInt32(row["Sequence"]) : 0,
                IsCurrentlyUsed = row["IsCurrentlyUsed"] != DBNull.Value && Convert.ToBoolean(row["IsCurrentlyUsed"])
            };

            string productionTypeFromDb = row["ProductionType"] != DBNull.Value ? row["ProductionType"].ToString() : "";
            if (string.IsNullOrWhiteSpace(productionTypeFromDb) || productionTypeFromDb.Contains("?")) 
            {
                string desc = processDisplay.Description.ToLower();
                if (desc.Contains("装配")) processDisplay.ProductionType = "装配";
                else if (desc.Contains("测试")) processDisplay.ProductionType = "测试";
                else if (desc.Contains("组装")) processDisplay.ProductionType = "组装";
                else if (desc.Contains("打包")) processDisplay.ProductionType = "打包";
                else if (desc.Contains("入库")) processDisplay.ProductionType = "入库";
                else processDisplay.ProductionType = "待定义";
            }
            else
            {
                processDisplay.ProductionType = productionTypeFromDb;
            }

            propertyGrid.SelectedObject = processDisplay;
            lblProperties.Text = "工艺流程";
            label2.Text = $"属性: {processDisplay.Description}";
            label1.Text = $"版本: {processDisplay.Version}";
        }
        
        private void DisplaySelectedRouteProperties(DataRowView row)
        {
            if (row == null) { propertyGrid.SelectedObject = null; return; }
            RouteDisplay routeDisplay = new RouteDisplay
            {
                RouteId = row["RouteId"].ToString(),
                StationId = row["StationId"].ToString(),
                Version = row["Version"].ToString(),
                ProcessId = row["ProcessId"].ToString(),
                Description = row["Description"].ToString(),
                Sequence = row["Sequence"] != DBNull.Value ? Convert.ToInt32(row["Sequence"]) : 0,
                CreateTime = row.Row.Table.Columns.Contains("CreateTime") && row["CreateTime"] != DBNull.Value ? Convert.ToDateTime(row["CreateTime"]) : DateTime.MinValue,
                Status = row.Row.Table.Columns.Contains("Status") && row["Status"] != DBNull.Value ? row["Status"].ToString() : string.Empty
            };

            string stationTypeFromDb = row["StationType"] != DBNull.Value ? row["StationType"].ToString() : "";
            if (string.IsNullOrWhiteSpace(stationTypeFromDb) || stationTypeFromDb.Contains("?"))
            {
                string desc = routeDisplay.Description.ToLower();
                if (desc.Contains("准备") || desc.Contains("清点")) routeDisplay.StationType = "准备站";
                else if (desc.Contains("安装") || desc.Contains("组装")) routeDisplay.StationType = "组装站";
                else if (desc.Contains("测试")) routeDisplay.StationType = "测试站";
                else if (desc.Contains("焊接")) routeDisplay.StationType = "焊接站";
                else if (desc.Contains("压合")) routeDisplay.StationType = "压合站";
                else if (desc.Contains("打包") || desc.Contains("包装")) routeDisplay.StationType = "包装站";
                else if (desc.Contains("扫描")) routeDisplay.StationType = "扫描站";
                else if (desc.Contains("分配")) routeDisplay.StationType = "分配站";
                else if (desc.Contains("ai") || desc.Contains("智能")) routeDisplay.StationType = "智能工位";
                else routeDisplay.StationType = "待定义";
            }
            else
            {
                routeDisplay.StationType = stationTypeFromDb;
            }

            propertyGrid.SelectedObject = routeDisplay;
            lblProperties.Text = "工艺路线";
            label2.Text = $"属性: {routeDisplay.Description}";
            label1.Text = $"版本: {routeDisplay.Version}";
        }
        
        #endregion
    }

    #region POCO Classes for PropertyGrid Display

    public class PackageDisplay
    {
        [DisplayName("工艺包ID")]
        [Category("基本信息")]
        public string PackageId { get; set; }

        [DisplayName("版本")]
        [Category("基本信息")]
        public string Version { get; set; }

        [DisplayName("描述")]
        [Category("基本信息")]
        public string Description { get; set; }

        [DisplayName("产品ID")]
        [Category("产品信息")]
        public string ProductId { get; set; }

        [DisplayName("产品名称")]
        [Category("产品信息")]
        public string ProductName { get; set; }

        [DisplayName("创建时间")]
        [Category("系统信息")]
        public DateTime CreateTime { get; set; }

        [DisplayName("状态")]
        [Category("系统信息")]
        public string Status { get; set; }
    }

    public class ProcessDisplay
    {
        [DisplayName("工艺流程ID")]
        [Category("基本信息")]
        public string ProcessId { get; set; }

        [DisplayName("版本")]
        [Category("基本信息")]
        public string Version { get; set; }

        [DisplayName("工艺包ID")]
        [Category("关联信息")]
        public string PackageId { get; set; }

        [DisplayName("描述")]
        [Category("基本信息")]
        public string Description { get; set; }

        [DisplayName("工艺类型")]
        [Category("配置信息")]
        public string ProductionType { get; set; }

        [DisplayName("顺序")]
        [Category("配置信息")]
        public int Sequence { get; set; }

        [DisplayName("当前使用")]
        [Category("状态信息")]
        public bool IsCurrentlyUsed { get; set; }
    }

    public class RouteDisplay
    {
        [DisplayName("工艺路线ID")]
        [Category("基本信息")]
        public string RouteId { get; set; }

        [DisplayName("工位编号")]
        [Category("基本信息")]
        public string StationId { get; set; }

        [DisplayName("版本")]
        [Category("基本信息")]
        public string Version { get; set; }

        [DisplayName("工艺流程ID")]
        [Category("关联信息")]
        public string ProcessId { get; set; }

        [DisplayName("工艺说明")]
        [Category("基本信息")]
        public string Description { get; set; }

        [DisplayName("顺序")]
        [Category("配置信息")]
        public int Sequence { get; set; }

        [DisplayName("工位类型")]
        [Category("配置信息")]
        public string StationType { get; set; }
        
        [DisplayName("创建时间")]
        [Category("系统信息")]
        public DateTime CreateTime { get; set; }

        [DisplayName("状态")]
        [Category("系统信息")]
        public string Status { get; set; }
    }

    #endregion
} 