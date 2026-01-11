/*
 Navicat Premium Dump SQL

 Source Server         : VS 2022
 Source Server Type    : SQL Server
 Source Server Version : 15004382 (15.00.4382)
 Source Host           : np:\\.\pipe\LOCALDB#F3C35A43\tsql\query:1433
 Source Catalog        : UserDB
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 15004382 (15.00.4382)
 File Encoding         : 65001

 Date: 23/05/2025 12:40:05
*/


-- ----------------------------
-- Table structure for Area
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Area]') AND type IN ('U'))
	DROP TABLE [dbo].[Area]
GO

CREATE TABLE [dbo].[Area] (
  [AreaId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [AreaName] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ParentAreaId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [PostalCode] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Area] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Area
-- ----------------------------
INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'BJ', N'北京', N'CN', N'100000', NULL)
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'BJ-CY', N'朝阳区', N'BJ', N'100020', N'北京朝阳区')
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'BJ-HD', N'海淀区', N'BJ', N'100080', N'北京海淀区')
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'CN', N'中国', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'GD', N'广东', N'CN', N'510000', NULL)
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'GD-GZ', N'广州', N'GD', N'510000', N'广东广州')
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'GD-SZ', N'深圳', N'GD', N'518000', N'广东深圳')
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'SH', N'上海', N'CN', N'200000', NULL)
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'SH-JA', N'静安区', N'SH', N'200040', N'上海静安区')
GO

INSERT INTO [dbo].[Area] ([AreaId], [AreaName], [ParentAreaId], [PostalCode], [Remark]) VALUES (N'SH-PD', N'浦东新区', N'SH', N'200120', N'上海浦东新区')
GO


-- ----------------------------
-- Table structure for DataChangeLog
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[DataChangeLog]') AND type IN ('U'))
	DROP TABLE [dbo].[DataChangeLog]
GO

CREATE TABLE [dbo].[DataChangeLog] (
  [LogId] int  IDENTITY(1,1) NOT NULL,
  [TableName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [RecordId] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [FieldName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [OldValue] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [NewValue] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ChangeType] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ChangeTime] datetime DEFAULT getdate() NOT NULL,
  [UserId] int  NOT NULL,
  [UserName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[DataChangeLog] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of DataChangeLog
-- ----------------------------
SET IDENTITY_INSERT [dbo].[DataChangeLog] ON
GO

SET IDENTITY_INSERT [dbo].[DataChangeLog] OFF
GO


-- ----------------------------
-- Table structure for Department
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Department]') AND type IN ('U'))
	DROP TABLE [dbo].[Department]
GO

CREATE TABLE [dbo].[Department] (
  [DeptId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [DeptName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ParentDeptId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Manager] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Description] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  [ManagerEmployeeId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Department] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Department
-- ----------------------------
INSERT INTO [dbo].[Department] ([DeptId], [DeptName], [ParentDeptId], [FactoryId], [Manager], [Description], [CreateTime], [ManagerEmployeeId]) VALUES (N'D001', N'测试部门1', NULL, N'F001', N'张三', N'这是测试部门的描述信息', N'2025-04-22 23:17:16.797', NULL)
GO


-- ----------------------------
-- Table structure for Employee
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND type IN ('U'))
	DROP TABLE [dbo].[Employee]
GO

CREATE TABLE [dbo].[Employee] (
  [EmployeeId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [EmployeeName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Gender] nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [BirthDate] date  NULL,
  [IdCard] varchar(18) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Phone] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Email] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Address] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [DeptId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Position] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [HireDate] date  NOT NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'在职' NOT NULL,
  [UserId] int  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[Employee] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Employee
-- ----------------------------
INSERT INTO [dbo].[Employee] ([EmployeeId], [EmployeeName], [Gender], [BirthDate], [IdCard], [Phone], [Email], [Address], [DeptId], [Position], [HireDate], [Status], [UserId], [CreateTime]) VALUES (N'EMP001', N'张三', N'男', N'1980-01-01', N'110101198001010011', N'13800138001', N'zhang@example.com', N'北京市海淀区', N'D001', N'工程师', N'2010-01-01', N'在职', NULL, N'2025-04-22 23:17:16.800')
GO

INSERT INTO [dbo].[Employee] ([EmployeeId], [EmployeeName], [Gender], [BirthDate], [IdCard], [Phone], [Email], [Address], [DeptId], [Position], [HireDate], [Status], [UserId], [CreateTime]) VALUES (N'EMP002', N'李四', N'男', N'1985-05-05', N'110101198505050022', N'13800138002', N'li@example.com', N'北京市海淀区', N'D001', N'工程师', N'2015-06-01', N'在职', NULL, N'2025-04-22 23:17:16.800')
GO

INSERT INTO [dbo].[Employee] ([EmployeeId], [EmployeeName], [Gender], [BirthDate], [IdCard], [Phone], [Email], [Address], [DeptId], [Position], [HireDate], [Status], [UserId], [CreateTime]) VALUES (N'EMP003', N'王五', N'男', N'1990-10-10', N'110101199010100033', N'13800138003', N'wang@example.com', N'北京市海淀区', N'D001', N'工程师', N'2018-03-15', N'在职', NULL, N'2025-04-22 23:17:16.800')
GO


-- ----------------------------
-- Table structure for eqp_group
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[eqp_group]') AND type IN ('U'))
	DROP TABLE [dbo].[eqp_group]
GO

CREATE TABLE [dbo].[eqp_group] (
  [eqp_group_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [eqp_group_type] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [eqp_group_description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [factory_id] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [event_user] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [event_remark] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [edit_time] datetime  NULL,
  [create_time] datetime DEFAULT getdate() NOT NULL,
  [event_type] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[eqp_group] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of eqp_group
-- ----------------------------
INSERT INTO [dbo].[eqp_group] ([eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'ASSEMBLY_LINE_A', N'组装线', N'A号产品自动组装线', N'F001', N'admin', N'初始创建', N'2025-04-29 11:35:54.457', N'2025-04-29 11:35:54.457', N'Create')
GO

INSERT INTO [dbo].[eqp_group] ([eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'CNC_GROUP_01', N'CNC机床', N'负责精密加工的CNC机床组', N'F001', N'admin', N'初始创建', N'2025-04-29 11:35:54.457', N'2025-04-29 11:35:54.457', N'Create')
GO

INSERT INTO [dbo].[eqp_group] ([eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'TEST_BENCH_05', N'测试台', N'电子产品功能测试台', N'F002', N'admin', N'初始创建', N'2025-04-29 11:35:54.457', N'2025-04-29 11:35:54.457', N'Create')
GO

INSERT INTO [dbo].[eqp_group] ([eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'陈平安', N'陈平安666', N'陈平安666', N'F002', N'admin', N'设备组更新', N'2025-05-06 19:41:54.817', N'2025-05-06 19:41:31.660', N'Update')
GO

INSERT INTO [dbo].[eqp_group] ([eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'神', N'神', N'神', N'F001', N'admin', N'设备组创建', N'2025-05-06 19:30:33.243', N'2025-05-06 19:30:33.250', N'Create')
GO


-- ----------------------------
-- Table structure for eqp_group_his
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[eqp_group_his]') AND type IN ('U'))
	DROP TABLE [dbo].[eqp_group_his]
GO

CREATE TABLE [dbo].[eqp_group_his] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [eqp_group_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [eqp_group_type] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [eqp_group_description] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [factory_id] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [event_user] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [event_remark] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [edit_time] datetime  NULL,
  [create_time] datetime DEFAULT getdate() NOT NULL,
  [event_type] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[eqp_group_his] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of eqp_group_his
-- ----------------------------
SET IDENTITY_INSERT [dbo].[eqp_group_his] ON
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'1', N'111', N'111', N'111', N'F001', N'admin', N'设备组创建', N'2025-04-29 11:48:53.633', N'2025-04-29 11:48:53.663', N'Create')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'2', N'111', N'555', N'111', N'F001', N'admin', N'设备组更新', N'2025-04-29 13:16:28.427', N'2025-04-29 13:16:28.467', N'Update')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'3', N'999', N'999', N'999', N'F002', N'admin', N'设备组创建', N'2025-04-29 13:16:42.377', N'2025-04-29 13:16:42.397', N'Create')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'4', N'999', N'999', N'999', N'F002', N'admin', N'设备组删除', N'2025-04-29 13:19:08.627', N'2025-04-29 13:19:08.633', N'Delete')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'5', N'999', N'999', N'999', N'F001', N'admin', N'设备组创建', N'2025-04-29 13:20:24.383', N'2025-04-29 13:20:24.410', N'Create')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'6', N'999', N'999', N'999', N'F001', N'admin', N'设备组删除', N'2025-04-29 13:21:10.233', N'2025-04-29 13:21:10.237', N'Delete')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'1002', N'111111', N'1111', N'1111', N'F001', N'admin', N'设备组创建', N'2025-04-29 13:57:10.840', N'2025-04-29 13:57:10.873', N'Create')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'2002', N'111', N'555', N'111', N'F001', N'admin', N'设备组删除', N'2025-04-29 14:11:24.957', N'2025-04-29 14:11:24.963', N'Delete')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'3002', N'111111', N'1111', N'122', N'F001', N'admin', N'设备组更新', N'2025-05-06 18:48:55.830', N'2025-05-06 18:48:55.870', N'Update')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'4002', N'1', N'1', N'11', N'F002', N'admin', N'设备组创建', N'2025-05-06 19:30:22.137', N'2025-05-06 19:30:22.177', N'Create')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'4003', N'神', N'神', N'神', N'F001', N'admin', N'设备组创建', N'2025-05-06 19:30:33.243', N'2025-05-06 19:30:33.260', N'Create')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'4004', N'1', N'1', N'11', N'F002', N'admin', N'设备组删除', N'2025-05-06 19:31:00.797', N'2025-05-06 19:31:00.803', N'Delete')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'4005', N'111111', N'999', N'122', N'F001', N'admin', N'设备组更新', N'2025-05-06 19:31:08.193', N'2025-05-06 19:31:08.220', N'Update')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'5002', N'陈平安', N'陈平安', N'陈平安', N'F001', N'admin', N'设备组创建', N'2025-05-06 19:41:31.643', N'2025-05-06 19:41:31.670', N'Create')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'5003', N'陈平安', N'陈平安666', N'陈平安666', N'F002', N'admin', N'设备组更新', N'2025-05-06 19:41:54.817', N'2025-05-06 19:41:54.840', N'Update')
GO

INSERT INTO [dbo].[eqp_group_his] ([id], [eqp_group_id], [eqp_group_type], [eqp_group_description], [factory_id], [event_user], [event_remark], [edit_time], [create_time], [event_type]) VALUES (N'5004', N'111111', N'999', N'122', N'F001', N'admin', N'设备组删除', N'2025-05-06 19:42:04.100', N'2025-05-06 19:42:04.110', N'Delete')
GO

SET IDENTITY_INSERT [dbo].[eqp_group_his] OFF
GO


-- ----------------------------
-- Table structure for Equipment
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Equipment]') AND type IN ('U'))
	DROP TABLE [dbo].[Equipment]
GO

CREATE TABLE [dbo].[Equipment] (
  [EquipmentId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [EquipmentName] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [CategoryId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Model] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Manufacturer] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [PurchaseDate] date  NULL,
  [PurchasePrice] decimal(18,2)  NULL,
  [Status] nvarchar(20) COLLATE Chinese_PRC_CI_AS DEFAULT N'正常' NOT NULL,
  [Location] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ResponsiblePerson] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [MaintenanceCycle] int  NULL,
  [LastMaintenanceDate] date  NULL,
  [NextMaintenanceDate] date  NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  [eqp_group_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [sub_device_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [port_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Equipment] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Equipment
-- ----------------------------
INSERT INTO [dbo].[Equipment] ([EquipmentId], [EquipmentName], [CategoryId], [FactoryId], [Model], [Manufacturer], [PurchaseDate], [PurchasePrice], [Status], [Location], [ResponsiblePerson], [MaintenanceCycle], [LastMaintenanceDate], [NextMaintenanceDate], [Description], [CreateTime], [eqp_group_id], [sub_device_id], [port_id]) VALUES (N'EQ001', N'设备A机', N'EC001', N'F001', N'CNC-A100', N'北京机械厂', N'2020-01-15', N'150000.00', N'正常', N'1号车间', N'EMP001', N'90', N'2023-04-15', N'2023-07-14', N'高精度加工设备，自动化程度高', N'2025-04-22 23:17:16.800', N'ASSEMBLY_LINE_A', N'SDEQ001-1', NULL)
GO

INSERT INTO [dbo].[Equipment] ([EquipmentId], [EquipmentName], [CategoryId], [FactoryId], [Model], [Manufacturer], [PurchaseDate], [PurchasePrice], [Status], [Location], [ResponsiblePerson], [MaintenanceCycle], [LastMaintenanceDate], [NextMaintenanceDate], [Description], [CreateTime], [eqp_group_id], [sub_device_id], [port_id]) VALUES (N'EQ002', N'设备B机', N'EC001', N'F001', N'XNC-B200', N'北京机械厂', N'2020-02-20', N'180000.00', N'正常', N'1号车间', N'EMP001', N'90', N'2023-05-20', N'2023-08-18', N'高精度加工设备，自动化程度高', N'2025-04-22 23:17:16.800', N'ASSEMBLY_LINE_A', NULL, NULL)
GO

INSERT INTO [dbo].[Equipment] ([EquipmentId], [EquipmentName], [CategoryId], [FactoryId], [Model], [Manufacturer], [PurchaseDate], [PurchasePrice], [Status], [Location], [ResponsiblePerson], [MaintenanceCycle], [LastMaintenanceDate], [NextMaintenanceDate], [Description], [CreateTime], [eqp_group_id], [sub_device_id], [port_id]) VALUES (N'EQ003', N'检测C机', N'EC003', N'F001', N'JCY-C300', N'南京仪器厂', N'2021-03-10', N'80000.00', N'正常', N'2号车间', N'EMP002', N'180', N'2023-03-10', N'2023-09-06', N'高精度自动化设备', N'2025-04-22 23:17:16.800', N'CNC_GROUP_01', NULL, NULL)
GO

INSERT INTO [dbo].[Equipment] ([EquipmentId], [EquipmentName], [CategoryId], [FactoryId], [Model], [Manufacturer], [PurchaseDate], [PurchasePrice], [Status], [Location], [ResponsiblePerson], [MaintenanceCycle], [LastMaintenanceDate], [NextMaintenanceDate], [Description], [CreateTime], [eqp_group_id], [sub_device_id], [port_id]) VALUES (N'EQ004', N'组装D机', N'EC002', N'F001', N'HJJ-D400', N'上海工具厂', N'2021-04-05', N'60000.00', N'维修中', N'2号车间', N'EMP002', N'60', N'2023-06-01', N'2023-07-31', N'高精度自动化设备', N'2025-04-22 23:17:16.800', N'CNC_GROUP_01', NULL, NULL)
GO

INSERT INTO [dbo].[Equipment] ([EquipmentId], [EquipmentName], [CategoryId], [FactoryId], [Model], [Manufacturer], [PurchaseDate], [PurchasePrice], [Status], [Location], [ResponsiblePerson], [MaintenanceCycle], [LastMaintenanceDate], [NextMaintenanceDate], [Description], [CreateTime], [eqp_group_id], [sub_device_id], [port_id]) VALUES (N'EQ005', N'包装E机', N'EC002', N'F001', N'ZZT-E500', N'广州机械厂', N'2022-05-20', N'30000.00', N'正常', N'3号车间', N'EMP003', N'365', NULL, NULL, N'高精度自动化设备', N'2025-04-22 23:17:16.800', N'TEST_BENCH_05', NULL, NULL)
GO


-- ----------------------------
-- Table structure for EquipmentCategory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[EquipmentCategory]') AND type IN ('U'))
	DROP TABLE [dbo].[EquipmentCategory]
GO

CREATE TABLE [dbo].[EquipmentCategory] (
  [CategoryId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [CategoryName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Description] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[EquipmentCategory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of EquipmentCategory
-- ----------------------------
INSERT INTO [dbo].[EquipmentCategory] ([CategoryId], [CategoryName], [Description], [CreateTime]) VALUES (N'EC001', N'加工设备', N'用于产品加工的各类设备', N'2025-04-22 12:40:54.743')
GO

INSERT INTO [dbo].[EquipmentCategory] ([CategoryId], [CategoryName], [Description], [CreateTime]) VALUES (N'EC002', N'包装设备', N'用于产品包装的各类设备', N'2025-04-22 12:40:54.743')
GO

INSERT INTO [dbo].[EquipmentCategory] ([CategoryId], [CategoryName], [Description], [CreateTime]) VALUES (N'EC003', N'检测设备', N'用于产品检测', N'2025-04-22 12:40:54.743')
GO


-- ----------------------------
-- Table structure for EquipmentMaintenance
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[EquipmentMaintenance]') AND type IN ('U'))
	DROP TABLE [dbo].[EquipmentMaintenance]
GO

CREATE TABLE [dbo].[EquipmentMaintenance] (
  [MaintenanceId] int  IDENTITY(1,1) NOT NULL,
  [EquipmentId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [MaintenanceDate] date  NOT NULL,
  [MaintenanceType] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [MaintenancePerson] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Cost] decimal(18,2)  NULL,
  [Result] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'正常' NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[EquipmentMaintenance] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of EquipmentMaintenance
-- ----------------------------
SET IDENTITY_INSERT [dbo].[EquipmentMaintenance] ON
GO

INSERT INTO [dbo].[EquipmentMaintenance] ([MaintenanceId], [EquipmentId], [MaintenanceDate], [MaintenanceType], [MaintenancePerson], [Description], [Cost], [Result], [CreateTime]) VALUES (N'1', N'EQ001', N'2023-04-15', N'定期维护', N'EMP002', N'设备标准维护与保养', N'1200.00', N'正常', N'2025-04-22 23:17:16.800')
GO

INSERT INTO [dbo].[EquipmentMaintenance] ([MaintenanceId], [EquipmentId], [MaintenanceDate], [MaintenanceType], [MaintenancePerson], [Description], [Cost], [Result], [CreateTime]) VALUES (N'2', N'EQ002', N'2023-05-20', N'定期维护', N'EMP002', N'设备标准维护与保养', N'1500.00', N'正常', N'2025-04-22 23:17:16.800')
GO

INSERT INTO [dbo].[EquipmentMaintenance] ([MaintenanceId], [EquipmentId], [MaintenanceDate], [MaintenanceType], [MaintenancePerson], [Description], [Cost], [Result], [CreateTime]) VALUES (N'3', N'EQ003', N'2023-03-10', N'定期维护', N'EMP003', N'设备标准维护与保养', N'800.00', N'正常', N'2025-04-22 23:17:16.800')
GO

INSERT INTO [dbo].[EquipmentMaintenance] ([MaintenanceId], [EquipmentId], [MaintenanceDate], [MaintenanceType], [MaintenancePerson], [Description], [Cost], [Result], [CreateTime]) VALUES (N'4', N'EQ004', N'2023-06-01', N'定期维护', N'EMP002', N'设备标准维护与保养', N'3500.00', N'正常', N'2025-04-22 23:17:16.800')
GO

INSERT INTO [dbo].[EquipmentMaintenance] ([MaintenanceId], [EquipmentId], [MaintenanceDate], [MaintenanceType], [MaintenancePerson], [Description], [Cost], [Result], [CreateTime]) VALUES (N'5', N'EQ005', N'2025-04-23', N'1111', N'EMP003', N'111', N'1111.00', N'111', N'2025-04-23 00:18:46.127')
GO

SET IDENTITY_INSERT [dbo].[EquipmentMaintenance] OFF
GO


-- ----------------------------
-- Table structure for Factory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Factory]') AND type IN ('U'))
	DROP TABLE [dbo].[Factory]
GO

CREATE TABLE [dbo].[Factory] (
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [FactoryName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Address] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Manager] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Phone] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ManagerEmployeeId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Factory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Factory
-- ----------------------------
INSERT INTO [dbo].[Factory] ([FactoryId], [FactoryName], [Address], [Manager], [Phone], [ManagerEmployeeId]) VALUES (N'F001', N'第一机械厂', N'北京市海淀区', N'张三', N'13800138000', NULL)
GO

INSERT INTO [dbo].[Factory] ([FactoryId], [FactoryName], [Address], [Manager], [Phone], [ManagerEmployeeId]) VALUES (N'F002', N'第二电子厂', N'上海市浦东新区', N'李四', N'13900139000', NULL)
GO


-- ----------------------------
-- Table structure for FactoryExtendInfo
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[FactoryExtendInfo]') AND type IN ('U'))
	DROP TABLE [dbo].[FactoryExtendInfo]
GO

CREATE TABLE [dbo].[FactoryExtendInfo] (
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [EstablishDate] date  NULL,
  [ProductionType] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Scale] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Area] decimal(18,2)  NULL,
  [EmployeeCount] int  NULL,
  [AnnualOutput] decimal(18,2)  NULL,
  [AnnualRevenue] decimal(18,2)  NULL,
  [TaxID] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Website] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [UpdateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[FactoryExtendInfo] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of FactoryExtendInfo
-- ----------------------------
INSERT INTO [dbo].[FactoryExtendInfo] ([FactoryId], [EstablishDate], [ProductionType], [Scale], [Area], [EmployeeCount], [AnnualOutput], [AnnualRevenue], [TaxID], [Website], [UpdateTime]) VALUES (N'F001', N'2010-01-01', N'电子产品制造', N'大型', N'50000.00', N'1200', NULL, NULL, NULL, NULL, N'2025-04-22 12:40:54.743')
GO

INSERT INTO [dbo].[FactoryExtendInfo] ([FactoryId], [EstablishDate], [ProductionType], [Scale], [Area], [EmployeeCount], [AnnualOutput], [AnnualRevenue], [TaxID], [Website], [UpdateTime]) VALUES (N'F002', N'2015-06-15', N'电子产品制造', N'大型', N'20000.00', N'650', NULL, NULL, NULL, NULL, N'2025-04-22 12:40:54.743')
GO


-- ----------------------------
-- Table structure for FactoryMenuPermissions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[FactoryMenuPermissions]') AND type IN ('U'))
	DROP TABLE [dbo].[FactoryMenuPermissions]
GO

CREATE TABLE [dbo].[FactoryMenuPermissions] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [MenuId] int  NOT NULL
)
GO

ALTER TABLE [dbo].[FactoryMenuPermissions] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of FactoryMenuPermissions
-- ----------------------------
SET IDENTITY_INSERT [dbo].[FactoryMenuPermissions] ON
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'1', N'F001', N'4')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'2', N'F001', N'5')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'3', N'F001', N'6')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'4', N'F001', N'7')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'5', N'F001', N'8')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'6', N'F001', N'9')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'7', N'F001', N'10')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'8', N'F002', N'4')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'9', N'F002', N'5')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'10', N'F002', N'8')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'11', N'F002', N'9')
GO

INSERT INTO [dbo].[FactoryMenuPermissions] ([Id], [FactoryId], [MenuId]) VALUES (N'12', N'F002', N'10')
GO

SET IDENTITY_INSERT [dbo].[FactoryMenuPermissions] OFF
GO


-- ----------------------------
-- Table structure for Inventory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Inventory]') AND type IN ('U'))
	DROP TABLE [dbo].[Inventory]
GO

CREATE TABLE [dbo].[Inventory] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [ProductId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Quantity] decimal(18,2) DEFAULT 0 NOT NULL,
  [SafetyStock] decimal(18,2) DEFAULT 0 NULL,
  [Location] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [LastUpdateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[Inventory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Inventory
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Inventory] ON
GO

SET IDENTITY_INSERT [dbo].[Inventory] OFF
GO


-- ----------------------------
-- Table structure for Menu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Menu]') AND type IN ('U'))
	DROP TABLE [dbo].[Menu]
GO

CREATE TABLE [dbo].[Menu] (
  [MenuId] int  IDENTITY(1,1) NOT NULL,
  [MenuName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ParentMenuId] int  NULL,
  [MenuOrder] int DEFAULT 0 NULL,
  [MenuIcon] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Menu] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Menu
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Menu] ON
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'1', N'工厂', NULL, N'1', N'factory')
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'2', N'管理员', NULL, N'2', N'admin')
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'3', N'系统', NULL, N'3', N'system')
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'4', N'工厂管理', N'1', N'1', NULL)
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'5', N'生产地信息', N'1', N'2', NULL)
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'6', N'用户管理', N'2', N'1', NULL)
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'7', N'权限设置', N'2', N'2', NULL)
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'8', N'修改密码', N'3', N'1', NULL)
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'9', N'注销', N'3', N'2', NULL)
GO

INSERT INTO [dbo].[Menu] ([MenuId], [MenuName], [ParentMenuId], [MenuOrder], [MenuIcon]) VALUES (N'10', N'退出', N'3', N'3', NULL)
GO

SET IDENTITY_INSERT [dbo].[Menu] OFF
GO


-- ----------------------------
-- Table structure for MenuPermission
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[MenuPermission]') AND type IN ('U'))
	DROP TABLE [dbo].[MenuPermission]
GO

CREATE TABLE [dbo].[MenuPermission] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [MenuId] int  NOT NULL,
  [PermissionId] int  NOT NULL
)
GO

ALTER TABLE [dbo].[MenuPermission] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of MenuPermission
-- ----------------------------
SET IDENTITY_INSERT [dbo].[MenuPermission] ON
GO

INSERT INTO [dbo].[MenuPermission] ([Id], [MenuId], [PermissionId]) VALUES (N'1', N'4', N'1')
GO

INSERT INTO [dbo].[MenuPermission] ([Id], [MenuId], [PermissionId]) VALUES (N'2', N'5', N'12')
GO

INSERT INTO [dbo].[MenuPermission] ([Id], [MenuId], [PermissionId]) VALUES (N'3', N'6', N'7')
GO

INSERT INTO [dbo].[MenuPermission] ([Id], [MenuId], [PermissionId]) VALUES (N'4', N'7', N'17')
GO

SET IDENTITY_INSERT [dbo].[MenuPermission] OFF
GO


-- ----------------------------
-- Table structure for Permissions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type IN ('U'))
	DROP TABLE [dbo].[Permissions]
GO

CREATE TABLE [dbo].[Permissions] (
  [PermissionId] int  IDENTITY(1,1) NOT NULL,
  [ModuleName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ActionName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Permissions] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Permissions
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Permissions] ON
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'1', N'factory', N'view', N'查看工厂信息')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'2', N'factory', N'add', N'添加工厂')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'3', N'factory', N'edit', N'编辑工厂')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'4', N'factory', N'delete', N'删除工厂')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'5', N'factory', N'print', N'打印工厂信息')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'6', N'factory', N'export', N'导出工厂数据')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'7', N'user', N'view', N'查看用户信息')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'8', N'user', N'add', N'添加用户')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'9', N'user', N'edit', N'编辑用户')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'10', N'user', N'delete', N'删除用户')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'11', N'user', N'reset_pwd', N'重置用户密码')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'12', N'area', N'view', N'查看生产地信息')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'13', N'area', N'add', N'添加生产地')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'14', N'area', N'edit', N'编辑生产地')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'15', N'area', N'delete', N'删除生产地')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'16', N'area', N'print', N'打印生产地信息')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'17', N'system', N'view', N'查看系统设置')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'18', N'system', N'backup', N'备份系统')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'19', N'system', N'restore', N'恢复系统')
GO

INSERT INTO [dbo].[Permissions] ([PermissionId], [ModuleName], [ActionName], [Description]) VALUES (N'20', N'system', N'log', N'查看系统日志')
GO

SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO


-- ----------------------------
-- Table structure for port
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[port]') AND type IN ('U'))
	DROP TABLE [dbo].[port]
GO

CREATE TABLE [dbo].[port] (
  [port_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [port_name] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [port_type] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [port_number] int  NULL,
  [protocol] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [parent_device_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [create_time] datetime DEFAULT getdate() NOT NULL,
  [create_user] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [edit_time] datetime  NULL,
  [edit_user] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[port] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of port
-- ----------------------------
INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P001', N'串口通讯接口', N'RS232', N'1', N'RS232', N'SD001', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P002', N'电源输入端', N'AC 220V', NULL, N'AC220V', N'SD001', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P003', N'以太网接口', N'RJ45', N'1', N'TCP/IP', N'SD001', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P004', N'CAN总线接口', N'CAN Bus', N'1', N'CAN', N'SD002', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P005', N'USB调试口', N'USB Type-A', N'2', N'USB', N'SD003', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P006', N'Modbus通讯口', N'Modbus RTU', N'3', N'Modbus', N'SD003', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P007', N'模拟量输入', N'4-20mA', N'1', N'4-20mA', N'SD004', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'P008', N'高速USB接口', N'USB 3.0', N'1', N'USB', N'SD005', N'2025-05-13 11:47:15.010', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'PEQA-1', N'控制板A通信口', N'COM', N'1', N'RS232', N'SDEQ001-1', N'2025-05-13 15:22:20.183', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[port] ([port_id], [port_name], [port_type], [port_number], [protocol], [parent_device_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'PEQA-2', N'控制板A网络口', N'RJ45', N'1', N'TCP/IP', N'SDEQ001-1', N'2025-05-13 15:22:20.183', N'admin', NULL, NULL)
GO


-- ----------------------------
-- Table structure for Process
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Process]') AND type IN ('U'))
	DROP TABLE [dbo].[Process]
GO

CREATE TABLE [dbo].[Process] (
  [ProcessId] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Version] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [PackageId] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ProductionType] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Sequence] int DEFAULT 0 NOT NULL,
  [IsCurrentlyUsed] bit DEFAULT 1 NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  [Status] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Active' NOT NULL
)
GO

ALTER TABLE [dbo].[Process] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Process
-- ----------------------------
INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250001', N'V1.0', N'PKG20250001', N'华为Mate60 Pro装配流程', N'??', N'1', N'1', N'2025-05-10 10:30:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250002', N'V1.0', N'PKG20250001', N'华为Mate60 Pro测试流程', N'??', N'2', N'1', N'2025-05-10 11:00:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250003', N'V1.0', N'PKG20250001', N'华为Mate60 Pro组装流程', N'??', N'3', N'1', N'2025-05-10 11:30:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250004', N'V1.0', N'PKG20250001', N'华为Mate60 Pro打包流程', N'??', N'4', N'1', N'2025-05-10 12:00:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250005', N'V1.0', N'PKG20250001', N'华为Mate60 Pro入库', N'??', N'5', N'1', N'2025-05-10 12:30:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250006', N'V1.0', N'PKG20250002', N'iPad Pro装配流程', N'??', N'1', N'1', N'2025-05-11 10:30:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250007', N'V1.0', N'PKG20250002', N'iPad Pro测试流程', N'??', N'2', N'1', N'2025-05-11 11:00:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250008', N'V1.0', N'PKG20250003', N'ThinkPad X1装配流程', N'??', N'1', N'1', N'2025-05-12 10:30:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250009', N'V1.0', N'PKG20250003', N'ThinkPad X1测试流程', N'??', N'2', N'1', N'2025-05-12 11:00:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250010', N'V2.0', N'PKG20250004', N'华为Mate60 Pro增强版流程', N'????', N'1', N'1', N'2025-05-15 10:30:00.000', N'Active')
GO

INSERT INTO [dbo].[Process] ([ProcessId], [Version], [PackageId], [Description], [ProductionType], [Sequence], [IsCurrentlyUsed], [CreateTime], [Status]) VALUES (N'PRC20250011', N'V1.0', N'PKG20250005', N'OPPO Find X7 Ultra装配流程', N'??', N'1', N'1', N'2025-05-16 10:30:00.000', N'Active')
GO


-- ----------------------------
-- Table structure for ProcessPackage
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ProcessPackage]') AND type IN ('U'))
	DROP TABLE [dbo].[ProcessPackage]
GO

CREATE TABLE [dbo].[ProcessPackage] (
  [PackageId] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Version] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ProductId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  [Status] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Active' NOT NULL
)
GO

ALTER TABLE [dbo].[ProcessPackage] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ProcessPackage
-- ----------------------------
INSERT INTO [dbo].[ProcessPackage] ([PackageId], [Version], [Description], [ProductId], [CreateTime], [Status]) VALUES (N'PKG20250001', N'V1.0', N'华为Mate60 Pro工艺包', N'P000001', N'2025-05-10 10:00:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessPackage] ([PackageId], [Version], [Description], [ProductId], [CreateTime], [Status]) VALUES (N'PKG20250002', N'V1.0', N'iPad Pro工艺包', N'P000002', N'2025-05-11 10:00:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessPackage] ([PackageId], [Version], [Description], [ProductId], [CreateTime], [Status]) VALUES (N'PKG20250003', N'V1.0', N'ThinkPad X1工艺包', N'P000003', N'2025-05-12 10:00:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessPackage] ([PackageId], [Version], [Description], [ProductId], [CreateTime], [Status]) VALUES (N'PKG20250004', N'V2.0', N'华为Mate60 Pro增强版工艺包', N'P000001', N'2025-05-15 10:00:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessPackage] ([PackageId], [Version], [Description], [ProductId], [CreateTime], [Status]) VALUES (N'PKG20250005', N'V1.0', N'OPPO Find X7 Ultra工艺包', N'P000009', N'2025-05-16 10:00:00.000', N'Active')
GO


-- ----------------------------
-- Table structure for ProcessRoute
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ProcessRoute]') AND type IN ('U'))
	DROP TABLE [dbo].[ProcessRoute]
GO

CREATE TABLE [dbo].[ProcessRoute] (
  [RouteId] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [StationId] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Version] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProcessId] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Sequence] int DEFAULT 0 NOT NULL,
  [StationType] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL,
  [Status] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Active' NOT NULL
)
GO

ALTER TABLE [dbo].[ProcessRoute] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ProcessRoute
-- ----------------------------
INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250001', N'ST001', N'V1.0', N'PRC20250001', N'零件准备与清点', N'1', N'???', N'2025-05-10 10:35:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250002', N'ST002', N'V1.0', N'PRC20250001', N'主板安装', N'2', N'???', N'2025-05-10 10:40:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250003', N'ST003', N'V1.0', N'PRC20250001', N'屏幕组装', N'3', N'???', N'2025-05-10 10:45:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250004', N'ST004', N'V1.0', N'PRC20250002', N'电池与后盖安装', N'1', N'???', N'2025-05-10 11:05:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250005', N'ST005', N'V1.0', N'PRC20250002', N'功能测试', N'2', N'???', N'2025-05-10 11:10:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250006', N'ST006', N'V1.0', N'PRC20250002', N'性能测试', N'3', N'???', N'2025-05-10 11:15:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250007', N'ST007', N'V1.0', N'PRC20250003', N'防水测试', N'1', N'???', N'2025-05-10 11:35:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250008', N'ST008', N'V1.0', N'PRC20250003', N'信号与网络测试', N'2', N'???', N'2025-05-10 11:40:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250009', N'ST009', N'V1.0', N'PRC20250004', N'包装准备', N'1', N'???', N'2025-05-10 12:05:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250010', N'ST010', N'V1.0', N'PRC20250004', N'包装完成', N'2', N'???', N'2025-05-10 12:10:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250011', N'ST011', N'V1.0', N'PRC20250005', N'质检扫描', N'1', N'???', N'2025-05-10 12:35:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250012', N'ST012', N'V1.0', N'PRC20250005', N'入库扫描', N'2', N'???', N'2025-05-10 12:40:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250013', N'ST013', N'V1.0', N'PRC20250005', N'库位分配', N'3', N'???', N'2025-05-10 12:45:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250014', N'ST014', N'V1.0', N'PRC20250006', N'iPad零件准备与清点', N'1', N'???', N'2025-05-11 10:35:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250015', N'ST015', N'V1.0', N'PRC20250006', N'iPad主板安装', N'2', N'???', N'2025-05-11 10:40:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250016', N'ST016', N'V1.0', N'PRC20250007', N'iPad屏幕与电池安装', N'1', N'???', N'2025-05-11 11:05:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250017', N'ST017', N'V1.0', N'PRC20250007', N'iPad功能测试', N'2', N'???', N'2025-05-11 11:10:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250018', N'ST018', N'V1.0', N'PRC20250008', N'ThinkPad零件准备与清点', N'1', N'???', N'2025-05-12 10:35:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250019', N'ST019', N'V1.0', N'PRC20250008', N'ThinkPad主板安装', N'2', N'???', N'2025-05-12 10:40:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250020', N'ST020', N'V1.0', N'PRC20250009', N'ThinkPad功能测试', N'1', N'???', N'2025-05-12 11:05:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250021', N'ST021', N'V2.0', N'PRC20250010', N'人工智能辅助检测', N'1', N'????', N'2025-05-15 10:35:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250022', N'ST022', N'V2.0', N'PRC20250010', N'增强信号测试', N'2', N'???', N'2025-05-15 10:40:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250023', N'ST023', N'V2.0', N'PRC20250010', N'卫星通信测试', N'3', N'???', N'2025-05-15 10:45:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250024', N'ST024', N'V1.0', N'PRC20250011', N'OPPO零件准备与清点', N'1', N'???', N'2025-05-16 10:35:00.000', N'Active')
GO

INSERT INTO [dbo].[ProcessRoute] ([RouteId], [StationId], [Version], [ProcessId], [Description], [Sequence], [StationType], [CreateTime], [Status]) VALUES (N'RT20250025', N'ST025', N'V1.0', N'PRC20250011', N'OPPO主板安装', N'2', N'???', N'2025-05-16 10:40:00.000', N'Active')
GO


-- ----------------------------
-- Table structure for Product
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type IN ('U'))
	DROP TABLE [dbo].[Product]
GO

CREATE TABLE [dbo].[Product] (
  [ProductId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProductName] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [CategoryId] varchar(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Specification] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Unit] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Price] decimal(18,2)  NULL,
  [Cost] decimal(18,2)  NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'正常' NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[Product] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Product
-- ----------------------------
INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000001', N'Huawei Mate60 Pro', N'C510B369-3AF0-48C3-88CA-F69BAD48A2B1', N'8GB+512GB', N'台', N'6999.00', NULL, N'华为旗舰5G手机，配备麒麟9000处理器', N'正常', N'2025-05-19 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000002', N'iPad Pro', N'3972A60F-FD2C-4019-B463-4A6C4D403BA1', N'11英寸 M2芯片', N'台', N'6799.00', NULL, N'Apple专业平板，配备M2芯片，支持Apple Pencil', N'正常', N'2025-05-18 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000003', N'ThinkPad X1', N'248FE9E6-CF70-4CA2-9300-6BCFAB26DAB8', N'14英寸 i7-1360P', N'台', N'11999.00', NULL, N'联想商务笔记本，轻薄坚固', N'正常', N'2025-05-17 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000004', N'iPhone 14 Ultra', N'C510B369-3AF0-48C3-88CA-F69BAD48A2B1', N'12GB+512GB', N'台', N'6499.00', NULL, N'苹果高端旗舰手机，支持全天候显示', N'热销', N'2025-05-16 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000005', N'Galaxy Tab S9', N'3972A60F-FD2C-4019-B463-4A6C4D403BA1', N'12.4英寸 AMOLED', N'台', N'7699.00', NULL, N'三星高端平板，配备S Pen', N'正常', N'2025-05-15 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000006', N'格力空调', N'5C840130-1DEE-4365-B416-7219695EAA5E', N'2匹 KFR-50GW', N'台', N'4599.00', NULL, N'节能静音变频空调', N'正常', N'2025-05-14 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000007', N'XPS 15', N'248FE9E6-CF70-4CA2-9300-6BCFAB26DAB8', N'15.6?? i9-13900H', N'台', N'15999.00', NULL, N'戴尔轻薄商务本，高性能显卡', N'限量', N'2025-05-13 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000008', N'ROG Gaming', N'248FE9E6-CF70-4CA2-9300-6BCFAB26DAB8', N'17.3?? RTX4090', N'台', N'25999.00', NULL, N'华硕电竞笔记本，高刷新率屏幕', N'限量', N'2025-05-12 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000009', N'OPPO Find X7 Ultra', N'C510B369-3AF0-48C3-88CA-F69BAD48A2B1', N'16GB+512GB', N'台', N'6299.00', NULL, N'OPPO高端旗舰，哈苏影像', N'正常', N'2025-05-11 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000010', N'海尔冰箱', N'D541059D-7683-403C-A4CE-1A8F2BA6D63D', N'452升 三门', N'台', N'4599.00', NULL, N'智能家电，节能控温', N'正常', N'2025-05-10 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000011', N'Redmi X100 Pro', N'C510B369-3AF0-48C3-88CA-F69BAD48A2B1', N'12GB+512GB', N'台', N'5999.00', NULL, N'小米旗舰手机，徕卡影像', N'热销', N'2025-05-09 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000012', N'美的洗衣机', N'5EFD1DA1-998C-45FD-BF92-45EC50561140', N'10公斤 变频', N'台', N'5499.00', NULL, N'智能变频洗衣机，省水省电', N'促销', N'2025-05-08 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000013', N'Elite DragonFly', N'248FE9E6-CF70-4CA2-9300-6BCFAB26DAB8', N'13.5?? i7-1365U', N'台', N'12999.00', NULL, N'惠普商务轻薄本，超长续航', N'正常', N'2025-05-07 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000014', N'Surface Pro 9', N'3972A60F-FD2C-4019-B463-4A6C4D403BA1', N'13英寸 SQ3芯片', N'台', N'9299.00', NULL, N'微软二合一平板电脑', N'正常', N'2025-05-06 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000015', N'Magic6 RSR', N'C510B369-3AF0-48C3-88CA-F69BAD48A2B1', N'16GB+1TB', N'台', N'8999.00', NULL, N'荣耀旗舰手机，高通骁龙处理器', N'正常', N'2025-05-05 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000016', N'大金空调', N'5C840130-1DEE-4365-B416-7219695EAA5E', N'1.5匹 变频', N'台', N'2899.00', NULL, N'日系高端空调，静音节能', N'促销', N'2025-05-04 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000017', N'MacBook Pro', N'248FE9E6-CF70-4CA2-9300-6BCFAB26DAB8', N'16英寸 M3 Pro', N'台', N'19999.00', NULL, N'Apple专业笔记本，M3芯片', N'限量', N'2025-05-03 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000018', N'S24 Ultra', N'C510B369-3AF0-48C3-88CA-F69BAD48A2B1', N'12GB+1TB', N'台', N'10999.00', NULL, N'三星旗舰手机，Galaxy AI智能', N'热销', N'2025-05-02 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000019', N'小米冰箱', N'D541059D-7683-403C-A4CE-1A8F2BA6D63D', N'630升 风冷无霜', N'台', N'3999.00', NULL, N'小米智能冰箱，支持手机控制', N'正常', N'2025-05-01 09:13:00.000')
GO

INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [CategoryId], [Specification], [Unit], [Price], [Cost], [Description], [Status], [CreateTime]) VALUES (N'P000020', N'LG洗衣机', N'5EFD1DA1-998C-45FD-BF92-45EC50561140', N'12公斤 滚筒', N'台', N'6899.00', NULL, N'LG智能洗衣机，AI识别衣物', N'正常', N'2025-04-30 09:13:00.000')
GO


-- ----------------------------
-- Table structure for ProductCategory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductCategory]') AND type IN ('U'))
	DROP TABLE [dbo].[ProductCategory]
GO

CREATE TABLE [dbo].[ProductCategory] (
  [CategoryId] varchar(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [CategoryName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ParentCategoryId] varchar(36) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Description] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[ProductCategory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ProductCategory
-- ----------------------------
INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'248FE9E6-CF70-4CA2-9300-6BCFAB26DAB8', N'笔记本电脑', N'PC001', N'便携式个人电脑', N'2025-04-25 14:15:36.027')
GO

INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'3972A60F-FD2C-4019-B463-4A6C4D403BA1', N'平板电脑', N'PC001', N'触摸屏移动设备', N'2025-04-25 14:15:36.027')
GO

INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'5C840130-1DEE-4365-B416-7219695EAA5E', N'空调', N'PC002', N'用于调节室内温度', N'2025-04-25 14:15:36.030')
GO

INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'5EFD1DA1-998C-45FD-BF92-45EC50561140', N'洗衣机', N'PC002', N'用于清洗衣物', N'2025-04-25 14:15:36.030')
GO

INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'C510B369-3AF0-48C3-88CA-F69BAD48A2B1', N'手机', N'PC001', N'各种智能手机和功能手机', N'2025-04-25 14:15:36.027')
GO

INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'D541059D-7683-403C-A4CE-1A8F2BA6D63D', N'冰箱', N'PC002', N'用于冷藏和冷冻食物', N'2025-04-25 14:15:36.030')
GO

INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'PC001', N'电子产品', NULL, N'各种消费类电子产品', N'2025-04-22 12:40:54.743')
GO

INSERT INTO [dbo].[ProductCategory] ([CategoryId], [CategoryName], [ParentCategoryId], [Description], [CreateTime]) VALUES (N'PC002', N'家用电器', NULL, N'冰箱、洗衣机、空调等', N'2025-04-22 12:40:54.743')
GO


-- ----------------------------
-- Table structure for ProductionPlan
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductionPlan]') AND type IN ('U'))
	DROP TABLE [dbo].[ProductionPlan]
GO

CREATE TABLE [dbo].[ProductionPlan] (
  [PlanId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [PlanName] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProductId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [PlanQuantity] decimal(18,2)  NOT NULL,
  [StartDate] date  NOT NULL,
  [EndDate] date  NOT NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'计划中' NOT NULL,
  [CompletedQuantity] decimal(18,2) DEFAULT 0 NOT NULL,
  [ResponsiblePerson] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[ProductionPlan] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ProductionPlan
-- ----------------------------

-- ----------------------------
-- Table structure for ProductionRecord
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductionRecord]') AND type IN ('U'))
	DROP TABLE [dbo].[ProductionRecord]
GO

CREATE TABLE [dbo].[ProductionRecord] (
  [RecordId] int  IDENTITY(1,1) NOT NULL,
  [PlanId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProductionDate] date  NOT NULL,
  [Quantity] decimal(18,2)  NOT NULL,
  [Quality] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'合格' NULL,
  [Operator] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Remarks] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[ProductionRecord] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ProductionRecord
-- ----------------------------
SET IDENTITY_INSERT [dbo].[ProductionRecord] ON
GO

SET IDENTITY_INSERT [dbo].[ProductionRecord] OFF
GO


-- ----------------------------
-- Table structure for PurchaseOrder
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchaseOrder]') AND type IN ('U'))
	DROP TABLE [dbo].[PurchaseOrder]
GO

CREATE TABLE [dbo].[PurchaseOrder] (
  [OrderId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [SupplierId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [OrderDate] date  NOT NULL,
  [ExpectedDeliveryDate] date  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'待审核' NOT NULL,
  [TotalAmount] decimal(18,2) DEFAULT 0 NOT NULL,
  [Purchaser] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Approver] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ApprovalDate] datetime  NULL,
  [Remarks] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[PurchaseOrder] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of PurchaseOrder
-- ----------------------------

-- ----------------------------
-- Table structure for PurchaseOrderDetail
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchaseOrderDetail]') AND type IN ('U'))
	DROP TABLE [dbo].[PurchaseOrderDetail]
GO

CREATE TABLE [dbo].[PurchaseOrderDetail] (
  [DetailId] int  IDENTITY(1,1) NOT NULL,
  [OrderId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProductId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Quantity] decimal(18,2)  NOT NULL,
  [UnitPrice] decimal(18,2)  NOT NULL,
  [Amount] decimal(18,2)  NOT NULL,
  [ReceivedQuantity] decimal(18,2) DEFAULT 0 NOT NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'待收货' NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[PurchaseOrderDetail] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of PurchaseOrderDetail
-- ----------------------------
SET IDENTITY_INSERT [dbo].[PurchaseOrderDetail] ON
GO

SET IDENTITY_INSERT [dbo].[PurchaseOrderDetail] OFF
GO


-- ----------------------------
-- Table structure for Roles
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type IN ('U'))
	DROP TABLE [dbo].[Roles]
GO

CREATE TABLE [dbo].[Roles] (
  [RoleId] int  IDENTITY(1,1) NOT NULL,
  [RoleName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[Roles] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Roles
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Roles] ON
GO

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (N'1', N'普通用户')
GO

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName]) VALUES (N'2', N'超级管理员')
GO

SET IDENTITY_INSERT [dbo].[Roles] OFF
GO


-- ----------------------------
-- Table structure for sub_device
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[sub_device]') AND type IN ('U'))
	DROP TABLE [dbo].[sub_device]
GO

CREATE TABLE [dbo].[sub_device] (
  [sub_device_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [sub_device_name] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [sub_device_type] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [eqp_group_id] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [create_time] datetime DEFAULT getdate() NOT NULL,
  [create_user] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [edit_time] datetime  NULL,
  [edit_user] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[sub_device] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of sub_device
-- ----------------------------
INSERT INTO [dbo].[sub_device] ([sub_device_id], [sub_device_name], [sub_device_type], [eqp_group_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'SD001', N'装配站控制器', N'PLC模块', N'ASSEMBLY_LINE_A', N'2025-05-13 11:46:33.000', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[sub_device] ([sub_device_id], [sub_device_name], [sub_device_type], [eqp_group_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'SD002', N'机器人手臂A', N'六轴机械手', N'ASSEMBLY_LINE_A', N'2025-05-13 11:46:33.000', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[sub_device] ([sub_device_id], [sub_device_name], [sub_device_type], [eqp_group_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'SD003', N'CNC主轴单元', N'伺服驱动器', N'CNC_GROUP_01', N'2025-05-13 11:46:33.000', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[sub_device] ([sub_device_id], [sub_device_name], [sub_device_type], [eqp_group_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'SD004', N'冷却系统', N'循环水冷', N'CNC_GROUP_01', N'2025-05-13 11:46:33.000', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[sub_device] ([sub_device_id], [sub_device_name], [sub_device_type], [eqp_group_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'SD005', N'信号采集卡', N'数据采集器', N'TEST_BENCH_05', N'2025-05-13 11:46:33.000', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[sub_device] ([sub_device_id], [sub_device_name], [sub_device_type], [eqp_group_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'SDEQ001-1', N'控制板A主控器', N'控制器', N'ASSEMBLY_LINE_A', N'2025-05-13 15:22:20.183', N'admin', NULL, NULL)
GO

INSERT INTO [dbo].[sub_device] ([sub_device_id], [sub_device_name], [sub_device_type], [eqp_group_id], [create_time], [create_user], [edit_time], [edit_user]) VALUES (N'SDEQ001-2', N'接口板A扩展', N'接口卡', N'ASSEMBLY_LINE_A', N'2025-05-13 15:22:20.183', N'admin', NULL, NULL)
GO


-- ----------------------------
-- Table structure for Supplier
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier]') AND type IN ('U'))
	DROP TABLE [dbo].[Supplier]
GO

CREATE TABLE [dbo].[Supplier] (
  [SupplierId] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [SupplierName] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ContactPerson] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Phone] varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Email] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Address] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'正常' NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[Supplier] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Supplier
-- ----------------------------

-- ----------------------------
-- Table structure for SystemLog
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemLog]') AND type IN ('U'))
	DROP TABLE [dbo].[SystemLog]
GO

CREATE TABLE [dbo].[SystemLog] (
  [LogId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [UserName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [OperationType] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [OperationModule] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [IPAddress] varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [LogTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[SystemLog] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of SystemLog
-- ----------------------------
SET IDENTITY_INSERT [dbo].[SystemLog] ON
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'1', N'1', N'admin', N'Delete', N'EqpGroup', N'设备组 [111] 删除成功', NULL, N'2025-04-29 14:11:24.983')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'2', N'1', N'admin', N'Delete', N'User', N'用户 [无敌风火轮] (ID: 5004) 删除成功', NULL, N'2025-04-29 14:27:31.947')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'3', N'1', N'admin', N'Create', N'User', N'用户 [神人] 添加成功', NULL, N'2025-04-29 14:31:08.827')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'4', N'1', N'admin', N'Update', N'User', N'用户 [神人] (ID: 5006) 更新成功', NULL, N'2025-04-29 14:35:22.970')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'1002', N'1', N'admin', N'Update', N'Permission', N'用户 [神人] 的权限已更新。授予权限数量: 6', NULL, N'2025-04-29 14:52:28.903')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'1003', N'1', N'admin', N'Update', N'Permission', N'用户 [神人] 的权限已更新。 授予权限: factory(edit, print, view)', NULL, N'2025-04-29 14:55:02.357')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'2002', N'1', N'admin', N'ResetPassword', N'User', N'用户 [神人] (ID: 5006) 密码重置成功', NULL, N'2025-04-29 20:28:16.077')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'3002', N'1', N'admin', N'Create', N'User', N'用户 [陈平安] 添加成功', NULL, N'2025-04-29 20:45:37.247')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'3003', N'1', N'admin', N'Update', N'User', N'用户 [陈平安] (ID: 6004) 更新成功', NULL, N'2025-04-29 20:45:49.060')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'3004', N'1', N'admin', N'Delete', N'User', N'用户 [陈平安] (ID: 6004) 删除成功', NULL, N'2025-04-29 20:45:57.590')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'3005', N'1', N'admin', N'ResetPassword', N'User', N'用户 [神人] (ID: 5006) 密码重置成功', NULL, N'2025-04-29 20:46:21.557')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'4002', N'1', N'admin', N'Update', N'EqpGroup', N'设备组 [111111] 更新成功', NULL, N'2025-05-06 18:48:55.890')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'5002', N'1', N'admin', N'Create', N'EqpGroup', N'设备组 [1] 添加成功', NULL, N'2025-05-06 19:30:22.197')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'5003', N'1', N'admin', N'Create', N'EqpGroup', N'设备组 [神] 添加成功', NULL, N'2025-05-06 19:30:33.270')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'5004', N'1', N'admin', N'Delete', N'EqpGroup', N'设备组 [1] 删除成功', NULL, N'2025-05-06 19:31:00.820')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'5005', N'1', N'admin', N'Update', N'EqpGroup', N'设备组 [111111] 更新成功', NULL, N'2025-05-06 19:31:08.230')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'6002', N'1', N'admin', N'Create', N'EqpGroup', N'设备组 [陈平安] 添加成功', NULL, N'2025-05-06 19:41:31.687')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'6003', N'1', N'admin', N'Update', N'EqpGroup', N'设备组 [陈平安] 更新成功', NULL, N'2025-05-06 19:41:54.860')
GO

INSERT INTO [dbo].[SystemLog] ([LogId], [UserId], [UserName], [OperationType], [OperationModule], [Description], [IPAddress], [LogTime]) VALUES (N'6004', N'1', N'admin', N'Delete', N'EqpGroup', N'设备组 [111111] 删除成功', NULL, N'2025-05-06 19:42:04.127')
GO

SET IDENTITY_INSERT [dbo].[SystemLog] OFF
GO


-- ----------------------------
-- Table structure for UserFactory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[UserFactory]') AND type IN ('U'))
	DROP TABLE [dbo].[UserFactory]
GO

CREATE TABLE [dbo].[UserFactory] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [FactoryId] varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [IsDefault] bit DEFAULT 0 NOT NULL,
  [CreateTime] datetime DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[UserFactory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of UserFactory
-- ----------------------------
SET IDENTITY_INSERT [dbo].[UserFactory] ON
GO

INSERT INTO [dbo].[UserFactory] ([Id], [UserId], [FactoryId], [IsDefault], [CreateTime]) VALUES (N'1', N'1', N'F001', N'1', N'2025-04-22 12:40:54.747')
GO

INSERT INTO [dbo].[UserFactory] ([Id], [UserId], [FactoryId], [IsDefault], [CreateTime]) VALUES (N'2', N'1', N'F002', N'0', N'2025-04-22 12:40:54.747')
GO

INSERT INTO [dbo].[UserFactory] ([Id], [UserId], [FactoryId], [IsDefault], [CreateTime]) VALUES (N'3', N'2', N'F001', N'1', N'2025-04-22 12:40:54.747')
GO

SET IDENTITY_INSERT [dbo].[UserFactory] OFF
GO


-- ----------------------------
-- Table structure for UserPermissions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPermissions]') AND type IN ('U'))
	DROP TABLE [dbo].[UserPermissions]
GO

CREATE TABLE [dbo].[UserPermissions] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [PermissionId] int  NOT NULL
)
GO

ALTER TABLE [dbo].[UserPermissions] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of UserPermissions
-- ----------------------------
SET IDENTITY_INSERT [dbo].[UserPermissions] ON
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'11', N'1', N'1')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'6', N'1', N'2')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'8', N'1', N'3')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'7', N'1', N'4')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'10', N'1', N'5')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'9', N'1', N'6')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'20', N'1', N'7')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'16', N'1', N'8')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'18', N'1', N'9')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'17', N'1', N'10')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'19', N'1', N'11')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'5', N'1', N'12')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'1', N'1', N'13')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'3', N'1', N'14')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'2', N'1', N'15')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'4', N'1', N'16')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'15', N'1', N'17')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'12', N'1', N'18')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'14', N'1', N'19')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'13', N'1', N'20')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'1002', N'2', N'1')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'1003', N'2', N'7')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'1004', N'2', N'12')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'1005', N'2', N'17')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'3010', N'5006', N'1')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'3008', N'5006', N'3')
GO

INSERT INTO [dbo].[UserPermissions] ([Id], [UserId], [PermissionId]) VALUES (N'3009', N'5006', N'5')
GO

SET IDENTITY_INSERT [dbo].[UserPermissions] OFF
GO


-- ----------------------------
-- Table structure for Users
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type IN ('U'))
	DROP TABLE [dbo].[Users]
GO

CREATE TABLE [dbo].[Users] (
  [Id] int  IDENTITY(1,1) NOT NULL,
  [Username] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Password] nvarchar(64) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [LastLoginTime] datetime  NULL,
  [RealName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Role] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [RoleId] int  NULL
)
GO

ALTER TABLE [dbo].[Users] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Users
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Users] ON
GO

INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [LastLoginTime], [RealName], [Role], [RoleId]) VALUES (N'1', N'admin', N'6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', N'2025-05-23 12:08:39.290', N'系统管理员', N'超级管理员', N'2')
GO

INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [LastLoginTime], [RealName], [Role], [RoleId]) VALUES (N'2', N'test', N'6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', N'2025-04-24 16:52:27.557', N'测试用户', N'普通用户', N'1')
GO

INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [LastLoginTime], [RealName], [Role], [RoleId]) VALUES (N'1004', N'test2', N'c9643e02141075df6e125ea8b76d1a732d3cfd3e04aa6e0680cf884dd635b4be', N'2025-04-14 00:21:38.053', NULL, N'普通用户', N'1')
GO

INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [LastLoginTime], [RealName], [Role], [RoleId]) VALUES (N'1005', N'test00', N'c9643e02141075df6e125ea8b76d1a732d3cfd3e04aa6e0680cf884dd635b4be', N'2025-04-14 00:23:21.907', NULL, N'普通用户', N'1')
GO

INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [LastLoginTime], [RealName], [Role], [RoleId]) VALUES (N'3003', N'天才少年萧炎', N'6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', NULL, N'天才少年萧炎', NULL, N'1')
GO

INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [LastLoginTime], [RealName], [Role], [RoleId]) VALUES (N'5005', N'无敌风火轮', N'f6e0a1e2ac41945a9aa7ff8a8aaa0cebc12a3bcc981a929ad5cf810a090e11ae', NULL, N'111', NULL, N'1')
GO

INSERT INTO [dbo].[Users] ([Id], [Username], [Password], [LastLoginTime], [RealName], [Role], [RoleId]) VALUES (N'5006', N'神人', N'9ebf7eb9f32c21ab2858173c447f2884d9a4b243235ae92b86412e6f6253fe44', NULL, N'ddd', NULL, N'1')
GO

SET IDENTITY_INSERT [dbo].[Users] OFF
GO


-- ----------------------------
-- function structure for GetCategoryDescendants
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCategoryDescendants]') AND type IN ('FN', 'FS', 'FT', 'IF', 'TF'))
	DROP FUNCTION[dbo].[GetCategoryDescendants]
GO

CREATE FUNCTION [dbo].[GetCategoryDescendants]
(
    @RootCategoryId VARCHAR(20) -- 起始类别ID
)
RETURNS @DescendantTable TABLE
(
    CategoryId VARCHAR(20) PRIMARY KEY
)
AS
BEGIN
    -- 使用递归公用表表达式(CTE)查找所有子孙节点
    ;WITH CategoryHierarchy AS (
        -- 锚点成员: 起始类别本身
        SELECT CategoryId
        FROM dbo.ProductCategory
        WHERE CategoryId = @RootCategoryId

        UNION ALL

        -- 递归成员: 连接父类别ID查找子类别
        SELECT c.CategoryId
        FROM dbo.ProductCategory c
        INNER JOIN CategoryHierarchy ch ON c.ParentCategoryId = ch.CategoryId
    )
    -- 将CTE的结果插入到返回的表变量中
    INSERT INTO @DescendantTable (CategoryId)
    SELECT CategoryId
    FROM CategoryHierarchy
    OPTION (MAXRECURSION 0); -- 允许无限递归深度 (或根据需要设置合理限制)

    RETURN;
END;
GO


-- ----------------------------
-- Primary Key structure for table Area
-- ----------------------------
ALTER TABLE [dbo].[Area] ADD CONSTRAINT [PK__Area__70B8204850AAB3F8] PRIMARY KEY CLUSTERED ([AreaId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for DataChangeLog
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[DataChangeLog]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table DataChangeLog
-- ----------------------------
ALTER TABLE [dbo].[DataChangeLog] ADD CONSTRAINT [PK__DataChan__5E5486484DB79CA5] PRIMARY KEY CLUSTERED ([LogId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Department
-- ----------------------------
ALTER TABLE [dbo].[Department] ADD CONSTRAINT [PK__Departme__014881AEEE745102] PRIMARY KEY CLUSTERED ([DeptId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Uniques structure for table Employee
-- ----------------------------
ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [UK_Employee_Email] UNIQUE NONCLUSTERED ([Email] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [UK_Employee_IdCard] UNIQUE NONCLUSTERED ([IdCard] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Checks structure for table Employee
-- ----------------------------
ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [CHK_Employee_Status] CHECK ([Status]=N'休假' OR [Status]=N'离职' OR [Status]=N'在职')
GO


-- ----------------------------
-- Primary Key structure for table Employee
-- ----------------------------
ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [PK__Employee__7AD04F11D8FB33F3] PRIMARY KEY CLUSTERED ([EmployeeId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table eqp_group
-- ----------------------------
ALTER TABLE [dbo].[eqp_group] ADD CONSTRAINT [PK_eqp_group] PRIMARY KEY CLUSTERED ([eqp_group_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for eqp_group_his
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[eqp_group_his]', RESEED, 6001)
GO


-- ----------------------------
-- Indexes structure for table eqp_group_his
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_eqp_group_his_eqp_group_id]
ON [dbo].[eqp_group_his] (
  [eqp_group_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table eqp_group_his
-- ----------------------------
ALTER TABLE [dbo].[eqp_group_his] ADD CONSTRAINT [PK_eqp_group_his] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Equipment
-- ----------------------------
ALTER TABLE [dbo].[Equipment] ADD CONSTRAINT [PK__Equipmen__34474479D348FFDB] PRIMARY KEY CLUSTERED ([EquipmentId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table EquipmentCategory
-- ----------------------------
ALTER TABLE [dbo].[EquipmentCategory] ADD CONSTRAINT [PK__Equipmen__19093A0B55449C82] PRIMARY KEY CLUSTERED ([CategoryId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for EquipmentMaintenance
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[EquipmentMaintenance]', RESEED, 1001)
GO


-- ----------------------------
-- Primary Key structure for table EquipmentMaintenance
-- ----------------------------
ALTER TABLE [dbo].[EquipmentMaintenance] ADD CONSTRAINT [PK__Equipmen__E60542D5250B9C06] PRIMARY KEY CLUSTERED ([MaintenanceId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Factory
-- ----------------------------
ALTER TABLE [dbo].[Factory] ADD CONSTRAINT [PK__Factory__732C8F7DA9B76D01] PRIMARY KEY CLUSTERED ([FactoryId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table FactoryExtendInfo
-- ----------------------------
ALTER TABLE [dbo].[FactoryExtendInfo] ADD CONSTRAINT [PK__FactoryE__732C8F7DFA7CF791] PRIMARY KEY CLUSTERED ([FactoryId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for FactoryMenuPermissions
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[FactoryMenuPermissions]', RESEED, 1001)
GO


-- ----------------------------
-- Uniques structure for table FactoryMenuPermissions
-- ----------------------------
ALTER TABLE [dbo].[FactoryMenuPermissions] ADD CONSTRAINT [UK_FactoryMenuPermissions] UNIQUE NONCLUSTERED ([FactoryId] ASC, [MenuId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table FactoryMenuPermissions
-- ----------------------------
ALTER TABLE [dbo].[FactoryMenuPermissions] ADD CONSTRAINT [PK__FactoryM__3214EC07CA214F99] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Inventory
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Inventory]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table Inventory
-- ----------------------------
ALTER TABLE [dbo].[Inventory] ADD CONSTRAINT [UK_Inventory] UNIQUE NONCLUSTERED ([ProductId] ASC, [FactoryId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Inventory
-- ----------------------------
ALTER TABLE [dbo].[Inventory] ADD CONSTRAINT [PK__Inventor__3214EC074B93983A] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Menu
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Menu]', RESEED, 10)
GO


-- ----------------------------
-- Primary Key structure for table Menu
-- ----------------------------
ALTER TABLE [dbo].[Menu] ADD CONSTRAINT [PK__Menu__C99ED2303AC776F6] PRIMARY KEY CLUSTERED ([MenuId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for MenuPermission
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[MenuPermission]', RESEED, 1001)
GO


-- ----------------------------
-- Uniques structure for table MenuPermission
-- ----------------------------
ALTER TABLE [dbo].[MenuPermission] ADD CONSTRAINT [UK_MenuPermission] UNIQUE NONCLUSTERED ([MenuId] ASC, [PermissionId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table MenuPermission
-- ----------------------------
ALTER TABLE [dbo].[MenuPermission] ADD CONSTRAINT [PK__MenuPerm__3214EC070B862BF5] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Permissions
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Permissions]', RESEED, 20)
GO


-- ----------------------------
-- Uniques structure for table Permissions
-- ----------------------------
ALTER TABLE [dbo].[Permissions] ADD CONSTRAINT [UK_Permissions] UNIQUE NONCLUSTERED ([ModuleName] ASC, [ActionName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Permissions
-- ----------------------------
ALTER TABLE [dbo].[Permissions] ADD CONSTRAINT [PK__Permissi__EFA6FB2F5AC6828D] PRIMARY KEY CLUSTERED ([PermissionId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table port
-- ----------------------------
ALTER TABLE [dbo].[port] ADD CONSTRAINT [PK_port] PRIMARY KEY CLUSTERED ([port_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Process
-- ----------------------------
ALTER TABLE [dbo].[Process] ADD CONSTRAINT [PK_Process] PRIMARY KEY CLUSTERED ([ProcessId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ProcessPackage
-- ----------------------------
ALTER TABLE [dbo].[ProcessPackage] ADD CONSTRAINT [PK_ProcessPackage] PRIMARY KEY CLUSTERED ([PackageId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ProcessRoute
-- ----------------------------
ALTER TABLE [dbo].[ProcessRoute] ADD CONSTRAINT [PK_ProcessRoute] PRIMARY KEY CLUSTERED ([RouteId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Product
-- ----------------------------
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [PK__Product__B40CC6CD0CB37ABA] PRIMARY KEY CLUSTERED ([ProductId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ProductCategory
-- ----------------------------
ALTER TABLE [dbo].[ProductCategory] ADD CONSTRAINT [PK__ProductC__19093A0BA8129F48] PRIMARY KEY CLUSTERED ([CategoryId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ProductionPlan
-- ----------------------------
ALTER TABLE [dbo].[ProductionPlan] ADD CONSTRAINT [PK__Producti__755C22B7144C303B] PRIMARY KEY CLUSTERED ([PlanId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ProductionRecord
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ProductionRecord]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table ProductionRecord
-- ----------------------------
ALTER TABLE [dbo].[ProductionRecord] ADD CONSTRAINT [PK__Producti__FBDF78E9C193CFB7] PRIMARY KEY CLUSTERED ([RecordId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table PurchaseOrder
-- ----------------------------
ALTER TABLE [dbo].[PurchaseOrder] ADD CONSTRAINT [PK__Purchase__C3905BCF7A38BAC7] PRIMARY KEY CLUSTERED ([OrderId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for PurchaseOrderDetail
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[PurchaseOrderDetail]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table PurchaseOrderDetail
-- ----------------------------
ALTER TABLE [dbo].[PurchaseOrderDetail] ADD CONSTRAINT [PK__Purchase__135C316D7B772DB9] PRIMARY KEY CLUSTERED ([DetailId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Roles
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Roles]', RESEED, 2)
GO


-- ----------------------------
-- Uniques structure for table Roles
-- ----------------------------
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [UK_Roles_RoleName] UNIQUE NONCLUSTERED ([RoleName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Roles
-- ----------------------------
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table sub_device
-- ----------------------------
ALTER TABLE [dbo].[sub_device] ADD CONSTRAINT [PK_sub_device] PRIMARY KEY CLUSTERED ([sub_device_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Supplier
-- ----------------------------
ALTER TABLE [dbo].[Supplier] ADD CONSTRAINT [PK__Supplier__4BE666B461748BF9] PRIMARY KEY CLUSTERED ([SupplierId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for SystemLog
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SystemLog]', RESEED, 7001)
GO


-- ----------------------------
-- Primary Key structure for table SystemLog
-- ----------------------------
ALTER TABLE [dbo].[SystemLog] ADD CONSTRAINT [PK__SystemLo__5E548648E032A204] PRIMARY KEY CLUSTERED ([LogId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for UserFactory
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[UserFactory]', RESEED, 3)
GO


-- ----------------------------
-- Uniques structure for table UserFactory
-- ----------------------------
ALTER TABLE [dbo].[UserFactory] ADD CONSTRAINT [UK_UserFactory] UNIQUE NONCLUSTERED ([UserId] ASC, [FactoryId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table UserFactory
-- ----------------------------
ALTER TABLE [dbo].[UserFactory] ADD CONSTRAINT [PK__UserFact__3214EC076CBEB63C] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for UserPermissions
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[UserPermissions]', RESEED, 4001)
GO


-- ----------------------------
-- Uniques structure for table UserPermissions
-- ----------------------------
ALTER TABLE [dbo].[UserPermissions] ADD CONSTRAINT [UK_UserPermissions] UNIQUE NONCLUSTERED ([UserId] ASC, [PermissionId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table UserPermissions
-- ----------------------------
ALTER TABLE [dbo].[UserPermissions] ADD CONSTRAINT [PK__UserPerm__3214EC07FEB0C428] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Users
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 7003)
GO


-- ----------------------------
-- Uniques structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UK_Users_Username] UNIQUE NONCLUSTERED ([Username] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK__Users__3214EC0713A6B6A4] PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table DataChangeLog
-- ----------------------------
ALTER TABLE [dbo].[DataChangeLog] ADD CONSTRAINT [FK_DataChangeLog_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Department
-- ----------------------------
ALTER TABLE [dbo].[Department] ADD CONSTRAINT [FK_Department_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Department] ADD CONSTRAINT [FK_Department_Parent] FOREIGN KEY ([ParentDeptId]) REFERENCES [dbo].[Department] ([DeptId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Department] ADD CONSTRAINT [FK_Department_Manager_Employee] FOREIGN KEY ([ManagerEmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Employee
-- ----------------------------
ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [FK_Employee_Department] FOREIGN KEY ([DeptId]) REFERENCES [dbo].[Department] ([DeptId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Employee] ADD CONSTRAINT [FK_Employee_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table eqp_group
-- ----------------------------
ALTER TABLE [dbo].[eqp_group] ADD CONSTRAINT [FK_eqp_group_Factory] FOREIGN KEY ([factory_id]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Equipment
-- ----------------------------
ALTER TABLE [dbo].[Equipment] ADD CONSTRAINT [FK_Equipment_EqpGroup] FOREIGN KEY ([eqp_group_id]) REFERENCES [dbo].[eqp_group] ([eqp_group_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Equipment] ADD CONSTRAINT [FK_Equipment_SubDevice] FOREIGN KEY ([sub_device_id]) REFERENCES [dbo].[sub_device] ([sub_device_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Equipment] ADD CONSTRAINT [FK_Equipment_Port] FOREIGN KEY ([port_id]) REFERENCES [dbo].[port] ([port_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Equipment] ADD CONSTRAINT [FK_Equipment_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[EquipmentCategory] ([CategoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Equipment] ADD CONSTRAINT [FK_Equipment_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Equipment] ADD CONSTRAINT [FK_Equipment_Employee] FOREIGN KEY ([ResponsiblePerson]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table EquipmentMaintenance
-- ----------------------------
ALTER TABLE [dbo].[EquipmentMaintenance] ADD CONSTRAINT [FK_Maintenance_Equipment] FOREIGN KEY ([EquipmentId]) REFERENCES [dbo].[Equipment] ([EquipmentId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[EquipmentMaintenance] ADD CONSTRAINT [FK_Maintenance_Employee] FOREIGN KEY ([MaintenancePerson]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Factory
-- ----------------------------
ALTER TABLE [dbo].[Factory] ADD CONSTRAINT [FK_Factory_Manager_Employee] FOREIGN KEY ([ManagerEmployeeId]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table FactoryExtendInfo
-- ----------------------------
ALTER TABLE [dbo].[FactoryExtendInfo] ADD CONSTRAINT [FK_FactoryExtendInfo_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table FactoryMenuPermissions
-- ----------------------------
ALTER TABLE [dbo].[FactoryMenuPermissions] ADD CONSTRAINT [FK_FactoryMenuPermissions_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[FactoryMenuPermissions] ADD CONSTRAINT [FK_FactoryMenuPermissions_Menu] FOREIGN KEY ([MenuId]) REFERENCES [dbo].[Menu] ([MenuId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Inventory
-- ----------------------------
ALTER TABLE [dbo].[Inventory] ADD CONSTRAINT [FK_Inventory_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Inventory] ADD CONSTRAINT [FK_Inventory_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Menu
-- ----------------------------
ALTER TABLE [dbo].[Menu] ADD CONSTRAINT [FK_Menu_Parent] FOREIGN KEY ([ParentMenuId]) REFERENCES [dbo].[Menu] ([MenuId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table MenuPermission
-- ----------------------------
ALTER TABLE [dbo].[MenuPermission] ADD CONSTRAINT [FK_MenuPermission_Menu] FOREIGN KEY ([MenuId]) REFERENCES [dbo].[Menu] ([MenuId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[MenuPermission] ADD CONSTRAINT [FK_MenuPermission_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permissions] ([PermissionId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table port
-- ----------------------------
ALTER TABLE [dbo].[port] ADD CONSTRAINT [FK_port_sub_device] FOREIGN KEY ([parent_device_id]) REFERENCES [dbo].[sub_device] ([sub_device_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Process
-- ----------------------------
ALTER TABLE [dbo].[Process] ADD CONSTRAINT [FK_Process_ProcessPackage] FOREIGN KEY ([PackageId]) REFERENCES [dbo].[ProcessPackage] ([PackageId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ProcessRoute
-- ----------------------------
ALTER TABLE [dbo].[ProcessRoute] ADD CONSTRAINT [FK_ProcessRoute_Process] FOREIGN KEY ([ProcessId]) REFERENCES [dbo].[Process] ([ProcessId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Product
-- ----------------------------
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[ProductCategory] ([CategoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ProductCategory
-- ----------------------------
ALTER TABLE [dbo].[ProductCategory] ADD CONSTRAINT [FK_ProductCategory_Parent] FOREIGN KEY ([ParentCategoryId]) REFERENCES [dbo].[ProductCategory] ([CategoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ProductionPlan
-- ----------------------------
ALTER TABLE [dbo].[ProductionPlan] ADD CONSTRAINT [FK_ProductionPlan_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[ProductionPlan] ADD CONSTRAINT [FK_ProductionPlan_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[ProductionPlan] ADD CONSTRAINT [FK_ProductionPlan_Employee] FOREIGN KEY ([ResponsiblePerson]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ProductionRecord
-- ----------------------------
ALTER TABLE [dbo].[ProductionRecord] ADD CONSTRAINT [FK_ProductionRecord_Plan] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[ProductionPlan] ([PlanId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[ProductionRecord] ADD CONSTRAINT [FK_ProductionRecord_Employee] FOREIGN KEY ([Operator]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table PurchaseOrder
-- ----------------------------
ALTER TABLE [dbo].[PurchaseOrder] ADD CONSTRAINT [FK_PurchaseOrder_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[PurchaseOrder] ADD CONSTRAINT [FK_PurchaseOrder_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Supplier] ([SupplierId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[PurchaseOrder] ADD CONSTRAINT [FK_PurchaseOrder_Purchaser] FOREIGN KEY ([Purchaser]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[PurchaseOrder] ADD CONSTRAINT [FK_PurchaseOrder_Approver] FOREIGN KEY ([Approver]) REFERENCES [dbo].[Employee] ([EmployeeId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table PurchaseOrderDetail
-- ----------------------------
ALTER TABLE [dbo].[PurchaseOrderDetail] ADD CONSTRAINT [FK_PurchaseOrderDetail_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[PurchaseOrder] ([OrderId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[PurchaseOrderDetail] ADD CONSTRAINT [FK_PurchaseOrderDetail_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table sub_device
-- ----------------------------
ALTER TABLE [dbo].[sub_device] ADD CONSTRAINT [FK_sub_device_eqp_group] FOREIGN KEY ([eqp_group_id]) REFERENCES [dbo].[eqp_group] ([eqp_group_id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table SystemLog
-- ----------------------------
ALTER TABLE [dbo].[SystemLog] ADD CONSTRAINT [FK_SystemLog_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table UserFactory
-- ----------------------------
ALTER TABLE [dbo].[UserFactory] ADD CONSTRAINT [FK_UserFactory_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[UserFactory] ADD CONSTRAINT [FK_UserFactory_Factory] FOREIGN KEY ([FactoryId]) REFERENCES [dbo].[Factory] ([FactoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table UserPermissions
-- ----------------------------
ALTER TABLE [dbo].[UserPermissions] ADD CONSTRAINT [FK_UserPermissions_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[UserPermissions] ADD CONSTRAINT [FK_UserPermissions_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permissions] ([PermissionId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Table structure for SystemParameters
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemParameters]') AND type IN ('U'))
    DROP TABLE [dbo].[SystemParameters]
GO

CREATE TABLE [dbo].[SystemParameters] (
  [ParamKey] nvarchar(120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
  [ParamValue] nvarchar(2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [Description] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
  [UpdatedAt] datetime NOT NULL DEFAULT getdate(),
  CONSTRAINT [PK_SystemParameters] PRIMARY KEY CLUSTERED ([ParamKey])
)
GO

ALTER TABLE [dbo].[SystemParameters] SET (LOCK_ESCALATION = TABLE)
GO

-- ----------------------------
-- Records of SystemParameters
-- ----------------------------
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Security.MaxFailedLogin', N'5', N'登录失败锁定阈值')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Security.LockoutMinutes', N'15', N'锁定时长(分钟)')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Security.PasswordMinLength', N'8', N'密码最小长度')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Security.PasswordRequireNumber', N'true', N'密码需包含数字')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Security.PasswordRequireUpper', N'false', N'密码需包含大写字母')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Security.PasswordRequireLower', N'false', N'密码需包含小写字母')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Security.PasswordRequireSpecial', N'false', N'密码需包含特殊字符')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Backup.RetentionDays', N'7', N'备份保留天数')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'Backup.Directory', N'', N'备份目录(为空则使用默认)')
GO
INSERT INTO [dbo].[SystemParameters] ([ParamKey], [ParamValue], [Description]) VALUES (N'UI.AccentColor', N'00A3FF', N'主题强调色')
GO

-- ----------------------------
-- Table structure for UserSecurity
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[UserSecurity]') AND type IN ('U'))
    DROP TABLE [dbo].[UserSecurity]
GO

CREATE TABLE [dbo].[UserSecurity] (
  [UserId] int NOT NULL,
  [FailedCount] int NOT NULL DEFAULT 0,
  [LastFailedAt] datetime NULL,
  [LockoutUntil] datetime NULL,
  [LastSuccessAt] datetime NULL,
  CONSTRAINT [PK_UserSecurity] PRIMARY KEY CLUSTERED ([UserId])
)
GO

ALTER TABLE [dbo].[UserSecurity] SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE [dbo].[UserSecurity] ADD CONSTRAINT [FK_UserSecurity_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO
