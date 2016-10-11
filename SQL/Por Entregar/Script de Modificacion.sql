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
-- Tabla:		SalesmenChanges
-- [edgrodriguez]	11/Oct/2016 Se elimina la llave principal.
--								Se modifica la columna schsa para que acepte Nulos.
--								Se agregan los campos schID(PK),schgu(FK con Guest),schmt(FK con GuestMovementsType)
--
--							
-- =============================================
ALTER TABLE [dbo].[SalesmenChanges]
--Eliminamos la llave primaria
DROP CONSTRAINT PK_SalesmenChanges

GO

ALTER TABLE [dbo].[SalesmenChanges]
--Modificamos la columna ID de venta.
ALTER COLUMN [schsa] int NULL

GO

ALTER TABLE [dbo].[SalesmenChanges]
--Agregamos las nuevas columnas
ADD [schID] INT IDENTITY,
	[schgu]int NULL,
	[schmt] varchar(10) NOT NULL DEFAULT 'SL'

GO

SET ANSI_PADDING OFF

GO
--Agregamos las descripciones
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave Primaria de la tabla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GuestID a quién se le hizo el cambio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schgu'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Clave del tipo de movimiento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SalesmenChanges', @level2type=N'COLUMN',@level2name=N'schmt'
GO
--Agregamos las llaves foraneas de las nuevas columnas
ALTER TABLE [dbo].[SalesmenChanges] 
ADD CONSTRAINT PK_SalesmenChanges
PRIMARY KEY(schID)

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