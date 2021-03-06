if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TourTimesByDay]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TourTimesByDay]
GO

CREATE TABLE [dbo].[TourTimesByDay] (
	[ttls] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ttsr] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ttDay] [tinyint] NOT NULL ,
	[ttT] [datetime] NOT NULL ,
	[ttPickUpT] [datetime] NOT NULL ,
	[ttMaxBooks] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TourTimesByDay] WITH NOCHECK ADD 
	CONSTRAINT [PK_TourTimesByDay] PRIMARY KEY  CLUSTERED 
	(
		[ttls],
		[ttsr],
		[ttDay],
		[ttT]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TourTimesByDay] ADD 
	CONSTRAINT [DF__TourTimes__ttMax__1D864D1D] DEFAULT (0) FOR [ttMaxBooks]
GO

