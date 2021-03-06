if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ScoreRulesByLeadSource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ScoreRulesByLeadSource]
GO

CREATE TABLE [dbo].[ScoreRulesByLeadSource] (
	[sbls] [varchar] (10) NOT NULL ,
	[sbA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScoreRulesByLeadSource] ADD 
	CONSTRAINT [PK_ScoreRulesByLeadSource] PRIMARY KEY  CLUSTERED 
	(
		[sbls]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ScoreRulesByLeadSource] ADD 
	CONSTRAINT [FK_ScoreRulesByLeadSource_LeadSources] FOREIGN KEY 
	(
		[sbls]
	) REFERENCES [dbo].[LeadSources] (
		[lsID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de reglas de puntuaciones por Lead Source', N'user', N'dbo', N'table', N'ScoreRulesByLeadSource'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'ScoreRulesByLeadSource', N'column', N'sbA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del Lead Source', N'user', N'dbo', N'table', N'ScoreRulesByLeadSource', N'column', N'sbls'


GO

