if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMonthAgencyLeadSourceShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Creado
** [wtorres]	14/May/2010 Agregue el parametro @ConsiderDirectsAntesInOut
** [wtorres]	24/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	20/Dic/2013 Agregue el parametro @ConsiderQuinellas
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@Agencies varchar(8000) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
	LeadSource varchar(10),
	Shows int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(G.guShowD),
		Month(G.guShowD),
		G.guag,
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de show
		G.guShowD between @DateFrom and @DateTo
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
	group by Year(G.guShowD), Month(G.guShowD), G.guag, G.guls

-- si se debe basar en la fecha de llegada
else

	insert @Table
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guag,
		G.guls,
		Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
	from Guests G
		left join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de llegada
		G.guCheckInD between @DateFrom and @DateTo
		-- Con show
		and G.guShow = 1
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, G.guls

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

