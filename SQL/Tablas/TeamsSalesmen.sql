if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TeamsSalesmen]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TeamsSalesmen]
GO

CREATE TABLE [dbo].[TeamsSalesmen] (
	[tssr] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tsID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tsN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tsLeader] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tsA] [bit] NOT NULL 
) ON [PRIMARY]
GO

