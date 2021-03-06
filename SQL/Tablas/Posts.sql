if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Personnel_Posts]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT FK_Personnel_Posts
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Posts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Posts]
GO

CREATE TABLE [dbo].[Posts] (
	[poID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[poN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[poA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Posts] WITH NOCHECK ADD 
	CONSTRAINT [PK_Posts] PRIMARY KEY  CLUSTERED 
	(
		[poID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Personnel]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Personnel] ADD 
	CONSTRAINT [FK_Personnel_Posts] FOREIGN KEY 
	(
		[pepo]
	) REFERENCES [dbo].[Posts] (
		[poID]
	)
GO