
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