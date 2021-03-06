USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserSales]    Script Date: 05/09/2016 09:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por Closer
** 
** [edgrodriguez]	07/May/2016 Created.
**
*/
CREATE function [dbo].[UFN_IM_GetCloserSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas	
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes
)
returns @Table table (
	Closer varchar(10),
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (Closer 1)
-- =============================================
select
	S.saCloser1,
	Sum(dbo.UFN_OR_SecureDivision(S.saCloser1P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saCloser1 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser1
UNION ALL
-- Numero de ventas (Closer 2)
-- =============================================
select
	S.saCloser2,
	Sum(dbo.UFN_OR_SecureDivision(S.saCloser2P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saCloser2 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser2
UNION
-- Numero de ventas (Closer 3)
-- =============================================
select
	S.saCloser3,
	Sum(dbo.UFN_OR_SecureDivision(S.saCloser3P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saCloser3 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser3
UNION ALL
-- Numero de ventas (Exit 1)
-- =============================================
select
	S.saExit1,
	Sum(dbo.UFN_OR_SecureDivision(S.saExit1P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saExit1 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saExit1
UNION ALL
-- Numero de ventas (Closer 1)
-- =============================================
select
	S.saExit2,
	Sum(dbo.UFN_OR_SecureDivision(S.saExit2P,(S.saCloser1P + S.saCloser2P + S.saCloser3P + S.saExit1P + S.saExit2P)))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saExit2 is not null
	
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo)))))
		
	-- No downgrades
	and ST.ststc <> 'DG'
	
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	
	-- Sales Rooms
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saExit2
return
end

