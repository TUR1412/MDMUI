using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;

namespace MDMUI
{
    /// <summary>
    /// 表单操作模式
    /// </summary>
    public enum FormMode
    {
        Add,
        Edit
    }

    public partial class FrmSubDeviceEdit : Form
    {
        private string parentEqpGroupId;
        private string subDeviceId;
        private bool isEdit;
        private User currentUser;
        private SubDeviceService subDeviceService;

        /// <summary>
        /// 构造函数 - 添加新子设备
        /// </summary>
        /// <param name="parentEqpGroupId">父设备组ID</param>
        /// <param name="currentUser">当前用户</param>
        public FrmSubDeviceEdit(string parentEqpGroupId, User currentUser)
        {
            InitializeComponent();
            this.parentEqpGroupId = parentEqpGroupId;
            this.currentUser = currentUser;
            this.isEdit = false;
            this.subDeviceService = new SubDeviceService();
            
            // 设置窗体标题
            this.Text = "添加子设备";
            lblTitle.Text = "添加子设备";
        }

        /// <summary>
        /// 构造函数 - 编辑现有子设备
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <param name="currentUser">当前用户</param>
        /// <param name="mode">表单操作模式，必须是FormMode.Edit</param>
        public FrmSubDeviceEdit(string subDeviceId, User currentUser, FormMode mode)
        {
            if (mode != FormMode.Edit)
                throw new ArgumentException("此构造函数仅用于编辑模式");

            InitializeComponent();
            this.subDeviceId = subDeviceId;
            this.currentUser = currentUser;
            this.isEdit = true;
            this.subDeviceService = new SubDeviceService();
            
            // 设置窗体标题
            this.Text = "编辑子设备";
            lblTitle.Text = "编辑子设备";
        }

        private void FrmSubDeviceEdit_Load(object sender, EventArgs e)
        {
            try
            {
                SetupFormStyle();
                
                // 加载设备类型到下拉框
                LoadDeviceTypes();
                
                // 如果是编辑模式，加载现有数据
                if (isEdit && !string.IsNullOrEmpty(subDeviceId))
                {
                    LoadSubDeviceData();
                }
                else
                {
                    // 新增模式，默认值设置
                    // 生成新的子设备ID (可以在保存时自动生成)
                    txtSubDeviceId.Text = GenerateNewSubDeviceId();
                    txtSubDeviceId.ReadOnly = true; // 不允许修改自动生成的ID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化窗体时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void SetupFormStyle()
        {
            // 设置窗体样式
            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.BackColor = Color.WhiteSmoke;
            
            // 标题样式
            lblTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            lblTitle.ForeColor = Color.FromArgb(64, 64, 64);
            
            // 按钮样式
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnSave.BackColor = Color.FromArgb(100, 151, 177);
            btnSave.ForeColor = Color.White;
            
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            
            // 面板样式
            panelTop.BackColor = Color.FromArgb(240, 240, 240);
            panelBottom.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void LoadDeviceTypes()
        {
            // 加载设备类型选项
            cboDeviceType.Items.Clear();
            cboDeviceType.Items.Add("PLC模块");
            cboDeviceType.Items.Add("六轴机械手");
            cboDeviceType.Items.Add("控制器");
            cboDeviceType.Items.Add("接口卡");
            cboDeviceType.Items.Add("AGV");
            cboDeviceType.Items.Add("其他");
            
            if (cboDeviceType.Items.Count > 0)
            {
                cboDeviceType.SelectedIndex = 0;
            }
        }
        
        private string GenerateNewSubDeviceId()
        {
            // 生成规则: SD + 3位数字
            try
            {
                // 获取现有最大ID
                int maxId = subDeviceService.GetMaxSubDeviceId();
                return $"SD{(maxId + 1).ToString("D3")}"; // 格式化为三位数，例如SD001
            }
            catch
            {
                // 发生异常时使用时间戳
                return $"SD{DateTime.Now.ToString("yyMMddHHmm")}";
            }
        }
        
        private void LoadSubDeviceData()
        {
            try
            {
                // 获取子设备数据
                DataTable subDeviceData = subDeviceService.GetSubDeviceById(subDeviceId);
                
                if (subDeviceData != null && subDeviceData.Rows.Count > 0)
                {
                    DataRow row = subDeviceData.Rows[0];
                    
                    // 填充表单
                    txtSubDeviceId.Text = row["sub_device_id"].ToString();
                    txtSubDeviceId.ReadOnly = true; // 编辑模式下ID不可修改
                    
                    txtSubDeviceName.Text = row["sub_device_name"].ToString();
                    
                    string deviceType = row["sub_device_type"].ToString();
                    for (int i = 0; i < cboDeviceType.Items.Count; i++)
                    {
                        if (cboDeviceType.Items[i].ToString() == deviceType)
                        {
                            cboDeviceType.SelectedIndex = i;
                            break;
                        }
                    }
                    
                    // 保存所属设备组ID
                    this.parentEqpGroupId = row["eqp_group_id"].ToString();
                }
                else
                {
                    MessageBox.Show($"无法找到子设备 {subDeviceId} 的数据。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载子设备数据时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtSubDeviceId.Text))
                {
                    MessageBox.Show("子设备ID不能为空。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSubDeviceId.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtSubDeviceName.Text))
                {
                    MessageBox.Show("子设备名称不能为空。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSubDeviceName.Focus();
                    return;
                }
                
                if (cboDeviceType.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择设备类型。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboDeviceType.Focus();
                    return;
                }
                
                // 创建子设备对象
                SubDevice subDevice = new SubDevice
                {
                    SubDeviceId = txtSubDeviceId.Text.Trim(),
                    SubDeviceName = txtSubDeviceName.Text.Trim(),
                    SubDeviceType = cboDeviceType.SelectedItem.ToString(),
                    EqpGroupId = parentEqpGroupId,
                };
                
                bool success;
                if (isEdit)
                {
                    // 更新现有子设备
                    success = subDeviceService.UpdateSubDevice(subDevice, currentUser.Id.ToString());
                }
                else
                {
                    // 添加新子设备
                    success = subDeviceService.AddSubDevice(subDevice, currentUser.Id.ToString());
                }

                // 操作成功后直接返回，不显示消息框
                if (success)
                {
                    // 设置对话框结果，通知调用者操作成功
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    string action = isEdit ? "更新" : "添加";
                    MessageBox.Show($"子设备{action}失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存子设备时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 