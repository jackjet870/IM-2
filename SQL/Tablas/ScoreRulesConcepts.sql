if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ScoreRulesByLeadSourceDetail_ScoreRulesConcepts]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ScoreRulesByLeadSourceDetail] DROP CONSTRAINT FK_ScoreRulesByLeadSourceDetail_ScoreRulesConcepts
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ScoreRulesDetail_ScoreRulesConcepts]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ScoreRulesDetail] DROP CONSTRAINT FK_ScoreRulesDetail_ScoreRulesConcepts
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesConcepts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ScoreRulesConcepts]
GO

CREATE TABLE [dbo].[ScoreRulesConcepts] (
	[spID] [tinyint] NOT NULL ,
	[spN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[spA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScoreRulesConcepts] WITH NOCHECK ADD 
	CONSTRAINT [PK_ScoreRulesConcepts] PRIMARY KEY  CLUSTERED 
	(
		[spID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de conceptos de puntuación', N'user', N'dbo', N'table', N'ScoreRulesConcepts'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'ScoreRulesConcepts', N'column', N'spA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'ScoreRulesConcepts', N'column', N'spID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'ScoreRulesConcepts', N'column', N'spN'


GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[ScoreRulesDetail] ADD 
	CONSTRAINT [FK_ScoreRulesDetail_ScoreRulesConcepts] FOREIGN KEY 
	(
		[sisp]
	) REFERENCES [dbo].[ScoreRulesConcepts] (
		[spID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesByLeadSourceDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[ScoreRulesByLeadSourceDetail] ADD 
	CONSTRAINT [FK_ScoreRulesByLeadSourceDetail_ScoreRulesConcepts] FOREIGN KEY 
	(
		[sjsp]
	) REFERENCES [dbo].[ScoreRulesConcepts] (
		[spID]
	)
GO
