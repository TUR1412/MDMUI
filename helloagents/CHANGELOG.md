# Changelog

本文件记录项目所有重要变更。
格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/)，
版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

## [Unreleased]
### Added
- AppLog：诊断日志写入文件（按天 + 轮转 + 保留策略）
- CrashReporter：未处理异常统一崩溃报告窗（复制详情/打开日志目录）
- AppTelemetry：轻量性能埋点（`using (Measure)` 记录耗时）
- MSTest 单元测试工程 `MDMUI.Tests` 与 `scripts/test.ps1`
- 系统设置菜单新增“打开日志目录”快捷入口

### Changed
- CI：`msbuild /restore` + `dotnet test` 覆盖基础单测
- 关键路径补充性能埋点：菜单创建、窗体打开、登录初始化

## [1.1.0] - 2026-01-11
### Added
- 系统参数中心与安全参数持久化能力
- 登录失败锁定与审计记录链路
- 数据备份中心与可执行备份流程
- 命令面板使用记录与智能排序/固定
- 统一主题管理与界面风格升级

### Changed
- 登录流程改为结果对象返回与统一锁定处理
- 数据库引导器增强表结构与默认参数初始化

### Fixed
- 用户密码重置与策略校验一致性
