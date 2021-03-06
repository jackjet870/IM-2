if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LeadSources_Programs]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT FK_LeadSources_Programs
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Programs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Programs]
GO

CREATE TABLE [dbo].[Programs] (
	[pgID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[pgN] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Programs] WITH NOCHECK ADD 
	CONSTRAINT [PK_Programs] PRIMARY KEY  CLUSTERED 
	(
		[pgID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Programs', N'column', N'pgID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'Programs', N'column', N'pgN'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LeadSources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[LeadSources] ADD 
	CONSTRAINT [FK_LeadSources_Programs] FOREIGN KEY 
	(
		[lspg]
	) REFERENCES [dbo].[Programs] (
		[pgID]
	)
GO
