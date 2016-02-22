USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Wholesalers]    Script Date: 02/28/2015 11:00:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Wholesalers](
	[wscl] [int] NOT NULL,
	[wsCompany] [decimal](2, 0) NOT NULL,
	[wsApplication] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Wholesalers] PRIMARY KEY CLUSTERED 
(
	[wscl] ASC,
	[wsCompany] ASC,
	[wsApplication] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del club' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Wholesalers', @level2type=N'COLUMN',@level2name=N'wscl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la compañía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Wholesalers', @level2type=N'COLUMN',@level2name=N'wsCompany'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Wholesalers', @level2type=N'COLUMN',@level2name=N'wsApplication'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Catálogo de membresías mayoristas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Wholesalers'
GO

ALTER TABLE [dbo].[Wholesalers]  WITH CHECK ADD  CONSTRAINT [FK_Wholesalers_Clubs] FOREIGN KEY([wscl])
REFERENCES [dbo].[Clubs] ([clID])
GO

ALTER TABLE [dbo].[Wholesalers] CHECK CONSTRAINT [FK_Wholesalers_Clubs]
GO


