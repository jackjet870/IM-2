USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Depts_deA]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Depts] DROP CONSTRAINT [DF_Depts_deA]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Depts]    Script Date: 11/14/2013 10:47:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Depts]') AND type in (N'U'))
DROP TABLE [dbo].[Depts]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Depts]    Script Date: 11/14/2013 10:47:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Depts](
	[deID] [varchar](10) NOT NULL,
	[deN] [varchar](50) NOT NULL,
	[deA] [bit] NOT NULL,
 CONSTRAINT [PK_Depts] PRIMARY KEY CLUSTERED 
(
	[deID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Depts', @level2type=N'COLUMN',@level2name=N'deID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Depts', @level2type=N'COLUMN',@level2name=N'deN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Depts', @level2type=N'COLUMN',@level2name=N'deA'
GO

ALTER TABLE [dbo].[Depts] ADD  CONSTRAINT [DF_Depts_deA]  DEFAULT (1) FOR [deA]
GO

