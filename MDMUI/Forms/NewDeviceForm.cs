using MDMUI.BLL;
using MDMUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MDMUI.Forms
{
    public partial class NewDeviceForm : Form
    {
        private readonly EquipmentService equipmentService;
        private readonly EqpGroupService eqpGroupService;
        private readonly SubDeviceService subDeviceService;
        private readonly PortService portService;
        private readonly User currentUser;

        public NewDeviceForm(User user)
        {
            InitializeComponent();
            this.currentUser = user;
            
            // 初始化服务
            equipmentService = new EquipmentService();
            eqpGroupService = new EqpGroupService();
            subDeviceService = new SubDeviceService();
            portService = new PortService();
        }

        private void NewDeviceForm_Load(object sender, EventArgs e)
        {
            // 加载筛选条件
            LoadFilterComboBoxes();
            
            // 配置数据网格
            ConfigureDataGridViews();
            
            // 加载初始数据
            LoadDeviceData();
        }

        private void LoadFilterComboBoxes()
        {
            try
            {
                // 加载设备类型下拉框
                cmbDeviceType.Items.Clear();
                cmbDeviceType.Items.Add(new ComboboxItem("全部", ""));
                var categoryItems = equipmentService.GetEquipmentCategoriesForComboBox();
                if (categoryItems != null)
                {
                    foreach (var item in categoryItems)
                    {
                        cmbDeviceType.Items.Add(item);
                    }
                }
                cmbDeviceType.DisplayMember = "Text";
                cmbDeviceType.ValueMember = "Value";
                cmbDeviceType.SelectedIndex = 0;

                // 加载设备组下拉框
                cmbDeviceGroup.Items.Clear();
                cmbDeviceGroup.Items.Add(new ComboboxItem("全部", ""));
                var groupItems = eqpGroupService.GetAllEqpGroupsForFilter();
                if (groupItems != null)
                {
                    foreach (var group in groupItems)
                    {
                        cmbDeviceGroup.Items.Add(new ComboboxItem(group.EqpGroupDescription, group.EqpGroupId));
                    }
                }
                cmbDeviceGroup.DisplayMember = "Text";
                cmbDeviceGroup.ValueMember = "Value";
                cmbDeviceGroup.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载筛选数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridViews()
        {
            // 配置设备网格
            dgvDevices.AutoGenerateColumns = false;
            dgvDevices.Columns.Clear();
            dgvDevices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EquipmentId",
                DataPropertyName = "EquipmentId",
                HeaderText = "设备ID",
                Width = 100
            });
            dgvDevices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EquipmentName",
                DataPropertyName = "EquipmentName",
                HeaderText = "设备名称",
                Width = 150
            });
            dgvDevices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                DataPropertyName = "Description",
                HeaderText = "设备描述",
                Width = 200
            });
            
            // 配置子设备网格
            dgvSubDevices.AutoGenerateColumns = false;
            dgvSubDevices.Columns.Clear();
            dgvSubDevices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubDeviceId",
                DataPropertyName = "SubDeviceId",
                HeaderText = "子设备ID",
                Width = 100
            });
            dgvSubDevices.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SubDeviceName",
                DataPropertyName = "SubDeviceName",
                HeaderText = "子设备名称",
                Width = 150
            });
            
            // 配置端口网格
            dgvPorts.AutoGenerateColumns = false;
            dgvPorts.Columns.Clear();
            dgvPorts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PortId",
                DataPropertyName = "PortId",
                HeaderText = "端口ID",
                Width = 80
            });
            dgvPorts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PortName",
                DataPropertyName = "PortName",
                HeaderText = "端口名称",
                Width = 150
            });
        }

        private void LoadDeviceData()
        {
            try
            {
                string deviceType = cmbDeviceType.SelectedIndex > 0 ? ((ComboboxItem)cmbDeviceType.SelectedItem).Value?.ToString() : "";
                string deviceGroup = cmbDeviceGroup.SelectedIndex > 0 ? ((ComboboxItem)cmbDeviceGroup.SelectedItem).Value?.ToString() : "";
                
                var data = equipmentService.GetAllEquipment(deviceType, deviceGroup);
                dgvDevices.DataSource = data;
                
                // 清空子设备和端口
                dgvSubDevices.DataSource = null;
                dgvPorts.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载设备数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDeviceData();
        }

        private void dgvDevices_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count > 0)
            {
                var row = dgvDevices.SelectedRows[0].DataBoundItem as DataRowView;
                if (row != null)
                {
                    string eqpGroupId = row["eqp_group_id"]?.ToString() ?? "";
                    LoadSubDevices(eqpGroupId);
                }
            }
        }

        private void LoadSubDevices(string eqpGroupId)
        {
            try
            {
                if (!string.IsNullOrEmpty(eqpGroupId))
                {
                    var data = subDeviceService.GetSubDevicesByGroupId(eqpGroupId);
                    dgvSubDevices.DataSource = data;
                }
                else
                {
                    dgvSubDevices.DataSource = null;
                }
                
                // 清空端口
                dgvPorts.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载子设备数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSubDevices_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSubDevices.SelectedRows.Count > 0)
            {
                var row = dgvSubDevices.SelectedRows[0].DataBoundItem as DataRowView;
                if (row != null)
                {
                    string subDeviceId = row["SubDeviceId"]?.ToString() ?? "";
                    LoadPorts(subDeviceId);
                }
            }
        }

        private void LoadPorts(string subDeviceId)
        {
            try
            {
                if (!string.IsNullOrEmpty(subDeviceId))
                {
                    var data = portService.GetPortsByParentDeviceId(subDeviceId);
                    dgvPorts.DataSource = data;
                }
                else
                {
                    dgvPorts.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载端口数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 