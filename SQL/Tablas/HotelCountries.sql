if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HotelAgencies_HotelCountries]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HotelAgencies] DROP CONSTRAINT FK_HotelAgencies_HotelCountries
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HotelCountries]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[HotelCountries]
GO

CREATE TABLE [dbo].[HotelCountries] (
	[hcID] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[hcN] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[hcA] [bit] NOT NULL ,
	[hcco] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[hcum] [tinyint] NOT NULL ,
	[hcla] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[HotelCountries] WITH NOCHECK ADD 
	CONSTRAINT [PK_HotelCountries] PRIMARY KEY  CLUSTERED 
	(
		[hcID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[HotelCountries] ADD 
	CONSTRAINT [DF__HotelCountr__hcA__108B795B] DEFAULT (1) FOR [hcA],
	CONSTRAINT [DF__HotelCount__hcum__117F9D94] DEFAULT (0) FOR [hcum],
	CONSTRAINT [DF__HotelCount__hcla__1273C1CD] DEFAULT ('EN') FOR [hcla]
GO

ALTER TABLE [dbo].[HotelCountries] ADD 
	CONSTRAINT [FK_HotelCountries_Languages] FOREIGN KEY 
	(
		[hcla]
	) REFERENCES [dbo].[Languages] (
		[laID]
	),
	CONSTRAINT [FK_HotelCountries_UnavailMots] FOREIGN KEY 
	(
		[hcum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'HotelCountries', N'column', N'hcA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del pais en Origos', N'user', N'dbo', N'table', N'HotelCountries', N'column', N'hcco'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'HotelCountries', N'column', N'hcID'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del idioma', N'user', N'dbo', N'table', N'HotelCountries', N'column', N'hcla'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripcion', N'user', N'dbo', N'table', N'HotelCountries', N'column', N'hcN'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del motivo de indisponibilidad', N'user', N'dbo', N'table', N'HotelCountries', N'column', N'hcum'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HotelCountries]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[HotelAgencies] ADD 
	CONSTRAINT [FK_HotelAgencies_HotelCountries] FOREIGN KEY 
	(
		[haco]
	) REFERENCES [dbo].[HotelCountries] (
		[hcID]
	)
GO
