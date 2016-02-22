if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TeamsTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TeamsTypes]
GO

CREATE TABLE [dbo].[TeamsTypes] (
	[ttID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ttN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ttA] [bit] NOT NULL 
) ON [PRIMARY]
GO

