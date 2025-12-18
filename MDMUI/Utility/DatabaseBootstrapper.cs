using System;
using System.Data;
using System.Data.SqlClient;

namespace MDMUI.Utility
{
    /// <summary>
    /// 数据库引导器：确保数据库与最小可运行表结构存在，并尽量补齐基础数据（幂等、非破坏）。
    /// 目标：让应用“首次启动”不会因为缺库/缺表直接崩溃，同时不覆盖用户已有数据。
    /// </summary>
    public static class DatabaseBootstrapper
    {
        public static void EnsureDatabaseReady()
        {
            string connectionString = DbConnectionHelper.GetConnectionString();

            EnsureDatabaseExists(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                EnsureCoreTablesExist(connection);
                EnsureCoreSeedData(connection);
            }
        }

        private static void EnsureDatabaseExists(string connectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;

            // 没配置数据库名时，认为外部已处理
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                return;
            }

            SqlConnectionStringBuilder masterBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };

            using (SqlConnection masterConnection = new SqlConnection(masterBuilder.ConnectionString))
            {
                masterConnection.Open();

                const string sql = @"
IF DB_ID(@DbName) IS NULL
BEGIN
    DECLARE @createSql NVARCHAR(MAX) = N'CREATE DATABASE ' + QUOTENAME(@DbName) + N';';
    EXEC(@createSql);
END";

                using (SqlCommand cmd = new SqlCommand(sql, masterConnection))
                {
                    cmd.Parameters.Add("@DbName", SqlDbType.NVarChar, 128).Value = databaseName;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void EnsureCoreTablesExist(SqlConnection connection)
        {
            EnsureRolesTableExists(connection);
            EnsureUsersTableExists(connection);
            EnsureFactoryTableExists(connection);
            EnsureUserFactoryTableExists(connection);
            EnsurePermissionsTableExists(connection);
            EnsureUserPermissionsTableExists(connection);

            // 业务功能表：确保主要功能页不会因为缺表直接崩溃（幂等、非破坏）
            EnsureProductCategoryTableExists(connection);
            EnsureProductTableExists(connection);
            EnsureGetCategoryDescendantsFunctionExists(connection);
        }

        private static void EnsureRolesTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles (
        RoleId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        RoleName NVARCHAR(50) NOT NULL
    );

    CREATE UNIQUE INDEX UX_Roles_RoleName ON dbo.Roles(RoleName);
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureUsersTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL,
        Password NVARCHAR(64) NOT NULL,
        LastLoginTime DATETIME NULL,
        RealName NVARCHAR(50) NULL,
        Role NVARCHAR(20) NULL,
        RoleId INT NULL
    );

    CREATE UNIQUE INDEX UX_Users_Username ON dbo.Users(Username);
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureFactoryTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.Factory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Factory (
        FactoryId VARCHAR(10) NOT NULL PRIMARY KEY,
        FactoryName NVARCHAR(50) NOT NULL,
        Address NVARCHAR(100) NULL,
        Manager NVARCHAR(50) NULL,
        Phone VARCHAR(20) NULL,
        ManagerEmployeeId VARCHAR(20) NULL
    );
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureUserFactoryTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.UserFactory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserFactory (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId INT NOT NULL,
        FactoryId VARCHAR(10) NOT NULL,
        IsDefault BIT NOT NULL CONSTRAINT DF_UserFactory_IsDefault DEFAULT(0),
        CreateTime DATETIME NOT NULL CONSTRAINT DF_UserFactory_CreateTime DEFAULT(GETDATE())
    );

    CREATE UNIQUE INDEX UX_UserFactory_User_Factory ON dbo.UserFactory(UserId, FactoryId);
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsurePermissionsTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Permissions (
        PermissionId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ModuleName NVARCHAR(50) NOT NULL,
        ActionName NVARCHAR(50) NOT NULL,
        Description NVARCHAR(200) NULL
    );

    CREATE UNIQUE INDEX UX_Permissions_Module_Action ON dbo.Permissions(ModuleName, ActionName);
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureUserPermissionsTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.UserPermissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserPermissions (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId INT NOT NULL,
        PermissionId INT NOT NULL
    );

    CREATE UNIQUE INDEX UX_UserPermissions_User_Permission ON dbo.UserPermissions(UserId, PermissionId);
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureProductCategoryTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.ProductCategory', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductCategory (
        CategoryId VARCHAR(36) NOT NULL PRIMARY KEY,
        CategoryName NVARCHAR(50) NOT NULL,
        ParentCategoryId VARCHAR(36) NULL,
        Description NVARCHAR(200) NULL,
        CreateTime DATETIME NOT NULL CONSTRAINT DF_ProductCategory_CreateTime DEFAULT(GETDATE())
    );

    CREATE INDEX IX_ProductCategory_Parent ON dbo.ProductCategory(ParentCategoryId);
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureProductTableExists(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.Product', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Product (
        ProductId VARCHAR(20) NOT NULL PRIMARY KEY,
        ProductName NVARCHAR(100) NOT NULL,
        CategoryId VARCHAR(36) NOT NULL,
        Specification NVARCHAR(100) NULL,
        Unit NVARCHAR(20) NULL,
        Price DECIMAL(18,2) NULL,
        Cost DECIMAL(18,2) NULL,
        Description NVARCHAR(500) NULL,
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_Product_Status DEFAULT(N'正常'),
        CreateTime DATETIME NOT NULL CONSTRAINT DF_Product_CreateTime DEFAULT(GETDATE()),
        CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES dbo.ProductCategory(CategoryId)
    );

    CREATE INDEX IX_Product_CategoryId ON dbo.Product(CategoryId);
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureGetCategoryDescendantsFunctionExists(SqlConnection connection)
        {
            // CREATE FUNCTION 必须是 batch 的第一个语句，因此使用动态 SQL。
            const string sql = @"
IF OBJECT_ID(N'dbo.GetCategoryDescendants', N'FN') IS NULL
BEGIN
    EXEC(N'
CREATE FUNCTION [dbo].[GetCategoryDescendants]
(
    @RootCategoryId VARCHAR(36)
)
RETURNS @DescendantTable TABLE
(
    CategoryId VARCHAR(36) PRIMARY KEY
)
AS
BEGIN
    ;WITH CategoryHierarchy AS (
        SELECT CategoryId
        FROM dbo.ProductCategory
        WHERE CategoryId = @RootCategoryId

        UNION ALL

        SELECT c.CategoryId
        FROM dbo.ProductCategory c
        INNER JOIN CategoryHierarchy ch ON c.ParentCategoryId = ch.CategoryId
    )
    INSERT INTO @DescendantTable (CategoryId)
    SELECT CategoryId
    FROM CategoryHierarchy
    OPTION (MAXRECURSION 0);

    RETURN;
END
');
END";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void EnsureCoreSeedData(SqlConnection connection)
        {
            int normalRoleId = EnsureRole(connection, "普通用户");
            int superAdminRoleId = EnsureRole(connection, "超级管理员");

            int adminUserId = EnsureUser(
                connection,
                username: "admin",
                passwordHash: PasswordEncryptor.EncryptPassword("1"),
                realName: "系统管理员",
                roleId: superAdminRoleId,
                roleName: "超级管理员");

            EnsureFactory(connection, "F001", "第一机械厂", "北京市海淀区", "张三", "13800138000");
            EnsureFactory(connection, "F002", "第二电子厂", "上海市浦东新区", "李四", "13900139000");

            EnsureUserFactory(connection, adminUserId, "F001", isDefault: true);

            // 最小权限集合：覆盖应用内常用模块（菜单/功能显示需要）
            EnsurePermission(connection, "factory", "view", "查看工厂信息");
            EnsurePermission(connection, "factory", "add", "添加工厂");
            EnsurePermission(connection, "factory", "edit", "编辑工厂");
            EnsurePermission(connection, "factory", "delete", "删除工厂");
            EnsurePermission(connection, "factory", "print", "打印工厂信息");
            EnsurePermission(connection, "factory", "export", "导出工厂数据");

            EnsurePermission(connection, "user", "view", "查看用户信息");
            EnsurePermission(connection, "user", "add", "添加用户");
            EnsurePermission(connection, "user", "edit", "编辑用户");
            EnsurePermission(connection, "user", "delete", "删除用户");
            EnsurePermission(connection, "user", "reset_pwd", "重置用户密码");

            EnsurePermission(connection, "area", "view", "查看生产地信息");
            EnsurePermission(connection, "area", "add", "添加生产地");
            EnsurePermission(connection, "area", "edit", "编辑生产地");
            EnsurePermission(connection, "area", "delete", "删除生产地");
            EnsurePermission(connection, "area", "print", "打印生产地信息");

            EnsurePermission(connection, "system", "view", "查看系统设置");
            EnsurePermission(connection, "system", "backup", "备份系统");
            EnsurePermission(connection, "system", "restore", "恢复系统");
            EnsurePermission(connection, "system", "log", "查看系统日志");

            // MainForm 菜单权限（至少保证“view”存在）
            EnsurePermission(connection, "process", "view", "查看工艺管理");
            EnsurePermission(connection, "product", "view", "查看产品管理");
            EnsurePermission(connection, "product", "add", "添加产品");
            EnsurePermission(connection, "product", "edit", "编辑产品");
            EnsurePermission(connection, "product", "delete", "删除产品");
            EnsurePermission(connection, "product", "export", "导出产品");
            EnsurePermission(connection, "production", "view", "查看生产管理");
            EnsurePermission(connection, "equipment", "view", "查看设备管理");
            EnsurePermission(connection, "inventory", "view", "查看库存管理");

            // 其他窗体/模块权限（覆盖当前代码里 HasPermission 使用到的 Module/Action 组合）
            EnsurePermission(connection, "department", "view", "查看部门信息");
            EnsurePermission(connection, "department", "add", "添加部门");
            EnsurePermission(connection, "department", "edit", "编辑部门");
            EnsurePermission(connection, "department", "delete", "删除部门");

            EnsurePermission(connection, "equipment_group", "view", "查看设备组");
            EnsurePermission(connection, "equipment_group", "add", "添加设备组");
            EnsurePermission(connection, "equipment_group", "edit", "编辑设备组");
            EnsurePermission(connection, "equipment_group", "delete", "删除设备组");

            EnsurePermission(connection, "sub_device", "manage", "管理子设备");
            EnsurePermission(connection, "port_config", "manage", "管理端口配置");

            EnsurePermission(connection, "permission", "view", "查看权限设置");
            EnsurePermission(connection, "permission", "edit", "编辑权限设置");

            EnsurePermission(connection, "product_category", "view", "查看产品类别");
            EnsurePermission(connection, "product_category", "add", "添加产品类别");
            EnsurePermission(connection, "product_category", "edit", "编辑产品类别");
            EnsurePermission(connection, "product_category", "delete", "删除产品类别");

            // 默认让管理员拥有全部已定义权限（即便代码有“超级管理员直通”，也保证数据一致）
            EnsureUserHasAllPermissions(connection, adminUserId);

            // 最小产品类别种子数据（幂等、不覆盖）
            EnsureProductCategory(connection, "PC001", "电子产品", null, "各种消费类电子产品");
            EnsureProductCategory(connection, "PC002", "家用电器", null, "冰箱、洗衣机、空调等");
            EnsureProductCategory(connection, "PC001-01", "手机", "PC001", "各种智能手机和功能手机");
            EnsureProductCategory(connection, "PC001-02", "笔记本电脑", "PC001", "便携式个人电脑");
            EnsureProductCategory(connection, "PC001-03", "平板电脑", "PC001", "触摸屏移动设备");
            EnsureProductCategory(connection, "PC002-01", "冰箱", "PC002", "用于冷藏和冷冻食物");
            EnsureProductCategory(connection, "PC002-02", "洗衣机", "PC002", "用于清洗衣物");
            EnsureProductCategory(connection, "PC002-03", "空调", "PC002", "用于调节室内温度");
        }

        private static int EnsureRole(SqlConnection connection, string roleName)
        {
            const string selectSql = "SELECT RoleId FROM dbo.Roles WHERE RoleName = @RoleName";
            using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
            {
                selectCmd.Parameters.AddWithValue("@RoleName", roleName);
                object existing = selectCmd.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                {
                    return Convert.ToInt32(existing);
                }
            }

            const string insertSql = "INSERT INTO dbo.Roles (RoleName) VALUES (@RoleName); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
            {
                insertCmd.Parameters.AddWithValue("@RoleName", roleName);
                return Convert.ToInt32(insertCmd.ExecuteScalar());
            }
        }

        private static int EnsureUser(
            SqlConnection connection,
            string username,
            string passwordHash,
            string realName,
            int roleId,
            string roleName)
        {
            const string selectSql = "SELECT Id FROM dbo.Users WHERE Username = @Username";
            using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
            {
                selectCmd.Parameters.AddWithValue("@Username", username);
                object existing = selectCmd.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                {
                    return Convert.ToInt32(existing);
                }
            }

            const string insertSql = @"
INSERT INTO dbo.Users (Username, Password, RealName, RoleId, Role)
VALUES (@Username, @Password, @RealName, @RoleId, @RoleName);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
            {
                insertCmd.Parameters.AddWithValue("@Username", username);
                insertCmd.Parameters.AddWithValue("@Password", passwordHash);
                insertCmd.Parameters.AddWithValue("@RealName", (object)realName ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@RoleId", roleId);
                insertCmd.Parameters.AddWithValue("@RoleName", (object)roleName ?? DBNull.Value);
                return Convert.ToInt32(insertCmd.ExecuteScalar());
            }
        }

        private static void EnsureFactory(
            SqlConnection connection,
            string factoryId,
            string factoryName,
            string address,
            string manager,
            string phone)
        {
            const string selectSql = "SELECT COUNT(1) FROM dbo.Factory WHERE FactoryId = @FactoryId";
            using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
            {
                selectCmd.Parameters.AddWithValue("@FactoryId", factoryId);
                int count = Convert.ToInt32(selectCmd.ExecuteScalar());
                if (count > 0)
                {
                    return;
                }
            }

            const string insertSql = @"
INSERT INTO dbo.Factory (FactoryId, FactoryName, Address, Manager, Phone)
VALUES (@FactoryId, @FactoryName, @Address, @Manager, @Phone);";

            using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
            {
                insertCmd.Parameters.AddWithValue("@FactoryId", factoryId);
                insertCmd.Parameters.AddWithValue("@FactoryName", factoryName);
                insertCmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Manager", (object)manager ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);
                insertCmd.ExecuteNonQuery();
            }
        }

        private static void EnsureUserFactory(SqlConnection connection, int userId, string factoryId, bool isDefault)
        {
            const string selectSql = "SELECT COUNT(1) FROM dbo.UserFactory WHERE UserId = @UserId AND FactoryId = @FactoryId";
            using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
            {
                selectCmd.Parameters.AddWithValue("@UserId", userId);
                selectCmd.Parameters.AddWithValue("@FactoryId", factoryId);
                int count = Convert.ToInt32(selectCmd.ExecuteScalar());
                if (count > 0)
                {
                    return;
                }
            }

            const string insertSql = @"
INSERT INTO dbo.UserFactory (UserId, FactoryId, IsDefault)
VALUES (@UserId, @FactoryId, @IsDefault);";

            using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
            {
                insertCmd.Parameters.AddWithValue("@UserId", userId);
                insertCmd.Parameters.AddWithValue("@FactoryId", factoryId);
                insertCmd.Parameters.AddWithValue("@IsDefault", isDefault);
                insertCmd.ExecuteNonQuery();
            }
        }

        private static void EnsurePermission(SqlConnection connection, string moduleName, string actionName, string description)
        {
            const string selectSql = "SELECT PermissionId FROM dbo.Permissions WHERE ModuleName = @ModuleName AND ActionName = @ActionName";
            using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
            {
                selectCmd.Parameters.AddWithValue("@ModuleName", moduleName);
                selectCmd.Parameters.AddWithValue("@ActionName", actionName);
                object existing = selectCmd.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                {
                    return;
                }
            }

            const string insertSql = @"
INSERT INTO dbo.Permissions (ModuleName, ActionName, Description)
VALUES (@ModuleName, @ActionName, @Description);";

            using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
            {
                insertCmd.Parameters.AddWithValue("@ModuleName", moduleName);
                insertCmd.Parameters.AddWithValue("@ActionName", actionName);
                insertCmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                insertCmd.ExecuteNonQuery();
            }
        }

        private static void EnsureProductCategory(
            SqlConnection connection,
            string categoryId,
            string categoryName,
            string parentCategoryId,
            string description)
        {
            const string selectSql = "SELECT COUNT(1) FROM dbo.ProductCategory WHERE CategoryId = @CategoryId";
            using (SqlCommand selectCmd = new SqlCommand(selectSql, connection))
            {
                selectCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                int count = Convert.ToInt32(selectCmd.ExecuteScalar());
                if (count > 0)
                {
                    return;
                }
            }

            const string insertSql = @"
INSERT INTO dbo.ProductCategory (CategoryId, CategoryName, ParentCategoryId, Description)
VALUES (@CategoryId, @CategoryName, @ParentCategoryId, @Description);";

            using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
            {
                insertCmd.Parameters.AddWithValue("@CategoryId", categoryId);
                insertCmd.Parameters.AddWithValue("@CategoryName", categoryName);
                insertCmd.Parameters.AddWithValue("@ParentCategoryId", (object)parentCategoryId ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                insertCmd.ExecuteNonQuery();
            }
        }

        private static void EnsureUserHasAllPermissions(SqlConnection connection, int userId)
        {
            const string sql = @"
INSERT INTO dbo.UserPermissions (UserId, PermissionId)
SELECT @UserId, p.PermissionId
FROM dbo.Permissions p
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.UserPermissions up
    WHERE up.UserId = @UserId AND up.PermissionId = p.PermissionId
);";

            using (SqlCommand cmd = new SqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
