if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetShowProgramProgramShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetShowProgramProgramShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por programa de show y programa
** 
** [wtorres]	21/Sep/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetShowProgramProgramShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Program varchar(10),
	ShowProgram varchar(10),
	Shows int
)
as
begin

insert @Table
select
	D.Program,
	D.ShowProgram,
	Sum(D.Shows)
from (
	select
		L.lspg as Program,
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, G.guInOut, SR.srAppointment) as ShowProgram,
		case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end as Shows
	from Guests G
		inner join SalesRooms SR on G.gusr = SR.srID
		inner join LeadSources L on G.guls = L.lsID
	where
		-- Fecha de show
		((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
		-- Fecha de llegada
		or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
		-- Con show
		and G.guShow = 1))
		-- Salas de ventas
		and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
		-- No Quiniela Split
		and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
		-- No Directas no Antes In & Out
		and (@ConsiderDirectsAntesInOut = 0 or not (G.guDirect = 1 and G.guAntesIO = 0))
) as D
group by D.Program, D.ShowProgram

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

