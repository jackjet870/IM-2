if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_GuestsMovements_Computers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[GuestsMovements] DROP CONSTRAINT FK_GuestsMovements_Computers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Computers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Computers]
GO

CREATE TABLE [dbo].[Computers] (
	[cpID] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cpN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[cpIPAddress] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[cpdk] [int] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Computers] WITH NOCHECK ADD 
	CONSTRAINT [PK_Computers] PRIMARY KEY  CLUSTERED 
	(
		[cpID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Computers] ADD 
	CONSTRAINT [FK_Computers_Desks] FOREIGN KEY 
	(
		[cpdk]
	) REFERENCES [dbo].[Desks] (
		[dkID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Clave del escritorio', N'user', N'dbo', N'table', N'Computers', N'column', N'cpdk'
GO
exec sp_addextendedproperty N'MS_Description', N'Nombre de la computadora', N'user', N'dbo', N'table', N'Computers', N'column', N'cpID'
GO
exec sp_addextendedproperty N'MS_Description', N'Dirección IP de la computadora', N'user', N'dbo', N'table', N'Computers', N'column', N'cpIPAddress'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción de la computadora', N'user', N'dbo', N'table', N'Computers', N'column', N'cpN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsMovements]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[GuestsMovements] ADD 
	CONSTRAINT [FK_GuestsMovements_Computers] FOREIGN KEY 
	(
		[gmcp]
	) REFERENCES [dbo].[Computers] (
		[cpID]
	)
GO
