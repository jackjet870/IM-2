if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetTourTimesAvailables]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetTourTimesAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los horarios de tour disponibles para una sala de ventas y fecha
** 
** [wtorres]	03/Mar/2011 Created
** [wtorres]	02/Jun/2011 Ahora ya no solo se contempla el esquema de horarios de tour por Lead Source, sala de ventas y dia de la semana, sino
**							que ahora se contemplan los siguientes esquemas:
**							- Por Lead Source y sala de ventas
**							- Por sala de ventas y dia de la semana
**
*/
create procedure [dbo].[USP_OR_GetTourTimesAvailables]
	@LeadSource varchar(10),		-- Clave del Lead Source
	@SalesRoom varchar(10),			-- Clave de la sala de ventas
	@Date datetime,					-- Fecha
	@DateOriginal datetime = null,	-- Fecha original
	@TimeOriginal datetime = null,	-- Hora original
	@CurrentDate datetime = null	-- Fecha actual
as
set nocount on;

-- obtenemos el esquema de horarios de tour
declare @Schema int
select @Schema = ocTourTimesSchema from osConfig

-- obtenemos los horarios de tour disponibles por Lead Source y sala de ventas
if @Schema = 1
	exec USP_OR_GetTourTimesByLeadSourceSalesRoomAvailables @LeadSource, @SalesRoom, @Date, @DateOriginal, @TimeOriginal, @CurrentDate

-- obtenemos los horarios de tour disponibles por Lead Source, sala de ventas y dia se la semana
else if @Schema = 2
	exec USP_OR_GetTourTimesByLeadSourceSalesRoomWeekDayAvailables @LeadSource, @SalesRoom, @Date, @DateOriginal, @TimeOriginal, @CurrentDate

-- obtenemos los horarios de tour disponibles por sala de ventas y dia de la semana
else
	exec USP_OR_GetTourTimesBySalesRoomWeekDayAvailables @SalesRoom, @Date, @DateOriginal, @TimeOriginal, @CurrentDate

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

