USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Gifts]    Script Date: 06/12/2014 09:55:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Gifts](
	[giID] [varchar](10) NOT NULL,
	[giN] [varchar](50) NOT NULL,
	[giShortN] [varchar](10) NULL,
	[giPrice1] [money] NOT NULL,
	[giPrice2] [money] NOT NULL,
	[giPrice3] [money] NOT NULL,
	[giPrice4] [money] NOT NULL,
	[giPack] [bit] NOT NULL,
	[gigc] [varchar](10) NOT NULL,
	[giInven] [bit] NOT NULL,
	[giA] [bit] NOT NULL,
	[giWFolio] [bit] NOT NULL,
	[giWPax] [bit] NOT NULL,
	[giO] [int] NOT NULL,
	[giUnpack] [bit] NOT NULL,
	[giMaxQty] [int] NOT NULL,
	[giMonetary] [bit] NOT NULL,
	[giAmount] [money] NOT NULL,
	[giProductGiftsCard] [varchar](15) NULL,
	[giCountInPackage] [bit] NOT NULL,
	[giCountInCancelledReceipts] [bit] NOT NULL,
	[gipr] [varchar](10) NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[Gifts] ADD [giPVPPromotion] [varchar](15) NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la promoción de PVP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPVPPromotion'
ALTER TABLE [dbo].[Gifts] ADD [giWCost] [bit] NOT NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo tiene Costo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giWCost'
ALTER TABLE [dbo].[Gifts] ADD [giPublicPrice] [money] NOT NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica el precio público del regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPublicPrice'
ALTER TABLE [dbo].[Gifts] ADD [giAmountModifiable] [bit] NOT NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el monto del regalo es modificable en los recibos de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giAmountModifiable'
SET ANSI_PADDING ON
ALTER TABLE [dbo].[Gifts] ADD [giOperaTransactionType] [varchar](10) NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de transacción de Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giOperaTransactionType'
ALTER TABLE [dbo].[Gifts] ADD [giSale] [bit] NOT NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo es un concepto de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giSale'
ALTER TABLE [dbo].[Gifts] ADD [giPromotionOpera] [varchar](20) NULL
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la promoción de Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPromotionOpera'
/****** Object:  Index [PK_Gifts]    Script Date: 06/12/2014 09:55:51 ******/
ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [PK_Gifts] PRIMARY KEY CLUSTERED 
(
	[giID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre corto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giShortN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costo de adultos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPrice1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costo de menores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPrice2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costo de adultos para empleados' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPrice3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costo de menores para empleados' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPrice4'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo es un paquete' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la categoría del regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'gigc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo es inventariable' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giInven'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalos maneja folios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giWFolio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo maneja Pax' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giWPax'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orden' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si un paquete de regalos se debe desglosar' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giUnpack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad máxima' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giMaxQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo es monetario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giMonetary'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del producto en el sistema de Monedero Electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giProductGiftsCard'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se debe contar si el regalo forma parte de un paquete de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giCountInPackage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se debe contar si el regalo está en un recibo cancelado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giCountInCancelledReceipts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de producto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'gipr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la promoción de PVP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPVPPromotion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo tiene Costo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giWCost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica el precio público del regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPublicPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el monto del regalo es modificable en los recibos de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giAmountModifiable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de transacción de Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giOperaTransactionType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo es un concepto de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giSale'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la promoción de Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts', @level2type=N'COLUMN',@level2name=N'giPromotionOpera'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Gifts'
GO

ALTER TABLE [dbo].[Gifts]  WITH CHECK ADD  CONSTRAINT [FK_Gifts_Products] FOREIGN KEY([gipr])
REFERENCES [dbo].[Products] ([prID])
GO

ALTER TABLE [dbo].[Gifts] CHECK CONSTRAINT [FK_Gifts_Products]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giID]  DEFAULT (0) FOR [giID]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giPrice1]  DEFAULT (0) FOR [giPrice1]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giPrice2]  DEFAULT (0) FOR [giPrice2]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giPrice3]  DEFAULT (0) FOR [giPrice3]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giPrice4_1]  DEFAULT (0) FOR [giPrice4]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giPack]  DEFAULT (0) FOR [giPack]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_gigc]  DEFAULT ('ITEMS') FOR [gigc]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giInven]  DEFAULT (1) FOR [giInven]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giA]  DEFAULT (0) FOR [giA]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giWFolio]  DEFAULT (0) FOR [giWFolio]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giWpax]  DEFAULT (0) FOR [giWPax]
GO

ALTER TABLE [dbo].[Gifts] ADD  DEFAULT (10000) FOR [giO]
GO

ALTER TABLE [dbo].[Gifts] ADD  DEFAULT (0) FOR [giUnpack]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giMaxQty]  DEFAULT (0) FOR [giMaxQty]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giMonetary]  DEFAULT (0) FOR [giMonetary]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giAmount]  DEFAULT (0) FOR [giAmount]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giCountInPackage]  DEFAULT (1) FOR [giCountInPackage]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giCountInCancelledReceipts]  DEFAULT (1) FOR [giCountInCancelledReceipts]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giWCost]  DEFAULT ((0)) FOR [giWCost]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giPublicPrice]  DEFAULT ((0)) FOR [giPublicPrice]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giAmountModifiable]  DEFAULT ((0)) FOR [giAmountModifiable]
GO

ALTER TABLE [dbo].[Gifts] ADD  CONSTRAINT [DF_Gifts_giSale]  DEFAULT ((0)) FOR [giSale]
GO


