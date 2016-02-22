if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetLocations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetLocations]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de locaciones
** 
** [wtorres]	12/May/2014 Created
** [wtorres]	11/Ago/2014 Modified. Agregue el parametro @Regions
**
*/
create procedure [dbo].[USP_OR_GetLocations]
	@Programs varchar(8000) = 'ALL',	-- Clave de programas
	@Status tinyint = 1,				-- Filtro de estatus
										--		0. Sin filtro
										--		1. Activas
										--		2. Inactivas
	@Regions varchar(8000) = 'ALL'		-- Clave de regiones
	
as
set nocount on

select L.loID, L.loN
from Locations L
	left join LeadSources LS on LS.lsID = L.lols
	left join Areas A on A.arID = LS.lsar
where
	-- Programas
	(@Programs = 'ALL' or LS.lspg in (select item from split(@Programs, ',')))
	-- Estatus
	and (@Status = 0 or (@Status = 1 and L.loA = 1) or (@Status = 2 and L.loA = 0))
	-- Regiones
	and (@Regions = 'ALL' or A.arrg in (select item from split(@Regions, ',')))
order by L.loN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

