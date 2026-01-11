# MDMUI

```text
 __  __ ____   __  __ _   _ ___
|  \\/  |  _ \\ |  \\/  | | | |_ _|
| |\\/| | | | || |\\/| | | | || |
| |  | | |_| || |  | | |_| || |
|_|  |_|____/ |_|  |_|\\___/|___|
WinForms · .NET Framework 4.8 · SQL Server LocalDB
```

[![build](https://github.com/TUR1412/MDMUI/actions/workflows/build.yml/badge.svg)](https://github.com/TUR1412/MDMUI/actions/workflows/build.yml)
![platform](https://img.shields.io/badge/platform-Windows%2010%2F11-0078D4?logo=windows&logoColor=white)
![dotnet](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?logo=.net&logoColor=white)
![ui](https://img.shields.io/badge/UI-WinForms-0A0A0A?logo=windows11&logoColor=white)

> 中文：一个可直接运行的 WinForms 管理端示例，强调现代视觉 + 清晰分层 + 可配置策略。
> English: A runnable WinForms management demo focusing on modern visuals, clean layering, and configurable policies.

---

## ✨ 功能亮点 | Highlights

- 🔐 登录安全：失败计数 + 锁定策略 + 密码策略
- ⚙️ 系统参数中心：安全/备份/UI 均可配置
- 🗃️ 数据备份中心：一键备份 + 保留策略
- ⌨️ 命令面板智能排序：最近/常用优先，支持 Ctrl+P 固定
- 🧊 未来感主题系统：统一色彩、字体与控件风格
- 🧯 全局异常兜底：崩溃报告窗 + 复制详情 + 日志定位
- 📝 文件日志：默认写入 `%LOCALAPPDATA%\\MDMUI\\logs`，支持轮转与保留策略
- ✅ 单元测试：MSTest + CI 测试步骤
- 🧾 操作审计：关键操作写入 SystemLog

---

## 🧭 架构 | Architecture

```mermaid
flowchart LR
  UI[WinForms UI] -->|交互| BLL[Business Logic Layer]
  BLL -->|SQL| DAL[Data Access Layer]
  DAL --> DB[(SQL Server LocalDB)]

  UI --> Theme[ThemeManager & Controls]
```

---

## 🚀 快速开始 | Quick Start

### 1) 环境要求 | Requirements
- Windows 10/11
- .NET Framework 4.8（运行时）
- Visual Studio 2022（.NET 桌面开发）或 .NET SDK 8/9
- SQL Server LocalDB（默认实例 `MSSQLLocalDB`）

### 2) 构建 | Build

```powershell
dotnet build .\MDMUI\MDMUI.sln -c Release
```

或使用脚本：

```powershell
pwsh -File .\scripts\build.ps1 -Configuration Release
```

### 3) 测试 | Test

```powershell
pwsh -File .\scripts\test.ps1 -Configuration Release
```

### 4) 数据库初始化 | Database
- 默认连接字符串在 `MDMUI/App.config` -> `DefaultConnection`
- 可用环境变量覆盖：

```powershell
$env:MDMUI_CONNECTIONSTRING = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True"
```

应用启动会自动创建最小可运行结构（不覆盖已有数据）。如需完整演示数据，可执行：  
- `MDMUI/dbo.sql`

### 5) 默认账号 | Default Account
- 用户名：`admin`
- 密码：`1`

---

## ⚙️ 系统参数 | System Parameters

参数页面支持统一管理安全/备份/主题策略，例如：
- `Security.MaxFailedLogin`
- `Security.LockoutMinutes`
- `Backup.RetentionDays`
- `UI.AccentColor`

---

## 🗃️ 数据备份 | Backup

在“数据备份中心”中选择目录并执行备份，支持保留策略自动清理旧文件。

---

## ⌨️ Command Palette

- `Ctrl + K` 打开命令面板
- 支持空格分词检索
- `Ctrl + P` 固定/取消固定常用功能

---

## 📂 目录结构 | Structure

- `MDMUI/Forms/`：WinForms 界面
- `MDMUI/Controls/`：自定义控件
- `MDMUI/BLL/`：业务逻辑层
- `MDMUI/DAL/`：数据访问层
- `MDMUI/Model/`：模型
- `MDMUI/Utility/`：主题/安全/工具
- `scripts/`：构建与清理脚本

---

## 🔐 安全说明 | Security

- 登录失败触发锁定策略，可在系统参数中调整阈值与时长
- 密码策略可配置（长度/数字/大小写/特殊字符）

---

## 🧰 日志与诊断 | Logging & Diagnostics

- 默认日志目录：`%LOCALAPPDATA%\\MDMUI\\logs`
- 默认日志文件：`mdmui-YYYYMMDD.log`（按天），并对单文件大小做轮转
- 应用内入口：`系统设置 -> 打开日志目录`（需要 system:view 权限或超级管理员）
- 配置覆盖（环境变量优先于 App.config）：
  - `MDMUI_LOG_DIR` / `MDMUI.LogDirectory`
  - `MDMUI_LOG_MAX_MB` / `MDMUI.LogMaxMB`
  - `MDMUI_LOG_RETENTION_DAYS` / `MDMUI.LogRetentionDays`
  - `MDMUI_LOG_DISABLED` / `MDMUI.LogDisabled`
- 未处理异常会弹出崩溃报告窗（可复制详情/打开日志目录），便于定位问题

English:
- Default log dir: `%LOCALAPPDATA%\\MDMUI\\logs` (daily file `mdmui-YYYYMMDD.log`, with size-based rotation)
- In-app shortcut: `System Settings -> Open Log Folder` (requires system:view or Super Admin)
- Overrides: environment variables > `App.config` (`MDMUI_LOG_DIR`, `MDMUI_LOG_MAX_MB`, `MDMUI_LOG_RETENTION_DAYS`, `MDMUI_LOG_DISABLED`)
- Unhandled exceptions show a crash report dialog (copy details / open logs)

---

## 🌐 English Summary

MDMUI is a WinForms management demo with modern UI, a clear BLL/DAL split, configurable security policies, a backup center, and a smart command palette. It targets LocalDB for a zero-setup experience while keeping logs and parameter management consistent.
