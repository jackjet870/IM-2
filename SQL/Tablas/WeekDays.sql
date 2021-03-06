if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WeekDays]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[WeekDays]
GO

CREATE TABLE [dbo].[WeekDays] (
	[wdDay] [tinyint] NOT NULL ,
	[wdla] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[wdN] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WeekDays] WITH NOCHECK ADD 
	CONSTRAINT [PK_WeekDays] PRIMARY KEY  CLUSTERED 
	(
		[wdDay],
		[wdla]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

