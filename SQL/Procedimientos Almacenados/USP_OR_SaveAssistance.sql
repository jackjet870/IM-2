if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveAssistance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveAssistance]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Guarda la asistencia semanal de una persona
** 
** [wtorres]	02/Jul/2014 Creado
**
*/
create procedure [dbo].[USP_OR_SaveAssistance]
	@PlaceType varchar(10),	-- Tipo de lugar
	@Place varchar(10),		-- Clave del lugar
	@DateFrom datetime,		-- Fecha inicial de la semana
	@DateTo datetime,		-- Fecha final de la semana
	@Person varchar(10),	-- Clave de la persona
	@Monday varchar(10),	-- Clave del estatus de asistencia del Lunes
	@Tuesday varchar(10),	-- Clave del estatus de asistencia del Martes
	@Wednesday varchar(10),	-- Clave del estatus de asistencia del Miercoles
	@Thursday varchar(10),	-- Clave del estatus de asistencia del Jueves
	@Friday varchar(10),	-- Clave del estatus de asistencia del Viernes
	@Saturday varchar(10),	-- Clave del estatus de asistencia del Sabado
	@Sunday varchar(10)		-- Clave del estatus de asistencia del Domingo
as
set nocount on

-- si no existe la asistencia, la agregamos
if not exists (select top 1 null from Assistance where
		-- Tipo de lugar
		asPlaceType = @PlaceType
		-- Clave de lugar
		and asPlaceID = @Place
		-- Fechas
		and asStartD = @DateFrom and asEndD = @DateTo
		-- Persona
		and aspe = @Person)
	insert into Assistance (asPlaceType, asPlaceID, asStartD, asEndD, aspe,
		asMonday, asTuesday, asWednesday, asThursday, asFriday, asSaturday, asSunday)
	values (@PlaceType, @Place, @DateFrom, @DateTo, @Person,
		@Monday, @Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday)

-- si existe la asistencia, la actualizamos
else
	update Assistance
	set asMonday = @Monday,
		asTuesday = @Tuesday,
		asWednesday = @Wednesday,
		asThursday = @Thursday,
		asFriday = @Friday,
		asSaturday = @Saturday,
		asSunday = @Sunday
	where
		-- Tipo de lugar
		asPlaceType = @PlaceType
		-- Clave de lugar
		and asPlaceID = @Place
		-- Fechas
		and asStartD = @DateFrom and asEndD = @DateTo
		-- Persona
		and aspe = @Person

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

