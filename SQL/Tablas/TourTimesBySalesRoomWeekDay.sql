if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TourTimesBySalesRoomWeekDay]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TourTimesBySalesRoomWeekDay]
GO

CREATE TABLE [dbo].[TourTimesBySalesRoomWeekDay] (
	[ttsr] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ttDay] [tinyint] NOT NULL ,
	[ttT] [datetime] NOT NULL ,
	[ttPickUpT] [datetime] NOT NULL ,
	[ttMaxBooks] [tinyint] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TourTimesBySalesRoomWeekDay] WITH NOCHECK ADD 
	CONSTRAINT [PK_TourTimesBySalesRoomWeekDay] PRIMARY KEY  CLUSTERED 
	(
		[ttsr],
		[ttDay],
		[ttT]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TourTimesBySalesRoomWeekDay] ADD 
	CONSTRAINT [DF_TourTimesBySalesRoomWeekDay__ttMaxBooks] DEFAULT (0) FOR [ttMaxBooks]
GO

ALTER TABLE [dbo].[TourTimesBySalesRoomWeekDay] ADD 
	CONSTRAINT [FK_TourTimesBySalesRoomWeekDay_SalesRooms] FOREIGN KEY 
	(
		[ttsr]
	) REFERENCES [dbo].[SalesRooms] (
		[srID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de horarios de tour por sala de ventas y día de la semana', N'user', N'dbo', N'table', N'TourTimesBySalesRoomWeekDay'

GO

exec sp_addextendedproperty N'MS_Description', N'Clave del día de la semana', N'user', N'dbo', N'table', N'TourTimesBySalesRoomWeekDay', N'column', N'ttDay'
GO
exec sp_addextendedproperty N'MS_Description', N'Número máximo de bookings', N'user', N'dbo', N'table', N'TourTimesBySalesRoomWeekDay', N'column', N'ttMaxBooks'
GO
exec sp_addextendedproperty N'MS_Description', N'Horario de pick up', N'user', N'dbo', N'table', N'TourTimesBySalesRoomWeekDay', N'column', N'ttPickUpT'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la sala de ventas', N'user', N'dbo', N'table', N'TourTimesBySalesRoomWeekDay', N'column', N'ttsr'
GO
exec sp_addextendedproperty N'MS_Description', N'Horario de tour', N'user', N'dbo', N'table', N'TourTimesBySalesRoomWeekDay', N'column', N'ttT'


GO

