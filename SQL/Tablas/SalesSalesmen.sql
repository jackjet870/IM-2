if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesSalesmen]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SalesSalesmen]
GO

CREATE TABLE [dbo].[SalesSalesmen] (
	[smsa] [int] NOT NULL ,
	[smpe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[smSaleAmountOwn] [money] NOT NULL ,
	[smSaleAmountWith] [money] NOT NULL ,
	[smSale] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SalesSalesmen] WITH NOCHECK ADD 
	CONSTRAINT [PK_SalesSalesmen] PRIMARY KEY  CLUSTERED 
	(
		[smsa],
		[smpe]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SalesSalesmen] WITH NOCHECK ADD 
	CONSTRAINT [DF_SalesSalesmen_smSaleAmountOwn] DEFAULT (0) FOR [smSaleAmountOwn],
	CONSTRAINT [DF_SalesSalesmen_smSaleAmountWith] DEFAULT (0) FOR [smSaleAmountWith],
	CONSTRAINT [DF_SalesSalesmen_smSale] DEFAULT (1) FOR [smSale]
GO

ALTER TABLE [dbo].[SalesSalesmen] ADD 
	CONSTRAINT [FK_SalesSalesmen_Personnel] FOREIGN KEY 
	(
		[smpe]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	),
	CONSTRAINT [FK_SalesSalesmen_Sales] FOREIGN KEY 
	(
		[smsa]
	) REFERENCES [dbo].[Sales] (
		[saID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Clave del personal', N'user', N'dbo', N'table', N'SalesSalesmen', N'column', N'smpe'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la venta', N'user', N'dbo', N'table', N'SalesSalesmen', N'column', N'smsa'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si se debe de tomar en cuenta la venta en las estadísticas del vendedor', N'user', N'dbo', N'table', N'SalesSalesmen', N'column', N'smSale'
GO
exec sp_addextendedproperty N'MS_Description', N'Monto de la venta propio', N'user', N'dbo', N'table', N'SalesSalesmen', N'column', N'smSaleAmountOwn'
GO
exec sp_addextendedproperty N'MS_Description', N'Monto de la venta con otros vendedores', N'user', N'dbo', N'table', N'SalesSalesmen', N'column', N'smSaleAmountWith'


GO

