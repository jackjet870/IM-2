if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GiftsPacks]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GiftsPacks]
GO

CREATE TABLE [dbo].[GiftsPacks] (
	[gpPack] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gpgi] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gpQty] [int] NOT NULL 
) ON [PRIMARY]
GO

