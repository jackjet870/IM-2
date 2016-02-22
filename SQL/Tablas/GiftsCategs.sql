if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GiftsCategs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GiftsCategs]
GO

CREATE TABLE [dbo].[GiftsCategs] (
	[gcID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gcN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gcA] [bit] NOT NULL 
) ON [PRIMARY]
GO

