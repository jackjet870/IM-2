USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GuestsPromotions]    Script Date: 07/12/2014 11:52:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

SET ARITHABORT ON
GO

CREATE TABLE [dbo].[GuestsPromotions](
	[gpgu] [int] NOT NULL,
	[gpgr] [int] NOT NULL,
	[gpgi] [varchar](10) NOT NULL,
	[gpPromotion]  AS (CONVERT([varchar],[gpgr],(0))+[gpgi]),
	[gpPromotionOpera] [varchar](20) NOT NULL,
	[gpQty] [int] NOT NULL,
	[gpBalance] [int] NOT NULL,
	[gpD] [datetime] NOT NULL,
	[gpHotel] [varchar](10) NULL,
	[gpFolio] [varchar](15) NULL,
	[gpNotified] [bit] NOT NULL,
 CONSTRAINT [PK_GuestsPromotions] PRIMARY KEY CLUSTERED 
(
	[gpgr] ASC,
	[gpgi] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpgu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del recibo de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpgr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpgi'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la promoción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpPromotion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la promoción de Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpPromotionOpera'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cantidad asignada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Saldo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpBalance'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel de la reservación que consumió la promoción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpHotel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio de la reservación que consumió la promoción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpFolio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la promoción ya le fue notificada al cliente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions', @level2type=N'COLUMN',@level2name=N'gpNotified'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Promociones de huéspedes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsPromotions'
GO

ALTER TABLE [dbo].[GuestsPromotions]  WITH CHECK ADD  CONSTRAINT [FK_GuestsPromotions_Gifts] FOREIGN KEY([gpgi])
REFERENCES [dbo].[Gifts] ([giID])
GO

ALTER TABLE [dbo].[GuestsPromotions] CHECK CONSTRAINT [FK_GuestsPromotions_Gifts]
GO

ALTER TABLE [dbo].[GuestsPromotions]  WITH CHECK ADD  CONSTRAINT [FK_GuestsPromotions_GiftsReceipts] FOREIGN KEY([gpgr])
REFERENCES [dbo].[GiftsReceipts] ([grID])
GO

ALTER TABLE [dbo].[GuestsPromotions] CHECK CONSTRAINT [FK_GuestsPromotions_GiftsReceipts]
GO

ALTER TABLE [dbo].[GuestsPromotions]  WITH CHECK ADD  CONSTRAINT [FK_GuestsPromotions_Guests] FOREIGN KEY([gpgu])
REFERENCES [dbo].[Guests] ([guID])
GO

ALTER TABLE [dbo].[GuestsPromotions] CHECK CONSTRAINT [FK_GuestsPromotions_Guests]
GO

ALTER TABLE [dbo].[GuestsPromotions] ADD  CONSTRAINT [DF_GuestsPromotions_gpNotified]  DEFAULT ((0)) FOR [gpNotified]
GO


