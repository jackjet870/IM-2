USE [OrigosVCPalace]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeadSources_Areas]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeadSources]'))
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT [FK_LeadSources_Areas]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeadSources_Personnel_Boss]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeadSources]'))
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT [FK_LeadSources_Personnel_Boss]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeadSources_Programs]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeadSources]'))
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT [FK_LeadSources_Programs]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeadSources_Regions]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeadSources]'))
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT [FK_LeadSources_Regions]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeadSources_SegmentsByLeadSource]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeadSources]'))
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT [FK_LeadSources_SegmentsByLeadSource]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LeadSources_Zones]') AND parent_object_id = OBJECT_ID(N'[dbo].[LeadSources]'))
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT [FK_LeadSources_Zones]
GO
USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[LeadSources]    Fecha de la secuencia de comandos: 08/08/2012 11:46:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LeadSources]') AND type in (N'U'))
DROP TABLE [dbo].[LeadSources]
GO
/****** Objeto:  Table [dbo].[LeadSources]    Fecha de la secuencia de comandos: 08/08/2012 11:46:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LeadSources](
	[lsID] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[lsN] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[lspg] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[lsRooms] [int] NOT NULL,
	[lssr] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsar] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsrg] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsHoursDif] [smallint] NOT NULL CONSTRAINT [DF_LeadSources_lsHoursDif]  DEFAULT (0),
	[lsPayInOut] [bit] NOT NULL CONSTRAINT [DF_LeadSources_lsPayInOut]  DEFAULT (0),
	[lsPayWalkOut] [bit] NOT NULL CONSTRAINT [DF_LeadSources_lsPayInOut1]  DEFAULT (0),
	[lsRunTrans] [bit] NOT NULL CONSTRAINT [DF_LeadSources_RunTrans_1]  DEFAULT (0),
	[lsTransDT] [datetime] NOT NULL CONSTRAINT [DF_LeadSources_TransDT_1]  DEFAULT (getdate()),
	[lsTransBridgeDT] [datetime] NOT NULL CONSTRAINT [DF_LeadSources_lsTransDT1]  DEFAULT (getdate()),
	[lsEfficiencyTh] [money] NOT NULL CONSTRAINT [DF_LeadSources_EfficiencyTh_1]  DEFAULT (1200),
	[lsShowFactorTh] [money] NOT NULL CONSTRAINT [DF_LeadSources_ShowFactorTh_1]  DEFAULT (0.6),
	[lsBookFactorTh] [money] NOT NULL CONSTRAINT [DF_LeadSources_BookFactorTh_1]  DEFAULT (55),
	[lsShowsTh] [int] NOT NULL CONSTRAINT [DF_LeadSources_ShowsTh_1]  DEFAULT (10),
	[lsDBMaint] [bit] NOT NULL CONSTRAINT [DF_LeadSources_Maintenance_1]  DEFAULT (0),
	[lsA] [bit] NOT NULL CONSTRAINT [DF_LeadSources_lsA]  DEFAULT (1),
	[lsStartD] [datetime] NULL,
	[lsFTPServer] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsFTPPath] [varchar](70) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsFTPFile] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsFTPUserN] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsFTPPwd] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsNTDrive] [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsNTPath] [varchar](70) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsIHEStdFile] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsCommand1] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsProcesar] [bit] NOT NULL CONSTRAINT [DF_LeadSources_lsProcesar_1]  DEFAULT (1),
	[lsMaxAuthGifts] [money] NOT NULL DEFAULT (0),
	[lsHotel] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsso] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsZone] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lszn] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsBoss] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[lsUseOpera] [bit] NOT NULL CONSTRAINT [DF_LeadSources_lsUseOpera]  DEFAULT ((0)),
 CONSTRAINT [PK_LeadSources] PRIMARY KEY CLUSTERED 
(
	[lsID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del programa' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lspg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de habitaciones' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsRooms'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lssr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del área' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsar'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la región' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsrg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Horas de diferencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsHoursDif'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si se pagan In & Outs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsPayInOut'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si se pagan Walk Outs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsPayWalkOut'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si se está transfiriendo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsRunTrans'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Eficiencia esperada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsEfficiencyTh'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de show esperado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsShowFactorTh'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de booking esperado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsBookFactorTh'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numero de shows esperado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsShowsTh'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si está en mantenimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsDBMaint'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Servidor FTP del archivo de transferencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsFTPServer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ruta donde será descargado el archivo de transferencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsFTPPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre con el que será descargado el archivo de transferencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsFTPFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre de usuario del servidor FTP para obtener el archivo de transferencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsFTPUserN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contraseña del servidor FTP para obtener el archivo de transferencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsFTPPwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unidad de disco del archivo de transferencia en el servidor FTP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsNTDrive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ruta del archivo de transferencia en el servidor FTP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsNTPath'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre del archivo de transferencia en el servidor FTP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsIHEStdFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto máximo de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsMaxAuthGifts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel del sistema de Hotel' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsHotel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del segmento por Lead Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsso'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la zona en el sistema hotelero' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsZone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la zona' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lszn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de patrón' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsBoss'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si usa Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources', @level2type=N'COLUMN',@level2name=N'lsUseOpera'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de Lead Sources' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LeadSources'
GO
ALTER TABLE [dbo].[LeadSources]  WITH CHECK ADD  CONSTRAINT [FK_LeadSources_Areas] FOREIGN KEY([lsar])
REFERENCES [dbo].[Areas] ([arID])
GO
ALTER TABLE [dbo].[LeadSources] CHECK CONSTRAINT [FK_LeadSources_Areas]
GO
ALTER TABLE [dbo].[LeadSources]  WITH CHECK ADD  CONSTRAINT [FK_LeadSources_Personnel_Boss] FOREIGN KEY([lsBoss])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[LeadSources] CHECK CONSTRAINT [FK_LeadSources_Personnel_Boss]
GO
ALTER TABLE [dbo].[LeadSources]  WITH CHECK ADD  CONSTRAINT [FK_LeadSources_Programs] FOREIGN KEY([lspg])
REFERENCES [dbo].[Programs] ([pgID])
GO
ALTER TABLE [dbo].[LeadSources] CHECK CONSTRAINT [FK_LeadSources_Programs]
GO
ALTER TABLE [dbo].[LeadSources]  WITH CHECK ADD  CONSTRAINT [FK_LeadSources_Regions] FOREIGN KEY([lsrg])
REFERENCES [dbo].[Regions] ([rgID])
GO
ALTER TABLE [dbo].[LeadSources] CHECK CONSTRAINT [FK_LeadSources_Regions]
GO
ALTER TABLE [dbo].[LeadSources]  WITH CHECK ADD  CONSTRAINT [FK_LeadSources_SegmentsByLeadSource] FOREIGN KEY([lsso])
REFERENCES [dbo].[SegmentsByLeadSource] ([soID])
GO
ALTER TABLE [dbo].[LeadSources] CHECK CONSTRAINT [FK_LeadSources_SegmentsByLeadSource]
GO
ALTER TABLE [dbo].[LeadSources]  WITH CHECK ADD  CONSTRAINT [FK_LeadSources_Zones] FOREIGN KEY([lszn])
REFERENCES [dbo].[Zones] ([znID])
GO
ALTER TABLE [dbo].[LeadSources] CHECK CONSTRAINT [FK_LeadSources_Zones]