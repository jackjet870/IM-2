if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SegmentsCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SegmentsCategories]
GO

CREATE TABLE [dbo].[SegmentsCategories] (
	[scID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[scN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[scA] [bit] NOT NULL ,
	[scO] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SegmentsCategories] WITH NOCHECK ADD 
	CONSTRAINT [PK_SegmentsCategories] PRIMARY KEY  CLUSTERED 
	(
		[scID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[SegmentsCategories] WITH NOCHECK ADD 
	CONSTRAINT [DF_SegmentsCategories_scO] DEFAULT (9999) FOR [scO]
GO


exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'SegmentsCategories', N'column', N'scID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'SegmentsCategories', N'column', N'scN'
GO
exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'SegmentsCategories', N'column', N'scA'
GO
exec sp_addextendedproperty N'MS_Description', N'Orden', N'user', N'dbo', N'table', N'SegmentsCategories', N'column', N'scO'


GO

