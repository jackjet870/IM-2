if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMemberAvailables]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMemberAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de disponibles por membresia
** 
** [wtorres]	25/Feb/2015 Created
** [wtorres]	19/Mar/2015 Modified. Ahora se agrupa por tipo de huesped
**
*/
create function [dbo].[UFN_OR_GetMemberAvailables](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@Application varchar(15) = 'ALL',	-- Clave de membresia
	@Company decimal(2,0) = 0,			-- Clave de compania
	@Club int = 0,						-- Clave de club
	@OnlyWholesalers bit = 0			-- Indica si se desean solo mayoristas
)
returns @Table table (
	Club int,
	Company decimal(2,0),
	Application varchar(15),
	GuestType varchar(1),
	Availables int
)
as
begin

insert @Table
select
	dbo.UFN_OR_GetClub(A.agcl, G.gucl),
	G.guCompany,
	G.guMembershipNum,
	dbo.UFN_OR_GetGuestType(G.guO1, G.guGuestRef),
	Sum(case when @ConsiderQuinellas = 0 or G.guShow = 0 then 1 else G.guShowsQty end)
from Guests G
	left join Agencies A on A.agID = G.guag
	inner join LeadSources L on L.lsID = G.guls
	left join Wholesalers W on W.wscl = dbo.UFN_OR_GetClub(A.agcl, G.gucl) and W.wsCompany = G.guCompany and W.wsApplication = G.guMembershipNum
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Con Check In
	and G.guCheckIn = 1
	-- Disponible
	and G.guAvail = 1
	-- No Rebook
	and G.guRef is null
	-- Contactado
	and G.guInfo = 1
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- Con membresia
	and IsNull(G.guMembershipNum, '') <> ''
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Membresia
	and (@Application = 'ALL' or (G.guMembershipNum = @Application and G.guCompany = @Company and dbo.UFN_OR_GetClub(A.agcl, G.gucl) = @Club))
	-- Mayoristas
	and (@OnlyWholesalers = 0 or W.wsApplication is not null)
group by dbo.UFN_OR_GetClub(A.agcl, G.gucl), G.guCompany, G.guMembershipNum, dbo.UFN_OR_GetGuestType(G.guO1, G.guGuestRef)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

