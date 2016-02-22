USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsLog_ChargeTo]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsLog]'))
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [FK_GiftsReceiptsLog_ChargeTo]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsLog_Currencies]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsLog]'))
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [FK_GiftsReceiptsLog_Currencies]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsLog_GiftsReceipts]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsLog]'))
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [FK_GiftsReceiptsLog_GiftsReceipts]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsLog_PaymentTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsLog]'))
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [FK_GiftsReceiptsLog_PaymentTypes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsLog_Personnel_Host]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsLog]'))
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [FK_GiftsReceiptsLog_Personnel_Host]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsReceiptsLog_Personnel_PR]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsLog]'))
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [FK_GiftsReceiptsLog_Personnel_PR]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsLog_goDeposit]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [DF_GiftsReceiptsLog_goDeposit]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsLog_goBurned]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [DF_GiftsReceiptsLog_goBurned]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsLog_goTaxiOut]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [DF_GiftsReceiptsLog_goTaxiOut]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsLog_goTotalGifts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [DF_GiftsReceiptsLog_goTotalGifts]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsLog_goCXCGifts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [DF_GiftsReceiptsLog_goCXCGifts]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsLog_goTaxiOutDiff]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [DF_GiftsReceiptsLog_goTaxiOutDiff]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftsReceiptsLog_gopt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftsReceiptsLog] DROP CONSTRAINT [DF_GiftsReceiptsLog_gopt]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceiptsLog]    Script Date: 11/14/2013 10:47:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftsReceiptsLog]') AND type in (N'U'))
DROP TABLE [dbo].[GiftsReceiptsLog]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsReceiptsLog]    Script Date: 11/14/2013 10:47:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GiftsReceiptsLog](
	[goID] [datetime] NOT NULL,
	[gogr] [int] NOT NULL,
	[goD] [datetime] NOT NULL,
	[goHost] [varchar](10) NOT NULL,
	[goDeposit] [money] NOT NULL,
	[goBurned] [money] NOT NULL,
	[gocu] [varchar](10) NOT NULL,
	[goCXCPRDeposit] [money] NOT NULL,
	[goTaxiOut] [money] NOT NULL,
	[goTotalGifts] [money] NOT NULL,
	[goct] [varchar](10) NOT NULL,
	[gope] [varchar](10) NOT NULL,
	[goCXCGifts] [money] NOT NULL,
	[goCXCAdj] [money] NOT NULL,
	[goTaxiOutDiff] [money] NOT NULL,
	[goChangedBy] [varchar](10) NOT NULL,
	[gopt] [varchar](10) NOT NULL,
 CONSTRAINT [PK_GiftsReceiptsLog] PRIMARY KEY CLUSTERED 
(
	[goID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'fecha y hora de la modificacion en el recibo de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'gogr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'fecha del recibo de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Host' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de Deposito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goDeposit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del Deposito Quemado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goBurned'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la moneda' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'gocu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del CxC del deposito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goCXCPRDeposit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del Taxi de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goTaxiOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto total del costo de los regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goTotalGifts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave con "cargo a"  ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goct'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR"  ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'gope'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del CxC de regalos"  ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goCXCGifts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto del Ajuste del CxC de regalos"  ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goCXCAdj'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de la diferencia del Taxi de Salida"  ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goTaxiOutDiff'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del usuario que hizo el cambio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'goChangedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'forma de pago de depósitos de un recibo de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsReceiptsLog', @level2type=N'COLUMN',@level2name=N'gopt'
GO

ALTER TABLE [dbo].[GiftsReceiptsLog]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsLog_ChargeTo] FOREIGN KEY([goct])
REFERENCES [dbo].[ChargeTo] ([ctID])
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] CHECK CONSTRAINT [FK_GiftsReceiptsLog_ChargeTo]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsLog_Currencies] FOREIGN KEY([gocu])
REFERENCES [dbo].[Currencies] ([cuID])
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] CHECK CONSTRAINT [FK_GiftsReceiptsLog_Currencies]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsLog_GiftsReceipts] FOREIGN KEY([gogr])
REFERENCES [dbo].[GiftsReceipts] ([grID])
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] CHECK CONSTRAINT [FK_GiftsReceiptsLog_GiftsReceipts]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsLog_PaymentTypes] FOREIGN KEY([gopt])
REFERENCES [dbo].[PaymentTypes] ([ptID])
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] CHECK CONSTRAINT [FK_GiftsReceiptsLog_PaymentTypes]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsLog_Personnel_Host] FOREIGN KEY([goHost])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] CHECK CONSTRAINT [FK_GiftsReceiptsLog_Personnel_Host]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog]  WITH CHECK ADD  CONSTRAINT [FK_GiftsReceiptsLog_Personnel_PR] FOREIGN KEY([gope])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] CHECK CONSTRAINT [FK_GiftsReceiptsLog_Personnel_PR]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] ADD  CONSTRAINT [DF_GiftsReceiptsLog_goDeposit]  DEFAULT (0) FOR [goDeposit]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] ADD  CONSTRAINT [DF_GiftsReceiptsLog_goBurned]  DEFAULT (0) FOR [goBurned]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] ADD  CONSTRAINT [DF_GiftsReceiptsLog_goTaxiOut]  DEFAULT (0) FOR [goTaxiOut]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] ADD  CONSTRAINT [DF_GiftsReceiptsLog_goTotalGifts]  DEFAULT (0) FOR [goTotalGifts]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] ADD  CONSTRAINT [DF_GiftsReceiptsLog_goCXCGifts]  DEFAULT (0) FOR [goCXCGifts]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] ADD  CONSTRAINT [DF_GiftsReceiptsLog_goTaxiOutDiff]  DEFAULT (0) FOR [goTaxiOutDiff]
GO

ALTER TABLE [dbo].[GiftsReceiptsLog] ADD  CONSTRAINT [DF_GiftsReceiptsLog_gopt]  DEFAULT ('CS') FOR [gopt]
GO

