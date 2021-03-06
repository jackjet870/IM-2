if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MealTicketTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[MealTicketTypes]
GO

CREATE TABLE [dbo].[MealTicketTypes] (
	[myID] [varchar] (10) COLLATE Modern_Spanish_CI_AS NOT NULL ,
	[myN] [varchar] (30) COLLATE Modern_Spanish_CI_AS NOT NULL ,
	[myWPax] [bit] NOT NULL ,
	[myPriceA] [money] NOT NULL ,
	[myPriceM] [money] NOT NULL 
) ON [PRIMARY]
GO

