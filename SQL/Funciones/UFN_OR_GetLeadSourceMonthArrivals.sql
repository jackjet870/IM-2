if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLeadSourceMonthArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLeadSourceMonthArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por mes y lead source
** 
** [caduran]	23/Sep/2013 Created. Se agrega agrupacion por Lead Source, se baso en la funcion 'UFN_OR_GetMonthArrivals'
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetLeadSourceMonthArrivals](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@Market varchar(10) = 'ALL',			-- Clave del mercado
	@ConsiderNights bit = 0,				-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,					-- Numero de noches desde
	@NightsTo int = 0,						-- Numero de noches hasta
	@Agency varchar(35) = 'ALL',			-- Clave de agencia
	@ConsiderOriginallyAvailable bit = 0,	-- Indica si se debe considerar originalmente disponible
	@Contracts varchar(8000) = 'ALL',		-- Claves de contratos
	@External int = 0						-- Filtro de invitaciones externas
											--		0. Sin filtro
											--		1. Excluir invitaciones externas
											--		2. Solo invitaciones externas
)
returns @Table table (
	[Year] int,
	[Month] int,
	LeadSource varchar(10),
	Arrivals int
)
as
begin

insert @Table
select
	Year(G.guCheckInD),
	Month(G.guCheckInD),
	G.guls,
	Count(*)
from Guests G
	left join LeadSources L on L.lsID = G.guls
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and G.guCheckIn = 1
	-- No Rebook
	and G.guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
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
	-- Contrato
	and (@Contracts = 'ALL' or (CharIndex(',', @Contracts) > 0 and G.guO1 in (select item from Split(@Contracts, ',')))
		or (CharIndex(',', @Contracts) = 0 and G.guO1 like @Contracts))
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

