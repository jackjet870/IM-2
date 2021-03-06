if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetUnavailableMotiveArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetUnavailableMotiveArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de llegadas por motivo de indisponibilidad
** 
** [wtorres]	30/Jul/2010 Creado
** [wtorres]	03/Nov/2011 Agregue el campo de indisponibles por usuario
**
*/
create function [dbo].[UFN_OR_GetUnavailableMotiveArrivals](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Claves de Lead Sources
)
returns @Table table (
	UnavailableMotive int,
	Arrivals int,
	ByUser int
)
as
begin

insert @Table
select
	guum,
	Count(*),
	Sum(case when guAvail <> guAvailBySystem then 1 else 0 end)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- No Rebook
	and guRef is null
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- No disponible
	and guAvail = 0 and guum > 0
group by guum

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

