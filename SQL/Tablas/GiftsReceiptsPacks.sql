USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsPacks_Gifts]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsPacks]'))
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [FK_GiftsReceiptsPacks_Gifts]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsPacks_Gifts_Pack]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsPacks]'))
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [FK_GiftsReceiptsPacks_Gifts_Pack]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsPacks_GiftsReceipts]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsPacks]'))
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [FK_GiftsReceiptsPacks_GiftsReceipts]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkQty]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkQty]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkAdults]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkAdults]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkMinors]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkMinors]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkPriceA]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkPriceA]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkPriceM]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkPriceM]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkInGiftsCard]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkInGiftsCard]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkCancel]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkCancel]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkInPVPPromo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkInPVPPromo]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkCancelPVPPromo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkCancelPVPPromo]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkInOpera]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkInOpera]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsPacks_gkCancelOpera]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsPacks] DROP CONSTRAINT [DF_GiftsReceiptsPacks_gkCancelOpera]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceiptsPacks]    Script Date: 11/14/2013 09:49:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsPacks]') AND type in (N'U'))
DROP TABLE [dbo].[GiftsReceiptsPacks]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceiptsPacks]    Script Date: 11/14/2013 09:49:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GiftsReceiptsPacks](
	[gkgr] [int] NOT NULL,
	[gkPack] [varchar](10) NOT NULL,
	[gkgi] [varchar](10) NOT NULL,
	[gkQty] [int] NOT NULL,
	[gkAdults] [int] NOT NULL,
	[gkMinors] [int] NOT NULL,
	[gkPriceA] [money] NOT NULL,
	[gkPriceM] [money] NOT NULL,
	[gkFolios] [varchar](100) NULL,
	[gkComments] [varchar](100) NULL,
	[gkInElectronicPurse] [bit] NOT NULL,
	[gkConsecutiveElectronicPurse] [int] NULL,
	[gkCancelElectronicPurse] [bit] NOT NULL,
	[gkInPVPPromo] [bit] NOT NULL,
	[gkCancelPVPPromo] [bit] NOT NULL,
	[gkInOpera] [bit] NOT NULL,
	[gkCancelOpera] [bit] NOT NULL,
 CONSTRAINT [PK_GiftsReceiptsPacks] PRIMARY KEY CLUSTERED 
(
	[gkgr] ASC,
	[gkPack] ASC,
	[gkgi] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del paquete de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkPack'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkgi'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad de adultos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkAdults'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad de menores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkMinors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Precio de adultos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkPriceA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Precio de menores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkPriceM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkFolios'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkComments'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se ha guardado en el monedero electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkInElectronicPurse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consecutivo del regalo en el monedero electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkConsecutiveElectronicPurse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo está cancelado en el monedero electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkCancelElectronicPurse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se ha guardado en PVP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkInPVPPromo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo está cancelado en PVP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkCancelPVPPromo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se ha guardado en Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkInOpera'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo está cancelado en Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsPacks', @level2type=N'COLUMN',@level2name=N'gkCancelOpera'
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsPacks_Gifts] FOREIGN KEY([gkgi])
REFERENCES [dbo].[Gifts] ([giID])
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] CHECK CONSTRAINT [FK_GiftsReceiptsPacks_Gifts]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsPacks_Gifts_Pack] FOREIGN KEY([gkPack])
REFERENCES [dbo].[Gifts] ([giID])
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] CHECK CONSTRAINT [FK_GiftsReceiptsPacks_Gifts_Pack]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsPacks_GiftsReceipts] FOREIGN KEY([gkgr])
REFERENCES [dbo].[GiftsReceipts] ([grID])
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] CHECK CONSTRAINT [FK_GiftsReceiptsPacks_GiftsReceipts]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkQty]  DEFAULT (0) FOR [gkQty]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkAdults]  DEFAULT (0) FOR [gkAdults]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkMinors]  DEFAULT (0) FOR [gkMinors]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkPriceA]  DEFAULT (0) FOR [gkPriceA]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkPriceM]  DEFAULT (0) FOR [gkPriceM]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkInGiftsCard]  DEFAULT (0) FOR [gkInElectronicPurse]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkCancel]  DEFAULT (0) FOR [gkCancelElectronicPurse]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkInPVPPromo]  DEFAULT ((0)) FOR [gkInPVPPromo]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkCancelPVPPromo]  DEFAULT ((0)) FOR [gkCancelPVPPromo]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkInOpera]  DEFAULT ((0)) FOR [gkInOpera]
GO

ALTER TABLE [dbo].[GiftsReceiptsPacks] ADD  CONSTRAINT [DF_GiftsReceiptsPacks_gkCancelOpera]  DEFAULT ((0)) FOR [gkCancelOpera]
GO

