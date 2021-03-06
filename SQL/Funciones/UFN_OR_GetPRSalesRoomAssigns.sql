if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRSalesRoomAssigns]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRSalesRoomAssigns]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de asignaciones por PR y Sala de Ventas
** 
** [axperez]	03/Dic/2013 Creado. copeado de UFN_OR_GetPRAssigns
**
*/
create function [dbo].[UFN_OR_GetPRSalesRoomAssigns](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Clave de los Lead Sources
)
returns @Table table (
	PR varchar(10),
	SalesRoom varchar(10),
	Assigns int
)
as
begin

insert @Table
select
	guPRAssign,
	gusr,
	Count(*)
from Guests
where
	-- PR asignado
	guPRAssign is not null
	-- Fecha de llegada
	and guCheckInD between @DateFrom and @DateTo
	-- No rebook
	and guRef is null
	-- Con Check In
	and guCheckIn = 1
	-- No antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ',')))
group by guPRAssign, gusr

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

