if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPRsAssigned]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPRsAssigned]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los PRs que tienen asignado al menos a un huesped
** 
** [wtorres]	10/Nov/2011 Ahora se pasa la lista de mercados como un solo parametro
** [wtorres]	24/Ene/2014 Ahora los roles se obtienen de la tabla de roles de personal
**
*/
create procedure [dbo].[USP_OR_GetPRsAssigned]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSource varchar(10),	-- Clave del Lead Source
	@Markets varchar(8000),		-- Claves de mercados
	@PRGuests bit,				-- Indica si se desean los PRs de huespedes
	@PRMembers bit				-- Indica si se desean los PRs de socios
as
set nocount on

select
	guPRAssign,
	Count(*) as TotalAssigned
into #tblGuests
from Guests
where
	-- Lead Source
	guls = @LeadSource
	-- Fecha de llegada
	and guCheckInD between @DateFrom and @DateTo
	-- Mercados
	and gumk in (select item from Split(@Markets, ','))
group by guPRAssign

select distinct
	P.peID,
	P.peN,
	Cast(0 as int) as Assigned
into #tblPRs
from Personnel P
	inner join PersLSSR PL on PL.plpe = P.peID
	left join PersonnelRoles PR on PR.prpe = P.peID
where
	-- Tipo de lugar
	PL.plLSSR = 'LS'
	-- Lead Source
	and PL.plLSSRID = @LeadSource
	-- Rol de PR
	and PR.prro = 'PR'
	-- Rol de PR de socios
	and (
		-- Solo los PRs de huespedes, es decir, que no sean PRs de socios
		(@PRGuests = 1 and @PRMembers = 0 and not exists (select prpe from PersonnelRoles where prpe = P.peID and prro = 'PRMEMBER'))
		-- Solo los PRs de socios
		or (@PRGuests = 0 and @PRMembers = 1 and exists (select prpe from PersonnelRoles where prpe = P.peID and prro = 'PRMEMBER'))
		-- Ambos tipos de PRs
		or (@PRGuests = 1 and @PRMembers = 1))
	-- Activo
	and P.peA = 1

update #tblPRs
set Assigned = TotalAssigned
from #tblPRs
	inner join #tblGuests on peID = guPRAssign

select * from #tblPRs order by peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

