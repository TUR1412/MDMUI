using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;
using System.Linq;
using System.Collections.Generic;

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

        public FrmFactory(User user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.permissionChecker = new PermissionChecker();
            this.factoryService = new FactoryService(); // 初始化 FactoryService
            this.Load += FrmFactory_Load;

            // Configure DataGridView columns - 最好在这里或Designer中配置一次
            ConfigureDataGridView();
            SetButtonVisibility();
        }

        private void FrmFactory_Load(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine($"工厂管理窗体加载 - 用户: {currentUser.Username}, 工厂: {currentUser.FactoryId}");
                LoadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"工厂管理窗体加载错误: {ex.Message}");
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
                List<Factory> factories = factoryService.GetFactories(currentUser, searchTerm);
                
                // 使用 BindingSource 以获得更好的数据绑定体验
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = factories;
                this.dataGridView1.DataSource = bindingSource;

                // 更新状态或日志
                Console.WriteLine($"已加载 {factories.Count} 个工厂数据。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 this.dataGridView1.DataSource = null; // 出错时清空 DGV
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