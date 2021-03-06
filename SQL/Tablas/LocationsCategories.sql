if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LocationsCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LocationsCategories]
GO

CREATE TABLE [dbo].[LocationsCategories] (
	[lcID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[lcN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[lcA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LocationsCategories] WITH NOCHECK ADD 
	CONSTRAINT [PK_LocationsCategories] PRIMARY KEY  CLUSTERED 
	(
		[lcID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'LocationsCategories', N'column', N'lcID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'LocationsCategories', N'column', N'lcN'
GO
exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'LocationsCategories', N'column', N'lcA'


GO

