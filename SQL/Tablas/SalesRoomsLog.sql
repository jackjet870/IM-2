if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesRoomsLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SalesRoomsLog]
GO

CREATE TABLE [dbo].[SalesRoomsLog] (
	[sqID] [datetime] NOT NULL ,
	[sqChangedBy] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[sqsr] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[sqGiftsRcptCloseD] [datetime] NOT NULL ,
	[sqCxCCloseD] [datetime] NOT NULL 
) ON [PRIMARY]
GO

