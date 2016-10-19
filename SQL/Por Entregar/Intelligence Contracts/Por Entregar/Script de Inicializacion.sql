/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de inicializacion
** 
** [wtorres]	20/Feb/2016 Created
**
*/
USE Hotel

-- I. CATALOGOS
--		1. CLRoles
--		2. CLCLAOPC

-- =============================================
--					CATALOGOS
-- =============================================

-- =============================================

-- Tabla:		analista_h.CLRoles
-- [erosado]	15/10/2016 Se agregaron dos nuevos roles FTM,FTB
-- =============================================
INSERT INTO analista_h.CLRoles ([Id], [Description], [Order], [NumSalesmen])
SELECT 'FTM','Front To Middle',3,2
UNION SELECT'FTB','Front To Back',4,2
GO

-- Se actualizaron el campo Order En Closer, Exit, VLO
UPDATE Hotel.analista_h.CLRoles
SET	[Order] =
	(CASE Id
		WHEN 'CLOSER' THEN 5
		WHEN 'EXIT' THEN 6
		WHEN 'VLO' THEN 7
	END)
WHERE Id IN ('CLOSER', 'EXIT', 'VLO')
GO

-- Se actualizaron el campo NumSales en OPC, Liner, Closer, Exit, Vlo
UPDATE Hotel.analista_h.CLRoles
SET	 [NumSalesmen] =
	(CASE Id
		WHEN 'OPC' THEN 3
		WHEN 'LINER' THEN 3
		WHEN 'CLOSER' THEN 4
		WHEN 'EXIT' THEN 3
		WHEN 'VLO' THEN 1
	END)
WHERE Id IN ('OPC', 'LINER', 'CLOSER', 'EXIT', 'VLO')
GO

-- Tabla:		analista_h.CLCLAOPC
-- [erosado]	17/10/2016 Se actualizaron los valores de Role para FTM y FTB
-- =============================================
UPDATE analista_h.CLCLAOPC
set [Role] = 
(CASE CLAOPC_ID
	WHEN 'FTM' THEN 'FTM'
	WHEN 'FTB' THEN 'FTB'
	END)
WHERE CLAOPC_ID IN ('FTM','FTB')
GO