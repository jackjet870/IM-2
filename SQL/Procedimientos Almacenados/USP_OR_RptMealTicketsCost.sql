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
** Devuelve los datos para el reporte de cupones de comida
** 
** [wtorres]	20/Dic/2010 Ahora se pasa la lista de salas como un sólo parámetro
** [LorMartinez] 27/Nov/2015 Madified, Se agrega parameto ratetypes para filtro por ratetype
*/
CREATE procedure [dbo].[USP_OR_RptMealTicketsCost]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@SalesRooms varchar(8000),	-- Claves de salas
  @RateTypes varchar(max) = 'ALL'
as 
set nocount on

-- Datos
-- =============================================
select
	meD as [Date],
	meType as [Type],
	Sum(meAdults * meQty) as Adults,
	Sum(meMinors * meQty) as Minors,
	Sum(meTAdults) as AdultsAmount,
	Sum(meTMinors) as MinorsAmount,
	Sum(meTAdults) + Sum(meTMinors) as TotalAmount
into #tbl_Data
from MealTickets
where
	-- Fecha de cupón
	meD between @DateFrom and @DateTo
	-- Salas
	and mesr in (select item from Split(@SalesRooms, ','))
  --Rate types
  and (@RateTypes='ALL' OR (@RateTypes <> 'ALL' AND  mera in (select item from split(@RateTypes,','))))
group by meD, meType
order by meD, meType

-- 1. Reporte
-- =============================================
select * from #tbl_Data

-- 2. Tipos de cupones comida
-- =============================================
select distinct myID, myN
from #tbl_Data D
	inner join MealTicketTypes T on D.Type = T.myID



GO