/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de modificacion de tablas
** 
** [wtorres]	20/Feb/2016 Created
**
*/
use Hotel

-- I. MODIFICACIONES DE TABLAS
--		1. analista_h.CLRoles

-- =============================================
--			MODIFICACIONES DE TABLAS
-- =============================================

-- Tabla:			SalesLog
-- [erosado]		13/10/2016 Se agregaron las columnas Recnum, NumSalesmen
-- =============================================
ALTER TABLE analista_h.CLRoles ADD
Recnum INT IDENTITY(1,1) NOT NULL,
NumSalesmen INT NULL
GO
