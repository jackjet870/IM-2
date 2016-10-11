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