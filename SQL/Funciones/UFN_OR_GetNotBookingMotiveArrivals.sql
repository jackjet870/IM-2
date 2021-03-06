if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetNotBookingMotiveArrivals]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetNotBookingMotiveArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Obtener llegadas de motivo de no booking
-- Descripción:		Devuelve el número de llegadas por motivo de no booking
-- Histórico:		[wtorres] 30/Jul/2010 Creado
-- =============================================
create function [dbo].[UFN_OR_GetNotBookingMotiveArrivals](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Claves de Lead Sources
)
returns @Table table (
	NotBookingMotive int,
	Arrivals int
)
as
begin

insert @Table
select
	gunb,
	Count(*)
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
	-- No booking
	and gunb > 0 and guInvit = 0
group by gunb

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

