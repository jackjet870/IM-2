if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Payments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Payments]
GO

CREATE TABLE [dbo].[Payments] (
	[pasa] [int] NOT NULL ,
	[papt] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pacc] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

