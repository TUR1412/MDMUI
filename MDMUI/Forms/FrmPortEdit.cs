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
    public partial class FrmPortEdit : Form
    {
        private string parentSubDeviceId;
        private string portId;
        private bool isEdit;
        private User currentUser;
        private PortService portService;

        /// <summary>
        /// 构造函数 - 添加新端口
        /// </summary>
        /// <param name="parentSubDeviceId">父子设备ID</param>
        /// <param name="currentUser">当前用户</param>
        public FrmPortEdit(string parentSubDeviceId, User currentUser)
        {
            InitializeComponent();
            this.parentSubDeviceId = parentSubDeviceId;
            this.currentUser = currentUser;
            this.isEdit = false;
            this.portService = new PortService();
            
            // 设置窗体标题
            this.Text = "添加端口";
            lblTitle.Text = "添加端口";
        }

        /// <summary>
        /// 构造函数 - 编辑现有端口
        /// </summary>
        /// <param name="portId">端口ID</param>
        /// <param name="currentUser">当前用户</param>
        /// <param name="mode">表单操作模式，必须是FormMode.Edit</param>
        public FrmPortEdit(string portId, User currentUser, FormMode mode)
        {
            if (mode != FormMode.Edit)
                throw new ArgumentException("此构造函数仅用于编辑模式");
                
            InitializeComponent();
            this.portId = portId;
            this.currentUser = currentUser;
            this.isEdit = true;
            this.portService = new PortService();
            
            // 设置窗体标题
            this.Text = "编辑端口";
            lblTitle.Text = "编辑端口";
        }

        private void FrmPortEdit_Load(object sender, EventArgs e)
        {
            try
            {
                SetupFormStyle();
                
                // 加载端口类型和协议类型到下拉框
                LoadPortTypes();
                LoadProtocolTypes();
                
                // 如果是编辑模式，加载现有数据
                if (isEdit && !string.IsNullOrEmpty(portId))
                {
                    LoadPortData();
                }
                else
                {
                    // 新增模式，默认值设置
                    // 生成新的端口ID
                    txtPortId.Text = GenerateNewPortId();
                    txtPortId.ReadOnly = true; // 不允许修改自动生成的ID
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

        private void LoadPortTypes()
        {
            // 加载端口类型选项
            cboPortType.Items.Clear();
            cboPortType.Items.Add("COM");
            cboPortType.Items.Add("RJ45");
            cboPortType.Items.Add("USB");
            cboPortType.Items.Add("控制器");
            cboPortType.Items.Add("接口卡");
            cboPortType.Items.Add("其他");
            
            if (cboPortType.Items.Count > 0)
            {
                cboPortType.SelectedIndex = 0;
            }
        }
        
        private void LoadProtocolTypes()
        {
            // 加载协议类型选项
            cboProtocol.Items.Clear();
            cboProtocol.Items.Add("Modbus RTU");
            cboProtocol.Items.Add("Modbus TCP");
            cboProtocol.Items.Add("ProfiBus");
            cboProtocol.Items.Add("ProfiNet");
            cboProtocol.Items.Add("Ethernet/IP");
            cboProtocol.Items.Add("DeviceNet");
            cboProtocol.Items.Add("无");
            
            if (cboProtocol.Items.Count > 0)
            {
                cboProtocol.SelectedIndex = 0;
            }
        }
        
        private string GenerateNewPortId()
        {
            // 生成规则: P + 设备ID后缀 + 序号
            try
            {
                // 获取设备ID中的数字部分
                string deviceNumberPart = "";
                if (parentSubDeviceId != null && parentSubDeviceId.Length > 2)
                {
                    deviceNumberPart = parentSubDeviceId.Substring(2);
                }
                
                // 获取现有最大ID
                int maxId = portService.GetMaxPortNumberForDevice(parentSubDeviceId);
                return $"P{deviceNumberPart}-{(maxId + 1).ToString("D1")}"; // 例如P001-1
            }
            catch
            {
                // 发生异常时使用时间戳
                return $"PORT{DateTime.Now.ToString("yyMMddHHmm")}";
            }
        }
        
        private void LoadPortData()
        {
            try
            {
                // 获取端口数据
                DataTable portData = portService.GetPortById(portId);
                
                if (portData != null && portData.Rows.Count > 0)
                {
                    DataRow row = portData.Rows[0];
                    
                    // 填充表单
                    txtPortId.Text = row["port_id"].ToString();
                    txtPortId.ReadOnly = true; // 编辑模式下ID不可修改
                    
                    txtPortName.Text = row["port_name"].ToString();
                    txtPortNumber.Text = row["port_number"].ToString();
                    
                    string portType = row["port_type"].ToString();
                    for (int i = 0; i < cboPortType.Items.Count; i++)
                    {
                        if (cboPortType.Items[i].ToString() == portType)
                        {
                            cboPortType.SelectedIndex = i;
                            break;
                        }
                    }
                    
                    string protocol = row["protocol"].ToString();
                    for (int i = 0; i < cboProtocol.Items.Count; i++)
                    {
                        if (cboProtocol.Items[i].ToString() == protocol)
                        {
                            cboProtocol.SelectedIndex = i;
                            break;
                        }
                    }
                    
                    // 保存所属子设备ID
                    this.parentSubDeviceId = row["parent_device_id"].ToString();
                }
                else
                {
                    MessageBox.Show($"无法找到端口 {portId} 的数据。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载端口数据时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtPortId.Text))
                {
                    MessageBox.Show("端口ID不能为空。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPortId.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtPortName.Text))
                {
                    MessageBox.Show("端口名称不能为空。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPortName.Focus();
                    return;
                }
                
                if (cboPortType.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择端口类型。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboPortType.Focus();
                    return;
                }
                
                if (cboProtocol.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择协议类型。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboProtocol.Focus();
                    return;
                }
                
                // 创建端口对象
                Port port = new Port
                {
                    PortId = txtPortId.Text.Trim(),
                    PortName = txtPortName.Text.Trim(),
                    PortNumber = txtPortNumber.Text.Trim(),
                    PortType = cboPortType.SelectedItem.ToString(),
                    Protocol = cboProtocol.SelectedItem.ToString(),
                    ParentDeviceId = parentSubDeviceId
                };
                
                bool success;
                if (isEdit)
                {
                    // 更新现有端口
                    success = portService.UpdatePort(port, currentUser.Id.ToString());
                }
                else
                {
                    // 添加新端口
                    success = portService.AddPort(port, currentUser.Id.ToString());
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
                    MessageBox.Show($"端口{action}失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存端口时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 