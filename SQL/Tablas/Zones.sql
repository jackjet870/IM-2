if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LeadSources_Zones]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LeadSources] DROP CONSTRAINT FK_LeadSources_Zones
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Zones]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Zones]
GO

CREATE TABLE [dbo].[Zones] (
	[znID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[znN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[znA] [bit] NOT NULL ,
	[znZoneHotel] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Zones] WITH NOCHECK ADD 
	CONSTRAINT [PK_Zones] PRIMARY KEY  CLUSTERED 
	(
		[znID]
	)  ON [PRIMARY] 
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de zonas', N'user', N'dbo', N'table', N'Zones'

GO

exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'Zones', N'column', N'znA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Zones', N'column', N'znID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'Zones', N'column', N'znN'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave de la zona del sistema hotelero', N'user', N'dbo', N'table', N'Zones', N'column', N'znZoneHotel'


GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LeadSources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
ALTER TABLE [dbo].[LeadSources] ADD 
	CONSTRAINT [FK_LeadSources_Zones] FOREIGN KEY 
	(
		[lszn]
	) REFERENCES [dbo].[Zones] (
		[znID]
	)
GO
