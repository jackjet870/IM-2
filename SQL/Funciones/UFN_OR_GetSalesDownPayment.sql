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
select 	saDownPayment,saDownPaymentPaid
	from Guests
	left outer join Sales on guID = sagu
	where guShowD between @DateFrom and @DateTo 
    --Sales room
    and (@SalesRoom='ALL' OR (@SalesRoom <> 'ALL' AND gusr in (select item from dbo.split(@SalesRoom,','))))
    --and gusr = @SalesRoom
		-- Procesado
    and saProc = 1
		-- No se toman en cuenta las ventas pendientes
		and (guShowD = saProcD or (guShowD <> saProcD and saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas Bump ni Regen
		and sast <> 'BUMP' and sast <> 'REGEN'
		-- No se toman en cuenta las ventas canceladas
		and saCancel <> 1
UNION ALL    
select
	saDownPayment,saDownPaymentPaid
	from Guests
	left outer join Sales on guID = sagu
	where 
		-- Ventas que no tienen Show o que su show es de otro dia
		( (guShowD is null or not guShowD between @DateFrom and @DateTo)
		-- Ventas del dia  y ventas procesables del dia (no se toman en cuenta las canceladas)
		and (saD between @DateFrom and @DateTo or saProcD between @DateFrom and @DateTo)
		-- Bumps, Regen y las ventas de otra sala
		or ( (sast = 'BUMP' or sast = 'REGEN' or gusr <> sasr) and saD between @DateFrom and @DateTo) )
		-- Ventas de la sala  and sasr = @SalesRoom
		and (@SalesRoom='ALL' OR (@SalesRoom <> 'ALL' AND gusr in (select item from dbo.split(@SalesRoom,','))))
		-- No se toman en cuenta los Deposit Before
		and not (sast = 'NS' and guDepSale > 0 and not (saProc = 1 and saD <> saProcD and saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas pendientes
		and not (saProcD > @DateTo)
)
SELECT @Res = CASE 
      WHEN @Collected = 0 THEN sum(isnull(c.saDownPayment,0))
      ELSE sum(isnull(c.saDownPaymentPaid,0))
      END
FROM cte c

RETURN ISNULL(@Res,0)

END
GO