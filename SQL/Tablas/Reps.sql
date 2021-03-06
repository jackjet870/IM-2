if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Agencies_Reps]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT FK_Agencies_Reps
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reps]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Reps]
GO

CREATE TABLE [dbo].[Reps] (
	[rpID] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[rpA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Reps] WITH NOCHECK ADD 
	CONSTRAINT [PK_Reps] PRIMARY KEY  CLUSTERED 
	(
		[rpID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Reps] ADD 
	CONSTRAINT [DF_Reps_rpA] DEFAULT (1) FOR [rpA]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Agencies] ADD 
	CONSTRAINT [FK_Agencies_Reps] FOREIGN KEY 
	(
		[agrp]
	) REFERENCES [dbo].[Reps] (
		[rpID]
	)
GO