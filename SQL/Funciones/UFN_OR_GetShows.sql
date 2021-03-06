if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetShows]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows
** 
** [wtorres]	23/Nov/2009 Created
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	16/Nov/2013 Modified. Agregue los parametros @SalesRooms, @TourType y @ConsiderDirectsAntesInOut
** [wtorres]	19/Dic/2013 Modified. Agregue el parametro @ConsiderQuinellas
** [wtorres]	02/Sep/2016 Modified. Ahora si no hay registros devuelve cero en lugar de NULL
**
*/
create function [dbo].[UFN_OR_GetShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@TourType int = 0,					-- Filtro de tipo de tour:
										--		0. Sin filtro
										--		1. Tours regulares
										--		2. Tours de cortesia
										--		3. Tours de rescate
										--		4. Tours regulares o Walk Outs (sirve para los reportes de Processor Sales)
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns int
as
begin

declare @Result int

select @Result = IsNull(Sum(case when @ConsiderQuinellas = 0 then 1 else guShowsQty end), 0)
from Guests
where
	-- Fecha de show
	((@BasedOnArrival = 0 and guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Con show
	and guShow = 1))
	-- Lead Sources
	and (@LeadSources = 'ALL' or guls in (select item from split(@LeadSources, ','))) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or gusr in (select item from split(@SalesRooms, ','))) 
	-- Filtro de tipo de tour
	and (@TourType = 0 or (@TourType = 1 and guTour = 1) or (@TourType = 2 and guCTour = 1) or (@TourType = 3 and guSaveProgram = 1)
		or (@TourType = 4 and (guTour = 1 or guWalkOut = 1)))
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))

return @Result
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

