if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Hotels]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Hotels]
GO

CREATE TABLE [dbo].[Hotels] (
	[hoID] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[hoGroup] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[hoar] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[hoA] [bit] NOT NULL 
) ON [PRIMARY]
GO

