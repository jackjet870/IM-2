if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentPlaces]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentPlaces]
GO

CREATE TABLE [dbo].[PaymentPlaces] (
	[pcID] [varchar](10) NOT NULL,
	[pcN] [varchar](30) NOT NULL,
	[pcA] [bit] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentPlaces] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentPlaces] PRIMARY KEY  CLUSTERED 
	(
		[pcID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 

GO
