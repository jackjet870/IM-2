USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[MembershipTypes]    Fecha de la secuencia de comandos: 05/30/2013 13:35:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MembershipTypes](
	[mtID] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[mtN] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[mtGroup] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[mtA] [bit] NOT NULL CONSTRAINT [DF_MembershipTypes_mtA]  DEFAULT (1),
	[mtLevel] [tinyint] NULL,
 CONSTRAINT [PK_MembTypes] PRIMARY KEY CLUSTERED 
(
	[mtID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MembershipTypes', @level2type=N'COLUMN',@level2name=N'mtID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MembershipTypes', @level2type=N'COLUMN',@level2name=N'mtN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de grupo de membresías' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MembershipTypes', @level2type=N'COLUMN',@level2name=N'mtGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MembershipTypes', @level2type=N'COLUMN',@level2name=N'mtA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nivel' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MembershipTypes', @level2type=N'COLUMN',@level2name=N'mtLevel'
GO
ALTER TABLE [dbo].[MembershipTypes]  WITH CHECK ADD  CONSTRAINT [FK_MembershipTypes_MembershipGroups] FOREIGN KEY([mtGroup])
REFERENCES [dbo].[MembershipGroups] ([mgID])
GO
ALTER TABLE [dbo].[MembershipTypes] CHECK CONSTRAINT [FK_MembershipTypes_MembershipGroups]