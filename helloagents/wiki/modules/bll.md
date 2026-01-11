# BLL

## 目的
封装业务规则与策略，协调 DAL 与 UI。

## 模块概述
- **职责:** 登录安全、系统参数、备份策略、权限与日志
- **状态:** ✅稳定
- **最后更新:** 2026-01-11

## 规范

### 需求: 登录安全策略
**模块:** BLL
集中处理锁定阈值、密码策略与失败记录。

#### 场景: 超过失败阈值
- 失败次数累加
- 触发锁定并记录日志

### 需求: 参数中心
**模块:** BLL
提供参数缓存与类型化读取。

#### 场景: 读取参数
- 缓存命中优先
- 失败回退默认值

### 需求: 备份与保留
**模块:** BLL
执行数据库备份与保留策略清理。

## API接口
- SystemParameterService.GetAllParameters
- UserSecurityService.TryGetLockout
- DatabaseBackupService.CreateBackup

## 数据模型
- SystemParameters
- UserSecurity

## 依赖
- DAL
- Utility

## 变更历史
- 202601112006_mdmui_revamp (history/2026-01/202601112006_mdmui_revamp/)
