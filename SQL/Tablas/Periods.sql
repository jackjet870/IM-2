if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Efficiency_Periods]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Efficiency] DROP CONSTRAINT FK_Efficiency_Periods
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Goals_Periods]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Goals] DROP CONSTRAINT FK_Goals_Periods
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Periods]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Periods]
GO

CREATE TABLE [dbo].[Periods] (
	[pdID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pdN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pdA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Periods] WITH NOCHECK ADD 
	CONSTRAINT [PK_Periods] PRIMARY KEY  CLUSTERED 
	(
		[pdID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'Periods', N'column', N'pdA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Periods', N'column', N'pdID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'Periods', N'column', N'pdN'


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Efficiency]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Efficiency] ADD 
	CONSTRAINT [FK_Efficiency_Periods] FOREIGN KEY 
	(
		[efpd]
	) REFERENCES [dbo].[Periods] (
		[pdID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Goals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Goals] ADD 
	CONSTRAINT [FK_Goals_Periods] FOREIGN KEY 
	(
		[gopd]
	) REFERENCES [dbo].[Periods] (
		[pdID]
	)
GO
