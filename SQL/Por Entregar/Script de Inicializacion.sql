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
--		1. Permisos
--		2. Permisos de personal

-- =============================================
--					CATALOGOS
-- =============================================
-- Tabla:		Permisos
-- [wtorres]	07/Sep/2016 Agregue el permiso de exportar reportes a Excel
-- =============================================
INSERT INTO Permissions (pmID, pmN, pmA, pmToolTipText)
SELECT 'RPTEXCEL', 'Report Excel', 1, 'Permite exportar los reportes a Excel'
GO

-- =============================================
-- Tabla:		Permisos de personal
-- [wtorres]	07/Sep/2016 Agregue el permiso de exportar reportes a Excel al personal de Contraloria,
--							Soporte Tecnico y Programacion
-- =============================================
INSERT INTO PersonnelPermissions (pppe, pppm, pppl)
SELECT peID, 'RPTEXCEL', 1
FROM Personnel
-- CONTR	Contraloria
-- SOPORTE	Soporte Tecnico
-- PROG		Programacion
WHERE pede IN ('CONTR', 'SOPORTE', 'PROG') AND peA = 1
GO