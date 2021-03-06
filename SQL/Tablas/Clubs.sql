USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[Clubs]    Fecha de la secuencia de comandos: 04/05/2013 15:02:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clubs]') AND type in (N'U'))
DROP TABLE [dbo].[Clubs]
GO
/****** Objeto:  Table [dbo].[Clubs]    Fecha de la secuencia de comandos: 04/05/2013 15:02:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Clubs](
	[clID] [int] NOT NULL,
	[clN] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[clA] [bit] NOT NULL,
 CONSTRAINT [PK_Clubs] PRIMARY KEY CLUSTERED 
(
	[clID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Clubs', @level2type=N'COLUMN',@level2name=N'clID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Clubs', @level2type=N'COLUMN',@level2name=N'clN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Clubs', @level2type=N'COLUMN',@level2name=N'clA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de clubes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Clubs'