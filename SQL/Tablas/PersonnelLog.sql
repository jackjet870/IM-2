USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PersonnelLog]    Script Date: 10/14/2016 09:48:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PersonnelLog](
	[plgID] [int] IDENTITY(1,1) NOT NULL,
	[plgpe] [varchar](10) NOT NULL,
	[plgDT] [datetime] NOT NULL,
	[plgChangedBy] [varchar](10) NOT NULL,
	[plgde] [varchar](10) NOT NULL,
	[plgpo] [varchar](10) NOT NULL,
	[plgsr] [varchar](10) NULL,
	[plglo] [varchar](10) NULL,
 CONSTRAINT [PK_PersonnelLog] PRIMARY KEY CLUSTERED 
(
	[plgID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plgID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plgpe'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha y hora de la modificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plgDT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del usuario que modificó' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plgChangedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del departamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plgde'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del puesto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plgpo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plgsr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la Locacion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog', @level2type=N'COLUMN',@level2name=N'plglo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Historico de cambios del personnal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelLog'
GO

ALTER TABLE [dbo].[PersonnelLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLog_Depts] FOREIGN KEY([plgde])
REFERENCES [dbo].[Depts] ([deID])
GO

ALTER TABLE [dbo].[PersonnelLog] CHECK CONSTRAINT [FK_PersonnelLog_Depts]
GO

ALTER TABLE [dbo].[PersonnelLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLog_Locations] FOREIGN KEY([plglo])
REFERENCES [dbo].[Locations] ([loID])
GO

ALTER TABLE [dbo].[PersonnelLog] CHECK CONSTRAINT [FK_PersonnelLog_Locations]
GO

ALTER TABLE [dbo].[PersonnelLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLog_Personnel] FOREIGN KEY([plgpe])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[PersonnelLog] CHECK CONSTRAINT [FK_PersonnelLog_Personnel]
GO

ALTER TABLE [dbo].[PersonnelLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLog_Personnel_ChangedBy] FOREIGN KEY([plgChangedBy])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[PersonnelLog] CHECK CONSTRAINT [FK_PersonnelLog_Personnel_ChangedBy]
GO

ALTER TABLE [dbo].[PersonnelLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLog_Posts] FOREIGN KEY([plgpo])
REFERENCES [dbo].[Posts] ([poID])
GO

ALTER TABLE [dbo].[PersonnelLog] CHECK CONSTRAINT [FK_PersonnelLog_Posts]
GO

ALTER TABLE [dbo].[PersonnelLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelLog_SalesRooms] FOREIGN KEY([plgsr])
REFERENCES [dbo].[SalesRooms] ([srID])
GO

ALTER TABLE [dbo].[PersonnelLog] CHECK CONSTRAINT [FK_PersonnelLog_SalesRooms]
GO


