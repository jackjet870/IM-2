USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[UnderPaymentMotives]    Script Date: 12/19/2013 11:13:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UnderPaymentMotives](
	[upID] [int] IDENTITY(1,1) NOT NULL,
	[upN] [varchar](50) NOT NULL,
	[upA] [bit] NOT NULL,
 CONSTRAINT [PK_UnderPaymentMotives] PRIMARY KEY CLUSTERED 
(
	[upID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave primaria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UnderPaymentMotives', @level2type=N'COLUMN',@level2name=N'upID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del motivo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UnderPaymentMotives', @level2type=N'COLUMN',@level2name=N'upN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UnderPaymentMotives', @level2type=N'COLUMN',@level2name=N'upA'
GO
