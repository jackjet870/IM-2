if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NoticesByLeadSource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[NoticesByLeadSource]
GO

CREATE TABLE [dbo].[NoticesByLeadSource] (
	[nlno] [int] NOT NULL ,
	[nlls] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[NoticesByLeadSource] WITH NOCHECK ADD 
	CONSTRAINT [PK_NoticesByLeadSource] PRIMARY KEY  CLUSTERED 
	(
		[nlno],
		[nlls]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[NoticesByLeadSource] ADD 
	CONSTRAINT [FK_NoticesByLeadSource_LeadSources] FOREIGN KEY 
	(
		[nlls]
	) REFERENCES [dbo].[LeadSources] (
		[lsID]
	),
	CONSTRAINT [FK_NoticesByLeadSource_Notices] FOREIGN KEY 
	(
		[nlno]
	) REFERENCES [dbo].[Notices] (
		[noID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de avisos por Lead Source', N'user', N'dbo', N'table', N'NoticesByLeadSource'

GO

exec sp_addextendedproperty N'MS_Description', N'Clave del Lead Source', N'user', N'dbo', N'table', N'NoticesByLeadSource', N'column', N'nlls'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del aviso', N'user', N'dbo', N'table', N'NoticesByLeadSource', N'column', N'nlno'


GO

