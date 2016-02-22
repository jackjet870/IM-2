if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptStatsBySalesRoomLocation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptStatsBySalesRoomLocation]
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
** [caduran]	29/Sep/2014 Creado, basado en USP_OR_RptStatsByLocation  del 25/07/2014
**
*/
create procedure [dbo].[USP_OR_RptStatsBySalesRoomLocation]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(8000)  = 'ALL'	-- Clave de la sala de ventas
as
set nocount on

select
	--Zona del Sales Room
	Z.znN as Zona,
	-- Locacion Id
	L.loID as LocationId,
	-- Locacion
	L.loN as Location,
	-- Sales Room Id
	SR.srId as SalesRoomId,
	-- Sales Room
	SR.srN as SalesRoom,
	-- Program
	P.pgN as Program,
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
	select Location, Shows, 0 as Sales, 0 as SalesVIP, 0 as SalesRegular, 0 as SalesExit, 0 as SalesAmount, 0 as SalesAmountOOP, SalesRoom
	from UFN_OR_GetSalesRoomLocationShows(@DateFrom, @DateTo, @SalesRoom)
	-- Numero de ventas VIP
	union all
	select Location, 0, Sales, Sales, 0, 0, 0, 0, SalesRoom from UFN_OR_GetSalesRoomLocationSales(@DateFrom, @DateTo, @SalesRoom, 'VIP,DIA')
	-- Numero de ventas regulares
	union all
	select Location, 0, Sales, 0, Sales, 0, 0, 0, SalesRoom from UFN_OR_GetSalesRoomLocationSales(@DateFrom, @DateTo, @SalesRoom, 'REG')
	-- Numero de ventas exit
	union all
	select Location, 0, Sales, 0, 0, Sales, 0, 0, SalesRoom from UFN_OR_GetSalesRoomLocationSales(@DateFrom, @DateTo, @SalesRoom, 'EXIT')
	-- Monto de ventas
	union all
	select Location, 0, 0, 0, 0, 0, SalesAmount, 0, SalesRoom from UFN_OR_GetSalesRoomLocationSalesAmount(@DateFrom, @DateTo, @SalesRoom, default)
	-- Monto de ventas Out Of Pending
	union all
	select Location, 0, 0, 0, 0, 0, 0, SalesAmount, SalesRoom from UFN_OR_GetSalesRoomLocationSalesAmount(@DateFrom, @DateTo, @SalesRoom, 1)
) as D
	left join Locations L on D.Location = L.loID
	left join SalesRooms SR on D.SalesRoom = SR.srID
	left join LeadSources LS on L.lols = LS.lsID
	left join Programs P on LS.lspg = P.pgID
	left join LeadSources LSZ on LSZ.lsID = SR.srID
	left join Zones Z on LSZ.lszn = Z.znID
group by Z.znN, SR.srN, SR.srID, L.loN, L.loID, P.pgN
order by Zona, SalesRoom, Program, SalesAmount desc, Efficiency desc, Shows desc, L.loN

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

