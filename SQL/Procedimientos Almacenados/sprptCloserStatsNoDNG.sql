if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptCloserStatsNoDNG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptCloserStatsNoDNG]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de estadisticas de Liners
** 
** [wtorres]	19/Nov/2013 Ahora se pasa la lista de salas de ventas como un solo parametro
**							Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create procedure [dbo].[sprptCloserStatsNoDNG]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@SalesRooms as varchar(8000)	-- Claves de las salas de ventas
as
set nocount on

-- Huespedes
-- =============================================
select 
	G.guWalkOut,
	G.guCloser1,
	C1.peN as guCloser1N,
	C1.peps as guCloser1ps,
	G.guCloser2,
	C2.peN as guCloser2N,
	C2.peps as guCloser2ps,
	G.guCloser3,
	C3.peN as guCloser3N,
	C3.peps as guCloser3ps,
	G.guExit1,
	E1.peN as guExit1N,
	E1.peps as guExit1ps,
	G.guExit2,
	E2.peN as guExit2N,
	E2.peps as guExit2ps
from Guests G
	left join Personnel C1 on C1.peID = G.guCloser1
	left join Personnel C2 on C2.peID = G.guCloser2
	left join Personnel C3 on C3.peID = G.guCloser3
	left join Personnel E1 on E1.peID = G.guExit1
	left join Personnel E2 on E2.peID = G.guExit2
where
	-- Salas de ventas
	G.gusr in (select item from split(@SalesRooms, ','))
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Tour regular
	and G.guTour = 1

-- Ventas
-- =============================================
select 
	S.saCloser1,
	C1.peN as saCloser1N,
	C1.peps as saCloser1ps,
	S.saCloser2,
	C2.peN as saCloser2N,
	C2.peps as saCloser2ps,
	S.saCloser3,
	C3.peN as saCloser3N,
	C3.peps as saCloser3ps,
	S.saExit1,
	E1.peN as saExit1N,
	E1.peps as saExit1ps,
	S.saExit2,
	E2.peN as saExit2N,
	E2.peps as saExit2ps,
	S.saCloser1P,
	S.saCloser2P,
	S.saCloser3P,
	S.saExit1P,
	S.saExit2P,
	ST.ststc,
	S.saD,
	S.saProcD,
	S.saCancelD,
	S.saGrossAmount
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Personnel C1 on C1.peID = S.saCloser1
	left join Personnel C2 on C2.peID = S.saCloser2
	left join Personnel C3 on C3.peID = S.saCloser3
	left join Personnel E1 on E1.peID = S.saExit1
	left join Personnel E2 on E2.peID = S.saExit2
where
	-- Salas de ventas
	S.sasr in (select item from split(@SalesRooms, ','))
	-- Fecha de cancelacion
	and (S.saCancelD is null or S.saCancelD not between @DateFrom and @DateTo)
	-- Fecha de venta
	and ((S.saD between @DateFrom and @DateTo and S.saProc = 0)
	-- Fecha de procesable
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)
	or (S.saProcD between @DateFrom and @DateTo and ST.ststc <> 'DG'))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

