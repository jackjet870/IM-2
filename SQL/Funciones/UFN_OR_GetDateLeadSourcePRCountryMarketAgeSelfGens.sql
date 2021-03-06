if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeSelfGens]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeSelfGens]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Obtener self gens de fecha, Lead Source, PR, país, mercado y edad
-- Descripción:		Devuelve el número de self gens por fecha, Lead Source, PR, país, mercado y edad
-- Histórico:		[wtorres] 25/Jun/2010 Creado
-- =============================================
create function [dbo].[UFN_OR_GetDateLeadSourcePRCountryMarketAgeSelfGens](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@ConsiderQuinellas bit = 0		-- Indica si se debe considerar quinielas
)
returns @Table table (
	[Date] datetime,
	LeadSource varchar(10),
	PR varchar(10),
	Country varchar(25),
	Market varchar(10),
	Age tinyint,
	SelfGens money
)
as
begin

insert @Table
select
	guShowD,
	guls,
	guPRInvit1,
	guco,
	gumk,
	guAge1,
	Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end)
from Guests
where
	-- Fecha de show
	guShowD between @DateFrom and @DateTo
	-- Self Gen
	and guSelfGen = 1
group by guShowD, guls, guPRInvit1, guco, gumk, guAge1

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

