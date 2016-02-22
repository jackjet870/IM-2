USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GuestsSisturPromotions]    Script Date: 11/06/2015 17:31:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GuestsSisturPromotions](
	[gspRecnum] [int] IDENTITY(1,1) NOT NULL,
	[gspHotel] [varchar](10) NOT NULL,
	[gspFolio] [varchar](15) NOT NULL,
	[gspPromotion] [varchar](15) NOT NULL,
	[gspCoupon] [int] NOT NULL,
	[gspD] [datetime] NOT NULL,
	[gspUsed] [int] NOT NULL,
	[gspPax] [decimal](4, 1) NOT NULL,
	[gspAmount] [money] NOT NULL,
 CONSTRAINT [PK_GuestsSisturPromotions] PRIMARY KEY CLUSTERED 
(
	[gspHotel] ASC,
	[gspFolio] ASC,
	[gspPromotion] ASC,
	[gspCoupon] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspRecnum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspHotel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio de la reservación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspFolio'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la promoción de Sistur' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspPromotion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio del cupón de Sistur' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspCoupon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha del cupón' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de promociones usadas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspUsed'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de adultos y menores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspPax'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de la factura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions', @level2type=N'COLUMN',@level2name=N'gspAmount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Promociones de Sistur de huéspedes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestsSisturPromotions'
GO


