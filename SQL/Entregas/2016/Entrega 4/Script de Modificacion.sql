/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de modificacion de tablas
** 
** [wtorres]	13/Oct/2016 Created
**
*/
use OrigosVCPalace

-- I. MODIFICACIONES DE TABLAS
--		1. BookingDeposits

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================
-- Tabla:		BookingDeposits
-- [wtorres]	12/Oct/2016 Modifique el campo de numero de tarjeta de credito para que sea varchar
-- =============================================
ALTER TABLE BookingDeposits
	ALTER COLUMN bdCardNum varchar(4) NULL
GO