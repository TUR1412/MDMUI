# MDMUI

MDMUI 是一个基于 **.NET Framework 4.8 / WinForms** 的桌面管理端示例项目，配套使用 **SQL Server LocalDB** 作为本地数据库。

本仓库已做工程化清理与增强：
- 源码不再以 zip 形式存放，已解包并纳入版本管理
- 默认支持 `dotnet build` 构建（已处理 .NET Framework WinForms 在部分 MSBuild 版本下的 `GenerateResource` 构建问题）
- 启动时会进行“非破坏性”数据库引导（缺库/缺表会自动创建最小可运行结构，不会覆盖用户已有数据）

## 1) 环境要求

- Windows 10/11
- .NET Framework 4.8（运行时）
- 构建工具（二选一）：
  - Visual Studio 2022（包含 .NET 桌面开发），或
  - .NET SDK（建议 8.x / 9.x 均可）
- SQL Server LocalDB：实例名默认 `MSSQLLocalDB`

## 2) 构建

仓库根目录执行：

```powershell
dotnet build .\MDMUI\MDMUI.sln -c Release
```

生成产物默认在：
- `MDMUI/bin/Release/MDMUI.exe`

## 3) 数据库与初始化

默认连接字符串位于：`MDMUI/App.config` 的 `DefaultConnection`。

应用启动时会自动：
- 如果 `UserDB` 不存在：尝试创建数据库
- 如果核心表不存在：创建最小可运行表结构（如 `Users/Roles/Factory/UserFactory/Permissions/UserPermissions`）
- 写入必要的基础数据（幂等、不会覆盖已有数据）

如果你需要完整演示数据与更多表结构，可在数据库中执行：
- `MDMUI/dbo.sql`

## 4) 默认账号（首次初始化时）

- 用户名：`admin`
- 密码：`1`

建议首次登录后立即修改默认密码。

