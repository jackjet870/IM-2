if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_GuestsMovements_GuestsMovementsTypes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[GuestsMovements] DROP CONSTRAINT FK_GuestsMovements_GuestsMovementsTypes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsMovementsTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestsMovementsTypes]
GO

CREATE TABLE [dbo].[GuestsMovementsTypes] (
	[gnID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gnN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[gnA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GuestsMovementsTypes] WITH NOCHECK ADD 
	CONSTRAINT [PK_GuestsMovementsTypes] PRIMARY KEY  CLUSTERED 
	(
		[gnID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'GuestsMovementsTypes', N'column', N'gnA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'GuestsMovementsTypes', N'column', N'gnID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'GuestsMovementsTypes', N'column', N'gnN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsMovements]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[GuestsMovements] ADD 
	CONSTRAINT [FK_GuestsMovements_GuestsMovementsTypes] FOREIGN KEY 
	(
		[gmgn]
	) REFERENCES [dbo].[GuestsMovementsTypes] (
		[gnID]
	)
GO