if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GiftsLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GiftsLog]
GO

CREATE TABLE [dbo].[GiftsLog] (
	[ggID] [datetime] NOT NULL ,
	[ggChangedBy] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gggi] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ggPrice1] [money] NOT NULL ,
	[ggPrice2] [money] NOT NULL ,
	[ggPrice3] [money] NOT NULL ,
	[ggPrice4] [money] NOT NULL ,
	[ggPack] [bit] NOT NULL ,
	[gggc] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ggInven] [bit] NOT NULL ,
	[ggA] [bit] NOT NULL ,
	[ggWFolio] [bit] NOT NULL ,
	[ggWPax] [bit] NOT NULL ,
	[ggO] [int] NOT NULL 
) ON [PRIMARY]
GO

