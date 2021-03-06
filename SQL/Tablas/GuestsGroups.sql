if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_GuestsGroupsIntegrants_GuestsGroups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[GuestsGroupsIntegrants] DROP CONSTRAINT FK_GuestsGroupsIntegrants_GuestsGroups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsGroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GuestsGroups]
GO

CREATE TABLE [dbo].[GuestsGroups] (
	[gxID] [int] IDENTITY (1, 1) NOT NULL ,
	[gxN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GuestsGroups] WITH NOCHECK ADD 
	CONSTRAINT [PK_GuestsGroups] PRIMARY KEY  CLUSTERED 
	(
		[gxID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'GuestsGroups', N'column', N'gxID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'GuestsGroups', N'column', N'gxN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestsGroupsIntegrants]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[GuestsGroupsIntegrants] ADD 
	CONSTRAINT [FK_GuestsGroupsIntegrants_GuestsGroups] FOREIGN KEY 
	(
		[gjgx]
	) REFERENCES [dbo].[GuestsGroups] (
		[gxID]
	)
GO
