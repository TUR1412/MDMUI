# how - 实施方式

## 1) 诊断日志（AppLog）
- 新增 `Utility/AppLog.cs`：默认写入 `%LOCALAPPDATA%\\MDMUI\\logs`
- 支持：
  - 按天切分：`mdmui-YYYYMMDD.log`
  - 单文件大小轮转（`MDMUI.LogMaxMB` / `MDMUI_LOG_MAX_MB`）
  - 保留策略清理（`MDMUI.LogRetentionDays` / `MDMUI_LOG_RETENTION_DAYS`）
  - 可禁用（`MDMUI.LogDisabled` / `MDMUI_LOG_DISABLED`）
- 通过自定义 `TraceListener` 捕获 `Debug.WriteLine` / `Trace.WriteLine`，实现“现有日志不改动也能落盘”的增量接入

## 2) 全局异常兜底（CrashReporter + CrashReportForm）
- 在 `Program.cs` 统一注册：
  - `Application.ThreadException`
  - `AppDomain.CurrentDomain.UnhandledException`
  - `TaskScheduler.UnobservedTaskException`
- 新增 `CrashReportForm`：
  - 事件 ID（便于支持定位）
  - 可复制完整详情（堆栈/环境信息）
  - 一键打开日志目录

## 3) 性能埋点（AppTelemetry）
- 新增 `Utility/AppTelemetry.cs`，提供 `using (Measure(\"xxx\"))` 的轻量耗时记录
- 默认写入 AppLog，以日志方式承载“埋点事件”与耗时

## 4) 单元测试与 CI
- 新增 `MDMUI.Tests`（MSTest，`net48`）
- 新增 `scripts/test.ps1` 作为统一测试入口
- 更新 GitHub Actions：
  - `msbuild /restore` 以兼容新增测试工程的 NuGet 依赖
  - 增加 `dotnet test` 步骤

## 5) UI 原子控件增量扩展
- 新增 `Controls/Atoms/AppButton` 与 `Controls/Atoms/CardPanel`
- CrashReportForm 使用原子控件与 ThemeManager 进行一致化样式渲染（不影响原有窗体结构）

