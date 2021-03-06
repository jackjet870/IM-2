if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLeadSourceMarketMonthSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLeadSourceMarketMonthSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por Lead Source, mercado y mes
** 
** [wtorres]	04/Feb/2010 Created
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [caduran]	17/Dic/2014 Modified. Se agregaron los parametros @LeadSources y @Program; Se agrego columna Program
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetLeadSourceMarketMonthSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@External int = 0,					-- Filtro de invitaciones externas
										--		0. Sin filtro
										--		1. Excluir invitaciones externas
										--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	LeadSource varchar(10),
	Market varchar(10),
	[Year] int,
	[Month] int,
	Program varchar(10),
	SalesAmount money
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		S.sals,
		G.gumk,
		Year(S.saProcD),
		Month(S.saProcD),
		L.lspg,
		Sum(saGrossAmount)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on G.guID = S.sagu
		left join LeadSources L on L.lsID = S.sals
	where
		-- Fecha de procesable
		S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and (ST.ststc <> 'DG'
		-- Downgrades cuya membresia de referencia esta dentro del periodo
		or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Programa
		and (@Program = 'ALL' or L.lspg = @Program)
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by S.sals, G.gumk, Year(S.saProcD), Month(S.saProcD), L.lspg

-- si se debe basar en la fecha de llegada
else

	insert @Table

	-- Ventas con huesped
	select
		S.sals,
		G.gumk,
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		L.lspg,
		Sum(saGrossAmount)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on G.guID = S.sagu
		left join LeadSources L on L.lsID = S.sals
	where
		-- Con huesped
		S.sagu is not null
		-- Fecha de llegada
		and G.guCheckInD between @DateFrom and @DateTo
		-- Procesable
		and S.saProc = 1
		-- No downgrades
		and (ST.ststc <> 'DG'
		-- Downgrades cuya membresia de referencia esta dentro del periodo
		or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Programa
		and (@Program = 'ALL' or L.lspg = @Program)
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by S.sals, G.gumk, Year(G.guCheckInD), Month(G.guCheckInD), L.lspg

	-- Ventas sin huesped
	union all
	select
		S.sals,
		G.gumk,
		Year(S.saProcD),
		Month(S.saProcD),
		L.lspg,
		Sum(saGrossAmount)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on G.guID = S.sagu
		left join LeadSources L on L.lsID = S.sals
	where
		-- Sin huesped
		S.sagu is null
		-- Fecha de procesable
		and S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and (ST.ststc <> 'DG'
		-- Downgrades cuya membresia de referencia esta dentro del periodo
		or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Programa
		and (@Program = 'ALL' or L.lspg = @Program)
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
		-- Lead Sources
		and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ',')))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by S.sals, G.gumk, Year(S.saProcD), Month(S.saProcD), L.lspg

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

