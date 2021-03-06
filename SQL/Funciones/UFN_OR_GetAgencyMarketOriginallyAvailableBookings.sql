if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por agencia, mercado y originalmente disponible
** 
** [wtorres]	23/Oct/2009 Created
** [wtorres]	19/Abr/2010 Modified. Agregue el parametro @ConsiderDirects
** [wtorres]	15/Oct/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [wtorres]	26/Nov/2010 Modified. Ahora no se cuentan los bookings cancelados
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableBookings](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@ConsiderNights bit = 0,	-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,		-- Numero de noches desde
	@NightsTo int = 0,			-- Numero de noches hasta
	@OnlyQuinellas bit = 0,		-- Indica si se desean solo las quinielas
	@Direct int = -1,			-- Filtro de directas:
								--		-1. Sin filtro
								--		 0. No directas
								--		 1. Directas
	@External int = 0,			-- Filtro de invitaciones externas
								--		0. Sin filtro
								--		1. Excluir invitaciones externas
								--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Agency varchar(35),
	Market varchar(10),
	OriginallyAvailable varchar(30),
	Books int
)
as
begin

insert @Table
select
	G.guag,
	G.gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (G.guOriginAvail | G.guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	left join LeadSources L on L.lsID = G.guls
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Numero de noches
	and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
	-- Quinielas
	and (@OnlyQuinellas = 0 or G.guQuinella = 1)
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- No bookings cancelados
	and G.guBookCanc = 0
	-- Invitaciones externas
	and (@External = 0
		or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
		or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
group by G.guag, G.gumk, (G.guOriginAvail | G.guInvit)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

