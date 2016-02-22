if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetAssistance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetAssistance]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta la asistencia del personal de un lugar
** 
** [wtorres]	02/Jul/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetAssistance]
	@PlaceType varchar(10),	-- Tipo de lugar
	@Place varchar(10),		-- Clave del lugar
	@DateFrom datetime,		-- Fecha inicial de la semana
	@DateTo datetime		-- Fecha final de la semana
as
set nocount on

select
	-- Tipo de lugar
	A.asPlaceType,
	-- Lugar
	A.asPlaceID,
	-- Fechas
	A.asStartD,
	A.asEndD,
	-- Persona
	A.aspe,
	P.peN,
	-- Dias de asistencia
	A.asMonday,
	A.asTuesday,
	A.asWednesday,
	A.asThursday,
	A.asFriday,
	A.asSaturday,
	A.asSunday,
	-- Numero de asistencias
	A.asNum
from Assistance A
	left join Personnel P on a.aspe = p.peID
where
	-- Tipo de lugar
	A.asPlaceType = @PlaceType
	-- Clave de lugar
	and A.asPlaceID = @Place
	-- Fechas
	and asStartD = @DateFrom and asEndD = @DateTo
order by P.peN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

