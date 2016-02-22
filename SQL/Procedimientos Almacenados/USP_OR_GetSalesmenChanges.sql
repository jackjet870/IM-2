if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesmenChanges]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesmenChanges]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los cambios de vendedores de una venta
** 
** [wtorres]	26/Ago/2015 Created
**
*/
create procedure [dbo].[USP_OR_GetSalesmenChanges] 
	@Sale int	-- Clave de la venta
as
set nocount on

select
	-- Fecha y hora
	C.schDT,
	
	-- Autorizado por
	C.schAuthorizedBy,
	A.peN as AuthorizedByN,
	
	-- Hecho por
	C.schMadeBy,
	M.peN as MadeByN,
	
	-- Rol y posicion
	R.roN,
	C.schPosition,
	
	-- Antiguo vendedor
	C.schOldSalesman,
	O.peN as OldSalesmanN,
	
	-- Nuevo vendedor
	C.schNewSalesman,
	N.peN as NewSalesmanN
from SalesmenChanges C
	left join Roles R on R.roID = C.schro
	left join Personnel A on A.peID = C.schAuthorizedBy
	left join Personnel M on M.peID = C.schMadeBy
	left join Personnel O on O.peID = C.schOldSalesman
	left join Personnel N on N.peID = C.schNewSalesman
where C.schsa = @Sale
order by C.schDT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

