USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[TransferMemberships]    Fecha de la secuencia de comandos: 05/28/2013 16:36:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransferMemberships]') AND type in (N'U'))
DROP TABLE [dbo].[TransferMemberships]
GO
/****** Objeto:  Table [dbo].[TransferMemberships]    Fecha de la secuencia de comandos: 05/28/2013 16:36:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransferMemberships](
	[tmCompany] [int] NOT NULL,
	[tmApplication] [varchar](12) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmFirstName] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmLastName] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmFirstName2] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmLastName2] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmSaleD] [datetime] NOT NULL,
	[tmProcD] [datetime] NULL,
	[tmCancel] [bit] NOT NULL,
	[tmCancelD] [datetime] NULL,
	[tmst] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmstN] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmmt] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmmtN] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[tmTotal] [money] NOT NULL,
	[tmSubtotal] [money] NOT NULL,
	[tmDownPayment] [money] NOT NULL,
	[tmDownPaymentPercentage] [money] NOT NULL,
	[tmDownPaymentPaid] [money] NOT NULL,
	[tmDownPaymentPaidPercentage] [money] NOT NULL,
	[tmPreviousApplication] [varchar](12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[tmPreviousTotal] [money] NULL,
	[tmPreviousSubtotal] [money] NULL,
	[tmEditDT] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de compañía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmCompany'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmApplication'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmFirstName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellido' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmLastName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre del copropietario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmFirstName2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellido del copropietario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmLastName2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmSaleD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de procesable' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmProcD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la membresía está cancelada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmCancel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cancelación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmCancelD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmst'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del tipo de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmstN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmmt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descripción del tipo de membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmmtN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto total' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto sin IVA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmSubtotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de enganche pactado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmDownPayment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de enganche pactado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmDownPaymentPercentage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de enganche pagado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmDownPaymentPaid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de enganche pagado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmDownPaymentPaidPercentage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la membresía anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmPreviousApplication'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de la membresía anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmPreviousTotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto sin IVA de la membresía anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmPreviousSubtotal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha y hora de modificación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships', @level2type=N'COLUMN',@level2name=N'tmEditDT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla de transferencia de membresías' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferMemberships'