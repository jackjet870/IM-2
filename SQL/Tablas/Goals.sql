if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Goals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Goals]
GO

CREATE TABLE [dbo].[Goals] (
	[gopy] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[goPlaceID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[goDateFrom] [datetime] NOT NULL ,
	[goDateTo] [datetime] NOT NULL ,
	[gopd] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[goGoal] [money] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Goals] WITH NOCHECK ADD 
	CONSTRAINT [PK_Goals] PRIMARY KEY  CLUSTERED 
	(
		[gopy],
		[goPlaceID],
		[goDateFrom],
		[goDateTo],
		[gopd]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Goals] ADD 
	CONSTRAINT [FK_Goals_Periods] FOREIGN KEY 
	(
		[gopd]
	) REFERENCES [dbo].[Periods] (
		[pdID]
	),
	CONSTRAINT [FK_Goals_PlaceTypes] FOREIGN KEY 
	(
		[gopy]
	) REFERENCES [dbo].[PlaceTypes] (
		[pyID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Fecha desde', N'user', N'dbo', N'table', N'Goals', N'column', N'goDateFrom'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha hasta', N'user', N'dbo', N'table', N'Goals', N'column', N'goDateTo'
GO
exec sp_addextendedproperty N'MS_Description', N'Meta', N'user', N'dbo', N'table', N'Goals', N'column', N'goGoal'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del periodo', N'user', N'dbo', N'table', N'Goals', N'column', N'gopd'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del lugar', N'user', N'dbo', N'table', N'Goals', N'column', N'goPlaceID'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del tipo de lugar', N'user', N'dbo', N'table', N'Goals', N'column', N'gopy'


GO

