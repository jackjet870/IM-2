if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLeadSourceMonthBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLeadSourceMonthBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por nacionalidad y lead source
** 
** [caduran]	23/Sep/2013 Created. Se agrego la agrupacion por Lead Source, se baso en el procedimiento 'UFN_OR_GetMonthBookings'
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetLeadSourceMonthBookings](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,				-- Indica si se debe considerar quinielas
	@Market varchar(10) = 'ALL',			-- Clave del mercado
	@ConsiderNights bit = 0,				-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,					-- Numero de noches desde
	@NightsTo int = 0,						-- Numero de noches hasta
	@Agency varchar(35) = 'ALL',			-- Clave de agencia
	@ConsiderOriginallyAvailable bit = 0,	-- Indica si se debe considerar originalmente disponible
	@Direct int = -1,						-- Filtro de directas:
											--		-1. Sin filtro
											--		 0. No directas
											--		 1. Directas
	@External int = 0,						-- Filtro de invitaciones externas
											--		0. Sin filtro
											--		1. Excluir invitaciones externas
											--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0					-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	LeadSource varchar(10),
	Books int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(G.guBookD),
		Month(G.guBookD),
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
	from Guests G
		left join LeadSources L on L.lsID = G.guls
	where
		-- Fecha de booking
		G.guBookD between @DateFrom and @DateTo
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- Mercado
		and (@Market = 'ALL' or G.gumk = @Market)
		-- Numero de noches
		and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
		-- Agencia
		and (@Agency = 'ALL' or G.guag like @Agency)
		-- Originalmente disponible
		and (@ConsiderOriginallyAvailable = 0 or (G.guOriginAvail = 1 or G.guInvit = 1))
		-- Directas
		and (@Direct = -1 or G.guDirect = @Direct)
		-- No bookings cancelados
		and G.guBookCanc = 0
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by Year(G.guBookD), Month(G.guBookD), G.guls

-- si se debe basar en la fecha de llegada
else

	insert @Table
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
	from Guests G
		left join LeadSources L on L.lsID = G.guls
	where
		-- Fecha de llegada
		G.guCheckInD between @DateFrom and @DateTo
		-- Invitado
		and G.guInvit = 1
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- Mercado
		and (@Market = 'ALL' or G.gumk = @Market)
		-- Numero de noches
		and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
		-- Agencia
		and (@Agency = 'ALL' or G.guag like @Agency)
		-- Originalmente disponible
		and (@ConsiderOriginallyAvailable = 0 or (G.guOriginAvail = 1 or G.guInvit = 1))
		-- Directas
		and (@Direct = -1 or G.guDirect = @Direct)
		-- No bookings cancelados
		and G.guBookCanc = 0
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guls

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

