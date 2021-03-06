USE [OrigosVCPalace]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductsLegends_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductsLegends]'))
ALTER TABLE [dbo].[ProductsLegends] DROP CONSTRAINT [FK_ProductsLegends_Languages]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProductsLegends_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProductsLegends]'))
ALTER TABLE [dbo].[ProductsLegends] DROP CONSTRAINT [FK_ProductsLegends_Products]
GO
USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[ProductsLegends]    Fecha de la secuencia de comandos: 05/17/2012 10:45:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductsLegends]') AND type in (N'U'))
DROP TABLE [dbo].[ProductsLegends]
GO
USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[ProductsLegends]    Fecha de la secuencia de comandos: 05/17/2012 10:44:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductsLegends](
	[pxpr] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[pxla] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[pxText] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_ProductsLegends] PRIMARY KEY CLUSTERED 
(
	[pxpr] ASC,
	[pxla] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de producto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductsLegends', @level2type=N'COLUMN',@level2name=N'pxpr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de idioma' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductsLegends', @level2type=N'COLUMN',@level2name=N'pxla'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Texto de la leyenda' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductsLegends', @level2type=N'COLUMN',@level2name=N'pxText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de leyendas de productos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductsLegends'
GO
ALTER TABLE [dbo].[ProductsLegends]  WITH CHECK ADD  CONSTRAINT [FK_ProductsLegends_Languages] FOREIGN KEY([pxla])
REFERENCES [dbo].[Languages] ([laID])
GO
ALTER TABLE [dbo].[ProductsLegends] CHECK CONSTRAINT [FK_ProductsLegends_Languages]
GO
ALTER TABLE [dbo].[ProductsLegends]  WITH CHECK ADD  CONSTRAINT [FK_ProductsLegends_Products] FOREIGN KEY([pxpr])
REFERENCES [dbo].[Products] ([prID])
GO
ALTER TABLE [dbo].[ProductsLegends] CHECK CONSTRAINT [FK_ProductsLegends_Products]