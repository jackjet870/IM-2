if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetDaysOff]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetDaysOff]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los dias de descanso del personal de un lugar
** 
** [wtorres]	17/Jun/2011 Created
** [lchairez]	27/Nov/2013 Modified. Agregue el parametro @TeamType
** [wtorres]	02/Jul/2014 Modified. Agregue el campo de nombre
**
*/
create procedure [dbo].[USP_OR_GetDaysOff]
	@TeamType varchar(10),	-- Tipo de equipo
	@PlaceID varchar(10)	-- Clave del lugar
as
set nocount on

select
	-- Persona
	D.dope, P.peN,
	-- Lista de dias de descanso
	D.doList,
	-- Dias de descanso
	D.doMonday, D.doTuesday, D.doWednesday, D.doThursday, D.doFriday, D.doSaturday, D.doSunday
from DaysOff D
	left join Personnel P on P.peID = D.dope
where
	-- Tipo de equipo
	P.peTeamType = @TeamType
	-- Clave de lugar
	and P.pePlaceID = @PlaceID
order by P.peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

