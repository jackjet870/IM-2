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

-- II. ELIMINACION DE FUNCIONES
--		1. UFN_OR_GetPRSalesRoomDirects

-- =============================================
--  ELIMINACION DE PROCEDIMIENTOS ALMACENADOS
-- =============================================


-- =============================================
--			ELIMINACION DE FUNCIONES
-- =============================================
-- UFN_OR_GetPRSalesRoomDirects
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRSalesRoomDirects]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRSalesRoomDirects]
GO