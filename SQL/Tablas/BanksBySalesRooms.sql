USE [OrigosVCPalace]
GO
/****** Object:  Table [dbo].[BanksBySalesRooms]    Script Date: 07/03/2015 13:14:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BanksBySalesRooms](
	[bsbk] [varchar](10) NOT NULL,
	[bssr] [varchar](10) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[BanksBySalesRooms]  WITH CHECK ADD  CONSTRAINT [FK_BanksBySalesRooms_Banks] FOREIGN KEY([bsbk])
REFERENCES [dbo].[Banks] ([bkID])
GO
ALTER TABLE [dbo].[BanksBySalesRooms] CHECK CONSTRAINT [FK_BanksBySalesRooms_Banks]
GO
ALTER TABLE [dbo].[BanksBySalesRooms]  WITH CHECK ADD  CONSTRAINT [FK_BanksBySalesRooms_SalesRooms] FOREIGN KEY([bssr])
REFERENCES [dbo].[SalesRooms] ([srID])
GO
ALTER TABLE [dbo].[BanksBySalesRooms] CHECK CONSTRAINT [FK_BanksBySalesRooms_SalesRooms]