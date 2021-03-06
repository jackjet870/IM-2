if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Efficiency_EfficiencyTypes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Efficiency] DROP CONSTRAINT FK_Efficiency_EfficiencyTypes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EfficiencyTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[EfficiencyTypes]
GO

CREATE TABLE [dbo].[EfficiencyTypes] (
	[etID] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[etN] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[etA] [bit] NOT NULL 
) ON [PRIMARY]
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'EfficiencyTypes', N'column', N'etA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'EfficiencyTypes', N'column', N'etID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'EfficiencyTypes', N'column', N'etN'


GO

