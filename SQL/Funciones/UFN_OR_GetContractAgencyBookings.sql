if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetContractAgencyBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetContractAgencyBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Creado
** [aalcocer]	10/Jun/2016 Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencyBookings](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@Direct int = -1,					-- Filtro de directas:
										--		-1. Sin filtro
										--		 0. No directas
										--		 1. Directas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Books int
)
as
begin

insert @Table
select
	guO1,
	guag,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	((@BasedOnArrival = 0 and guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and guInvit = 1))
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- Directas
	and (@Direct = -1 or guDirect = @Direct)
	-- No bookings cancelados
	and guBookCanc = 0
group by guO1, guag

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

