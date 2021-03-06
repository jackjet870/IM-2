if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LeadSources_Areas]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT FK_LeadSources_Areas
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Areas]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Areas]
GO

CREATE TABLE [dbo].[Areas] (
	[arID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[arN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[arA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Areas] WITH NOCHECK ADD 
	CONSTRAINT [PK_Areas] PRIMARY KEY  CLUSTERED 
	(
		[arID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Areas] ADD 
	CONSTRAINT [DF_Areas_arA] DEFAULT (1) FOR [arA]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LeadSources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[LeadSources] ADD 
	CONSTRAINT [FK_LeadSources_Areas] FOREIGN KEY 
	(
		[lsar]
	) REFERENCES [dbo].[Areas] (
		[arID]
	)
GO
