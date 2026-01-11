# API 手册

## 概述
本项目为桌面应用，无对外 HTTP API。以下列出核心内部服务接口，供模块协作参考。

## 认证方式
- 本地数据库用户校验 + 权限表控制

---

## 接口列表

### 用户认证

#### [Method] UserBLL.TryLogin
**描述:** 验证用户并返回包含锁定状态的结果

**请求参数:**
| 参数名 | 类型 | 必填 | 说明 |
|--------|------|------|------|
| username | string | 是 | 用户名 |
| password | string | 是 | 密码（明文） |

**响应:**
```json
{
  "success": true,
  "failureReason": null,
  "lockoutUntil": null,
  "remainingAttempts": 3
}
```

### 系统参数

#### [Method] SystemParameterService.GetAllParameters
**描述:** 获取所有系统参数

### 数据备份

#### [Method] DatabaseBackupService.CreateBackup
**描述:** 触发数据库备份并按保留策略清理
