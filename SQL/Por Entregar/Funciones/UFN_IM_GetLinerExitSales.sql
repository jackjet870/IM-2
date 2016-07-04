/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas Exit por Liner
** 
** [ecanul]	27/06/2016 Created.
** [ecanul]	29/06/2016 Modified. Agregado parameto @Regen Solo para validar shows Regen
**
*/
ALTER FUNCTION dbo.UFN_IM_GetLinerExitSales(
	@DateFrom Datetime,
	@DateTo Datetime,
	@SalesRooms varchar(MAX) = 'ALL',
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas	
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@saLiner1Type int = -1,			--Filtro de Tipo de liner.
									-- -1.- Sin Filtro
									--	0.- Liner
									--	1.- Front to Middle
									--	2.- Front to Back
	@Regen bit = 0					-- Solo ventas regen Regen	
)

RETURNS @Table TABLE(
	Liner varchar(10),
	ExitSales money
)
AS
BEGIN
INSERT @Table
SELECT S.saLiner1, Sum(dbo.UFN_OR_GetPercentageSalesman(S.saLiner1,S.saLiner2,default,default,default))
FROM dbo.Sales S
	LEFT JOIN dbo.SaleTypes ST ON ST.stID = S.sast
	LEFT JOIN dbo.MembershipTypes MT ON MT.mtID = S.samt
	LEFT JOIN dbo.MembershipGroups MG ON MG.mgID = MT.mtGroup
WHERE 
	-- Liner 1
	S.saLiner1 IS NOT NULL
	--Que sean Exit
	AND MG.mgID = 'EXIT'
	-- Fecha Procesable
	AND ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
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
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
GROUP BY S.saLiner1
UNION ALL
SELECT S.saLiner2, Sum(dbo.UFN_OR_GetPercentageSalesman(S.saLiner1,S.saLiner2,default,default,default))
FROM dbo.Sales S
	LEFT JOIN dbo.SaleTypes ST ON ST.stID = S.sast
	LEFT JOIN dbo.MembershipTypes MT ON MT.mtID = S.samt
	LEFT JOIN dbo.MembershipGroups MG ON MG.mgID = MT.mtGroup
WHERE 
	-- Liner 2
	S.saLiner2 IS NOT NULL
	--Que sean Exit
	AND MG.mgID = 'EXIT'
	-- Fecha Procesable
	AND ((((@ConsiderPending = 0 and S.saProcD between @DateFrom and @DateTo)
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
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
GROUP BY S.saLiner2
RETURN
END