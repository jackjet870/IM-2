if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptPersonnel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptPersonnel]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de personal
** 
** [wtorres]	24/Ene/2014 Modified. Ahora los roles se obtienen de la tabla de roles de personal.
**							Elimine el rol Contracts Person y agregue el rol de PR Supervisor.
**							Agregue el campo de departamento.
** [wtorres]	30/May/2015 Modified. Agregue los campos de puesto, lugar y numero de colaborador
**
*/
create procedure [dbo].[USP_OR_RptPersonnel]
as
set nocount on

select
	P.peps,
	IsNull(D.deN, 'NO DEPARTMENT') as deN,
	IsNull(PO.poN, 'NO POST') as poN,
	IsNull(case when P.peTeamType = 'GS' then LO.loN else SR.srN end, 'NO PLACE') as Place,
	P.peID,
	P.peN,
	P.peA,
	P.peCollaboratorID,
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
	left join Depts D on D.deID = P.pede
	left join Posts PO on PO.poID = P.pepo
	left join Locations LO on P.peTeamType = 'GS' and LO.loID = P.pePlaceID
	left join SalesRooms SR on P.peTeamType = 'SA' and SR.srID = P.pePlaceID
order by P.peps, D.deN, PO.poN, Place, P.peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

