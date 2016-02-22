USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PersonnelRoles]    Script Date: 01/21/2014 17:07:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PersonnelRoles](
	[prpe] [varchar](10) NOT NULL,
	[prro] [varchar](10) NOT NULL,
 CONSTRAINT [PK_PersonnelRoles] PRIMARY KEY CLUSTERED 
(
	[prpe] ASC,
	[prro] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelRoles', @level2type=N'COLUMN',@level2name=N'prpe'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del rol' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelRoles', @level2type=N'COLUMN',@level2name=N'prro'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Roles de personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelRoles'
GO

ALTER TABLE [dbo].[PersonnelRoles]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelRoles_Personnel] FOREIGN KEY([prpe])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[PersonnelRoles] CHECK CONSTRAINT [FK_PersonnelRoles_Personnel]
GO

ALTER TABLE [dbo].[PersonnelRoles]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelRoles_Roles] FOREIGN KEY([prro])
REFERENCES [dbo].[Roles] ([roID])
GO

ALTER TABLE [dbo].[PersonnelRoles] CHECK CONSTRAINT [FK_PersonnelRoles_Roles]
GO


