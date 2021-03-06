if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPersonnelDaysOff]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPersonnelDaysOff]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el personal de un lugar y sus dias de descanso
** 
** [wtorres]	03/Jul/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetPersonnelDaysOff]
	@TeamType varchar(10),	-- Tipo de equipo
	@PlaceID varchar(10)	-- Clave del lugar
as
set nocount on

select
	-- Persona
	P.peID as dope, P.peN,
	-- Lista de dias de descanso
	D.doList,
	-- Puesto
	PO.poN,
	-- Dias de descanso
	D.doMonday, D.doTuesday, D.doWednesday, D.doThursday, D.doFriday, D.doSaturday, D.doSunday
from Personnel P
	left join DaysOff D on D.dope = P.peID
	left join Posts PO on PO.poID = P.pepo
where
	-- Tipo de equipo
	P.peTeamType = @TeamType
	-- Clave de lugar
	and P.pePlaceID = @PlaceID
	-- Personal activo
	and P.peA = 1
order by P.peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

