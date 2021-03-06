if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[osConfig]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[osConfig]
GO

CREATE TABLE [dbo].[osConfig] (
	[ocDBVersion] [numeric](5, 2) NOT NULL ,
	[ocDBRevision] [char] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ocQs] [bit] NOT NULL ,
	[ocRunTrans] [bit] NOT NULL ,
	[ocTransDT] [datetime] NOT NULL ,
	[ocOneNightV] [bit] NOT NULL ,
	[ocUseAgencyIDs] [bit] NOT NULL ,
	[ocEfficiencyTh] [money] NOT NULL ,
	[ocShowFactorTh] [money] NOT NULL ,
	[ocBookFactorTh] [money] NOT NULL ,
	[ocShowsTh] [int] NOT NULL ,
	[ocDBMaint] [bit] NOT NULL ,
	[ocWeekDayStart] [tinyint] NOT NULL ,
	[ocStartD] [datetime] NOT NULL ,
	[ocAdminUser] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ocTwoNightV] [bit] NOT NULL ,
	[ocWelcomeCopies] [tinyint] NOT NULL ,
	[ocTourTimesSchema] [int] NOT NULL ,
	[ocBoss] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[osConfig] WITH NOCHECK ADD 
	CONSTRAINT [PK_osConfig] PRIMARY KEY  CLUSTERED 
	(
		[ocDBVersion]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[osConfig] ADD 
	CONSTRAINT [DF_osConfig_Qs] DEFAULT (0) FOR [ocQs],
	CONSTRAINT [DF_osConfig_RunTrans] DEFAULT (0) FOR [ocRunTrans],
	CONSTRAINT [DF_osConfig_OneNightV] DEFAULT (1) FOR [ocOneNightV],
	CONSTRAINT [DF_osConfig_UseAgencyIDs] DEFAULT (0) FOR [ocUseAgencyIDs],
	CONSTRAINT [DF_osConfig_EfficiencyTh] DEFAULT (1200) FOR [ocEfficiencyTh],
	CONSTRAINT [DF_osConfig_ShowFactorTh] DEFAULT (0.6) FOR [ocShowFactorTh],
	CONSTRAINT [DF_osConfig_BookFactorTh] DEFAULT (55) FOR [ocBookFactorTh],
	CONSTRAINT [DF_osConfig_ShowsMin] DEFAULT (10) FOR [ocShowsTh],
	CONSTRAINT [DF_osConfig_Maintenance] DEFAULT (0) FOR [ocDBMaint],
	CONSTRAINT [DF_osConfig_ocWeekDayStart] DEFAULT (7) FOR [ocWeekDayStart],
	CONSTRAINT [DF__osConfig__ocTwoN__4C413C06] DEFAULT (0) FOR [ocTwoNightV],
	CONSTRAINT [DF__osconfig__ocWelc__57B2EEB2] DEFAULT (1) FOR [ocWelcomeCopies],
	CONSTRAINT [DF_osConfig_ocTourTimesSchema] DEFAULT (2) FOR [ocTourTimesSchema]
GO

ALTER TABLE [dbo].[osConfig] ADD 
	CONSTRAINT [FK_osConfig_Personnel] FOREIGN KEY 
	(
		[ocAdminUser]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	),
	CONSTRAINT [FK_osConfig_Personnel_Boss] FOREIGN KEY 
	(
		[ocBoss]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	),
	CONSTRAINT [FK_osConfig_TourTimesSchemas] FOREIGN KEY 
	(
		[ocTourTimesSchema]
	) REFERENCES [dbo].[TourTimesSchemas] (
		[tcID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Clave del usuario administrador', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocAdminUser'
GO
exec sp_addextendedproperty N'MS_Description', N'Porcentaje de booking', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocBookFactorTh'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del patrón', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocBoss'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si la base de datos está en mantenimiento', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocDBMaint'
GO
exec sp_addextendedproperty N'MS_Description', N'Número de revisión', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocDBRevision'
GO
exec sp_addextendedproperty N'MS_Description', N'Número de versión', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocDBVersion'
GO
exec sp_addextendedproperty N'MS_Description', N'Eficiencia', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocEfficiencyTh'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si está corriendo el proceso de transferencia de reservaciones (Origos Transfer)', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocRunTrans'
GO
exec sp_addextendedproperty N'MS_Description', N'Porcentaje de show', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocShowFactorTh'
GO
exec sp_addextendedproperty N'MS_Description', N'Número de shows', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocShowsTh'
GO
exec sp_addextendedproperty N'MS_Description', N'Esquema de horarios de tour', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocTourTimesSchema'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha y hora en que se ejecutó la última tranferencia de reservaciones', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocTransDT'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica se se desean usar sólo las claves de las agencias en lugar de clave y descripción', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocUseAgencyIDs'
GO
exec sp_addextendedproperty N'MS_Description', N'Día de inicio de la semana', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocWeekDayStart'
GO
exec sp_addextendedproperty N'MS_Description', N'Número de copias del reporte de Guest Registration', N'user', N'dbo', N'table', N'osConfig', N'column', N'ocWelcomeCopies'


GO

