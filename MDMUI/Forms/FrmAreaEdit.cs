using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
// using System.Data.SqlClient; // 移除 SQL Client 引用
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.Model; // 确保引入了 Area 和 ComboboxItem 模型
using MDMUI.BLL; // 引入 BLL

namespace MDMUI
{
    /// <summary>
    /// 区域编辑窗体
    /// </summary>
    public partial class FrmAreaEdit : Form
    {
        private bool isNew = true;             // 是否为新增
        private string areaId = string.Empty;  // 当前编辑的区域ID
        private AreaService areaService;     // 添加 AreaService 引用
        // 移除硬编码连接字符串
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True";
        private User currentUser; // 添加字段以接收当前用户（可选，用于日志记录）

        /// <summary>
        /// 构造函数 - 新增区域
        /// </summary>
        public FrmAreaEdit(User user = null) // 可选接收当前用户
        {
            isNew = true;
            InitializeComponent();
            this.areaService = new AreaService(); // 初始化 Service
            this.currentUser = user;
            this.Text = "添加区域";
        }

        /// <summary>
        /// 构造函数 - 编辑区域
        /// </summary>
        /// <param name="areaId">要编辑的区域ID</param>
        /// <param name="user">当前用户 (可选)</param>
        public FrmAreaEdit(string areaId, User user = null) 
        {
            isNew = false;
            this.areaId = areaId;
            InitializeComponent();
            this.areaService = new AreaService(); // 初始化 Service
            this.currentUser = user;
            this.Text = "编辑区域";
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmAreaEdit_Load(object sender, EventArgs e)
        {
            try
            {
                LoadParentAreas(); // 加载上级区域下拉列表

                if (!isNew)
                {
                    LoadAreaData(); // 如果是编辑模式，加载现有数据
                    txtAreaId.ReadOnly = true; // 编辑模式下ID不可修改
                }
                else
                {
                   // 新增模式下ID允许用户输入
                   txtAreaId.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载上级区域下拉列表
        /// </summary>
        private void LoadParentAreas()
        {
            try
            {
                cboParentArea.Items.Clear();
                // 添加一个表示"无上级"或"根区域"的选项
                cboParentArea.Items.Add(new ComboboxItem("(无)", "")); // 值为空字符串

                // 从 AreaService 获取数据
                List<ComboboxItem> parentAreaItems = areaService.GetAreasForComboBox();
                
                foreach (ComboboxItem item in parentAreaItems)
                {
                    // 编辑模式下，过滤掉自身，防止选择自己作为父节点
                    if (!isNew && item.Value.ToString() == this.areaId)
                    {
                        continue;
                    }
                    // TODO: 更复杂的逻辑 - 过滤掉自身的子孙节点以防止循环引用
                    cboParentArea.Items.Add(item);
                }

                 // 默认选中"(无)"
                cboParentArea.SelectedIndex = 0;
                cboParentArea.DisplayMember = "Text"; // 确保显示 Text
                cboParentArea.ValueMember = "Value";   // 确保 ValueMember 设置正确 (虽然我们直接用 SelectedItem.Value)
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载上级区域数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载现有区域数据（用于编辑模式）
        /// </summary>
        private void LoadAreaData()
        {
            try
            {
                // 调用 AreaService 获取数据
                Area area = areaService.GetAreaById(this.areaId);

                if (area != null)
                {
                    txtAreaId.Text = area.AreaId;
                    txtAreaName.Text = area.AreaName;
                    txtPostalCode.Text = area.PostalCode ?? "";
                    txtRemark.Text = area.Remark ?? "";

                    // 设置上级区域下拉框的选中项
                    SelectParentAreaById(area.ParentAreaId);
                }
                else
                {
                     MessageBox.Show("未找到指定的区域数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     this.Close(); // 关闭窗口如果数据找不到
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载区域数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

         /// <summary>
        /// 根据 ParentAreaId 在下拉列表中选中对应的项
        /// </summary>
        /// <param name="parentId">上级区域ID</param>
        private void SelectParentAreaById(string parentId)
        {
            // 如果 parentId 为空或 null，选中"(无)"
            if (string.IsNullOrEmpty(parentId))
            {
                 // 安全地设置索引，防止 Items 为空时出错
                 if (cboParentArea.Items.Count > 0) 
                 {
                     cboParentArea.SelectedIndex = 0;
                 }
                return;
            }

            for (int i = 0; i < cboParentArea.Items.Count; i++)
            {
                // 使用 as 进行类型转换，更安全
                if (cboParentArea.Items[i] is ComboboxItem item && item.Value != null && item.Value.ToString() == parentId)
                {
                    cboParentArea.SelectedIndex = i;
                    return;
                }
            }
             // 如果找不到匹配项，默认选中"(无)"
             if (cboParentArea.Items.Count > 0)
             {
                cboParentArea.SelectedIndex = 0;
             }
        }


        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 基本验证
            if (string.IsNullOrWhiteSpace(txtAreaId.Text))
            {
                MessageBox.Show("区域编号不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAreaId.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAreaName.Text))
            {
                MessageBox.Show("区域名称不能为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAreaName.Focus();
                return;
            }

            // 获取父区域ID
            string parentId = (cboParentArea.SelectedItem as ComboboxItem)?.Value?.ToString();
            // 如果是根节点("(无)")，父ID 设为 null
            string parentAreaIdForDb = string.IsNullOrEmpty(parentId) ? null : parentId;

            // 创建 Area 对象
            Area area = new Area
            {
                AreaId = txtAreaId.Text.Trim(),
                AreaName = txtAreaName.Text.Trim(),
                ParentAreaId = parentAreaIdForDb,
                PostalCode = string.IsNullOrWhiteSpace(txtPostalCode.Text) ? null : txtPostalCode.Text.Trim(),
                Remark = string.IsNullOrWhiteSpace(txtRemark.Text) ? null : txtRemark.Text.Trim()
            };

            try
            {
                bool success = false;
                if (isNew)
                {
                     // 调用 AreaService 添加
                    success = areaService.AddArea(area, this.currentUser);
                }
                else
                {
                    // 调用 AreaService 更新
                    success = areaService.UpdateArea(area, this.currentUser);
                }

                if (success)
                {
                    MessageBox.Show("保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // 设置 DialogResult 以便父窗口知道操作成功
                    this.Close();
                }
                 // 如果 BLL/DAL 抛出异常，会被下面的 catch 捕获
            }
            catch (ArgumentException argEx) // 捕获 BLL 参数验证异常
            {
                 MessageBox.Show(argEx.Message, "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex) // 捕获其他异常 (如数据库主键冲突、连接失败等)
            {
                MessageBox.Show("保存区域数据时发生错误：\n" + ex.Message, "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 