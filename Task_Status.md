# [MDMUI-20251218] 任务看板
> **环境**: Windows 11 | **Shell**: `pwsh -NoLogo -NoProfile -Command '...'` | **.NET SDK**: 9.x | **档位**: 4档（架构重构/清理+修复）
> **已激活矩阵**: [模块 B: 逻辑直通] + [模块 E: 幽灵防御] + [模块 F: 需求镜像]

## 1. 需求镜像 (Requirement Mirroring)
> **我的理解**: 对远程仓库 `TUR1412/MDMUI` 做“原子级”审查，补齐可构建性/可维护性/可用性，修复明显缺陷并增加工程化能力（文档、CI、初始化/容错），最终推送到远程；推送完成后删除本地克隆目录。
>
> **不做什么**:
> - 不在后台启动任何常驻服务（无幽灵进程）。
> - 不做破坏性数据库“覆盖式重建”（不会自动 DROP 用户已有表/数据）。

## 2. 进化知识库 (Evolutionary Knowledge - Ω)
- [!] 仓库当前仅包含 `MDMUI.zip`，源码被打包在 zip 内，且 zip 内含 `bin/obj/.vs` 等产物，应解包并清理后再入库。
- [!] `dotnet build` 构建 .NET Framework WinForms 时，`GenerateResource` 在默认 x86 TaskHost 可能失败，需要固定 `GenerateResourceMSBuildArchitecture=x64`。
- [!] `Forms/Form1.cs` 存在多个空实现（`// ... existing code ...`），导致“表初始化/权限表创建/工厂表创建/默认密码重置”逻辑缺失，需要补齐为可运行实现。
- [!] `DAL/UserDAL.ResetPassword` 存在“二次加密”逻辑缺陷，需要修复。
- [i] 数据库引导已扩展：除核心表外，补齐 `ProductCategory/Product`、`Department/Employee`、`Area`，并提供最小种子数据，降低“首次使用即报错”概率。
- [i] 轮询迭代要求：每次迭代必须做到“单一主题变更 + 构建校验 + commit + push”。

## 3. 执行清单 (Execution)
- [x] 第一轮：克隆远程仓库并解包源码到工作区
- [x] 第一轮：修复 `dotnet build` 构建失败（MSB4216/MSB4028）
- [x] 第一轮：清理仓库结构（移除 zip、添加 `.gitignore`、补文档/CI）
- [x] 第一轮：补齐 DB 初始化/权限表/工厂表创建逻辑（避免首启崩溃）
- [x] 第一轮：修复关键逻辑缺陷（ResetPassword 二次加密）
- [x] 第一轮：全量构建校验后推送到远程
- [x] 第一轮：安全删除本地克隆目录

- [x] 第二轮：10 次原子级轮询迭代（iter11-iter20）逐次推送
- [x] 第二轮：完成后安全删除本地克隆目录
