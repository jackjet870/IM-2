USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetLinerSales]    Script Date: 06/29/2016 11:14:47 ******/
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
** [ecanul]			29/06/2016 Modified. Agregado parameto @Regen Solo para validar shows Regen
**
*/
ALTER function [dbo].[UFN_IM_GetLinerSales](
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
	@Regen bit = 0					-- Solo ventas regen Regen
)
returns @Table table (
	Liner varchar(10),
	Sales money
)
as
begin

insert @Table

-- Numero de ventas (Liner 1)
-- =============================================
select
	S.saLiner1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saLiner1,S.saLiner2,default,default,default))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Closer1 1
	S.saLiner1 is not null
	
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
	
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner1
UNION ALL
-- Numero de ventas (Liner 2)
-- =============================================
select
	S.saLiner2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(S.saLiner1,S.saLiner2,default,default,default))
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Liner 2
	S.saLiner2 is not null
	
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
	
	--Tipo de Liner
	and(@saLiner1Type = -1 or S.saLiner1Type=@saLiner1Type)
	-- Ventas Regen
	AND (@Regen = 0 OR S.sast = 'REGEN')
group by S.saLiner2
return
end

