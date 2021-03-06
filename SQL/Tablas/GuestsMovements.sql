if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsMovements]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestsMovements]
GO

CREATE TABLE [dbo].[GuestsMovements] (
	[gmgu] [int] NOT NULL ,
	[gmgn] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gmDT] [datetime] NOT NULL ,
	[gmpe] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gmcp] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gmIpAddress] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GuestsMovements] WITH NOCHECK ADD 
	CONSTRAINT [PK_GuestsMovements] PRIMARY KEY  CLUSTERED 
	(
		[gmgu],
		[gmgn]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[GuestsMovements] WITH NOCHECK ADD 
	CONSTRAINT [DF_GuestsMovements_gmDT] DEFAULT (getdate()) FOR [gmDT]
GO

ALTER TABLE [dbo].[GuestsMovements] ADD 
	CONSTRAINT [FK_GuestsMovements_Computers] FOREIGN KEY 
	(
		[gmcp]
	) REFERENCES [dbo].[Computers] (
		[cpID]
	),
	CONSTRAINT [FK_GuestsMovements_Guests] FOREIGN KEY 
	(
		[gmgu]
	) REFERENCES [dbo].[Guests] (
		[guID]
	),
	CONSTRAINT [FK_GuestsMovements_GuestsMovementsTypes] FOREIGN KEY 
	(
		[gmgn]
	) REFERENCES [dbo].[GuestsMovementsTypes] (
		[gnID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Nombre de la computadora donde se generó el movimiento', N'user', N'dbo', N'table', N'GuestsMovements', N'column', N'gmcp'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha y hora del movimiento', N'user', N'dbo', N'table', N'GuestsMovements', N'column', N'gmDT'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del tipo de movimiento', N'user', N'dbo', N'table', N'GuestsMovements', N'column', N'gmgn'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del huésped', N'user', N'dbo', N'table', N'GuestsMovements', N'column', N'gmgu'
GO
exec sp_addextendedproperty N'MS_Description', N'Dirección IP de la computadora donde se generó el movimiento', N'user', N'dbo', N'table', N'GuestsMovements', N'column', N'gmIpAddress'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del usuario que realizó el movimiento', N'user', N'dbo', N'table', N'GuestsMovements', N'column', N'gmpe'


GO

