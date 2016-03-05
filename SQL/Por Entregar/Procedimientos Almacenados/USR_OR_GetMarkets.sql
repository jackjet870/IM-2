/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de Markets
** 
** [erosado]	24/Febrero/2016 Creado
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



