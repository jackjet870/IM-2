USE [OrigosVCPalace]
GO
/****** Objeto:  Table [dbo].[RoomCharges]    Fecha de la secuencia de comandos: 09/21/2013 14:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RoomCharges](
	[rhls] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[rhFolio] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[rhConsecutive] [int] NOT NULL,
 CONSTRAINT [PK_RoomCharges] PRIMARY KEY CLUSTERED 
(
	[rhls] ASC,
	[rhFolio] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del hotel de la reservación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomCharges', @level2type=N'COLUMN',@level2name=N'rhls'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Folio de la reservación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomCharges', @level2type=N'COLUMN',@level2name=N'rhFolio'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Consecutivo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomCharges', @level2type=N'COLUMN',@level2name=N'rhConsecutive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cargos a habitación' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomCharges'
GO
ALTER TABLE [dbo].[RoomCharges]  WITH CHECK ADD  CONSTRAINT [FK_RoomCharges_LeadSources] FOREIGN KEY([rhls])
REFERENCES [dbo].[LeadSources] ([lsID])
GO
ALTER TABLE [dbo].[RoomCharges] CHECK CONSTRAINT [FK_RoomCharges_LeadSources]