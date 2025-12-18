using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MDMUI.BLL;
using MDMUI.Model;
using MDMUI.Utility;
using System.Linq;

namespace MDMUI
{
    // 注意：此类已重构为使用设计器生成的控件
    public partial class FrmPermission : Form
    {
        // Restore connection string needed by LoadUsers
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        
        // 当前选中的用户ID
        private int currentUserId = -1;

        // 当前登录用户
        private User currentUser;
        // 权限检查器
        private PermissionChecker permissionChecker;

        // Instantiate Services
        private PermissionService permissionService;
        private UserService userService; // For loading users (ideally)

        // Store all possible permissions loaded from DB
        private List<Permission> allAvailablePermissions = new List<Permission>();
        // Map checkbox name to PermissionId for easy lookup
        private Dictionary<string, int> checkBoxNameToPermissionIdMap = new Dictionary<string, int>();

        public FrmPermission(User user)
        {
            InitializeComponent(); // 调用设计器生成的方法
            this.currentUser = user;
            this.permissionChecker = new PermissionChecker();
            this.permissionService = new PermissionService(); // Instantiate PermissionService
            this.userService = new UserService(); // Instantiate UserService

            // EnsurePermissionTablesExist(); // Can be removed if DAL/Service assumes tables exist
            LoadUsers(); // Still uses direct SQL for now, should be refactored later
            LoadAllPermissions(); // Load all available permissions once
        }

        // FrmPermission_Load 现在由设计器绑定
        private void FrmPermission_Load(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine($"权限设置窗体加载 - 用户: {currentUser.Username}");
                // 可以在这里添加其他加载时逻辑，但避免创建主UI元素
            }
            catch (Exception ex)
            {
                Console.WriteLine($"权限设置窗体加载错误: {ex.Message}");
            }
        }

        // FrmPermission_Shown 现在由设计器绑定 (如果需要)
        // 注意：SplitContainer 的 SplitterDistance 最好在设计器中设置一个合理的默认值
        // 或者仍然在 Shown 事件中设置，但引用设计器控件 splitContainer1
        private void FrmPermission_Shown(object sender, EventArgs e)
        {
            var splitContainer = this.splitContainer1; // 直接引用设计器控件
            if (splitContainer != null)
            {
                if (splitContainer.Width >= splitContainer.Panel1MinSize + splitContainer.Panel2MinSize)
                {
                    int defaultDistance = splitContainer.Width / 3;
                    int validDistance = Math.Max(splitContainer.Panel1MinSize, Math.Min(defaultDistance, splitContainer.Width - splitContainer.Panel2MinSize));
                    try 
                    {
                         splitContainer.SplitterDistance = validDistance;
                         // splitContainer.IsSplitterFixed = false; // 设计器中可以设置
                         Console.WriteLine($"FrmPermission SplitterDistance set to {validDistance} in Shown event.");
                    } 
                    catch (Exception ex)
                    {
                         Console.WriteLine($"设置 SplitterDistance 时出错 (FrmPermission Shown): {ex.Message}");
                         // ... 后备逻辑 ...
                    }
                }
                else
                {
                     Console.WriteLine("SplitContainer 宽度不足 (FrmPermission Shown)，无法设置有效的 SplitterDistance。");
                }
            }
            else
            {
                Console.WriteLine("SplitContainer 未找到 (FrmPermission Shown)。");
            }
        }

        /// <summary>
        /// Loads all available permission definitions from the database.
        /// Also populates the mapping between CheckBox names and Permission IDs.
        /// </summary>
        private void LoadAllPermissions()
        {
            try
            {
                allAvailablePermissions = permissionService.GetAllPermissions();
                
                checkBoxNameToPermissionIdMap.Clear();
                
                // Find all checkboxes recursively within permissionPanel
                var allCheckBoxes = GetAllControlsOfType<CheckBox>(this.permissionPanel).ToList();

                foreach (var perm in allAvailablePermissions)
                {
                    string moduleName = perm.ModuleName.ToLower(); 
                    string actionName = perm.ActionName; // Keep original case

                    // Transform actionName: view -> View, reset_pwd -> ResetPwd
                    string transformedActionName = string.Join("", 
                        actionName.Split('_')
                                  .Select(part => part.Length > 0 ? char.ToUpper(part[0]) + part.Substring(1) : ""));

                    // Construct expected name: e.g., factoryViewCheckBox
                    string expectedCheckBoxName = $"{moduleName}{transformedActionName}CheckBox"; 
                    
                    // Find the checkbox by name (use IgnoreCase for robustness)
                    var foundCheckBox = allCheckBoxes.FirstOrDefault(cb => 
                        cb.Name.Equals(expectedCheckBoxName, StringComparison.OrdinalIgnoreCase));
                                          
                    if (foundCheckBox != null)
                    {                        
                        checkBoxNameToPermissionIdMap[foundCheckBox.Name] = perm.PermissionId;
                    }
                    else
                    {                        
                        // Keep warning for debugging if needed, but less critical now
                         Console.WriteLine($"[Warning] LoadAllPermissions: Could not find CheckBox named '{expectedCheckBoxName}' for permission '{perm.ModuleName}/{perm.ActionName}'");
                    }
                }
                // Console.WriteLine($"[DEBUG] LoadAllPermissions finished. Map Count: {checkBoxNameToPermissionIdMap.Count}"); // Removed debug output
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载所有权限定义失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allAvailablePermissions = new List<Permission>();
                checkBoxNameToPermissionIdMap.Clear();
            }
        }

        // Helper method to recursively get all controls of a specific type
        private IEnumerable<T> GetAllControlsOfType<T>(Control parent) where T : Control
        {
            foreach (Control control in parent.Controls)
            {
                if (control is T specificControl)
                {
                    yield return specificControl;
                }

                if (control.HasChildren)
                {
                    foreach (T childControl in GetAllControlsOfType<T>(control))
                    {
                        yield return childControl;
                    }
                }
            }
        }

        // 加载用户列表
        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Username, RealName, Role FROM Users ORDER BY Username";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    this.userListBox.Items.Clear(); // 使用设计器控件

                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string realName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        string role = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);

                        UserItem item = new UserItem
                        {
                            UserId = userId,
                            Username = username,
                            RealName = realName,
                            Role = role,
                            DisplayText = $"{username} - {realName} ({role})"
                        };
                        this.userListBox.Items.Add(item); // 使用设计器控件
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载用户列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads and displays the permissions for the specified user.
        /// </summary>
        private void LoadUserPermissions(int userId)
        {
            ResetAllCheckBoxes(this.permissionPanel); 

            if (userId <= 0 || !checkBoxNameToPermissionIdMap.Any()) 
            {
                 this.btnSave.Enabled = false; 
                return;
            }

            try
            {
                // Console.WriteLine($"[DEBUG] LoadUserPermissions - Entering Try block for UserId: {userId}"); // Removed debug
                HashSet<int> userPermissionIds = permissionService.GetUserPermissionIds(userId);
                
                foreach (var kvp in checkBoxNameToPermissionIdMap)
                {
                     var checkBox = this.permissionPanel.Controls.Find(kvp.Key, true).FirstOrDefault() as CheckBox;
                     if (checkBox != null)
                     {
                         checkBox.Checked = userPermissionIds.Contains(kvp.Value);
                     }
                }
                 // Enable all GroupBoxes within the permission panel
                 foreach (Control ctrl in this.permissionPanel.Controls)
                 {
                     if (ctrl is GroupBox gb)
                     {
                         gb.Enabled = true;
                         // Console.WriteLine($"[DEBUG] LoadUserPermissions - Enabled GroupBox: {gb.Name}"); // Removed debug
                     }
                 }
                 this.btnSave.Enabled = true; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载用户 {userId} 的权限失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.btnSave.Enabled = false; 
            }
        }

        // Resets all CheckBoxes within a given container control.
        private void ResetAllCheckBoxes(Control container)
        {
            foreach (Control c in container.Controls)
            {
                if (c is CheckBox checkbox)
                {
                    checkbox.Checked = false;
                }
                else if (c.HasChildren)
                {
                    ResetAllCheckBoxes(c); // Recursively reset in child containers (like GroupBox)
                }
            }
            this.btnSave.Enabled = false; // Disable save when resetting

            // Disable all GroupBoxes within the container as well
            foreach (Control c in container.Controls)
            {
                if (c is GroupBox gb)
                {
                    gb.Enabled = false;
                }
                // No need for recursion here as we only disable top-level group boxes in the panel
            }
        }

        /// <summary>
        /// Gathers selected permission IDs and saves them for the current user.
        /// </summary>
        private void SaveUserPermissions()
        {
            if (currentUserId <= 0)
            {
                MessageBox.Show("请先选择一个用户。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<int> selectedPermissionIds = new List<int>();

            // Iterate through the map/checkboxes to find selected ones
             foreach (var kvp in checkBoxNameToPermissionIdMap)
            {
                 var checkBox = this.permissionPanel.Controls.Find(kvp.Key, true).FirstOrDefault() as CheckBox;
                 if (checkBox != null && checkBox.Checked)
                 {
                     selectedPermissionIds.Add(kvp.Value); // Add the PermissionId
                 }
            }

            try
            {
                // Call PermissionService to save permissions and log the action
                bool success = permissionService.SaveUserPermissions(currentUserId, selectedPermissionIds, this.currentUser);

                if (success)
                {
                    string targetUsername = (userListBox.SelectedItem as UserItem)?.Username ?? $"ID:{currentUserId}";
                    MessageBox.Show($"用户 [{targetUsername}] 的权限已成功保存。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Optionally reload permissions for the user to reflect changes, though usually not needed after save.
                    // LoadUserPermissions(currentUserId);
                }
                 // No else needed, service throws exception on failure
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存用户权限时出错: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 用户列表选择变更事件
        private void UserListBox_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if (userListBox.SelectedItem is UserItem selectedUserItem)
            {
                currentUserId = selectedUserItem.UserId;
                LoadUserPermissions(currentUserId);
            }
            else
            {
                currentUserId = -1;
                ResetAllCheckBoxes(this.permissionPanel);
            }
        }

        // 保存按钮点击事件
        private void BtnSave_Click(object sender, EventArgs e)
        {
             // Add permission check if saving requires specific permission
             bool canEditPermissions = permissionChecker.HasPermission(currentUser.Id, "permission", "edit") || currentUser.RoleName == "超级管理员"; // Example permission check
             if (!canEditPermissions)
            {
                 MessageBox.Show("您没有编辑权限的权限。", "权限不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }
            SaveUserPermissions();
        }
    }

    // 用户项
    public class UserItem
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string Role { get; set; }
        public string DisplayText { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}