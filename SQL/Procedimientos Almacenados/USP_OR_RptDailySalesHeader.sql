if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptDailySalesHeader]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptDailySalesHeader]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el encabezado del reporte de ventas diarias
** 
** [wtorres]	06/Nov/2008 Creado
** [wtorres]	07/Abr/2009 Dividi el reporte en encabezado y detalle
** [wtorres]	16/Nov/2013 Optimizado.
**							- Reemplace el uso de la funcion UFN_OR_ObtenerShowsReales por UFN_OR_GetShows
**							- Reemplace el uso de las funciones UFN_OR_ObtenerVentasPorTipoMembresia por UFN_OR_GetSales y UFN_OR_GetSalesAmount
** [LorMartinez] 18/Ene/2016 Modificado, Se aumenta el tamaño del campo de Salesroom ahora obtendra mas de un saleroom
*/
CREATE procedure [dbo].[USP_OR_RptDailySalesHeader]
	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final
	@SalesRoom varchar(MAX) ='ALL'	-- Clave de la sala
as
set nocount on

declare @DateFromPrevious datetime,
	@DateToPrevious datetime

set @DateFromPrevious = DateAdd(year, -1, @DateFrom)
set @DateToPrevious = DateAdd(year, -1, @DateTo)

-- devolvemos la tabla encabezado
select
	-- Shows
	dbo.UFN_OR_GetShows(@DateFrom, @DateTo, default, @SalesRoom, default, 4, default, default) as Shows,
	-- Monto de ventas
	dbo.UFN_OR_GetSalesAmount(@DateFrom, @DateTo, default, @SalesRoom, default, default, default, default) as SalesAmount,
	-- Shows del periodo anterior
	dbo.UFN_OR_GetShows(@DateFromPrevious, @DateToPrevious, default, @SalesRoom, default, 4, default, default) as ShowsPrevious,
	-- Monto de ventas del período anterior
	dbo.UFN_OR_GetSalesAmount(@DateFromPrevious, @DateToPrevious, default, @SalesRoom, default, default, default, default) as SalesAmountPrevious
