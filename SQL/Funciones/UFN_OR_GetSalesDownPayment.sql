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
** Obtiene las membresias que se vendieron en una sala en un rango de fechas determinado
** 
** [LorMartinez] 18/Ene/2016 Modificado, Se agrega funcionalidad para mas de un saleroom
*/
CREATE FUNCTION [dbo].[UFN_OR_GetSalesDownPayment]
(	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final  
	@SalesRoom varchar(MAX)='ALL',
  @Collected bit =0)
RETURNS MONEY
  -- Clave de la sala
as
BEGIN
--set nocount on

DECLARE @Res money
select @Res=0

;with cte as
(
select 	s.saDownPayment,s.saDownPaymentPaid
	from Guests g
	left outer join Sales s on g.guID = s.sagu
  OUTER APPLY (SELECT TOP 1 s2.saID SaleID
               FROM dbo.Sales s2
               WHERE s2.saMembershipNum= s.saMembershipNum
               ORDER By s2.saID DESC
              )LastSale
	where g.guShowD between @DateFrom and @DateTo 
    --Sales room
    and (@SalesRoom='ALL' OR (@SalesRoom <> 'ALL' AND g.gusr in (select item from dbo.split(@SalesRoom,','))))
    --and gusr = @SalesRoom
		-- Procesado
    and s.saProc = 1
		-- No se toman en cuenta las ventas pendientes
		and (g.guShowD = s.saProcD or (g.guShowD <> s.saProcD and s.saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas Bump ni Regen
		and s.sast <> 'BUMP' and s.sast <> 'REGEN'
		-- No se toman en cuenta las ventas canceladas
		and s.saCancel <> 1
UNION ALL    
select
	s.saDownPayment,s.saDownPaymentPaid
	from Guests g
	left outer join Sales s on guID = sagu
  OUTER APPLY (SELECT TOP 1 s2.saID SaleID
               FROM dbo.Sales s2
               WHERE s2.saMembershipNum= s.saMembershipNum
               ORDER By s2.saID DESC
              )LastSale
	where 
		-- Ventas que no tienen Show o que su show es de otro dia
		( (g.guShowD is null or not g.guShowD between @DateFrom and @DateTo)
		-- Ventas del dia  y ventas procesables del dia (no se toman en cuenta las canceladas)
		and (s.saD between @DateFrom and @DateTo or s.saProcD between @DateFrom and @DateTo)
		-- Bumps, Regen y las ventas de otra sala
		or ( (s.sast = 'BUMP' or s.sast = 'REGEN' or g.gusr <> s.sasr) and s.saD between @DateFrom and @DateTo) )
		-- Ventas de la sala  and sasr = @SalesRoom
		and (@SalesRoom='ALL' OR (@SalesRoom <> 'ALL' AND gusr in (select item from dbo.split(@SalesRoom,','))))
		-- No se toman en cuenta los Deposit Before
		and not (s.sast = 'NS' and g.guDepSale > 0 and not (s.saProc = 1 and s.saD <> s.saProcD and s.saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas pendientes
		and not (s.saProcD > @DateTo)
    AND Lastsale.SaleID = s.saID
)
SELECT @Res = CASE 
      WHEN @Collected = 0 THEN sum(isnull(c.saDownPayment,0))
      ELSE sum(isnull(c.saDownPaymentPaid,0))
      END
FROM cte c

RETURN ISNULL(@Res,0)

END
GO