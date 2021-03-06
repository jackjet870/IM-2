USE [OrigosVCPalace]
GO
/****** Object:  Table [dbo].[CxCPayments]    Script Date: 08/13/2015 16:04:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CxCPayments](
	[cxID] [int] IDENTITY(1,1) NOT NULL,
	[cxgr] [int] NOT NULL,
	[cxReceivedBy] [varchar](10) NOT NULL,
	[cxAmount] [money] NULL,
	[cxExchangeRate] [money] NULL,
	[cxAmountMXN] [money] NULL,
	[cxD] [datetime] NOT NULL CONSTRAINT [DF__CxCPayments__cxD__465423DD]  DEFAULT (getdate()),
	[cxSeq] [int] NULL,
 CONSTRAINT [PK_CxCPayments] PRIMARY KEY CLUSTERED 
(
	[cxID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CxCPayments]  WITH CHECK ADD  CONSTRAINT [FK_CxCPayments_GiftsReceipts] FOREIGN KEY([cxgr])
REFERENCES [dbo].[GiftsReceipts] ([grID])
GO
ALTER TABLE [dbo].[CxCPayments] CHECK CONSTRAINT [FK_CxCPayments_GiftsReceipts]
GO
ALTER TABLE [dbo].[CxCPayments]  WITH CHECK ADD  CONSTRAINT [FK_CxCPayments_Personnel] FOREIGN KEY([cxReceivedBy])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[CxCPayments] CHECK CONSTRAINT [FK_CxCPayments_Personnel]