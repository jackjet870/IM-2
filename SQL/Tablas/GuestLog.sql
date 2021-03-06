if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestLog]
GO

CREATE TABLE [dbo].[GuestLog](
	[glID] [datetime] NOT NULL,
	[glgu] [int] NOT NULL,
	[glsr] [varchar](10) NULL,
	[glPRInfo] [varchar](10) NULL,
	[glInfoD] [datetime] NULL,
	[glPRInvit1] [varchar](10) NULL,
	[glPRInvit2] [varchar](10) NULL,
	[glLiner1] [varchar](10) NULL,
	[glLiner2] [varchar](10) NULL,
	[glCloser1] [varchar](10) NULL,
	[glCloser2] [varchar](10) NULL,
	[glCloser3] [varchar](10) NULL,
	[glTO] [varchar](10) NULL,
	[glExit1] [varchar](10) NULL,
	[glExit2] [varchar](10) NULL,
	[glPodium] [varchar](10) NULL,
	[glVLO] [varchar](10) NULL,
	[glBookD] [datetime] NULL,
	[glBookT] [datetime] NULL,
	[glLW] [bit] NOT NULL,
	[glNW] [bit] NOT NULL,
	[glQuinella] [bit] NOT NULL,
	[glShow] [bit] NOT NULL,
	[glQ] [bit] NOT NULL,
	[glInOut] [bit] NOT NULL,
	[glWalkOut] [bit] NOT NULL,
	[glCTour] [bit] NOT NULL,
	[glChangedBy] [varchar](10) NULL,
	[glReschD] [datetime] NULL,
	[glBookCanc] [bit] NOT NULL,
	[glPRFollow] [varchar](10) NULL,
	[glFollowD] [datetime] NULL,
	[glOriginAvail] [bit] NOT NULL,
	[glAvail] [bit] NOT NULL,
	[glPRAvail] [varchar](10) NULL,
	[glum] [tinyint] NOT NULL,
	[glAvailBySystem] [bit] NOT NULL,
	[glReschT] [datetime] NULL,
	[glHReservID] [varchar](15) NULL,
	[glReimpresion] [tinyint] NULL,
	[glrm] [tinyint] NULL,
 CONSTRAINT [PK_GuestLog] PRIMARY KEY CLUSTERED 
(
	[glID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha del registro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glgu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glsr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR de contacto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glPRInfo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de contacto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glInfoD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR de invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glPRInvit1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do PR de invitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glPRInvit2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glLiner1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glLiner2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glCloser1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glCloser2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 3er Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glCloser3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glExit1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glExit2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Podium' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glPodium'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del verificado legal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glVLO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glBookD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de booking' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glBookT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la invitación es quiniela' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glQuinella'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped asistió al show' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glShow'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está calificado para poder comprar una membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el show fue In & Out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glInOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el show fue Walk Out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glWalkOut'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped tuvo tour de cortesía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glCTour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del huésped que hizo el cambio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glChangedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de reprogramación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glReschD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la invitación está cancelada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glBookCanc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR de seguimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glPRFollow'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de seguimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glFollowD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped es originalmente disponible' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glOriginAvail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está disponible' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glAvail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR que modificó la disponibilidad del huésped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glPRAvail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del motivo de indisponibilidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glum'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si el huésped está disponible por sistema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glAvailBySystem'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hora de reprogramación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glReschT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio de la reservación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GuestLog', @level2type=N'COLUMN',@level2name=N'glHReservID'
GO

ALTER TABLE [dbo].[GuestLog]  WITH CHECK ADD  CONSTRAINT [FK_GuestLog_Personnel_PRAvail] FOREIGN KEY([glPRAvail])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[GuestLog] CHECK CONSTRAINT [FK_GuestLog_Personnel_PRAvail]
GO

ALTER TABLE [dbo].[GuestLog]  WITH CHECK ADD  CONSTRAINT [FK_GuestLog_Personnel_PRFollow] FOREIGN KEY([glPRFollow])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[GuestLog] CHECK CONSTRAINT [FK_GuestLog_Personnel_PRFollow]
GO

ALTER TABLE [dbo].[GuestLog]  WITH CHECK ADD  CONSTRAINT [FK_GuestLog_ReimpresionMotives] FOREIGN KEY([glrm])
REFERENCES [dbo].[ReimpresionMotives] ([rmID])
GO

ALTER TABLE [dbo].[GuestLog] CHECK CONSTRAINT [FK_GuestLog_ReimpresionMotives]
GO

ALTER TABLE [dbo].[GuestLog]  WITH CHECK ADD  CONSTRAINT [FK_GuestLog_UnavailMots] FOREIGN KEY([glum])
REFERENCES [dbo].[UnavailMots] ([umID])
GO

ALTER TABLE [dbo].[GuestLog] CHECK CONSTRAINT [FK_GuestLog_UnavailMots]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glShow1_1]  DEFAULT (0) FOR [glLW]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glShow1_2]  DEFAULT (0) FOR [glNW]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glShow1]  DEFAULT (0) FOR [glQuinella]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glShow]  DEFAULT (0) FOR [glShow]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glShow1_3]  DEFAULT (0) FOR [glQ]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glQ1]  DEFAULT (0) FOR [glInOut]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glWalkOut]  DEFAULT (0) FOR [glWalkOut]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glQ1_1]  DEFAULT (0) FOR [glCTour]
GO

ALTER TABLE [dbo].[GuestLog] ADD  DEFAULT (0) FOR [glBookCanc]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glOriginAvail]  DEFAULT (0) FOR [glOriginAvail]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glAvail]  DEFAULT (0) FOR [glAvail]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glum]  DEFAULT (0) FOR [glum]
GO

ALTER TABLE [dbo].[GuestLog] ADD  CONSTRAINT [DF_GuestLog_glAvailBySystem]  DEFAULT ((0)) FOR [glAvailBySystem]
GO

ALTER TABLE [dbo].[GuestLog] ADD  DEFAULT ((0)) FOR [glReimpresion]
GO

ALTER TABLE [dbo].[GuestLog] ADD  DEFAULT (NULL) FOR [glrm]
GO


