# 项目技术约定

---

## 技术栈
- **核心:** C# / .NET Framework 4.8
- **UI:** WinForms
- **数据:** SQL Server LocalDB (默认 MSSQLLocalDB)
- **构建:** Visual Studio 2022 或 dotnet build

---

## 开发约定
- **代码规范:** C# 标准命名（PascalCase 类/方法，camelCase 变量）
- **命名约定:** 文件名与主类名一致，UI 控件使用语义前缀（如 btn/lbl/txt）
- **结构约定:** 分层结构固定为 Forms → BLL → DAL → DB

---

## 错误与日志
- **策略:** DAL/BLL 抛出明确异常，UI 层负责提示与降级
- **日志:** SystemLog 表记录关键操作；Debug/Console 仅用于开发诊断

---

## 测试与流程
- **测试:** 以本地人工验证为主（登录、菜单、备份、参数页）
- **提交:** 建议使用 feat/fix/refactor/chore 风格的提交消息
