USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PersonnelPermissions]    Script Date: 01/21/2014 17:08:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PersonnelPermissions](
	[pppe] [varchar](10) NOT NULL,
	[pppm] [varchar](10) NOT NULL,
	[pppl] [int] NOT NULL,
 CONSTRAINT [PK_PersonnelPermissions] PRIMARY KEY CLUSTERED 
(
	[pppe] ASC,
	[pppm] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelPermissions', @level2type=N'COLUMN',@level2name=N'pppe'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del permiso' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelPermissions', @level2type=N'COLUMN',@level2name=N'pppm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del nivel de permiso' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelPermissions', @level2type=N'COLUMN',@level2name=N'pppl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Permisos de personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelPermissions'
GO

ALTER TABLE [dbo].[PersonnelPermissions]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelPermissions_Permissions] FOREIGN KEY([pppm])
REFERENCES [dbo].[Permissions] ([pmID])
GO

ALTER TABLE [dbo].[PersonnelPermissions] CHECK CONSTRAINT [FK_PersonnelPermissions_Permissions]
GO

ALTER TABLE [dbo].[PersonnelPermissions]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelPermissions_PermissionsLevels] FOREIGN KEY([pppl])
REFERENCES [dbo].[PermissionsLevels] ([plID])
GO

ALTER TABLE [dbo].[PersonnelPermissions] CHECK CONSTRAINT [FK_PersonnelPermissions_PermissionsLevels]
GO

ALTER TABLE [dbo].[PersonnelPermissions]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelPermissions_Personnel] FOREIGN KEY([pppe])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[PersonnelPermissions] CHECK CONSTRAINT [FK_PersonnelPermissions_Personnel]
GO


