USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SalesmenChanges_Personnel_AuthorizedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[SalesmenChanges]'))
ALTER TABLE [dbo].[SalesmenChanges] DROP CONSTRAINT [FK_SalesmenChanges_Personnel_AuthorizedBy]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SalesmenChanges_Personnel_MadeBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[SalesmenChanges]'))
ALTER TABLE [dbo].[SalesmenChanges] DROP CONSTRAINT [FK_SalesmenChanges_Personnel_MadeBy]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SalesmenChanges_Personnel_NewSalesman]') AND parent_object_id = OBJECT_ID(N'[dbo].[SalesmenChanges]'))
ALTER TABLE [dbo].[SalesmenChanges] DROP CONSTRAINT [FK_SalesmenChanges_Personnel_NewSalesman]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SalesmenChanges_Personnel_OldSalesman]') AND parent_object_id = OBJECT_ID(N'[dbo].[SalesmenChanges]'))
ALTER TABLE [dbo].[SalesmenChanges] DROP CONSTRAINT [FK_SalesmenChanges_Personnel_OldSalesman]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SalesmenChanges_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[SalesmenChanges]'))
ALTER TABLE [dbo].[SalesmenChanges] DROP CONSTRAINT [FK_SalesmenChanges_Roles]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SalesmenChanges_Sales]') AND parent_object_id = OBJECT_ID(N'[dbo].[SalesmenChanges]'))
ALTER TABLE [dbo].[SalesmenChanges] DROP CONSTRAINT [FK_SalesmenChanges_Sales]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[SalesmenChanges]    Script Date: 09/17/2015 11:40:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesmenChanges]') AND type in (N'U'))
DROP TABLE [dbo].[SalesmenChanges]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[SalesmenChanges]    Script Date: 09/17/2015 11:40:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SalesmenChanges](
	[schDT] [datetime] NOT NULL,
	[schsa] [int] NOT NULL,
	[schAuthorizedBy] [varchar](10) NOT NULL,
	[schMadeBy] [varchar](10) NOT NULL,
	[schro] [varchar](10) NOT NULL,
	[schPosition] [tinyint] NOT NULL,
	[schOldSalesman] [varchar](10) NULL,
	[schNewSalesman] [varchar](10) NULL,
 CONSTRAINT [PK_SalesmenChanges] PRIMARY KEY CLUSTERED 
(
	[schDT] ASC,
	[schsa] ASC,
	[schro] ASC,
	[schPosition] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha y hora del cambio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schDT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schsa'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la persona que autorizó los cambios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schAuthorizedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la persona que hizo los cambios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schMadeBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del rol' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schro'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de posición' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schPosition'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del antiguo vendedor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schOldSalesman'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del nuevo vendedor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schNewSalesman'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cambios de vendedores' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges'
GO

ALTER TABLE [dbo].[SalesmenChanges]  WITH CHECK ADD  CONSTRAINT [FK_SalesmenChanges_Personnel_AuthorizedBy] FOREIGN KEY([schAuthorizedBy])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[SalesmenChanges] CHECK CONSTRAINT [FK_SalesmenChanges_Personnel_AuthorizedBy]
GO

ALTER TABLE [dbo].[SalesmenChanges]  WITH CHECK ADD  CONSTRAINT [FK_SalesmenChanges_Personnel_MadeBy] FOREIGN KEY([schMadeBy])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[SalesmenChanges] CHECK CONSTRAINT [FK_SalesmenChanges_Personnel_MadeBy]
GO

ALTER TABLE [dbo].[SalesmenChanges]  WITH CHECK ADD  CONSTRAINT [FK_SalesmenChanges_Personnel_NewSalesman] FOREIGN KEY([schNewSalesman])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[SalesmenChanges] CHECK CONSTRAINT [FK_SalesmenChanges_Personnel_NewSalesman]
GO

ALTER TABLE [dbo].[SalesmenChanges]  WITH CHECK ADD  CONSTRAINT [FK_SalesmenChanges_Personnel_OldSalesman] FOREIGN KEY([schOldSalesman])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[SalesmenChanges] CHECK CONSTRAINT [FK_SalesmenChanges_Personnel_OldSalesman]
GO

ALTER TABLE [dbo].[SalesmenChanges]  WITH CHECK ADD  CONSTRAINT [FK_SalesmenChanges_Roles] FOREIGN KEY([schro])
REFERENCES [dbo].[Roles] ([roID])
GO

ALTER TABLE [dbo].[SalesmenChanges] CHECK CONSTRAINT [FK_SalesmenChanges_Roles]
GO

ALTER TABLE [dbo].[SalesmenChanges]  WITH CHECK ADD  CONSTRAINT [FK_SalesmenChanges_Sales] FOREIGN KEY([schsa])
REFERENCES [dbo].[Sales] ([saID])
GO

ALTER TABLE [dbo].[SalesmenChanges] CHECK CONSTRAINT [FK_SalesmenChanges_Sales]
GO


