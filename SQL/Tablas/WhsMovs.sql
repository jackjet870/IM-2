if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WhsMovs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[WhsMovs]
GO

CREATE TABLE [dbo].[WhsMovs] (
	[wmID] [int] IDENTITY (1, 1) NOT NULL ,
	[wmD] [datetime] NOT NULL ,
	[wmwh] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[wmQty] [int] NOT NULL ,
	[wmgi] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[wmComments] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[wmpe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

