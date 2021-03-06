if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TourTimes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TourTimes]
GO

CREATE TABLE [dbo].[TourTimes] (
	[ttls] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ttsr] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ttT] [datetime] NOT NULL ,
	[ttPickUpT] [datetime] NOT NULL ,
	[ttMaxBooks] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TourTimes] WITH NOCHECK ADD 
	CONSTRAINT [PK_TourTimesSR] PRIMARY KEY  CLUSTERED 
	(
		[ttls],
		[ttsr],
		[ttT]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TourTimes] ADD 
	CONSTRAINT [DF_TourTimes_ttMaxBooks] DEFAULT (0) FOR [ttMaxBooks]
GO

