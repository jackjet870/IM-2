if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LeadSources_SegmentsByLeadSource]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT FK_LeadSources_SegmentsByLeadSource
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SegmentsByLeadSource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SegmentsByLeadSource]
GO

CREATE TABLE [dbo].[SegmentsByLeadSource] (
	[soID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[soN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[soA] [bit] NOT NULL ,
	[soO] [int] NOT NULL ,
	[sosc] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SegmentsByLeadSource] WITH NOCHECK ADD 
	CONSTRAINT [PK_SegmentsByLeadSource] PRIMARY KEY  CLUSTERED 
	(
		[soID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SegmentsByLeadSource] ADD 
	CONSTRAINT [DF_SegmentsByLeadSource_soO] DEFAULT (9999) FOR [soO]
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'SegmentsByLeadSource', N'column', N'soA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'SegmentsByLeadSource', N'column', N'soID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'SegmentsByLeadSource', N'column', N'soN'
GO
exec sp_addextendedproperty N'MS_Description', N'Orden', N'user', N'dbo', N'table', N'SegmentsByLeadSource', N'column', N'soO'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de categoría de segmento', N'user', N'dbo', N'table', N'SegmentsByLeadSource', N'column', N'sosc'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LeadSources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[LeadSources] ADD 
	CONSTRAINT [FK_LeadSources_SegmentsByLeadSource] FOREIGN KEY 
	(
		[lsso]
	) REFERENCES [dbo].[SegmentsByLeadSource] (
		[soID]
	)
GO
