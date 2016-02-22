USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PermissionsLevels]    Script Date: 01/21/2014 17:07:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PermissionsLevels](
	[plID] [int] NOT NULL,
	[plN] [varchar](20) NOT NULL,
	[plA] [bit] NOT NULL,
 CONSTRAINT [PK_PermissionsLevels] PRIMARY KEY CLUSTERED 
(
	[plID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionsLevels', @level2type=N'COLUMN',@level2name=N'plID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionsLevels', @level2type=N'COLUMN',@level2name=N'plN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionsLevels', @level2type=N'COLUMN',@level2name=N'plA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de niveles de permisos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PermissionsLevels'
GO


