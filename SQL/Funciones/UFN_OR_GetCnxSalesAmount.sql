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
** Devuelve el monto de de ventas canceladas
** [Lormartinez] 19/Ene/2016, Created
*/
CREATE FUNCTION dbo.UFN_OR_GetCnxSalesAmount(
@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final
	@SalesRoom varchar(MAX)= 'ALL'	-- Clave de la sala
) RETURNS MONEY
AS
BEGIN
 DECLARE @RES money
 SELECT @RES= 0
 
  select 
	    @res = sum(SALE)
  FROM dbo.Sales s  
  INNER JOIN hotel.analista_h.CLMEMBER m ON m.[APPLICATION] = s.saMembershipNum
  WHERE s.saProcD between @DateFrom AND @DateTo  
  --Salas de venta
  and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ','))) 
  -- Ventas canceladas por código de venta caída
  and CANCEL_SALE_DT <> '17530101' and cnxcod_id = 'VC'  
  -- Agrupadas por día
  group by  s.saProcD
  
  RETURN ISNULL(@RES,0)

END
GO