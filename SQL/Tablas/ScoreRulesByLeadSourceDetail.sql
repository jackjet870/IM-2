if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesByLeadSourceDetail]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ScoreRulesByLeadSourceDetail]
GO

CREATE TABLE [dbo].[ScoreRulesByLeadSourceDetail] (
	[sjls] [varchar] (10) NOT NULL ,
	[sjsp] [tinyint] NOT NULL ,
	[sjScore] [money] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScoreRulesByLeadSourceDetail] ADD 
	CONSTRAINT [PK_ScoreRulesByLeadSourceDetail] PRIMARY KEY  CLUSTERED 
	(
		[sjls],
		[sjsp]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ScoreRulesByLeadSourceDetail] ADD 
	CONSTRAINT [FK_ScoreRulesByLeadSourceDetail_ScoreRulesConcepts] FOREIGN KEY 
	(
		[sjsp]
	) REFERENCES [dbo].[ScoreRulesConcepts] (
		[spID]
	),
	CONSTRAINT [FK_ScoreRulesByLeadSourceDetail_LeadSources] FOREIGN KEY 
	(
		[sjls]
	) REFERENCES [dbo].[LeadSources] (
		[lsID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Detalle de las reglas de puntuaciones por Lead Source', N'user', N'dbo', N'table', N'ScoreRulesByLeadSourceDetail'

GO

exec sp_addextendedproperty N'MS_Description', N'Puntuación', N'user', N'dbo', N'table', N'ScoreRulesByLeadSourceDetail', N'column', N'sjScore'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del Lead Source', N'user', N'dbo', N'table', N'ScoreRulesByLeadSourceDetail', N'column', N'sjls'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del tipo de concepto de puntuación', N'user', N'dbo', N'table', N'ScoreRulesByLeadSourceDetail', N'column', N'sjsp'


GO

