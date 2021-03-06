if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AssistanceStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AssistanceStatus]
GO

CREATE TABLE [dbo].[AssistanceStatus] (
	[atID] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[atN] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[atA] [bit] NOT NULL 
) ON [PRIMARY]
GO


exec sp_addextendedproperty N'MS_Description', N'Activo', N'user', N'dbo', N'table', N'AssistanceStatus', N'column', N'atA'
GO
exec sp_addextendedproperty N'MS_Description', N'Clave', N'user', N'dbo', N'table', N'AssistanceStatus', N'column', N'atID'
GO
exec sp_addextendedproperty N'MS_Description', N'Descripción', N'user', N'dbo', N'table', N'AssistanceStatus', N'column', N'atN'


GO

