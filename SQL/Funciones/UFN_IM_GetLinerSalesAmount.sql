USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_GetLinerSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_GetLinerSalesAmount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por Liner
** 
** [edgrodriguez]	07/May/2016 Created.
** [edgrodriguez]	24/Jun/2016 Modified. Se agrego el parametro @Post para filtrar puestos.
** [ecanul]			29/06/2016 Modified. Agregado parameto @Regen Solo para validar shows Regen
**
*/
CREATE function [dbo].[UFN_IM_GetLinerSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending	
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@ConsiderPending bit = 0,		-- Indica si se debe considerar pendientes
	@saLiner1Type int = -1,			--Filtro de Tipo de liner.
									-- -1.- Sin Filtro
									--	0.- Liner
									--	1.- Front to Middle
									--	2.- Front to Back
	@Post varchar(max) = 'ALL',		-- Claves de los puestos.
	@AllowRepeated bit = 1,			-- Indica si permite vendedores repetidos en diferentes posiciones
	@Regen bit = 0					-- Solo ventas regen Regen
	
	
)
returns @Table table (
	Liner varchar(10),
	LastPost varchar(50),
	SalesAmount money
)
as
begin

insert @Table

-- Monto de ventas (Liner 1)

SELECT 
S.saLiner1,
	dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
	 WHEN @AllowRepeated = 1 THEN @DateTo
	 ELSE S.saD end), S.saLiner1) [LastPost],
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
left join Personnel Pe on S.saLiner1 = Pe.peID 
where
	-- Liner1
	S.saLiner1 is not null
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
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner1)='NP')))
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner1, S.saD

union all

-- Monto de Ventas (Liner 2)
SELECT 
S.saLiner2,
dbo.UFN_IM_GetPersonnelPostIDByDate((CASE
 WHEN @AllowRepeated = 1 THEN @DateTo
 ELSE S.saD end), S.saLiner2) [LastPost],
sum(S.saGrossAmount * dbo.UFN_OR_GetPercentageSalesman(s.saLiner1,s.saLiner2,default,default,default))
FROM
Sales S
left join SaleTypes ST on S.sast = ST.stID
left join Personnel Pe on S.saLiner2 = Pe.peID 
where
	-- Liner 2
	S.saLiner2 is not null
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
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Puestos
	and ((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2) in (select item from split(@Post, ',')))
	OR
	((@Post = 'ALL' or dbo.UFN_IM_GetPersonnelPostIDByDate(S.saD,S.saLiner2)='NP')))
	-- Permitir repetidos
	AND (@AllowRepeated = 1 OR S.saLiner2 NOT IN (IsNull(S.saLiner1, '')))	
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner2, S.saD
return
end
