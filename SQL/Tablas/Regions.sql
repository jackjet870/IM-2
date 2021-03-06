if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LeadSources_Regions]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT FK_LeadSources_Regions
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Regions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Regions]
GO

CREATE TABLE [dbo].[Regions] (
	[rgID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Regions] WITH NOCHECK ADD 
	CONSTRAINT [PK_Regions] PRIMARY KEY  CLUSTERED 
	(
		[rgID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LeadSources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[LeadSources] ADD 
	CONSTRAINT [FK_LeadSources_Regions] FOREIGN KEY 
	(
		[lsrg]
	) REFERENCES [dbo].[Regions] (
		[rgID]
	)
GO
