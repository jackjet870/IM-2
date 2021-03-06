if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_DeleteAssistance]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_DeleteAssistance]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina la asistencia semanal de una persona
** 
** [wtorres]	02/Jul/2014 Created
**
*/
create procedure [dbo].[USP_OR_DeleteAssistance]
	@PlaceType varchar(10),	-- Tipo de lugar
	@Place varchar(10),		-- Clave del lugar
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@Person varchar(10)		-- Clave de la persona
as
set nocount on

delete from Assistance
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

