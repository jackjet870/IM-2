/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de inicializacion
** 
** [wtorres]	13/Oct/2016 Created
**
*/
use OrigosVCPalace

-- I. CATALOGOS
--		1. BookingDeposits

-- =============================================
--					CATALOGOS
-- =============================================
-- Tabla:		BookingDeposits
-- [wtorres]	12/Oct/2016 Modifique el campo de numero de tarjeta de credito para que sea varchar, por eso haya que rellenar de ceros al
--							principio los numeros de tarjeta que tengan de 1 a 3 caracteres
-- =============================================
UPDATE BookingDeposits SET bdCardNum = NULL WHERE bdCardNum = '0'
UPDATE BookingDeposits SET bdCardNum = dbo.Pad(bdCardNum, '0', 4, 0) WHERE Len(bdCardNum) BETWEEN 1 AND 3
GO