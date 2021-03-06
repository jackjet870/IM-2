if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptMemberships]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptMemberships]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de membresías
** 
** [wtorres]	15/Dic/2010 Ahora se pasa la lista de salas como un sólo parámetro y agregué el parámetro @LeadSources
**
*/
create procedure [dbo].[USP_OR_RptMemberships]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas
	@LeadSources varchar(8000) = 'ALL'	-- Claves de Lead Sources
as
set nocount on

select
	*,
	ProcAmount + OOPAmount - CancAmount as TotalProcAmount
from (
	select
		saID,
		saD,
		saProcD,
		saCancelD,
		salo,
		sasr,
		saMembershipNum,
		samt,
		sast,
		saLastName1,
		saFirstName1,
		saLastName2,
		saFirstName2,
		-- Monto de ventas procesables
		case when saD = saProcD
			and saProcD between @DateFrom and @DateTo
		then saGrossAmount else 0 end
		as ProcAmount,
		-- Monto de ventas out of pending
		case when saD <> saProcD
			and saProcD between @DateFrom and @DateTo
		then saGrossAmount else 0 end
		as OOPAmount,
		-- Monto de ventas canceladas
		case when saProc = 1
			and saCancelD between @DateFrom and @DateTo
		then saGrossAmount else 0 end
		as CancAmount,
		-- Monto de ventas pendientes
		case when (saProc = 0 or saProcD > @DateTo)
			and saD between @DateFrom and @DateTo
			and (saCancel = 0 or saCancelD > @DateTo)
		then saGrossAmount else 0 end
		as PendAmount,
		saPR1,
		saPR2,
		saPR3,
		saLiner1,
		saLiner2,
		saCloser1,
		saCloser2,
		saCloser3,
		saExit1,
		saExit2,
		saVLO,
		saPodium,
		sagu,
		saComments
	from Sales
	where
		-- Fecha de venta
		(saD between @DateFrom and @DateTo
		-- Fecha de procesable
		or saProcD between @DateFrom and @DateTo
		-- Fecha de cancelación
		or saCancelD between @DateFrom and @DateTo)
		-- Salas
		and (@SalesRooms = 'ALL' or sasr in (select item from Split(@SalesRooms, ',')))
		-- LeadSources
		and (@LeadSources = 'ALL' or sals in (select item from Split(@LeadSources, ',')))
) as D
order by saMembershipNum, saID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

