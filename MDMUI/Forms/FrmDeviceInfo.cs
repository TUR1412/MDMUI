using MDMUI.BLL;
using MDMUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDMUI.Forms
{
    public partial class FrmDeviceInfo : Form
    {
        private readonly FactoryService factoryService;
        private readonly User CurrentUser;
        private DataTable equipmentData;
        private string currentEquipmentId = string.Empty;

        public FrmDeviceInfo(User currentUser)
        {
            InitializeComponent();
            this.CurrentUser = currentUser;
            factoryService = new FactoryService();

            ConfigureEquipmentDataGridView();
        }

        private void FrmDeviceInfo_Load(object sender, EventArgs e)
        {
            LoadFactoriesComboBox();
            LoadData();
            SetDetailsPanelReadOnly(true);
            ClearDetailsPanel();
        }

        private void ConfigureEquipmentDataGridView()
        {
            dgvEquipment.AutoGenerateColumns = false;
            dgvEquipment.Columns.Clear();

            AddColumnToDataGridView(dgvEquipment, "colEquip_EquipmentId", "EquipmentId", "设备ID", true, 100, DataGridViewContentAlignment.MiddleLeft, true);
            AddColumnToDataGridView(dgvEquipment, "colEquip_EquipmentName", "EquipmentName", "设备名称", true, 150, DataGridViewContentAlignment.MiddleLeft, true);
            AddColumnToDataGridView(dgvEquipment, "colEquip_Description", "Description", "设备描述", true, 200, DataGridViewContentAlignment.MiddleLeft, true);
            AddColumnToDataGridView(dgvEquipment, "colEquip_CreateTime", "CreateTime", "创建时间", true, 150, DataGridViewContentAlignment.MiddleLeft, true, "yyyy-MM-dd HH:mm:ss");

            DataGridViewLinkColumn historyLinkColumn = new DataGridViewLinkColumn
            {
                Name = "colHistoryLink",
                HeaderText = "历史",
                Text = "查看",
                UseColumnTextForLinkValue = true,
                TrackVisitedState = false,
                Width = 60,
                MinimumWidth = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            dgvEquipment.Columns.Add(historyLinkColumn);
            dgvEquipment.CellContentClick += dgvEquipment_CellContentClick;

            dgvEquipment.SelectionChanged += dgvEquipment_SelectionChanged;
            dgvEquipment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void AddColumnToDataGridView(DataGridView dgv, string name, string dataPropertyName, string headerText, bool visible, int width, DataGridViewContentAlignment alignment = DataGridViewContentAlignment.MiddleLeft, bool readOnly = true, string format = null)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                Name = name,
                DataPropertyName = dataPropertyName,
                HeaderText = headerText,
                Visible = visible,
                Width = width,
                MinimumWidth = width,
                ReadOnly = readOnly,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = alignment }
            };
            if (!string.IsNullOrEmpty(format))
            {
                column.DefaultCellStyle.Format = format;
            }
            dgv.Columns.Add(column);
        }

        private void LoadFactoriesComboBox()
        {
             try
             {
                List<Factory> factories = factoryService.GetFactories(this.CurrentUser);

                cmbFactory.DataSource = factories;
                cmbFactory.DisplayMember = "FactoryName";
                cmbFactory.ValueMember = "FactoryId";
                cmbFactory.SelectedIndex = -1;
             }
             catch (Exception ex)
            {
                MessageBox.Show($"加载工厂列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbFactory.DataSource = null;
                cmbFactory.Enabled = false;
            }
        }

        private void LoadData()
        {
            try
            {
                // 临时使用空DataTable代替，之后可实现EquipmentService
                if (equipmentData == null)
                {
                    equipmentData = new DataTable();
                    equipmentData.Columns.Add("EquipmentId", typeof(string));
                    equipmentData.Columns.Add("EquipmentName", typeof(string));
                    equipmentData.Columns.Add("Description", typeof(string));
                    equipmentData.Columns.Add("CreateTime", typeof(DateTime));
                    equipmentData.Columns.Add("FactoryId", typeof(string));
                    equipmentData.Columns.Add("FactoryName", typeof(string));
                    equipmentData.Columns.Add("CategoryName", typeof(string));
                    equipmentData.Columns.Add("Model", typeof(string));
                }
                
                dgvEquipment.DataSource = equipmentData;

                if (dgvEquipment.Rows.Count > 0)
                {
                    dgvEquipment.CurrentCell = dgvEquipment.Rows[0].Cells[0];
                    PopulateDetailsPanel((DataRowView)dgvEquipment.Rows[0].DataBoundItem);
                }
                else
                {
                    ClearDetailsPanel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载设备数据失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvEquipment_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEquipment.CurrentRow != null && dgvEquipment.CurrentRow.DataBoundItem != null)
            {
                DataRowView drv = (DataRowView)dgvEquipment.CurrentRow.DataBoundItem;
                PopulateDetailsPanel(drv);
                SetDetailsPanelReadOnly(true);
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
                btnEdit.Enabled = dgvEquipment.SelectedRows.Count > 0;
                btnDelete.Enabled = dgvEquipment.SelectedRows.Count > 0;
            }
            else
            {
                ClearDetailsPanel();
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

        private void dgvEquipment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvEquipment.Columns[e.ColumnIndex].Name == "colHistoryLink")
            {
                if (dgvEquipment.Rows[e.RowIndex].DataBoundItem is DataRowView drv)
                {
                    string equipmentId = drv["EquipmentId"].ToString();
                    MessageBox.Show($"历史记录功能 (设备ID: {equipmentId}) 尚未实现。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void PopulateDetailsPanel(DataRowView drv)
        {
            if (drv == null)
            {
                ClearDetailsPanel();
                return;
            }
            try
            {
                txtEquipmentId.Text = drv["EquipmentId"]?.ToString() ?? string.Empty;
                txtEquipmentName.Text = drv["EquipmentName"]?.ToString() ?? string.Empty;
                txtEquipmentDescription.Text = drv["Description"]?.ToString() ?? string.Empty;

                if (drv.Row.Table.Columns.Contains("CategoryName") && drv["CategoryName"] != DBNull.Value)
                    txtEquipmentType.Text = drv["CategoryName"].ToString();
                else
                    txtEquipmentType.Text = string.Empty;
                
                if (drv.Row.Table.Columns.Contains("Model") && drv["Model"] != DBNull.Value)
                    txtEquipmentSubType.Text = drv["Model"].ToString();
                else
                    txtEquipmentSubType.Text = string.Empty;
                
                if (cmbFactory.Items.Count > 0 && drv.Row.Table.Columns.Contains("FactoryId") && drv["FactoryId"] != DBNull.Value)
                {
                    string factoryIdValue = drv["FactoryId"].ToString();
                    cmbFactory.SelectedValue = factoryIdValue;
                    
                    if (cmbFactory.SelectedValue == null || !cmbFactory.SelectedValue.Equals(factoryIdValue)) 
                    {
                        if (drv.Row.Table.Columns.Contains("FactoryName") && drv["FactoryName"] != DBNull.Value)
                        {
                            string factoryName = drv["FactoryName"].ToString();
                            for (int i = 0; i < cmbFactory.Items.Count; i++)
                            {
                                Factory factory = cmbFactory.Items[i] as Factory;
                                if (factory != null && factory.FactoryName == factoryName)
                                {
                                    cmbFactory.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        else if (cmbFactory.Items.Count > 0)
                        {
                            cmbFactory.SelectedIndex = -1;
                        }
                    }
                }
                else if (cmbFactory.Items.Count > 0)
                {
                    cmbFactory.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"填充详细信息面板时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearDetailsPanel()
        {
            txtEquipmentId.Text = string.Empty;
            txtEquipmentName.Text = string.Empty;
            txtEquipmentDescription.Text = string.Empty;
            txtEquipmentType.Text = string.Empty;
            txtEquipmentSubType.Text = string.Empty;
            if (cmbFactory.Items.Count > 0) cmbFactory.SelectedIndex = -1;
        }

        private void SetDetailsPanelReadOnly(bool readOnly)
        {
            txtEquipmentId.ReadOnly = true;
            txtEquipmentName.ReadOnly = readOnly;
            txtEquipmentDescription.ReadOnly = readOnly;
            txtEquipmentType.ReadOnly = readOnly;
            txtEquipmentSubType.ReadOnly = readOnly;
            cmbFactory.Enabled = !readOnly;

            btnAdd.Enabled = true;
            btnEdit.Enabled = dgvEquipment.SelectedRows.Count > 0 && readOnly;
            btnDelete.Enabled = dgvEquipment.SelectedRows.Count > 0 && readOnly;
            btnSave.Enabled = !readOnly;
            btnCancel.Enabled = !readOnly;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearDetailsPanel();
            SetDetailsPanelReadOnly(false);
            currentEquipmentId = string.Empty;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            
            txtEquipmentId.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEquipment.CurrentRow != null && dgvEquipment.CurrentRow.DataBoundItem != null)
            {
                DataRowView drv = (DataRowView)dgvEquipment.CurrentRow.DataBoundItem;
                currentEquipmentId = drv["EquipmentId"].ToString();
                
                SetDetailsPanelReadOnly(false);
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                
                txtEquipmentName.Focus();
            }
            else
            {
                MessageBox.Show("请先选择一个设备进行编辑。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEquipment.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRowView drv = (DataRowView)dgvEquipment.SelectedRows[0].DataBoundItem;
            string equipmentId = drv["EquipmentId"].ToString();
            string equipmentName = drv["EquipmentName"].ToString();

            if (MessageBox.Show($"确定要删除设备 '{equipmentName}' (ID: {equipmentId}) 吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // bool success = equipmentService.DeleteEquipment(equipmentId, CurrentUser);
                    bool success = false; // Manually set to false as the call is commented out
                    if (success)
                    {
                        MessageBox.Show("设备删除成功。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("设备删除失败。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"删除设备时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 检查必填字段
            if (string.IsNullOrWhiteSpace(txtEquipmentId.Text) ||
                string.IsNullOrWhiteSpace(txtEquipmentName.Text))
            {
                MessageBox.Show("设备ID和设备名称为必填项！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Equipment equipment = new Equipment
                {
                    EquipmentId = txtEquipmentId.Text.Trim(),
                    EquipmentName = txtEquipmentName.Text.Trim(),
                    Description = txtEquipmentDescription.Text.Trim(),
                    EquipmentType = txtEquipmentType.Text.Trim(),
                    EquipmentSubType = txtEquipmentSubType.Text.Trim()
                };

                // 设置工厂ID，如果有选择的话
                if (cmbFactory.SelectedValue != null)
                {
                    equipment.FactoryId = cmbFactory.SelectedValue.ToString();
                }

                bool success = false;
                if (string.IsNullOrEmpty(currentEquipmentId))
                {
                    // 新增设备
                    // success = equipmentService.AddEquipment(equipment, CurrentUser);
                    if (success)
                    {
                        MessageBox.Show("设备添加成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // 更新设备
                    // success = equipmentService.UpdateEquipment(equipment, CurrentUser);
                    if (success)
                    {
                        MessageBox.Show("设备更新成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (success)
                {
                    LoadData();
                    SetDetailsPanelReadOnly(true);
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存设备数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 重置编辑状态
            currentEquipmentId = string.Empty;
            
            // 如果当前有选中的行，重新加载其数据
            if (dgvEquipment.CurrentRow != null && dgvEquipment.CurrentRow.DataBoundItem != null)
            {
                DataRowView drv = (DataRowView)dgvEquipment.CurrentRow.DataBoundItem;
                PopulateDetailsPanel(drv);
            }
            else
            {
                ClearDetailsPanel();
            }
            
            // 恢复UI状态
            SetDetailsPanelReadOnly(true);
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            btnEdit.Enabled = dgvEquipment.CurrentRow != null;
            btnDelete.Enabled = dgvEquipment.CurrentRow != null;
        }
    }
} 