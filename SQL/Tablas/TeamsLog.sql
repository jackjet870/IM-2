if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TeamsLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TeamsLog]
GO

CREATE TABLE [dbo].[TeamsLog] (
	[tlID] [int] IDENTITY (1, 1) NOT NULL ,
	[tlDT] [datetime] NOT NULL ,
	[tlChangedBy] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tlpe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tlTeamType] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tlPlaceID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[tlTeam] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

