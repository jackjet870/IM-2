USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Prefixes]    Script Date: 12/14/2015 09:36:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Prefixes](
	[prxID] [nvarchar](50) NOT NULL,
	[prxTable] [nvarchar](100) NOT NULL,
	[prxN] [nvarchar](200) NOT NULL
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prefijo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Prefixes', @level2type=N'COLUMN',@level2name=N'prxID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Prefixes', @level2type=N'COLUMN',@level2name=N'prxTable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Prefixes', @level2type=N'COLUMN',@level2name=N'prxN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de prefijos de tablas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Prefixes'
GO


