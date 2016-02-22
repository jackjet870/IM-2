if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DemAge]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[DemAge]
GO

CREATE TABLE [dbo].[DemAge] (
	[daID] [int] IDENTITY (1, 1) NOT NULL ,
	[daN] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[daFrom] [tinyint] NOT NULL ,
	[daTo] [tinyint] NOT NULL ,
	[daO] [tinyint] NOT NULL ,
	[daA] [bit] NOT NULL 
) ON [PRIMARY]
GO

