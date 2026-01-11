# DAL

## 目的
负责数据库访问、数据映射与持久化。

## 模块概述
- **职责:** 用户/权限/日志/参数/安全数据的读写
- **状态:** ✅稳定
- **最后更新:** 2026-01-11

## 规范

### 需求: 参数持久化
**模块:** DAL
支持系统参数的读取、更新与幂等初始化。

#### 场景: Upsert 参数
- 参数存在则更新
- 不存在则插入

### 需求: 登录安全状态
**模块:** DAL
记录失败次数与锁定时间。

#### 场景: 登录失败
- 失败次数累加
- 按阈值更新锁定时间

## API接口
- SystemParameterDAL.GetAll
- UserSecurityDAL.RecordFailure

## 数据模型
- SystemParameters
- UserSecurity

## 依赖
- Utility.DbConnectionHelper

## 变更历史
- 202601112006_mdmui_revamp (history/2026-01/202601112006_mdmui_revamp/)
