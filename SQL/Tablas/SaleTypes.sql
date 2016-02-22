USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[SaleTypes]    Fecha de la secuencia de comandos: 05/30/2013 13:26:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SaleTypes](
	[stID] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[stN] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[stUpdate] [bit] NOT NULL CONSTRAINT [DF_SaleTypes_stUpdate]  DEFAULT (0),
	[stA] [bit] NOT NULL CONSTRAINT [DF_SaleTypes_stA]  DEFAULT (1),
 CONSTRAINT [PK_SaleTypes] PRIMARY KEY CLUSTERED 
(
	[stID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypes', @level2type=N'COLUMN',@level2name=N'stID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypes', @level2type=N'COLUMN',@level2name=N'stN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es un tipo de venta que actualiza a otro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypes', @level2type=N'COLUMN',@level2name=N'stUpdate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypes', @level2type=N'COLUMN',@level2name=N'stA'