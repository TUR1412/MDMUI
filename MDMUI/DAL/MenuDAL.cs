using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.DAL
{
    public class MenuDAL
    {
        // 数据库连接字符串
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        // 获取所有菜单
        public List<Menu> GetAllMenus()
        {
            List<Menu> menuList = new List<Menu>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    EnsureMenuTablesExist(connection);

                    string query = @"
                        SELECT MenuId, MenuName, ParentMenuId, MenuOrder, MenuIcon
                        FROM Menu
                        ORDER BY ParentMenuId, MenuOrder";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Menu menu = new Menu
                        {
                            MenuId = Convert.ToInt32(reader["MenuId"]),
                            MenuName = reader["MenuName"].ToString(),
                            ParentMenuId = reader.IsDBNull(reader.GetOrdinal("ParentMenuId")) ?
                                          (int?)null : Convert.ToInt32(reader["ParentMenuId"]),
                            MenuOrder = Convert.ToInt32(reader["MenuOrder"]),
                            MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ?
                                      null : reader["MenuIcon"].ToString()
                        };

                        menuList.Add(menu);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取菜单列表失败: " + ex.Message);
            }

            return menuList;
        }

        // 获取指定用户有权限访问的菜单
        public List<Menu> GetUserMenus(int userId, string factoryId = null)
        {
            List<Menu> menuList = new List<Menu>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    EnsureMenuTablesExist(connection);

                    // 检查用户是否是管理员
                    bool isAdmin = IsAdminUser(userId, connection);
                    Console.WriteLine($"用户ID: {userId}, 是否管理员: {isAdmin}, 工厂ID: {factoryId}");

                    if (isAdmin)
                    {
                        if (string.IsNullOrEmpty(factoryId))
                        {
                            // 管理员且未指定工厂时，返回所有菜单
                            Console.WriteLine("管理员用户 - 返回所有菜单");
                            return GetAllMenus();
                        }
                        else
                        {
                            // 管理员指定了工厂，返回该工厂有权限的所有菜单（合并管理员权限和工厂权限）
                            Console.WriteLine($"管理员用户指定工厂 - 返回工厂 {factoryId} 菜单");
                            List<Menu> factoryMenus = GetMenusByFactory(factoryId, connection);
                            List<Menu> allMenus = GetAllMenus();
                            
                            // 合并菜单，移除重复项
                            HashSet<int> existingMenuIds = new HashSet<int>(factoryMenus.Select(m => m.MenuId));
                            foreach (Menu menu in allMenus)
                            {
                                if (!existingMenuIds.Contains(menu.MenuId))
                                {
                                    factoryMenus.Add(menu);
                                }
                            }
                            
                            return factoryMenus;
                        }
                    }

                    // 非管理员用户，基于用户权限和工厂权限过滤菜单
                    string query;
                    
                    if (!string.IsNullOrEmpty(factoryId))
                    {
                        // 有工厂ID，同时考虑用户权限和工厂权限
                        Console.WriteLine($"非管理用户 - 根据用户权限和工厂 {factoryId} 权限过滤菜单");
                        query = @"
                            SELECT DISTINCT m.MenuId, m.MenuName, m.ParentMenuId, m.MenuOrder, m.MenuIcon
                            FROM Menu m
                            LEFT JOIN MenuPermission mp ON m.MenuId = mp.MenuId
                            LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
                            LEFT JOIN FactoryMenuPermissions fmp ON m.MenuId = fmp.MenuId
                            WHERE (up.UserId = @UserId AND fmp.FactoryId = @FactoryId)
                            ORDER BY m.ParentMenuId, m.MenuOrder";
                    }
                    else
                    {
                        // 没有工厂ID，只考虑用户权限
                        Console.WriteLine("非管理用户 - 仅根据用户权限过滤菜单");
                        query = @"
                            SELECT DISTINCT m.MenuId, m.MenuName, m.ParentMenuId, m.MenuOrder, m.MenuIcon
                            FROM Menu m
                            LEFT JOIN MenuPermission mp ON m.MenuId = mp.MenuId
                            LEFT JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
                            WHERE up.UserId = @UserId
                            ORDER BY m.ParentMenuId, m.MenuOrder";
                    }

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", userId);
                    
                    if (!string.IsNullOrEmpty(factoryId))
                    {
                        command.Parameters.AddWithValue("@FactoryId", factoryId);
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Menu menu = new Menu
                        {
                            MenuId = Convert.ToInt32(reader["MenuId"]),
                            MenuName = reader["MenuName"].ToString(),
                            ParentMenuId = reader.IsDBNull(reader.GetOrdinal("ParentMenuId")) ?
                                          (int?)null : Convert.ToInt32(reader["ParentMenuId"]),
                            MenuOrder = Convert.ToInt32(reader["MenuOrder"]),
                            MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ?
                                      null : reader["MenuIcon"].ToString()
                        };

                        menuList.Add(menu);
                        Console.WriteLine($"添加菜单: {menu.MenuName} (ID: {menu.MenuId}, 父ID: {menu.ParentMenuId})");
                    }
                    
                    reader.Close();
                    
                    // 添加父菜单
                    AddParentMenus(menuList, connection);
                    
                    // 添加系统菜单（所有用户都应该有）
                    AddSystemMenus(menuList, connection);
                    
                    Console.WriteLine($"为用户 {userId} 返回了 {menuList.Count} 个菜单项");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户菜单失败: {ex.Message}");
                throw new Exception("获取用户菜单失败: " + ex.Message);
            }

            return menuList;
        }

        // 根据工厂ID获取菜单
        private List<Menu> GetMenusByFactory(string factoryId, SqlConnection connection)
        {
            List<Menu> menuList = new List<Menu>();

            try
            {
                Console.WriteLine($"获取工厂 {factoryId} 的菜单");
                
                // 首先确保FactoryMenuPermissions表存在
                EnsureFactoryMenuPermissionsTableExists(connection);
                
                string query = @"
                    SELECT DISTINCT m.MenuId, m.MenuName, m.ParentMenuId, m.MenuOrder, m.MenuIcon
                    FROM Menu m
                    JOIN FactoryMenuPermissions fmp ON m.MenuId = fmp.MenuId
                    WHERE fmp.FactoryId = @FactoryId
                    ORDER BY m.ParentMenuId, m.MenuOrder";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FactoryId", factoryId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Menu menu = new Menu
                    {
                        MenuId = Convert.ToInt32(reader["MenuId"]),
                        MenuName = reader["MenuName"].ToString(),
                        ParentMenuId = reader.IsDBNull(reader.GetOrdinal("ParentMenuId")) ?
                                      (int?)null : Convert.ToInt32(reader["ParentMenuId"]),
                        MenuOrder = Convert.ToInt32(reader["MenuOrder"]),
                        MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ?
                                  null : reader["MenuIcon"].ToString()
                    };

                    menuList.Add(menu);
                    Console.WriteLine($"添加工厂菜单: {menu.MenuName} (ID: {menu.MenuId})");
                }
                
                reader.Close();
                
                // 确保包含父菜单
                AddParentMenus(menuList, connection);
                
                // 添加系统菜单
                AddSystemMenus(menuList, connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"根据工厂获取菜单失败: {ex.Message}");
                throw new Exception("根据工厂获取菜单失败: " + ex.Message);
            }

            return menuList;
        }

        // 确保工厂菜单权限表存在
        private void EnsureFactoryMenuPermissionsTableExists(SqlConnection connection)
        {
            try
            {
                string checkFactoryMenuPermissionsTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FactoryMenuPermissions')
                    BEGIN
                        CREATE TABLE FactoryMenuPermissions (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            FactoryId VARCHAR(10) NOT NULL,
                            MenuId INT NOT NULL,
                            CONSTRAINT FK_FactoryMenuPermissions_Factory FOREIGN KEY (FactoryId) REFERENCES Factory(FactoryId),
                            CONSTRAINT FK_FactoryMenuPermissions_Menu FOREIGN KEY (MenuId) REFERENCES Menu(MenuId),
                            CONSTRAINT UK_FactoryMenuPermissions UNIQUE (FactoryId, MenuId)
                        );
                        
                        -- 获取菜单ID
                        DECLARE @factoryMenuId INT;
                        DECLARE @areaMenuId INT;
                        DECLARE @userMenuId INT;
                        
                        SELECT @factoryMenuId = MenuId FROM Menu WHERE MenuName = N'工厂管理';
                        SELECT @areaMenuId = MenuId FROM Menu WHERE MenuName = N'生产地信息';
                        SELECT @userMenuId = MenuId FROM Menu WHERE MenuName = N'用户管理';
                        
                        -- 为第一机械厂添加菜单权限
                        INSERT INTO FactoryMenuPermissions (FactoryId, MenuId)
                        VALUES ('F001', @factoryMenuId), ('F001', @areaMenuId), ('F001', @userMenuId);
                        
                        -- 为第二电子厂添加菜单权限（不包含用户管理权限）
                        INSERT INTO FactoryMenuPermissions (FactoryId, MenuId)
                        VALUES ('F002', @factoryMenuId), ('F002', @areaMenuId);
                    END";

                SqlCommand factoryMenuPermissionsCommand = new SqlCommand(checkFactoryMenuPermissionsTableQuery, connection);
                factoryMenuPermissionsCommand.ExecuteNonQuery();
                Console.WriteLine("已检查并确保FactoryMenuPermissions表存在");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"确保FactoryMenuPermissions表存在失败: {ex.Message}");
                // 捕获但不抛出异常
            }
        }

        // 确保包含所有子菜单对应的父菜单
        private void AddParentMenus(List<Menu> menuList, SqlConnection connection)
        {
            // 获取所有需要的父菜单ID
            HashSet<int> parentIds = new HashSet<int>();
            HashSet<int> existingIds = new HashSet<int>(menuList.Select(m => m.MenuId));

            foreach (Menu menu in menuList)
            {
                if (menu.ParentMenuId.HasValue && !existingIds.Contains(menu.ParentMenuId.Value))
                {
                    parentIds.Add(menu.ParentMenuId.Value);
                }
            }

            // 如果有需要添加的父菜单
            if (parentIds.Count > 0)
            {
                string idList = string.Join(",", parentIds);
                string query = $@"
                    SELECT MenuId, MenuName, ParentMenuId, MenuOrder, MenuIcon
                    FROM Menu 
                    WHERE MenuId IN ({idList})
                    ORDER BY ParentMenuId, MenuOrder";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Menu menu = new Menu
                    {
                        MenuId = Convert.ToInt32(reader["MenuId"]),
                        MenuName = reader["MenuName"].ToString(),
                        ParentMenuId = reader.IsDBNull(reader.GetOrdinal("ParentMenuId")) ?
                                      (int?)null : Convert.ToInt32(reader["ParentMenuId"]),
                        MenuOrder = Convert.ToInt32(reader["MenuOrder"]),
                        MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ?
                                  null : reader["MenuIcon"].ToString()
                    };

                    menuList.Add(menu);
                    existingIds.Add(menu.MenuId);

                    // 如果这个菜单也有父菜单，添加到需要检查的列表
                    if (menu.ParentMenuId.HasValue && !existingIds.Contains(menu.ParentMenuId.Value))
                    {
                        parentIds.Add(menu.ParentMenuId.Value);
                    }
                }
                
                reader.Close();
                
                // 递归检查是否还有父菜单需要添加
                if (parentIds.Any(id => !existingIds.Contains(id)))
                {
                    AddParentMenus(menuList, connection);
                }
            }
        }

        // 判断用户是否是管理员
        private bool IsAdminUser(int userId, SqlConnection connection = null)
        {
            bool shouldCloseConnection = false;
            try
            {
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    shouldCloseConnection = true;
                }

                string query = "SELECT Role FROM Users WHERE Id = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    string role = result.ToString();
                    return role == "超级管理员" || role == "管理员";
                }
            }
            catch
            {
                // 忽略异常，默认返回false
            }
            finally
            {
                if (shouldCloseConnection && connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return false;
        }

        // 确保菜单相关表存在
        private void EnsureMenuTablesExist(SqlConnection connection)
        {
            // 首先确保权限表存在
            EnsurePermissionsTableExists(connection);

            // 检查Menu表是否存在
            string checkMenuTableQuery = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Menu')
                BEGIN
                    CREATE TABLE Menu (
                        MenuId INT PRIMARY KEY IDENTITY(1,1),
                        MenuName NVARCHAR(50) NOT NULL,
                        ParentMenuId INT,
                        MenuOrder INT DEFAULT 0,
                        MenuIcon NVARCHAR(50),
                        CONSTRAINT FK_Menu_Parent FOREIGN KEY (ParentMenuId) REFERENCES Menu(MenuId)
                    );
                    
                    -- 插入顶级菜单
                    INSERT INTO Menu (MenuName, ParentMenuId, MenuOrder, MenuIcon)
                    VALUES (N'工厂', NULL, 1, 'factory');
                    
                    INSERT INTO Menu (MenuName, ParentMenuId, MenuOrder, MenuIcon)
                    VALUES (N'管理员', NULL, 2, 'admin');

                    INSERT INTO Menu (MenuName, ParentMenuId, MenuOrder, MenuIcon)
                    VALUES (N'系统', NULL, 3, 'system');
                    
                    -- 定义变量存储父菜单ID
                    DECLARE @factoryParentId INT;
                    DECLARE @adminParentId INT;
                    DECLARE @systemParentId INT;
                    
                    SELECT @factoryParentId = MenuId FROM Menu WHERE MenuName = N'工厂';
                    SELECT @adminParentId = MenuId FROM Menu WHERE MenuName = N'管理员';
                    SELECT @systemParentId = MenuId FROM Menu WHERE MenuName = N'系统';
                    
                    -- 插入子菜单
                    INSERT INTO Menu (MenuName, ParentMenuId, MenuOrder)
                    VALUES 
                        (N'工厂管理', @factoryParentId, 1),
                        (N'生产地信息', @factoryParentId, 2),
                        (N'用户管理', @adminParentId, 1),
                        (N'权限设置', @adminParentId, 2),
                        (N'修改密码', @systemParentId, 1),
                        (N'注销', @systemParentId, 2),
                        (N'退出', @systemParentId, 3);
                END";

            SqlCommand checkMenuCommand = new SqlCommand(checkMenuTableQuery, connection);
            checkMenuCommand.ExecuteNonQuery();

            // 检查MenuPermission表是否存在
            string checkMenuPermissionTableQuery = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'MenuPermission')
                BEGIN
                    CREATE TABLE MenuPermission (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        MenuId INT NOT NULL,
                        PermissionId INT NOT NULL,
                        CONSTRAINT FK_MenuPermission_Menu FOREIGN KEY (MenuId) REFERENCES Menu(MenuId),
                        CONSTRAINT FK_MenuPermission_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(PermissionId),
                        CONSTRAINT UK_MenuPermission UNIQUE (MenuId, PermissionId)
                    );
                    
                    -- 插入菜单-权限关联
                    -- 获取菜单ID
                    DECLARE @factoryMenuId INT;
                    DECLARE @areaMenuId INT;
                    DECLARE @userMenuId INT;
                    DECLARE @permissionMenuId INT;
                    
                    SELECT @factoryMenuId = MenuId FROM Menu WHERE MenuName = N'工厂管理';
                    SELECT @areaMenuId = MenuId FROM Menu WHERE MenuName = N'生产地信息';
                    SELECT @userMenuId = MenuId FROM Menu WHERE MenuName = N'用户管理';
                    SELECT @permissionMenuId = MenuId FROM Menu WHERE MenuName = N'权限设置';
                    
                    -- 工厂管理菜单 - 工厂查看权限
                    INSERT INTO MenuPermission (MenuId, PermissionId)
                    SELECT @factoryMenuId, PermissionId FROM Permissions 
                    WHERE ModuleName = 'factory' AND ActionName = 'view';
                    
                    -- 生产地信息菜单 - 生产地查看权限
                    INSERT INTO MenuPermission (MenuId, PermissionId)
                    SELECT @areaMenuId, PermissionId FROM Permissions 
                    WHERE ModuleName = 'area' AND ActionName = 'view';
                    
                    -- 用户管理菜单 - 用户查看权限
                    INSERT INTO MenuPermission (MenuId, PermissionId)
                    SELECT @userMenuId, PermissionId FROM Permissions 
                    WHERE ModuleName = 'user' AND ActionName = 'view';
                    
                    -- 权限设置菜单 - 系统查看权限
                    INSERT INTO MenuPermission (MenuId, PermissionId)
                    SELECT @permissionMenuId, PermissionId FROM Permissions 
                    WHERE ModuleName = 'system' AND ActionName = 'view';
                END";

            SqlCommand checkMenuPermissionCommand = new SqlCommand(checkMenuPermissionTableQuery, connection);
            checkMenuPermissionCommand.ExecuteNonQuery();
        }

        // 确保权限表存在
        private void EnsurePermissionsTableExists(SqlConnection connection)
        {
            string checkTableQuery = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Permissions')
                BEGIN
                    CREATE TABLE Permissions (
                        PermissionId INT PRIMARY KEY IDENTITY(1,1),
                        ModuleName NVARCHAR(50) NOT NULL,
                        ActionName NVARCHAR(50) NOT NULL,
                        Description NVARCHAR(200),
                        CONSTRAINT UK_Permissions UNIQUE (ModuleName, ActionName)
                    );
                    
                    -- 插入默认权限
                    -- 工厂管理
                    INSERT INTO Permissions (ModuleName, ActionName, Description)
                    VALUES 
                        ('factory', 'view', N'查看工厂信息'),
                        ('factory', 'add', N'添加工厂'),
                        ('factory', 'edit', N'编辑工厂'),
                        ('factory', 'delete', N'删除工厂'),
                        ('factory', 'print', N'打印工厂信息'),
                        ('factory', 'export', N'导出工厂数据');
                        
                    -- 用户管理
                    INSERT INTO Permissions (ModuleName, ActionName, Description)
                    VALUES 
                        ('user', 'view', N'查看用户信息'),
                        ('user', 'add', N'添加用户'),
                        ('user', 'edit', N'编辑用户'),
                        ('user', 'delete', N'删除用户'),
                        ('user', 'reset_pwd', N'重置用户密码');
                        
                    -- 生产地信息
                    INSERT INTO Permissions (ModuleName, ActionName, Description)
                    VALUES 
                        ('area', 'view', N'查看生产地信息'),
                        ('area', 'add', N'添加生产地'),
                        ('area', 'edit', N'编辑生产地'),
                        ('area', 'delete', N'删除生产地'),
                        ('area', 'print', N'打印生产地信息');
                        
                    -- 系统设置
                    INSERT INTO Permissions (ModuleName, ActionName, Description)
                    VALUES 
                        ('system', 'view', N'查看系统设置'),
                        ('system', 'backup', N'备份系统'),
                        ('system', 'restore', N'恢复系统'),
                        ('system', 'log', N'查看系统日志');
                END";

            SqlCommand command = new SqlCommand(checkTableQuery, connection);
            command.ExecuteNonQuery();
        }

        // 添加菜单
        public int AddMenu(Menu menu)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Menu (MenuName, ParentMenuId, MenuOrder, MenuIcon)
                        VALUES (@MenuName, @ParentMenuId, @MenuOrder, @MenuIcon);
                        SELECT SCOPE_IDENTITY();";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuName", menu.MenuName);
                    command.Parameters.AddWithValue("@ParentMenuId", menu.ParentMenuId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MenuOrder", menu.MenuOrder);
                    command.Parameters.AddWithValue("@MenuIcon", string.IsNullOrEmpty(menu.MenuIcon) ? DBNull.Value : (object)menu.MenuIcon);

                    decimal id = (decimal)command.ExecuteScalar();
                    return Convert.ToInt32(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("添加菜单失败: " + ex.Message);
            }
        }

        // 修改菜单
        public bool UpdateMenu(Menu menu)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Menu
                        SET MenuName = @MenuName,
                            ParentMenuId = @ParentMenuId,
                            MenuOrder = @MenuOrder,
                            MenuIcon = @MenuIcon
                        WHERE MenuId = @MenuId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuId", menu.MenuId);
                    command.Parameters.AddWithValue("@MenuName", menu.MenuName);
                    command.Parameters.AddWithValue("@ParentMenuId", menu.ParentMenuId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MenuOrder", menu.MenuOrder);
                    command.Parameters.AddWithValue("@MenuIcon", string.IsNullOrEmpty(menu.MenuIcon) ? DBNull.Value : (object)menu.MenuIcon);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("修改菜单失败: " + ex.Message);
            }
        }

        // 删除菜单
        public bool DeleteMenu(int menuId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // 先检查是否有子菜单
                    string checkQuery = "SELECT COUNT(*) FROM Menu WHERE ParentMenuId = @MenuId";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@MenuId", menuId);

                    int childCount = (int)checkCommand.ExecuteScalar();
                    if (childCount > 0)
                    {
                        throw new Exception("该菜单下存在子菜单，无法删除");
                    }

                    // 删除菜单与权限的关联
                    string deletePermQuery = "DELETE FROM MenuPermission WHERE MenuId = @MenuId";
                    SqlCommand deletePermCommand = new SqlCommand(deletePermQuery, connection);
                    deletePermCommand.Parameters.AddWithValue("@MenuId", menuId);
                    deletePermCommand.ExecuteNonQuery();

                    // 删除菜单
                    string deleteQuery = "DELETE FROM Menu WHERE MenuId = @MenuId";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@MenuId", menuId);

                    int result = deleteCommand.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("删除菜单失败: " + ex.Message);
            }
        }

        // 获取指定菜单
        public Menu GetMenuById(int menuId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT MenuId, MenuName, ParentMenuId, MenuOrder, MenuIcon
                        FROM Menu
                        WHERE MenuId = @MenuId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MenuId", menuId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Menu menu = new Menu
                        {
                            MenuId = Convert.ToInt32(reader["MenuId"]),
                            MenuName = reader["MenuName"].ToString(),
                            ParentMenuId = reader.IsDBNull(reader.GetOrdinal("ParentMenuId")) ?
                                          (int?)null : Convert.ToInt32(reader["ParentMenuId"]),
                            MenuOrder = Convert.ToInt32(reader["MenuOrder"]),
                            MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ?
                                      null : reader["MenuIcon"].ToString()
                        };

                        return menu;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取菜单失败: " + ex.Message);
            }

            return null;
        }

        // 添加系统菜单（修改密码、登出等基础功能）
        private void AddSystemMenus(List<Menu> menuList, SqlConnection connection)
        {
            try
            {
                // 检查系统菜单是否已存在于列表中
                bool hasSystemMenu = menuList.Any(m => m.MenuName == "系统" && m.ParentMenuId == null);
                
                if (!hasSystemMenu)
                {
                    // 获取系统菜单
                    string query = @"
                        SELECT m.MenuId, m.MenuName, m.ParentMenuId, m.MenuOrder, m.MenuIcon
                        FROM Menu m
                        WHERE m.MenuName = N'系统' AND m.ParentMenuId IS NULL";
                        
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Menu menu = new Menu
                        {
                            MenuId = Convert.ToInt32(reader["MenuId"]),
                            MenuName = reader["MenuName"].ToString(),
                            ParentMenuId = reader.IsDBNull(reader.GetOrdinal("ParentMenuId")) ?
                                          (int?)null : Convert.ToInt32(reader["ParentMenuId"]),
                            MenuOrder = Convert.ToInt32(reader["MenuOrder"]),
                            MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ?
                                      null : reader["MenuIcon"].ToString()
                        };
                        
                        menuList.Add(menu);
                        Console.WriteLine($"添加系统菜单: {menu.MenuName} (ID: {menu.MenuId})");
                    }
                    
                    reader.Close();
                    
                    // 获取系统菜单的子菜单
                    int? systemMenuId = menuList.FirstOrDefault(m => m.MenuName == "系统" && m.ParentMenuId == null)?.MenuId;
                    
                    if (systemMenuId.HasValue)
                    {
                        query = @"
                            SELECT m.MenuId, m.MenuName, m.ParentMenuId, m.MenuOrder, m.MenuIcon
                            FROM Menu m
                            WHERE m.ParentMenuId = @ParentMenuId";
                            
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ParentMenuId", systemMenuId.Value);
                        reader = command.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            Menu menu = new Menu
                            {
                                MenuId = Convert.ToInt32(reader["MenuId"]),
                                MenuName = reader["MenuName"].ToString(),
                                ParentMenuId = reader.IsDBNull(reader.GetOrdinal("ParentMenuId")) ?
                                              (int?)null : Convert.ToInt32(reader["ParentMenuId"]),
                                MenuOrder = Convert.ToInt32(reader["MenuOrder"]),
                                MenuIcon = reader.IsDBNull(reader.GetOrdinal("MenuIcon")) ?
                                          null : reader["MenuIcon"].ToString()
                            };
                            
                            menuList.Add(menu);
                            Console.WriteLine($"添加系统子菜单: {menu.MenuName} (ID: {menu.MenuId})");
                        }
                        
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加系统菜单失败: {ex.Message}");
                // 忽略异常继续处理
            }
        }

        /// <summary>
        /// 获取用户可访问的模块列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户可访问的模块数据表</returns>
        public DataTable GetUserAccessibleMenus(string userId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MenuId", typeof(string));
            dt.Columns.Add("MenuName", typeof(string));
            dt.Columns.Add("ParentMenuId", typeof(string));
            dt.Columns.Add("MenuOrder", typeof(int));
            dt.Columns.Add("MenuIcon", typeof(string));
            dt.Columns.Add("HasAccess", typeof(bool));

            try
            {
                // 尝试转换userId为int，因为现有方法使用int类型userId
                if (int.TryParse(userId, out int userIdInt))
                {
                    List<Menu> userMenus = GetUserMenus(userIdInt);
                    
                    // 将Menu对象转换为DataTable行
                    foreach (Menu menu in userMenus)
                    {
                        DataRow row = dt.NewRow();
                        row["MenuId"] = menu.MenuId.ToString();
                        row["MenuName"] = menu.MenuName;
                        row["ParentMenuId"] = menu.ParentMenuId.HasValue ? (object)menu.ParentMenuId.ToString() : DBNull.Value;
                        row["MenuOrder"] = menu.MenuOrder;
                        row["MenuIcon"] = menu.MenuIcon != null ? (object)menu.MenuIcon : DBNull.Value;
                        row["HasAccess"] = true;
                        dt.Rows.Add(row);
                    }
                }
                else
                {
                    Console.WriteLine($"无法将用户ID '{userId}' 转换为整数。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户可访问模块失败: {ex.Message}");
                throw new Exception($"获取用户可访问模块失败: {ex.Message}", ex);
            }

            return dt;
        }

        /// <summary>
        /// 检查用户对指定模块的访问权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleId">模块ID</param>
        /// <returns>如果用户有访问权限，则返回true；否则返回false</returns>
        public bool CheckUserModuleAccess(string userId, string moduleId)
        {
            try
            {
                // 尝试转换userId和moduleId为int，因为现有方法使用int类型
                if (int.TryParse(userId, out int userIdInt) && int.TryParse(moduleId, out int moduleIdInt))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        
                        // 检查用户是否是管理员
                        if (IsAdminUser(userIdInt, connection))
                        {
                            return true; // 管理员拥有所有权限
                        }
                        
                        // 检查用户是否有该模块的权限
                        string query = @"
                            SELECT COUNT(1)
                            FROM Menu m
                            JOIN MenuPermission mp ON m.MenuId = mp.MenuId
                            JOIN UserPermissions up ON mp.PermissionId = up.PermissionId
                            WHERE m.MenuId = @ModuleId AND up.UserId = @UserId";
                        
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ModuleId", moduleIdInt);
                            command.Parameters.AddWithValue("@UserId", userIdInt);
                            
                            int count = Convert.ToInt32(command.ExecuteScalar());
                            return count > 0;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"无法将用户ID '{userId}' 或模块ID '{moduleId}' 转换为整数。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"检查用户模块访问权限失败: {ex.Message}");
                throw new Exception($"检查用户模块访问权限失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 为用户分配模块访问权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleIds">模块ID列表</param>
        /// <returns>如果分配成功，则返回true；否则返回false</returns>
        public bool AssignModulesToUser(string userId, List<string> moduleIds)
        {
            try
            {
                // 尝试转换userId为int
                if (!int.TryParse(userId, out int userIdInt))
                {
                    Console.WriteLine($"无法将用户ID '{userId}' 转换为整数。");
                    return false;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. 先移除用户的所有模块权限（通过移除UserPermissions表中的记录）
                            // 获取与指定模块相关的权限ID
                            string getPermissionIdsQuery = @"
                                SELECT DISTINCT mp.PermissionId
                                FROM MenuPermission mp
                                WHERE mp.MenuId IN (SELECT value FROM STRING_SPLIT(@ModuleIds, ','))";

                            List<int> permissionIds = new List<int>();
                            using (SqlCommand command = new SqlCommand(getPermissionIdsQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@ModuleIds", string.Join(",", moduleIds));
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        permissionIds.Add(Convert.ToInt32(reader["PermissionId"]));
                                    }
                                }
                            }

                            // 删除用户现有的这些权限
                            if (permissionIds.Count > 0)
                            {
                                string deletePermissionsQuery = @"
                                    DELETE FROM UserPermissions 
                                    WHERE UserId = @UserId AND PermissionId IN (SELECT value FROM STRING_SPLIT(@PermissionIds, ','))";

                                using (SqlCommand command = new SqlCommand(deletePermissionsQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@UserId", userIdInt);
                                    command.Parameters.AddWithValue("@PermissionIds", string.Join(",", permissionIds));
                                    command.ExecuteNonQuery();
                                }
                            }

                            // 2. 添加新的模块权限
                            foreach (string moduleId in moduleIds)
                            {
                                if (int.TryParse(moduleId, out int moduleIdInt))
                                {
                                    // 获取该模块的所有权限
                                    string getModulePermissionsQuery = @"
                                        SELECT PermissionId 
                                        FROM MenuPermission 
                                        WHERE MenuId = @ModuleId";

                                    using (SqlCommand command = new SqlCommand(getModulePermissionsQuery, connection, transaction))
                                    {
                                        command.Parameters.AddWithValue("@ModuleId", moduleIdInt);
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                int permissionId = Convert.ToInt32(reader["PermissionId"]);
                                                
                                                // 为用户添加该权限
                                                string addPermissionQuery = @"
                                                    INSERT INTO UserPermissions (UserId, PermissionId)
                                                    VALUES (@UserId, @PermissionId)";

                                                using (SqlCommand addCommand = new SqlCommand(addPermissionQuery, connection, transaction))
                                                {
                                                    addCommand.Parameters.AddWithValue("@UserId", userIdInt);
                                                    addCommand.Parameters.AddWithValue("@PermissionId", permissionId);
                                                    addCommand.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"分配用户模块权限失败: {ex.Message}");
                            throw new Exception($"分配用户模块权限失败: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"分配用户模块权限失败: {ex.Message}");
                throw new Exception($"分配用户模块权限失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 移除用户的模块访问权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleIds">模块ID列表</param>
        /// <returns>如果移除成功，则返回true；否则返回false</returns>
        public bool RemoveUserModules(string userId, List<string> moduleIds)
        {
            try
            {
                // 尝试转换userId为int
                if (!int.TryParse(userId, out int userIdInt))
                {
                    Console.WriteLine($"无法将用户ID '{userId}' 转换为整数。");
                    return false;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (string moduleId in moduleIds)
                            {
                                if (int.TryParse(moduleId, out int moduleIdInt))
                                {
                                    // 获取该模块的所有权限
                                    string getModulePermissionsQuery = @"
                                        SELECT PermissionId 
                                        FROM MenuPermission 
                                        WHERE MenuId = @ModuleId";

                                    List<int> permissionIds = new List<int>();
                                    using (SqlCommand command = new SqlCommand(getModulePermissionsQuery, connection, transaction))
                                    {
                                        command.Parameters.AddWithValue("@ModuleId", moduleIdInt);
                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                permissionIds.Add(Convert.ToInt32(reader["PermissionId"]));
                                            }
                                        }
                                    }

                                    // 删除用户的这些权限
                                    if (permissionIds.Count > 0)
                                    {
                                        string deletePermissionsQuery = @"
                                            DELETE FROM UserPermissions 
                                            WHERE UserId = @UserId AND PermissionId IN (SELECT value FROM STRING_SPLIT(@PermissionIds, ','))";

                                        using (SqlCommand command = new SqlCommand(deletePermissionsQuery, connection, transaction))
                                        {
                                            command.Parameters.AddWithValue("@UserId", userIdInt);
                                            command.Parameters.AddWithValue("@PermissionIds", string.Join(",", permissionIds));
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"移除用户模块权限失败: {ex.Message}");
                            throw new Exception($"移除用户模块权限失败: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"移除用户模块权限失败: {ex.Message}");
                throw new Exception($"移除用户模块权限失败: {ex.Message}", ex);
            }
        }
    }
}
