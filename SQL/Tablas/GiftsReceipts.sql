USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceipts_PaymentTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceipts]'))
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [FK_GiftsReceipts_PaymentTypes]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grExchange]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grExchange]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grPax]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grPax]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grDeposit]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grDeposit]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grDepositB]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grDepositB]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grcu]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grcu]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grcu1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grcu1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__GiftsRece__grCxC__2E31B632]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF__GiftsRece__grCxC__2E31B632]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grExchangeRate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grExchangeRate]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__GiftsRece__grMax__2F25DA6B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF__GiftsRece__grMax__2F25DA6B]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__GiftsRece__grcxc__3019FEA4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF__GiftsRece__grcxc__3019FEA4]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__GiftsRece__grcxc__310E22DD]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF__GiftsRece__grcxc__310E22DD]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grTaxiIn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grTaxiIn]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grTaxiOut]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grTaxiOut]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grCancel]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grCancel]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grClosed]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grClosed]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Giftsrece__grTax__550B8C31]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF__Giftsrece__grTax__550B8C31]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceipts_grpt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceipts] DROP CONSTRAINT [DF_GiftsReceipts_grpt]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceipts]    Script Date: 11/14/2013 10:46:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftsReceipts]') AND type in (N'U'))
DROP TABLE [dbo].[GiftsReceipts]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceipts]    Script Date: 11/14/2013 10:46:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GiftsReceipts](
	[grID] [int] IDENTITY(1,1) NOT NULL,
	[grNum] [varchar](10) NULL,
	[grD] [datetime] NOT NULL,
	[grgu] [int] NULL,
	[grExchange] [bit] NOT NULL,
	[grGuest] [varchar](20) NULL,
	[grPax] [decimal](4, 1) NOT NULL,
	[grHotel] [varchar](20) NULL,
	[grRoomNum] [varchar](6) NULL,
	[grpe] [varchar](10) NULL,
	[grlo] [varchar](10) NULL,
	[grls] [varchar](10) NULL,
	[grsr] [varchar](10) NULL,
	[grWh] [varchar](10) NULL,
	[grMemberNum] [varchar](10) NULL,
	[grHost] [varchar](10) NOT NULL,
	[grComments] [nvarchar](100) NULL,
	[grDeposit] [money] NOT NULL,
	[grDepositTwisted] [money] NOT NULL,
	[grcu] [varchar](10) NOT NULL,
	[grcxcPRDeposit] [money] NOT NULL,
	[grcucxcPRDeposit] [varchar](10) NOT NULL,
	[grCxCClosed] [bit] NOT NULL,
	[grExchangeRate] [money] NOT NULL,
	[grct] [varchar](10) NOT NULL,
	[grMaxAuthGifts] [money] NOT NULL,
	[grcxcGifts] [money] NOT NULL,
	[grcxcAdj] [money] NOT NULL,
	[grcxcComments] [varchar](50) NULL,
	[grTaxiIn] [money] NOT NULL,
	[grTaxiOut] [money] NOT NULL,
	[grCancel] [bit] NOT NULL,
	[grClosed] [bit] NOT NULL,
	[grCxCAppD] [datetime] NULL,
	[grTaxiOutDiff] [money] NOT NULL,
	[grGuest2] [varchar](20) NULL,
	[grpt] [varchar](10) NOT NULL,
 CONSTRAINT [PK_GiftsReceipts] PRIMARY KEY CLUSTERED 
(
	[grID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica la forma de pago del depósito en un recibo de regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceipts', @level2type=N'COLUMN',@level2name=N'grpt'
GO

ALTER TABLE [dbo].[GiftsReceipts]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceipts_PaymentTypes] FOREIGN KEY([grpt])
REFERENCES [dbo].[PaymentTypes] ([ptID])
GO

ALTER TABLE [dbo].[GiftsReceipts] CHECK CONSTRAINT [FK_GiftsReceipts_PaymentTypes]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grExchange]  DEFAULT (0) FOR [grExchange]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grPax]  DEFAULT (0) FOR [grPax]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grDeposit]  DEFAULT (0) FOR [grDeposit]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grDepositB]  DEFAULT (0) FOR [grDepositTwisted]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grcu]  DEFAULT ('US') FOR [grcu]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grcu1]  DEFAULT ('US') FOR [grcucxcPRDeposit]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF__GiftsRece__grCxC__2E31B632]  DEFAULT (0) FOR [grCxCClosed]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grExchangeRate]  DEFAULT (0) FOR [grExchangeRate]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF__GiftsRece__grMax__2F25DA6B]  DEFAULT (0) FOR [grMaxAuthGifts]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF__GiftsRece__grcxc__3019FEA4]  DEFAULT (0) FOR [grcxcGifts]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF__GiftsRece__grcxc__310E22DD]  DEFAULT (0) FOR [grcxcAdj]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grTaxiIn]  DEFAULT (0) FOR [grTaxiIn]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grTaxiOut]  DEFAULT (0) FOR [grTaxiOut]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grCancel]  DEFAULT (0) FOR [grCancel]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grClosed]  DEFAULT (0) FOR [grClosed]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  DEFAULT (0) FOR [grTaxiOutDiff]
GO

ALTER TABLE [dbo].[GiftsReceipts] ADD  CONSTRAINT [DF_GiftsReceipts_grpt]  DEFAULT ('CS') FOR [grpt]
GO

