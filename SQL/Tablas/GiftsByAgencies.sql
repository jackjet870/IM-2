USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[GiftsByAgencies]    Script Date: 05/14/2014 11:17:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[GiftsByAgencies](
	[gbgi] [varchar](10) NOT NULL,
	[gbag] [varchar](35) NOT NULL,
 CONSTRAINT [PK_GiftsByAgencies] PRIMARY KEY CLUSTERED 
(
	[gbgi] ASC,
	[gbag] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del regalo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsByAgencies', @level2type=N'COLUMN',@level2name=N'gbgi'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la agencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GiftsByAgencies', @level2type=N'COLUMN',@level2name=N'gbag'
GO

ALTER TABLE [dbo].[GiftsByAgencies]  WITH CHECK ADD  CONSTRAINT [FK_GiftsByAgencies_Agencies] FOREIGN KEY([gbag])
REFERENCES [dbo].[Agencies] ([agID])
GO

ALTER TABLE [dbo].[GiftsByAgencies] CHECK CONSTRAINT [FK_GiftsByAgencies_Agencies]
GO

ALTER TABLE [dbo].[GiftsByAgencies]  WITH CHECK ADD  CONSTRAINT [FK_GiftsByAgencies_Gifts] FOREIGN KEY([gbgi])
REFERENCES [dbo].[Gifts] ([giID])
GO

ALTER TABLE [dbo].[GiftsByAgencies] CHECK CONSTRAINT [FK_GiftsByAgencies_Gifts]
GO


