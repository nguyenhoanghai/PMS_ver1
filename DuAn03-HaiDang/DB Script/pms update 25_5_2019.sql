alter table sanpham add MaKhachHang nvarchar(250) null
alter table sanpham add DonGiaCat float not null default(0)
GO

alter table thanhpham add LDOff int not null default(0)
alter table thanhpham add LDVacation int not null default(0)
alter table thanhpham add LDPregnant int not null default(0) 
alter table thanhpham add LDNew int not null default(0)
GO

alter table chuyen_sanpham add DateInput datetime   null 
alter table chuyen_sanpham add DateOutput datetime   null 
GO

/****** Object:  Table [dbo].[P_Department]    Script Date: 05/23/2019 3:48:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[P_Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[BaseLabours] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_P_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_Department] ADD  CONSTRAINT [DF_P_Department_BaseLabours]  DEFAULT ((0)) FOR [BaseLabours]
GO

ALTER TABLE [dbo].[P_Department] ADD  CONSTRAINT [DF_P_Department_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

/****** Object:  Table [dbo].[P_DepartmentDailyLabour]    Script Date: 05/23/2019 5:13:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[P_DepartmentDailyLabour](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [varchar](10) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[LDCurrent] [int] NOT NULL,
	[LDOff] [int] NOT NULL,
	[LDVacation] [int] NOT NULL,
	[LDPregnant] [int] NOT NULL,
	[LDNew] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_P_DepartmentDailyLabour] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour] ADD  CONSTRAINT [DF_P_DepartmentDailyLabour_LDOff1]  DEFAULT ((0)) FOR [LDCurrent]
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour] ADD  CONSTRAINT [DF_P_DepartmentDailyLabour_LDOff]  DEFAULT ((0)) FOR [LDOff]
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour] ADD  CONSTRAINT [DF_P_DepartmentDailyLabour_LDVacation]  DEFAULT ((0)) FOR [LDVacation]
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour] ADD  CONSTRAINT [DF_P_DepartmentDailyLabour_LDPregnant]  DEFAULT ((0)) FOR [LDPregnant]
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour] ADD  CONSTRAINT [DF_P_DepartmentDailyLabour_LDNew]  DEFAULT ((0)) FOR [LDNew]
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour] ADD  CONSTRAINT [DF_P_DepartmentDailyLabour_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour]  WITH CHECK ADD  CONSTRAINT [FK_P_DepartmentDailyLabour_P_Department] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[P_Department] ([Id])
GO

ALTER TABLE [dbo].[P_DepartmentDailyLabour] CHECK CONSTRAINT [FK_P_DepartmentDailyLabour_P_Department]
GO

INSERT INTO [dbo].[Config]
           ([DisplayName]
           ,[Name]
           ,[ValueDefault]
           ,[Description]
           ,[IsActive])
     VALUES
           (N'Có sử dụng BTP hoàn chỉnh không ?'
           ,N'IsUseBTP_HC'
           ,'0'
           ,N'1->co , 0 -> không'
           ,1)
GO