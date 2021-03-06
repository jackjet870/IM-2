if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Agencies_SegmentsByAgency]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT FK_Agencies_SegmentsByAgency
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SegmentsByAgency]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SegmentsByAgency]
GO

CREATE TABLE [dbo].[SegmentsByAgency] (
	[seID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[seN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[seA] [bit] NOT NULL ,
	[seO] [int] NOT NULL ,
	[sesc] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SegmentsByAgency] WITH NOCHECK ADD 
	CONSTRAINT [PK_SegmentsByAgency] PRIMARY KEY  CLUSTERED 
	(
		[seID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SegmentsByAgency] ADD 
	CONSTRAINT [DF_SegmentsByAgency_seO] DEFAULT (9999) FOR [seO]
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'SegmentsByAgency', N'column', N'seA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'SegmentsByAgency', N'column', N'seID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'SegmentsByAgency', N'column', N'seN'
GO
exec sp_addextendedproperty N'MS_Description', N'Orden', N'user', N'dbo', N'table', N'SegmentsByAgency', N'column', N'seO'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de categoría de segmento', N'user', N'dbo', N'table', N'SegmentsByAgency', N'column', N'sesc'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Agencies] ADD 
	CONSTRAINT [FK_Agencies_SegmentsByAgency] FOREIGN KEY 
	(
		[agse]
	) REFERENCES [dbo].[SegmentsByAgency] (
		[seID]
	)
GO