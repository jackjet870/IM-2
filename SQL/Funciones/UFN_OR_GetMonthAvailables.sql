if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMonthAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMonthAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por mes
** 
** [wtorres]	07/Oct/2009 Created
** [wtorres]	10/May/2010 Modified. Agregue los parametros @ConsiderQuinellas y @ConsiderOriginallyAvailable
**							Ahora acepta varios Lead Sources
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetMonthAvailables](
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
	@External int = 0						-- Filtro de invitaciones externas
											--		0. Sin filtro
											--		1. Excluir invitaciones externas
											--		2. Solo invitaciones externas
)
returns @Table table (
	[Year] int,
	[Month] int,
	Availables int
)
as
begin

insert @Table
select
	Year(G.guCheckInD),
	Month(G.guCheckInD),
	Sum(case when @ConsiderQuinellas = 0 or G.guShow = 0 then 1 else G.guShowsQty end)
from Guests G
	left join LeadSources L on L.lsID = G.guls
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and G.guCheckIn = 1
	-- Disponible
	and G.guAvail = 1
	-- No Rebook
	and G.guRef is null
	-- Contactado
	and G.guInfo = 1
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
	-- Invitaciones externas
	and (@External = 0
		or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
		or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
group by Year(G.guCheckInD), Month(G.guCheckInD)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

