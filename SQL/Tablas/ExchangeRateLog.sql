USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[ExchangeRateLog]    Script Date: 10/08/2014 09:27:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExchangeRateLog](
	[elID] [datetime] NOT NULL,
	[elcu] [varchar](10) NOT NULL,
	[elD] [datetime] NOT NULL,
	[elChangedBy] [varchar](10) NOT NULL,
	[elExchangeRate] [decimal](12, 8) NOT NULL,
 CONSTRAINT [PK_ExchangeRateLog] PRIMARY KEY CLUSTERED 
(
	[elID] ASC,
	[elcu] ASC,
	[elD] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha y hora de modificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExchangeRateLog', @level2type=N'COLUMN',@level2name=N'elID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de moneda' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExchangeRateLog', @level2type=N'COLUMN',@level2name=N'elcu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExchangeRateLog', @level2type=N'COLUMN',@level2name=N'elD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del usuario que realizó la modificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExchangeRateLog', @level2type=N'COLUMN',@level2name=N'elChangedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de cambio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExchangeRateLog', @level2type=N'COLUMN',@level2name=N'elExchangeRate'
GO

ALTER TABLE [dbo].[ExchangeRateLog]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeRateLog_Currencies] FOREIGN KEY([elcu])
REFERENCES [dbo].[Currencies] ([cuID])
GO

ALTER TABLE [dbo].[ExchangeRateLog] CHECK CONSTRAINT [FK_ExchangeRateLog_Currencies]
GO

ALTER TABLE [dbo].[ExchangeRateLog]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeRateLog_Personnel] FOREIGN KEY([elChangedBy])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[ExchangeRateLog] CHECK CONSTRAINT [FK_ExchangeRateLog_Personnel]
GO


