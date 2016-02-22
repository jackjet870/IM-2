USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_osTransfer_Clubs]') AND parent_object_id = OBJECT_ID(N'[dbo].[osTransfer]'))
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [FK_osTransfer_Clubs]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfer__tPax__57E7F8DC]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfer__tPax__57E7F8DC]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_osTransfer_tcoID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF_osTransfer_tcoID]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_osTransfer_tcoN]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF_osTransfer_tcoN]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_osTransfer_tagID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF_osTransfer_tagID]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_osTransfer_tagN]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF_osTransfer_tagN]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfe__tOnGr__58DC1D15]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfe__tOnGr__58DC1D15]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfe__tComp__59D0414E]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfe__tComp__59D0414E]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfer__tVIP__5AC46587]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfer__tVIP__5AC46587]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfe__tMemb__5BB889C0]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfe__tMemb__5BB889C0]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfer__tum__5CACADF9]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfer__tum__5CACADF9]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_osTransfer_tmk]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF_osTransfer_tmk]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfe__tAvai__5DA0D232]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfe__tAvai__5DA0D232]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfer__tla__5E94F66B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfer__tla__5E94F66B]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__osTransfe__tDivR__4E298478]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF__osTransfe__tDivR__4E298478]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Transfer_tCompany]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF_Transfer_tCompany]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_osTransfer_tIdProfileOpera]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[osTransfer] DROP CONSTRAINT [DF_osTransfer_tIdProfileOpera]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[osTransfer]    Script Date: 11/14/2013 09:53:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[osTransfer]') AND type in (N'U'))
DROP TABLE [dbo].[osTransfer]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[osTransfer]    Script Date: 11/14/2013 09:53:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[osTransfer](
	[tls] [varchar](5) NOT NULL,
	[tHReservID] [varchar](15) NOT NULL,
	[tFirstName] [varchar](30) NULL,
	[tLastName] [varchar](65) NULL,
	[tRoomNum] [varchar](6) NULL,
	[tPax] [decimal](4, 1) NOT NULL,
	[tCheckInD] [datetime] NULL,
	[tCheckOutD] [datetime] NULL,
	[tcoID] [varchar](25) NOT NULL,
	[tcoN] [varchar](50) NOT NULL,
	[tagID] [varchar](35) NOT NULL,
	[tagN] [varchar](100) NOT NULL,
	[tOnGroup] [bit] NOT NULL,
	[tComplim] [bit] NOT NULL,
	[tVIP] [bit] NOT NULL,
	[tMember] [bit] NOT NULL,
	[tum] [tinyint] NOT NULL,
	[tmk] [varchar](10) NOT NULL,
	[tAvail] [bit] NOT NULL,
	[tla] [varchar](2) NOT NULL,
	[toagID] [varchar](35) NULL,
	[tocoID] [varchar](25) NULL,
	[tGuestStatus] [varchar](1) NULL,
	[tMembershipNum] [varchar](15) NULL,
	[tcoAID] [varchar](25) NOT NULL,
	[tcoAN] [varchar](50) NOT NULL,
	[tO1] [varchar](20) NULL,
	[tO2] [varchar](20) NULL,
	[tO3] [varchar](20) NULL,
	[tO4] [varchar](20) NULL,
	[tO5] [varchar](20) NULL,
	[tO6] [varchar](20) NULL,
	[tEditDT] [datetime] NOT NULL,
	[tHotel] [varchar](20) NULL,
	[tCCType] [varchar](30) NULL,
	[tDivResConsec] [tinyint] NOT NULL,
	[tDivResLeadSource] [varchar](10) NULL,
	[tDivResResNum] [varchar](15) NULL,
	[tGuestRef] [varchar](12) NULL,
	[tBirthDate1] [datetime] NULL,
	[tBirthDate2] [datetime] NULL,
	[tBirthDate3] [datetime] NULL,
	[tBirthDate4] [datetime] NULL,
	[tAge1] [tinyint] NULL,
	[tAge2] [tinyint] NULL,
	[tReservationType] [varchar](1) NULL,
	[tHotelPrevious] [varchar](10) NULL,
	[tFolioPrevious] [varchar](15) NULL,
	[tEmail] [varchar](50) NULL,
	[tCity] [varchar](40) NULL,
	[tState] [varchar](30) NULL,
	[tCompany] [decimal](2, 0) NOT NULL,
	[tIdProfileOpera] [varchar](15) NOT NULL,
	[trt] [varchar](10) NULL,
	[trtN] [varchar](50) NULL,
	[tcnN] [varchar](100) NULL,
	[tcl] [int] NULL,
 CONSTRAINT [PK_osTransfer] PRIMARY KEY CLUSTERED 
(
	[tls] ASC,
	[tHReservID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Lead Source (Hotel)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tls'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tHReservID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tFirstName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellido' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tLastName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Habitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tRoomNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de huéspedes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tPax'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de llegada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tCheckInD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de salida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tCheckOutD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del país' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tcoID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del país' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tcoN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tagID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tagN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si pertenece a un grupo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tOnGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si es de cortesía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tComplim'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del motivo de indisponibilidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del mercado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tmk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Disponible' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tAvail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del idioma' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tla'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'toagID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del país' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tocoID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estatus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tGuestStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tMembershipNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del país de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tcoAID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del país de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tcoAN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 1 (Contrato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tO1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 2 (Tipo de invitado)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tO2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tO3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tO4'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tO5'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo opcional 6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tO6'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha y hora de modificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tEditDT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tHotel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tarjeta de crédito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tCCType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consecutivo de una reservación enlazada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tDivResConsec'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hotel anterior de una reservación enlazada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tDivResLeadSource'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio de la reservación anterior de una reservación enlazada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tDivResResNum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de socio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tGuestRef'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tBirthDate1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tBirthDate2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños del 3er huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tBirthDate3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cumpleaños del 4to huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tBirthDate4'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tAge1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edad del acompañante' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tAge2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de reservación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tReservationType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tHotelPrevious'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tFolioPrevious'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Correo electrónico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tEmail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ciudad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tCity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tState'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la compañía de la membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tCompany'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id del perfil de Opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tIdProfileOpera'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de habitacion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'trt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripcion del tipo de habitacion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'trtN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripcion del contrato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tcnN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del club' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'osTransfer', @level2type=N'COLUMN',@level2name=N'tcl'
GO

ALTER TABLE [dbo].[osTransfer]  WITH CHECK ADD  CONSTRAINT [FK_osTransfer_Clubs] FOREIGN KEY([tcl])
REFERENCES [dbo].[Clubs] ([clID])
GO

ALTER TABLE [dbo].[osTransfer] CHECK CONSTRAINT [FK_osTransfer_Clubs]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfer__tPax__57E7F8DC]  DEFAULT (0) FOR [tPax]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF_osTransfer_tcoID]  DEFAULT ('UNKNOWN') FOR [tcoID]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF_osTransfer_tcoN]  DEFAULT ('UNKNOWN') FOR [tcoN]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF_osTransfer_tagID]  DEFAULT ('UNKNOWN') FOR [tagID]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF_osTransfer_tagN]  DEFAULT ('UNKNOWN') FOR [tagN]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfe__tOnGr__58DC1D15]  DEFAULT (0) FOR [tOnGroup]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfe__tComp__59D0414E]  DEFAULT (0) FOR [tComplim]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfer__tVIP__5AC46587]  DEFAULT (0) FOR [tVIP]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfe__tMemb__5BB889C0]  DEFAULT (0) FOR [tMember]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfer__tum__5CACADF9]  DEFAULT (0) FOR [tum]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF_osTransfer_tmk]  DEFAULT ('AGENCIES') FOR [tmk]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfe__tAvai__5DA0D232]  DEFAULT (0) FOR [tAvail]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF__osTransfer__tla__5E94F66B]  DEFAULT ('EN') FOR [tla]
GO

ALTER TABLE [dbo].[osTransfer] ADD  DEFAULT (0) FOR [tDivResConsec]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF_Transfer_tCompany]  DEFAULT (0) FOR [tCompany]
GO

ALTER TABLE [dbo].[osTransfer] ADD  CONSTRAINT [DF_osTransfer_tIdProfileOpera]  DEFAULT ('') FOR [tIdProfileOpera]
GO

