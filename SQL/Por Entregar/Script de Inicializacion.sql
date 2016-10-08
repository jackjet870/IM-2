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
--		1.Roles

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