USE [OrigosVCPalace]
GO
/****** Object:  Table [dbo].[ReimpresionMotives]    Script Date: 12/18/2013 16:53:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReimpresionMotives](
	[rmID] [tinyint] NOT NULL,
	[rmN] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[rmA] [bit] NOT NULL,
 CONSTRAINT [PK_ReimpresionMotives] PRIMARY KEY CLUSTERED 
(
	[rmID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF