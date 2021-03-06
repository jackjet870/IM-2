if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Guests_RoomTypes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT FK_Guests_RoomTypes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RoomTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[RoomTypes]
GO

CREATE TABLE [dbo].[RoomTypes] (
	[rtID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[rtN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[rtA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RoomTypes] WITH NOCHECK ADD 
	CONSTRAINT [PK_RoomTypes] PRIMARY KEY  CLUSTERED 
	(
		[rtID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[RoomTypes] ADD 
	CONSTRAINT [DF_RoomTypes_rtA] DEFAULT (1) FOR [rtA]
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'RoomTypes', N'column', N'rtA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'RoomTypes', N'column', N'rtID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripcion', N'user', N'dbo', N'table', N'RoomTypes', N'column', N'rtN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Guests]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Guests] ADD 
	CONSTRAINT [FK_Guests_RoomTypes] FOREIGN KEY 
	(
		[gurt]
	) REFERENCES [dbo].[RoomTypes] (
		[rtID]
	)
GO