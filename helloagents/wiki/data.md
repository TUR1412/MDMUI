# 数据模型

## 概述
本项目以 SQL Server LocalDB 为主要存储，核心数据表覆盖用户、权限、基础资料与日志。

---

## 数据表/集合

### Users
**描述:** 用户主数据

| 字段名 | 类型 | 约束 | 说明 |
|--------|------|------|------|
| Id | INT | PK | 用户ID |
| Username | NVARCHAR(50) | UNIQUE | 登录名 |
| Password | NVARCHAR(64) | NOT NULL | SHA-256 哈希 |
| RoleId | INT | FK | 角色 |
| LastLoginTime | DATETIME | NULL | 最后登录 |

### Roles
| 字段名 | 类型 | 约束 | 说明 |
| RoleId | INT | PK | 角色ID |
| RoleName | NVARCHAR(50) | UNIQUE | 角色名 |

### Permissions / UserPermissions
**描述:** 权限定义与用户授权关系

### Factory / UserFactory
**描述:** 工厂信息与用户默认工厂绑定

### Product / ProductCategory
**描述:** 产品与类别数据

### Department / Employee / Area
**描述:** 组织与区域基础数据

### SystemLog
**描述:** 关键操作审计日志

| 字段名 | 类型 | 约束 | 说明 |
| LogId | INT | PK | 日志ID |
| UserId | INT | FK | 用户ID |
| OperationType | NVARCHAR(50) |  | 操作类型 |
| OperationModule | NVARCHAR(50) |  | 操作模块 |
| Description | NVARCHAR(200) |  | 描述 |
| LogTime | DATETIME |  | 时间 |

### SystemParameters
**描述:** 系统参数中心（可配置策略）

| 字段名 | 类型 | 约束 | 说明 |
| ParamKey | NVARCHAR(120) | PK | 参数键 |
| ParamValue | NVARCHAR(2000) |  | 参数值 |
| Description | NVARCHAR(200) |  | 说明 |
| UpdatedAt | DATETIME |  | 更新时间 |

### UserSecurity
**描述:** 登录安全与锁定状态

| 字段名 | 类型 | 约束 | 说明 |
| UserId | INT | PK/FK | 用户ID |
| FailedCount | INT |  | 失败次数 |
| LockoutUntil | DATETIME |  | 锁定至 |
| LastSuccessAt | DATETIME |  | 最近成功 |

---

**索引:**
- Users.Username (唯一)
- Permissions(ModuleName, ActionName) (唯一)
- SystemLog.LogTime (查询排序)

**关联关系:**
- Users ↔ Roles
- Users ↔ UserPermissions ↔ Permissions
- Users ↔ UserSecurity
