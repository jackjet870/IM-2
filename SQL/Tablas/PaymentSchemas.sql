USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[PaymentSchemas]    Script Date: 12/16/2015 13:08:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PaymentSchemas](
	[pasID] [int] IDENTITY(1,1) NOT NULL,
	[pasN] [varchar](30) NULL,
	[pasCoupleFrom] [int] NULL,
	[pasCoupleTo] [int] NULL,
	[pasShowFactor] [money] NULL,
	[pasA] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[pasID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PaymentSchemas] ADD  DEFAULT ((1)) FOR [pasA]
GO


