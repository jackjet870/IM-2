if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HotelAgencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HotelAgencies]
GO

CREATE TABLE [dbo].[HotelAgencies] (
	[haID] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[haN] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[haA] [bit] NOT NULL ,
	[haag] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[haum] [tinyint] NOT NULL ,
	[hamk] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[haco] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[HotelAgencies] WITH NOCHECK ADD 
	CONSTRAINT [PK_HotelAgencies] PRIMARY KEY  CLUSTERED 
	(
		[haID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[HotelAgencies] ADD 
	CONSTRAINT [DF__HotelAgenci__haA__15502E78] DEFAULT (1) FOR [haA],
	CONSTRAINT [DF__HotelAgenc__haum__164452B1] DEFAULT (0) FOR [haum],
	CONSTRAINT [DF__HotelAgenc__hamk__173876EA] DEFAULT ('AGENCIES') FOR [hamk],
	CONSTRAINT [DF__HotelAgenc__haco__182C9B23] DEFAULT ('UNKNOWN') FOR [haco]
GO

 CREATE  INDEX [K_HotelAgencies_haag] ON [dbo].[HotelAgencies]([haag]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

ALTER TABLE [dbo].[HotelAgencies] ADD 
	CONSTRAINT [FK_HotelAgencies_HotelCountries] FOREIGN KEY 
	(
		[haco]
	) REFERENCES [dbo].[HotelCountries] (
		[hcID]
	),
	CONSTRAINT [FK_HotelAgencies_Markets] FOREIGN KEY 
	(
		[hamk]
	) REFERENCES [dbo].[Markets] (
		[mkID]
	),
	CONSTRAINT [FK_HotelAgencies_UnavailMots] FOREIGN KEY 
	(
		[haum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'HotelAgencies', N'column', N'haA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la agencia en Origos', N'user', N'dbo', N'table', N'HotelAgencies', N'column', N'haag'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del pais', N'user', N'dbo', N'table', N'HotelAgencies', N'column', N'haco'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'HotelAgencies', N'column', N'haID'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del mercado', N'user', N'dbo', N'table', N'HotelAgencies', N'column', N'hamk'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripcion', N'user', N'dbo', N'table', N'HotelAgencies', N'column', N'haN'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del motivo de indisponibilidad', N'user', N'dbo', N'table', N'HotelAgencies', N'column', N'haum'


GO

