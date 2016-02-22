if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PersStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PersStatus]
GO

CREATE TABLE [dbo].[PersStatus] (
	[psID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[psN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

