USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geQty]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geQty]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geAdults_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geAdults_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geMinors_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geMinors_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_gePrice]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_gePrice]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_gePriceM_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_gePriceM_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geCharge]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geCharge]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geInGiftsCard]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geInGiftsCard]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geCancel]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geCancel]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geExtraAdults]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geExtraAdults]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geInPVPPromo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geInPVPPromo]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geCancelPVPPromo]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geCancelPVPPromo]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geInOpera]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geInOpera]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsC_geCancelOpera]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsC] DROP CONSTRAINT [DF_GiftsReceiptsC_geCancelOpera]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceiptsC]    Script Date: 11/14/2013 09:48:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsC]') AND type in (N'U'))
DROP TABLE [dbo].[GiftsReceiptsC]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceiptsC]    Script Date: 11/14/2013 09:48:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GiftsReceiptsC](
	[gegr] [int] NOT NULL,
	[gegi] [varchar](10) NOT NULL,
	[gect] [varchar](10) NOT NULL,
	[geQty] [int] NOT NULL,
	[geAdults] [int] NOT NULL,
	[geMinors] [int] NOT NULL,
	[geFolios] [varchar](100) NULL,
	[gePriceA] [money] NOT NULL,
	[gePriceM] [money] NOT NULL,
	[geCharge] [money] NOT NULL,
	[gecxc] [varchar](10) NULL,
	[geComments] [varchar](25) NULL,
	[geInElectronicPurse] [bit] NOT NULL,
	[geConsecutiveElectronicPurse] [int] NULL,
	[geCancelElectronicPurse] [bit] NOT NULL,
	[geExtraAdults] [int] NOT NULL,
	[geInPVPPromo] [bit] NOT NULL,
	[geCancelPVPPromo] [bit] NOT NULL,
	[geInOpera] [bit] NOT NULL,
	[geCancelOpera] [bit] NOT NULL,
 CONSTRAINT [PK_GiftsReceiptsC] PRIMARY KEY CLUSTERED 
(
	[gegr] ASC,
	[gegi] ASC,
	[gect] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del recibo de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'gegr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'gegi'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de Cargar A' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'gect'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de adultos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geAdults'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de menores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geMinors'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geFolios'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Precio de adultos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'gePriceA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Precio de menores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'gePriceM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geComments'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se ha guardado en el monedero electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geInElectronicPurse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consecutivo del regalo en el monedero electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geConsecutiveElectronicPurse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo está cancelado en el monedero electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geCancelElectronicPurse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de adultos extra' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geExtraAdults'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se ha guardado en PVP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geInPVPPromo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo está cancelado en PVP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geCancelPVPPromo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo se ha guardado en Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geInOpera'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el regalo está cancelado en Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsC', @level2type=N'COLUMN',@level2name=N'geCancelOpera'
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geQty]  DEFAULT (0) FOR [geQty]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geAdults_1]  DEFAULT (0) FOR [geAdults]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geMinors_1]  DEFAULT (0) FOR [geMinors]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_gePrice]  DEFAULT (0) FOR [gePriceA]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_gePriceM_1]  DEFAULT (0) FOR [gePriceM]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geCharge]  DEFAULT (0) FOR [geCharge]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geInGiftsCard]  DEFAULT (0) FOR [geInElectronicPurse]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geCancel]  DEFAULT (0) FOR [geCancelElectronicPurse]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geExtraAdults]  DEFAULT (0) FOR [geExtraAdults]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geInPVPPromo]  DEFAULT ((0)) FOR [geInPVPPromo]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geCancelPVPPromo]  DEFAULT ((0)) FOR [geCancelPVPPromo]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geInOpera]  DEFAULT ((0)) FOR [geInOpera]
GO

ALTER TABLE [dbo].[GiftsReceiptsC] ADD  CONSTRAINT [DF_GiftsReceiptsC_geCancelOpera]  DEFAULT ((0)) FOR [geCancelOpera]
GO

