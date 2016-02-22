USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[FolioCxCCancellation]    Script Date: 01/12/2016 17:22:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FolioCxCCancellation](
	[fccID] [int] IDENTITY(1,1) NOT NULL,
	[fccpe] [varchar](10) NOT NULL,
	[fccFrom] [int] NOT NULL,
	[fccTo] [int] NOT NULL,
	[fccrcf] [varchar](3) NOT NULL,
 CONSTRAINT [PK_FolioCxCCancellation] PRIMARY KEY CLUSTERED 
(
	[fccID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID de la tabla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCCancellation', @level2type=N'COLUMN',@level2name=N'fccID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'llave foránea de la tabla FolioCxCCancellation con PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCCancellation', @level2type=N'COLUMN',@level2name=N'fccpe'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inicio del rango de folios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCCancellation', @level2type=N'COLUMN',@level2name=N'fccFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Final del rango de folios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCCancellation', @level2type=N'COLUMN',@level2name=N'fccTo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'llave foránea de la tabla FolioCxCCancellation con ReasonCancellationFolios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioCxCCancellation', @level2type=N'COLUMN',@level2name=N'fccrcf'
GO

ALTER TABLE [dbo].[FolioCxCCancellation]  WITH CHECK ADD  CONSTRAINT [FK_FolioCxCCancellation_Personnel] FOREIGN KEY([fccpe])
REFERENCES [dbo].[Personnel] ([peID])
GO

ALTER TABLE [dbo].[FolioCxCCancellation] CHECK CONSTRAINT [FK_FolioCxCCancellation_Personnel]
GO

ALTER TABLE [dbo].[FolioCxCCancellation]  WITH CHECK ADD  CONSTRAINT [FK_FolioCxCCancellation_ReasonCancellationFolios] FOREIGN KEY([fccrcf])
REFERENCES [dbo].[ReasonCancellationFolios] ([rcfID])
GO

ALTER TABLE [dbo].[FolioCxCCancellation] CHECK CONSTRAINT [FK_FolioCxCCancellation_ReasonCancellationFolios]
GO

EXEC dbo.USP_OR_ADDPREFIX 'fcc','FolioCxCCancellation','Catálogo de folios cancelados de CxC'
