USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[ReportsTexts]    Fecha de la secuencia de comandos: 06/12/2013 09:33:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportsTexts]') AND type in (N'U'))
DROP TABLE [dbo].[ReportsTexts]
GO
/****** Objeto:  Table [dbo].[ReportsTexts]    Fecha de la secuencia de comandos: 06/12/2013 09:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportsTexts](
	[reReport] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[reTextId] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[rela] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[reO] [int] NOT NULL,
	[reIsSection] [bit] NOT NULL,
	[reText] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_ReportsTexts] PRIMARY KEY CLUSTERED 
(
	[reReport] ASC,
	[reTextId] ASC,
	[rela] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del reporte' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportsTexts', @level2type=N'COLUMN',@level2name=N'reReport'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del texto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportsTexts', @level2type=N'COLUMN',@level2name=N'reTextId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del idioma' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportsTexts', @level2type=N'COLUMN',@level2name=N'rela'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Texto del reporte de la promoción en el idioma especificado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportsTexts', @level2type=N'COLUMN',@level2name=N'reText'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Orden' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportsTexts', @level2type=N'COLUMN',@level2name=N'reO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si en verdad representa una seccion del reporte' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportsTexts', @level2type=N'COLUMN',@level2name=N'reIsSection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que contiene los textos de los reportes en los diferentes idiomas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReportsTexts'
GO
ALTER TABLE [dbo].[ReportsTexts]  WITH CHECK ADD  CONSTRAINT [FK_ReportsTexts_Languages] FOREIGN KEY([rela])
REFERENCES [dbo].[Languages] ([laID])
GO
ALTER TABLE [dbo].[ReportsTexts] CHECK CONSTRAINT [FK_ReportsTexts_Languages]