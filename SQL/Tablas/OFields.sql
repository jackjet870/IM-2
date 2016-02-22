if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OFields]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[OFields]
GO

CREATE TABLE [dbo].[OFields] (
	[ofField] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ofla] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ofLabel] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ofGridHead] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ofToolTip] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

