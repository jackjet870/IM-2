USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el PR con Detalle de Gifts
** 
** [gmaya]			07/Jul/2014 Created
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]  16/Dic/2015 Modified. Se agrega paranetro default en las funciones de 
                  GetBooking y GetShows
*/
CREATE procedure [dbo].[USP_OR_RptCostByPRWithDetailGifts]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderQuinellas bit = 0,	-- Indica si se debe considerar quinielas
	@DetailGifts bit 	-- Indica si desea Detail Gifts
as
set nocount on
	
select
	-- PR
	D.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	D.Qty,
	D.GiftsID, 
	D.GiftsName,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Costo
	Sum(D.Cost) as TotalCost,
	-- Costo promedio
	dbo.UFN_OR_SecureDivision(Sum(D.Cost), Sum(D.Shows)) as AverageCost
from (
	-- Shows
	select PR, Shows, 0 as Cost, 0 as Qty, ''  as GiftsID, '' as GiftsName 
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, default, default, @ConsiderQuinellas, default, default, 0, 0, default, default, default,
		default, default,default)
	-- Monto de recibos de regalos
	union all
	select PR, 0 as Show, GiftsReceiptsAmount, 0 as Qty, ''  as GiftsID, '' as GiftsName 
	from UFN_OR_GetPRGiftsReceiptsAmount(@DateFrom, @DateTo, @LeadSources, 0)
	-- Detalles
	union all
	select PR, 0 as Show, 0 as Cost, Qty, GiftsID, GiftsName 
	from UFN_OR_GetCostByPRWithDetailGifts(@DateFrom, @DateTo, @LeadSources, 0, @DetailGifts)
) as D
	left join Personnel P on D.PR = P.peID
group by D.PR, P.peN, D.Qty, D.GiftsID, D.GiftsName
order by  AverageCost, D.PR
GO