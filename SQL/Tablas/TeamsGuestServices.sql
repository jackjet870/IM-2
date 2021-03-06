if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TeamsGuestServices]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TeamsGuestServices]
GO

CREATE TABLE [dbo].[TeamsGuestServices] (
	[tglo] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tgID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tgN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tgLeader] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[tgA] [bit] NOT NULL 
) ON [PRIMARY]
GO

