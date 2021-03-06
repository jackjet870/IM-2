if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Computers_Desks]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Computers] DROP CONSTRAINT FK_Computers_Desks
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Desks]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Desks]
GO

CREATE TABLE [dbo].[Desks] (
	[dkID] [int] IDENTITY (1, 1) NOT NULL ,
	[dkN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[dkA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Desks] WITH NOCHECK ADD 
	CONSTRAINT [PK_Desks] PRIMARY KEY  CLUSTERED 
	(
		[dkID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'Desks', N'column', N'dkA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Desks', N'column', N'dkID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'Desks', N'column', N'dkN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Computers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Computers] ADD 
	CONSTRAINT [FK_Computers_Desks] FOREIGN KEY 
	(
		[cpdk]
	) REFERENCES [dbo].[Desks] (
		[dkID]
	)
GO
