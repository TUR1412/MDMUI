using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
// using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// using System.Configuration;
using MDMUI.Model;
using MDMUI.BLL; // 引入 BLL

namespace MDMUI
{
    /// <summary>
    /// 部门编辑窗体
    /// </summary>
    public partial class FrmDepartmentEdit : Form
    {
        private bool isNew = true;                // 是否为新增
        private string departmentId = string.Empty; // 部门编号
        private string initialFactoryId = string.Empty;    // 传入的工厂编号（用于新增时的默认选择）
        private User currentUser; 
        private DepartmentService departmentService; // 添加 Service 引用
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True"; // 移除
        
        /// <summary>
        /// Constructor - Handles both Add and Edit
        /// </summary>
        /// <param name="deptId">Department ID (null or empty for new)</param>
        /// <param name="factoryId">Parent Factory ID</param>
        /// <param name="user">Current User</param>
        public FrmDepartmentEdit(string deptId, string factoryId, User user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.initialFactoryId = factoryId; // 记录传入的工厂ID
            this.departmentId = deptId;
            this.isNew = string.IsNullOrEmpty(deptId);
            this.departmentService = new DepartmentService(); // 实例化 Service

            this.Text = isNew ? "添加部门" : "编辑部门";
            this.Load += FrmDepartmentEdit_Load; // 绑定 Load 事件
            this.cboFactory.SelectedIndexChanged += CboFactory_SelectedIndexChanged; // 绑定工厂选择事件
            this.btnSave.Click += BtnSave_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }
        
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmDepartmentEdit_Load(object sender, EventArgs e)
        {
            try
            {
                LoadFactories(); // 加载工厂下拉列表
                
                if (isNew)
                {
                    // 新增模式
                    txtDepartmentId.ReadOnly = false;
                    // txtDepartmentId.Text = GenerateDepartmentId(); // 可以考虑移除自动生成，让用户输入或后端生成
                    // 默认选中传入的工厂ID
                     SelectComboBoxItemByValue(cboFactory, this.initialFactoryId);
                     // 根据选中的工厂加载上级部门 (此时 SelectedIndexChanged 可能已触发，或者手动调用)
                     if(cboFactory.SelectedIndex > -1) LoadDepartmentsByFactory(); 
                }
                else
                {
                    // 编辑模式
                    txtDepartmentId.ReadOnly = true; // ID 不可编辑
                    LoadDepartmentData(); // 加载部门数据 (此方法会选中正确的工厂和上级部门)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 加载工厂下拉列表 (调用 Service)
        /// </summary>
        private void LoadFactories()
        {
            try
            {
                cboFactory.Items.Clear();
                List<ComboboxItem> factoryItems = departmentService.GetFactoriesForComboBox(currentUser);
                cboFactory.Items.AddRange(factoryItems.ToArray());
                cboFactory.DisplayMember = "Text";
                cboFactory.ValueMember = "Value";
                
                // 设置默认选中项
                if (isNew && !string.IsNullOrEmpty(initialFactoryId)) {
                    SelectComboBoxItemByValue(cboFactory, initialFactoryId);
                }
                else if (cboFactory.Items.Count > 0)
                {
                    cboFactory.SelectedIndex = 0;
                }
                // 非管理员且有固定工厂时，禁用选择
                 cboFactory.Enabled = currentUser.RoleName == "超级管理员";
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载工厂数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 工厂选择改变时，加载对应工厂的部门 (调用 Service)
        /// </summary>
        private void CboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartmentsByFactory();
        }

        /// <summary>
        /// 根据当前选择的工厂加载部门列表 (调用 Service)
        /// </summary>
        private void LoadDepartmentsByFactory()
        {
            try
            {
                cboParentDept.DataSource = null; // 先清空数据源
                cboParentDept.Items.Clear();
                
                string selectedFactoryId = (cboFactory.SelectedItem as ComboboxItem)?.Value?.ToString();
                if (string.IsNullOrEmpty(selectedFactoryId)) 
                {
                     cboParentDept.Items.Add(new ComboboxItem("(无)", "")); 
                     cboParentDept.SelectedIndex = 0;
                     return;
                } 
                
                // 调用 Service 获取该工厂下的部门列表，并排除当前编辑的部门ID（如果是编辑模式）
                string excludeDeptId = isNew ? null : this.departmentId;
                List<ComboboxItem> deptItems = departmentService.GetDepartmentsForComboBox(selectedFactoryId, excludeDeptId);
                
                cboParentDept.Items.AddRange(deptItems.ToArray());
                cboParentDept.DisplayMember = "Text";
                cboParentDept.ValueMember = "Value";
                
                // 默认选中"(无)"
                 if (cboParentDept.Items.Count > 0) cboParentDept.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载上级部门数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 加载部门数据 (调用 Service)
        /// </summary>
        private void LoadDepartmentData()
        {
            try
            {
                 Department dept = departmentService.GetDepartmentById(this.departmentId);
                 if (dept != null)
                 {
                     txtDepartmentId.Text = dept.DeptId;
                     txtDepartmentName.Text = dept.DeptName;
                     txtDescription.Text = dept.Description;
                     
                     SelectComboBoxItemByValue(cboFactory, dept.FactoryId);
                     // 等待工厂加载完成后再加载部门列表，然后选中父部门
                     LoadDepartmentsByFactory(); 
                     SelectComboBoxItemByValue(cboParentDept, dept.ParentDeptId);
                     // 选中负责人
                     txtManager.Text = dept.ManagerEmployeeId ?? "";
                 }
                 else
                 {
                     MessageBox.Show("未找到部门信息！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     this.Close();
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载部门数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 辅助方法：根据值选中 ComboBox 项
        /// </summary>
        private void SelectComboBoxItemByValue(ComboBox comboBox, object value)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
            {
                 // Select the "(无)" item if value is null or empty
                 for (int i = 0; i < comboBox.Items.Count; i++)
                 {
                      if (comboBox.Items[i] is ComboboxItem item && item.Value != null && string.IsNullOrEmpty(item.Value.ToString()))
                      {
                           comboBox.SelectedIndex = i;
                           return;
                      }
                 }
                 // Fallback if "(无)" not found or items empty
                 if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0; 
                 else comboBox.SelectedIndex = -1;
                 return;
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i] is ComboboxItem item && item.Value != null && item.Value.Equals(value))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
                // Handle DataRowView if needed (though we aim for ComboboxItem)
                 else if (comboBox.Items[i] is DataRowView rowView && rowView.Row[comboBox.ValueMember].Equals(value))
                 {
                      comboBox.SelectedIndex = i;
                      return;
                 }
            }
             // If no match found, default to the first item (usually "(无)")
            if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0; 
            else comboBox.SelectedIndex = -1;
        }

        // 移除旧的 SelectFactoryById 和 SelectDepartmentById
        
        // 移除自动生成ID方法，让用户输入或由后端处理
        // private string GenerateDepartmentId() { ... }
        
        /// <summary>
        /// 保存按钮点击事件 (调用 Service)
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 验证
            if (string.IsNullOrWhiteSpace(txtDepartmentId.Text))
            {
                MessageBox.Show("部门编号不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDepartmentId.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
            {
                MessageBox.Show("部门名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDepartmentName.Focus();
                return;
            }
             if (cboFactory.SelectedItem == null || string.IsNullOrEmpty((cboFactory.SelectedItem as ComboboxItem)?.Value?.ToString()))
            {
                MessageBox.Show("请选择所属工厂。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboFactory.Focus();
                return;
            }

            // 收集数据
            Department dept = new Department
            {
                DeptId = txtDepartmentId.Text.Trim(),
                DeptName = txtDepartmentName.Text.Trim(),
                FactoryId = (cboFactory.SelectedItem as ComboboxItem).Value.ToString(),
                ParentDeptId = (cboParentDept.SelectedItem as ComboboxItem)?.Value?.ToString(),
                ManagerEmployeeId = string.IsNullOrWhiteSpace(txtManager.Text) ? null : txtManager.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };
             // 处理父部门ID，如果选的是"(无)"，则设为null
            if (string.IsNullOrEmpty(dept.ParentDeptId)) dept.ParentDeptId = null;

            try
            {
                bool success = false;
                if (isNew)
                {
                    success = departmentService.AddDepartment(dept, currentUser);
                }
                else
                {
                     // 对于更新，DeptId 不应改变，直接使用 this.departmentId
                     dept.DeptId = this.departmentId;
                    success = departmentService.UpdateDepartment(dept, currentUser);
                }

                if (success)
                {
                    MessageBox.Show("保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                 // 异常由 BLL/DAL 抛出，在此捕获
            }
            catch (ArgumentException argEx) // BLL 验证异常
            {
                 MessageBox.Show(argEx.Message, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException opEx) // BLL 业务逻辑异常
            {
                 MessageBox.Show(opEx.Message, "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // 包括 DAL 的主键冲突、数据库连接等异常
            {
                MessageBox.Show("保存部门数据时发生错误：\n" + ex.Message, "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}