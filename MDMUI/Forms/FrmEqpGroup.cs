using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using MDMUI.Model;
using MDMUI.BLL;
using MDMUI.Utility;
using System.Linq; 
using System.Drawing.Drawing2D; // 添加圆角支持
using System.Diagnostics;

namespace MDMUI
{
    // 自定义圆角按钮类
    public class RoundButton : Button
    {
        private string toolTipTextValue;
        
        public RoundButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            this.Region = new Region(path);
            base.OnPaint(e);
        }
        
        public string ToolTipText
        {
            get { return toolTipTextValue; }
            set { toolTipTextValue = value; }
        }
    }

    public partial class FrmEqpGroup : Form
    {
        private User CurrentUser;
        private EqpGroupService eqpGroupService;
        private DataTable eqpGroupData;
        private PermissionChecker permissionChecker;

        // 声明子设备和端口相关的服务和数据源
        private SubDeviceService subDeviceService; // 假设存在 SubDeviceService
        private PortService portService;         // 假设存在 PortService
        private DataTable subDeviceData;
        private DataTable portData;

        // 加载指示器相关
        private Panel loadingPanel = null;

        public FrmEqpGroup(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            eqpGroupService = new EqpGroupService();
            permissionChecker = new PermissionChecker();
            // 初始化子设备和端口相关的服务
            subDeviceService = new SubDeviceService(); 
            portService = new PortService();         

            // 在窗体显示前完全禁用SplitContainer的初始SplitterDistance设置
            this.splitContainer1.Panel1MinSize = 50; // 减小最小尺寸要求
            this.splitContainer1.Panel2MinSize = 50;
            this.bottomSplitContainer.Panel1MinSize = 50;
            this.bottomSplitContainer.Panel2MinSize = 50;
            
            // 禁用分隔条固定，让用户可以拖动调整
            this.splitContainer1.IsSplitterFixed = false;
            this.bottomSplitContainer.IsSplitterFixed = false;
            
            // 先禁用布局，防止过早计算分割位置
            this.splitContainer1.SuspendLayout();
            this.bottomSplitContainer.SuspendLayout();
            
            // 为确保安全，设置一个绝对安全的初始位置
            try {
                this.splitContainer1.SplitterDistance = 1;
                this.bottomSplitContainer.SplitterDistance = 1;
            } catch {
                // 忽略任何初始设置错误
            }
            
            // 重新启用布局
            this.splitContainer1.ResumeLayout(false);
            this.bottomSplitContainer.ResumeLayout(false);
            
            // 添加多个事件处理，在窗体生命周期的不同阶段设置分隔位置
            this.Load += (sender, e) => {
                // Load 事件中先不设置，只准备布局
                this.PerformLayout();
                Application.DoEvents();
            };
            
            // Shown事件发生在窗体首次显示后，此时尺寸已确定
            this.Shown += new EventHandler(FrmEqpGroup_Shown);
            
            // 窗体大小改变时重新计算分割位置
            this.Resize += (sender, e) => {
                try {
                    // 只有当窗体已完全创建并可见时才设置
                    if (this.IsHandleCreated && this.Visible) {
                        SafeSetSplitterDistances();
                        FixFilterPanelButtonsSize(); // 先修复filterPanel按钮排列
                        AdjustTopPanelButtonsPosition(); // 再调整顶部面板按钮位置
                        FixSubDevicesPanelButtonsSize(); // 修复子设备面板按钮
                        FixPortsPanelButtonsSize(); // 修复端口面板按钮
                        StyleAllButtons(); // 最后应用所有按钮样式
                        
                        // 更新详细信息面板
                        if (detailsPanel != null)
                        {
                            // 如果当前被选中了数据行，更新一次详细信息面板
                            if (dgvEqpGroup.SelectedRows.Count > 0)
                            {
                                UpdateDetailsPanel(dgvEqpGroup.SelectedRows[0]);
                            }
                            // 确保详细信息面板显示在顶层
                            detailsPanel.BringToFront();
                        }
                    }
                } catch (Exception ex) {
                    Debug.WriteLine($"窗体Resize时设置SplitterDistance出错: {ex.Message}");
                }
            };
            
            // 为panelTop添加尺寸改变事件处理
            panelTop.SizeChanged += (sender, e) => {
                try {
                    AdjustTopPanelButtonsPosition();
                } catch (Exception ex) {
                    Debug.WriteLine($"面板尺寸改变时调整按钮位置出错: {ex.Message}");
                }
            };
            
            // 为filterPanel添加尺寸改变事件处理
            if (filterPanel != null)
            {
                filterPanel.SizeChanged += (sender, e) => {
                    try {
                        FixFilterPanelButtonsSize(); // 修复按钮尺寸
                        foreach (Control ctrl in filterPanel.Controls)
                        {
                            if (ctrl is Button btn)
                            {
                                StyleSpecialButton(btn);
                            }
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine($"filterPanel尺寸改变时调整按钮样式出错: {ex.Message}");
                    }
                };
            }

            this.Text = "设备组管理"; 
            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.BackColor = Color.White; 
            
            // 设置窗体样式
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // this.Icon = Properties.Resources.settings; // 资源文件不存在，先注释掉
            
            // 添加顶部装饰条
            AddTopDecorationBar();
            
            // 确保顶部按钮存在并设置正确
            EnsureTopButtonsExist();
        }

        private void ApplyModernTheme()
        {
            try
            {
                // 窗体背景色
                this.BackColor = Color.White;
                
                // 搜索区域面板样式
                if (panelTop != null)
                {
                    panelTop.BackColor = Color.FromArgb(244, 247, 251);
                    panelTop.BorderStyle = BorderStyle.None;
                    
                    // 确保顶部按钮正确显示和样式应用
                    if (btnAdd != null)
                    {
                        btnAdd.Size = new Size(73, 36);
                        btnAdd.Text = "➕ 添加";
                        btnAdd.FlatStyle = FlatStyle.Flat;
                        btnAdd.FlatAppearance.BorderSize = 0;
                        btnAdd.BackColor = Color.FromArgb(92, 184, 92); // 绿色添加按钮
                        btnAdd.ForeColor = Color.White;
                        btnAdd.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                        btnAdd.Visible = true;
                        btnAdd.Cursor = Cursors.Hand;
                        btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(112, 204, 112);
                        btnAdd.FlatAppearance.MouseDownBackColor = Color.FromArgb(72, 164, 72);
                        
                        // 添加圆角效果
                        try {
                            GraphicsPath path = new GraphicsPath();
                            ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btnAdd.Width, btnAdd.Height), 5);
                            btnAdd.Region = new Region(path);
                        } catch (Exception ex) {
                            Debug.WriteLine($"添加按钮添加圆角效果出错: {ex.Message}");
                        }
                    }
                    
                    if (btnEdit != null)
                    {
                        btnEdit.Size = new Size(73, 36);
                        btnEdit.Text = "✏️ 编辑";
                        btnEdit.FlatStyle = FlatStyle.Flat;
                        btnEdit.FlatAppearance.BorderSize = 0;
                        btnEdit.BackColor = Color.FromArgb(91, 192, 222); // 蓝色编辑按钮
                        btnEdit.ForeColor = Color.White;
                        btnEdit.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                        btnEdit.Visible = true;
                        btnEdit.Cursor = Cursors.Hand;
                        btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(111, 212, 242);
                        btnEdit.FlatAppearance.MouseDownBackColor = Color.FromArgb(71, 172, 202);
                        
                        // 添加圆角效果
                        try {
                            GraphicsPath path = new GraphicsPath();
                            ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btnEdit.Width, btnEdit.Height), 5);
                            btnEdit.Region = new Region(path);
                        } catch (Exception ex) {
                            Debug.WriteLine($"编辑按钮添加圆角效果出错: {ex.Message}");
                        }
                    }
                    
                    if (btnDelete != null)
                    {
                        btnDelete.Size = new Size(73, 36);
                        btnDelete.Text = "🗑️ 删除";
                        btnDelete.FlatStyle = FlatStyle.Flat;
                        btnDelete.FlatAppearance.BorderSize = 0;
                        btnDelete.BackColor = Color.FromArgb(217, 83, 79); // 红色删除按钮
                        btnDelete.ForeColor = Color.White;
                        btnDelete.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                        btnDelete.Visible = true;
                        btnDelete.Cursor = Cursors.Hand;
                        btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(237, 103, 99);
                        btnDelete.FlatAppearance.MouseDownBackColor = Color.FromArgb(197, 63, 59);
                        
                        // 添加圆角效果
                        try {
                            GraphicsPath path = new GraphicsPath();
                            ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btnDelete.Width, btnDelete.Height), 5);
                            btnDelete.Region = new Region(path);
                        } catch (Exception ex) {
                            Debug.WriteLine($"删除按钮添加圆角效果出错: {ex.Message}");
                        }
                    }
                    
                    if (btnRefresh != null)
                    {
                        btnRefresh.Size = new Size(73, 36);
                        btnRefresh.Text = "🔄 刷新";
                        btnRefresh.FlatStyle = FlatStyle.Flat;
                        btnRefresh.FlatAppearance.BorderSize = 0;
                        btnRefresh.BackColor = Color.FromArgb(100, 151, 177);
                        btnRefresh.ForeColor = Color.White;
                        btnRefresh.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                        btnRefresh.Visible = true;
                        btnRefresh.Cursor = Cursors.Hand;
                        btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 171, 197);
                        btnRefresh.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 131, 157);
                        
                        // 添加圆角效果
                        try {
                            GraphicsPath path = new GraphicsPath();
                            ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btnRefresh.Width, btnRefresh.Height), 5);
                            btnRefresh.Region = new Region(path);
                        } catch (Exception ex) {
                            Debug.WriteLine($"刷新按钮添加圆角效果出错: {ex.Message}");
                        }
                    }
                }
                
                // 应用数据表格样式
                if (dgvEqpGroup != null) ApplyBasicDataGridViewStyle(dgvEqpGroup);
                if (dgvSubEquipment != null) ApplyBasicDataGridViewStyle(dgvSubEquipment);
                if (dgvPorts != null) ApplyBasicDataGridViewStyle(dgvPorts);
                
                // 应用按钮样式
                if (panelSubDeviceTop != null) ApplyButtonStyleToPanel(panelSubDeviceTop);
                if (panelPortsTop != null) ApplyButtonStyleToPanel(panelPortsTop);
                
                // 美化搜索按钮
                if (btnSearch != null)
                {
                    btnSearch.FlatStyle = FlatStyle.Flat;
                    btnSearch.FlatAppearance.BorderSize = 0;
                    btnSearch.BackColor = Color.FromArgb(100, 151, 177);
                    btnSearch.ForeColor = Color.White;
                    btnSearch.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                    btnSearch.Size = new Size(80, 32);
                    btnSearch.Cursor = Cursors.Hand;
                    btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 171, 197);
                    btnSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 131, 157);
                    btnSearch.Text = "🔍 搜索";
                    
                    // 添加圆角效果
                    try {
                        GraphicsPath path = new GraphicsPath();
                        path.AddRoundedRectangle(new Rectangle(0, 0, btnSearch.Width, btnSearch.Height), 5);
                        btnSearch.Region = new Region(path);
                    } catch (Exception ex) {
                        Debug.WriteLine($"搜索按钮添加圆角效果出错: {ex.Message}");
                    }
                }
                
                // 美化搜索区域文本框和下拉框
                if (panelTop != null)
                {
                    foreach (Control ctl in panelTop.Controls)
                    {
                        if (ctl is TextBox txt)
                        {
                            txt.BorderStyle = BorderStyle.FixedSingle;
                            txt.Font = new Font("Microsoft YaHei UI", 10F);
                            txt.BackColor = Color.White;
                            txt.ForeColor = Color.FromArgb(40, 40, 40);
                        }
                        else if (ctl is ComboBox cmb)
                        {
                            cmb.FlatStyle = FlatStyle.Flat;
                            cmb.Font = new Font("Microsoft YaHei UI", 10F);
                            cmb.BackColor = Color.White;
                            cmb.ForeColor = Color.FromArgb(40, 40, 40);
                        }
                        else if (ctl is Label lbl)
                        {
                            lbl.Font = new Font("Microsoft YaHei UI", 10F);
                            lbl.ForeColor = Color.FromArgb(60, 60, 60);
                        }
                    }
                }
                
                // 为分组添加样式 - 注释掉引用不存在控件的代码
                // if (grpEqpGroup != null) ApplyGroupBoxStyle(grpEqpGroup);
                // if (grpSubDevice != null) ApplyGroupBoxStyle(grpSubDevice);
                // if (grpPort != null) ApplyGroupBoxStyle(grpPort);
                
                // 设置窗体跟踪鼠标移动的事件
                ApplyAnimationEffects();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"应用现代主题时出错: {ex.Message}");
                // 失败时不要中断整个应用程序
            }
        }
        
        private void ApplyGroupBoxStyle(GroupBox grp)
        {
            if (grp == null) return;
            
            grp.FlatStyle = FlatStyle.Flat;
            grp.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            grp.ForeColor = Color.FromArgb(100, 151, 177);
            grp.BackColor = Color.FromArgb(246, 248, 250);
        }

        private void ApplyAnimationEffects()
        {
            try
            {
                // 添加轻微的按钮移动动画
                foreach (Control ctl in this.Controls)
                {
                    if (ctl is Panel panel)
                    {
                        foreach (Control panelCtl in panel.Controls)
                        {
                            if (panelCtl is Button btn)
                            {
                                try
                                {
                                    // 保存按钮原始位置
                                    Point originalLocation = btn.Location;
                                    
                                    // 鼠标进入按钮时轻微移动
                                    btn.MouseEnter += (s, e) => {
                                        try {
                                            btn.Location = new Point(originalLocation.X, originalLocation.Y - 2);
                                        } catch {}
                                    };
                                    
                                    // 鼠标离开按钮时恢复原位
                                    btn.MouseLeave += (s, e) => {
                                        try {
                                            btn.Location = originalLocation;
                                        } catch {}
                                    };
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"为按钮 {btn.Name} 添加动画效果时出错: {ex.Message}");
                                    // 跳过此按钮，继续处理其他按钮
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"应用动画效果时出错: {ex.Message}");
                // 失败时不要中断整个应用程序
            }
        }
        
        private void ApplyButtonStyleToPanel(Panel panel)
        {
            foreach (Control ctl in panel.Controls)
            {
                if (ctl is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0; // 无边框
                    btn.BackColor = Color.FromArgb(100, 151, 177); // 蓝色按钮背景
                    btn.ForeColor = Color.White; // 白色文字
                    btn.Padding = new Padding(5);
                    btn.Margin = new Padding(3);
                    btn.Size = new Size(32, 32); // 略微增大按钮
                    btn.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold); 
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.Cursor = Cursors.Hand; // 鼠标指针变为手形
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 171, 197); // 鼠标悬停颜色
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 131, 157); // 鼠标按下颜色
                    
                    // 特殊处理添加、编辑、删除按钮
                    if (btn.Text == "+" || btn.Name == "btnAdd" || btn.Name == "btnAddSubDevice" || btn.Name == "btnAddPort")
                    {
                        btn.Text = "➕";
                        btn.BackColor = Color.FromArgb(92, 184, 92); // 绿色添加按钮
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(112, 204, 112);
                        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(72, 164, 72);
                        btn.Tag = "添加";
                        // 使用ToolTip类而非ToolTipText属性
                        ToolTip tt = new ToolTip();
                        tt.SetToolTip(btn, "添加新记录");
                    }
                    else if (btn.Text == "✎" || btn.Name == "btnEdit" || btn.Name == "btnEditSubDevice" || btn.Name == "btnEditPort")
                    {
                        btn.Text = "✏️";
                        btn.BackColor = Color.FromArgb(91, 192, 222); // 蓝色编辑按钮
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(111, 212, 242);
                        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(71, 172, 202);
                        btn.Tag = "编辑";
                        // 使用ToolTip类而非ToolTipText属性
                        ToolTip tt = new ToolTip();
                        tt.SetToolTip(btn, "编辑选中记录");
                    }
                    else if (btn.Text == "-" || btn.Name == "btnDelete" || btn.Name == "btnDeleteSubDevice" || btn.Name == "btnDeletePort")
                    {
                        btn.Text = "🗑️";
                        btn.BackColor = Color.FromArgb(217, 83, 79); // 红色删除按钮
                        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(237, 103, 99);
                        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(197, 63, 59);
                        btn.Tag = "删除";
                        // 使用ToolTip类而非ToolTipText属性
                        ToolTip tt = new ToolTip();
                        tt.SetToolTip(btn, "删除选中记录");
                    }
                }
            }
        }

        private void ApplyPermissions()
        {
            bool canView = permissionChecker.HasPermission(CurrentUser.Id, "equipment_group", "view") || CurrentUser.RoleName == "超级管理员";
            bool canAdd = permissionChecker.HasPermission(CurrentUser.Id, "equipment_group", "add") || CurrentUser.RoleName == "超级管理员";
            bool canEdit = permissionChecker.HasPermission(CurrentUser.Id, "equipment_group", "edit") || CurrentUser.RoleName == "超级管理员";
            bool canDelete = permissionChecker.HasPermission(CurrentUser.Id, "equipment_group", "delete") || CurrentUser.RoleName == "超级管理员";

            btnAdd.Enabled = canAdd;
            btnEdit.Enabled = canEdit;
            btnDelete.Enabled = canDelete;
            
            if (!canView)
            {
                 MessageBox.Show("您没有查看设备组的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 // 在实际应用中，可能需要更早地阻止访问或不加载数据
            }

            // 添加子设备和端口的权限控制 (假设权限模块名为 "sub_device" 和 "port_config")
            bool canManageSubDevices = permissionChecker.HasPermission(CurrentUser.Id, "sub_device", "manage") || CurrentUser.RoleName == "超级管理员";
            btnAddSubDevice.Enabled = canManageSubDevices && canAdd; // 通常添加子设备也需要父项的编辑权限
            btnEditSubDevice.Enabled = canManageSubDevices && canEdit;
            btnDeleteSubDevice.Enabled = canManageSubDevices && canDelete;
            // dgvSubEquipment.Enabled = canView; // DataGridView 本身是否启用应基于查看权限

            bool canManagePorts = permissionChecker.HasPermission(CurrentUser.Id, "port_config", "manage") || CurrentUser.RoleName == "超级管理员";
            btnAddPort.Enabled = canManagePorts && canAdd;
            btnEditPort.Enabled = canManagePorts && canEdit;
            btnDeletePort.Enabled = canManagePorts && canDelete;
            // dgvPorts.Enabled = canView;
        }

        private void ConfigureDataGridView()
        {
            dgvEqpGroup.AutoGenerateColumns = false; 
            dgvEqpGroup.Columns.Clear(); 
            dgvEqpGroup.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 
            dgvEqpGroup.MultiSelect = false; 
            dgvEqpGroup.AllowUserToAddRows = false; 
            dgvEqpGroup.AllowUserToDeleteRows = false; 
            dgvEqpGroup.ReadOnly = true; 
            dgvEqpGroup.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 

            dgvEqpGroup.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEqpGroup.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font.FontFamily, 10F, FontStyle.Bold); 
            dgvEqpGroup.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlLight;
            dgvEqpGroup.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
            dgvEqpGroup.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240); 
            dgvEqpGroup.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            dgvEqpGroup.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            dgvEqpGroup.DefaultCellStyle.WrapMode = DataGridViewTriState.False; 
            dgvEqpGroup.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            AddColumn(dgvEqpGroup, "eqp_group_id", "设备组编号", 100, align: DataGridViewContentAlignment.MiddleCenter);
            AddColumn(dgvEqpGroup, "eqp_group_type", "设备组类型", 100);
            AddColumn(dgvEqpGroup, "eqp_group_description", "设备组说明", 250);
            AddColumn(dgvEqpGroup, "FactoryName", "所属工厂", 120);
            AddColumn(dgvEqpGroup, "event_user", "最后操作用户", 100, align: DataGridViewContentAlignment.MiddleCenter);
            AddColumn(dgvEqpGroup, "event_remark", "最后操作备注", 200);
            AddColumn(dgvEqpGroup, "edit_time", "最后编辑时间", 140, "yyyy-MM-dd HH:mm:ss", align: DataGridViewContentAlignment.MiddleCenter);
            AddColumn(dgvEqpGroup, "create_time", "创建时间", 140, "yyyy-MM-dd HH:mm:ss", align: DataGridViewContentAlignment.MiddleCenter);
            AddColumn(dgvEqpGroup, "event_type", "最后操作类型", 80, align: DataGridViewContentAlignment.MiddleCenter);
            AddColumn(dgvEqpGroup, "factory_id", "工厂ID", 80, isVisible: false); 

            DataGridViewTextBoxColumn historyLinkCol = new DataGridViewTextBoxColumn();
            historyLinkCol.Name = "colHistoryLink"; 
            historyLinkCol.HeaderText = "历史";
            historyLinkCol.Width = 60; 
            historyLinkCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; 
            historyLinkCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            historyLinkCol.DefaultCellStyle.ForeColor = Color.Blue; 
            historyLinkCol.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Underline); 
            historyLinkCol.ReadOnly = true; 
            historyLinkCol.ToolTipText = "点击查看历史记录";
            historyLinkCol.Visible = true; 
            
            dgvEqpGroup.CellContentClick += dgvEqpGroup_CellContentClick;
            dgvEqpGroup.CellFormatting += DgvEqpGroup_CellFormatting; 
            
            // 添加乱码处理事件
            dgvEqpGroup.CellFormatting += DataGridView_CellFormatting;

            dgvEqpGroup.Columns.Insert(0, historyLinkCol);
        }
        
        // 实现 ConfigureSubDeviceDataGridView 方法
        private void ConfigureSubDeviceDataGridView()
        {
            dgvSubEquipment.AutoGenerateColumns = false;
            dgvSubEquipment.Columns.Clear();
            // 示例列，请根据实际情况修改 DataPropertyName 和 HeaderText
            AddColumn(dgvSubEquipment, "sub_device_id", "子设备ID", 100);
            AddColumn(dgvSubEquipment, "sub_device_name", "子设备名称", 150);
            AddColumn(dgvSubEquipment, "sub_device_type", "类型", 80);
            // AddColumn(dgvSubEquipment, "ip_address", "IP地址", 120); 
            // ... 其他需要的子设备列 ...
            AddColumn(dgvSubEquipment, "eqp_group_id", "所属组ID", 80, isVisible: false); // 用于关联
            
            // 添加单元格格式化事件，处理可能出现的乱码
            dgvSubEquipment.CellFormatting += DataGridView_CellFormatting;
        }

        // 实现 ConfigurePortsDataGridView 方法
        private void ConfigurePortsDataGridView()
        {
            dgvPorts.AutoGenerateColumns = false;
            dgvPorts.Columns.Clear();
            // 示例列，请根据实际情况修改 DataPropertyName 和 HeaderText
            AddColumn(dgvPorts, "port_id", "端口ID", 80);
            AddColumn(dgvPorts, "port_name", "端口名称", 120);
            AddColumn(dgvPorts, "port_type", "端口类型", 100);
            AddColumn(dgvPorts, "port_number", "端口地址", 60, align: DataGridViewContentAlignment.MiddleCenter);
            AddColumn(dgvPorts, "protocol", "配置", 80);
            // ... 其他需要的端口列 ...
            AddColumn(dgvPorts, "parent_device_id", "所属设备ID", 80, isVisible: false); // 用于关联子设备
            
            // 添加单元格格式化事件，处理可能出现的乱码
            dgvPorts.CellFormatting += DataGridView_CellFormatting;
        }
        
        // 处理DataGridView单元格中的乱码
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                string value = e.Value.ToString();
                
                // 检测可能的乱码
                if (value.Contains("?") || value.Contains("�") || 
                    value.Contains("\\u") || value == "????" || 
                    value.StartsWith("??") || value.EndsWith("??"))
                {
                    e.Value = "[数据错误]";
                    e.FormattingApplied = true;
                    
                    // 设置单元格样式为灰色斜体，提示用户这是无效数据
                    DataGridView dgv = sender as DataGridView;
                    if (dgv != null && e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font(dgv.Font, FontStyle.Italic);
                    }
                }
            }
        }

        // AddColumn 辅助方法保持不变
        private void AddColumn(DataGridView dgv, string dataPropertyName, string headerText, int width, string format = null, bool isVisible = true, DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = dataPropertyName;
            column.HeaderText = headerText;
            column.Name = "col" + dataPropertyName; 
            column.Width = width;
            column.DefaultCellStyle.Alignment = align;
            if (!string.IsNullOrEmpty(format))
            {
                column.DefaultCellStyle.Format = format;
            }
            column.Visible = isVisible;
            dgv.Columns.Add(column);
        }

        private void LoadGroupFilterComboBox()
        {
            // 确保下拉框在下拉时显示宽度足够
            if (cmbGroupTypeFilter != null)
            {
                cmbGroupTypeFilter.DropDown += (s, e) => {
                    ComboBox cmb = s as ComboBox;
                    if (cmb != null)
                    {
                        // 计算所有项的最大宽度
                        int maxWidth = 0;
                        using (Graphics g = cmb.CreateGraphics())
                        {
                            foreach (var item in cmb.Items)
                            {
                                string text = cmb.GetItemText(item);
                                int width = (int)g.MeasureString(text, cmb.Font).Width;
                                if (width > maxWidth)
                                    maxWidth = width;
                            }
                        }
                        // 设置足够宽的下拉宽度
                        cmb.DropDownWidth = maxWidth + 50;
                    }
                };
            }
            
             try
             {
                 List<EqpGroup> groups = eqpGroupService.GetAllEqpGroupsForFilter();
                 EqpGroup allOption = new EqpGroup { EqpGroupId = "All", EqpGroupDescription = "(全部)" }; 
                 groups.Insert(0, allOption); 

                 cmbGroupTypeFilter.DataSource = groups;
                 cmbGroupTypeFilter.DisplayMember = "DisplayInfo"; 
                 cmbGroupTypeFilter.ValueMember = "EqpGroupId"; 
                 
                 // 先检查Items数量再设置SelectedIndex
                 if (cmbGroupTypeFilter.Items.Count > 0)
                 {
                     cmbGroupTypeFilter.SelectedIndex = 0;
                 }
                 
                 AdjustComboBoxDropDownWidth(cmbGroupTypeFilter);
             }
             catch (Exception ex)
             {
                 MessageBox.Show("加载设备组筛选列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 
                 // 创建一个新的本地数据源
                 List<EqpGroup> fallbackList = new List<EqpGroup> { new EqpGroup { EqpGroupId = "All", EqpGroupDescription = "(全部)" } };
                 
                 cmbGroupTypeFilter.DataSource = fallbackList;
                 cmbGroupTypeFilter.DisplayMember = "DisplayInfo";
                 cmbGroupTypeFilter.ValueMember = "EqpGroupId";
                 
                 // 先检查Items数量再设置SelectedIndex
                 if (cmbGroupTypeFilter.Items.Count > 0)
                 {
                     cmbGroupTypeFilter.SelectedIndex = 0;
                 }
                 
                 AdjustComboBoxDropDownWidth(cmbGroupTypeFilter);
             }
        }

        private void AdjustComboBoxDropDownWidth(ComboBox comboBox)
        {
            int maxWidth = 0;
            using (Graphics g = comboBox.CreateGraphics())
            {
                foreach (var item in comboBox.Items)
                {
                    string displayText = string.Empty;
                    if (item is EqpGroup eqpItem)
                    {
                        displayText = eqpItem.DisplayInfo;
                    }
                    else if (comboBox.DisplayMember != null && item.GetType().GetProperty(comboBox.DisplayMember) != null)
                    {
                        displayText = comboBox.GetItemText(item);
                    }
                    else
                    {
                        displayText = item.ToString(); 
                    }
                    
                    int itemWidth = (int)g.MeasureString(displayText, comboBox.Font).Width;
                    if (itemWidth > maxWidth)
                    {
                        maxWidth = itemWidth;
                    }
                }
            }
            comboBox.DropDownWidth = Math.Max(maxWidth + SystemInformation.VerticalScrollBarWidth + 50, 500); // 大幅增加下拉框宽度
        }

        // 窗体大小改变事件处理
        private void FrmEqpGroup_Resize(object sender, EventArgs e)
        {
            try
            {
                // 如果详细信息面板存在，调整位置
                if (detailsPanel != null)
                {
                    // 如果当前被选中了数据行，更新一次详细信息面板
                    if (dgvEqpGroup.SelectedRows.Count > 0)
                    {
                        UpdateDetailsPanel(dgvEqpGroup.SelectedRows[0]);
                    }
                }
                
                // 确保按钮位置正确
                AdjustTopPanelButtonsPosition();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"窗体大小改变时出错: {ex.Message}");
            }
        }

        private void LoadData()
        {
            try
            {
                string filterGroupId = txtGroupIdSearch.Text.Trim();
                string filterGroupType = cmbGroupTypeFilter.SelectedValue?.ToString();

                if (filterGroupType == "All")
                {
                    filterGroupType = null;
                }

                // 记录当前选中的行，以便刷新后尝试恢复
                string selectedEqpGroupId = null;
                if (dgvEqpGroup.SelectedRows.Count > 0)
                {
                    selectedEqpGroupId = dgvEqpGroup.SelectedRows[0].Cells["coleqp_group_id"].Value?.ToString();
                }

                eqpGroupData = eqpGroupService.GetEqpGroupList(filterGroupType, filterGroupId);
                dgvEqpGroup.DataSource = eqpGroupData;

                // 尝试恢复之前的选中行
                if (selectedEqpGroupId != null)
                {
                    foreach (DataGridViewRow row in dgvEqpGroup.Rows)
                    {
                        if (row.Cells["coleqp_group_id"].Value?.ToString() == selectedEqpGroupId)
                        {
                            row.Selected = true;
                            dgvEqpGroup.CurrentCell = row.Cells[0]; // 将当前单元格设为选中行的第一列
                            break;
                        }
                    }
                }
                // 如果没有选中行（或者刷新后之前的行没了），则清空子表
                if (dgvEqpGroup.SelectedRows.Count == 0) 
                {
                    dgvSubEquipment.DataSource = null;
                    dgvPorts.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvEqpGroup_SelectionChanged(object sender, EventArgs e)
        {
            // 实现选中设备组后，加载对应的子设备和端口信息
            if (dgvEqpGroup.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvEqpGroup.SelectedRows[0];
                string selectedEqpGroupId = selectedRow.Cells["coleqp_group_id"].Value?.ToString();
                
                // 更新右侧详细信息面板
                UpdateDetailsPanel(selectedRow);
                
                if (!string.IsNullOrEmpty(selectedEqpGroupId))
                {
                    LoadSubDeviceData(selectedEqpGroupId);
                    // 通常端口是属于子设备的，所以加载端口的逻辑可能依赖于子设备的选择
                    // 这里暂时清空端口列表，或者您可以修改为默认加载第一个子设备的端口
                    dgvPorts.DataSource = null; 
                    // 或者，如果端口也可以直接关联到设备组，则像下面这样加载：
                    // LoadPortData(selectedEqpGroupId, null); // 第二个参数表示没有选中子设备
                }
            }
            else
            {
                // 清空子设备和端口的DataGridView
                dgvSubEquipment.DataSource = null;
                dgvPorts.DataSource = null;
                
                // 清空详细信息面板或显示默认提示
                if (detailsTablePanel != null)
                {
                    detailsTablePanel.Controls.Clear();
                    Label lblNoSelection = new Label();
                    lblNoSelection.Text = "请选择一个设备组查看详细信息";
                    lblNoSelection.Font = new Font("Microsoft YaHei UI", 10F);
                    lblNoSelection.ForeColor = Color.Gray;
                    lblNoSelection.TextAlign = ContentAlignment.MiddleCenter;
                    lblNoSelection.Dock = DockStyle.Fill;
                    detailsTablePanel.Controls.Add(lblNoSelection, 0, 0);
                    detailsTablePanel.SetColumnSpan(lblNoSelection, 2);
                }
            }
        }

        // 实现 LoadSubDeviceData 方法
        private void LoadSubDeviceData(string eqpGroupId)
        {
            try
            {
                Debug.WriteLine($"开始加载设备组[{eqpGroupId}]的子设备数据");
                if (string.IsNullOrEmpty(eqpGroupId))
                {
                    Debug.WriteLine("警告：尝试加载子设备但设备组ID为空");
                    return;
                }
                
                subDeviceData = subDeviceService.GetSubDevicesByGroupId(eqpGroupId); // 假设此方法存在
                
                Debug.WriteLine($"获取到 {(subDeviceData?.Rows.Count ?? 0)} 条子设备数据");
                
                // 确保返回的DataTable有效且包含必要的列
                if (subDeviceData == null)
                {
                    Debug.WriteLine("子设备服务返回的数据表为空，创建新表");
                    subDeviceData = new DataTable();
                }
                
                // 确保列存在
                EnsureColumnsExist(subDeviceData, new string[] { 
                    "sub_device_id", "sub_device_name", "sub_device_type" 
                });
                
                // 清理可能的乱码数据
                CleanDataTableText(subDeviceData);
                
                dgvSubEquipment.DataSource = subDeviceData;
                
                // 默认选中子设备列表的第一行（如果存在），并加载其端口
                if (dgvSubEquipment.Rows.Count > 0)
                {
                    dgvSubEquipment.Rows[0].Selected = true;
                    string selectedSubDeviceId = dgvSubEquipment.Rows[0].Cells["colsub_device_id"].Value?.ToString();
                    if (!string.IsNullOrEmpty(selectedSubDeviceId))
                    {
                        LoadPortData(selectedSubDeviceId);
                    }
                    else 
                    {
                        Debug.WriteLine("第一行子设备ID为空，无法加载端口");
                        dgvPorts.DataSource = null;
                    }
                }
                else
                {
                    Debug.WriteLine("无子设备数据，清空端口列表");
                    dgvPorts.DataSource = null; // 如果没有子设备，清空端口列表
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"加载子设备数据异常: {ex.GetType().Name}: {ex.Message}");
                Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
                
                // 不向用户显示完整的技术堆栈，只显示简单的错误消息
                MessageBox.Show($"加载子设备数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // 确保UI不会因异常而显示不正确的状态
                dgvSubEquipment.DataSource = null;
                dgvPorts.DataSource = null;
            }
        }
        
        // 添加 dgvSubEquipment 的 SelectionChanged 事件处理器
        private void dgvSubEquipment_SelectionChanged(object sender, EventArgs e)
        {
             if (dgvSubEquipment.SelectedRows.Count > 0)
            {
                string selectedSubDeviceId = dgvSubEquipment.SelectedRows[0].Cells["colsub_device_id"].Value?.ToString();
                if (!string.IsNullOrEmpty(selectedSubDeviceId))
                {
                    LoadPortData(selectedSubDeviceId);
                }
            }
            else
            {
                dgvPorts.DataSource = null;
            }
        }

        // 实现 LoadPortData 方法 (根据子设备ID加载)
        private void LoadPortData(string subDeviceId) 
        {
            try
            {
                Debug.WriteLine($"开始加载子设备[{subDeviceId}]的端口数据");
                if (string.IsNullOrEmpty(subDeviceId))
                {
                    Debug.WriteLine("警告：尝试加载端口但子设备ID为空");
                    return;
                }
                
                portData = portService.GetPortsByParentDeviceId(subDeviceId); 
                Debug.WriteLine($"获取到 {(portData?.Rows.Count ?? 0)} 条端口数据");
                
                // 确保返回的DataTable有效且包含必要的列
                if (portData == null)
                {
                    Debug.WriteLine("端口服务返回的数据表为空，创建新表");
                    portData = new DataTable();
                }
                
                // 确保列存在
                EnsureColumnsExist(portData, new string[] { 
                    "port_id", "port_name", "port_type", "port_number", "protocol" 
                });
                
                // 清理可能的乱码数据
                CleanDataTableText(portData);
                
                dgvPorts.DataSource = portData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"加载端口数据异常: {ex.GetType().Name}: {ex.Message}");
                Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
                
                // 不向用户显示完整的技术堆栈，只显示简单的错误消息
                MessageBox.Show($"加载端口数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                dgvPorts.DataSource = null; // 确保UI显示正确的状态
            }
        }

        // 辅助方法：确保表包含必要的列
        private void EnsureColumnsExist(DataTable dt, string[] requiredColumns)
        {
            if (dt == null) return;
            
            foreach (string column in requiredColumns)
            {
                if (!dt.Columns.Contains(column))
                {
                    Debug.WriteLine($"添加缺失列: {column}");
                    dt.Columns.Add(column);
                }
            }
        }
        
        // 清理数据表中可能存在的乱码
        private void CleanDataTableText(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            
            // 遍历所有行和列
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (row[col] != null && row[col] != DBNull.Value)
                    {
                        string value = row[col].ToString();
                        
                        // 检测并替换可能的乱码
                        if (value.Contains("?") || value.Contains("�") || 
                            value.Contains("\\u") || value == "????")
                        {
                            row[col] = "[未知数据]";
                        }
                    }
                }
            }
        }

        private void dgvEqpGroup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; 

            if (dgvEqpGroup.Columns[e.ColumnIndex].Name == "colHistoryLink")
            {
                try
                {
                    string eqpGroupId = dgvEqpGroup.Rows[e.RowIndex].Cells["coleqp_group_id"].Value?.ToString();
                    if (!string.IsNullOrEmpty(eqpGroupId))
                    {
                        FrmEqpGroupHis historyForm = new FrmEqpGroupHis(eqpGroupId);
                        historyForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("打开历史记录窗体时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(); 
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtGroupIdSearch.Clear(); 
            if (cmbGroupTypeFilter.Items.Count > 0) cmbGroupTypeFilter.SelectedIndex = 0; 
            LoadData(); 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmEqpGroupEdit editForm = new FrmEqpGroupEdit(CurrentUser); 
            DialogResult result = editForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadData(); 
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEqpGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要编辑的设备组。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow selectedRow = dgvEqpGroup.SelectedRows[0];
            EqpGroup groupToEdit = new EqpGroup
            {
                EqpGroupId = selectedRow.Cells["coleqp_group_id"].Value?.ToString(),
                EqpGroupType = selectedRow.Cells["coleqp_group_type"].Value?.ToString(),
                EqpGroupDescription = selectedRow.Cells["coleqp_group_description"].Value?.ToString(),
                FactoryId = selectedRow.Cells["colfactory_id"].Value?.ToString(),
            };
            FrmEqpGroupEdit editForm = new FrmEqpGroupEdit(groupToEdit, CurrentUser);
            DialogResult result = editForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadData(); 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEqpGroup.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要删除的设备组。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string eqpGroupId = dgvEqpGroup.SelectedRows[0].Cells["coleqp_group_id"].Value?.ToString();
            string eqpGroupDescription = dgvEqpGroup.SelectedRows[0].Cells["coleqp_group_description"].Value?.ToString();
            if (string.IsNullOrEmpty(eqpGroupId))
            {
                MessageBox.Show("无法获取选定设备组的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult confirmResult = MessageBox.Show($"确定要删除设备组 '{eqpGroupId} - {eqpGroupDescription}' 吗？\n此操作不可恢复，且会同时删除其下的所有子设备和端口！", 
                                                      "确认删除", 
                                                      MessageBoxButtons.YesNo, 
                                                      MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // TODO: 删除操作可能需要先检查子设备/端口，或由 BLL/DAL 处理级联删除
                    bool success = eqpGroupService.DeleteEqpGroup(eqpGroupId, CurrentUser);
                    if (success)
                    {
                        // 直接刷新数据，不显示重复消息
                        LoadData(); 
                    }
                    else
                    {
                        MessageBox.Show("设备组删除失败。可能原因：设备组不存在或操作失败。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除设备组时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Filter_Changed(object sender, EventArgs e)
        { 
            // 通常由搜索按钮或回车触发，此处留空或按需实现实时筛选
        }

        private void txtGroupIdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData(); 
                e.SuppressKeyPress = true; 
            }
        }

        private void DgvEqpGroup_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvEqpGroup.Columns[e.ColumnIndex].Name == "colHistoryLink" && e.Value == null)
            {
                e.Value = "查看";
                e.CellStyle.ForeColor = Color.Blue;
                e.CellStyle.Font = new Font(dgvEqpGroup.Font, FontStyle.Underline);
            }
        }

        // --- 子设备按钮事件处理程序 (占位符，后续需要实现) ---
        // 防止重复执行的标记
        private bool isProcessingAddSubDevice = false;
        private bool isProcessingEditSubDevice = false;
        private bool isProcessingDeleteSubDevice = false;
        private bool isProcessingAddPort = false;
        private bool isProcessingEditPort = false;
        private bool isProcessingDeletePort = false;
        
        /// <summary>
        /// 移除所有按钮事件处理器，防止重复触发
        /// </summary>
        private void RemoveAllButtonEventHandlers()
        {
            Debug.WriteLine("开始移除所有按钮事件处理器");
            
            // 获取窗体上的所有控件
            var allControls = this.GetAllControls();
            
            foreach (Control ctrl in allControls)
            {
                if (ctrl is Button)
                {
                    Button btn = (Button)ctrl;
                    
                    // 根据按钮名称移除相应的事件处理器
                    if (btn.Name == "btnAddSubDevice")
                    {
                        Debug.WriteLine("移除 btnAddSubDevice 的事件处理器");
                        btn.Click -= btnAddSubDevice_Click;
                    }
                    else if (btn.Name == "btnEditSubDevice")
                    {
                        Debug.WriteLine("移除 btnEditSubDevice 的事件处理器");
                        btn.Click -= btnEditSubDevice_Click;
                    }
                    else if (btn.Name == "btnDeleteSubDevice")
                    {
                        Debug.WriteLine("移除 btnDeleteSubDevice 的事件处理器");
                        btn.Click -= btnDeleteSubDevice_Click;
                    }
                    else if (btn.Name == "btnAddPort")
                    {
                        Debug.WriteLine("移除 btnAddPort 的事件处理器");
                        btn.Click -= btnAddPort_Click;
                    }
                    else if (btn.Name == "btnEditPort")
                    {
                        Debug.WriteLine("移除 btnEditPort 的事件处理器");
                        btn.Click -= btnEditPort_Click;
                    }
                    else if (btn.Name == "btnDeletePort")
                    {
                        Debug.WriteLine("移除 btnDeletePort 的事件处理器");
                        btn.Click -= btnDeletePort_Click;
                    }
                }
            }
            
            Debug.WriteLine("所有按钮事件处理器移除完成");
        }
        
        /// <summary>
        /// 递归获取控件及其子控件
        /// </summary>
        private List<Control> GetAllControls()
        {
            List<Control> allControls = new List<Control>();
            GetAllControlsRecursive(this, allControls);
            return allControls;
        }
        
        /// <summary>
        /// 递归获取控件及其子控件的辅助方法
        /// </summary>
        private void GetAllControlsRecursive(Control parent, List<Control> result)
        {
            foreach (Control ctrl in parent.Controls)
            {
                result.Add(ctrl);
                
                // 递归处理子控件
                if (ctrl.Controls.Count > 0)
                {
                    GetAllControlsRecursive(ctrl, result);
                }
            }
        }
        
        private void btnAddSubDevice_Click(object sender, EventArgs e)
        {
            // 检查是否已经在处理中
            if (isProcessingAddSubDevice)
                return;
                
            isProcessingAddSubDevice = true;
            
            try
            {
                if (dgvEqpGroup.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择一个设备组以添加子设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            string parentEqpGroupId = dgvEqpGroup.SelectedRows[0].Cells["coleqp_group_id"].Value?.ToString();
            
            // 打开子设备添加窗体
            // 注意: FrmSubDeviceEdit类在Windows Forms设计器环境中编译时可能报错
            // 使用前需要先将类添加到项目并确保它正确编译
            using (var subDeviceForm = new FrmSubDeviceEdit(parentEqpGroupId, CurrentUser))
            {
                if (subDeviceForm.ShowDialog() == DialogResult.OK)
                {
                    // 刷新子设备列表
                    LoadSubDeviceData(parentEqpGroupId);
                }
            }
            }
            finally
            {
                isProcessingAddSubDevice = false;
            }
        }

        private void btnEditSubDevice_Click(object sender, EventArgs e)
        {
            // 检查是否已经在处理中
            if (isProcessingEditSubDevice)
                return;
                
            isProcessingEditSubDevice = true;
            
            try
            {
                if (dgvSubEquipment.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要编辑的子设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            string subDeviceId = dgvSubEquipment.SelectedRows[0].Cells["colsub_device_id"].Value?.ToString();
            
            // 打开子设备编辑窗体
            using (FrmSubDeviceEdit subDeviceForm = new FrmSubDeviceEdit(subDeviceId, CurrentUser, FormMode.Edit))
            {
                if (subDeviceForm.ShowDialog() == DialogResult.OK)
                {
                    // 刷新子设备列表
                    string parentEqpGroupId = dgvEqpGroup.SelectedRows.Count > 0 ? dgvEqpGroup.SelectedRows[0].Cells["coleqp_group_id"].Value?.ToString() : null;
                    if (parentEqpGroupId != null) LoadSubDeviceData(parentEqpGroupId);
                }
            }
            }
            finally
            {
                isProcessingEditSubDevice = false;
            }
        }

        private void btnDeleteSubDevice_Click(object sender, EventArgs e)
        {
            // 检查是否已经在处理中
            if (isProcessingDeleteSubDevice)
                return;
                
            isProcessingDeleteSubDevice = true;
            
            try
            {
                if (dgvSubEquipment.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要删除的子设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            string subDeviceId = dgvSubEquipment.SelectedRows[0].Cells["colsub_device_id"].Value?.ToString();
            string subDeviceName = dgvSubEquipment.SelectedRows[0].Cells["colsub_device_name"].Value?.ToString();

            if (string.IsNullOrEmpty(subDeviceId))
            {
                 MessageBox.Show("无法获取选定子设备的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

             DialogResult confirmResult = MessageBox.Show($"确定要删除子设备 '{subDeviceId} - {subDeviceName}' 吗？\n此操作可能不可恢复。", 
                                                       "确认删除", 
                                                       MessageBoxButtons.YesNo, 
                                                       MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // 调用 subDeviceService.DeleteSubDevice
                    bool success = subDeviceService.DeleteSubDevice(subDeviceId, CurrentUser.Id.ToString());

                    if (success)
                    {
                        // 刷新子设备列表
                        string parentEqpGroupId = dgvEqpGroup.SelectedRows.Count > 0 ? dgvEqpGroup.SelectedRows[0].Cells["coleqp_group_id"].Value?.ToString() : null;
                        if (!string.IsNullOrEmpty(parentEqpGroupId))
                        {
                            LoadSubDeviceData(parentEqpGroupId);
                        }
                        else
                        {
                             dgvSubEquipment.DataSource = null; // 如果找不到父组ID，清空列表
                             dgvPorts.DataSource = null;
                        }
                    }
                    else
                    {
                         MessageBox.Show("子设备删除失败。可能原因：子设备不存在或操作失败。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    // 捕获并显示来自 BLL 或 DAL 的异常（例如，有关联端口无法删除的提示）
                    MessageBox.Show("删除子设备时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            }
            finally
            {
                isProcessingDeleteSubDevice = false;
            }
        }

        // --- 端口按钮事件处理程序 (占位符，后续需要实现) ---
        private void btnAddPort_Click(object sender, EventArgs e)
        {
            // 检查是否已经在处理中
            if (isProcessingAddPort)
                return;
                
            isProcessingAddPort = true;
            
            try
            {
                // 确定端口是添加到设备组还是子设备？ 假设添加到子设备
                if (dgvSubEquipment.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择一个子设备以添加端口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            string parentSubDeviceId = dgvSubEquipment.SelectedRows[0].Cells["colsub_device_id"].Value?.ToString();
            
            // 打开端口添加窗体
            using (FrmPortEdit portForm = new FrmPortEdit(parentSubDeviceId, CurrentUser))
            {
                if (portForm.ShowDialog() == DialogResult.OK)
                {
                    // 刷新端口列表
                    LoadPortData(parentSubDeviceId);
                }
            }
            }
            finally
            {
                isProcessingAddPort = false;
            }
        }

        private void btnEditPort_Click(object sender, EventArgs e)
        {
            // 检查是否已经在处理中
            if (isProcessingEditPort)
                return;
                
            isProcessingEditPort = true;
            
            try
            {
                if (dgvPorts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要编辑的端口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            string portId = dgvPorts.SelectedRows[0].Cells["colport_id"].Value?.ToString();
            
            // 打开端口编辑窗体
            using (FrmPortEdit portForm = new FrmPortEdit(portId, CurrentUser, FormMode.Edit))
            {
                if (portForm.ShowDialog() == DialogResult.OK)
                {
                    // 刷新端口列表
                    string parentSubDeviceId = dgvSubEquipment.SelectedRows.Count > 0 ? dgvSubEquipment.SelectedRows[0].Cells["colsub_device_id"].Value?.ToString() : null;
                    if (parentSubDeviceId != null) LoadPortData(parentSubDeviceId);
                }
            }
            }
            finally
            {
                isProcessingEditPort = false;
            }
        }

        private void btnDeletePort_Click(object sender, EventArgs e)
        {
            // 检查是否已经在处理中
            if (isProcessingDeletePort)
                return;
                
            isProcessingDeletePort = true;
            
            try
            {
                if (dgvPorts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要删除的端口。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            string portId = dgvPorts.SelectedRows[0].Cells["colport_id"].Value?.ToString();
            string portName = dgvPorts.SelectedRows[0].Cells["colport_name"].Value?.ToString();

             if (string.IsNullOrEmpty(portId))
            {
                 MessageBox.Show("无法获取选定端口的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

            DialogResult confirmResult = MessageBox.Show($"确定要删除端口 '{portId} - {portName}' 吗？", 
                                                      "确认删除", 
                                                      MessageBoxButtons.YesNo, 
                                                      MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // 调用服务删除端口
                    bool success = portService.DeletePort(portId, CurrentUser.Id.ToString());
                    
                    if (success)
                    {
                        // 刷新端口列表
                        string parentSubDeviceId = dgvSubEquipment.SelectedRows.Count > 0 ? dgvSubEquipment.SelectedRows[0].Cells["colsub_device_id"].Value?.ToString() : null;
                        if (parentSubDeviceId != null) LoadPortData(parentSubDeviceId);
                    }
                    else
                    {
                        MessageBox.Show("端口删除失败。可能原因：端口不存在或操作失败。", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除端口时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            }
            finally
            {
                isProcessingDeletePort = false;
            }
        }

        // 安全设置SplitterDistance的方法
        // 右侧详细信息面板
        private Panel detailsPanel;
        private Label lblDetailsTitle;
        private TableLayoutPanel detailsTablePanel;
        
        // 创建右侧详细信息面板
        private void CreateDetailsPanel()
        {
            try
            {
                // 如果已存在则先移除
                if (detailsPanel != null && Controls.Contains(detailsPanel))
                {
                    Controls.Remove(detailsPanel);
                    detailsPanel.Dispose();
                }
                
                // 创建主面板
                detailsPanel = new Panel();
                detailsPanel.Name = "detailsPanel";
                detailsPanel.BorderStyle = BorderStyle.None;
                detailsPanel.BackColor = Color.FromArgb(248, 249, 250);
                detailsPanel.Dock = DockStyle.Right;
                detailsPanel.Width = 220; // 减小宽度，避免挤压其他控件
                
                // 添加阴影和边框效果
                detailsPanel.Paint += (sender, e) => {
                    // 绘制左侧边框线
                    using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                    {
                        e.Graphics.DrawLine(pen, 0, 0, 0, detailsPanel.Height);
                    }
                    
                    // 绘制上边框装饰条
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, 151, 177)))
                    {
                        e.Graphics.FillRectangle(brush, 0, 0, detailsPanel.Width, 5);
                    }
                };
                
                // 创建标题
                lblDetailsTitle = new Label();
                lblDetailsTitle.Text = "设备详细信息";  // 缩短标题
                lblDetailsTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
                lblDetailsTitle.ForeColor = Color.FromArgb(80, 80, 80);
                lblDetailsTitle.BackColor = Color.FromArgb(240, 240, 240);
                lblDetailsTitle.TextAlign = ContentAlignment.MiddleCenter;
                lblDetailsTitle.Dock = DockStyle.Top;
                lblDetailsTitle.Height = 40;
                lblDetailsTitle.Padding = new Padding(0, 5, 0, 5);
                
                // 创建TableLayoutPanel用于显示详细信息
                detailsTablePanel = new TableLayoutPanel();
                detailsTablePanel.ColumnCount = 2;
                detailsTablePanel.RowCount = 10; // 足够显示所有字段
                detailsTablePanel.Dock = DockStyle.Fill;
                detailsTablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                detailsTablePanel.BackColor = Color.White;
                detailsTablePanel.Padding = new Padding(10);
                
                // 设置列宽比例 - 调整比例以确保文本不会被截断
                detailsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
                detailsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
                
                // 设置统一的行高
                for (int i = 0; i < 10; i++)
                {
                    detailsTablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                }
                
                // 添加初始提示
                Label lblInitialHint = new Label();
                lblInitialHint.Text = "请选择一个设备组查看详细信息";
                lblInitialHint.Font = new Font("Microsoft YaHei UI", 10F);
                lblInitialHint.ForeColor = Color.Gray;
                lblInitialHint.TextAlign = ContentAlignment.MiddleCenter;
                lblInitialHint.Dock = DockStyle.Fill;
                detailsTablePanel.Controls.Add(lblInitialHint, 0, 0);
                detailsTablePanel.SetColumnSpan(lblInitialHint, 2);
                
                // 组装控件
                detailsPanel.Controls.Add(detailsTablePanel);
                detailsPanel.Controls.Add(lblDetailsTitle);
                
                // 添加到窗体
                Controls.Add(detailsPanel);
                detailsPanel.BringToFront(); // 确保显示在最前
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"创建详细信息面板时出错: {ex.Message}");
            }
        }
        
        // 更新详细信息面板内容
        private void UpdateDetailsPanel(DataGridViewRow selectedRow)
        {
            try
            {
                if (detailsPanel == null || detailsTablePanel == null || selectedRow == null)
                    return;
                
                // 清空现有控件
                detailsTablePanel.Controls.Clear();
                
                // 添加字段标签和值，确保完整显示
                AddDetailRow("组ID:", GetCellValue(selectedRow, "coleqp_group_id"), 0);
                AddDetailRow("组类型:", GetCellValue(selectedRow, "coleqp_group_type"), 1);
                AddDetailRow("组说明:", GetCellValue(selectedRow, "coleqp_group_description"), 2);
                AddDetailRow("所属工厂:", GetCellValue(selectedRow, "colFactoryName"), 3);
                AddDetailRow("工厂ID:", GetCellValue(selectedRow, "colfactory_id"), 4);
                AddDetailRow("操作用户:", GetCellValue(selectedRow, "colevent_user"), 5);
                AddDetailRow("操作类型:", GetCellValue(selectedRow, "colevent_type"), 6);
                AddDetailRow("操作备注:", GetCellValue(selectedRow, "colevent_remark"), 7);
                AddDetailRow("编辑时间:", GetCellValue(selectedRow, "coledit_time"), 8);
                AddDetailRow("创建时间:", GetCellValue(selectedRow, "colcreate_time"), 9);
                
                // 如果有额外信息，可以继续添加
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"更新详细信息面板时出错: {ex.Message}");
            }
        }
        
        // 辅助方法：添加一行详细信息
        private void AddDetailRow(string label, string value, int rowIndex)
        {
            Label lblField = new Label();
            lblField.Text = label;
            lblField.Font = new Font("Microsoft YaHei UI", 8.5F, FontStyle.Bold); // 稍微减小字体
            lblField.ForeColor = Color.FromArgb(90, 90, 90);
            lblField.TextAlign = ContentAlignment.MiddleLeft;
            lblField.Dock = DockStyle.Fill;
            lblField.Margin = new Padding(1); // 减小边距
            
            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Microsoft YaHei UI", 8.5F); // 稍微减小字体
            lblValue.ForeColor = Color.FromArgb(50, 50, 50);
            lblValue.TextAlign = ContentAlignment.MiddleLeft;
            lblValue.Dock = DockStyle.Fill;
            lblValue.Margin = new Padding(1); // 减小边距
            lblValue.AutoEllipsis = true; // 自动添加省略号
            
            // 为偶数行添加背景色，增强可读性
            if (rowIndex % 2 == 0)
            {
                lblField.BackColor = Color.FromArgb(248, 248, 248);
                lblValue.BackColor = Color.FromArgb(248, 248, 248);
            }
            
            detailsTablePanel.Controls.Add(lblField, 0, rowIndex);
            detailsTablePanel.Controls.Add(lblValue, 1, rowIndex);
        }
        
        // 辅助方法：获取单元格值并处理空值和乱码
        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            try
            {
                if (row.Cells[columnName].Value != null)
                {
                    string value = row.Cells[columnName].Value.ToString();
                    
                    // 检测可能的乱码
                    if (value.Contains("?") || value.Contains("�") || value.Contains("\\u"))
                    {
                        return "[数据错误]";
                    }
                    
                    return value;
                }
                return "-";
            }
            catch
            {
                return "-";
            }
        }
        
        private void SafeSetSplitterDistances()
        {
            try
            {
                Debug.WriteLine("开始设置SplitContainer分隔位置...");
                // 强制执行布局，确保控件尺寸已确定
                this.PerformLayout();
                
                // 检查SplitContainer是否已初始化
                if (splitContainer1 == null)
                {
                    Debug.WriteLine("警告: splitContainer1为null，无法设置分隔位置");
                    return;
                }
                
                if (bottomSplitContainer == null)
                {
                    Debug.WriteLine("警告: bottomSplitContainer为null，无法设置分隔位置");
                    return;
                }
                
                // 临时暂停布局以避免可能的循环事件
                this.splitContainer1.SuspendLayout();
                this.bottomSplitContainer.SuspendLayout();
                
                Debug.WriteLine($"splitContainer1大小: {splitContainer1.Width}x{splitContainer1.Height}");
                Debug.WriteLine($"Panel1MinSize: {splitContainer1.Panel1MinSize}, Panel2MinSize: {splitContainer1.Panel2MinSize}");
                
                // 针对splitContainer1设置分隔条位置
                if (splitContainer1 != null && splitContainer1.Width > 10)
                {
                    // 确保安全计算
                    int panel2Min = Math.Max(10, splitContainer1.Panel2MinSize);
                    int panel1Min = Math.Max(10, splitContainer1.Panel1MinSize);
                    
                    // 确保总宽度足够容纳两个面板的最小尺寸
                    if (splitContainer1.Width > (panel1Min + panel2Min + 5))
                    {
                        int maxDistance = splitContainer1.Width - panel2Min - 5; // 留出5像素安全余量
                        int minDistance = panel1Min + 5; // 留出5像素安全余量
                        
                        // 计算一个安全的值：先用30%宽度，如果不行再用50%，最后才是简单平均
                        int preferredDistance = (int)(splitContainer1.Width * 0.3);
                        int safeDistance;
                        
                        if (preferredDistance >= minDistance && preferredDistance <= maxDistance)
                        {
                            safeDistance = preferredDistance;
                        }
                        else
                        {
                            // 尝试50%
                            preferredDistance = splitContainer1.Width / 2;
                            if (preferredDistance >= minDistance && preferredDistance <= maxDistance)
                            {
                                safeDistance = preferredDistance;
                            }
                            else
                            {
                                // 用平均值
                                safeDistance = (minDistance + maxDistance) / 2;
                            }
                        }
                        
                        Debug.WriteLine($"splitContainer1 - 计算值: safeDistance={safeDistance}, 有效范围[{minDistance}-{maxDistance}]");
                        
                        // 进行最终安全检查
                        if (safeDistance >= minDistance && safeDistance <= maxDistance)
                        {
                            try {
                                splitContainer1.SplitterDistance = safeDistance;
                                Debug.WriteLine($"splitContainer1.SplitterDistance设置为: {safeDistance}");
                            } catch (Exception ex) {
                                Debug.WriteLine($"设置splitContainer1.SplitterDistance失败: {ex.Message}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"计算的分隔位置{safeDistance}超出有效范围[{minDistance}-{maxDistance}]，跳过设置");
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"splitContainer1宽度不足: {splitContainer1.Width} <= {panel1Min + panel2Min + 5}");
                    }
                }
                else
                {
                    Debug.WriteLine($"splitContainer1为null或宽度过小: {(splitContainer1 == null ? "null" : splitContainer1.Width.ToString())}");
                }
                
                Debug.WriteLine($"bottomSplitContainer大小: {bottomSplitContainer.Width}x{bottomSplitContainer.Height}");
                Debug.WriteLine($"Panel1MinSize: {bottomSplitContainer.Panel1MinSize}, Panel2MinSize: {bottomSplitContainer.Panel2MinSize}");
                
                // 针对bottomSplitContainer设置分隔条位置
                if (bottomSplitContainer != null && bottomSplitContainer.Width > 10)
                {
                    // 确保安全计算
                    int panel2Min = Math.Max(10, bottomSplitContainer.Panel2MinSize);
                    int panel1Min = Math.Max(10, bottomSplitContainer.Panel1MinSize);
                    
                    // 确保总宽度足够容纳两个面板的最小尺寸
                    if (bottomSplitContainer.Width > (panel1Min + panel2Min + 5))
                    {
                        int maxDistance = bottomSplitContainer.Width - panel2Min - 5; // 留出5像素安全余量
                        int minDistance = panel1Min + 5; // 留出5像素安全余量
                        
                        // 计算一个安全的值：先用60%宽度，如果不行再用50%，最后才是简单平均
                        int preferredDistance = (int)(bottomSplitContainer.Width * 0.6);
                        int safeDistance;
                        
                        if (preferredDistance >= minDistance && preferredDistance <= maxDistance)
                        {
                            safeDistance = preferredDistance;
                        }
                        else
                        {
                            // 尝试50%
                            preferredDistance = bottomSplitContainer.Width / 2;
                            if (preferredDistance >= minDistance && preferredDistance <= maxDistance)
                            {
                                safeDistance = preferredDistance;
                            }
                            else
                            {
                                // 用平均值
                                safeDistance = (minDistance + maxDistance) / 2;
                            }
                        }
                        
                        Debug.WriteLine($"bottomSplitContainer - 计算值: safeDistance={safeDistance}, 有效范围[{minDistance}-{maxDistance}]");
                        
                        // 进行最终安全检查
                        if (safeDistance >= minDistance && safeDistance <= maxDistance)
                        {
                            try {
                                bottomSplitContainer.SplitterDistance = safeDistance;
                                Debug.WriteLine($"bottomSplitContainer.SplitterDistance设置为: {safeDistance}");
                            } catch (Exception ex) {
                                Debug.WriteLine($"设置bottomSplitContainer.SplitterDistance失败: {ex.Message}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"计算的分隔位置{safeDistance}超出有效范围[{minDistance}-{maxDistance}]，跳过设置");
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"bottomSplitContainer宽度不足: {bottomSplitContainer.Width} <= {panel1Min + panel2Min + 5}");
                    }
                }
                else
                {
                    Debug.WriteLine($"bottomSplitContainer为null或宽度过小: {(bottomSplitContainer == null ? "null" : bottomSplitContainer.Width.ToString())}");
                }
                
                // 恢复布局处理
                this.splitContainer1.ResumeLayout(true);
                this.bottomSplitContainer.ResumeLayout(true);
                
                Debug.WriteLine("SplitContainer分隔位置设置完成");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"设置SplitterDistance时出错: {ex.Message}");
                // 失败时不抛出异常，让窗体继续加载
            }
        }

        // 添加Shown事件处理程序
        private void FrmEqpGroup_Shown(object sender, EventArgs e)
{
    try
    {
        // 先执行应用消息队列中的待处理事件
        Application.DoEvents();
        
        // 确保获得正确的窗体尺寸
        this.PerformLayout();
        
        // 确保只有一套按钮起作用 - 在窗体完全加载后最后一次移除所有事件并重新绑定
        Debug.WriteLine("窗体显示后，最后一次重新绑定按钮事件");
        RemoveAllButtonEventHandlers();
        
        // 获取所有控件
        var allControls = this.GetAllControls();
        
        // 只为子设备面板上的按钮绑定事件
        bool foundSubDeviceButtons = false;
        bool foundPortButtons = false;
        
        // 尝试查找并绑定subDevicesTitlePanel中的按钮
        foreach (Control ctrl in allControls)
        {
            if (ctrl.Name == "subDevicesTitlePanel" && ctrl is TableLayoutPanel)
            {
                // 在这个面板中查找按钮
                foreach (Control panelCtrl in ctrl.Controls)
                {
                    if (panelCtrl is Button)
                    {
                        Button btn = (Button)panelCtrl;
                        
                        if (btn.Name == "btnAddSubDevice")
                        {
                            Debug.WriteLine("Shown事件: 为subDevicesTitlePanel中的btnAddSubDevice绑定事件");
                            btn.Click += btnAddSubDevice_Click;
                            foundSubDeviceButtons = true;
                        }
                        else if (btn.Name == "btnEditSubDevice")
                        {
                            Debug.WriteLine("Shown事件: 为subDevicesTitlePanel中的btnEditSubDevice绑定事件");
                            btn.Click += btnEditSubDevice_Click;
                            foundSubDeviceButtons = true;
                        }
                        else if (btn.Name == "btnDeleteSubDevice")
                        {
                            Debug.WriteLine("Shown事件: 为subDevicesTitlePanel中的btnDeleteSubDevice绑定事件");
                            btn.Click += btnDeleteSubDevice_Click;
                            foundSubDeviceButtons = true;
                        }
                    }
                }
            }
            else if (ctrl.Name == "portsTitlePanel" && ctrl is TableLayoutPanel)
            {
                // 在这个面板中查找按钮
                foreach (Control panelCtrl in ctrl.Controls)
                {
                    if (panelCtrl is Button)
                    {
                        Button btn = (Button)panelCtrl;
                        
                        if (btn.Name == "btnAddPort")
                        {
                            Debug.WriteLine("Shown事件: 为portsTitlePanel中的btnAddPort绑定事件");
                            btn.Click += btnAddPort_Click;
                            foundPortButtons = true;
                        }
                        else if (btn.Name == "btnEditPort")
                        {
                            Debug.WriteLine("Shown事件: 为portsTitlePanel中的btnEditPort绑定事件");
                            btn.Click += btnEditPort_Click;
                            foundPortButtons = true;
                        }
                        else if (btn.Name == "btnDeletePort")
                        {
                            Debug.WriteLine("Shown事件: 为portsTitlePanel中的btnDeletePort绑定事件");
                            btn.Click += btnDeletePort_Click;
                            foundPortButtons = true;
                        }
                    }
                }
            }
        }
        
        // 如果在TableLayoutPanel中没找到按钮，尝试使用成员变量中的按钮
        if (!foundSubDeviceButtons)
        {
            Debug.WriteLine("在subDevicesTitlePanel中未找到按钮，尝试使用成员变量按钮");
            if (btnAddSubDevice != null) btnAddSubDevice.Click += btnAddSubDevice_Click;
            if (btnEditSubDevice != null) btnEditSubDevice.Click += btnEditSubDevice_Click;
            if (btnDeleteSubDevice != null) btnDeleteSubDevice.Click += btnDeleteSubDevice_Click;
        }
        
        if (!foundPortButtons)
        {
            Debug.WriteLine("在portsTitlePanel中未找到按钮，尝试使用成员变量按钮");
            if (btnAddPort != null) btnAddPort.Click += btnAddPort_Click;
            if (btnEditPort != null) btnEditPort.Click += btnEditPort_Click;
            if (btnDeletePort != null) btnDeletePort.Click += btnDeletePort_Click;
        }
        
        // 创建右侧详细信息面板
        CreateDetailsPanel();
        
        // 特殊处理：重建筛选区域并处理下拉框
        try
        {
            // 首先重建筛选区域，确保下拉框正确显示
            RecreateFilerPanel();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"重建筛选区域时出错: {ex.Message}");
        }
        
        // 确保顶部按钮存在且在所有控件之上
        this.Invoke((MethodInvoker)delegate {
            // 在UI线程上执行以避免线程问题
            EnsureTopButtonsExist();
            
            // 设置Z顺序，确保按钮在最上层
            if (Controls.Contains(btnAdd)) Controls.SetChildIndex(btnAdd, 0);
            if (Controls.Contains(btnEdit)) Controls.SetChildIndex(btnEdit, 0);
            if (Controls.Contains(btnDelete)) Controls.SetChildIndex(btnDelete, 0);
            if (Controls.Contains(btnRefresh)) Controls.SetChildIndex(btnRefresh, 0);
        });
        
        // 最后调整顶部按钮位置
        AdjustTopPanelButtonsPosition();
        
        // 设置SplitContainer
        try
        {
            // 如果SplitContainer可见且宽度合理，设置分隔位置
            if (splitContainer1.Visible && splitContainer1.Width >= 150)
            {
                int validDistance1 = splitContainer1.Width * 2 / 3; // 使用2/3的宽度作为分隔位置
                
                try
                {
                    splitContainer1.SplitterDistance = validDistance1;
                    Debug.WriteLine($"Shown事件中设置splitContainer1.SplitterDistance = {validDistance1}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"无法在Shown事件中设置splitContainer1分隔条位置: {ex.Message}");
                    // 失败时尝试使用一个保守的值
                    if (splitContainer1.Width >= 100)
                    {
                        try { 
                            splitContainer1.SplitterDistance = splitContainer1.Width * 1 / 2;
                        } catch { /* 忽略二次尝试的错误 */ }
                    }
                }
            }
            
            // 处理底部SplitContainer
            if (bottomSplitContainer.Visible && bottomSplitContainer.Width >= 150)
            {
                int validDistance2 = bottomSplitContainer.Width * 1 / 2; // 使用一半宽度作为分隔位置
                
                try
                {
                    bottomSplitContainer.SplitterDistance = validDistance2;
                    Debug.WriteLine($"Shown事件中设置bottomSplitContainer.SplitterDistance = {validDistance2}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"无法在Shown事件中设置bottomSplitContainer分隔条位置: {ex.Message}");
                    // 失败时尝试使用一个保守的值
                    if (bottomSplitContainer.Width >= 100)
                    {
                        try { 
                            bottomSplitContainer.SplitterDistance = bottomSplitContainer.Width * 1 / 3;
                        } catch { /* 忽略二次尝试的错误 */ }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"设置SplitContainer出错: {ex.Message}");
        }
        
        // 再次确保控件可见
        try
        {
            EnsureAllControlsVisibleAndStyled();
            
            // 特别确保顶部按钮可见
            foreach (Button btn in new[] { btnAdd, btnEdit, btnDelete, btnRefresh })
            {
                if (btn != null)
                {
                    btn.Visible = true;
                    btn.Enabled = true;
                    btn.BringToFront();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"确保控件可见时出错: {ex.Message}");
        }
        
        // 强制刷新
        this.Invalidate(true);
        this.Update();
        Application.DoEvents();
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"FrmEqpGroup_Shown事件出错: {ex.Message}");
        // 不要在UI线程上抛出异常
    }
}

        private void AddTopDecorationBar()
        {
            try
            {
                // 创建顶部装饰面板
                Panel topBar = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 5,
                    BackColor = Color.FromArgb(100, 151, 177)
                };
                
                this.Controls.Add(topBar);
                topBar.BringToFront();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"添加顶部装饰条时出错: {ex.Message}");
            }
        }

        private void ShowLoadingIndicator()
        {
            try
            {
                // 创建半透明的加载面板
                loadingPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(200, 255, 255, 255)
                };
                
                // 创建加载提示标签
                Label lblLoading = new Label
                {
                    Text = "正在加载...",
                    Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(100, 151, 177),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.None,
                    Size = new Size(200, 40)
                };
                
                // 将标签放置在面板中央
                lblLoading.Location = new Point(
                    (this.ClientSize.Width - lblLoading.Width) / 2,
                    (this.ClientSize.Height - lblLoading.Height) / 2
                );
                
                loadingPanel.Controls.Add(lblLoading);
                this.Controls.Add(loadingPanel);
                loadingPanel.BringToFront();
                
                // 重绘
                this.Refresh();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"显示加载指示器时出错: {ex.Message}");
            }
        }
        
        private void HideLoadingIndicator()
        {
            try
            {
                if (loadingPanel != null)
                {
                    loadingPanel.Controls.Clear();
                    this.Controls.Remove(loadingPanel);
                    loadingPanel.Dispose();
                    loadingPanel = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"隐藏加载指示器时出错: {ex.Message}");
            }
        }

        // 提取一个应用DataGridView基础样式的方法，以便复用
        private void ApplyBasicDataGridViewStyle(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 151, 177); // 更加突出的蓝色标题
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // 白色字体增加对比度
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv.ColumnHeadersHeight = 38; // 稍微增加高度
            dgv.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9.5F);
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(135, 177, 203); // 稍深一点的选中色
            dgv.DefaultCellStyle.SelectionForeColor = Color.White; // 选中时使用白色文字
            dgv.RowTemplate.Height = 32; // 增加行高
            dgv.DefaultCellStyle.Padding = new Padding(5);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 251); // 淡蓝色间隔行
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = dgv.DefaultCellStyle.ForeColor;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.SelectionBackColor;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.SelectionForeColor;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None; // 去掉边框
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; 
            dgv.GridColor = Color.FromArgb(220, 230, 240); // 淡蓝色网格线
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // 圆角和阴影效果需要自定义绘制，这里只设置基本样式
        }
        
        private void FrmEqpGroup_Load(object sender, EventArgs e)
        {
            try
            {
                // 重置所有处理标记
                isProcessingAddSubDevice = false;
                isProcessingEditSubDevice = false;
                isProcessingDeleteSubDevice = false;
                isProcessingAddPort = false;
                isProcessingEditPort = false;
                isProcessingDeletePort = false;
                
                // 在Load事件中不绑定按钮事件，只移除可能存在的重复绑定
                // 注意：这里有两个版本的按钮，我们必须找到所有的按钮并取消绑定
                RemoveAllButtonEventHandlers();
                
                // 所有按钮事件将在Shown事件中绑定，而不是在Load事件中
                
                // 创建加载指示器
                ShowLoadingIndicator();
                
                // 暂停布局处理，减少闪烁和重绘
                this.SuspendLayout();
                this.splitContainer1.SuspendLayout();
                this.bottomSplitContainer.SuspendLayout();
                
                // 先执行初始化，再进行UI修复
                try
                {
                    // 设置面板外观
                    panelTop.Padding = new Padding(5);
                    panelTop.BackColor = Color.FromArgb(248, 249, 250); 
                    panelTop.BorderStyle = BorderStyle.None;
                    panelTop.Height = Math.Max(panelTop.Height, 50);
                    
                    // 设置其他面板样式
                    if (filterPanel != null)
                    {
                        filterPanel.BackColor = Color.FromArgb(248, 249, 250);
                        filterPanel.BorderStyle = BorderStyle.None;
                        filterPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    }
                    
                    // 配置数据网格视图和数据绑定
                    ConfigureDataGridView();
                    
                    // 加载筛选下拉框数据（这一步可能会抛出异常）
                    try {
                        LoadGroupFilterComboBox();
                    }
                    catch (Exception ex) {
                        Debug.WriteLine($"加载筛选下拉框出错: {ex.Message}");
                        // 创建一个空的数据源防止后续操作出错
                        List<EqpGroup> fallbackList = new List<EqpGroup> { new EqpGroup { EqpGroupId = "All", EqpGroupDescription = "(全部)" } };
                        cmbGroupTypeFilter.DataSource = fallbackList;
                        cmbGroupTypeFilter.DisplayMember = "DisplayInfo";
                        cmbGroupTypeFilter.ValueMember = "EqpGroupId";
                        if (cmbGroupTypeFilter.Items.Count > 0) {
                            cmbGroupTypeFilter.SelectedIndex = 0;
                        }
                    }
                    
                    // 加载数据（更安全地处理加载数据部分）
                    try
                    {
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"加载数据时发生异常: {ex.Message}");
                        MessageBox.Show($"加载数据时出错: {ex.Message}", "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    // 设置权限
                    ApplyPermissions();
                    
                    // 配置子设备和端口表格
                    ConfigureSubDeviceDataGridView();
                    ConfigurePortsDataGridView();
                    
                    // 应用样式
                    ApplyModernTheme();
                    
                    // 确保DataGridView样式正确应用
                    ApplyBasicDataGridViewStyle(dgvSubEquipment);
                    ApplyBasicDataGridViewStyle(dgvPorts);
                    ApplyBasicDataGridViewStyle(dgvEqpGroup);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"初始化控件时出错: {ex.Message}");
                    MessageBox.Show($"初始化窗体时出错: {ex.Message}", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                // 应用UI全面修复
                try
                {
                    // 确保所有按钮可见
                    if (btnAdd != null) btnAdd.Visible = true;
                    if (btnEdit != null) btnEdit.Visible = true;
                    if (btnDelete != null) btnDelete.Visible = true;
                    if (btnRefresh != null) btnRefresh.Visible = true;
                    if (btnSearch != null) btnSearch.Visible = true;
                    
                    // 确保下拉框和搜索框可见
                    if (cmbGroupTypeFilter != null) cmbGroupTypeFilter.Visible = true;
                    if (txtGroupIdSearch != null) txtGroupIdSearch.Visible = true;
                    
                    // 调整顶部面板按钮位置
                    AdjustTopPanelButtonsPosition();
                    
                    // 特殊处理：完全重建筛选区域
                    RecreateFilerPanel();
                    
                    // 再次确保所有控件可见并样式正确
                    EnsureAllControlsVisibleAndStyled();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"应用UI修复时出错: {ex.Message}");
                }
                
                // 添加对子设备 DataGridView 选择改变事件的监听
                try
                {
                    this.dgvSubEquipment.SelectionChanged += new System.EventHandler(this.dgvSubEquipment_SelectionChanged);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"添加事件监听器时出错: {ex.Message}");
                }
                
                // 恢复布局处理
                try
                {
                    this.splitContainer1.ResumeLayout(true);
                    this.bottomSplitContainer.ResumeLayout(true);
                    this.ResumeLayout(true);
                    
                    // 强制执行一次布局计算，为Shown事件做准备
                    this.PerformLayout();
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"恢复布局时出错: {ex.Message}");
                }
                
                Debug.WriteLine("FrmEqpGroup_Load完成");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FrmEqpGroup_Load出错: {ex.Message}");
                MessageBox.Show($"初始化窗体时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            // 应用平滑过渡效果
            ApplyFadeInEffect();
        }
        
        // 平滑过渡效果方法
        private void ApplyFadeInEffect()
{
    // 平滑过渡效果并隐藏加载指示器
    try
    {
        this.Opacity = 0;
        Timer fadeInTimer = new Timer();
        fadeInTimer.Interval = 30;
        fadeInTimer.Tick += (s, args) => {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.1;
            }
            else
            {
                ((Timer)s).Stop();
                ((Timer)s).Dispose();
                
                // 隐藏加载指示器
                HideLoadingIndicator();
            }
        };
        fadeInTimer.Start();
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"设置过渡效果出错: {ex.Message}");
        // 确保加载指示器被隐藏
        HideLoadingIndicator();
    }
}

// 确保顶部四个按钮存在的方法
private void EnsureTopButtonsExist()
{
    try
    {
        // 计算按钮的位置
        int topMargin = 5; // 顶部边距，调整更靠近顶部
        int width = this.Width;
        int buttonWidth = 73;
        int buttonHeight = 36;
        int spacing = 5;
        int rightMargin = 15;
        
        // 从右向左排列按钮的起始位置
        int right = width - rightMargin;
        
        // 将按钮从panelTop中移除，直接添加到窗体
        if (panelTop.Controls.Contains(btnAdd)) panelTop.Controls.Remove(btnAdd);
        if (panelTop.Controls.Contains(btnEdit)) panelTop.Controls.Remove(btnEdit);
        if (panelTop.Controls.Contains(btnDelete)) panelTop.Controls.Remove(btnDelete);
        if (panelTop.Controls.Contains(btnRefresh)) panelTop.Controls.Remove(btnRefresh);
        
        // 检查按钮是否存在，如果不存在就创建
        if (btnAdd == null || !Controls.Contains(btnAdd)) // 直接添加到窗体，而不是panelTop
        {
            btnAdd = new Button();
            btnAdd.Name = "btnAdd";
            btnAdd.Text = "➕ 添加";
            btnAdd.Size = new Size(buttonWidth, buttonHeight);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.BackColor = Color.FromArgb(92, 184, 92);
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            btnAdd.Click += btnAdd_Click;
            
            // 设置位置
            right -= buttonWidth;
            btnAdd.Location = new Point(right, topMargin); // 固定Y坐标
            
            Controls.Add(btnAdd); // 添加到窗体
            btnAdd.BringToFront(); // 确保显示在最上层
        }
        else
        {
            // 如果按钮已存在，确保正确的位置和属性
            right -= buttonWidth;
            btnAdd.Parent = this; // 确保父容器是窗体
            btnAdd.Location = new Point(right, topMargin);
            btnAdd.BringToFront();
        }
        
        if (btnEdit == null || !Controls.Contains(btnEdit))
        {
            btnEdit = new Button();
            btnEdit.Name = "btnEdit";
            btnEdit.Text = "✏️ 编辑";
            btnEdit.Size = new Size(buttonWidth, buttonHeight);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.BackColor = Color.FromArgb(91, 192, 222);
            btnEdit.ForeColor = Color.White;
            btnEdit.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            btnEdit.Click += btnEdit_Click;
            
            // 设置位置
            right -= (buttonWidth + spacing);
            btnEdit.Location = new Point(right, topMargin); // 固定Y坐标
            
            Controls.Add(btnEdit);
            btnEdit.BringToFront();
        }
        else
        {
            // 如果按钮已存在，确保正确的位置和属性
            right -= (buttonWidth + spacing);
            btnEdit.Parent = this; // 确保父容器是窗体
            btnEdit.Location = new Point(right, topMargin);
            btnEdit.BringToFront();
        }
        
        if (btnDelete == null || !Controls.Contains(btnDelete))
        {
            btnDelete = new Button();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "🗑️ 删除";
            btnDelete.Size = new Size(buttonWidth, buttonHeight);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.BackColor = Color.FromArgb(217, 83, 79);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            btnDelete.Click += btnDelete_Click;
            
            // 设置位置
            right -= (buttonWidth + spacing);
            btnDelete.Location = new Point(right, topMargin); // 固定Y坐标
            
            Controls.Add(btnDelete);
            btnDelete.BringToFront();
        }
        else
        {
            // 如果按钮已存在，确保正确的位置和属性
            right -= (buttonWidth + spacing);
            btnDelete.Parent = this; // 确保父容器是窗体
            btnDelete.Location = new Point(right, topMargin);
            btnDelete.BringToFront();
        }
        
        if (btnRefresh == null || !Controls.Contains(btnRefresh))
        {
            btnRefresh = new Button();
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Text = "🔄 刷新";
            btnRefresh.Size = new Size(buttonWidth, buttonHeight);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.BackColor = Color.FromArgb(100, 151, 177);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
            btnRefresh.Click += btnRefresh_Click;
            
            // 设置位置
            right -= (buttonWidth + spacing);
            btnRefresh.Location = new Point(right, topMargin); // 固定Y坐标
            
            Controls.Add(btnRefresh);
            btnRefresh.BringToFront();
        }
        else
        {
            // 如果按钮已存在，确保正确的位置和属性
            right -= (buttonWidth + spacing);
            btnRefresh.Parent = this; // 确保父容器是窗体
            btnRefresh.Location = new Point(right, topMargin);
            btnRefresh.BringToFront();
        }
        
        // 添加按钮圆角效果
        foreach (Button btn in new[] { btnAdd, btnEdit, btnDelete, btnRefresh })
        {
            try {
                GraphicsPath path = new GraphicsPath();
                ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                btn.Region = new Region(path);
                btn.Visible = true;
                btn.Enabled = true;
            } catch {}
        }

        // 按钮加入后强制刷新
        this.Invalidate(true);
        this.Update();
        Application.DoEvents();
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"确保顶部按钮存在时出错: {ex.Message}");
    }
}

        // 添加新方法来调整顶部面板按钮位置
        private void AdjustTopPanelButtonsPosition()
{
    try
    {
        // 确保按钮存在
        EnsureTopButtonsExist();
        
        // 确保按钮间隔和位置正确
        int rightMargin = 235; // 增加右边距，为详细信息面板留出空间(220宽度+15边距)
        int buttonWidth = 73; // 增加按钮宽度确保文字显示完整
        int buttonHeight = 36;
        int spacing = 5;
        int yPosition = 5; // 调整固定Y坐标靠上
        
        // 计算窗体宽度和按钮的位置
        int width = this.Width;
        int right = width - rightMargin;
        
        // 从右向左布局所有按钮，使四个按钮一排排列
        if (btnAdd != null)
        {
            btnAdd.Size = new Size(buttonWidth, buttonHeight);
            // 最右边的按钮
            right -= buttonWidth;
            btnAdd.Location = new Point(right, yPosition);
            btnAdd.Visible = true;
            btnAdd.Text = "➕ 添加";
            btnAdd.BackColor = Color.FromArgb(92, 184, 92);
            btnAdd.BringToFront(); // 确保按钮在最上层
        }
        
        if (btnEdit != null)
        {
            btnEdit.Size = new Size(buttonWidth, buttonHeight);
            // 第二个按钮
            right -= (buttonWidth + spacing);
            btnEdit.Location = new Point(right, yPosition);
            btnEdit.Visible = true;
            btnEdit.Text = "✏️ 编辑";
            btnEdit.BackColor = Color.FromArgb(91, 192, 222);
            btnEdit.BringToFront(); // 确保按钮在最上层
        }
        
        if (btnDelete != null)
        {
            btnDelete.Size = new Size(buttonWidth, buttonHeight);
            // 第三个按钮
            right -= (buttonWidth + spacing);
            btnDelete.Location = new Point(right, yPosition);
            btnDelete.Visible = true;
            btnDelete.Text = "🗑️ 删除";
            btnDelete.BackColor = Color.FromArgb(217, 83, 79);
            btnDelete.BringToFront(); // 确保按钮在最上层
        }
        
        if (btnRefresh != null)
        {
            btnRefresh.Size = new Size(buttonWidth, buttonHeight);
            // 第四个按钮
            right -= (buttonWidth + spacing);
            btnRefresh.Location = new Point(right, yPosition);
            btnRefresh.Visible = true;
            btnRefresh.Text = "🔄 刷新";
            btnRefresh.BackColor = Color.FromArgb(100, 151, 177);
            btnRefresh.BringToFront(); // 确保按钮在最上层
        }
        
        // 为按钮设置一致的样式
        foreach (Button btn in new[] { btnAdd, btnEdit, btnDelete, btnRefresh })
        {
            if (btn != null)
            {
                // 设置按钮为固定大小，确保四个按钮一排
                btn.Size = new Size(buttonWidth, buttonHeight);
                btn.MinimumSize = new Size(buttonWidth, buttonHeight);
                btn.MaximumSize = new Size(buttonWidth, buttonHeight);
                btn.Visible = true;
                btn.Enabled = true; // 确保按钮可用
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                
                // 添加圆角效果
                try 
                {
                    GraphicsPath path = new GraphicsPath();
                    ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                    btn.Region = new Region(path);
                } 
                catch {}
            }
        }
        
        // 强制刷新窗体显示
        this.Invalidate(true);
        this.Update();
        Application.DoEvents();
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"调整顶部面板按钮位置时出错: {ex.Message}");
        // 失败时不影响应用程序继续运行
    }
}

        private void ApplyUIStyles()
        {
            try
            {
                // 美化网格视图
                StyleDataGridView(dgvEqpGroup);
                StyleDataGridView(dgvSubEquipment);
                StyleDataGridView(dgvPorts);
                
                // 美化按钮
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Panel panel)
                    {
                        foreach (Control panelCtrl in panel.Controls)
                        {
                            if (panelCtrl is Button btn)
                            {
                                StyleButton(btn);
                            }
                        }
                    }
                    else if (ctrl is Button btn)
                    {
                        StyleButton(btn);
                    }
                }
                
                // 特别处理顶部面板
                if (panelTop != null)
                {
                    foreach (Control ctrl in panelTop.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleButton(btn);
                        }
                        else if (ctrl is TextBox txtBox)
                        {
                            txtBox.BorderStyle = BorderStyle.FixedSingle;
                            txtBox.BackColor = Color.White;
                        }
                        else if (ctrl is ComboBox cmb)
                        {
                            cmb.FlatStyle = FlatStyle.Flat;
                            cmb.BackColor = Color.White;
                        }
                    }
                }
                
                // 处理子设备和端口面板
                if (panelSubDeviceTop != null)
                {
                    foreach (Control ctrl in panelSubDeviceTop.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleButton(btn);
                        }
                    }
                }
                
                if (panelPortsTop != null)
                {
                    foreach (Control ctrl in panelPortsTop.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleButton(btn);
                        }
                    }
                }
                
                // 添加顶部装饰条
                AddTopDecorationPanel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"应用UI样式时出错: {ex.Message}");
                // 出错时静默处理，不影响主要功能
            }
        }

        private void StyleButton(Button btn)
        {
            if (btn == null) return;
            
            try
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(100, 151, 177);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                
                // 特殊处理不同类型的按钮
                if (btn.Name.Contains("Add"))
                {
                    btn.BackColor = Color.FromArgb(92, 184, 92); // 绿色
                }
                else if (btn.Name.Contains("Edit"))
                {
                    btn.BackColor = Color.FromArgb(91, 192, 222); // 蓝色
                }
                else if (btn.Name.Contains("Delete"))
                {
                    btn.BackColor = Color.FromArgb(217, 83, 79); // 红色
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"设置按钮样式时出错: {ex.Message}");
            }
        }
        
        private void StyleDataGridView(DataGridView dgv)
        {
            if (dgv == null) return;
            
            try
            {
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 151, 177);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(135, 177, 203);
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.RowTemplate.Height = 30;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 251);
                dgv.BackgroundColor = Color.White;
                dgv.BorderStyle = BorderStyle.Fixed3D;
                dgv.GridColor = Color.FromArgb(220, 230, 240);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"设置DataGridView样式时出错: {ex.Message}");
            }
        }
        
        private void AddTopDecorationPanel()
        {
            try
            {
                Panel decorationPanel = new Panel();
                decorationPanel.Dock = DockStyle.Top;
                decorationPanel.Height = 5;
                decorationPanel.BackColor = Color.FromArgb(100, 151, 177);
                
                this.Controls.Add(decorationPanel);
                decorationPanel.BringToFront();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"添加顶部装饰面板时出错: {ex.Message}");
            }
        }

        // 添加专用的方法来处理特殊按钮样式
        private void StyleSpecialButton(Button btn)
        {
            if (btn == null) return;
            
            try
            {
                // 设置固定的按钮尺寸，防止挤压
                btn.MinimumSize = new Size(40, 36);
                btn.Size = new Size(68, 36);
                
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                
                // 针对不同按钮应用不同颜色
                if (btn.Name.Contains("Add"))
                {
                    btn.Text = "➕";
                    btn.BackColor = Color.FromArgb(92, 184, 92); // 绿色
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(112, 204, 112);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(72, 164, 72);
                }
                else if (btn.Name.Contains("Edit"))
                {
                    btn.Text = "✏️";
                    btn.BackColor = Color.FromArgb(91, 192, 222); // 蓝色
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(111, 212, 242);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(71, 172, 202);
                }
                else if (btn.Name.Contains("Delete"))
                {
                    btn.Text = "🗑️";
                    btn.BackColor = Color.FromArgb(217, 83, 79); // 红色
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(237, 103, 99);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(197, 63, 59);
                }
                else if (btn.Name.Contains("Refresh"))
                {
                    btn.Text = "🔄";
                    btn.BackColor = Color.FromArgb(100, 151, 177); // 蓝灰色
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 171, 197);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 131, 157);
                }
                else if (btn.Name.Contains("Search"))
                {
                    btn.Text = "🔍";
                    btn.BackColor = Color.FromArgb(100, 151, 177); // 蓝灰色
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 171, 197);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 131, 157);
                }
                
                // 确保按钮可见
                btn.Visible = true;
                
                // 添加圆角效果
                try {
                    GraphicsPath path = new GraphicsPath();
                    ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                    btn.Region = new Region(path);
                } catch (Exception ex) {
                    Debug.WriteLine($"按钮 {btn.Name} 添加圆角效果出错: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"设置按钮 {btn.Name} 样式时出错: {ex.Message}");
            }
        }

        // 调整主窗体中所有按钮的外观，确保统一的样式
        private void StyleAllButtons()
        {
            try
            {
                // 处理mainTableLayoutPanel中的所有按钮
                if (mainTableLayoutPanel != null)
                {
                    ApplyStyleToControlsRecursively(mainTableLayoutPanel);
                }
                
                // 处理其他面板中的按钮
                if (panelTop != null)
                {
                    foreach (Control ctrl in panelTop.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleSpecialButton(btn);
                        }
                    }
                }
                
                // 处理subDevicesTitlePanel中的按钮
                if (subDevicesTitlePanel != null)
                {
                    foreach (Control ctrl in subDevicesTitlePanel.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleSpecialButton(btn);
                        }
                    }
                }
                
                // 处理portsTitlePanel中的按钮
                if (portsTitlePanel != null)
                {
                    foreach (Control ctrl in portsTitlePanel.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleSpecialButton(btn);
                        }
                    }
                }
                
                // 处理子设备和端口面板中的按钮
                if (panelSubDeviceTop != null)
                {
                    foreach (Control ctrl in panelSubDeviceTop.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleSpecialButton(btn);
                        }
                    }
                }
                
                if (panelPortsTop != null)
                {
                    foreach (Control ctrl in panelPortsTop.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            StyleSpecialButton(btn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"StyleAllButtons出错: {ex.Message}");
            }
        }

        // 递归处理容器内的所有控件
        private void ApplyStyleToControlsRecursively(Control container)
        {
            foreach (Control ctrl in container.Controls)
            {
                if (ctrl is Button btn)
                {
                    StyleSpecialButton(btn);
                }
                else if (ctrl.Controls.Count > 0)
                {
                    ApplyStyleToControlsRecursively(ctrl);
                }
            }
        }

        // 添加专门修复filterPanel中按钮大小的方法
        private void FixFilterPanelButtonsSize()
        {
            try
            {
                if (filterPanel != null)
                {
                    // 备份并暂停布局
                    filterPanel.SuspendLayout();
                    
                    // 先强制更新布局计算
                    filterPanel.PerformLayout();
                    
                    // 确保行高足够
                    while (filterPanel.RowStyles.Count < 1)
                    {
                        filterPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    }
                    filterPanel.RowStyles[0] = new RowStyle(SizeType.Percent, 100F);
                    
                    // 处理列宽
                    while (filterPanel.ColumnStyles.Count < 11)
                    {
                        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 73));
                    }
                    
                    // 设置列宽分配，确保有足够的空间放置四个按钮在右边一排
                    filterPanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 90F); // 第一列 标签
                    filterPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Absolute, 140F); // 第二列 下拉框
                    filterPanel.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 20F); // 间隔
                    filterPanel.ColumnStyles[3] = new ColumnStyle(SizeType.Absolute, 90F); // 标签
                    filterPanel.ColumnStyles[4] = new ColumnStyle(SizeType.Absolute, 120F); // 文本框
                    filterPanel.ColumnStyles[5] = new ColumnStyle(SizeType.Absolute, 80F); // 搜索按钮
                    filterPanel.ColumnStyles[6] = new ColumnStyle(SizeType.Percent, 100F); // 剩余空间
                    
                    // 设置整个filterPanel的样式，防止显示为一条线
                    filterPanel.BackColor = Color.FromArgb(248, 249, 250);
                    filterPanel.BorderStyle = BorderStyle.None; // 移除可能导致显示为线的边框
                    filterPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None; // 移除单元格边框
                    filterPanel.Height = Math.Max(filterPanel.Height, 54); // 确保高度足够
                    
                    // 从容器中移除要重新安排的按钮
                    Button btnAddRef = null, btnEditRef = null, btnDeleteRef = null, btnRefreshRef = null;
                    
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            if (btn.Name == "btnAdd") btnAddRef = btn;
                            else if (btn.Name == "btnEdit") btnEditRef = btn;
                            else if (btn.Name == "btnDelete") btnDeleteRef = btn;
                            else if (btn.Name == "btnRefresh") btnRefreshRef = btn;
                        }
                    }
                    
                    // 直接手动设置按钮位置，放在右侧一排
                    if (btnAddRef != null && btnEditRef != null && btnDeleteRef != null && btnRefreshRef != null)
                    {
                        // 计算右侧起始位置
                        int rightEdge = filterPanel.Width - 10; // 右边缘留10px边距
                        int buttonWidth = 65; // 每个按钮宽度
                        int buttonSpacing = 5; // 按钮之间的间距
                        int buttonHeight = 36; // 按钮高度
                        int vertCenter = (filterPanel.Height - buttonHeight) / 2; // 垂直居中
                        
                        // 从右向左设置4个按钮位置
                        // 刷新按钮
                        btnRefreshRef.Parent = filterPanel;
                        btnRefreshRef.Size = new Size(buttonWidth, buttonHeight);
                        btnRefreshRef.Location = new Point(rightEdge - buttonWidth, vertCenter);
                        rightEdge -= (buttonWidth + buttonSpacing);
                        
                        // 删除按钮
                        btnDeleteRef.Parent = filterPanel;
                        btnDeleteRef.Size = new Size(buttonWidth, buttonHeight);
                        btnDeleteRef.Location = new Point(rightEdge - buttonWidth, vertCenter);
                        rightEdge -= (buttonWidth + buttonSpacing);
                        
                        // 编辑按钮
                        btnEditRef.Parent = filterPanel;
                        btnEditRef.Size = new Size(buttonWidth, buttonHeight);
                        btnEditRef.Location = new Point(rightEdge - buttonWidth, vertCenter);
                        rightEdge -= (buttonWidth + buttonSpacing);
                        
                        // 添加按钮
                        btnAddRef.Parent = filterPanel;
                        btnAddRef.Size = new Size(buttonWidth, buttonHeight);
                        btnAddRef.Location = new Point(rightEdge - buttonWidth, vertCenter);
                        
                        // 设置按钮样式
                        foreach (Button btn in new[] { btnAddRef, btnEditRef, btnDeleteRef, btnRefreshRef })
                        {
                            // 设置统一的按钮尺寸
                            btn.Size = new Size(buttonWidth, buttonHeight);
                            btn.MinimumSize = new Size(buttonWidth, buttonHeight);
                            btn.MaximumSize = new Size(buttonWidth, buttonHeight);
                            btn.Visible = true;
                            btn.Margin = new Padding(2);
                            
                            // 确保内容不超过按钮大小
                            if (btn.Name == "btnAdd")
                            {
                                btn.Text = "➕";
                                btn.BackColor = Color.FromArgb(92, 184, 92); // 绿色
                            }
                            else if (btn.Name == "btnEdit")
                            {
                                btn.Text = "✏️";
                                btn.BackColor = Color.FromArgb(91, 192, 222); // 蓝色
                            }
                            else if (btn.Name == "btnDelete")
                            {
                                btn.Text = "🗑️";
                                btn.BackColor = Color.FromArgb(217, 83, 79); // 红色
                            }
                            else if (btn.Name == "btnRefresh")
                            {
                                btn.Text = "🔄";
                                btn.BackColor = Color.FromArgb(100, 151, 177); // 蓝灰色
                            }
                            else if (btn.Name == "btnSearch")
                            {
                                btn.Text = "🔍";
                                btn.BackColor = Color.FromArgb(100, 151, 177); // 蓝灰色
                            }
                            
                            // 应用按钮基本样式
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderSize = 0;
                            btn.ForeColor = Color.White;
                            btn.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                            btn.Cursor = Cursors.Hand;
                            
                            // 添加圆角效果
                            try {
                                GraphicsPath path = new GraphicsPath();
                                ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                                btn.Region = new Region(path);
                            } catch {}
                        }
                    }
                    
                    // 处理其他控件样式
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Label lbl)
                        {
                            // 确保标签可见且有合适样式
                            lbl.BackColor = Color.Transparent;
                            lbl.ForeColor = Color.FromArgb(60, 60, 60);
                            lbl.Font = new Font("Microsoft YaHei UI", 10F);
                            lbl.AutoSize = true;
                            lbl.Visible = true;
                        }
                        else if (ctrl is ComboBox cmb)
                        {
                            // 确保下拉框可见且有合适样式
                            cmb.BackColor = Color.White;
                            cmb.ForeColor = Color.FromArgb(40, 40, 40);
                            cmb.Font = new Font("Microsoft YaHei UI", 10F);
                            cmb.Size = new Size(140, 30);
                            cmb.Visible = true;
                        }
                        else if (ctrl is TextBox txt)
                        {
                            // 确保文本框可见且有合适样式
                            txt.BackColor = Color.White;
                            txt.ForeColor = Color.FromArgb(40, 40, 40);
                            txt.Font = new Font("Microsoft YaHei UI", 10F);
                            txt.Size = new Size(120, 30);
                            txt.BorderStyle = BorderStyle.FixedSingle;
                            txt.Visible = true;
                        }
                        else if (ctrl is Button btn && btn.Name == "btnSearch")
                        {
                            // 搜索按钮特别处理
                            btn.Size = new Size(70, 32);
                            btn.Text = "🔍";
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderSize = 0;
                            btn.BackColor = Color.FromArgb(100, 151, 177);
                            btn.ForeColor = Color.White;
                            btn.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                            btn.Cursor = Cursors.Hand;
                            
                            // 添加圆角效果
                            try {
                                GraphicsPath path = new GraphicsPath();
                                ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                                btn.Region = new Region(path);
                            } catch {}
                        }
                    }
                    
                    // 恢复布局
                    filterPanel.ResumeLayout(true);
                    
                    // 强制重绘
                    filterPanel.Invalidate();
                    filterPanel.Refresh();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"修复filterPanel按钮尺寸时出错: {ex.Message}");
            }
        }

        // 添加修复子设备标题面板按钮的方法
        private void FixSubDevicesPanelButtonsSize()
        {
            try
            {
                if (subDevicesTitlePanel != null)
                {
                    // 暂停布局处理
                    subDevicesTitlePanel.SuspendLayout();
                    
                    // 设置行高
                    while (subDevicesTitlePanel.RowStyles.Count < 1)
                    {
                        subDevicesTitlePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    }
                    subDevicesTitlePanel.RowStyles[0] = new RowStyle(SizeType.Percent, 100F);
                    
                    // 确保列宽足够
                    while (subDevicesTitlePanel.ColumnStyles.Count < 5)
                    {
                        subDevicesTitlePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
                    }
                    
                    // 设置列宽
                    subDevicesTitlePanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 80F); // 标签
                    subDevicesTitlePanel.ColumnStyles[1] = new ColumnStyle(SizeType.Absolute, 70F); // 添加按钮
                    subDevicesTitlePanel.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 70F); // 编辑按钮
                    subDevicesTitlePanel.ColumnStyles[3] = new ColumnStyle(SizeType.Absolute, 70F); // 删除按钮
                    subDevicesTitlePanel.ColumnStyles[4] = new ColumnStyle(SizeType.Percent, 100F); // 剩余空间
                    
                    // 修改按钮样式和大小
                    foreach (Control ctrl in subDevicesTitlePanel.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            // 设置统一的按钮尺寸和样式
                            btn.Size = new Size(64, 32);
                            btn.MinimumSize = new Size(64, 32);
                            btn.MaximumSize = new Size(64, 32);
                            btn.Visible = true;
                            btn.Margin = new Padding(3);
                            btn.Anchor = AnchorStyles.None; // 居中显示
                            
                            // 应用按钮样式
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderSize = 0;
                            btn.ForeColor = Color.White;
                            btn.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
                            btn.Cursor = Cursors.Hand;
                            
                            // 根据按钮类型设置不同的背景色和图标
                            if (btn.Name == "btnAddSubDevice")
                            {
                                btn.Text = "➕";
                                btn.BackColor = Color.FromArgb(92, 184, 92); // 绿色
                                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(112, 204, 112);
                                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(72, 164, 72);
                                // 添加提示
                                ToolTip tt = new ToolTip();
                                tt.SetToolTip(btn, "添加子设备");
                            }
                            else if (btn.Name == "btnEditSubDevice")
                            {
                                btn.Text = "✏️";
                                btn.BackColor = Color.FromArgb(91, 192, 222); // 蓝色
                                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(111, 212, 242);
                                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(71, 172, 202);
                                // 添加提示
                                ToolTip tt = new ToolTip();
                                tt.SetToolTip(btn, "编辑子设备");
                            }
                            else if (btn.Name == "btnDeleteSubDevice")
                            {
                                btn.Text = "🗑️";
                                btn.BackColor = Color.FromArgb(217, 83, 79); // 红色
                                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(237, 103, 99);
                                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(197, 63, 59);
                                // 添加提示
                                ToolTip tt = new ToolTip();
                                tt.SetToolTip(btn, "删除子设备");
                            }
                            
                            // 添加圆角效果
                            try {
                                GraphicsPath path = new GraphicsPath();
                                ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                                btn.Region = new Region(path);
                            } catch {}
                        }
                    }
                    
                    // 恢复布局
                    subDevicesTitlePanel.ResumeLayout(true);
                    
                    // 强制重绘
                    subDevicesTitlePanel.Invalidate();
                    subDevicesTitlePanel.Update();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"修复subDevicesTitlePanel按钮样式时出错: {ex.Message}");
            }
        }

        // 添加修复端口标题面板按钮的方法
        private void FixPortsPanelButtonsSize()
        {
            try
            {
                if (portsTitlePanel != null)
                {
                    // 暂停布局处理
                    portsTitlePanel.SuspendLayout();
                    
                    // 设置行高
                    while (portsTitlePanel.RowStyles.Count < 1)
                    {
                        portsTitlePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    }
                    portsTitlePanel.RowStyles[0] = new RowStyle(SizeType.Percent, 100F);
                    
                    // 确保列宽足够
                    while (portsTitlePanel.ColumnStyles.Count < 5)
                    {
                        portsTitlePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
                    }
                    
                    // 设置列宽
                    portsTitlePanel.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 80F); // 标签
                    portsTitlePanel.ColumnStyles[1] = new ColumnStyle(SizeType.Absolute, 70F); // 添加按钮
                    portsTitlePanel.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, 70F); // 编辑按钮
                    portsTitlePanel.ColumnStyles[3] = new ColumnStyle(SizeType.Absolute, 70F); // 删除按钮
                    portsTitlePanel.ColumnStyles[4] = new ColumnStyle(SizeType.Percent, 100F); // 剩余空间
                    
                    // 修改按钮样式和大小
                    foreach (Control ctrl in portsTitlePanel.Controls)
                    {
                        if (ctrl is Button btn)
                        {
                            // 设置统一的按钮尺寸和样式
                            btn.Size = new Size(64, 32);
                            btn.MinimumSize = new Size(64, 32);
                            btn.MaximumSize = new Size(64, 32);
                            btn.Visible = true;
                            btn.Margin = new Padding(3);
                            btn.Anchor = AnchorStyles.None; // 居中显示
                            
                            // 应用按钮样式
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderSize = 0;
                            btn.ForeColor = Color.White;
                            btn.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
                            btn.Cursor = Cursors.Hand;
                            
                            // 根据按钮类型设置不同的背景色和图标
                            if (btn.Name == "btnAddPort")
                            {
                                btn.Text = "➕";
                                btn.BackColor = Color.FromArgb(92, 184, 92); // 绿色
                                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(112, 204, 112);
                                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(72, 164, 72);
                                // 添加提示
                                ToolTip tt = new ToolTip();
                                tt.SetToolTip(btn, "添加端口");
                            }
                            else if (btn.Name == "btnEditPort")
                            {
                                btn.Text = "✏️";
                                btn.BackColor = Color.FromArgb(91, 192, 222); // 蓝色
                                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(111, 212, 242);
                                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(71, 172, 202);
                                // 添加提示
                                ToolTip tt = new ToolTip();
                                tt.SetToolTip(btn, "编辑端口");
                            }
                            else if (btn.Name == "btnDeletePort")
                            {
                                btn.Text = "🗑️";
                                btn.BackColor = Color.FromArgb(217, 83, 79); // 红色
                                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(237, 103, 99);
                                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(197, 63, 59);
                                // 添加提示
                                ToolTip tt = new ToolTip();
                                tt.SetToolTip(btn, "删除端口");
                            }
                            
                            // 添加圆角效果
                            try {
                                GraphicsPath path = new GraphicsPath();
                                ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                                btn.Region = new Region(path);
                            } catch {}
                        }
                    }
                    
                    // 恢复布局
                    portsTitlePanel.ResumeLayout(true);
                    
                    // 强制重绘
                    portsTitlePanel.Invalidate();
                    portsTitlePanel.Update();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"修复portsTitlePanel按钮样式时出错: {ex.Message}");
            }
        }

        // 添加专门处理红框区域线条问题的方法
        private void FixRedFrameAreaLines()
        {
            try
            {
                // 如果filterPanel存在，则彻底重新创建红框区域UI
                if (filterPanel != null)
                {
                    // 停止布局计算
                    filterPanel.SuspendLayout();
                    
                    // 首先清除所有之前的处理
                    // 清除边框
                    filterPanel.BorderStyle = BorderStyle.None;
                    filterPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    filterPanel.BackColor = Color.FromArgb(248, 249, 250);
                    filterPanel.Height = Math.Max(filterPanel.Height, 54);
                    
                    // 移除任何可能显示为线的小控件
                    List<Control> controlsToRemove = new List<Control>();
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        // 如果是非必要控件且高度小，可能显示为线
                        if (!(ctrl is Button) && !(ctrl is Label) && !(ctrl is ComboBox) && !(ctrl is TextBox) &&
                            (ctrl.Height <= 5 || ctrl.Width <= 5))
                        {
                            controlsToRemove.Add(ctrl);
                        }
                    }
                    
                    // 安全移除控件
                    foreach (Control ctrl in controlsToRemove)
                    {
                        filterPanel.Controls.Remove(ctrl);
                        ctrl.Dispose();
                    }
                    
                    // 在这里采用一个完全不同的方法 - 创建一个新的覆盖面板作为背景
                    Panel coverBackground = new Panel();
                    coverBackground.BackColor = Color.FromArgb(248, 249, 250);
                    coverBackground.Dock = DockStyle.Fill; // 填充整个区域
                    coverBackground.BorderStyle = BorderStyle.None;
                    coverBackground.Tag = "CoverBackground";
                    
                    // 检查是否已经有这个面板
                    bool hasBackground = false;
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Panel p && p.Tag?.ToString() == "CoverBackground")
                        {
                            hasBackground = true;
                            break;
                        }
                    }
                    
                    // 如果没有添加过，则添加新背景
                    if (!hasBackground)
                    {
                        filterPanel.Controls.Add(coverBackground);
                        coverBackground.SendToBack(); // 确保在最底层
                    }
                    
                    // 额外创建左上角区域的覆盖Panel
                    Panel leftTopCover = new Panel();
                    leftTopCover.BackColor = Color.FromArgb(248, 249, 250);
                    leftTopCover.Location = new Point(0, 0);
                    leftTopCover.Size = new Size(300, filterPanel.Height);
                    leftTopCover.BorderStyle = BorderStyle.None;
                    leftTopCover.Tag = "LeftTopCover";
                    
                    // 检查是否已经有左上角覆盖面板
                    bool hasLeftCover = false;
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Panel p && p.Tag?.ToString() == "LeftTopCover")
                        {
                            hasLeftCover = true;
                            p.Size = new Size(300, filterPanel.Height); // 更新大小确保覆盖
                            break;
                        }
                    }
                    
                    // 如果没有添加过，则添加
                    if (!hasLeftCover)
                    {
                        filterPanel.Controls.Add(leftTopCover);
                        leftTopCover.BringToFront(); // 确保在其他控件之上
                    }
                    
                    // 特殊处理 - 强制重新创建布局
                    filterPanel.ColumnStyles.Clear();
                    
                    // 重新设置列样式
                    while (filterPanel.ColumnStyles.Count < 11)
                    {
                        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
                    }
                    
                    // 确保行高足够
                    while (filterPanel.RowStyles.Count < 1)
                    {
                        filterPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    }
                    filterPanel.RowStyles[0] = new RowStyle(SizeType.Absolute, 54); // 固定高度确保不会被压缩
                    
                    // 重设所有必要控件的Z顺序和位置
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Label || ctrl is ComboBox || ctrl is TextBox || ctrl is Button)
                        {
                            ctrl.BringToFront(); // 确保所有实际控件在覆盖面板上方
                            
                            // 设置最小尺寸避免被挤压
                            if (ctrl is ComboBox cmb)
                            {
                                cmb.MinimumSize = new Size(140, 24);
                                cmb.Height = Math.Max(cmb.Height, 24);
                                cmb.Visible = true;
                            }
                            else if (ctrl is Label lbl)
                            {
                                lbl.AutoSize = true;
                                lbl.MinimumSize = new Size(0, 20);
                                lbl.Visible = true;
                            }
                            else if (ctrl is TextBox txt)
                            {
                                txt.MinimumSize = new Size(100, 24);
                                txt.Height = Math.Max(txt.Height, 24);
                                txt.Visible = true;
                            }
                        }
                    }
                    
                    // 最后手动添加一个特殊的区域标记Label
                    Label lblAreaMark = new Label();
                    lblAreaMark.Text = string.Empty; // 空文本
                    lblAreaMark.BackColor = Color.FromArgb(248, 249, 250);
                    lblAreaMark.Location = new Point(0, 0);
                    lblAreaMark.Size = new Size(filterPanel.Width, 5);
                    lblAreaMark.Visible = true;
                    lblAreaMark.Tag = "AreaMark";
                    
                    // 检查是否已有此标记
                    bool hasAreaMark = false;
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Label lbl && lbl.Tag?.ToString() == "AreaMark")
                        {
                            hasAreaMark = true;
                            break;
                        }
                    }
                    
                    // 如果没有添加过，则添加
                    if (!hasAreaMark)
                    {
                        filterPanel.Controls.Add(lblAreaMark);
                        lblAreaMark.SendToBack(); // 放到底层，但在覆盖面板之上
                    }
                    
                    // 直接修改搜索标签和下拉框的位置和大小，确保它们可见且排列正确
                    Label lblType = null;
                    ComboBox cmbType = null;
                    Label lblId = null;
                    TextBox txtId = null;
                    Button btnSearch = null;
                    
                    // 查找关键控件
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Label lbl)
                        {
                            if (lbl.Text.Contains("类型"))
                                lblType = lbl;
                            else if (lbl.Text.Contains("编号"))
                                lblId = lbl;
                        }
                        else if (ctrl is ComboBox cmb && (cmb.Name == "cmbGroupTypeFilter" || cmb == cmbGroupTypeFilter))
                        {
                            cmbType = cmb;
                        }
                        else if (ctrl is TextBox txt && (txt.Name == "txtGroupIdSearch" || txt == txtGroupIdSearch))
                        {
                            txtId = txt;
                        }
                        else if (ctrl is Button btn && (btn.Name == "btnSearch" || btn == btnSearch))
                        {
                            btnSearch = btn;
                        }
                    }
                    
                    // 如果找到了关键控件，重新设置它们的位置，确保可见
                    if (lblType != null && cmbType != null && lblId != null && txtId != null && btnSearch != null)
                    {
                        int labelWidth = 90;
                        int controlWidth = 140;
                        int height = 30;
                        int spacing = 10;
                        int verticalCenter = (filterPanel.Height - height) / 2;
                        
                        // 从左向右布局
                        int left = 10;
                        
                        // 设备组类型
                        lblType.Location = new Point(left, verticalCenter);
                        lblType.AutoSize = true;
                        lblType.Visible = true;
                        left += labelWidth + spacing;
                        
                        cmbType.Location = new Point(left, verticalCenter);
                        cmbType.Size = new Size(controlWidth, height);
                        cmbType.Visible = true;
                        left += controlWidth + spacing * 2;
                        
                        // 设备组编号
                        lblId.Location = new Point(left, verticalCenter);
                        lblId.AutoSize = true;
                        lblId.Visible = true;
                        left += labelWidth + spacing;
                        
                        txtId.Location = new Point(left, verticalCenter);
                        txtId.Size = new Size(controlWidth, height);
                        txtId.Visible = true;
                        left += controlWidth + spacing;
                        
                        // 搜索按钮
                        btnSearch.Location = new Point(left, verticalCenter);
                        btnSearch.Size = new Size(70, height);
                        btnSearch.Visible = true;
                        
                        // 重置这些控件的父级
                        leftTopCover.Controls.Add(lblType);
                        leftTopCover.Controls.Add(cmbType);
                        leftTopCover.Controls.Add(lblId);
                        leftTopCover.Controls.Add(txtId);
                        leftTopCover.Controls.Add(btnSearch);
                    }
                    
                    // 恢复布局计算
                    filterPanel.ResumeLayout(true);
                    filterPanel.PerformLayout();
                    
                    // 强制重绘
                    filterPanel.Invalidate();
                    filterPanel.Refresh();
                    Application.DoEvents();
                }
                
                // 处理其他可能显示为线的区域
                if (panelTop != null)
                {
                    panelTop.BorderStyle = BorderStyle.None;
                    panelTop.BackColor = Color.FromArgb(248, 249, 250);
                    panelTop.Height = Math.Max(panelTop.Height, 60);
                }
                
                if (subDevicesTitlePanel != null)
                {
                    subDevicesTitlePanel.BorderStyle = BorderStyle.None;
                    subDevicesTitlePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    subDevicesTitlePanel.BackColor = Color.FromArgb(248, 249, 250);
                    subDevicesTitlePanel.Height = Math.Max(subDevicesTitlePanel.Height, 40);
                }
                
                if (portsTitlePanel != null)
                {
                    portsTitlePanel.BorderStyle = BorderStyle.None;
                    portsTitlePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    portsTitlePanel.BackColor = Color.FromArgb(248, 249, 250);
                    portsTitlePanel.Height = Math.Max(portsTitlePanel.Height, 40);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"修复红框区域线条问题时出错: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

        // 添加一个最终的解决方案，直接替换红框区域的TableLayoutPanel
        private void ReplaceRedFrameArea()
        {
            try
            {
                // 如果找不到filterPanel，则无需处理
                if (filterPanel == null || panelTop == null)
                    return;
                
                // 清除所有可能的覆盖层或其他干扰
                List<Control> controlsToRemove = new List<Control>();
                foreach (Control ctrl in filterPanel.Controls)
                {
                    if (ctrl is Panel p && (p.Tag?.ToString() == "PureBackground" || 
                                           p.Tag?.ToString() == "DirectCover" || 
                                           p.Tag?.ToString() == "CoverBackground" || 
                                           p.Tag?.ToString() == "LeftTopCover"))
                    {
                        controlsToRemove.Add(ctrl);
                    }
                }
                
                // 安全移除这些面板
                foreach (Control ctrl in controlsToRemove)
                {
                    // 如果这些面板包含了其他控件，需要将控件重新添加到filterPanel
                    foreach (Control child in ctrl.Controls)
                    {
                        child.Parent = filterPanel;
                    }
                    
                    filterPanel.Controls.Remove(ctrl);
                    ctrl.Dispose();
                }
                
                // 设置面板基本属性
                filterPanel.BackColor = Color.FromArgb(248, 249, 250);
                filterPanel.Height = Math.Max(filterPanel.Height, 56);
                filterPanel.BorderStyle = BorderStyle.None;
                filterPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                
                // 直接定位和设置筛选区域控件
                // 第一步：找到相关的控件
                Label lblType = null;
                ComboBox cmbType = null;
                Label lblId = null;
                TextBox txtId = null;
                Button btnSearch = null;
                
                foreach (Control ctrl in filterPanel.Controls)
                {
                    if (ctrl is Label lbl)
                    {
                        if (lbl.Text.Contains("类型"))
                            lblType = lbl;
                        else if (lbl.Text.Contains("编号"))
                            lblId = lbl;
                    }
                    else if (ctrl is ComboBox cmb && (cmb.Name == "cmbGroupTypeFilter" || cmb == cmbGroupTypeFilter))
                    {
                        cmbType = cmb;
                    }
                    else if (ctrl is TextBox txt && (txt.Name == "txtGroupIdSearch" || txt == txtGroupIdSearch))
                    {
                        txtId = txt;
                    }
                    else if (ctrl is Button btn && (btn.Name == "btnSearch" || btn == btnSearch))
                    {
                        btnSearch = btn;
                    }
                }
                
                // 如果找不到控件，尝试直接从类成员获取
                if (cmbType == null) cmbType = cmbGroupTypeFilter;
                if (txtId == null) txtId = txtGroupIdSearch;
                if (btnSearch == null && this.Controls.Find("btnSearch", true).Length > 0)
                    btnSearch = (Button)this.Controls.Find("btnSearch", true)[0];
                
                // 如果仍然找不到必要控件，尝试创建它们
                if (lblType == null)
                {
                    lblType = new Label();
                    lblType.Text = "设备组类型:";
                    lblType.AutoSize = true;
                    filterPanel.Controls.Add(lblType);
                }
                
                if (lblId == null)
                {
                    lblId = new Label();
                    lblId.Text = "设备组编号:";
                    lblId.AutoSize = true;
                    filterPanel.Controls.Add(lblId);
                }
                
                if (cmbType == null)
                {
                    cmbType = new ComboBox();
                    cmbType.Name = "cmbGroupTypeFilter";
                    filterPanel.Controls.Add(cmbType);
                    // 关联到全局控件
                    cmbGroupTypeFilter = cmbType;
                    
                    // 加载数据
                    try
                    {
                        LoadGroupFilterComboBox();
                    }
                    catch {}
                }
                
                if (txtId == null)
                {
                    txtId = new TextBox();
                    txtId.Name = "txtGroupIdSearch";
                    filterPanel.Controls.Add(txtId);
                    // 关联到全局控件
                    txtGroupIdSearch = txtId;
                }
                
                if (btnSearch == null)
                {
                    btnSearch = new Button();
                    btnSearch.Name = "btnSearch";
                    btnSearch.Text = "🔍 搜索";
                    btnSearch.Click += btnSearch_Click;
                    filterPanel.Controls.Add(btnSearch);
                }
                
                // 第二步：直接设置控件位置和大小
                int height = 30; // 控件高度
                int padding = 5; // 控件间距
                int topMargin = (filterPanel.Height - height) / 2; // 垂直居中
                
                // 组织控件位置 - 从左到右排列
                int left = padding;
                
                // 设备组类型标签
                lblType.Location = new Point(left, topMargin + 5);
                left += lblType.Width + padding;
                
                // 设备组类型下拉框
                cmbType.Size = new Size(150, height);
                cmbType.Location = new Point(left, topMargin);
                left += cmbType.Width + padding * 3;
                
                // 设备组编号标签
                lblId.Location = new Point(left, topMargin + 5);
                left += lblId.Width + padding;
                
                // 设备组编号文本框
                txtId.Size = new Size(150, height);
                txtId.Location = new Point(left, topMargin);
                left += txtId.Width + padding * 2;
                
                // 搜索按钮
                btnSearch.Size = new Size(80, height);
                btnSearch.Location = new Point(left, topMargin);
                
                // 确保所有控件可见
                lblType.Visible = true;
                cmbType.Visible = true;
                lblId.Visible = true;
                txtId.Visible = true;
                btnSearch.Visible = true;
                
                // 应用控件样式
                lblType.BackColor = Color.Transparent;
                lblType.ForeColor = Color.FromArgb(60, 60, 60);
                lblType.Font = new Font("Microsoft YaHei UI", 10F);
                
                lblId.BackColor = Color.Transparent;
                lblId.ForeColor = Color.FromArgb(60, 60, 60);
                lblId.Font = new Font("Microsoft YaHei UI", 10F);
                
                cmbType.BackColor = Color.White;
                cmbType.ForeColor = Color.FromArgb(40, 40, 40);
                cmbType.Font = new Font("Microsoft YaHei UI", 10F);
                
                txtId.BackColor = Color.White;
                txtId.ForeColor = Color.FromArgb(40, 40, 40);
                txtId.Font = new Font("Microsoft YaHei UI", 10F);
                txtId.BorderStyle = BorderStyle.FixedSingle;
                
                btnSearch.FlatStyle = FlatStyle.Flat;
                btnSearch.FlatAppearance.BorderSize = 0;
                btnSearch.BackColor = Color.FromArgb(100, 151, 177);
                btnSearch.ForeColor = Color.White;
                btnSearch.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                btnSearch.Cursor = Cursors.Hand;
                
                // 添加搜索按钮圆角效果
                try {
                    GraphicsPath path = new GraphicsPath();
                    ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btnSearch.Width, btnSearch.Height), 5);
                    btnSearch.Region = new Region(path);
                } catch {}
                
                // 重新执行布局然后刷新
                filterPanel.PerformLayout();
                filterPanel.Invalidate();
                filterPanel.Update();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"替换红框区域时出错: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

        // 确保所有重要控件可见并样式正确的方法
        private void EnsureAllControlsVisibleAndStyled()
        {
            try
            {
                // 1. 主操作按钮
                Button[] mainButtons = new Button[] { btnAdd, btnEdit, btnDelete, btnRefresh };
                foreach (Button btn in mainButtons)
                {
                    if (btn != null)
                    {
                        btn.Visible = true;
                        btn.Size = new Size(73, 36);
                        
                        // 根据按钮类型设置不同样式
                        if (btn == btnAdd)
                        {
                            btn.Text = "➕ 添加";
                            btn.BackColor = Color.FromArgb(92, 184, 92); // 绿色
                        }
                        else if (btn == btnEdit)
                        {
                            btn.Text = "✏️ 编辑";
                            btn.BackColor = Color.FromArgb(91, 192, 222); // 蓝色
                        }
                        else if (btn == btnDelete)
                        {
                            btn.Text = "🗑️ 删除";
                            btn.BackColor = Color.FromArgb(217, 83, 79); // 红色
                        }
                        else if (btn == btnRefresh)
                        {
                            btn.Text = "🔄 刷新";
                            btn.BackColor = Color.FromArgb(100, 151, 177); // 蓝灰色
                        }
                        
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.ForeColor = Color.White;
                        btn.Font = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold);
                        btn.Cursor = Cursors.Hand;
                        
                        // 添加圆角效果
                        try {
                            GraphicsPath path = new GraphicsPath();
                            ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btn.Width, btn.Height), 5);
                            btn.Region = new Region(path);
                        } catch {}
                    }
                }
                
                // 2. 搜索按钮和筛选控件
                if (btnSearch != null)
                {
                    btnSearch.Visible = true;
                    btnSearch.Text = "🔍 搜索";
                    btnSearch.Size = new Size(80, 32);
                    btnSearch.BackColor = Color.FromArgb(100, 151, 177);
                    btnSearch.FlatStyle = FlatStyle.Flat;
                    btnSearch.FlatAppearance.BorderSize = 0;
                    btnSearch.ForeColor = Color.White;
                    btnSearch.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                    btnSearch.Cursor = Cursors.Hand;
                    
                    // 添加圆角效果
                    try {
                        GraphicsPath path = new GraphicsPath();
                        ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, btnSearch.Width, btnSearch.Height), 5);
                        btnSearch.Region = new Region(path);
                    } catch {}
                }
                
                // 确保筛选下拉框和搜索文本框可见
                if (cmbGroupTypeFilter != null)
                {
                    cmbGroupTypeFilter.Visible = true;
                    cmbGroupTypeFilter.Enabled = true;
                    cmbGroupTypeFilter.BackColor = Color.White;
                    cmbGroupTypeFilter.ForeColor = Color.FromArgb(40, 40, 40);
                    cmbGroupTypeFilter.Font = new Font("Microsoft YaHei UI", 10F);
                }
                
                if (txtGroupIdSearch != null)
                {
                    txtGroupIdSearch.Visible = true;
                    txtGroupIdSearch.Enabled = true;
                    txtGroupIdSearch.BackColor = Color.White;
                    txtGroupIdSearch.ForeColor = Color.FromArgb(40, 40, 40);
                    txtGroupIdSearch.Font = new Font("Microsoft YaHei UI", 10F);
                    txtGroupIdSearch.BorderStyle = BorderStyle.FixedSingle;
                }
                
                // 确保红框区域内的所有标签可见
                if (filterPanel != null)
                {
                    foreach (Control ctrl in filterPanel.Controls)
                    {
                        if (ctrl is Label lbl)
                        {
                            lbl.Visible = true;
                            lbl.BackColor = Color.Transparent;
                            lbl.ForeColor = Color.FromArgb(60, 60, 60);
                            lbl.Font = new Font("Microsoft YaHei UI", 10F);
                        }
                    }
                }
                
                // 强制重绘以确保所有变更生效
                this.Invalidate(true);
                this.Update();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"确保控件可见和样式时出错: {ex.Message}");
            }
        }

        // 完全重建筛选区域的新方法
        private void RecreateFilerPanel()
        {
            try
            {
                // 1. 找到原始的filterPanel位置和父控件
                Control filterPanelParent = panelTop; // 假设filterPanel的父容器是panelTop
                Point filterPanelLocation = new Point(0, 0);
                Size filterPanelSize = new Size(panelTop.Width, 60);
                
                if (filterPanel != null)
                {
                    filterPanelParent = filterPanel.Parent;
                    filterPanelLocation = filterPanel.Location;
                    filterPanelSize = filterPanel.Size;
                    
                    // 保存数据源和事件处理
                    ComboBox oldComboBox = cmbGroupTypeFilter;
                    TextBox oldTextBox = txtGroupIdSearch;
                    object comboDataSource = null;
                    string comboDisplayMember = null;
                    string comboValueMember = null;
                    object selectedValue = null;
                    
                    if (oldComboBox != null && oldComboBox.DataSource != null)
                    {
                        comboDataSource = oldComboBox.DataSource;
                        comboDisplayMember = oldComboBox.DisplayMember;
                        comboValueMember = oldComboBox.ValueMember;
                        selectedValue = oldComboBox.SelectedValue;
                    }
                    
                    // 2. 从父容器中移除原始的filterPanel
                    filterPanelParent.Controls.Remove(filterPanel);
                    filterPanel.Dispose();
                    
                    // 3. 创建新的Panel替代TableLayoutPanel
                    Panel newFilterPanel = new Panel();
                    newFilterPanel.Name = "filterPanel_new";
                    newFilterPanel.Location = filterPanelLocation;
                    newFilterPanel.Size = filterPanelSize;
                    newFilterPanel.BackColor = Color.FromArgb(248, 249, 250);
                    newFilterPanel.BorderStyle = BorderStyle.None;
                    filterPanelParent.Controls.Add(newFilterPanel);
                    
                    // 4. 创建和添加所有需要的控件
                    Label lblType = new Label();
                    lblType.Name = "lblType";
                    lblType.Text = "设备组类型:";
                    lblType.AutoSize = true;
                    lblType.Location = new Point(10, 22);
                    lblType.BackColor = Color.Transparent;
                    lblType.ForeColor = Color.FromArgb(60, 60, 60);
                    lblType.Font = new Font("Microsoft YaHei UI", 10F);
                    newFilterPanel.Controls.Add(lblType);
                    
                                // 创建新的下拉框，增强版本
            ComboBox newCmbType = new ComboBox();
            newCmbType.Name = "cmbGroupTypeFilter";
                                newCmbType.Size = new Size(300, 30); // 进一步增加宽度显示更多内容
            newCmbType.Location = new Point(lblType.Right + 5, 18);
            newCmbType.BackColor = Color.White;
            newCmbType.ForeColor = Color.FromArgb(40, 40, 40);
            newCmbType.Font = new Font("Microsoft YaHei UI", 10F);
            newCmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            
            // 全面优化下拉框显示设置
            newCmbType.DropDownHeight = 350; // 进一步增加下拉框最大高度
            newCmbType.IntegralHeight = false; // 允许部分项显示
            newCmbType.DrawMode = DrawMode.OwnerDrawFixed; // 自定义绘制
            newCmbType.MaxDropDownItems = 15; // 进一步增加显示项数量
            newCmbType.AutoSize = true; // 尝试自动调整大小
            
            // 完全重写下拉框绘制方法，确保显示完整内容
            newCmbType.DrawItem += (s, e) => {
                if (e.Index < 0) return;
                e.DrawBackground();
                
                // 获取完整文本
                string itemText = newCmbType.GetItemText(newCmbType.Items[e.Index]);
                
                // 自动调整下拉框宽度以适应最长的项
                using (Graphics g = newCmbType.CreateGraphics())
                {
                    int itemWidth = (int)g.MeasureString(itemText, newCmbType.Font).Width;
                    if (itemWidth > newCmbType.DropDownWidth)
                    {
                        newCmbType.DropDownWidth = itemWidth + 50; // 添加额外空间
                    }
                }
                
                                    // 扩大绘制区域，以便完整显示文本
                Rectangle drawRect = new Rectangle(
                    e.Bounds.X + 2,       // 左边留出2像素间距
                    e.Bounds.Y + 2,       // 顶部留出2像素间距
                    e.Bounds.Width + 300, // 大幅扩展绘制区域宽度
                    e.Bounds.Height - 4   // 高度缩小4像素
                );
                
                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    // 使用简化的绘制方法，确保文本完整显示
                    // 直接在指定位置绘制文本，不使用复杂格式
                    e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds.X + 2, e.Bounds.Y + 2);
                    
                    // 额外调整下拉宽度
                    ComboBox cmb = s as ComboBox;
                    if (cmb != null)
                    {
                        using (Graphics g = cmb.CreateGraphics())
                        {
                            SizeF textSize = g.MeasureString(itemText, cmb.Font);
                            if (textSize.Width > cmb.DropDownWidth)
                            {
                                // 在UI线程上执行修改
                                cmb.BeginInvoke(new Action(() => {
                                    cmb.DropDownWidth = (int)textSize.Width + 30;
                                }));
                            }
                        }
                    }
                }
                
                // 只在选中或悬停时绘制焦点矩形
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                    (e.State & DrawItemState.Focus) == DrawItemState.Focus)
                {
                    e.DrawFocusRectangle();
                }
            };
            
            newFilterPanel.Controls.Add(newCmbType);
                    
                    // 如果有原始数据源，设置到新下拉框
                    if (comboDataSource != null)
                    {
                        newCmbType.DataSource = comboDataSource;
                        newCmbType.DisplayMember = comboDisplayMember;
                        newCmbType.ValueMember = comboValueMember;
                        try
                        {
                            newCmbType.SelectedValue = selectedValue;
                        }
                        catch
                        {
                            if (newCmbType.Items.Count > 0)
                                newCmbType.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        // 如果没有原始数据，尝试重新加载
                        try
                        {
                            List<EqpGroup> groups = eqpGroupService.GetAllEqpGroupsForFilter();
                            EqpGroup allOption = new EqpGroup { EqpGroupId = "All", EqpGroupDescription = "(全部)" }; 
                            groups.Insert(0, allOption); 
                            
                            newCmbType.DataSource = groups;
                            newCmbType.DisplayMember = "DisplayInfo"; 
                            newCmbType.ValueMember = "EqpGroupId"; 
                            
                            if (newCmbType.Items.Count > 0)
                            {
                                newCmbType.SelectedIndex = 0; 
                            }
                        }
                        catch
                        {
                            // 创建一个基本选项
                            newCmbType.Items.Add("(全部)");
                            newCmbType.SelectedIndex = 0;
                        }
                    }
                    
                    // 创建ID标签
                    Label lblId = new Label();
                    lblId.Name = "lblId";
                    lblId.Text = "设备组编号:";
                    lblId.AutoSize = true;
                    lblId.Location = new Point(newCmbType.Right + 20, 22);
                    lblId.BackColor = Color.Transparent;
                    lblId.ForeColor = Color.FromArgb(60, 60, 60);
                    lblId.Font = new Font("Microsoft YaHei UI", 10F);
                    newFilterPanel.Controls.Add(lblId);
                    
                    // 创建ID文本框
                    TextBox newTxtId = new TextBox();
                    newTxtId.Name = "txtGroupIdSearch";
                    newTxtId.Size = new Size(150, 30);
                    newTxtId.Location = new Point(lblId.Right + 5, 18);
                    newTxtId.BackColor = Color.White;
                    newTxtId.ForeColor = Color.FromArgb(40, 40, 40);
                    newTxtId.Font = new Font("Microsoft YaHei UI", 10F);
                    newTxtId.BorderStyle = BorderStyle.FixedSingle;
                    
                    // 如果有旧文本框的值，复制过来
                    if (oldTextBox != null && !string.IsNullOrEmpty(oldTextBox.Text))
                    {
                        newTxtId.Text = oldTextBox.Text;
                    }
                    
                    // 添加回车键事件
                    newTxtId.KeyDown += (s, e) => {
                        if (e.KeyCode == Keys.Enter)
                        {
                            LoadData();
                            e.SuppressKeyPress = true;
                        }
                    };
                    
                    newFilterPanel.Controls.Add(newTxtId);
                    
                    // 创建搜索按钮
                    Button newBtnSearch = new Button();
                    newBtnSearch.Name = "btnSearch";
                    newBtnSearch.Text = "🔍 搜索";
                    newBtnSearch.Size = new Size(80, 30);
                    newBtnSearch.Location = new Point(newTxtId.Right + 10, 18);
                    newBtnSearch.FlatStyle = FlatStyle.Flat;
                    newBtnSearch.FlatAppearance.BorderSize = 0;
                    newBtnSearch.BackColor = Color.FromArgb(100, 151, 177);
                    newBtnSearch.ForeColor = Color.White;
                    newBtnSearch.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
                    newBtnSearch.Cursor = Cursors.Hand;
                    
                    // 添加圆角效果
                    try {
                        GraphicsPath path = new GraphicsPath();
                        ButtonRoundedCorners.AddRoundedRectangle(path, new Rectangle(0, 0, newBtnSearch.Width, newBtnSearch.Height), 5);
                        newBtnSearch.Region = new Region(path);
                    } catch {}
                    
                    // 添加点击事件
                    newBtnSearch.Click += (s, e) => {
                        LoadData();
                    };
                    
                    newFilterPanel.Controls.Add(newBtnSearch);
                    
                    // 重新关联控件到类成员变量
                    cmbGroupTypeFilter = newCmbType;
                    txtGroupIdSearch = newTxtId;
                    btnSearch = newBtnSearch;
                    filterPanel = null; // 不再引用旧的TableLayoutPanel
                    
                    // 确保所有控件可见
                    foreach (Control ctrl in newFilterPanel.Controls)
                    {
                        ctrl.Visible = true;
                        ctrl.BringToFront();
                    }
                    
                    newFilterPanel.Invalidate();
                    newFilterPanel.Update();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"重建筛选区域时出错: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }

    // 添加绘制圆角矩形的扩展方法
    static class ButtonRoundedCorners
    {
        public static void AddRoundedRectangle(this GraphicsPath path, Rectangle bounds, int radius)
        {
            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return;
            }
            
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            
            // 左上角
            path.AddArc(arc, 180, 90);
            
            // 右上角
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            
            // 右下角
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            
            // 左下角
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            
            path.CloseFigure();
        }
    }
} 