# Changelog

本文件记录项目所有重要变更。
格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/)，
版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

## [Unreleased]
### Added
- AppLog：诊断日志写入文件（按天 + 轮转 + 保留策略）
- CrashReporter：未处理异常统一崩溃报告窗（复制详情/打开日志目录）
- AppTelemetry：轻量性能埋点（`using (Measure)` 记录耗时）
- PasswordGenerator：生成强密码（满足常见策略要求，用于重置密码兜底）
- GridStyler：统一 DataGridView 视觉规范（表头/行高/交替行/字体）
- IThemeSelfStyled：主题系统的“自绘控件自管理”标记接口（避免覆盖关键样式）
- ActionToolbar：Atomic Design Molecule（输入区 + 操作区布局）
- UiThemingBootstrapper：全局自动应用主题与微交互（避免逐窗体遗漏）
- UiSafe：UI 事件安全执行（错误边界），捕获异常并写入 AppLog（可选弹窗提示）
- LogFileService：日志文件列表与 tail 读取（支持共享读写）
- FrmFileLogViewer：应用内诊断日志查看器（过滤/复制/外部打开）
- MSTest 单元测试工程 `MDMUI.Tests` 与 `scripts/test.ps1`
- 系统设置菜单新增“打开日志目录”快捷入口

### Changed
- CI：`msbuild /restore` + `dotnet test` 覆盖基础单测
- ThemeManager：Button 默认采用 Secondary 风格，仅 Form.AcceptButton 自动使用 Accent
- 关键路径补充性能埋点：菜单创建、窗体打开、登录初始化
- 系统参数中心：筛选/保存体验提升，支持未保存变更提示与行高亮
- 数据备份中心：使用统一工具栏布局，支持双击打开备份文件
- 操作日志查询：统一卡片布局与网格视觉规范，新增空态/状态栏并补充埋点
- 系统设置：新增“日志查看器”入口（与“打开日志目录”并列）
- 用户管理：重置密码优先使用配置默认值，策略不满足时自动生成强密码并复制到剪贴板
- 用户管理：运行时引入 CardPanel + ActionToolbar 工具栏，新增空态/状态栏提示并移除无结果弹窗
- 区域管理：运行时引入 CardPanel + ActionToolbar 工具栏，新增空态/状态栏提示并补充埋点
- 工厂管理：运行时引入 CardPanel + ActionToolbar 工具栏，新增空态/状态栏与加载埋点，并将加载异常写入 AppLog
- 产品管理：运行时引入 CardPanel + ActionToolbar 工具栏，新增空态/状态栏与搜索埋点，并统一按钮/标签文本避免乱码
- 产品类别管理：运行时引入 CardPanel + ActionToolbar 工具栏，新增树/列表空态提示并补充加载埋点
- 部门管理：运行时引入 CardPanel + ActionToolbar 工具栏，新增空态/状态栏与加载埋点，并补齐列表显示字段（上级部门/所属工厂/负责人）
- 区域管理：运行时替换旧控件后清理已释放引用，降低后续误用风险

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
