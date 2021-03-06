if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ScoreRulesTypes]
GO

CREATE TABLE [dbo].[ScoreRulesTypes] (
	[syID] [varchar] (10) NOT NULL ,
	[syN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[syA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScoreRulesTypes] ADD 
	CONSTRAINT [PK_ScoreRulesTypes] PRIMARY KEY  CLUSTERED 
	(
		[syID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de tipos de reglas de puntuación', N'user', N'dbo', N'table', N'ScoreRulesTypes'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'ScoreRulesTypes', N'column', N'syA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'ScoreRulesTypes', N'column', N'syID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'ScoreRulesTypes', N'column', N'syN'


GO
