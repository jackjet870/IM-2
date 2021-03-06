if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de bookings por fecha, Lead Source, PR, país, mercado y edad
** 
** [wtorres]	25/Jun/2010 Creado
** [wtorres]	26/Nov/2010 Ahora no se cuentan los bookings cancelados
**
*/
create function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeBookings](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@ConsiderDirects bit = 0	-- Indica si se debe considerar directas
)
returns @Table table (
	[Date] datetime,
	LeadSource varchar(10),
	PR varchar(10),
	Country varchar(25),
	Market varchar(10),
	Age tinyint,
	Books money
)
as
begin

insert @Table
select
	guBookD,
	guls,
	guPRInvit1,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guRoomsQty end)
from Guests
where
	-- Fecha de booking
	guBookD between @DateFrom and @DateTo
	-- No Antes In & Out
	and guAntesIO = 0
	-- No quinielas split
	and (@ConsiderQuinellas = 0 or guQuinellaSplit = 0)
	-- Directas
	and (@ConsiderDirects = 0 or guDirect = 0)
	-- No bookings cancelados
	and guBookCanc = 0
group by guBookD, guls, guPRInvit1, guco, gumk, guAge1

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

