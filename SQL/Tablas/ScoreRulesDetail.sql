if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ScoreRulesDetail]
GO

CREATE TABLE [dbo].[ScoreRulesDetail] (
	[sisu] [tinyint] NOT NULL ,
	[sisp] [tinyint] NOT NULL ,
	[siScore] [money] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScoreRulesDetail] ADD 
	CONSTRAINT [PK_ScoreRulesDetail] PRIMARY KEY  CLUSTERED 
	(
		[sisu],
		[sisp]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ScoreRulesDetail] ADD 
	CONSTRAINT [FK_ScoreRulesDetail_ScoreRulesConcepts] FOREIGN KEY 
	(
		[sisp]
	) REFERENCES [dbo].[ScoreRulesConcepts] (
		[spID]
	),
	CONSTRAINT [FK_ScoreRulesDetail_ScoreRules] FOREIGN KEY 
	(
		[sisu]
	) REFERENCES [dbo].[ScoreRules] (
		[suID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Detalle de las reglas de puntuaciones', N'user', N'dbo', N'table', N'ScoreRulesDetail'

GO

exec sp_addextendedproperty N'MS_Description', N'Puntuación', N'user', N'dbo', N'table', N'ScoreRulesDetail', N'column', N'siScore'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la regla de puntuación', N'user', N'dbo', N'table', N'ScoreRulesDetail', N'column', N'sisu'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del tipo de concepto de puntuación', N'user', N'dbo', N'table', N'ScoreRulesDetail', N'column', N'sisp'


GO

