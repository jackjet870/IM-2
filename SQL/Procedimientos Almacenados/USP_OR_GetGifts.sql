if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGifts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGifts]
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de regalos
** 
** [wtorres]	12/May/2014 Creado
** [edgrodriguez] 07/Mar/2016 Modificado --Se agregó la columna de categoria(gigc)
**
*/
create procedure [dbo].[USP_OR_GetGifts]
	@Locations varchar(8000) = 'ALL',	-- Claves de Locaciones
	@Status tinyint = 1					-- Filtro de estatus
										--		0. Sin filtro
										--		1. Activos
										--		2. Inactivos
as
set nocount on

select distinct G.giID, G.giN, G.gigc
from Gifts G
	left join GiftsByLoc L on L.glgi = G.giID
where
	-- Estatus
	(@Status = 0 or (@Status = 1 and G.giA = 1) or (@Status = 2 and G.giA = 0))
	-- Locaciones
	and (@Locations = 'ALL' or L.gllo in (select item from split(@Locations, ',')))
order by G.giN


