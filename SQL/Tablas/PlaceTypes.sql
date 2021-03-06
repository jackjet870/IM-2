if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Goals_PlaceTypes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Goals] DROP CONSTRAINT FK_Goals_PlaceTypes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PlaceTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PlaceTypes]
GO

CREATE TABLE [dbo].[PlaceTypes] (
	[pyID] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pyN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pyA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PlaceTypes] WITH NOCHECK ADD 
	CONSTRAINT [PK_PlaceTypes] PRIMARY KEY  CLUSTERED 
	(
		[pyID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Goals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Goals] ADD 
	CONSTRAINT [FK_Goals_PlaceTypes] FOREIGN KEY 
	(
		[gopy]
	) REFERENCES [dbo].[PlaceTypes] (
		[pyID]
	)
GO
