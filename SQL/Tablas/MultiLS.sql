if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MultiLS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MultiLS]
GO

CREATE TABLE [dbo].[MultiLS] (
	[mllo] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[mlls] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

