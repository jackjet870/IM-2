USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ChargeTo_ChargeCalculationTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[ChargeTo]'))
ALTER TABLE [dbo].[ChargeTo] DROP CONSTRAINT [FK_ChargeTo_ChargeCalculationTypes]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ChargeTo_ctPrice]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ChargeTo] DROP CONSTRAINT [DF_ChargeTo_ctPrice]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ChargeTo__ctCalc__4CAB505A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ChargeTo] DROP CONSTRAINT [DF__ChargeTo__ctCalc__4CAB505A]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ChargeTo_ctIsCxC]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ChargeTo] DROP CONSTRAINT [DF_ChargeTo_ctIsCxC]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[ChargeTo]    Script Date: 11/14/2013 09:47:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChargeTo]') AND type in (N'U'))
DROP TABLE [dbo].[ChargeTo]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[ChargeTo]    Script Date: 11/14/2013 09:47:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ChargeTo](
	[ctID] [varchar](10) NOT NULL,
	[ctPrice] [tinyint] NOT NULL,
	[ctCalcType] [varchar](10) NOT NULL,
	[ctIsCxC] [bit] NOT NULL,
 CONSTRAINT [PK_ChargeTo] PRIMARY KEY CLUSTERED 
(
	[ctID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChargeTo', @level2type=N'COLUMN',@level2name=N'ctID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Precio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChargeTo', @level2type=N'COLUMN',@level2name=N'ctPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de cálculo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChargeTo', @level2type=N'COLUMN',@level2name=N'ctCalcType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es una CxC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChargeTo', @level2type=N'COLUMN',@level2name=N'ctIsCxC'
GO

ALTER TABLE [dbo].[ChargeTo]  WITH CHECK ADD  CONSTRAINT [FK_ChargeTo_ChargeCalculationTypes] FOREIGN KEY([ctCalcType])
REFERENCES [dbo].[ChargeCalculationTypes] ([caID])
GO

ALTER TABLE [dbo].[ChargeTo] CHECK CONSTRAINT [FK_ChargeTo_ChargeCalculationTypes]
GO

ALTER TABLE [dbo].[ChargeTo] ADD  CONSTRAINT [DF_ChargeTo_ctPrice]  DEFAULT (1) FOR [ctPrice]
GO

ALTER TABLE [dbo].[ChargeTo] ADD  DEFAULT ('Z') FOR [ctCalcType]
GO

ALTER TABLE [dbo].[ChargeTo] ADD  CONSTRAINT [DF_ChargeTo_ctIsCxC]  DEFAULT ((0)) FOR [ctIsCxC]
GO

