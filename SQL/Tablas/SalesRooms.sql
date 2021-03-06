if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Assistance_SalesRooms]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Assistance] DROP CONSTRAINT FK_Assistance_SalesRooms
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Efficiency_SalesRooms]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Efficiency] DROP CONSTRAINT FK_Efficiency_SalesRooms
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LeadSources_SalesRooms]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT FK_LeadSources_SalesRooms
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_TourTimesBySalesRoomWeekDay_SalesRooms]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[TourTimesBySalesRoomWeekDay] DROP CONSTRAINT FK_TourTimesBySalesRoomWeekDay_SalesRooms
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesRooms]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SalesRooms]
GO

CREATE TABLE [dbo].[SalesRooms] (
	[srID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[srN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[srHoursDif] [smallint] NOT NULL ,
	[srAddOut] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[srDBMaint] [bit] NOT NULL ,
	[srWH] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[srar] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[srA] [bit] NOT NULL ,
	[srcu] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[srGiftsRcptCloseD] [datetime] NOT NULL ,
	[srCxCCloseD] [datetime] NOT NULL ,
	[srAppointment] [bit] NOT NULL ,
	[srBoss] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SalesRooms] WITH NOCHECK ADD 
	CONSTRAINT [PK_SalesRooms] PRIMARY KEY  CLUSTERED 
	(
		[srID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SalesRooms] ADD 
	CONSTRAINT [DF_SalesRooms_lsHoursDif] DEFAULT (0) FOR [srHoursDif],
	CONSTRAINT [DF_SalesRooms_Maintenance] DEFAULT (0) FOR [srDBMaint],
	CONSTRAINT [DF_SalesRooms_srA] DEFAULT (1) FOR [srA],
	CONSTRAINT [DF__SalesRooms__srcu__4BB72C21] DEFAULT ('US') FOR [srcu],
	CONSTRAINT [DF_SalesRooms_srGiftsRcptCloseD] DEFAULT (getdate()) FOR [srGiftsRcptCloseD],
	CONSTRAINT [DF_SalesRooms_srCxCCloseD] DEFAULT (getdate()) FOR [srCxCCloseD],
	CONSTRAINT [DF_SalesRooms_srAppointment] DEFAULT (0) FOR [srAppointment]
GO

ALTER TABLE [dbo].[SalesRooms] ADD 
	CONSTRAINT [FK_SalesRooms_Personnel_Boss] FOREIGN KEY 
	(
		[srBoss]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srA'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si es una sala de citas', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srAppointment'
GO
exec sp_addextendedproperty N'MS_Description', N'Área', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srar'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de patrón', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srBoss'
GO
exec sp_addextendedproperty N'MS_Description', N'Moneda', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srcu'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha de cierre de CxC', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srCxCCloseD'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si está en mantenimiento', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srDBMaint'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha de cierre de recibos de regalos', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srGiftsRcptCloseD'
GO
exec sp_addextendedproperty N'MS_Description', N'Horas de diferencia', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srHoursDif'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srN'
GO
exec sp_addextendedproperty N'MS_Description', N'Almacén', N'user', N'dbo', N'table', N'SalesRooms', N'column', N'srWH'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Assistance]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Assistance] ADD 
	CONSTRAINT [FK_Assistance_SalesRooms] FOREIGN KEY 
	(
		[assr]
	) REFERENCES [dbo].[SalesRooms] (
		[srID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Efficiency]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Efficiency] ADD 
	CONSTRAINT [FK_Efficiency_SalesRooms] FOREIGN KEY 
	(
		[efsr]
	) REFERENCES [dbo].[SalesRooms] (
		[srID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LeadSources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[LeadSources] ADD 
	CONSTRAINT [FK_LeadSources_SalesRooms] FOREIGN KEY 
	(
		[lssr]
	) REFERENCES [dbo].[SalesRooms] (
		[srID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TourTimesBySalesRoomWeekDay]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[TourTimesBySalesRoomWeekDay] ADD 
	CONSTRAINT [FK_TourTimesBySalesRoomWeekDay_SalesRooms] FOREIGN KEY 
	(
		[ttsr]
	) REFERENCES [dbo].[SalesRooms] (
		[srID]
	)
GO
