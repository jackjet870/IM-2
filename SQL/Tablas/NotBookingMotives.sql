if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Guests_NotBookingMotives]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT FK_Guests_NotBookingMotives
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NotBookingMotives]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[NotBookingMotives]
GO

CREATE TABLE [dbo].[NotBookingMotives] (
	[nbID] [int] IDENTITY (1, 1) NOT NULL ,
	[nbN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[nbA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[NotBookingMotives] WITH NOCHECK ADD 
	CONSTRAINT [PK_NotBookingMotives] PRIMARY KEY  CLUSTERED 
	(
		[nbID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'NotBookingMotives', N'column', N'nbA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'NotBookingMotives', N'column', N'nbID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'NotBookingMotives', N'column', N'nbN'


GO


ALTER TABLE [dbo].[Guests] ADD 
	CONSTRAINT [FK_Guests_NotBookingMotives] FOREIGN KEY 
	(
		[gunb]
	) REFERENCES [dbo].[NotBookingMotives] (
		[nbID]
	)
GO
