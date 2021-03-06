if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCostByPR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCostByPR]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de costo por PR
** 
** [wtorres]		17/Oct/2011 Created
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	16/Dic/2015 Modified. Se agrega paranetro default en las funciones de GetBooking y GetShows
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a la funcion UFN_OR_GetPRShows
**
*/
CREATE procedure [dbo].[USP_OR_RptCostByPR]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0	-- Indica si se debe considerar quinielas
as
set nocount on

select
	-- PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Costo
	Sum(D.Cost) as TotalCost,
	-- Costo promedio
	dbo.UFN_OR_SecureDivision(Sum(D.Cost), Sum(D.Shows)) as AverageCost
from (
	-- Shows
	select PR, Shows, 0 as Cost
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, 0, 0, default, default, default,
		default, default, default, default, default, default, default, default)
	-- Monto de recibos de regalos
	union all
	select PR, 0, GiftsReceiptsAmount
	from UFN_OR_GetPRGiftsReceiptsAmount(@DateFrom, @DateTo, @LeadSources, 0)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN
order by AverageCost, D.PR
