if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PostsLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PostsLog]
GO

CREATE TABLE [dbo].[PostsLog] (
	[ppID] [tinyint] IDENTITY (1, 1) NOT NULL ,
	[ppDT] [datetime] NOT NULL ,
	[ppChangedBy] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pppe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pppo] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

