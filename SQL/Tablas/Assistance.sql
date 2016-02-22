USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Assistance]    Script Date: 07/02/2014 12:49:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Assistance](
	[asPlaceType] [varchar](10) NOT NULL,
	[asPlaceID] [varchar](10) NOT NULL,
	[asStartD] [datetime] NOT NULL,
	[asEndD] [datetime] NOT NULL,
	[aspe] [varchar](10) NOT NULL,
	[asMonday] [varchar](10) NOT NULL,
	[asTuesday] [varchar](10) NOT NULL,
	[asWednesday] [varchar](10) NOT NULL,
	[asThursday] [varchar](10) NOT NULL,
	[asFriday] [varchar](10) NOT NULL,
	[asSaturday] [varchar](10) NOT NULL,
	[asSunday] [varchar](10) NOT NULL,
	[asNum]  AS ([dbo].[UFN_OR_ObtenerNumAsistencias]([asMonday],[asTuesday],[asWednesday],[asThursday],[asFriday],[asSaturday],[asSunday])),
 CONSTRAINT [PK_Assistance] PRIMARY KEY CLUSTERED 
(
	[asPlaceID] ASC,
	[asStartD] ASC,
	[asEndD] ASC,
	[aspe] ASC,
	[asPlaceType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de lugar' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asPlaceType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asPlaceID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha desde' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asStartD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha hasta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asEndD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del personal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'aspe'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lunes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asMonday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Martes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asTuesday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Miércoles' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asWednesday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Jueves' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asThursday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Viernes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asFriday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sábado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asSaturday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Domingo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asSunday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número de asistencias' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Assistance', @level2type=N'COLUMN',@level2name=N'asNum'
GO

ALTER TABLE [dbo].[Assistance]  WITH CHECK ADD  CONSTRAINT [FK_Assistance_Personnel] FOREIGN KEY([aspe])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[Assistance] CHECK CONSTRAINT [FK_Assistance_Personnel]
GO


