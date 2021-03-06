USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[Sales]    Fecha de la secuencia de comandos: 05/30/2013 12:31:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales](
	[saID] [int] IDENTITY(1,1) NOT NULL,
	[saMembershipNum] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[sagu] [int] NULL,
	[saD] [datetime] NOT NULL,
	[sast] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[saReference] [int] NULL,
	[saRefMember] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saUpdated] [bit] NOT NULL CONSTRAINT [DF_Sales_saUpdated]  DEFAULT (0),
	[samt] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[saLastName1] [varchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[saFirstName1] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[saLastName2] [varchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saFirstName2] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saOriginalAmount] [money] NOT NULL CONSTRAINT [DF_Sales_saNewAmount1]  DEFAULT (0),
	[saNewAmount] [money] NULL CONSTRAINT [DF_Sales_saAmount1]  DEFAULT (0),
	[saGrossAmount] [money] NOT NULL CONSTRAINT [DF_Sales_saAmount]  DEFAULT (0),
	[saProc] [bit] NOT NULL CONSTRAINT [DF_Sales_saProc]  DEFAULT (0),
	[saProcD] [datetime] NULL,
	[saCancel] [bit] NOT NULL CONSTRAINT [DF_Sales_saCancel]  DEFAULT (0),
	[saCancelD] [datetime] NULL,
	[salo] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[sals] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[sasr] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saPR1] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saSelfGen] [bit] NOT NULL CONSTRAINT [DF_Sales_saCancel1]  DEFAULT (0),
	[saPR2] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saPR3] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saPRCaptain1] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saPRCaptain2] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saPRCaptain3] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saLiner1Type] [tinyint] NOT NULL CONSTRAINT [DF_Sales_saCloser1P1_3]  DEFAULT (0),
	[saLiner1] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saLiner2] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saLinerCaptain1] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saCloser1] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saCloser2] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saCloser3] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saExit1] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saExit2] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saCloserCaptain1] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saPodium] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saVLO] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saLiner1P] [tinyint] NOT NULL CONSTRAINT [DF_Sales_saCloser1P1_2]  DEFAULT (0),
	[saCloser1P] [tinyint] NOT NULL CONSTRAINT [DF_Sales_saCloser1P]  DEFAULT (0),
	[saCloser2P] [tinyint] NOT NULL CONSTRAINT [DF_Sales_saCloser1P1]  DEFAULT (0),
	[saCloser3P] [tinyint] NOT NULL CONSTRAINT [DF_Sales_saCloser1P1_1]  DEFAULT (0),
	[saExit1P] [tinyint] NOT NULL CONSTRAINT [DF_Sales_saCloser3P1]  DEFAULT (0),
	[saExit2P] [tinyint] NOT NULL CONSTRAINT [DF_Sales_saExit1P1]  DEFAULT (0),
	[saClosingCost] [money] NOT NULL CONSTRAINT [DF_Sales_saClosingCost]  DEFAULT (0),
	[saOverPack] [money] NOT NULL CONSTRAINT [DF_Sales_saOverPack]  DEFAULT (0),
	[saComments] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saDeposit] [money] NOT NULL DEFAULT (0),
	[saProcRD] [datetime] NULL,
	[saByPhone] [bit] NOT NULL DEFAULT (0),
	[samtGlobal] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[saCompany] [int] NOT NULL CONSTRAINT [DF_Sales_saCompany]  DEFAULT ((0)),
	[saDownPayment] [money] NOT NULL CONSTRAINT [DF_Sales_saDownPayment]  DEFAULT ((0)),
	[saDownPaymentPercentage] [money] NOT NULL CONSTRAINT [DF_Sales_saDownPaymentPercentage]  DEFAULT ((0)),
	[saDownPaymentPaid] [money] NOT NULL CONSTRAINT [DF_Sales_saDownPaymentPaid]  DEFAULT ((0)),
	[saDownPaymentPaidPercentage] [money] NOT NULL CONSTRAINT [DF_Sales_saDownPaymentPaidPercentage]  DEFAULT ((0)),
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[saID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de membresía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saMembershipNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de huesped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'sagu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de tipo de venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'sast'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la venta de referencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saReference'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de la membresía de referencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saRefMember'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la venta esta referenciada por otra' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saUpdated'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de tipo de membresia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'samt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellido' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saLastName1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saFirstName1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Apellido de la pareja' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saLastName2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nombre de la pareja' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saFirstName2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de la membresia anterior' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saOriginalAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de la venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saNewAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto neto de la venta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saGrossAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la venta es procesable' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saProc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de procesable' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saProcD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la venta está cancelada' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCancel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de cancelación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCancelD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de locación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'salo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de Lead Source' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'sals'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de sala de ventas' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'sasr'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saPR1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la venta es Self Gen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saSelfGen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saPR2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 3er PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saPR3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán del PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saPRCaptain1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán del 2do PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saPRCaptain2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán del 3er PR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saPRCaptain3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saLiner1Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saLiner1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saLiner2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán de Liners' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saLinerCaptain1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCloser1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCloser2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 3er Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCloser3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saExit1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del 2do Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saExit2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del capitán de Closers' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCloserCaptain1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del Podium' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saPodium'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del verificador legal' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saVLO'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de volumen del Liner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saLiner1P'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de volumen del Closer 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCloser1P'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de volumen del 2do Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCloser2P'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de volumen del 3er Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCloser3P'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de volumen del Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saExit1P'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de volumen del 2do Exit Closer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saExit2P'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costo de cierre' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saClosingCost'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comentarios' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saComments'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fecha de procesable de la membresia de referencia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saProcRD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica si la venta fue hecha por teléfono' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saByPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de tipo de membresía global' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'samtGlobal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave de compañía' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saCompany'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de enganche pactado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saDownPayment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de enganche pactado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saDownPaymentPercentage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Monto de enganche pagado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saDownPaymentPaid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Porcentaje de enganche pagado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Sales', @level2type=N'COLUMN',@level2name=N'saDownPaymentPaidPercentage'
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Guests] FOREIGN KEY([sagu])
REFERENCES [dbo].[Guests] ([guID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Guests]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_LeadSources] FOREIGN KEY([sals])
REFERENCES [dbo].[LeadSources] ([lsID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_LeadSources]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Locations] FOREIGN KEY([salo])
REFERENCES [dbo].[Locations] ([loID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Locations]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_MembershipTypes] FOREIGN KEY([samt])
REFERENCES [dbo].[MembershipTypes] ([mtID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_MembershipTypes]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_MembershipTypes_Global] FOREIGN KEY([samtGlobal])
REFERENCES [dbo].[MembershipTypes] ([mtID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_MembershipTypes_Global]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_Closer1] FOREIGN KEY([saCloser1])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_Closer1]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_Closer2] FOREIGN KEY([saCloser2])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_Closer2]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_Closer3] FOREIGN KEY([saCloser3])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_Closer3]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_CloserCaptain] FOREIGN KEY([saCloserCaptain1])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_CloserCaptain]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_Exit1] FOREIGN KEY([saExit1])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_Exit1]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_Exit2] FOREIGN KEY([saExit2])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_Exit2]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_Podium] FOREIGN KEY([saPodium])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_Podium]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_PR1] FOREIGN KEY([saPR1])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_PR1]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_PR2] FOREIGN KEY([saPR2])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_PR2]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_PR3] FOREIGN KEY([saPR3])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_PR3]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_PRCaptain1] FOREIGN KEY([saPRCaptain1])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_PRCaptain1]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_PRCaptain2] FOREIGN KEY([saPRCaptain2])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_PRCaptain2]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_PRCaptain3] FOREIGN KEY([saPRCaptain3])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_PRCaptain3]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Personnel_VLO] FOREIGN KEY([saVLO])
REFERENCES [dbo].[Personnel] ([peID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Personnel_VLO]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Sales] FOREIGN KEY([saReference])
REFERENCES [dbo].[Sales] ([saID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Sales]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SalesRooms] FOREIGN KEY([sasr])
REFERENCES [dbo].[SalesRooms] ([srID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_SalesRooms]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SaleTypes] FOREIGN KEY([sast])
REFERENCES [dbo].[SaleTypes] ([stID])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_SaleTypes]