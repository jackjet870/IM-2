if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetShowProgramBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetShowProgramBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por programa de show
** 
** [wtorres]	05/Ago/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetShowProgramBookings](
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
	ShowProgram varchar(10),
	Books int
)
as
begin

insert @Table
select
	D.ShowProgram,
	Sum(D.Books)
from (
	select
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, G.guInOut, SR.srAppointment) as ShowProgram,
		case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end as Books
	from Guests G
		inner join SalesRooms SR on G.gusr = SR.srID
	where
		-- Fecha de booking
		((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
		-- Invitado
		and G.guInvit = 1))
		-- Salas de ventas
		and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Directas
		and (@Direct = -1 or G.guDirect = @Direct)
		-- No bookings cancelados
		and G.guBookCanc = 0
) as D
group by D.ShowProgram

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

