USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Personnel_Depts]') AND parent_object_id = OBJECT_ID(N'[dbo].[Personnel]'))
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [FK_Personnel_Depts]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stPR]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stPR]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stMembersPR]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stMembersPR]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stLiner]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stLiner]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stCloser]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stCloser]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stTrial]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stTrial]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stPodium]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stPodium]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pePRCaptain]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pePRCaptain]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pePRCaptain1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pePRCaptain1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pePRCaptain1_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pePRCaptain1_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stEntryHost]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stEntryHost]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stGiftsHost]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stGiftsHost]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stExitHost]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stExitHost]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stVLO]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stVLO]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stContracts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stContracts]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peAdmin1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peAdmin1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stAdmin]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stAdmin]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peRegister]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peRegister]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pePRInvitations1_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pePRInvitations1_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pePRInvitations1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pePRInvitations1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_2]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_3]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_4]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_5]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_5]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_6]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_6]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_7]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_7]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_8]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_8]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_9]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_9]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_10]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_10]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_11]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_11]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pe1_12]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pe1_12]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peTourTimes1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peTourTimes1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peTourTimes1_1]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peTourTimes1_1]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peTourTimes1_2]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peTourTimes1_2]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peTourTimes1_3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peTourTimes1_3]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_stA]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_stA]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peRecDep]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peRecDep]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peMealTicket]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peMealTicket]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peTaxiIn]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peTaxiIn]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peWH]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peWH]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peps]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peps]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Personnel__peVIP__310335E5]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF__Personnel__peVIP__310335E5]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Personnel__pePwd__06A2E7C5]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF__Personnel__pePwd__06A2E7C5]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Personnel__pePwd__07970BFE]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF__Personnel__pePwd__07970BFE]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peTeams]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peTeams]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peSecretary]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peSecretary]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_pePRSupervisor]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_pePRSupervisor]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peEquity]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peEquity]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Personnel_peBoss]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Personnel] DROP CONSTRAINT [DF_Personnel_peBoss]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Personnel]    Script Date: 11/14/2013 10:47:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Personnel]') AND type in (N'U'))
DROP TABLE [dbo].[Personnel]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Personnel]    Script Date: 11/14/2013 10:47:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Personnel](
	[peID] [varchar](10) NOT NULL,
	[peN] [varchar](40) NULL,
	[pePwd] [varchar](10) NULL,
	[peCaptain] [varchar](10) NULL,
	[pePR] [bit] NOT NULL,
	[peMembersPR] [bit] NOT NULL,
	[peLiner] [bit] NOT NULL,
	[peCloser] [bit] NOT NULL,
	[peExit] [bit] NOT NULL,
	[pePodium] [bit] NOT NULL,
	[pePRCaptain] [bit] NOT NULL,
	[peLinerCaptain] [bit] NOT NULL,
	[peCloserCaptain] [bit] NOT NULL,
	[peEntryHost] [bit] NOT NULL,
	[peGiftsHost] [bit] NOT NULL,
	[peExitHost] [bit] NOT NULL,
	[peVLO] [bit] NOT NULL,
	[peContractsPerson] [bit] NOT NULL,
	[peManagement] [bit] NOT NULL,
	[peAdmin] [bit] NOT NULL,
	[peRegister] [tinyint] NOT NULL,
	[peAvailable] [tinyint] NOT NULL,
	[pePRInvitations] [tinyint] NOT NULL,
	[peAssignment] [tinyint] NOT NULL,
	[peMOTexts] [tinyint] NOT NULL,
	[peMOConfig] [tinyint] NOT NULL,
	[peHost] [tinyint] NOT NULL,
	[peTourTracking] [tinyint] NULL,
	[peGiftsReceipts] [tinyint] NOT NULL,
	[peSales] [tinyint] NOT NULL,
	[peHostInvitations] [tinyint] NOT NULL,
	[peContracts] [tinyint] NOT NULL,
	[peMStatus] [tinyint] NOT NULL,
	[peGifts] [tinyint] NOT NULL,
	[peCurrencies] [tinyint] NOT NULL,
	[peUnavailMots] [tinyint] NOT NULL,
	[peTourTimes] [tinyint] NOT NULL,
	[peAgencies] [tinyint] NOT NULL,
	[peLocations] [tinyint] NOT NULL,
	[peLanguages] [tinyint] NOT NULL,
	[pePersonnel] [tinyint] NOT NULL,
	[peA] [bit] NOT NULL,
	[peRecDep] [tinyint] NOT NULL,
	[peMealTicket] [tinyint] NOT NULL,
	[peTaxiIn] [tinyint] NOT NULL,
	[peWH] [tinyint] NOT NULL,
	[peps] [varchar](10) NOT NULL,
	[peVIPOffice] [tinyint] NOT NULL,
	[pePwdD] [datetime] NOT NULL,
	[pePwdDays] [int] NOT NULL,
	[peTeamType] [varchar](2) NULL,
	[pePlaceID] [varchar](10) NULL,
	[peTeam] [varchar](10) NULL,
	[pepo] [varchar](10) NULL,
	[peLinerID] [varchar](10) NULL,
	[peTeams] [tinyint] NOT NULL,
	[peSecretary] [bit] NOT NULL,
	[pePRSupervisor] [bit] NOT NULL,
	[peEquity] [tinyint] NOT NULL,
	[peBoss] [bit] NOT NULL,
	[pede] [varchar](10) NULL,
 CONSTRAINT [PK_Personnel] PRIMARY KEY CLUSTERED 
(
	[peID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contraseña' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePwd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de su capitán' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peCaptain'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de PR de socios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peMembersPR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peLiner'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peCloser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de Exit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peExit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de Podium' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePodium'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de capitán de PRs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePRCaptain'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de capitán de Liners' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peLinerCaptain'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de capitán de Closers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peCloserCaptain'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de host de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peEntryHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de host de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peGiftsHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de host de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peExitHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de Verificador Legal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peVLO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de contratos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peContractsPerson'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de gerente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peManagement'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de Administrador' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peAdmin'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de registrar' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peRegister'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de disponibilidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peAvailable'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de invitaciones de PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePRInvitations'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de asignación de PRs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peAssignment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de correos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peMOTexts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de configuración de correos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peMOConfig'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de Host' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peHost'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de Tour Tracking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peTourTracking'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de recibos de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peGiftsReceipts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peSales'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de invitaciones de Host' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peHostInvitations'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de contratos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peContracts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de estados civiles' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peMStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de regalos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peGifts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de monedas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peCurrencies'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de motivos de indisponibilidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peUnavailMots'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de horarios de tour' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peTourTimes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de agencias' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peAgencies'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de locaciones' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peLocations'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de idiomas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peLanguages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePersonnel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de depósitos recibidos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peRecDep'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de cupones de comida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peMealTicket'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de taxi de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peTaxiIn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de almacenes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peWH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estatus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peps'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Última fecha en que se cambió de contraseña' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePwdD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Días de vigencia de la contraseña' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePwdDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de equipo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peTeamType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del lugar del equipo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePlaceID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del equipo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peTeam'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del puesto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pepo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peLinerID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso de equipos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peTeams'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de secretaria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peSecretary'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de supervisor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pePRSupervisor'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permiso del reporte de Equity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peEquity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rol de patrón' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'peBoss'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del departamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Personnel', @level2type=N'COLUMN',@level2name=N'pede'
GO

ALTER TABLE [dbo].[Personnel]  WITH CHECK ADD  CONSTRAINT [FK_Personnel_Depts] FOREIGN KEY([pede])
REFERENCES [dbo].[Depts] ([deID])
GO

ALTER TABLE [dbo].[Personnel] CHECK CONSTRAINT [FK_Personnel_Depts]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stPR]  DEFAULT (0) FOR [pePR]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stMembersPR]  DEFAULT (0) FOR [peMembersPR]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stLiner]  DEFAULT (0) FOR [peLiner]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stCloser]  DEFAULT (0) FOR [peCloser]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stTrial]  DEFAULT (0) FOR [peExit]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stPodium]  DEFAULT (0) FOR [pePodium]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pePRCaptain]  DEFAULT (0) FOR [pePRCaptain]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pePRCaptain1]  DEFAULT (0) FOR [peLinerCaptain]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pePRCaptain1_1]  DEFAULT (0) FOR [peCloserCaptain]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stEntryHost]  DEFAULT (0) FOR [peEntryHost]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stGiftsHost]  DEFAULT (0) FOR [peGiftsHost]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stExitHost]  DEFAULT (0) FOR [peExitHost]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stVLO]  DEFAULT (0) FOR [peVLO]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stContracts]  DEFAULT (0) FOR [peContractsPerson]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peAdmin1]  DEFAULT (0) FOR [peManagement]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stAdmin]  DEFAULT (0) FOR [peAdmin]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peRegister]  DEFAULT (0) FOR [peRegister]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pePRInvitations1_1]  DEFAULT (0) FOR [peAvailable]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe]  DEFAULT (0) FOR [pePRInvitations]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pePRInvitations1]  DEFAULT (0) FOR [peAssignment]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1]  DEFAULT (0) FOR [peMOTexts]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_1]  DEFAULT (0) FOR [peMOConfig]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_2]  DEFAULT (0) FOR [peHost]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_3]  DEFAULT (0) FOR [peTourTracking]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_4]  DEFAULT (0) FOR [peGiftsReceipts]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_5]  DEFAULT (0) FOR [peSales]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_6]  DEFAULT (0) FOR [peHostInvitations]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_7]  DEFAULT (0) FOR [peContracts]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_8]  DEFAULT (0) FOR [peMStatus]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_9]  DEFAULT (0) FOR [peGifts]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_10]  DEFAULT (0) FOR [peCurrencies]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_11]  DEFAULT (0) FOR [peUnavailMots]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pe1_12]  DEFAULT (0) FOR [peTourTimes]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peTourTimes1]  DEFAULT (0) FOR [peAgencies]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peTourTimes1_1]  DEFAULT (0) FOR [peLocations]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peTourTimes1_2]  DEFAULT (0) FOR [peLanguages]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peTourTimes1_3]  DEFAULT (0) FOR [pePersonnel]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_stA]  DEFAULT (1) FOR [peA]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peRecDep]  DEFAULT (0) FOR [peRecDep]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peMealTicket]  DEFAULT (0) FOR [peMealTicket]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peTaxiIn]  DEFAULT (0) FOR [peTaxiIn]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peWH]  DEFAULT (0) FOR [peWH]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peps]  DEFAULT ('ACTIVE') FOR [peps]
GO

ALTER TABLE [dbo].[Personnel] ADD  DEFAULT (0) FOR [peVIPOffice]
GO

ALTER TABLE [dbo].[Personnel] ADD  DEFAULT ('20070101') FOR [pePwdD]
GO

ALTER TABLE [dbo].[Personnel] ADD  DEFAULT (30) FOR [pePwdDays]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peTeams]  DEFAULT (0) FOR [peTeams]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peSecretary]  DEFAULT (0) FOR [peSecretary]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_pePRSupervisor]  DEFAULT (0) FOR [pePRSupervisor]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peEquity]  DEFAULT (0) FOR [peEquity]
GO

ALTER TABLE [dbo].[Personnel] ADD  CONSTRAINT [DF_Personnel_peBoss]  DEFAULT ((0)) FOR [peBoss]
GO

