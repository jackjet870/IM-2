if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceArrivals](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL'	-- Claves de agencias
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Arrivals int
)
as
begin

insert @Table
select
	Year(G.guCheckInD),
	Month(G.guCheckInD),
	G.guag,
	G.guls,
	Count(*)
from Guests G
	left join LeadSources L on G.guls = L.lsID
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and G.guCheckIn = 1
	-- No Rebook
	and G.guRef is null
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Agencias
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
	-- Programa
	and L.lspg = 'IH'
	-- Originalmente disponibles
	and (G.guOriginAvail = 1 or G.guInvit = 1)
group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, G.guls

return
end