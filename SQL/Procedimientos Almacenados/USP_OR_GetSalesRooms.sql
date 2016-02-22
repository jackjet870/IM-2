if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesRooms]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesRooms]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el catalogo de salas de ventas
** 
** [wtorres]	12/May/2014 Creado
**
*/
create procedure [dbo].[USP_OR_GetSalesRooms]
	@Status tinyint = 1	-- Filtro de estatus
						--		0. Sin filtro
						--		1. Activas
						--		2. Inactivas
as
set nocount on

select srID, srN
from SalesRooms
where
	-- Estatus
	(@Status = 0 or (@Status = 1 and srA = 1) or (@Status = 2 and srA = 0))
order by srN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

