/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de inicializacion
** 
** [wtorres]	20/Feb/2016 Created
**
*/
use OrigosVCPalace

-- I. CATALOGOS
--		1. Roles
--		2. Gifts
--		3. BookingDeposits

-- =============================================
--					CATALOGOS
-- =============================================

-- =============================================
-- Tabla:		Roles
-- [erosado]	07/10/2016 Se agregaron dos nuevos roles FTM,FTB
-- =============================================
INSERT INTO dbo.Roles(roID, roN, roA)
SELECT 'FTM','Front To Middle',1
UNION SELECT 'FTB','Front To Back',1
GO

-- =============================================
-- Tabla:		Gifts
-- [emoguel]	10/10/2016 Se Asigno el Amount y Monetary para todos los gifts que contengas el signo de $
-- =============================================
UPDATE Gifts SET giMonetary=1,giAmount=SUBSTRING(giN,2,CHARINDEX(' ',giN,1)-2) WHERE giID IN (SELECT giID from Gifts WHERE giN LIKE '$%')

-- =============================================
-- Tabla:		BookingDeposits
-- [wtorres]	12/Oct/2016 Modifique el campo de numero de tarjeta de credito para que sea varchar, por eso haya que rellenar de ceros al
--							principio los numeros de tarjeta que tengan de 1 a 3 caracteres
-- =============================================
UPDATE BookingDeposits SET bdCardNum = NULL WHERE bdCardNum = '0'
UPDATE BookingDeposits SET bdCardNum = dbo.Pad(bdCardNum, '0', 4, 0) WHERE Len(bdCardNum) BETWEEN 1 AND 3
GO