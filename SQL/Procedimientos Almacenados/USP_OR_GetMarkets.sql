if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetMarkets]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetMarkets]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de Markets
** 
** [erosado]	24/Febrero/2016 Created
**
*/
create procedure [dbo].[USP_OR_GetMarkets]
	@Status tinyint = 1	-- Filtro de estatus
						--		0. Sin filtro
						--		1. Activas
						--		2. Inactivas
as
set nocount on

select mkID, mkN
from Markets
where
	-- Estatus
	(@Status = 0 or (@Status = 1 and mkA = 1) or (@Status = 2 and mkA = 0))
order by mkN