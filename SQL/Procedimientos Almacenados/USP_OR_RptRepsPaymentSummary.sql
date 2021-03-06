if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptRepsPaymentSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptRepsPaymentSummary]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte del resumen pago de agentes
** 
** [wtorres]	14/Oct/2010 Ahora se pasa la lista de Lead Sources como un sólo parámetro
**
*/
create procedure [dbo].[USP_OR_RptRepsPaymentSummary]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000)	-- Claves de Lead Sources
as
set nocount on

-- Crea la tabla
create table #tblData (
	agrp varchar(20),
	TotalShow int default 0,
	agShowPay money default 0,
	SumagShowPay money default 0,
	TotalSales int default 0,
	agSalePay money default 0,
	SumSalesPay money default 0,
	TotalPay money default 0
)

-- Obtiene los shows a pagar
-- =============================================
insert into #tblData (
	agrp,
	TotalShow,
	agShowPay,
	SumagShowPay,
	TotalPay
)
select
	-- Agente
	A.agrp,
	-- Número de shows
	Count(*),
	-- Monto por show
	A.agShowPay,
	-- Monto total de shows
	Sum(A.agShowPay),
	-- Monto total
	Sum(A.agShowPay)
from Guests G
	inner join Agencies A on G.guag = A.agID
	inner join LeadSources L on G.guls = L.lsID
where
	-- Lead Sources
	G.guls in (select item from Split(@LeadSources, ','))
	-- Fecha de book
	and guBookD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1
	-- Mercado de agencias
	and A.agmk = 'AGENCIES'
	-- Agencias a las que se les pagan los shows
	and A.agShowPay > 0
	-- Huéspedes no In & Out ni Walk Out
	and ((G.guInOut = 0 and G.guWalkOut = 0)
	-- Huéspedes In & Out y que el Lead Source pague por In & Out
	or (G.guInOut = 1 and L.lsPayInOut = 1) 
	-- Huéspedes Walk Out y que el Lead Source pague por Walk Out
	or (G.guWalkOut = 1 and L.lsPayWalkOut = 1))
group by A.agrp, A.agShowPay

-- Obtiene las ventas a pagar
-- =============================================
insert into #tblData (
	agrp,
	TotalSales,
	agSalePay,
	SumSalesPay,
	TotalPay
)
select 
	-- Agente
	A.agrp,
	-- Número de ventas
	Count(*),
	-- Monto por venta
	agSalePay,
	-- Monto total de ventas
	sum(agSalePay), 
	-- Monto total
	sum(agSalePay)
from Guests G
	inner join Agencies A on G.guag = A.agID 
where
	-- Lead Sources
	G.guls in (select item from Split(@LeadSources, ','))
	-- Fecha de book
	and g.guBookD between @DateFrom and @DateTo 
	-- Mercado de agencias
	and A.agmk = 'AGENCIES'
	-- Agencias a las que se les pagan las ventas
	and A.agSalePay > 0
	-- Huéspedes con ventas
	and G.guID in (
		select sagu
		from Sales
		where
			-- Fecha de venta procesable
			saProcD between @DateFrom and @DateTo
			-- Ventas no canceladas
			and saCancel = 0
	)
group by A.agrp, A.agSalePay

-- Devuelve la información para el reporte
-- =============================================
select * from #tblData
order by agrp, agSalePay, agShowPay

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

