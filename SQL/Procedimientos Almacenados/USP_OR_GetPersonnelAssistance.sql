if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPersonnelAssistance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPersonnelAssistance]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta el personal de un lugar para crear las asistencias
** 
** [wtorres]	02/Jul/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetPersonnelAssistance]
	@PlaceType varchar(10),	-- Tipo de lugar
	@Place varchar(10),		-- Clave del lugar
	@DateFrom datetime,		-- Fecha inicial de la semana
	@DateTo datetime		-- Fecha final de la semana
as
set nocount on

select
	-- Tipo de lugar
	@PlaceType as asPlaceType,
	-- Lugar
	@Place as asPlaceID,
	-- Fechas
	@DateFrom as asStartD,
	@DateTo as asEndD,
	-- Persona
	P.peID as aspe,
	P.peN,
	-- Dias de asistencia
	case when doMonday = 1 then 'D' else 'A' end as asMonday,
	case when doTuesday = 1 then 'D' else 'A' end as asTuesday,
	case when doWednesday = 1 then 'D' else 'A' end as asWednesday,
	case when doThursday = 1 then 'D' else 'A' end as asThursday,
	case when doFriday = 1 then 'D' else 'A' end as asFriday,
	case when doSaturday = 1 then 'D' else 'A' end as asSaturday,
	case when doSunday = 1 then 'D' else 'A' end as asSunday,
	-- Numero de asistencias
	7 as asNum
from Personnel P
	left join DaysOff D on D.dope = P.peID
where
	-- Tipo de equipo
	P.peTeamType = (case when @PlaceType = 'LS' then 'GS' else 'SA' end)
	-- Clave del lugar
	and P.pePlaceID = @Place
	-- Personal activo
	and P.peA = 1
order by peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

