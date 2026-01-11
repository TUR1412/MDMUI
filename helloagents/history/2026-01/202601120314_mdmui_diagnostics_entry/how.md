# how - 实施方式

## 1) 日志目录快捷入口
- 在 `MainForm` 的“系统设置”菜单下增加“打开日志目录”
- 使用 `AppLog.LogDirectory` 作为 SSOT，必要时自动创建目录
- 采用 `explorer.exe` 打开目录，失败时提示并记录日志

## 2) 关键路径性能埋点
- 在以下位置加入 `AppTelemetry.Measure(...)`：
  - `MainForm.CreateMainMenu`（菜单构建耗时）
  - `MainForm.OpenFunctionForm`（打开功能窗体耗时）
  - `Form1` 登录初始化关键步骤（DB 引导、加载工厂、应用偏好）

## 3) 文档同步
- README 补充“系统设置 -> 打开日志目录”入口说明（中英）
- 更新知识库与变更日志，保持文档与代码一致

