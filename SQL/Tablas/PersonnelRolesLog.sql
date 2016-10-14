USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PersonnelRolesLog]    Script Date: 10/14/2016 09:53:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PersonnelRolesLog](
	[prlplg] [int] NOT NULL,
	[prlro] [varchar](10) NOT NULL,
 CONSTRAINT [PK_PersonnelRolesLog] PRIMARY KEY CLUSTERED 
(
	[prlplg] ASC,
	[prlro] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Personnel Log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelRolesLog', @level2type=N'COLUMN',@level2name=N'prlplg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Rol' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelRolesLog', @level2type=N'COLUMN',@level2name=N'prlro'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Historico de cambios de los roles del personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelRolesLog'
GO

ALTER TABLE [dbo].[PersonnelRolesLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelRolesLog_PersonnelLog] FOREIGN KEY([prlplg])
REFERENCES [dbo].[PersonnelLog] ([plgID])
GO

ALTER TABLE [dbo].[PersonnelRolesLog] CHECK CONSTRAINT [FK_PersonnelRolesLog_PersonnelLog]
GO

ALTER TABLE [dbo].[PersonnelRolesLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelRolesLog_Roles] FOREIGN KEY([prlro])
REFERENCES [dbo].[Roles] ([roID])
GO

ALTER TABLE [dbo].[PersonnelRolesLog] CHECK CONSTRAINT [FK_PersonnelRolesLog_Roles]
GO


