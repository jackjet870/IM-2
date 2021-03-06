if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ShowPrograms_ShowProgramsCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ShowPrograms] DROP CONSTRAINT FK_ShowPrograms_ShowProgramsCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ShowProgramsCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ShowProgramsCategories]
GO

CREATE TABLE [dbo].[ShowProgramsCategories] (
	[sgID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[sgN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[sgA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShowProgramsCategories] WITH NOCHECK ADD 
	CONSTRAINT [PK_ShowProgramsCategories] PRIMARY KEY  CLUSTERED 
	(
		[sgID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de categorías de programas de show', N'user', N'dbo', N'table', N'ShowProgramsCategories'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'ShowProgramsCategories', N'column', N'sgA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'ShowProgramsCategories', N'column', N'sgID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'ShowProgramsCategories', N'column', N'sgN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ShowPrograms]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[ShowPrograms] ADD 
	CONSTRAINT [FK_ShowPrograms_ShowProgramsCategories] FOREIGN KEY 
	(
		[sksg]
	) REFERENCES [dbo].[ShowProgramsCategories] (
		[sgID]
	)
GO