if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptMembershipsByHost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptMembershipsByHost]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de membresías por Host
** 
** [wtorres]	20/Dic/2010 Creado
**
*/
create procedure [dbo].[USP_OR_RptMembershipsByHost]
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
		IsNull(G.guEntryHost, '') as guEntryHost,
		IsNull(H.peN, 'Not Specified') as guEntryHostN,
		-- Ventas depósito
		(case when G.guDepSale > 0
			and G.guShowD <> G.guDepositSaleD
			then 'DEPOSIT SALE'		
		-- Ventas regen
		else (case when S.sast = 'REGEN'
			then 'REGEN'
		-- Ventas pendientes
		else (case when (S.saProc = 0 or S.saProcD > @DateTo)
			then 'PENDING'
		-- Ventas procesables
		else (case when S.saProc = 1
			then 'PROCESSABLE'
		else ''
		end) end) end) end) as SaleType,
		S.saID,
		S.saD,
		S.sals,
		S.sasr,
		S.saMembershipNum,
		S.samt,
		S.sast,
		S.saLastName1,
		S.saFirstName1,
		-- Monto de ventas procesables
		case when S.saD = S.saProcD
			and S.saProcD between @DateFrom and @DateTo
		then S.saGrossAmount else 0 end
		as ProcAmount,
		-- Monto de ventas out of pending
		case when S.saD <> S.saProcD
			and S.saProcD between @DateFrom and @DateTo
		then S.saGrossAmount else 0 end
		as OOPAmount,
		-- Monto de ventas canceladas
		case when S.saProc = 1
			and S.saCancelD between @DateFrom and @DateTo
		then S.saGrossAmount else 0 end
		as CancAmount,
		-- Monto de ventas pendientes
		case when (S.saProc = 0 or S.saProcD > @DateTo)
			and S.saD between @DateFrom and @DateTo
			and (S.saCancel = 0 or S.saCancelD > @DateTo)
		then S.saGrossAmount else 0 end
		as PendAmount,
		S.saComments
	from Sales S
		left join Guests G on S.sagu = G.guID
		left join Personnel H on G.guEntryHost = H.peID
	where
		-- Fecha de venta
		(S.saD between @DateFrom and @DateTo
		-- Fecha de procesable
		or S.saProcD between @DateFrom and @DateTo
		-- Fecha de cancelación
		or S.saCancelD between @DateFrom and @DateTo)
		-- Salas
		and (@SalesRooms = 'ALL' or S.sasr in (select item from Split(@SalesRooms, ',')))
		-- LeadSources
		and (@LeadSources = 'ALL' or S.sals in (select item from Split(@LeadSources, ',')))
) as D
order by guEntryHost, SaleType, saMembershipNum, saID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

