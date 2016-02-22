USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[HotelGroups]    Script Date: 01/16/2014 11:34:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[HotelGroups](
	[hgID] [varchar](10) NOT NULL,
	[hgN] [varchar](50) NOT NULL,
	[hgA] [bit] NOT NULL,
 CONSTRAINT [PK_HotelGroups] PRIMARY KEY CLUSTERED 
(
	[hgID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HotelGroups', @level2type=N'COLUMN',@level2name=N'hgID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HotelGroups', @level2type=N'COLUMN',@level2name=N'hgN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HotelGroups', @level2type=N'COLUMN',@level2name=N'hgA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de Grupos hoteleros' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HotelGroups'
GO


