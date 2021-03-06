if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsAdditional]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestsAdditional]
GO

CREATE TABLE [dbo].[GuestsAdditional] (
	[gagu] [int] NOT NULL ,
	[gaAdditional] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GuestsAdditional] WITH NOCHECK ADD 
	CONSTRAINT [PK_GuestsAdditional] PRIMARY KEY  CLUSTERED 
	(
		[gagu],
		[gaAdditional]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[GuestsAdditional] ADD 
	CONSTRAINT [FK_GuestsAdditional_Guests] FOREIGN KEY 
	(
		[gagu]
	) REFERENCES [dbo].[Guests] (
		[guID]
	),
	CONSTRAINT [FK_GuestsAdditional_Guests1] FOREIGN KEY 
	(
		[gaAdditional]
	) REFERENCES [dbo].[Guests] (
		[guID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Clave del invitado adicional', N'user', N'dbo', N'table', N'GuestsAdditional', N'column', N'gaAdditional'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del invitado', N'user', N'dbo', N'table', N'GuestsAdditional', N'column', N'gagu'


GO

