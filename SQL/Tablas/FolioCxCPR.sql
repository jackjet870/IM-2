USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[FolioCxCPR]    Script Date: 01/13/2016 11:34:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FolioCxCPR](
	[fcpID] [int] IDENTITY(1,1) NOT NULL,
	[fcppe] [varchar](10) NOT NULL,
	[fcpFrom] [int] NOT NULL,
	[fcpTo] [int] NOT NULL,
 CONSTRAINT [PK__FolioCxCPR__2918C6B1] PRIMARY KEY CLUSTERED 
(
	[fcpID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID de la tabla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCPR', @level2type=N'COLUMN',@level2name=N'fcpID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'llave foránea de FolioCxCPR con PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCPR', @level2type=N'COLUMN',@level2name=N'fcppe'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inicio del rango de los folios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCPR', @level2type=N'COLUMN',@level2name=N'fcpFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Final del rango de los folios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCPR', @level2type=N'COLUMN',@level2name=N'fcpTo'
GO

ALTER TABLE [dbo].[FolioCxCPR]  WITH CHECK ADD  CONSTRAINT [FK_FolioCxCPR_Personnel] FOREIGN KEY([fcppe])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[FolioCxCPR] CHECK CONSTRAINT [FK_FolioCxCPR_Personnel]
GO


