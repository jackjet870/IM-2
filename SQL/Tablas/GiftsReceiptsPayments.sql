if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GiftsReceiptsPayments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GiftsReceiptsPayments]
GO

CREATE TABLE [dbo].[GiftsReceiptsPayments] (
	[gygr] [int] NOT NULL ,
	[gypt] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gycu] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gyAmount] [money] NOT NULL ,
	[gyRefund] [money] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GiftsReceiptsPayments] WITH NOCHECK ADD 
	CONSTRAINT [PK_GiftsReceiptsPayments] PRIMARY KEY  CLUSTERED 
	(
		[gygr],
		[gypt],
		[gycu]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[GiftsReceiptsPayments] WITH NOCHECK ADD 
	CONSTRAINT [DF_GiftsReceiptsPayments_gyAmount] DEFAULT (0) FOR [gyAmount],
	CONSTRAINT [DF_GiftsReceiptsPayments_gyRefund] DEFAULT (0) FOR [gyRefund]
GO

ALTER TABLE [dbo].[GiftsReceiptsPayments] ADD 
	CONSTRAINT [FK_GiftsReceiptsPayments_Currencies] FOREIGN KEY 
	(
		[gycu]
	) REFERENCES [dbo].[Currencies] (
		[cuID]
	),
	CONSTRAINT [FK_GiftsReceiptsPayments_GiftsReceipts] FOREIGN KEY 
	(
		[gygr]
	) REFERENCES [dbo].[GiftsReceipts] (
		[grID]
	),
	CONSTRAINT [FK_GiftsReceiptsPayments_PaymentTypes] FOREIGN KEY 
	(
		[gypt]
	) REFERENCES [dbo].[PaymentTypes] (
		[ptID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Monto', N'user', N'dbo', N'table', N'GiftsReceiptsPayments', N'column', N'gyAmount'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la moneda', N'user', N'dbo', N'table', N'GiftsReceiptsPayments', N'column', N'gycu'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del recibo de regalos', N'user', N'dbo', N'table', N'GiftsReceiptsPayments', N'column', N'gygr'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del tipo de pago', N'user', N'dbo', N'table', N'GiftsReceiptsPayments', N'column', N'gypt'
GO
exec sp_addextendedproperty N'MS_Description', N'Monto devuelto', N'user', N'dbo', N'table', N'GiftsReceiptsPayments', N'column', N'gyRefund'


GO

