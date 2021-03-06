USE [Hotel]
GO
/****** Object:  StoredProcedure [analista_h].[USP_CL_GetRoles]    Script Date: 10/17/2016 16:48:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
** 
** Obtiene los roles de vendedores
**
** [wtorres]	13/08/2015 Created
** [erosado]	17/10/2016 Modified. Ahora trae nuevas columnas Order, Recnum, Numsalesmen
**
*/
ALTER procedure [analista_h].[USP_CL_GetRoles]
as
SET NOCOUNT ON -- Deshabilitamos el conteo de registros actualizados

select [Id], [Description], [Status], [Order], [Recnum],[NumSalesmen]
from analista_h.CLRoles
order by [Description]
