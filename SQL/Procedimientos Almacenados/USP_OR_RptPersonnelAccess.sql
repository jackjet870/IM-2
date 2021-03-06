if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptPersonnelAccess]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptPersonnelAccess]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de accesos de personal
** 
** [wtorres]	24/Ene/2014 Reemplace la clave del Lead Source por su descripcion.
**							Ahora los roles se obtienen de la tabla de roles de personal.
**							Elimine el rol Contracts Person y agregue el rol de PR Supervisor.
**
*/
create procedure [dbo].[USP_OR_RptPersonnelAccess]
	@LeadSources varchar(8000)	-- Claves de Lead Sources
as
set nocount on

select 
	L.lsN,
	P.peps,
	P.peID,
	P.peN,
	P.peCaptain,
	dbo.UFN_OR_HasRole(P.peID, 'PR') as PR,
	dbo.UFN_OR_HasRole(P.peID, 'PRMEMBERS') as PRMembers,
	dbo.UFN_OR_HasRole(P.peID, 'LINER') as Liner,
	dbo.UFN_OR_HasRole(P.peID, 'CLOSER') as Closer,
	dbo.UFN_OR_HasRole(P.peID, 'EXIT') as [Exit],
	dbo.UFN_OR_HasRole(P.peID, 'PODIUM') as Podium,
	dbo.UFN_OR_HasRole(P.peID, 'PRCAPT') as PRCaptain,
	dbo.UFN_OR_HasRole(P.peID, 'PRSUPER') as PRSupervisor,
	dbo.UFN_OR_HasRole(P.peID, 'LINERCAPT') as LinerCaptain,
	dbo.UFN_OR_HasRole(P.peID, 'CLOSERCAPT') as CloserCaptain,
	dbo.UFN_OR_HasRole(P.peID, 'HOSTEXIT') as EntryHost,
	dbo.UFN_OR_HasRole(P.peID, 'HOSTGIFTS') as GiftsHost,
	dbo.UFN_OR_HasRole(P.peID, 'HOSTEXIT') as ExitHost,
	dbo.UFN_OR_HasRole(P.peID, 'VLO') as VLO,
	dbo.UFN_OR_HasRole(P.peID, 'MANAGER') as Manager,
	dbo.UFN_OR_HasRole(P.peID, 'ADMIN') as Administrator
from Personnel P
	inner join PersLSSR PL on PL.plpe = P.peID
	left join LeadSources L on L.lsID = PL.plLSSRID
where
	-- Tipo de lugar 
	PL.plLSSR = 'LS'
	-- Clave de lugar
	and PL.plLSSRID in (select item from split(@LeadSources, ','))
order by L.lsN, P.peps, P.peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

