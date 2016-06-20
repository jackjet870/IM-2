USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetCloserSalesAmount]    Script Date: 05/09/2016 12:04:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por Closer
** 
** [edgrodriguez]	07/May/2016 Created.
**
*/
CREATE function [dbo].[UFN_IM_GetCloserSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending	
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@ConsiderPending bit = 0		-- Indica si se debe considerar pendientes
	
)
returns @Table table (
	Closer varchar(10),
	SalesAmount money
)
as
begin

insert @Table

-- Monto de ventas (Closer 1)

SELECT 
S.saCloser1,
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer1
	S.saCloser1 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser1

union all

-- Monto de Ventas (Closer 2)
SELECT 
S.saCloser2,
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer 2
	S.saCloser2 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser2

union all

-- Monto de Ventas (Closer 3)
SELECT 
S.saCloser3,
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer 3
	S.saCloser3 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saCloser3

union all

-- Monto de Ventas (Exit 1)
SELECT 
S.saExit1,
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
where
	-- Exit 1
	S.saExit1 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saExit1

union all

-- Monto de Ventas (Exit 2)
SELECT 
S.saExit2,
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
where
	-- Closer 2
	S.saExit2 is not null
	-- Fecha de procesable
	and ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de venta
	or (@ConsiderPending = 1 and S.saD between @DateFrom and @DateTo and (S.saProc = 0 or not S.saProcD between @DateFrom and @DateTo))))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.saExit2
return
end
