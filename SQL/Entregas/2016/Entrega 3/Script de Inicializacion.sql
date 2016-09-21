/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de inicializacion
** 
** [wtorres]	19/Sep/2016 Created
**
*/
use OrigosVCPalace

-- I. CATALOGOS
--		1. Permisos
--		2. Permisos de personal
--		3. Salas de ventas

-- =============================================
--					CATALOGOS
-- =============================================
-- Tabla:		Permisos
-- [bcanche]	10/Ene/2016 Se agrega valor para la opcion de menu de Folios CXC
-- [wtorres]	07/Sep/2016 Agregue el permiso de exportar reportes a Excel
-- =============================================
INSERT INTO Permissions (pmID, pmN, pmA, pmToolTipText)
SELECT 'FOLIOSCXC', 'Folios CXC', 1, 'Permite administrar el catálogo de Folios de CxC'
UNION ALL SELECT 'RPTEXCEL', 'Report Excel', 1, 'Permite exportar los reportes a Excel'
GO

-- =============================================
-- Tabla:		Permisos de personal
-- [lchairez]	12/May/2016 Se les otorga permiso al módulo de Folios CxC
-- [wtorres]	07/Sep/2016 Agregue el permiso de exportar reportes a Excel al personal de Contraloria,
--							Soporte Tecnico y Programacion
-- =============================================

-- [lchairez]	12/May/2016 Se les otorga permiso al módulo de Folios CxC
INSERT INTO PersonnelPermissions (pppe, pppm, pppl)
SELECT peID,'FOLIOSCXC', 2
FROM Personnel
WHERE peID IN ('4102', 'FOCTAVIANO', 'RAFHERNAN')
GO

-- [wtorres]	07/Sep/2016 Agregue el permiso de exportar reportes a Excel al personal de Contraloria,
--							Soporte Tecnico y Programacion
INSERT INTO PersonnelPermissions (pppe, pppm, pppl)
SELECT peID, 'RPTEXCEL', 1
FROM Personnel
WHERE
	-- CONTR	Contraloria
	-- SEC		Secretarias De Vents
	-- SOPORTE	Soporte Tecnico
	-- PROG		Programacion
	pede IN ('CONTR', 'SOPORTE', 'PROG', 'SEC')
	-- Activos
	AND peA = 1
	-- 0	No Registrado
	-- PVP	Usuarios Pvp
	-- RIFA	Rifa
	-- TEST	Usuario De Prueba
	AND peID NOT IN ('0', 'PVP', 'RIFA', 'TEST')
GO

-- =============================================
-- Tabla:		Salas de ventas
-- [lmartinez]	13/Jul/2016 Configura las zonas de las salas de ventas
-- =============================================
UPDATE SalesRooms
SET srzn = CASE
      WHEN srID in ('MPS','MP','MPG') THEN 'ZM'
      WHEN srID in ('PL','CZ') THEN 'ZA'
      WHEN srID in ('LBC','SP','BP','IM') THEN 'ZH'
      WHEN srID in ('ZCJG') THEN 'ZC'
      ELSE NULL END
GO