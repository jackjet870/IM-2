if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLeadSourceMarketMonthBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLeadSourceMarketMonthBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por Lead Source, mercado y mes
** 
** [wtorres]	04/Feb/2010 Created
** [wtorres]	14/May/2010 Modified. Agregue el parametro @ConsiderDirects
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [wtorres]	26/Nov/2010 Modified. Ahora no se cuentan los bookings cancelados
** [caduran]    17/Dic/2014 Modified. Se agregaron parametros @LeadSources y @Program; se agrega columna Program
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetLeadSourceMarketMonthBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@Direct int = -1,					-- Filtro de directas:
										--		-1. Sin filtro
										--		 0. No directas
										--		 1. Directas
	@External int = 0,					-- Filtro de invitaciones externas
										--		0. Sin filtro
										--		1. Excluir invitaciones externas
										--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	LeadSource varchar(10),
	Market varchar(10),
	[Year] int,
	[Month] int,
	Program varchar(10),
	Books int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		G.guls,
		G.gumk,
		Year(G.guBookD),
		Month(G.guBookD),
		L.lspg,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
	from Guests G
		left join LeadSources L on L.lsID = G.guls
	where
		-- Fecha de booking
		G.guBookD between @DateFrom and @DateTo
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- Programa
		and (@Program = 'ALL' or L.lspg = @Program)
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- Directas
		and (@Direct = -1 or G.guDirect = @Direct)
		-- No bookings cancelados
		and G.guBookCanc = 0
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by G.guls, G.gumk, Year(G.guBookD), Month(G.guBookD), L.lspg

-- si se debe basar en la fecha de llegada
else

	insert @Table
	select
		G.guls,
		G.gumk,
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		L.lspg,
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
		-- Programa
		and (@Program = 'ALL' or L.lspg = @Program)
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- Directas
		and (@Direct = -1 or G.guDirect = @Direct)
		-- No bookings cancelados
		and G.guBookCanc = 0
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by G.guls, G.gumk, Year(G.guCheckInD), Month(G.guCheckInD), L.lspg

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

