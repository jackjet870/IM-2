USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[MealTicketFolios]    Script Date: 02/07/2014 11:49:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MealTicketFolios](
	[mfsr] [varchar](10) NOT NULL,
	[mfmy] [varchar](10) NOT NULL,
	[mfFolio] [varchar](30) NOT NULL,
 CONSTRAINT [PK_MealTicketFolios] PRIMARY KEY CLUSTERED 
(
	[mfsr] ASC,
	[mfmy] ASC,
	[mfFolio] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MealTicketFolios', @level2type=N'COLUMN',@level2name=N'mfsr'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'clave tipo de comida' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MealTicketFolios', @level2type=N'COLUMN',@level2name=N'mfmy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de folio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MealTicketFolios', @level2type=N'COLUMN',@level2name=N'mfFolio'
GO

ALTER TABLE [dbo].[MealTicketFolios]  WITH CHECK ADD  CONSTRAINT [FK_MealTicketFolios_MealTicketFolios] FOREIGN KEY([mfmy])
REFERENCES [dbo].[MealTicketTypes] ([myID])
GO

ALTER TABLE [dbo].[MealTicketFolios] CHECK CONSTRAINT [FK_MealTicketFolios_MealTicketFolios]
GO

ALTER TABLE [dbo].[MealTicketFolios]  WITH CHECK ADD  CONSTRAINT [FK_MealTicketFolios_SalesRooms1] FOREIGN KEY([mfsr])
REFERENCES [dbo].[SalesRooms] ([srID])
GO

ALTER TABLE [dbo].[MealTicketFolios] CHECK CONSTRAINT [FK_MealTicketFolios_SalesRooms1]
GO


