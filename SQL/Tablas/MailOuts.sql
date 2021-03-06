if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MailOuts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MailOuts]
GO

CREATE TABLE [dbo].[MailOuts] (
	[mols] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[moCode] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[moOrder] [smallint] NOT NULL ,
	[moGroup] [varchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[moCCheckInDFrom] [smallint] NOT NULL ,
	[moCCheckInDTo] [smallint] NOT NULL ,
	[moCCheckOutDFrom] [smallint] NOT NULL ,
	[moCCheckOutDTo] [smallint] NOT NULL ,
	[moCBookDFrom] [smallint] NULL ,
	[moCBookDTo] [smallint] NULL ,
	[moCCheckIn] [bit] NOT NULL ,
	[moCInfo] [tinyint] NOT NULL ,
	[moCInvit] [tinyint] NOT NULL ,
	[moCBookCanc] [tinyint] NOT NULL ,
	[moCShow] [bit] NOT NULL ,
	[moCSale] [bit] NOT NULL ,
	[moCOnGroup] [tinyint] NOT NULL ,
	[moCRoomNumFrom] [varchar] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[moCRoomNumTo] [varchar] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[moCMarket] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[moCAgency] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[moCCountry] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[moA] [bit] NOT NULL 
) ON [PRIMARY]
GO

