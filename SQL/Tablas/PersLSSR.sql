if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PersLSSR]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PersLSSR]
GO

CREATE TABLE [dbo].[PersLSSR] (
	[plpe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[plLSSR] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[plLSSRID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

