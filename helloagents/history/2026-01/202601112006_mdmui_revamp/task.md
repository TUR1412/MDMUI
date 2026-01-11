# 任务清单: MDMUI 全链路重构与功能增强

目录: `helloagents/plan/202601112006_mdmui_revamp/`

---

## 1. 安全与参数中心
- [√] 1.1 在 `MDMUI/Utility/DatabaseBootstrapper.cs` 中补齐 SystemParameters 与 UserSecurity 表，验证 why.md#需求-登录安全与锁定-场景-连续失败
- [√] 1.2 新增 `MDMUI/DAL/SystemParameterDAL.cs` 与 `MDMUI/BLL/SystemParameterService.cs`，验证 why.md#需求-系统参数中心-场景-参数编辑
- [√] 1.3 新增 `MDMUI/DAL/UserSecurityDAL.cs` 与 `MDMUI/BLL/UserSecurityService.cs`，验证 why.md#需求-登录安全与锁定-场景-连续失败
- [√] 1.4 更新 `MDMUI/DAL/UserDAL.cs` 与 `MDMUI/BLL/UserBLL.cs` 的登录流程，验证 why.md#需求-登录安全与锁定-场景-连续失败

## 2. 数据备份中心
- [√] 2.1 新增 `MDMUI/BLL/DatabaseBackupService.cs`，验证 why.md#需求-数据备份中心-场景-一键备份
- [√] 2.2 更新 `MDMUI/Forms/FrmDataBackup.cs` 实现备份界面逻辑，验证 why.md#需求-数据备份中心-场景-一键备份

## 3. 命令面板智能
- [√] 3.1 更新 `MDMUI/Utility/AppPreferencesStore.cs` 与新增命令使用记录结构，验证 why.md#需求-命令面板智能-场景-快速打开功能页
- [√] 3.2 更新 `MDMUI/Forms/CommandPaletteForm.cs` 与 `MDMUI/Forms/MainForm.cs` 的排序与记录，验证 why.md#需求-命令面板智能-场景-快速打开功能页

## 4. 主题系统与 UI 统一
- [√] 4.1 新增 `MDMUI/Utility/ThemeManager.cs` 并统一主题参数，验证 why.md#需求-主题统一-场景-主界面子窗体
- [√] 4.2 更新 `MDMUI/Forms/Form1.cs`、`MDMUI/Forms/MainForm.cs` 的主题应用，验证 why.md#需求-主题统一-场景-主界面子窗体

## 5. 审计日志与修复
- [√] 5.1 更新 `MDMUI/DAL/SystemLogDAL.cs` 与新增 `MDMUI/Utility/AuditTrail.cs`，验证 why.md#需求-登录安全与锁定-场景-连续失败
- [√] 5.2 修复 `MDMUI/BLL/UserService.cs` 密码重置逻辑，验证 why.md#需求-系统参数中心-场景-参数编辑

## 6. 文档与版本
- [√] 6.1 更新 `README.md` 为双语说明
- [√] 6.2 更新 `MDMUI/Properties/AssemblyInfo.cs` 版本号
- [√] 6.3 同步更新知识库文档

## 7. 安全检查
- [√] 7.1 执行安全检查（按G9: 输入验证、敏感信息处理、权限控制、EHRB风险规避）

## 8. 测试
- [X] 8.1 执行本地构建或关键路径验证（失败：App.config 复制到 bin\\Release\\MDMUI.exe.config 被占用）
