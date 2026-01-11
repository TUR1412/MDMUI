# Forms

## 目的
承载 WinForms UI 与交互逻辑，连接业务层与用户操作。

## 模块概述
- **职责:** 登录、主界面、业务功能页、系统参数与备份界面
- **状态:** ✅稳定
- **最后更新:** 2026-01-12

## 规范

### 需求: 登录安全与提示
**模块:** Forms
登录页展示锁定提示与剩余尝试次数，避免误操作反复触发。

#### 场景: 登录失败/锁定
- 用户连续失败达到阈值
- 展示锁定至时间与剩余次数

### 需求: 系统参数管理
**模块:** Forms
系统参数页面支持查看与修改关键参数。

#### 场景: 参数编辑
- 用户修改参数值并保存
- 成功后刷新更新时间
- 支持筛选与未保存变更提示

### 需求: 数据备份
**模块:** Forms
备份页面支持选择路径并执行备份。

#### 场景: 一键备份
- 选择目录
- 执行备份并展示结果
- 支持双击打开备份文件

### 需求: 未处理异常兜底
**模块:** Forms
未处理异常统一弹出崩溃报告窗，支持复制详情与打开日志目录，便于定位问题。        
系统设置菜单提供“打开日志目录”快捷入口，便于日常排障。

### 需求: 操作日志查询体验
**模块:** Forms
操作日志页面支持按时间/用户/模块/类型筛选查询，并以非打扰方式呈现空态与状态信息。

#### 场景: 条件查询
- 默认最近 30 天
- Enter 快速查询
- 空态提示替代弹窗打断

### 需求: 用户密码重置更健壮
**模块:** Forms
用户管理页面在执行“重置密码”时，优先使用配置默认值；若不满足当前密码策略，则自动生成强密码并复制到剪贴板，降低失败率与沟通成本。

#### 场景: 重置密码
- 优先读取 `Security.DefaultResetPassword`（系统参数表或 App.config）
- 默认值不满足策略时自动回退为生成强密码（12/16/24/32）
- 成功后提示新密码并尝试复制到剪贴板

### 需求: 用户管理列表体验升级
**模块:** Forms
用户管理页面以运行时组合方式引入 Atomic Design（CardPanel + ActionToolbar + AppButton），并提供空态/状态栏信息，减少弹窗打断（遵循开闭原则，不修改 Designer 结构）。

#### 场景: 列表浏览/搜索
- 操作区与搜索区统一为卡片工具栏布局
- 无数据/无匹配结果时使用空态提示与状态栏文本
- 搜索支持 Enter 与防抖（500ms）

### 需求: 区域管理体验升级
**模块:** Forms
区域管理页面以运行时组合方式引入 Atomic Design（CardPanel + ActionToolbar + AppButton），并提供树空态/状态栏信息，避免无数据时界面空白（遵循开闭原则，不修改 Designer 结构）。

#### 场景: 树浏览/维护
- 工具栏统一为卡片布局（标题 + 操作按钮）
- 无数据时显示空态提示，并在状态栏展示区域数量
- 加载/详情查询补充 AppTelemetry 埋点与 AppLog 诊断日志

### 需求: 工厂管理列表体验升级
**模块:** Forms
工厂管理页面以运行时组合方式引入 Atomic Design（CardPanel + ActionToolbar + AppButton），并提供列表空态/状态栏信息；列表加载路径补充性能埋点与异常日志（遵循开闭原则，不修改 Designer 结构）。

#### 场景: 列表浏览/搜索
- 工具栏与搜索区合并为卡片工具栏（操作区 + 搜索区）
- 无数据/无匹配结果时使用空态提示与状态栏文本
- 加载列表使用 `AppTelemetry.Measure("factory.load_list")` 记录耗时

### 需求: 产品管理列表体验升级
**模块:** Forms
产品管理页面以运行时组合方式引入 Atomic Design（CardPanel + ActionToolbar + AppButton），并提供列表空态/状态栏信息；列表加载与搜索路径补充性能埋点与异常日志（遵循开闭原则，不修改 Designer 结构）。

#### 场景: 列表浏览/搜索
- 工具栏与搜索区统一为卡片工具栏（操作区 + 搜索区）
- 无数据/无匹配结果时使用空态提示与状态栏文本
- 加载列表使用 `AppTelemetry.Measure("product.load_list")` 记录耗时
- 搜索使用 `AppTelemetry.Measure("product.search")` 记录耗时

### 需求: 部门管理列表体验升级
**模块:** Forms
部门管理页面以运行时组合方式引入 Atomic Design（CardPanel + ActionToolbar + AppButton），并提供列表空态/状态栏信息；加载路径补充性能埋点与异常日志（遵循开闭原则，不修改 Designer 结构）。

#### 场景: 列表浏览/搜索
- 工具栏与搜索区统一为卡片工具栏（操作区 + 搜索区）
- 无数据/无匹配结果时使用空态提示与状态栏文本
- 列表字段补齐：上级部门/所属工厂/负责人显示（DAL JOIN + Model 扩展）
- 加载列表使用 `AppTelemetry.Measure("department.load_list")` 记录耗时

### 需求: 产品类别管理体验升级
**模块:** Forms
产品类别管理页面以运行时组合方式引入 Atomic Design（CardPanel + ActionToolbar + AppButton），统一标题与操作区；并提供树/列表空态提示与加载埋点（遵循开闭原则，不修改 Designer 结构）。

#### 场景: 树浏览/子类别列表
- 顶部统一卡片工具栏（标题 + 添加/编辑/删除/刷新）
- 左侧树与右侧列表无数据时提供空态提示，避免界面空白
- 加载树使用 `AppTelemetry.Measure("product_category.load_tree")` 记录耗时
- 加载子类别使用 `AppTelemetry.Measure("product_category.load_subcategories")` 记录耗时

### 需求: 文件日志查看器
**模块:** Forms
提供应用内“诊断日志”的可视化查看能力，支持过滤、复制与外部打开，降低排障门槛。  

#### 场景: 诊断日志排查
- 系统设置 -> 日志查看器
- 查看 mdmui-*.log 并按关键字过滤
- 一键复制当前视图内容

## API接口
- 无对外 API

## 数据模型
- 使用 SystemParameters / SystemLog

## 依赖
- BLL
- Utility

## 变更历史
- 202601112006_mdmui_revamp (history/2026-01/202601112006_mdmui_revamp/)
- 202601120248_mdmui_observability (history/2026-01/202601120248_mdmui_observability/)
- 202601120314_mdmui_diagnostics_entry (history/2026-01/202601120314_mdmui_diagnostics_entry/)
