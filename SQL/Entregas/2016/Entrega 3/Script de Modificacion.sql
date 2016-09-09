/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de modificacion de tablas
** 
** [wtorres]	05/Sep/2016 Created
**
*/
use OrigosVCPalace

-- I. MODIFICACIONES DE TABLAS
--		1. Huespedes

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:		Huespedes
-- [lchairez]	02/Ago/2016 Agregue el campo guNotifiedEmailShowNotInvited
-- =============================================
ALTER TABLE Guests 
	ADD guNotifiedEmailShowNotInvited BIT DEFAULT 0
GO

-- Descripciones
exec sp_addextendedproperty N'MS_Description', N'Indica si se envió notificación de presentación sin invitacion', N'user', N'dbo', N'table', N'Guests', N'column', N'guNotifiedEmailShowNotInvited'
GO