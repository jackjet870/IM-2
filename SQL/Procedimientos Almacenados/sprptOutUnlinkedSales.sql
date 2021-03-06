if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptOutUnlinkedSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptOutUnlinkedSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de ventas sin invitacion
** 
** [wtorres]	22/Nov/2013 Ahora se pasa la lista de Lead Sources como un solo parametro
**							Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create procedure [dbo].[sprptOutUnlinkedSales]
 	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@LeadSources as varchar(8000)	-- Claves de los Lead Sources
as
set nocount on

select 
	S.saID,
	S.saD,
	S.saProcD,
	S.saCancelD,
	S.salo,
	S.sasr,
	S.saMembershipNum,
	S.samt,
	S.sast,
	S.saLastName1,
	S.saFirstName1,
	S.saLastName2,
	S.saFirstName2,
	Cast(0 as money) as ProcAmount,
	Cast(0 as money) as OOPAmount,
	Cast(0 as money) as CancAmount,
	Cast(0 as money) as PendAmount,
	S.saPR1,
	S.saPR2,
	S.saPR3,
	S.saLiner1,
	S.saLiner2,
	S.saCloser1,
	S.saCloser2,
	S.saCloser3,
	S.saExit1,
	S.saExit2,
	S.saVLO,
	S.saPodium,
	S.sagu,
	S.saComments
into #Sales
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on G.guID = S.sagu
where
	-- Fecha de cancelacion
    ((S.saCancelD between @DateFrom and @DateTo
    -- Fecha de procesable
    and S.saProcD between @DateFrom and @DateTo)
    -- Fecha de venta
	or ((S.saD between @DateFrom and @DateTo and S.saProc = 0)
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc <> 'DG')))
	-- Lead Sources
	and S.sals in (select item from split(@LeadSources, ','))
	-- Sin invitacion
	and G.guDeposit is null

-- calculamos el monto de ventas procesables
update #Sales
set ProcAmount = saGrossAmount
from #Sales D
	inner join Sales S on S.saID = D.saID
where S.saD = S.saProcD and S.saD between @DateFrom and @DateTo

-- calculamos el monto de ventas Out Of Pending
update #Sales
set	OOPAmount = saGrossAmount
from #Sales D
	inner join Sales on D.saID = S.saID
where S.saD <> S.saProcD and S.saProcD between @DateFrom and @DateTo

-- calculamos el monto de ventas canceladas
update #Sales
set	CancAmount = saGrossAmount
from #Sales D
	inner join Sales S on S.saID = D.saID
where S.saProcD is not null and S.saCancelD between @DateFrom and @DateTo

-- calculamos el monto de ventas pendientes
update #Sales
set	PendAmount = saGrossAmount
from #Sales D
	inner join Sales S on S.saID = D.saID
where (S.saProcD is null or S.saProcD > @DateTo)
	and S.saD between @DateFrom and @DateTo
	and (S.saCancelD is null or S.saCancelD > @DateTo)

-- devolvemos los datos
select *, ProcAmount + OOPAmount - CancAmount AS TotalProcAmount
from #Sales 
order by saMembershipNum, saID

-- eliminamos la tabla temporal
drop table #Sales

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

