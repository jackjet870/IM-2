if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SeasonsDates_Seasons]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[SeasonsDates] DROP CONSTRAINT FK_SeasonsDates_Seasons
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Seasons]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Seasons]
GO

CREATE TABLE [dbo].[Seasons] (
	[ssID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ssN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ssA] [bit] NOT NULL ,
	[ssClosingFactor] [smallmoney] NOT NULL 
) ON [PRIMARY]
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'Seasons', N'column', N'ssA'
GO
exec sp_addextendedproperty N'MS_Description', N'Porcentaje de cierre', N'user', N'dbo', N'table', N'Seasons', N'column', N'ssClosingFactor'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'Seasons', N'column', N'ssID'
GO
exec sp_addextendedproperty N'MS_Description', N'Nombre', N'user', N'dbo', N'table', N'Seasons', N'column', N'ssN'


GO

