if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptStatsByLocation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptStatsByLocation]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas por locacion
** 
** [wtorres]	25/Sep/2009 Creado
** [wtorres]	26/Sep/2012 Renombre el campo Shows
** [wtorres]	16/Nov/2013 Reemplace el uso de la funcion UFN_OR_GetLocationSalesByMembershipType por UFN_OR_GetLocationSales
** [gmaya]		25/07/2014 Se modifico el parametro @SalesRoom a varchar(8000)  = 'ALL'
**
*/
create procedure [dbo].[USP_OR_RptStatsByLocation]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(8000)  = 'ALL'	-- Clave de la sala de ventas
as
set nocount on

select
	-- Locacion
	L.loN as Location,
	-- Shows
	Sum(D.Shows) as Shows,
	-- Ventas VIP
	Sum(D.SalesVIP) as SalesVIP,
	-- Ventas Regulares
	Sum(D.SalesRegular) as SalesRegular,
	-- Ventas Exit
	Sum(D.SalesExit) as SalesExit,
	-- Ventas
	Sum(D.Sales) as Sales,
	-- Monto de ventas
	Sum(SalesAmount) as SalesAmount,
	-- Monto de ventas Out Of Pending
	Sum(SalesAmountOOP) as SalesAmountOOP,
	-- Porcentaje de cierre
	[dbo].UFN_OR_SecureDivision(Sum(D.Sales), Sum(D.Shows)) as ClosingFactor,
	-- Eficiencia
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Shows)) as Efficiency,
	-- Venta promedio
	[dbo].UFN_OR_SecureDivision(Sum(D.SalesAmount), Sum(D.Sales)) as AverageSale
from (
	-- Shows
	select Location, Shows, 0 as Sales, 0 as SalesVIP, 0 as SalesRegular, 0 as SalesExit, 0 as SalesAmount, 0 as SalesAmountOOP
	from UFN_OR_GetLocationShows(@DateFrom, @DateTo, @SalesRoom)
	-- Numero de ventas VIP
	union all
	select Location, 0, Sales, Sales, 0, 0, 0, 0 from UFN_OR_GetLocationSales(@DateFrom, @DateTo, @SalesRoom, 'VIP,DIA')
	-- Numero de ventas regulares
	union all
	select Location, 0, Sales, 0, Sales, 0, 0, 0 from UFN_OR_GetLocationSales(@DateFrom, @DateTo, @SalesRoom, 'REG')
	-- Numero de ventas exit
	union all
	select Location, 0, Sales, 0, 0, Sales, 0, 0 from UFN_OR_GetLocationSales(@DateFrom, @DateTo, @SalesRoom, 'EXIT')
	-- Monto de ventas
	union all
	select Location, 0, 0, 0, 0, 0, SalesAmount, 0 from UFN_OR_GetLocationSalesAmount(@DateFrom, @DateTo, @SalesRoom, default)
	-- Monto de ventas Out Of Pending
	union all
	select Location, 0, 0, 0, 0, 0, 0, SalesAmount from UFN_OR_GetLocationSalesAmount(@DateFrom, @DateTo, @SalesRoom, 1)
) as D
	left join Locations L on D.Location = L.loID
group by L.loN
order by SalesAmount desc, Efficiency desc, Shows desc, L.loN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

