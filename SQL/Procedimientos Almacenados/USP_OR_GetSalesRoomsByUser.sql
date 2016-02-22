if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesRoomsByUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesRoomsByUser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las salas de ventas a las que tiene permiso un usuario
** 
** [wtorres]	07/Jun/2011 Created
** [wtorres]	21/Jun/2011 Modified. Agregue la posibilidad de obtener las salas de ventas asignadas a cualquier usuario
** [wtorres]	12/Ago/2014 Modified. Agregue el parametro @Regions
**
*/
create procedure [dbo].[USP_OR_GetSalesRoomsByUser]
	@User varchar(10) = 'ALL',		-- Clave del usuario
	@Regions varchar(8000) = 'ALL'	-- Clave de regiones
as
set nocount on

select S.srID, S.srN, S.srwh, P.plpe
from SalesRooms S
	inner join PersLSSR P on P.plLSSRID = S.srID
	left join Areas A on A.arID = S.srar
where
	-- Usuario
	(@User = 'ALL' or P.plpe = @User)
	-- Lugar de tipo sala de ventas
	and P.plLSSR = 'SR'
	-- Activo
	and S.srA = 1
	-- Regiones
	and (@Regions = 'ALL' or A.arrg in (select item from split(@Regions, ',')))
order by S.srN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

