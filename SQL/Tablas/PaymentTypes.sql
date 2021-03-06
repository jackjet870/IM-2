if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_GiftsReceiptsPayments_PaymentTypes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[GiftsReceiptsPayments] DROP CONSTRAINT FK_GiftsReceiptsPayments_PaymentTypes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PaymentTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PaymentTypes]
GO

CREATE TABLE [dbo].[PaymentTypes] (
	[ptID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ptN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ptA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentTypes] WITH NOCHECK ADD 
	CONSTRAINT [PK_PaymentTypes] PRIMARY KEY  CLUSTERED 
	(
		[ptID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GiftsReceiptsPayments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[GiftsReceiptsPayments] ADD 
	CONSTRAINT [FK_GiftsReceiptsPayments_PaymentTypes] FOREIGN KEY 
	(
		[gypt]
	) REFERENCES [dbo].[PaymentTypes] (
		[ptID]
	)
GO
