if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetContractAgencyAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetContractAgencyAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Creado
**
*/
create function [dbo].[UFN_OR_GetContractAgencyAvailables](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(8000) = 'ALL',	-- Claves de agencias
	@ConsiderQuinellas bit = 0			-- Indica si se debe considerar quinielas
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	Availables int
)
as
begin

insert @Table
select
	guO1,
	guag,
	Sum(case when @ConsiderQuinellas = 0 or guShow = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de llegada
	guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and guCheckIn = 1
	-- Disponible
	and guAvail = 1
	-- No Rebook
	and guRef is null
	-- Contactado
	and guInfo = 1
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or guag in (select item from split(@Agencies, ',')))
group by guO1, guag

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

