using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;
using System.Linq;
using System.ComponentModel;
using MDMUI.DAL;

namespace MDMUI
{
    // 添加DesignerCategory特性，告诉VS这是一个Windows窗体
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class FrmUser : Form
    {
        // DataGridView Column Name Constants
        private const string ColId = "Id";
        private const string ColUsername = "Username";
        private const string ColRealName = "RealName";
        private const string ColRoleName = "RoleNameColumn";
        private const string ColLastLoginTime = "LastLoginTime";

        // 当前用户
        private User currentUser;
        
        // 添加成员变量存储所有用户数据
        private List<User> allUsers = new List<User>();

        // 权限检查器
        private PermissionChecker permissionChecker;

        private UserDAL userDAL;
        private UserService userService;
        private RolesDAL rolesDAL;
        private EmployeeDAL employeeDAL;

        // For Hover Effect
        private int hoveredRowIndex = -1;
        private Color originalRowBackColor = Color.Empty;
        private Color originalAlternatingRowBackColor = Color.Empty;
        // Define hover color for light theme
        private Color hoverBackColor = System.Drawing.SystemColors.ControlLight; 

        public FrmUser(User user)
        {
            try
            {
                // 保存当前用户
                this.currentUser = user;
                this.permissionChecker = new PermissionChecker();
                this.userDAL = new UserDAL();
                this.userService = new UserService();
                this.rolesDAL = new RolesDAL();
                this.employeeDAL = new EmployeeDAL();
                
                // 初始化组件 (这是从Designer.cs文件中引用的方法)
                InitializeComponent();

                // Store original row colors for hover effect restoration
                originalRowBackColor = this.dgvUsers.DefaultCellStyle.BackColor;
                originalAlternatingRowBackColor = this.dgvUsers.AlternatingRowsDefaultCellStyle.BackColor;
                // Adjust hover color if it's the same as one of the base colors
                if (hoverBackColor == originalRowBackColor || hoverBackColor == originalAlternatingRowBackColor)
                {
                    // Fallback to a slightly different light gray if ControlLight conflicts
                    hoverBackColor = Color.FromArgb(238, 238, 238); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化窗体出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void FrmUser_Load(object sender, EventArgs e)
        {
            try
            {
                // 根据权限设置按钮可见性
                SetButtonVisibility();
                
                // 加载所有用户数据到内存
                LoadAllUsersData();

                // 初始绑定数据到 DataGridView
                FilterAndBindUsers(string.Empty); // Use filter method for initial load
                
                ClearUserDetails(); // Call this AFTER InitializeComponent and data load attempts
                
                // 强制刷新
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("窗体加载失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearUserDetails(); // Also clear on error
            }
        }
        
        private void FrmUser_Shown(object sender, EventArgs e)
        {
            // 窗体显示后再次确保控件都已经正确绘制
            this.SuspendLayout();
            
            // 确保所有控件可见
            foreach (Control ctrl in this.Controls)
            {
                MakeAllControlsVisible(ctrl);
            }
            
            // 强制重新布局
            if (this.dgvUsers != null)
            {
                this.dgvUsers.Visible = true;
                this.dgvUsers.BringToFront();
            }
            this.ResumeLayout(true);
            this.Refresh();
        }

        // 递归使所有控件可见
        private void MakeAllControlsVisible(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                control.Visible = true;
                
                // 递归处理子控件
                if (control.Controls.Count > 0)
                {
                    MakeAllControlsVisible(control);
                }
            }
        }

        private void SetButtonVisibility()
        {
            if (currentUser == null)
            {
                return;
            }

            // 使用 RoleName 或 RoleId 进行判断 (假设 RoleName 可用)
            // 注意: currentUser 可能是在登录时获取的，需要确保它也被更新以包含 RoleName 和 RoleId
            bool isAdmin = currentUser.RoleName == "超级管理员"; // 或者 currentUser.RoleId == SUPER_ADMIN_ROLE_ID;

            if (isAdmin)
            {
                this.btnAdd.Visible = true;
                this.btnEdit.Visible = true;
                this.btnDelete.Visible = true;
                this.btnResetPwd.Visible = true;
                return; // 管理员拥有所有权限
            }

            // 根据权限检查结果设置按钮可见性
            this.btnAdd.Visible = permissionChecker.HasPermission(currentUser.Id, "user", "add");
            this.btnEdit.Visible = permissionChecker.HasPermission(currentUser.Id, "user", "edit");
            this.btnDelete.Visible = permissionChecker.HasPermission(currentUser.Id, "user", "delete");
            this.btnResetPwd.Visible = permissionChecker.HasPermission(currentUser.Id, "user", "reset_pwd");
        }

        /// <summary>
        /// 使用 UserDAL 加载所有用户数据到 allUsers 列表
        /// </summary>
        private void LoadAllUsersData()
        {
            try
            {
                allUsers = userDAL.GetAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载用户数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allUsers = new List<User>(); // 出错时确保列表为空
            }
        }

        /// <summary>
        /// Filters the loaded users based on the search term and binds the result to the DataGridView.
        /// </summary>
        /// <param name="searchTerm">The term to filter by (Username or RealName).</param>
        private void FilterAndBindUsers(string searchTerm)
        {
            try
            {
                List<User> filteredUsers;
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    filteredUsers = allUsers; // Show all if search is empty
                }
                else
                {
                    string lowerSearchTerm = searchTerm.ToLowerInvariant();
                    filteredUsers = allUsers
                        .Where(u => (u.Username != null && u.Username.ToLowerInvariant().Contains(lowerSearchTerm)) ||
                                    (u.RealName != null && u.RealName.ToLowerInvariant().Contains(lowerSearchTerm)))
                        .ToList();
                }

                BindGrid(filteredUsers);

                // Show message if search term is present but no results found
                if (filteredUsers.Count == 0 && !string.IsNullOrWhiteSpace(searchTerm))
                {
                   MessageBox.Show("未找到匹配的用户。", "搜索结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show("筛选或绑定用户数据时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 将指定的用户列表数据绑定到 DataGridView (Helper method)
        /// </summary>
        /// <param name="usersToDisplay">要显示的用户列表</param>
        private void BindGrid(List<User> usersToDisplay)
        {
             try
            {
                // Suspend layout for performance
                this.dgvUsers.SuspendLayout();

                // 填充数据到DataGridView
                this.dgvUsers.Rows.Clear(); // 先清空现有行

                if (usersToDisplay == null) return; // 防止空引用

                foreach (var user in usersToDisplay)
                {
                    int rowIndex = this.dgvUsers.Rows.Add();
                    DataGridViewRow row = this.dgvUsers.Rows[rowIndex];
                    row.Cells[ColId].Value = user.Id;
                    row.Cells[ColUsername].Value = user.Username;
                    row.Cells[ColRealName].Value = user.RealName ?? ""; // 使用空合并运算符简化
                    row.Cells[ColRoleName].Value = user.RoleName ?? "(未分配)"; // 使用 RoleNameColumn
                    row.Cells[ColLastLoginTime].Value = user.LastLoginTime.HasValue ? (object)user.LastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : DBNull.Value; // Format DateTime
                    // Store the full User object in the row's Tag for easy access later
                    row.Tag = user;
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show("绑定数据到表格失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
             finally
            {
                 // Resume layout
                 this.dgvUsers.ResumeLayout();
                 // Clear selection after binding/rebinding
                 this.dgvUsers.ClearSelection();
                 ClearUserDetails(); // Clear details when grid is rebound
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // 权限检查
            if (!permissionChecker.HasPermission(currentUser.Id, "user", "add") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有添加用户的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 使用新的 FrmUserEdit 窗体
            using (FrmUserEdit frmEdit = new FrmUserEdit(this.currentUser)) // Pass current user for logging/context
            {
                if (frmEdit.ShowDialog() == DialogResult.OK)
                {
                    // 如果添加成功，刷新数据
                    LoadAllUsersData();
                    FilterAndBindUsers(txtSearch.Text); // Re-apply filter after add
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要编辑的用户。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 权限检查
            if (!permissionChecker.HasPermission(currentUser.Id, "user", "edit") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有编辑用户的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get user object from the selected row's Tag property
            User userToEdit = this.dgvUsers.SelectedRows[0].Tag as User;

            if (userToEdit == null)
            {
                 // Fallback: Get ID and load from memory/DB (should not happen if Tag is set correctly)
                 int userId = Convert.ToInt32(this.dgvUsers.SelectedRows[0].Cells[ColId].Value);
                 userToEdit = allUsers.FirstOrDefault(u => u.Id == userId);
                 if (userToEdit == null)
                 {
                     try
                     {
                        userToEdit = userDAL.GetUserById(userId);
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show($"加载用户信息失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return;
                     }
                 }
                 if (userToEdit == null)
                 {
                      MessageBox.Show("找不到要编辑的用户信息。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      return;
                  }
            }

            // 使用新的 FrmUserEdit 窗体，传递要编辑的用户对象
            using (FrmUserEdit frmEdit = new FrmUserEdit(userToEdit, this.currentUser))
            {
                if (frmEdit.ShowDialog() == DialogResult.OK)
                {
                    // 如果编辑成功，刷新数据
                    LoadAllUsersData();
                    FilterAndBindUsers(txtSearch.Text); // Re-apply filter after edit
                    // Optionally, re-select the edited user if needed
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的用户。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 权限检查
            if (!permissionChecker.HasPermission(currentUser.Id, "user", "delete") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有删除用户的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User selectedUser = dgvUsers.SelectedRows[0].Tag as User;
            if (selectedUser == null)
            {
                 MessageBox.Show("无法获取所选用户的信息。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

            // 不能删除自己
            if (selectedUser.Id == currentUser.Id)
            {
                MessageBox.Show("不能删除当前登录的用户。", "操作无效", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Add Confirmation Dialog ---
            DialogResult confirmResult = MessageBox.Show($"确定要删除用户 '{selectedUser.Username}' 吗？\n此操作不可恢复！",
                                                        "确认删除",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Warning,
                                                        MessageBoxDefaultButton.Button2); // Default to No

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    bool success = userService.DeleteUser(selectedUser.Id, currentUser);
                    if (success)
                    {
                        MessageBox.Show("用户删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllUsersData();
                        FilterAndBindUsers(txtSearch.Text); // Re-apply filter after delete
                    }
                    // else: userService should throw exception on failure
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除用户失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtSearch.Text = string.Empty; // Clear search box on refresh
                LoadAllUsersData();
                FilterAndBindUsers(string.Empty); // Show all users
            }
            catch (Exception ex)
            {
                 MessageBox.Show("刷新失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handles the Search button click
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FilterAndBindUsers(txtSearch.Text);
        }

        // Handles the Enter key in the search box
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterAndBindUsers(txtSearch.Text);
                e.SuppressKeyPress = true; // Prevent the 'ding' sound
            }
        }

        // Handles text changing in the search box (for debounce timer)
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchDebounceTimer.Stop(); // Stop previous timer if running
            searchDebounceTimer.Start(); // Start new timer
        }

        // Handles the timer tick event after debounce interval
        private void searchDebounceTimer_Tick(object sender, EventArgs e)
        {
            searchDebounceTimer.Stop(); // Stop the timer
            FilterAndBindUsers(txtSearch.Text); // Perform the search/filter
        }

        private void BtnResetPwd_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要重置密码的用户。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 权限检查
            if (!permissionChecker.HasPermission(currentUser.Id, "user", "reset_pwd") && currentUser.RoleName != "超级管理员")
            {
                MessageBox.Show("您没有重置用户密码的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User selectedUser = dgvUsers.SelectedRows[0].Tag as User;
            if (selectedUser == null)
            {
                 MessageBox.Show("无法获取所选用户的信息。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

            // --- Add Confirmation Dialog ---
            DialogResult confirmResult = MessageBox.Show(
                $"确定要重置用户 '{selectedUser.Username}' 的密码吗？\n\n"
                + "重置后将优先使用“系统默认重置密码”；若不满足当前密码策略，将自动生成强密码并提示复制。",
                "确认重置密码",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    using (AppTelemetry.Measure("user.reset_password"))
                    {
                        string configuredDefault = TryGetConfiguredDefaultResetPassword();
                        bool usedGenerated = false;
                        string newPassword = null;

                        // 按顺序尝试：系统默认 -> 生成强密码(12/16/24/32)
                        foreach (int length in new[] { 12, 16, 24, 32 })
                        {
                            string candidate;
                            if (!usedGenerated && !string.IsNullOrWhiteSpace(configuredDefault))
                            {
                                candidate = configuredDefault;
                            }
                            else
                            {
                                usedGenerated = true;
                                candidate = PasswordGenerator.GenerateStrong(length);
                            }

                            try
                            {
                                bool success = userService.ResetPassword(selectedUser.Id, candidate, currentUser);
                                if (success)
                                {
                                    newPassword = candidate;
                                    break;
                                }
                            }
                            catch (ArgumentException argEx)
                            {
                                // 默认密码可能不满足策略：记录后继续尝试自动生成的强密码
                                AppLog.Warn($"重置密码候选值不满足策略: user={selectedUser.Username}({selectedUser.Id}), reason={argEx.Message}");
                                usedGenerated = true;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(newPassword))
                        {
                            MessageBox.Show(
                                "重置失败：无法生成满足当前密码策略的密码。\n请检查系统参数 Security.PasswordMinLength 等设置。",
                                "重置失败",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        bool clipboardOk = false;
                        if (usedGenerated)
                        {
                            try
                            {
                                Clipboard.SetText(newPassword);
                                clipboardOk = true;
                            }
                            catch
                            {
                                // ignore clipboard failures
                            }
                        }

                        if (usedGenerated)
                        {
                            MessageBox.Show(
                                $"用户 '{selectedUser.Username}' 的密码已重置。\n\n新密码：{newPassword}\n\n"
                                + (clipboardOk ? "已自动复制到剪贴板" : "复制到剪贴板失败")
                                + "，请妥善保管并通知用户及时修改。",
                                "成功",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(
                                $"用户 '{selectedUser.Username}' 的密码已重置为系统默认重置密码。",
                                "成功",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }

                        AppLog.Info($"用户密码重置成功: target={selectedUser.Username}({selectedUser.Id}), by={currentUser?.Username}({currentUser?.Id}), generated={usedGenerated}");
                    }
                }
                catch (Exception ex)
                {
                    AppLog.Error(ex, $"重置密码失败: target={selectedUser?.Username}({selectedUser?.Id})");
                    MessageBox.Show("重置密码失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string TryGetConfiguredDefaultResetPassword()
        {
            // 1) 优先：系统参数表（便于运维在线调整）
            try
            {
                string value = new SystemParameterService().GetString("Security.DefaultResetPassword", null);
                if (!string.IsNullOrWhiteSpace(value)) return value;
            }
            catch
            {
                // ignore
            }

            // 2) 备选：App.config appSettings（便于本地部署）
            try
            {
                string value = ConfigurationManager.AppSettings["Security.DefaultResetPassword"];
                if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
            }
            catch
            {
                // ignore
            }

            return null;
        }

        private void DgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Allow double-click to trigger edit action, same as clicking edit button
            if (e.RowIndex >= 0) // Ensure double-click is on a row, not header
            {
                // Select the row that was double-clicked
                this.dgvUsers.ClearSelection();
                this.dgvUsers.Rows[e.RowIndex].Selected = true;
                // Call the existing edit button logic
                BtnEdit_Click(sender, e);
            }
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                // Get user from Tag
                User selectedUser = dgvUsers.SelectedRows[0].Tag as User;
                if (selectedUser != null)
                {
                    DisplayUserDetails(selectedUser);
                }
                else
                {
                    // Fallback or error handling if Tag is null (should not happen)
                    ClearUserDetails();
                }
            }
            else
            {
                ClearUserDetails();
            }
        }

        /// <summary>
        /// Displays the details of the given user in the side panel.
        /// </summary>
        /// <param name="user">The user whose details are to be shown.</param>
        private void DisplayUserDetails(User user)
        {
            if (user == null) {
                ClearUserDetails();
                return;
            }

            // Show detail controls, hide placeholder
            SetDetailControlsVisibility(true);

            txtDetailId.Text = user.Id.ToString();
            txtDetailUsername.Text = user.Username;
            txtDetailRealName.Text = user.RealName ?? "";
            txtDetailRoleName.Text = user.RoleName ?? "(未分配)";
            txtDetailLastLogin.Text = user.LastLoginTime.HasValue ? user.LastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "(从未登录)";
        }

        /// <summary>
        /// Clears the user detail panel and shows the placeholder.
        /// </summary>
        private void ClearUserDetails()
        {
            // Hide detail controls, show placeholder
            SetDetailControlsVisibility(false);

            // Clear textboxes (optional but good practice)
            txtDetailId.Text = "";
            txtDetailUsername.Text = "";
            txtDetailRealName.Text = "";
            txtDetailRoleName.Text = "";
            txtDetailLastLogin.Text = "";
        }

        /// <summary>
        /// Sets the visibility of the user detail labels and textboxes versus the placeholder.
        /// </summary>
        /// <param name="showDetails">True to show details, False to show placeholder.</param>
        private void SetDetailControlsVisibility(bool showDetails)
        {
            // Toggle visibility of all detail controls
            lblDetailHeader.Visible = showDetails;
            lblDetailId.Visible = showDetails;
            txtDetailId.Visible = showDetails;
            lblDetailUsername.Visible = showDetails;
            txtDetailUsername.Visible = showDetails;
            lblDetailRealName.Visible = showDetails;
            txtDetailRealName.Visible = showDetails;
            lblDetailRoleName.Visible = showDetails;
            txtDetailRoleName.Visible = showDetails;
            lblDetailLastLogin.Visible = showDetails;
            txtDetailLastLogin.Visible = showDetails;

            // Toggle placeholder visibility (opposite of details)
            lblDetailPlaceholder.Visible = !showDetails;
        }

        // --- DataGridView Hover Effect --- (Add these methods)

        private void dgvUsers_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the mouse is over a valid row (not header or new row)
            if (e.RowIndex >= 0 && e.RowIndex < this.dgvUsers.RowCount)
            {
                // Restore previous hovered row if any
                RestorePreviousHoveredRowStyle();

                // Store current row index and apply hover style
                hoveredRowIndex = e.RowIndex;
                DataGridViewRow row = this.dgvUsers.Rows[hoveredRowIndex];
                // Only change back color if not selected
                if (!row.Selected)
                {
                     // Store original color before changing (needed if row has specific style)
                     // We'll simplify and rely on Default/Alternating for restoration
                    row.DefaultCellStyle.BackColor = hoverBackColor;
                }
            }
        }

        private void dgvUsers_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            // When mouse leaves the grid area or a cell, restore the last hovered row
            RestorePreviousHoveredRowStyle();
            hoveredRowIndex = -1; // Reset hovered index
        }

        // Helper to restore the previously hovered row's background color
        private void RestorePreviousHoveredRowStyle()
        {
            if (hoveredRowIndex >= 0 && hoveredRowIndex < this.dgvUsers.RowCount)
            {
                DataGridViewRow previousRow = this.dgvUsers.Rows[hoveredRowIndex];
                // Only restore if not selected (selected rows have their own style)
                if (!previousRow.Selected)
                {
                     // Determine original color based on row index (even/odd for alternating)
                     Color originalColor = (hoveredRowIndex % 2 == 0) ? originalRowBackColor : originalAlternatingRowBackColor;
                     previousRow.DefaultCellStyle.BackColor = originalColor;
                }
            }
        }

        // --- End DataGridView Hover Effect ---
    }
}
