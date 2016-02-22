if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GiftsInventory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GiftsInventory]
GO

CREATE TABLE [dbo].[GiftsInventory] (
	[gvD] [datetime] NOT NULL ,
	[gvwh] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gvgi] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gvQty] [int] NOT NULL 
) ON [PRIMARY]
GO

