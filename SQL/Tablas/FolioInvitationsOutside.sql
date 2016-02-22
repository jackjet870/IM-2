USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[FolioInvitationsOutside]    Script Date: 01/15/2014 11:00:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FolioInvitationsOutside](
	[fiID] [int] IDENTITY(1,1) NOT NULL,
	[fiSerie] [varchar](5) NOT NULL,
	[fiFrom] [int] NOT NULL,
	[fiTo] [int] NOT NULL,
	[fiA] [bit] NOT NULL,
 CONSTRAINT [PK_FolioInvitationsOutside] PRIMARY KEY CLUSTERED 
(
	[fiID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioInvitationsOutside', @level2type=N'COLUMN',@level2name=N'fiID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Serie' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioInvitationsOutside', @level2type=N'COLUMN',@level2name=N'fiSerie'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número desde' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioInvitationsOutside', @level2type=N'COLUMN',@level2name=N'fiFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Número hasta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioInvitationsOutside', @level2type=N'COLUMN',@level2name=N'fiTo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FolioInvitationsOutside', @level2type=N'COLUMN',@level2name=N'fiA'
GO

