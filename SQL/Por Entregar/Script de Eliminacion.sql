/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script de eliminacion de procedimientos almacenados y funciones
** 
** [wtorres]	20/Feb/2016 Created
**
*/
use OrigosVCPalace

-- I. ELIMINACION DE PROCEDIMIENTOS ALMACENADOS
--		1. USP_OR_InMaintenance

-- II. ELIMINACION DE FUNCIONES

-- =============================================
--  ELIMINACION DE PROCEDIMIENTOS ALMACENADOS
-- =============================================
-- USP_OR_InMaintenance
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_InMaintenance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_InMaintenance]
GO


-- =============================================
--			ELIMINACION DE FUNCIONES
-- =============================================
