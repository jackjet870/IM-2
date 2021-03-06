USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_GetCloserSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_GetCloserSalesAmount]
GO
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
** [edgrodriguez]	24/Jun/2016 Modified. Se agrego el parametro @Post para filtrar puestos.
**
*/
CREATE function [dbo].[UFN_IM_GetCloserSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending	
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@Post varchar(max) = 'ALL',		-- Clave de puestos
	@AllowRepeated bit = 1,			-- Indica si permite vendedores repetidos en diferentes posiciones
	@Roles varchar(max) = 'ALL'					-- Indicar los roles que deben considerar
)
returns @Table table (
	Closer varchar(10),
	LastPost varchar(50),
	SalesAmount money
)
as
begin
insert @Table
-- Monto de ventas (Closer 1)
SELECT 
	S.saCloser1,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saCloser1) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'CLOSER' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,default,default)
		ELSE
		cast(0 as Money)
	END))
FROM Sales S
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
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser1)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saCloser1 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, '')))
group by S.saCloser1, S.saD

-- Monto de Ventas (Closer 2)
union all
SELECT 
	S.saCloser2,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saCloser2) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'CLOSER' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,default,default)
		ELSE
		Cast(0 as Money)
	END))
FROM Sales S
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
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser2)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saCloser2 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, '')))
	
group by S.saCloser2, S.saD

union all

-- Monto de Ventas (Closer 3)
SELECT 
	S.saCloser3,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saCloser3) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'CLOSER' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,default,default)
		ELSE
		Cast(0 as money)
	END))
FROM Sales S
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
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saCloser3)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saCloser3 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, ''), IsNull(S.saCloser2,'')))	
group by S.saCloser3, S.saD

union all

-- Monto de Ventas (Exit 1)
SELECT 
	S.saExit1,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saExit1) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'EXIT' THEN dbo.UFN_OR_GetPercentageSalesman(default,default,default,s.saExit1,s.saExit2)
		ELSE
		CAST(0 AS MONEY)
	END))
FROM Sales S
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
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit1)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saExit1 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, ''), IsNull(S.saCloser2,''),IsNull(S.saCloser3,'')))	
group by S.saExit1, S.saD

union all

-- Monto de Ventas (Exit 2)
SELECT 
	S.saExit2,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saExit2) [LastPost],
	sum(S.saGrossAmount * (CASE 
		WHEN @Roles = 'ALL' THEN dbo.UFN_OR_GetPercentageSalesman(s.saCloser1,s.saCloser2,s.saCloser3,s.saExit1,s.saExit2)
		WHEN @Roles = 'EXIT' THEN dbo.UFN_OR_GetPercentageSalesman(default,default,default,s.saExit1,s.saExit2)
		ELSE
		CAST(0 AS MONEY)
	END))FROM Sales S
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
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saExit2)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saExit2 NOT IN (IsNull(S.saLiner1, ''), IsNull(S.saLiner2, ''), IsNull(S.saCloser1, ''), IsNull(S.saCloser2,''),IsNull(S.saCloser3,''),IsNull(S.saExit1,'')))	
group by S.saExit2, S.saD
return
end
