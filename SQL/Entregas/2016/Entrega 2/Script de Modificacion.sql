/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de modificacion de tablas
** 
** [wtorres]	19/Ago/2016 Created
**
*/
use OrigosVCPalace

-- I. MODIFICACIONES DE TABLAS
--		1. Lead Sources
--		2. Depositos

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:		Lead Sources
-- [wtorres]	11/Ago/2016 Agregue la llave foranea con las salas de ventas
-- =============================================

-- Llaves foraneas - Lead Sources - Salas de ventas
ALTER TABLE LeadSources
	ADD CONSTRAINT FK_LeadSources_SalesRooms FOREIGN KEY(lssr)
	REFERENCES SalesRooms(srID)
GO

-- =============================================
-- Tabla:		Depositos
-- [wtorres]	19/Ago/2016 Modifique el campo de numero de tarjeta de credito para que sea numerico
-- =============================================
ALTER TABLE BookingDeposits
	ALTER COLUMN bdCardNum int NULL
GO