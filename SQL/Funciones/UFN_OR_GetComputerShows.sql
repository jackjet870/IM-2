if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetComputerShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetComputerShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de shows por computadora
** 
** [wtorres]	07/Jun/2010 Creado
** [wtorres]	18/Oct/2010 Agregué el parámetro @BasedOnArrival
**
*/
create function [dbo].[UFN_OR_GetComputerShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Computer varchar(15),
	Shows int
)
as
begin

insert @Table
select
	M.gmcp,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	left join GuestsMovements M on G.guID = M.gmgu and M.gmgn = 'BK'
where
	-- Fecha de show
	((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
group by M.gmcp

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

