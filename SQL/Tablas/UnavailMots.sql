if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Agencies_UnavailMots]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agencies] DROP CONSTRAINT FK_Agencies_UnavailMots
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Contracts_UnavailMots]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Contracts] DROP CONSTRAINT FK_Contracts_UnavailMots
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Countries_UnavailMots]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Countries] DROP CONSTRAINT FK_Countries_UnavailMots
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_GuestLog_UnavailMots]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[GuestLog] DROP CONSTRAINT FK_GuestLog_UnavailMots
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HotelAgencies_UnavailMots]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HotelAgencies] DROP CONSTRAINT FK_HotelAgencies_UnavailMots
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_HotelCountries_UnavailMots]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[HotelCountries] DROP CONSTRAINT FK_HotelCountries_UnavailMots
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnavailMots]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UnavailMots]
GO

CREATE TABLE [dbo].[UnavailMots] (
	[umID] [tinyint] NOT NULL ,
	[umN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[umA] [bit] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UnavailMots] WITH NOCHECK ADD 
	CONSTRAINT [PK_UnAvailMots] PRIMARY KEY  CLUSTERED 
	(
		[umID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[UnavailMots] ADD 
	CONSTRAINT [DF_UnAvailMots_uaA] DEFAULT (0) FOR [umA]
GO

 CREATE  UNIQUE  INDEX [K_umN] ON [dbo].[UnavailMots]([umN]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Agencies] ADD 
	CONSTRAINT [FK_Agencies_UnavailMots] FOREIGN KEY 
	(
		[agum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Contracts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Contracts] ADD 
	CONSTRAINT [FK_Contracts_UnavailMots] FOREIGN KEY 
	(
		[cnum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Countries]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[Countries] ADD 
	CONSTRAINT [FK_Countries_UnavailMots] FOREIGN KEY 
	(
		[coum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GuestLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[GuestLog] ADD 
	CONSTRAINT [FK_GuestLog_UnavailMots] FOREIGN KEY 
	(
		[glum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HotelAgencies]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[HotelAgencies] ADD 
	CONSTRAINT [FK_HotelAgencies_UnavailMots] FOREIGN KEY 
	(
		[haum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[HotelCountries]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[HotelCountries] ADD 
	CONSTRAINT [FK_HotelCountries_UnavailMots] FOREIGN KEY 
	(
		[hcum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO