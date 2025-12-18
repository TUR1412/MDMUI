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
            bool isAdmin = currentUser.RoleName == "超级管理员";
            
            if (this.toolStrip1 == null) return;

            ToolStripItem btnAdd = toolStrip1.Items["toolStripButtonAdd"];
            ToolStripItem btnEdit = toolStrip1.Items["toolStripButtonEdit"];
            ToolStripItem btnDelete = toolStrip1.Items["toolStripButtonDelete"];

            if (btnAdd == null || btnEdit == null || btnDelete == null)
            {
                Console.WriteLine("警告: FrmDepartment 工具栏按钮未全部找到。");
            }

            bool canAdd = isAdmin || permissionChecker.HasPermission(currentUser.Id, "department", "add");
            bool canEdit = isAdmin || permissionChecker.HasPermission(currentUser.Id, "department", "edit");
            bool canDelete = isAdmin || permissionChecker.HasPermission(currentUser.Id, "department", "delete");

            if (btnAdd != null) btnAdd.Enabled = canAdd;
            if (btnEdit != null) btnEdit.Enabled = canEdit;
            if (btnDelete != null) btnDelete.Enabled = canDelete;
        }

        /// <summary>
        /// 加载工厂数据到下拉列表
        /// </summary>
        private void LoadFactories()
        {
            try
            {
                List<ComboboxItem> factoryItems = departmentService.GetFactoriesForComboBox(currentUser);

                this.toolStripComboBoxFactory.ComboBox.DataSource = null;
                this.toolStripComboBoxFactory.ComboBox.Items.Clear();
                
                if (currentUser.RoleName == "超级管理员")
                {
                    this.toolStripComboBoxFactory.ComboBox.Items.Add(new ComboboxItem("所有工厂", ""));
                }
                
                this.toolStripComboBoxFactory.ComboBox.Items.AddRange(factoryItems.ToArray());
                this.toolStripComboBoxFactory.ComboBox.DisplayMember = "Text";
                this.toolStripComboBoxFactory.ComboBox.ValueMember = "Value";

                if (currentUser.RoleName != "超级管理员" && !string.IsNullOrEmpty(currentUser.FactoryId))
                {
                    SelectComboBoxItemByValue(this.toolStripComboBoxFactory.ComboBox, currentUser.FactoryId);
                    this.toolStripComboBoxFactory.Enabled = false; 
                }
                else
                {
                    this.toolStripComboBoxFactory.Enabled = true;
                    if (this.toolStripComboBoxFactory.ComboBox.Items.Count > 0)
                        this.toolStripComboBoxFactory.ComboBox.SelectedIndex = 0;
                }
                this.toolStripComboBoxFactory.ComboBox.Width = 150;
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
                string selectedFactoryId = (this.toolStripComboBoxFactory.ComboBox.SelectedItem as ComboboxItem)?.Value?.ToString();
                string searchTerm = this.toolStripTextBoxSearchName.Text.Trim();

                if (this.toolStripComboBoxFactory.ComboBox.SelectedItem == null && currentUser.RoleName != "超级管理员")
                {
                    this.dataGridViewDepartments.DataSource = null;
                    UpdateStatus("请先确保工厂列表已加载");
                    return;
                }
                
                if (currentUser.RoleName != "超级管理员" && string.IsNullOrEmpty(selectedFactoryId))
                {
                    ComboboxItem selectedItem = this.toolStripComboBoxFactory.ComboBox.SelectedItem as ComboboxItem;
                    if (selectedItem != null && selectedItem.Value.ToString() == "")
                    {
                        this.dataGridViewDepartments.DataSource = null;
                        UpdateStatus("请选择一个具体的工厂进行查看");
                        return;
                    }
                    else if(string.IsNullOrEmpty(currentUser.FactoryId)) {
                        this.dataGridViewDepartments.DataSource = null;
                        UpdateStatus("当前用户未关联工厂，无法加载部门");
                        return;
                    }
                }

                if (string.IsNullOrEmpty(selectedFactoryId))
                {
                    currentDepartments = departmentService.GetAllDepartments();
                    UpdateStatus($"加载了所有工厂共 {currentDepartments.Count} 条部门信息");
                }
                else
                {
                    currentDepartments = departmentService.GetDepartmentsByFactoryId(selectedFactoryId);
                    UpdateStatus($"加载了工厂 [{this.toolStripComboBoxFactory.ComboBox.Text}] 共 {currentDepartments.Count} 条部门信息");
                }

                List<Department> filteredDepartments = currentDepartments;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    filteredDepartments = currentDepartments
                        .Where(d => d.DeptName != null && d.DeptName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                    UpdateStatus($"筛选结果: {filteredDepartments.Count} 条");
                }

                var bindingList = new BindingList<Department>(filteredDepartments);
                var source = new BindingSource(bindingList, null);
                this.dataGridViewDepartments.DataSource = source;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载部门数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("加载部门数据失败");
                this.dataGridViewDepartments.DataSource = null;
                currentDepartments = new List<Department>();
            }
        }

        /// <summary>
        /// 配置 DataGridView (最好在 Load 事件或设计器中完成一次性设置)
        /// </summary>
        private void ConfigureDataGridView()
        {
            typeof(DataGridView).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(dataGridViewDepartments, true, null);

            dataGridViewDepartments.AutoGenerateColumns = false;
            dataGridViewDepartments.Columns.Clear();

            dataGridViewDepartments.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeptId", DataPropertyName = "DeptId", HeaderText = "部门ID", Width = 120 });
            dataGridViewDepartments.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeptName", DataPropertyName = "DeptName", HeaderText = "部门名称", Width = 150 });
            // Add other columns as needed here, e.g., ParentDeptName, ManagerName, CreateTime, UpdateTime
            // Ensure DataPropertyName matches the properties in the Department model
            // Example:
            // dataGridViewDepartments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colParentDeptName", DataPropertyName = "ParentDeptName", HeaderText = "上级部门", Width = 150 });
            // dataGridViewDepartments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colManagerName", DataPropertyName = "ManagerName", HeaderText = "负责人", Width = 120 });
            // dataGridViewDepartments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCreateTime", DataPropertyName = "CreateTime", HeaderText = "创建时间", Width = 140, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm" } });
            // dataGridViewDepartments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUpdateTime", DataPropertyName = "UpdateTime", HeaderText = "更新时间", Width = 140, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm" } });
            // dataGridViewDepartments.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDescription", DataPropertyName = "Description", HeaderText = "描述", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        private void UpdateStatus(string message)
        { 
             if (this.toolStripStatusLabelStatus != null)
            {
                this.toolStripStatusLabelStatus.Text = message;
            }
        }

        /// <summary>
        /// 添加按钮点击事件 (调用 Service)
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // 权限已在 SetButtonPermissions 中检查和禁用按钮
            // string selectedFactoryId = this.toolStripComboBoxFactory.ComboBox.SelectedValue?.ToString(); // Original problematic line
            
            // Safer way to get FactoryId
            string selectedFactoryId = null;
            ComboboxItem selectedFactoryItem = this.toolStripComboBoxFactory.ComboBox.SelectedItem as ComboboxItem;
            if (selectedFactoryItem != null && selectedFactoryItem.Value != null)
            {
                selectedFactoryId = selectedFactoryItem.Value.ToString();
            }

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

            // Attempt to get DeptId and DeptName from the selected row
            // Ensure the column names ("DeptId", "DeptName") match the actual DataGridView column names or DataPropertyNames
            string deptId = this.dataGridViewDepartments.SelectedRows[0].Cells["DeptId"]?.Value?.ToString();
            string deptName = this.dataGridViewDepartments.SelectedRows[0].Cells["DeptName"]?.Value?.ToString() ?? "选定部门"; // Default name if fetching fails

            if (string.IsNullOrEmpty(deptId))
            {
                 MessageBox.Show("无法获取选定部门的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

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
            if (this.toolStripComboBoxFactory.ComboBox.Focused || MouseButtons == MouseButtons.Left) 
            {
                 UpdateStatus("工厂已更改，正在重新加载部门列表...");
                 LoadDepartments();
            }
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

                 // Safer way to get DeptId
                 string deptId = this.dataGridViewDepartments.SelectedRows[0].Cells["DeptId"]?.Value?.ToString();
                 if (string.IsNullOrEmpty(deptId))
                 {
                     MessageBox.Show("无法获取选定部门的ID。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                 }

                 // Safer way to get FactoryId
                 string factoryId = null;
                 ComboboxItem selectedFactoryItem = this.toolStripComboBoxFactory.ComboBox.SelectedItem as ComboboxItem;
                 if (selectedFactoryItem != null && selectedFactoryItem.Value != null)
                 {
                     factoryId = selectedFactoryItem.Value.ToString();
                 }

                 // Ensure factoryId was obtained
                 if (string.IsNullOrEmpty(factoryId))
                 {
                     // This can happen if "所有工厂" is selected or due to other issues.
                     // Assuming editing requires a specific factory context.
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