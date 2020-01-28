
INSERT INTO [dbo].[Config]([DisplayName],[Name],[ValueDefault],[Description],[IsActive])
     VALUES
           (N'đọc cảnh báo khi sản lượng vượt ngưỡng'
           ,N'IsWarningIfProductIsOver'
           ,'0'
           ,N'1->co , 0 -> không'
           ,1)
GO

INSERT INTO [dbo].[Config]([DisplayName],[Name],[ValueDefault],[Description],[IsActive])
     VALUES
           (N'âm thanh báo btp vượt sản lượng kế hoạch'
           ,N'SoundBTPOrverPlan'
           ,'file.wav'
           ,N'file.wav'
           ,1)
GO

INSERT INTO [dbo].[Config]([DisplayName],[Name],[ValueDefault],[Description],[IsActive])
     VALUES
           (N'âm thanh báo KCS vượt TC'
           ,N'SoundKCSOrverTC'
           ,'file.wav'
           ,N'file.wav'
           ,1)
GO

INSERT INTO [dbo].[Config]([DisplayName],[Name],[ValueDefault],[Description],[IsActive])
     VALUES
           (N'âm thanh báo TC vượt BTP'
           ,N'SoundTCOrverBTP'
           ,'file.wav'
           ,N'file.wav'
           ,1)
GO

INSERT INTO [dbo].[Config]([DisplayName],[Name],[ValueDefault],[Description],[IsActive])
     VALUES
           (N'Thời gian chờ khi khởi tao thông tin keypad milisecond'
           ,N'TimeSleepWhenInitKeypad'
           ,'1000'
           ,N'Thời gian chờ khi khởi tao thông tin keypad milisecond'
           ,1)
GO


INSERT INTO [dbo].[Config]([DisplayName],[Name],[ValueDefault],[Description],[IsActive])
     VALUES
           (N'duong dan am thanh'
           ,N'SoundPath'
           ,''
           ,N'duong dan am thanh'
           ,1)
GO

INSERT [dbo].[MAIL_FILE] ( [SystemName], [Code], [Name], [Description], [Path], [IsActive], [IsDeleted]) VALUES ( N'NSCHUYENHANGGIO_SH_YES', N'NSChuyen_HomQua', N'Bảng theo dõi năng suất hàng ngày SH - Hôm qua', N'Báo cáo năng suất theo giờ ngày hôm qua', N'\Report\NSChuyen\', 1, 0)
GO

INSERT INTO [dbo].[Config]([DisplayName],[Name],[ValueDefault],[Description],[IsActive])
     VALUES
           (N'tính doanh thu theo'
           ,N'TypeOfCalculateRevenues'
           ,'TH'
           ,N'tính doanh thu theo ? TH -> thực hiện | TC -> thoát chuyền'
           ,1)
GO

 