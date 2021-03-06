if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableContacts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por agencia, mercado y originalmente disponible
** 
** [wtorres]	23/Oct/2009 Created
** [wtorres]	15/Oct/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Modified. Ahora no se cuentan los rebooks
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetAgencyMarketOriginallyAvailableContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderNights bit = 0,	-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,		-- Numero de noches desde
	@NightsTo int = 0,			-- Numero de noches hasta
	@OnlyQuinellas bit = 0,		-- Indica si se desean solo las quinielas
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
	Contacts int
)
as
begin

insert @Table
select
	G.guag,
	G.gumk,
	-- Si tiene invitacion, considerarlo en originalmente disponible
	case when (G.guOriginAvail | G.guInvit) = 1 then 'ORIGINALLY AVAILABLES' else 'ORIGINALLY UNAVAILABLES' end,
	Count(*)
from Guests G
	left join LeadSources L on L.lsID = G.guls
where
	-- Fecha de contacto
	((@BasedOnArrival = 0 and G.guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and G.guInfo = 1))
	-- No Rebook
	and G.guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- Numero de noches
	and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
	-- Quinielas
	and (@OnlyQuinellas = 0 or G.guQuinella = 1)
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

