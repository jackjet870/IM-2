USE [OrigosVCPalace]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BookingDeposits_PaymentTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BookingDeposits]'))
ALTER TABLE [dbo].[BookingDeposits] DROP CONSTRAINT [FK_BookingDeposits_PaymentTypes]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__BookingDe__bdAmo__0E44098D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BookingDeposits] DROP CONSTRAINT [DF__BookingDe__bdAmo__0E44098D]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__BookingDe__bdRec__0F382DC6]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BookingDeposits] DROP CONSTRAINT [DF__BookingDe__bdRec__0F382DC6]
END

GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[BookingDeposits]    Script Date: 11/14/2013 09:49:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BookingDeposits]') AND type in (N'U'))
DROP TABLE [dbo].[BookingDeposits]
GO

USE [OrigosVCPalace]
GO

/****** Object:  Table [dbo].[BookingDeposits]    Script Date: 11/14/2013 09:49:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[BookingDeposits](
	[bdID] [int] IDENTITY(1,1) NOT NULL,
	[bdgu] [int] NOT NULL,
	[bdcu] [varchar](10) NOT NULL,
	[bdAmount] [money] NOT NULL,
	[bdReceived] [money] NOT NULL,
	[bdpt] [varchar](10) NULL,
 CONSTRAINT [PK_BookingDeposits] PRIMARY KEY CLUSTERED 
(
	[bdID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica la forma de pago del depósito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BookingDeposits', @level2type=N'COLUMN',@level2name=N'bdpt'
GO

ALTER TABLE [dbo].[BookingDeposits]  WITH CHECK ADD  CONSTRAINT [FK_BookingDeposits_PaymentTypes] FOREIGN KEY([bdpt])
REFERENCES [dbo].[PaymentTypes] ([ptID])
GO

ALTER TABLE [dbo].[BookingDeposits] CHECK CONSTRAINT [FK_BookingDeposits_PaymentTypes]
GO

ALTER TABLE [dbo].[BookingDeposits] ADD  CONSTRAINT [DF__BookingDe__bdAmo__0E44098D]  DEFAULT ((0)) FOR [bdAmount]
GO

ALTER TABLE [dbo].[BookingDeposits] ADD  CONSTRAINT [DF__BookingDe__bdRec__0F382DC6]  DEFAULT ((0)) FOR [bdReceived]
GO

