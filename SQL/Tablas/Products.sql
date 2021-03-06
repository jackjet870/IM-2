USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[Products]    Fecha de la secuencia de comandos: 05/17/2012 10:05:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
DROP TABLE [dbo].[Products]
USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[Products]    Fecha de la secuencia de comandos: 05/17/2012 10:04:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Products](
	[prID] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[prN] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[prA] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[prID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Products', @level2type=N'COLUMN',@level2name=N'prID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripcion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Products', @level2type=N'COLUMN',@level2name=N'prN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Products', @level2type=N'COLUMN',@level2name=N'prA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de productos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Products'