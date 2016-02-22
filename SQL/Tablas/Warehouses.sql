if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Warehouses]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Warehouses]
GO

CREATE TABLE [dbo].[Warehouses] (
	[whID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[whN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[whA] [bit] NOT NULL 
) ON [PRIMARY]
GO

