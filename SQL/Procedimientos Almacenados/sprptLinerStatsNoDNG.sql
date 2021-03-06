if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptLinerStatsNoDNG]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptLinerStatsNoDNG]
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
create procedure [dbo].[sprptLinerStatsNoDNG]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@SalesRooms as varchar(8000)	-- Claves de las salas de ventas
as
set nocount on

-- Huespedes
-- =============================================
select
	G.guShowD,
	G.guWalkOut,
	G.guLiner1Type,
	G.guLiner1,
	L1.peN as guLiner1N,
	L1.peps as guLiner1ps,
	G.guLiner2,
	L2.peN as guLiner2N,
	L2.peps as guLiner2ps
from Guests G
	left join Personnel L1 on L1.peID = G.guLiner1
	left join Personnel L2 on L2.peID = G.guLiner2
where
	-- Salas de ventas
	G.gusr in (select item from split(@SalesRooms, ','))
	-- Fecha de show
	and G.guShowD between @DateFrom and @DateTo
	-- Tour regular
	and (G.guTour = 1
	-- Walk Out
	or G.guWalkOut = 1)

-- Ventas
-- =============================================
select 
	S.saLiner1Type,
	S.saLiner1,
	L1.peN as saLiner1N, 
	L1.peps as saLiner1ps,
	S.saLiner2,
	L2.peN as saLiner2N,
	L2.peps as saLiner2ps,
	S.saD,
	S.saProcD,
	S.saCancelD,
	S.saGrossAmount,
	ST.ststc
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Personnel L1 on L1.peID = S.saLiner1
	left join Personnel L2 on L2.peID = S.saLiner2
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

