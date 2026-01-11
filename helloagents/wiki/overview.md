# MDMUI

> 本文件包含项目级别的核心信息。详细的模块文档见 `modules/` 目录。

---

## 1. 项目概述

### 目标与背景
MDMUI 是一个基于 WinForms 的制造管理端示例项目，强调“本地可运行 + 现代 UI + 清晰分层”。

### 范围
- **范围内:** 登录与权限、基础资料管理、工艺管理、日志、系统参数、数据备份
- **范围外:** 对外 HTTP API、云端多租户、移动端

### 干系人
- **负责人:** 内部管理员/项目维护者

---

## 2. 模块索引

| 模块名称 | 职责 | 状态 | 文档 |
|---------|------|------|------|
| Forms | WinForms UI 与交互 | ✅稳定 | [forms](modules/forms.md) |
| BLL | 业务逻辑与策略 | ✅稳定 | [bll](modules/bll.md) |
| DAL | 数据访问与持久化 | ✅稳定 | [dal](modules/dal.md) |
| Model | 领域模型与 DTO | ✅稳定 | [model](modules/model.md) |
| Utility | 主题、配置、安全、工具 | ✅稳定 | [utility](modules/utility.md) |  
| Controls | 自定义控件与视觉组件 | ✅稳定 | [controls](modules/controls.md) |
| Scripts | 构建/清理脚本 | ✅稳定 | [scripts](modules/scripts.md) |

---

## 3. 快速链接
- [技术约定](../project.md)
- [架构设计](arch.md)
- [API 手册](api.md)
- [数据模型](data.md)
- [变更历史](../history/index.md)
