if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InvitsGifts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[InvitsGifts]
GO

CREATE TABLE [dbo].[InvitsGifts] (
	[iggu] [int] NOT NULL ,
	[iggi] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[igct] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[igQty] [int] NOT NULL ,
	[igAdults] [int] NOT NULL ,
	[igMinors] [int] NOT NULL ,
	[iggr] [int] NULL ,
	[igFolios] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[igPriceA] [money] NOT NULL ,
	[igPriceM] [money] NOT NULL ,
	[igComments] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InvitsGifts] WITH NOCHECK ADD 
	CONSTRAINT [PK_InvitsGifts] PRIMARY KEY  CLUSTERED 
	(
		[iggu],
		[iggi],
		[igct]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[InvitsGifts] WITH NOCHECK ADD 
	CONSTRAINT [DF__InvitsGift__igct__3EA749C6] DEFAULT ('MARKETING') FOR [igct],
	CONSTRAINT [DF__InvitsGif__igQty__3DB3258D] DEFAULT (0) FOR [igQty],
	CONSTRAINT [DF_InvitsGifts_igAdults_1] DEFAULT (0) FOR [igAdults],
	CONSTRAINT [DF_InvitsGifts_igMinors_1] DEFAULT (0) FOR [igMinors],
	CONSTRAINT [DF_InvitsGifts_igPriceA] DEFAULT (0) FOR [igPriceA],
	CONSTRAINT [DF_InvitsGifts_igPriceM] DEFAULT (0) FOR [igPriceM]
GO

