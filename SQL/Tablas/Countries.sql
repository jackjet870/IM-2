if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Agencies_Countries]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT FK_Agencies_Countries
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Guests_Countries]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Guests] DROP CONSTRAINT FK_Guests_Countries
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Countries]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Countries]
GO

CREATE TABLE [dbo].[Countries] (
	[coID] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[coum] [tinyint] NOT NULL ,
	[cola] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[coA] [bit] NOT NULL ,
	[coNationality] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[coList] [bit] NOT NULL ,
	[coN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Countries] WITH NOCHECK ADD 
	CONSTRAINT [PK_Countries] PRIMARY KEY  CLUSTERED 
	(
		[coID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Countries] ADD 
	CONSTRAINT [DF__Countries__coum__1BFD2C07] DEFAULT (0) FOR [coum],
	CONSTRAINT [DF__Countries__cola__1CF15040] DEFAULT ('EN') FOR [cola],
	CONSTRAINT [DF__Countries__coA__1DE57479] DEFAULT (1) FOR [coA],
	CONSTRAINT [DF__Countries__coLis__62307D25] DEFAULT (0) FOR [coList],
	CONSTRAINT [DF_Countries_coN] DEFAULT ('') FOR [coN]
GO

ALTER TABLE [dbo].[Countries] ADD 
	CONSTRAINT [FK_Countries_Languages] FOREIGN KEY 
	(
		[cola]
	) REFERENCES [dbo].[Languages] (
		[laID]
	),
	CONSTRAINT [FK_Countries_UnavailMots] FOREIGN KEY 
	(
		[coum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'Countries', N'column', N'coA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Countries', N'column', N'coID'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del idioma', N'user', N'dbo', N'table', N'Countries', N'column', N'cola'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si aparece en la lista de paises', N'user', N'dbo', N'table', N'Countries', N'column', N'coList'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripcion', N'user', N'dbo', N'table', N'Countries', N'column', N'coN'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la nacionalidad', N'user', N'dbo', N'table', N'Countries', N'column', N'coNationality'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del motivo de indisponibilidad', N'user', N'dbo', N'table', N'Countries', N'column', N'coum'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Agencies] ADD 
	CONSTRAINT [FK_Agencies_Countries] FOREIGN KEY 
	(
		[agco]
	) REFERENCES [dbo].[Countries] (
		[coID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Guests]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Guests] ADD 
	CONSTRAINT [FK_Guests_Countries] FOREIGN KEY 
	(
		[guco]
	) REFERENCES [dbo].[Countries] (
		[coID]
	)
GO