if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Contracts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Contracts]
GO

CREATE TABLE [dbo].[Contracts](
	[cnID] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[cnN] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[cnA] [bit] NOT NULL,
	[cnMinDaysTours] [int] NOT NULL CONSTRAINT [DF_Contracts_cnMinDaysTours]  DEFAULT (0),
	[cnum] [tinyint] NOT NULL ,
 CONSTRAINT [PK_Contracts] PRIMARY KEY CLUSTERED 
(
	[cnID] ASC
) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Contracts] ADD 
	CONSTRAINT [FK_Contracts_UnavailMots] FOREIGN KEY 
	(
		[cnum]
	) REFERENCES [dbo].[UnavailMots] (
		[umID]
	)
GO


exec sp_addextendedproperty N'MS_Description', N'Catálogo de contratos', N'user', N'dbo', N'table', N'Contracts'

GO

exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Contracts', N'column', N'cnID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'Contracts', N'column', N'cnN'
GO
exec sp_addextendedproperty N'MS_Description', N'Número de días mínimo para que se le puedan otorgar los tours incluidos', N'user', N'dbo', N'table', N'Contracts', N'column', N'cnMinDaysTours'
GO
exec sp_addextendedproperty N'MS_Description', N'Indica si el contrato está activo', N'user', N'dbo', N'table', N'Contracts', N'column', N'cnA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave del motivo de indisponibilidad', N'user', N'dbo', N'table', N'Contracts', N'column', N'cnum'


GO
