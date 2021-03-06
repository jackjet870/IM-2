if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Modified. Ahora no se cuentan los rebooks
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceContacts](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Contacts int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(G.guInfoD),
		Month(G.guInfoD),
		G.guag,
		G.guls,
		Count(*)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de contacto
		G.guInfoD between @DateFrom and @DateTo
		-- No Rebook
		and guRef is null
		-- No Antes In & Out
		and G.guAntesIO = 0
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(G.guInfoD), Month(G.guInfoD), G.guag, G.guls

-- si se debe basar en la fecha de llegada
else

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
		-- Contactado
		and G.guInfo = 1
		-- No Rebook
		and guRef is null
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