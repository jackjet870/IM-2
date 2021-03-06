USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[Locations]    Fecha de la secuencia de comandos: 08/08/2012 11:45:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Locations]') AND type in (N'U'))
DROP TABLE [dbo].[Locations]
GO
/****** Objeto:  Table [dbo].[Locations]    Fecha de la secuencia de comandos: 08/08/2012 11:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Locations](
	[loID] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[loN] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[lols] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[losr] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[loA] [bit] NOT NULL CONSTRAINT [DF_Locations_loA]  DEFAULT (1),
	[loPRCaptain] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[loRegen] [bit] NOT NULL DEFAULT (0),
	[loAnimacion] [bit] NOT NULL DEFAULT (0),
	[loFlyers] [bit] NOT NULL DEFAULT (0),
	[lolc] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[loID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'loID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'loN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Lead Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'lols'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'losr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'loA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán de PRs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'loPRCaptain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es una locación Regen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'loRegen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es una locación de animación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'loAnimacion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la locación maneja Flyers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'loFlyers'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la categoría de locaciones' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations', @level2type=N'COLUMN',@level2name=N'lolc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de locaciones' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Locations'