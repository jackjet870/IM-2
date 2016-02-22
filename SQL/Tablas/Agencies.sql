USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Agencies_Clubs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agencies]'))
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [FK_Agencies_Clubs]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Agencies_Countries]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agencies]'))
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [FK_Agencies_Countries]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Agencies_Markets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agencies]'))
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [FK_Agencies_Markets]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Agencies_Reps]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agencies]'))
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [FK_Agencies_Reps]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Agencies_SegmentsByAgency]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agencies]'))
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [FK_Agencies_SegmentsByAgency]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Agencies_UnavailMots]') AND parent_object_id = OBJECT_ID(N'[dbo].[Agencies]'))
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [FK_Agencies_UnavailMots]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Agencies__agum__20C1E124]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF__Agencies__agum__20C1E124]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Agencies__agmk__21B6055D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF__Agencies__agmk__21B6055D]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Agencies__agShow__22AA2996]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF__Agencies__agShow__22AA2996]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Agencies__agSale__239E4DCF]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF__Agencies__agSale__239E4DCF]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Agencies__agA__24927208]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF__Agencies__agA__24927208]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Agencies_agco]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF_Agencies_agco]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Agencies__agList__05AEC38C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF__Agencies__agList__05AEC38C]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Agencies__agVeri__14F1071C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF__Agencies__agVeri__14F1071C]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Agencies_agIncludedTour]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF_Agencies_agIncludedTour]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Agencies_agN]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT [DF_Agencies_agN]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Agencies]    Script Date: 11/14/2013 10:46:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agencies]') AND type in (N'U'))
DROP TABLE [dbo].[Agencies]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Agencies]    Script Date: 11/14/2013 10:46:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Agencies](
	[agID] [varchar](35) NOT NULL,
	[agum] [tinyint] NOT NULL,
	[agmk] [varchar](10) NOT NULL,
	[agShowPay] [money] NOT NULL,
	[agSalePay] [money] NOT NULL,
	[agrp] [varchar](20) NULL,
	[agA] [bit] NOT NULL,
	[agco] [varchar](25) NOT NULL,
	[agList] [bit] NOT NULL,
	[agVerified] [bit] NOT NULL,
	[agse] [varchar](10) NULL,
	[agIncludedTour] [bit] NOT NULL,
	[agN] [varchar](100) NOT NULL,
	[agcl] [int] NULL,
 CONSTRAINT [PK_Agencies] PRIMARY KEY CLUSTERED 
(
	[agID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del motivo de indisponibilidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del mercado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agmk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto que se le paga a la agencia por show' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agShowPay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto que se le paga a la agencia por venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agSalePay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del agente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agrp'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del pais' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agco'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si aparece en la lista de agencias' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agList'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la agencia ha sido verificada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agVerified'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del segmento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si una agencia tiene habiitado el uso de tours incluidos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agIncludedTour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripcion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del club' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agencies', @level2type=N'COLUMN',@level2name=N'agcl'
GO

ALTER TABLE [dbo].[Agencies]  WITH CHECK ADD  CONSTRAINT [FK_Agencies_Clubs] FOREIGN KEY([agcl])
REFERENCES [dbo].[Clubs] ([clID])
GO

ALTER TABLE [dbo].[Agencies] CHECK CONSTRAINT [FK_Agencies_Clubs]
GO

ALTER TABLE [dbo].[Agencies]  WITH CHECK ADD  CONSTRAINT [FK_Agencies_Countries] FOREIGN KEY([agco])
REFERENCES [dbo].[Countries] ([coID])
GO

ALTER TABLE [dbo].[Agencies] CHECK CONSTRAINT [FK_Agencies_Countries]
GO

ALTER TABLE [dbo].[Agencies]  WITH CHECK ADD  CONSTRAINT [FK_Agencies_Markets] FOREIGN KEY([agmk])
REFERENCES [dbo].[Markets] ([mkID])
GO

ALTER TABLE [dbo].[Agencies] CHECK CONSTRAINT [FK_Agencies_Markets]
GO

ALTER TABLE [dbo].[Agencies]  WITH CHECK ADD  CONSTRAINT [FK_Agencies_Reps] FOREIGN KEY([agrp])
REFERENCES [dbo].[Reps] ([rpID])
GO

ALTER TABLE [dbo].[Agencies] CHECK CONSTRAINT [FK_Agencies_Reps]
GO

ALTER TABLE [dbo].[Agencies]  WITH CHECK ADD  CONSTRAINT [FK_Agencies_SegmentsByAgency] FOREIGN KEY([agse])
REFERENCES [dbo].[SegmentsByAgency] ([seID])
GO

ALTER TABLE [dbo].[Agencies] CHECK CONSTRAINT [FK_Agencies_SegmentsByAgency]
GO

ALTER TABLE [dbo].[Agencies]  WITH CHECK ADD  CONSTRAINT [FK_Agencies_UnavailMots] FOREIGN KEY([agum])
REFERENCES [dbo].[UnavailMots] ([umID])
GO

ALTER TABLE [dbo].[Agencies] CHECK CONSTRAINT [FK_Agencies_UnavailMots]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF__Agencies__agum__20C1E124]  DEFAULT (0) FOR [agum]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF__Agencies__agmk__21B6055D]  DEFAULT ('AGENCIES') FOR [agmk]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF__Agencies__agShow__22AA2996]  DEFAULT (0) FOR [agShowPay]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF__Agencies__agSale__239E4DCF]  DEFAULT (0) FOR [agSalePay]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF__Agencies__agA__24927208]  DEFAULT (1) FOR [agA]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF_Agencies_agco]  DEFAULT ('?') FOR [agco]
GO

ALTER TABLE [dbo].[Agencies] ADD  DEFAULT (0) FOR [agList]
GO

ALTER TABLE [dbo].[Agencies] ADD  DEFAULT (0) FOR [agVerified]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF_Agencies_agIncludedTour]  DEFAULT (1) FOR [agIncludedTour]
GO

ALTER TABLE [dbo].[Agencies] ADD  CONSTRAINT [DF_Agencies_agN]  DEFAULT ('') FOR [agN]
GO

