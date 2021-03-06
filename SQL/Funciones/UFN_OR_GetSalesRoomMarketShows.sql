if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomMarketShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomMarketShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de shows por sala de ventas y mercado
** 
** [wtorres]	11/Jun/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetSalesRoomMarketShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	SalesRoom varchar(10),
	Market varchar(10),
	Shows int
)
as
begin

insert @Table
select
	gusr,
	gumk,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de show
	((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by gusr, gumk

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

