# Scripts

## 目的
封装构建、清理与环境初始化脚本。

## 模块概述
- **职责:** build/clean/genesis 脚本
- **状态:** ✅稳定
- **最后更新:** 2026-01-11

## 规范

### 需求: 可重复构建
**模块:** Scripts
提供一致的本地构建入口。

#### 场景: Release 构建
- 调用 build.ps1
- 输出 Release 产物

## API接口
- build.ps1
- clean.ps1
- genesis.ps1

## 数据模型
- 无

## 依赖
- PowerShell

## 变更历史
- 202601112006_mdmui_revamp (history/2026-01/202601112006_mdmui_revamp/)
