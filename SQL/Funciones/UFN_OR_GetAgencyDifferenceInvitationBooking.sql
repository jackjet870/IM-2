if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencyDifferenceInvitationBooking]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencyDifferenceInvitationBooking]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de dias de diferencia entre la fecha de invitacion y la fecha de booking por agencia
** 
** [wtorres]	20/Oct/2011 Creado
** [wtorres]	13/Nov/2013 Ya no se toma en cuenta la fecha de invitacion en el WHERE
**
*/
create function [dbo].[UFN_OR_GetAgencyDifferenceInvitationBooking](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL'	-- Claves de Lead Sources
)
returns @Table table (
	Agency varchar(25),
	[Difference] int
)
as
begin

insert @Table
select
	G.guag,
	Sum(DateDiff(Day, G.guInvitD, G.guBookD))
from Guests G
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- No bookings cancelados
	and G.guBookCanc = 0
group by G.guag

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

