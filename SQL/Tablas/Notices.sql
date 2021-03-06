if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_NoticesByLeadSource_Notices]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[NoticesByLeadSource] DROP CONSTRAINT FK_NoticesByLeadSource_Notices
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Notices]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Notices]
GO

CREATE TABLE [dbo].[Notices] (
	[noID] [int] IDENTITY (1, 1) NOT NULL ,
	[noTitle] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[noA] [bit] NOT NULL ,
	[noText] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[noStartD] [datetime] NOT NULL ,
	[noEndD] [datetime] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Notices] WITH NOCHECK ADD 
	CONSTRAINT [PK_Notices] PRIMARY KEY  CLUSTERED 
	(
		[noID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de avisos', N'user', N'dbo', N'table', N'Notices'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'Notices', N'column', N'noA'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha hasta', N'user', N'dbo', N'table', N'Notices', N'column', N'noEndD'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Notices', N'column', N'noID'
GO
exec sp_addextendedproperty N'MS_Description', N'Fecha desde', N'user', N'dbo', N'table', N'Notices', N'column', N'noStartD'
GO
exec sp_addextendedproperty N'MS_Description', N'Texto del aviso', N'user', N'dbo', N'table', N'Notices', N'column', N'noText'
GO
exec sp_addextendedproperty N'MS_Description', N'Titulo', N'user', N'dbo', N'table', N'Notices', N'column', N'noTitle'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NoticesByLeadSource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[NoticesByLeadSource] ADD 
	CONSTRAINT [FK_NoticesByLeadSource_Notices] FOREIGN KEY 
	(
		[nlno]
	) REFERENCES [dbo].[Notices] (
		[noID]
	)
GO
