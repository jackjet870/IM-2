if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MealTickets]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MealTickets]
GO

CREATE TABLE [dbo].[MealTickets] (
	[meID] [int] IDENTITY (1, 1) NOT NULL ,
	[meD] [datetime] NOT NULL ,
	[megu] [int] NOT NULL ,
	[meQty] [int] NOT NULL ,
	[meType] [varchar] (10) COLLATE Modern_Spanish_CI_AS NULL ,
	[meAdults] [int] NOT NULL ,
	[meMinors] [int] NOT NULL ,
	[meFolios] [varchar] (30) COLLATE Modern_Spanish_CI_AS NULL ,
	[meTAdults] [money] NOT NULL ,
	[meTMinors] [money] NOT NULL ,
	[meComments] [varchar] (50) COLLATE Modern_Spanish_CI_AS NULL ,
	[mesr] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[meCanc] [bit] NOT NULL 
) ON [PRIMARY]
GO

