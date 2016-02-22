if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ChargeCalculationTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ChargeCalculationTypes]
GO

CREATE TABLE [dbo].[ChargeCalculationTypes] (
	[caID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[caN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[caA] [bit] NOT NULL 
) ON [PRIMARY]
GO

