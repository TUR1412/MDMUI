using System;
using System.Data;
// using System.Data.SqlClient; // 移除
using System.Drawing;
using System.Windows.Forms;
using MDMUI.Model;
using MDMUI.BLL; // 添加 BLL using
using MDMUI.DAL; // 添加 DAL using
using MDMUI.Utility; // 添加 Utility for SelectComboBoxItemByValue
using System.Collections.Generic; // 添加 Collections

namespace MDMUI
{
    /// <summary>
    /// 工厂编辑窗体
    /// </summary>
    public partial class FrmFactoryEdit : Form
    {
        private bool isNew = true;
        private string factoryId = string.Empty;
        private User currentUser;
        // 移除硬编码的连接字符串
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True";
        private FactoryService factoryService; // 添加 FactoryService 成员
        private EmployeeDAL employeeDAL; // 添加 EmployeeDAL 成员

        // 定义下拉框中表示"无"的值
        private const string NO_MANAGER_VALUE = "<NONE>";

        /// <summary>
        /// Constructor - Handles both Add and Edit
        /// </summary>
        /// <param name="factId">Factory ID (null or empty for new)</param>
        /// <param name="user">Current User</param>
        public FrmFactoryEdit(string factId, User user) // 修改 factId 为 string 类型
        {
            InitializeComponent();
            this.currentUser = user;
            this.factoryService = new FactoryService(); // 初始化 FactoryService
            this.employeeDAL = new EmployeeDAL(); // 初始化 EmployeeDAL

            if (!string.IsNullOrEmpty(factId))
            {
                isNew = false;
                this.factoryId = factId;
                txtFactoryId.ReadOnly = true; // Make ID read-only when editing
                txtFactoryId.BackColor = SystemColors.Control; // 视觉上表示只读
            }
            else
            {
                isNew = true;
                txtFactoryId.ReadOnly = false; // Allow ID input for new factory
                 txtFactoryId.BackColor = SystemColors.Window; // 恢复默认背景色
            }

            this.Text = isNew ? "添加工厂" : "编辑工厂";
            this.Load += FrmFactoryEdit_Load; // 将 Load 事件绑定移到构造函数
        }

        private void FrmFactoryEdit_Load(object sender, EventArgs e)
        {
            try
            {
                LoadEmployees(); // 先加载员工列表
                if (!isNew)
                {
                    LoadFactoryData(); // 再加载工厂数据（包含设置负责人）
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // 初始化失败则关闭窗口
            }
        }

        // 加载员工列表到 ComboBox
        private void LoadEmployees()
        {
            try
            {
                List<ComboboxItem> employees = employeeDAL.GetAllEmployeesForComboBox();

                // 添加一个表示"无负责人"的选项在最前面
                employees.Insert(0, new ComboboxItem("无", NO_MANAGER_VALUE)); 

                comboBoxManager.DataSource = employees;
                comboBoxManager.DisplayMember = "Text"; // 显示 EmployeeName
                comboBoxManager.ValueMember = "Value"; // 值是 EmployeeId 或 NO_MANAGER_VALUE
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载负责人列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // 即使加载失败，也允许继续操作，只是无法选择负责人
            }
        }

        // 重构 LoadFactoryData 使用 FactoryService
        private void LoadFactoryData()
        {
            try
            {
                Factory factory = factoryService.GetFactoryById(this.factoryId);
                if (factory != null)
                {
                    txtFactoryId.Text = factory.FactoryId;
                    txtFactoryName.Text = factory.FactoryName;
                    txtAddress.Text = factory.Address ?? "";
                    txtPhone.Text = factory.Phone ?? "";

                    // 根据 ManagerEmployeeId 设置 ComboBox 的选中项
                    if (!string.IsNullOrEmpty(factory.ManagerEmployeeId))
                    {
                        SelectComboBoxItemByValue(comboBoxManager, factory.ManagerEmployeeId);
                    }
                    else
                    {
                        SelectComboBoxItemByValue(comboBoxManager, NO_MANAGER_VALUE); // 如果没有负责人ID，选中"无"
                    }
                }
                else
                {
                    MessageBox.Show("未找到指定的工厂数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close(); // 如果未找到数据，关闭窗口
                }
            }
            /* // 移除旧的数据库访问代码
            catch (SqlException sqlEx)
            {
                 MessageBox.Show($"数据库操作失败: {sqlEx.Message}", "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  this.Close();
            }
            */
            catch (Exception ex)
            {
                MessageBox.Show("加载工厂数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // 加载失败则关闭窗口
            }
        }

        // 重构 BtnSave_Click 使用 FactoryService
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFactoryId.Text))
            {
                MessageBox.Show("工厂编号不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFactoryId.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFactoryName.Text))
            {
                MessageBox.Show("工厂名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFactoryName.Focus();
                return;
            }

            // 获取选中的负责人 EmployeeId
            string selectedManagerId = null;
            if (comboBoxManager.SelectedValue != null && comboBoxManager.SelectedValue.ToString() != NO_MANAGER_VALUE)
            {
                selectedManagerId = comboBoxManager.SelectedValue.ToString();
            }

            // 创建 Factory 对象
            Factory factory = new Factory
            {
                FactoryId = txtFactoryId.Text.Trim(),
                FactoryName = txtFactoryName.Text.Trim(),
                Address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim(),
                Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                ManagerEmployeeId = selectedManagerId // 设置负责人ID
            };

            try
            {
                bool success = false;
                if (isNew)
                {
                    success = factoryService.AddFactory(factory);
                }
                else
                {
                    success = factoryService.UpdateFactory(factory);
                }

                if (success)
                {
                    MessageBox.Show("保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Service 层应该通过异常传递具体错误，或者可以返回更详细的结果对象
                    MessageBox.Show("保存失败。可能的原因：数据验证未通过或数据库操作未影响任何行。", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException argEx) // 捕获模型验证错误
            {
                 MessageBox.Show("输入数据无效: " + argEx.Message, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException opEx) // 捕获业务逻辑错误 (如ID已存在)
            {
                MessageBox.Show(opEx.Message, "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (isNew) txtFactoryId.Focus(); // 如果是新增时 ID 重复，聚焦 ID 输入框
            }
            /* // 移除旧的数据库访问代码
            catch (SqlException sqlEx)
            {
                MessageBox.Show("数据库操作失败：\n" + sqlEx.Message, "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
            catch (Exception ex)
            {
                MessageBox.Show("保存工厂数据时发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 添加辅助方法：根据值选中 ComboBox 项 (类似 FrmDepartment)
        private void SelectComboBoxItemByValue(ComboBox comboBox, object value)
        {
            if (value == null) 
            {
                // 如果值为 null，尝试选中"无"选项
                value = NO_MANAGER_VALUE;
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                // 假设 DataSource 是 List<ComboboxItem>
                if (comboBox.Items[i] is ComboboxItem item && item.Value != null && item.Value.Equals(value))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
                // 如果 DataSource 是其他类型，可能需要调整这里的逻辑
            }
            // 如果找不到匹配项，可以选择默认选中第一项或不选中 (-1)
            if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0; 
            else comboBox.SelectedIndex = -1;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 