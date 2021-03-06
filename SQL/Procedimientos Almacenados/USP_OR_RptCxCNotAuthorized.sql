if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptCxCNotAuthorized]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptCxCNotAuthorized]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de CxC no autorizados
** 
** [wtorres]	21/Feb/2014 Creado
**
*/
create procedure [dbo].[USP_OR_RptCxCNotAuthorized]
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
as
	set nocount on;

-- Recibos de regalos
select
	R.grID,
	R.grNum,
	R.grls,
	R.grgu,
	R.grGuest,
	R.grD,
	R.grpe,
	PR.peN,
	R.grCxCGifts + R.grCxCAdj as grCxCGifts,
	(R.grCxCPRDeposit * IsNull(ED.exExchRate, 1)) as grCxCPRDeposit,
	(R.grCxCTaxiOut * IsNull(ET.exExchRate, 1)) as grCxCTaxiOut,
	-- CxC = CxC de regalos (Cargo + Ajuste) + CxC del deposito + CxC del taxi de salida
	(R.grCxCGifts + R.grCxCAdj) + (R.grCxCPRDeposit * IsNull(ED.exExchRate, 1)) + (R.grCxCTaxiOut * IsNull(ET.exExchRate, 1)) as CxC
from GiftsReceipts R
	left join SalesRooms S on R.grsr = S.srID
	left join Personnel PR on R.grpe = PR.peID
	left join ExchangeRate ED on ED.exD = R.grD and ED.excu = R.grcuCxCPRDeposit
	left join ExchangeRate ET on ET.exD = R.grD and ET.excu = R.grcuCxCTaxiOut
where
	-- No autorizadas
	R.grCxCAppD is null
	-- Fecha del recibo
	and R.grD between @DateFrom and @DateTo
	-- Sala de ventas
	and R.grsr = @SalesRoom
	-- Tengan cargo (de regalos, de deposito o de taxi de salida)
    and ((grCxCGifts + grCxCAdj <> 0) or grCxCPRDeposit > 0 or grCxCTaxiOut > 0)
order by PR.peN, R.grD

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

