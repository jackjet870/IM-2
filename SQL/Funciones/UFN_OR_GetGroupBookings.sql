if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGroupBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGroupBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de bookings por grupo
** 
** [wtorres]	21/Jul/2010 Creado
** [wtorres]	17/Nov/2010 Agregué el parámetro @BasedOnArrival y reemplacé el parámetro @ConsiderDirects por @Direct
** [wtorres]	26/Nov/2010 Ahora no se cuentan los bookings cancelados
**
*/
create function [dbo].[UFN_OR_GetGroupBookings](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@Direct int = -1,			-- Filtro de directas:
								--		-1. Sin filtro
								--		 0. No directas
								--		 1. Directas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Group] int,
	Books int
)
as
begin

insert @Table
select
	I.gjgx,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Lead Source
	and G.guls in (select item from split(@LeadSources, ','))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by I.gjgx

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

