USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PersonnelSalesRoomsLog]    Script Date: 10/14/2016 10:04:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PersonnelSalesRoomsLog](
	[pslplg] [int] NOT NULL,
	[pslsr] [varchar](10) NOT NULL,
 CONSTRAINT [PK_PersonnelSalesRoomsLog] PRIMARY KEY CLUSTERED 
(
	[pslplg] ASC,
	[pslsr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Personnel Log' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelSalesRoomsLog', @level2type=N'COLUMN',@level2name=N'pslplg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelSalesRoomsLog', @level2type=N'COLUMN',@level2name=N'pslsr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Historico de cambios de las salas de venta del personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PersonnelSalesRoomsLog'
GO

ALTER TABLE [dbo].[PersonnelSalesRoomsLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelSalesRoomsLog_PersonnelLog] FOREIGN KEY([pslplg])
REFERENCES [dbo].[PersonnelLog] ([plgID])
GO

ALTER TABLE [dbo].[PersonnelSalesRoomsLog] CHECK CONSTRAINT [FK_PersonnelSalesRoomsLog_PersonnelLog]
GO

ALTER TABLE [dbo].[PersonnelSalesRoomsLog]  WITH CHECK ADD  CONSTRAINT [FK_PersonnelSalesRoomsLog_SalesRooms] FOREIGN KEY([pslsr])
REFERENCES [dbo].[SalesRooms] ([srID])
GO

ALTER TABLE [dbo].[PersonnelSalesRoomsLog] CHECK CONSTRAINT [FK_PersonnelSalesRoomsLog_SalesRooms]
GO


