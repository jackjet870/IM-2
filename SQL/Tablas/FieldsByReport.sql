USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FieldsByReport_frColPosition]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FieldsByReport] DROP CONSTRAINT [DF_FieldsByReport_frColPosition]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FieldsByReport_frWidth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FieldsByReport] DROP CONSTRAINT [DF_FieldsByReport_frWidth]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FieldsByReport_frVisible]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FieldsByReport] DROP CONSTRAINT [DF_FieldsByReport_frVisible]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[FieldsByReport]    Script Date: 11/14/2013 09:47:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FieldsByReport]') AND type in (N'U'))
DROP TABLE [dbo].[FieldsByReport]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[FieldsByReport]    Script Date: 11/14/2013 09:47:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FieldsByReport](
	[frReport] [varchar](50) NOT NULL,
	[frColPosition] [smallint] NOT NULL,
	[frFieldName] [varchar](30) NOT NULL,
	[frHeading] [varchar](25) NULL,
	[frToolTipText] [varchar](50) NULL,
	[frWidth] [int] NOT NULL,
	[frVisible] [tinyint] NOT NULL,
	[frFormat] [varchar](20) NULL,
	[frOperation] [varchar](300) NULL,
	[frFontSize] [tinyint] NULL,
 CONSTRAINT [PK_FieldsByReport] PRIMARY KEY CLUSTERED 
(
	[frReport] ASC,
	[frColPosition] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FieldsByReport] ADD  CONSTRAINT [DF_FieldsByReport_frColPosition]  DEFAULT (0) FOR [frColPosition]
GO

ALTER TABLE [dbo].[FieldsByReport] ADD  CONSTRAINT [DF_FieldsByReport_frWidth]  DEFAULT (0) FOR [frWidth]
GO

ALTER TABLE [dbo].[FieldsByReport] ADD  CONSTRAINT [DF_FieldsByReport_frVisible]  DEFAULT (1) FOR [frVisible]
GO

