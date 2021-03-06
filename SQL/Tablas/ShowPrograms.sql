if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ShowPrograms]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ShowPrograms]
GO

CREATE TABLE [dbo].[ShowPrograms] (
	[skID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[skN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[skA] [bit] NOT NULL ,
	[sksg] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShowPrograms] WITH NOCHECK ADD 
	CONSTRAINT [PK_ShowPrograms] PRIMARY KEY  CLUSTERED 
	(
		[skID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ShowPrograms] ADD 
	CONSTRAINT [FK_ShowPrograms_ShowProgramsCategories] FOREIGN KEY 
	(
		[sksg]
	) REFERENCES [dbo].[ShowProgramsCategories] (
		[sgID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de programas de show', N'user', N'dbo', N'table', N'ShowPrograms'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'ShowPrograms', N'column', N'skA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'ShowPrograms', N'column', N'skID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'ShowPrograms', N'column', N'skN'


GO

