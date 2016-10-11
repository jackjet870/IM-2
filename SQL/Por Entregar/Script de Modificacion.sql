/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de modificacion de tablas
** 
** [wtorres]	20/Feb/2016 Created
**
*/
use OrigosVCPalace

-- I. MODIFICACIONES DE TABLAS
--		1. Huespedes

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:		Huespedes
-- [wtorres]	11/Oct/2016 Aumente el ancho del campo de folio de invitacion Outhouse. Antes era de 8 caracteres
-- =============================================
ALTER TABLE Guests
	ALTER COLUMN guOutInvitNum varchar(15) NULL
GO

-- =============================================
--			SalesmenChanges
-- =============================================
USE [OrigosVCPalace]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[SalesmenChanges]
ADD [schgu]int NULL,
	[schmt] varchar(10) NOT NULL DEFAULT 'SL'

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GuestID a quién se le hizo el cambio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schgu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de movimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schmt'
GO

GO
ALTER TABLE [dbo].[SalesmenChanges] 
WITH CHECK ADD
CONSTRAINT [FK_SalesmenChanges_GuestMovementsType] FOREIGN KEY([schmt])
REFERENCES [dbo].[GuestsMovementsTypes] ([gnID])

GO
ALTER TABLE [dbo].[SalesmenChanges]
WITH CHECK ADD
CONSTRAINT [FK_SalesmenChanges_Guests] FOREIGN KEY([schgu])
REFERENCES [dbo].[Guests] ([guID])