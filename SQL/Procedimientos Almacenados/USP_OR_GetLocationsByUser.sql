if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetLocationsByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetLocationsByUser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las locaciones a las que tiene permiso un usuario
** 
** [wtorres]	07/Jun/2011 Creado
** [wtorres]	21/Jun/2011 Agregue el parametro @Program y la posibilidad de obtener las locaciones asignadas a cualquier usuario
**
*/
create procedure [dbo].[USP_OR_GetLocationsByUser]
	@User varchar(10) = 'ALL',		-- Clave del usuario
	@Program varchar(10) = 'ALL'	-- Clave del programa
as
set nocount on

select L.loID, L.loN, L.lols, L.losr, P.plpe
from Locations L
	inner join LeadSources LS on L.lols = LS.lsID
	inner join PersLSSR P on P.plLSSRID = LS.lsID
where
	-- Usuario
	(@User = 'ALL' or P.plpe = @User)
	-- Lugar de tipo Lead Source
	and P.plLSSR = 'LS'
	-- Activo
	and L.loA = 1
	-- Programa
	and (@Program = 'ALL' or LS.lspg = @Program)
order by L.loN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

