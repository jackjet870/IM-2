if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreditCardTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[CreditCardTypes]
GO

CREATE TABLE [dbo].[CreditCardTypes] (
	[ccID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ccN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ccA] [bit] NOT NULL ,
	[ccIsCreditCard] [bit] NOT NULL 
) ON [PRIMARY]
GO

