using System;
using System.Linq;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.DAL; // Needed for RolesDAL
using MDMUI.Model;
using MDMUI.Utility; // For PasswordEncryptor
using System.Data; // Added for DataRowView and DataTable

namespace MDMUI
{
    public partial class FrmUserEdit : Form
    {
        private User _editingUser; // Null when adding a new user
        private User _currentUser; // The user performing the action
        private UserService _userService = new UserService();
        private RolesDAL _rolesDAL = new RolesDAL(); // To load roles
        private SystemLogBLL _logBLL = new SystemLogBLL(); // For logging if needed directly

        // Constructor for Adding
        public FrmUserEdit(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            this.Text = "添加用户";
            LoadRoles();
        }

        // Constructor for Editing
        public FrmUserEdit(User userToEdit, User currentUser)
        {
            InitializeComponent();
            _editingUser = userToEdit;
            _currentUser = currentUser;
            this.Text = "编辑用户";
            LoadRoles();
            PopulateFields();
        }

        private void LoadRoles()
        {
            try
            {
                var roles = _rolesDAL.GetAllRoles();
                // Prepend a default or 'select role' item if necessary,
                // or handle the case where no role is selected.
                // roles.Insert(0, new Roles { RoleId = null, RoleName = "--请选择--" }); // Example
                cmbRole.DataSource = roles;
                cmbRole.DisplayMember = "RoleName";
                cmbRole.ValueMember = "RoleId";
                cmbRole.SelectedIndex = -1; // Default to no selection
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载角色列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Consider disabling the form or Save button
                btnSave.Enabled = false;
            }
        }

        private void PopulateFields()
        {
            if (_editingUser != null)
            {
                txtUsername.Text = _editingUser.Username;
                txtUsername.ReadOnly = true; // Typically username is not changed
                txtRealName.Text = _editingUser.RealName;
                // Don't show password when editing, provide reset functionality elsewhere.
                lblPassword.Visible = false;
                txtPassword.Visible = false;

                if (_editingUser.RoleId.HasValue)
                {
                     // Check if the role exists in the ComboBox DataSource before setting
                     var roleExists = ((cmbRole.DataSource as System.Data.DataTable) ?? new System.Data.DataTable())
                                     .AsEnumerable()
                                     .Any(r => r["RoleId"].ToString() == _editingUser.RoleId.ToString());
                    if (roleExists)
                    {
                         cmbRole.SelectedValue = _editingUser.RoleId.ToString();
                    }
                     else
                    {
                         cmbRole.SelectedIndex = -1; // Role not found or invalid
                         MessageBox.Show("用户当前关联的角色无效或不存在。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     }
                }
                else
                {
                    cmbRole.SelectedIndex = -1; // No role assigned
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // --- Input Validation ---
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("用户名不能为空。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }
             // RealName validation (optional but good practice)
            if (string.IsNullOrWhiteSpace(txtRealName.Text))
            {
                 MessageBox.Show("真实姓名不能为空。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 txtRealName.Focus();
                 return;
             }
            if (cmbRole.SelectedValue == null)
            {
                 MessageBox.Show("请为用户选择一个角色。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 cmbRole.Focus();
                 return;
             }

            // Password validation only when adding
            if (_editingUser == null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("添加用户时密码不能为空。", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // --- Prepare User Object ---
            User userToSave = _editingUser ?? new User(); // Use existing or create new
            userToSave.Username = txtUsername.Text.Trim();
            userToSave.RealName = txtRealName.Text.Trim();
            userToSave.RoleId = cmbRole.SelectedValue != null ? Convert.ToInt32(cmbRole.SelectedValue) : (int?)null;
            
            // Get RoleName from the selected DataRow
            if (cmbRole.SelectedItem is DataRowView rowView)
            {
                userToSave.RoleName = rowView["RoleName"].ToString();
            }

            // Only set password when adding
            if (_editingUser == null)
            {
                userToSave.Password = PasswordEncryptor.EncryptPassword(txtPassword.Text);
            }

            // --- Call Service ---
            try
            {
                bool success;
                if (_editingUser == null) // Adding
                {
                    // Check permission before adding (redundant if checked in FrmUser, but safer)
                    if (!new PermissionChecker().HasPermission(_currentUser.Id, "user", "add") && _currentUser.RoleName != "超级管理员")
                    {
                         MessageBox.Show("您没有添加用户的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                         return;
                     }
                    success = _userService.AddUser(userToSave, _currentUser);
                    if (success)
                    {
                        MessageBox.Show("用户添加成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Editing
                {
                     // Check permission before editing (redundant if checked in FrmUser, but safer)
                     if (!new PermissionChecker().HasPermission(_currentUser.Id, "user", "edit") && _currentUser.RoleName != "超级管理员")
                     {
                         MessageBox.Show("您没有编辑用户的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                         return;
                     }
                    success = _userService.UpdateUser(userToSave, _currentUser);
                     if (success)
                     {
                         MessageBox.Show("用户信息更新成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                }

                if (success)
                {
                    this.DialogResult = DialogResult.OK; // Signal success to caller
                    this.Close();
                }
                 // else: UserService should throw an exception on failure, handled below.
            }
            catch (Exception ex)
            {
                // Log the exception details if possible
                MessageBox.Show($"保存用户时出错: {ex.Message}", "数据库错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Keep the dialog open for correction
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 