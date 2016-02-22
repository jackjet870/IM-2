USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[SaleTypesCategories]    Script Date: 11/15/2013 16:47:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaleTypesCategories]') AND type in (N'U'))
DROP TABLE [dbo].[SaleTypesCategories]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[SaleTypesCategories]    Script Date: 11/15/2013 16:47:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SaleTypesCategories](
	[stcID] [varchar](10) NOT NULL,
	[stcN] [varchar](50) NOT NULL,
	[stcA] [bit] NOT NULL,
 CONSTRAINT [PK_SaleTypesCategories] PRIMARY KEY CLUSTERED 
(
	[stcID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypesCategories', @level2type=N'COLUMN',@level2name=N'stcID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypesCategories', @level2type=N'COLUMN',@level2name=N'stcN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypesCategories', @level2type=N'COLUMN',@level2name=N'stcA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de categorías de tipos de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SaleTypesCategories'
GO

