USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[Groups]    Script Date: 06/06/2014 14:36:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Groups](
	[gbID] [varchar](10) NOT NULL,
	[gbN] [varchar](50) NOT NULL,
	[gbgu] [int] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[gbID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Guests] FOREIGN KEY([gbgu])
REFERENCES [dbo].[Guests] ([guID])
GO

ALTER TABLE [dbo].[Groups] CHECK CONSTRAINT [FK_Groups_Guests]
GO

