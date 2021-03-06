if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomMarketBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomMarketBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de bookings por sala de ventas y mercado
** 
** [wtorres]	11/Jun/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetSalesRoomMarketBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@Direct int = -1,					-- Filtro de directas:
										--		-1. Sin filtro
										--		 0. No directas
										--		 1. Directas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	SalesRoom varchar(10),
	Market varchar(10),
	Books int
)
as
begin

insert @Table
select
	gusr,
	gumk,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ',')))
	-- No Antes In & Out
	and guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or guDirect = @Direct)
	-- No bookings cancelados
	and guBookCanc = 0
group by gusr, gumk

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

