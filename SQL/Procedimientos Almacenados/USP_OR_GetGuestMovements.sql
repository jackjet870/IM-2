if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestMovements]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestMovements]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los movimientos de un huesped
** 
** [wtorres]	17/Jun/2010 Creado
**
*/
create procedure [dbo].[USP_OR_GetGuestMovements] 
	@Guest int -- Clave del huesped
as
set nocount on;

select 
	M.gmpe,
	P.peN,
	M.gmDT,
	T.gnN,
	M.gmcp,
	C.cpN,
	M.gmIpAddress
from GuestsMovements M
	inner join Personnel P on M.gmpe = P.peID
	inner join GuestsMovementsTypes T on M.gmgn = T.gnID
	inner join Computers C on M.gmcp = C.cpID
where M.gmgu = @Guest
order by M.gmDT, M.gmgn desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

