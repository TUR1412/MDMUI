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
using MDMUI.BLL;
using MDMUI.DAL; // Need DAL for Factory list

namespace MDMUI
{
    public partial class FrmEqpGroupEdit : Form
    {
        private User currentUser;
        private EqpGroup eqpGroup;
        private bool isNewGroup = true;
        private readonly EqpGroupService eqpGroupService;
        private readonly FactoryService factoryService;

        public FrmEqpGroupEdit(User user)
        {
            InitializeComponent();
            currentUser = user;
            eqpGroup = new EqpGroup();
            eqpGroupService = new EqpGroupService();
            factoryService = new FactoryService();
            Text = "添加设备组";
            SetupVisualStyle();
        }

        public FrmEqpGroupEdit(EqpGroup group, User user)
        {
            InitializeComponent();
            currentUser = user;
            eqpGroup = group;
            isNewGroup = false;
            eqpGroupService = new EqpGroupService();
            factoryService = new FactoryService();
            Text = "编辑设备组";
            SetupVisualStyle();
        }

        private void SetupVisualStyle()
        {
            // 设置统一的按钮样式
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            
            // 统一字体和颜色
            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            
            // 在必填字段后添加星号标记
            lblEqpGroupId.Text += " *";
            lblEqpGroupId.ForeColor = Color.FromArgb(64, 64, 64);
            lblEqpGroupType.ForeColor = Color.FromArgb(64, 64, 64);
            lblFactory.ForeColor = Color.FromArgb(64, 64, 64);
            lblDescription.ForeColor = Color.FromArgb(64, 64, 64);
            
            // 设置文本框边框和背景
            foreach (Control ctrl in mainTableLayout.Controls)
            {
                if (ctrl is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = Color.White;
                }
                else if (ctrl is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = Color.White;
                }
            }
            
            // 调整按钮面板样式
            buttonPanel.BackColor = Color.WhiteSmoke;
        }

        private void FrmEqpGroupEdit_Load(object sender, EventArgs e)
        {
            try
            {
                LoadFactories();
                
                if (!isNewGroup)
                {
                    LoadGroupData();
                }
                else
                {
                    txtEqpGroupId.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载窗体时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFactories()
        {
            try
            {
                List<Factory> factories = factoryService.GetFactories(currentUser);
                cmbFactory.DataSource = factories;
                cmbFactory.DisplayMember = "FactoryName";
                cmbFactory.ValueMember = "FactoryId";
                AdjustComboBoxDropDownWidth(cmbFactory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载工厂列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdjustComboBoxDropDownWidth(ComboBox comboBox)
        {
            int maxWidth = 0;
            using (Graphics g = comboBox.CreateGraphics())
            {
                foreach (var item in comboBox.Items)
                {
                    string displayText = comboBox.GetItemText(item);
                    int itemWidth = (int)g.MeasureString(displayText, comboBox.Font).Width;
                    maxWidth = Math.Max(maxWidth, itemWidth);
                }
            }
            comboBox.DropDownWidth = maxWidth + SystemInformation.VerticalScrollBarWidth + 5;
        }

        private void LoadGroupData()
        {
            txtEqpGroupId.Text = eqpGroup.EqpGroupId;
            txtEqpGroupId.ReadOnly = true; // ID 不允许编辑
            txtEqpGroupType.Text = eqpGroup.EqpGroupType;
            txtDescription.Text = eqpGroup.EqpGroupDescription;
            
            // 设置工厂下拉框选中项
            if (!string.IsNullOrEmpty(eqpGroup.FactoryId))
            {
                for (int i = 0; i < cmbFactory.Items.Count; i++)
                {
                    var factory = cmbFactory.Items[i] as Factory;
                    if (factory != null && factory.FactoryId == eqpGroup.FactoryId)
                    {
                        cmbFactory.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    SaveEqpGroup();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存设备组时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtEqpGroupId.Text))
            {
                MessageBox.Show("请输入设备组编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEqpGroupId.Focus();
                return false;
            }

            if (isNewGroup)
            {
                // 检查设备组ID是否已存在
                bool exists = false;
                
                // 使用GetEqpGroupById方法检查是否存在
                var existingGroup = eqpGroupService.GetEqpGroupById(txtEqpGroupId.Text);
                exists = (existingGroup != null);
                
                if (exists)
                {
                    MessageBox.Show("设备组编号已存在，请使用其他编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEqpGroupId.Focus();
                    return false;
                }
            }

            return true;
        }

        private void SaveEqpGroup()
        {
            // 将表单数据映射到对象
            eqpGroup.EqpGroupId = txtEqpGroupId.Text.Trim();
            eqpGroup.EqpGroupType = txtEqpGroupType.Text.Trim();
            eqpGroup.EqpGroupDescription = txtDescription.Text.Trim();
            
            if (cmbFactory.SelectedItem != null)
            {
                var selectedFactory = cmbFactory.SelectedItem as Factory;
                eqpGroup.FactoryId = selectedFactory.FactoryId;
            }
            
            bool success;
            if (isNewGroup)
            {
                success = eqpGroupService.AddEqpGroup(eqpGroup, currentUser);
            }
            else
            {
                success = eqpGroupService.UpdateEqpGroup(eqpGroup, currentUser);
            }
            
            if (success)
            {
                MessageBox.Show("设备组保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("设备组保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 