if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Agencies_Markets]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT FK_Agencies_Markets
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HotelAgencies_Markets]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HotelAgencies] DROP CONSTRAINT FK_HotelAgencies_Markets
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Markets]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Markets]
GO

CREATE TABLE [dbo].[Markets] (
	[mkID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mkN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mkA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Markets] WITH NOCHECK ADD 
	CONSTRAINT [PK_Markets] PRIMARY KEY  CLUSTERED 
	(
		[mkID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Markets] ADD 
	CONSTRAINT [DF_Markets_mkA] DEFAULT (1) FOR [mkA]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Agencies] ADD 
	CONSTRAINT [FK_Agencies_Markets] FOREIGN KEY 
	(
		[agmk]
	) REFERENCES [dbo].[Markets] (
		[mkID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HotelAgencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[HotelAgencies] ADD 
	CONSTRAINT [FK_HotelAgencies_Markets] FOREIGN KEY 
	(
		[hamk]
	) REFERENCES [dbo].[Markets] (
		[mkID]
	)
GO