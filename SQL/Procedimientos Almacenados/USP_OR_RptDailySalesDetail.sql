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
** Devuelve los datos para el detalle del reporte de ventas diarias
** 
** [wtorres]	06/Nov/2008 Creado
** [wtorres]	07/Abr/2009 Dividi el reporte en encabezado y detalle
** [wtorres]	16/Nov/2013 Optimizado.
**							- Reemplace el uso de la funcion UFN_OR_ObtenerShowsReales por UFN_OR_GetShows
**							- Reemplace el uso de las funciones UFN_OR_ObtenerVentasPorTipoMembresia, UFN_OR_ObtenerOutOfPending
**							  y UFN_OR_ObtenerVentasCanceladas por UFN_OR_GetSales y UFN_OR_GetSalesAmount
** [LorMartinez] 18/Ene/2016, Modificado, Se agrega manejo de mas de un saleroom
*/
CREATE procedure [dbo].[USP_OR_RptDailySalesDetail]
	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final
	@SalesRoom varchar(MAX)= 'ALL'	-- Clave de la sala
as
set nocount on

-- Esta variable contendra la tabla de detalle
declare @TableDetail table(
	[Date] datetime,
	Shows int,
	SalesRegular int,
	SalesExit int,
	SalesVIP int,
	SalesAmount money,
	SalesAmountOOP money,
	SalesAmountCancel money,
  DownPact money,
  DownColl money,
  CnxSalesAmount money
)

declare @Date datetime

set @Date = @DateFrom

--Almacena el resultado de los colectados
declare @TableSale table(
	MembershipNum varchar(10),
  SaleRoom varchar(10),
  Down money,
  Down_Coll money
)


-- mientras haya mas fechas
while @Date <= @DateTo
begin
	
	-- agregamos el dia a la tabla detalle
	insert into @TableDetail--([Date],Shows,SalesRegular,SalesExit,SalesVIP,SalesAmount,SalesAmountOOP,SalesAmountCancel)
	values(
		@Date,
		-- Shows
		dbo.UFN_OR_GetShows(@Date, @Date, default, @SalesRoom, default, 4, default, default),
		-- Numero de ventas regulares
		dbo.UFN_OR_GetSales(@Date, @Date, default, @SalesRoom, 'REG', default, default, default),
		-- Numero de ventas exit
		dbo.UFN_OR_GetSales(@Date, @Date, default, @SalesRoom, 'EXIT', default, default, default),
		-- Numero de ventas VIP
		dbo.UFN_OR_GetSales(@Date, @Date, default, @SalesRoom, 'VIP,DIAMOND', default, default, default),
		-- Monto de ventas
		dbo.UFN_OR_GetSalesAmount(@Date, @Date, default, @SalesRoom, default, default, default, default),
		-- Monto de ventas Out Of Pending
		dbo.UFN_OR_GetSalesAmount(@Date, @Date, default, @SalesRoom, default, 1, default, default),
		-- Monto de ventas canceladas
		dbo.UFN_OR_GetSalesAmount(@Date, @Date, default, @SalesRoom, default, default, 1, default),
    --DownPayment
    dbo.UFN_OR_GetSalesDownPayment(@Date,@Date,@SalesRoom,0),
    --DownPayment Collected
    dbo.UFN_OR_GetSalesDownPayment(@Date,@Date,@SalesRoom,1),
    --Ventas canceladas
    dbo.UFN_OR_GetCnxSalesAmount(@Date,@Date,@SalesRoom)
	)
	/*
  INSERT INTO @TableSale
  exec USP_OR_ObtenerMembresiasPorSalaFecha @Date, @Date, @SalesRoom
  
  UPDATE @TableDetail
  SET downpact = (select sum(down) from @tablesale ),
      DownColl = (select sum(Down_Coll) from @tablesale),
      SHOWS = isnull(shows,0)
  WHERE [Date] = @Date
  
  delete @TableSale
  */  
	-- aumentamos la fecha temporal
	set @Date = DateAdd(day, 1, @Date)
end

-- devolvemos la tabla detalle
select [Date] ,
	     isnull(Shows,0) [Shows] ,
	     SalesRegular ,
	     SalesExit ,
	     SalesVIP ,
    	SalesAmount ,
    	SalesAmountOOP ,
    	SalesAmountCancel ,
      DownPact ,
      DownColl,
      CnxSalesAmount
from @TableDetail
GO