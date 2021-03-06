if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptMembershipsByAgencyMarket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptMembershipsByAgencyMarket]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos de reporte de memberships por agencia y mercado
** 
** [lchairez]	01/Nov/2013 Creado
** [wtorres]	16/Dic/2013 Dividi los porcentajes de enganche entre 100 para que aparecieran correctamente en el reporte
** 
*/
create procedure [dbo].[USP_OR_RptMembershipsByAgencyMarket]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL'	-- Claves de salas
as
set nocount on

select
	*,
	ProcAmount + OOPAmount - CancAmount as TotalProcAmount,
	saDownPaymentPercentage * ProcAmount as saDownPayment,
	saDownPaymentPaidPercentage * ProcAmount as saDownPaymentPaid
from (
	select
		M.mkN,
		A.agN,
		S.saID,
		S.saProcD,
		L.lsN,
		SR.srN,
		S.saMembershipNum,
		MT.mtN,
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
		-- Enganche pactado
		S.saDownPaymentPercentage / 100 as saDownPaymentPercentage,
		-- Enganche pagado
		S.saDownPaymentPaidPercentage / 100 as saDownPaymentPaidPercentage,
		S.saComments
	from Sales S
		left join LeadSources L on S.sals = L.lsID
		left join SalesRooms SR on S.sasr = SR.srID
		left join MembershipTypes MT on S.samt = MT.mtID
		left join Guests g on S.sagu = g.guID
		left join Agencies A on g.guAg = A.agID
		left join Markets M on A.agmk = M.mkID
	where
		-- Fecha de venta
		(S.saD between @DateFrom and @DateTo
		-- Fecha de procesable
		or S.saProcD between @DateFrom and @DateTo
		-- Fecha de cancelacion
		or S.saCancelD between @DateFrom and @DateTo)
		-- Salas de ventas
		and (@SalesRooms = 'ALL' or S.sasr in (select item from Split(@SalesRooms, ',')))
) as D
order by mkN, agN, saProcD, srN
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

