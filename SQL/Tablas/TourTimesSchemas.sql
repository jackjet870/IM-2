if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_osConfig_TourTimesSchemas]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[osConfig] DROP CONSTRAINT FK_osConfig_TourTimesSchemas
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TourTimesSchemas]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TourTimesSchemas]
GO

CREATE TABLE [dbo].[TourTimesSchemas] (
	[tcID] [int] IDENTITY (1, 1) NOT NULL ,
	[tcN] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tcA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TourTimesSchemas] WITH NOCHECK ADD 
	CONSTRAINT [PK_TourTimesSchemas] PRIMARY KEY  CLUSTERED 
	(
		[tcID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de esquemas de horarios de tour', N'user', N'dbo', N'table', N'TourTimesSchemas'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'TourTimesSchemas', N'column', N'tcA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'TourTimesSchemas', N'column', N'tcID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'TourTimesSchemas', N'column', N'tcN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[osConfig]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[osConfig] ADD 
	CONSTRAINT [FK_osConfig_TourTimesSchemas] FOREIGN KEY
	(
		[ocTourTimesSchema]
	) REFERENCES [dbo].[TourTimesSchemas]
	(
		[tcID]
	)
GO