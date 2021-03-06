if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExchangeRate]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ExchangeRate]
GO

CREATE TABLE [dbo].[ExchangeRate] (
	[exD] [datetime] NOT NULL ,
	[excu] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[exExchRate] [decimal](12, 8) NOT NULL, 
) ON [PRIMARY]
GO

