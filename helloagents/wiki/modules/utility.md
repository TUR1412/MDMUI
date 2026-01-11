# Utility

## 目的
提供主题、偏好、安全与通用工具。

## 模块概述
- **职责:** 主题系统、偏好存储、审计日志、连接与参数辅助、诊断日志、崩溃兜底、性能埋点
- **状态:** ✅稳定
- **最后更新:** 2026-01-12

## 规范

### 需求: 主题系统
**模块:** Utility
集中管理颜色、字体、控件样式。

#### 场景: 全局应用主题
- 主题参数来自系统参数或默认值
- 子窗体自动继承
- Button 默认采用 Secondary 风格；仅将 Form.AcceptButton 自动上色为 Accent，避免误覆盖“删除”等语义按钮
- 需要语义按钮时优先使用 `AppButton`（Primary/Secondary/Danger）或明确设置 `BackColor/ForeColor`

### 需求: 命令面板偏好
**模块:** Utility
保存最近使用与固定的命令。

#### 场景: 记录命令
- 记录使用次数与时间
- 供排序与推荐

### 需求: UI 错误边界（安全执行）
**模块:** Utility
为 WinForms 事件处理/用户操作提供统一的 try/catch 包装，记录诊断日志并在必要时提示用户，避免异常直接冒泡为“未处理异常”。

#### 场景: 点击按钮/选择变更导致异常
- 捕获异常并写入 `AppLog`（包含事件ID，便于日志定位）
- 支持注入自定义 `onError`（便于单元测试或页面自定义提示）
- 非 UI 消息循环环境下默认不弹窗（避免测试/后台任务卡住）

## API接口
- ThemeManager.ApplyTo
- GridStyler.Apply
- IThemeSelfStyled
- UiThemingBootstrapper.Install
- LogFileService.GetLogFiles / ReadTailLines
- CommandUsageStore.RecordUse / TogglePin
- AuditTrail.Log
- DbConnectionHelper.GetConnectionString
- AppLog.Initialize / Info / Warn / Error / Flush / Shutdown
- CrashReporter.Report
- AppTelemetry.Measure
- UiSafe.Run / Wrap
- PasswordGenerator.GenerateStrong / Generate

## 数据模型
- AppPreferences (本地 XML)

## 依赖
- CommonHelper
- AppPreferencesStore
- System.Configuration / System.Diagnostics

## 变更历史
- 202601112006_mdmui_revamp (history/2026-01/202601112006_mdmui_revamp/)        
- 202601120248_mdmui_observability (history/2026-01/202601120248_mdmui_observability/)
- 202601120314_mdmui_diagnostics_entry (history/2026-01/202601120314_mdmui_diagnostics_entry/)
