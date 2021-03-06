if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestStatus]
GO

CREATE TABLE [dbo].[GuestStatus] (
	[gsID] [nvarchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gsN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[gsA] [bit] NOT NULL ,
	[gsAgeStart] [tinyint] NOT NULL ,
	[gsAgeEnd] [tinyint] NOT NULL ,
	[gsMaxAuthGifts] [money] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GuestStatus] WITH NOCHECK ADD 
	CONSTRAINT [PK_GuestStatus] PRIMARY KEY  CLUSTERED 
	(
		[gsID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[GuestStatus] WITH NOCHECK ADD 
	CONSTRAINT [DF_GuestStatus_gsA] DEFAULT (0) FOR [gsA],
	CONSTRAINT [DF_GuestStatus_gsAgeStart] DEFAULT (0) FOR [gsAgeStart],
	CONSTRAINT [DF_GuestStatus_gsAgeEnd] DEFAULT (0) FOR [gsAgeEnd],
	CONSTRAINT [DF_GuestStatus_gsMaxAuthGifts] DEFAULT (0) FOR [gsMaxAuthGifts]
GO

