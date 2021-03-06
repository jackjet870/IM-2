if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DaysOff]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[DaysOff]
GO

CREATE TABLE [dbo].[DaysOff] (
	[dope] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[doMonday] [bit] NOT NULL ,
	[doTuesday] [bit] NOT NULL ,
	[doWednesday] [bit] NOT NULL ,
	[doThursday] [bit] NOT NULL ,
	[doFriday] [bit] NOT NULL ,
	[doSaturday] [bit] NOT NULL ,
	[doSunday] [bit] NOT NULL ,
	[doList] AS ([dbo].[UFN_OR_ObtenerListaDiasDescanso]([doMonday], [doTuesday], [doWednesday], [doThursday], [doFriday], [doSaturday], [doSunday])) 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DaysOff] WITH NOCHECK ADD 
	CONSTRAINT [PK_DaysOff] PRIMARY KEY  CLUSTERED 
	(
		[dope]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[DaysOff] ADD 
	CONSTRAINT [FK_DaysOff_Personnel] FOREIGN KEY 
	(
		[dope]
	) REFERENCES [dbo].[Personnel] (
		[peID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Viernes', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doFriday'
GO
exec sp_addextendedproperty N'MS_Description', N'Lista de días de descanso', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doList'
GO
exec sp_addextendedproperty N'MS_Description', N'Lunes', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doMonday'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del personal', N'user', N'dbo', N'table', N'DaysOff', N'column', N'dope'
GO
exec sp_addextendedproperty N'MS_Description', N'Sábado', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doSaturday'
GO
exec sp_addextendedproperty N'MS_Description', N'Domingo', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doSunday'
GO
exec sp_addextendedproperty N'MS_Description', N'Jueves', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doThursday'
GO
exec sp_addextendedproperty N'MS_Description', N'Martes', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doTuesday'
GO
exec sp_addextendedproperty N'MS_Description', N'Miércoles', N'user', N'dbo', N'table', N'DaysOff', N'column', N'doWednesday'


GO

