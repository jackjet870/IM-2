if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetTourTimesByLeadSourceSalesRoomAvailables]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetTourTimesByLeadSourceSalesRoomAvailables]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los horarios de tour disponibles por Lead Source y sala de ventas para una sala de ventas y fecha
** 
** [wtorres]	02/Jun/2011 Created
**
*/
create procedure [dbo].[USP_OR_GetTourTimesByLeadSourceSalesRoomAvailables]
	@LeadSource varchar(10),		-- Clave del Lead Source
	@SalesRoom varchar(10),			-- Clave de la sala de ventas
	@Date datetime,					-- Fecha
	@DateOriginal datetime = null,	-- Fecha original
	@TimeOriginal datetime = null,	-- Hora original
	@CurrentDate datetime = null	-- Fecha actual
as
set nocount on;

-- si tiene fecha
if @Date is not null
begin

	-- obtenemos la fecha actual, si no fue enviada
	if @CurrentDate is null
		set @CurrentDate = Convert(varchar, GetDate(), 112)

	select
		ttT as Tour,
		ttPickUpT as PickUp, 
		Convert(varchar(5), ttT, 114) + '   ' + Convert(varchar(5), ttPickUpT, 114) as [Text]
	from TourTimes
	where
		-- Lead Source
		ttls = @LeadSource
		-- Sala de ventas
		and ttsr = @SalesRoom
		-- si la fecha de booking es hoy, no validamos la disponibilidad
		and ((@Date = @CurrentDate and ttMaxBooks > 0)
		-- si la fecha de booking no es hoy, validamos que haya horarios disponibles
		or (@Date <> @CurrentDate and (exists (
			select Count(*)
			from Guests 
			where 
				guls = @LeadSource
				and gusr = @SalesRoom
				and ((guBookD = @Date and guBookT = ttT) 
				or (guReschD = @Date and guReschT = ttT))
			having Count(*) < ttMaxBooks)
		-- si la fecha es igual a la fecha original, permitimos seleccionar el mismo horario
		or (@Date = @DateOriginal and ttT = @TimeOriginal))))
end
else

	-- enviamos un conjunto de datos vacio
	select ttT as Tour, ttPickUpT as PickUp, '             ' as [Text] from TourTimes where 1 = 2

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

