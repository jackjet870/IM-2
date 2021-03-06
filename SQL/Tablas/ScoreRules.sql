if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ScoreRulesDetail_ScoreRules]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ScoreRulesDetail] DROP CONSTRAINT FK_ScoreRulesDetail_ScoreRules
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRules]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ScoreRules]
GO

CREATE TABLE [dbo].[ScoreRules] (
	[suID] [tinyint] NOT NULL ,
	[suN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[suA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScoreRules] ADD 
	CONSTRAINT [PK_ScoreRules] PRIMARY KEY  CLUSTERED 
	(
		[suID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de reglas de puntuaciones', N'user', N'dbo', N'table', N'ScoreRules'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'ScoreRules', N'column', N'suA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'ScoreRules', N'column', N'suID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'ScoreRules', N'column', N'suN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[ScoreRulesDetail] ADD 
	CONSTRAINT [FK_ScoreRulesDetail_ScoreRules] FOREIGN KEY 
	(
		[sisu]
	) REFERENCES [dbo].[ScoreRules] (
		[suID]
	)
GO