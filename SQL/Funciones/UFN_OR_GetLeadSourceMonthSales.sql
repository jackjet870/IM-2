if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLeadSourceMonthSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLeadSourceMonthSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por mes y lead source
** 
** [caduran]	23/Sep/2013 Created. Se agrego la agrupacion por Lead Source, se baso en el procedimiento 'UFN_OR_GetMonthSales'
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetLeadSourceMonthSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@Market varchar(10) = 'ALL',		-- Clave del mercado
	@ConsiderNights bit = 0,			-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,				-- Numero de noches desde
	@NightsTo int = 0,					-- Numero de noches hasta
	@Agency varchar(35) = 'ALL',		-- Clave de agencia
	@Contracts varchar(8000) = 'ALL',	-- Claves de contratos
	@External int = 0,					-- Filtro de invitaciones externas
										--		0. Sin filtro
										--		1. Excluir invitaciones externas
										--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	LeadSource varchar(10),
	Sales int
)
as
begin

-- si no se debe basar en la fecha de llegada
if @BasedOnArrival = 0

	insert @Table
	select
		Year(S.saProcD),
		Month(S.saProcD),
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on L.lsID = S.sals
	where
		-- Fecha de procesable
		S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Lead Sources
		and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
		-- Mercado
		and (@Market = 'ALL' or G.gumk = @Market)
		-- Numero de noches
		and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
		-- Agencia
		and (@Agency = 'ALL' or G.guag like @Agency)
		-- Contrato
		and (@Contracts = 'ALL' or (CharIndex(',', @Contracts) > 0 and G.guO1 in (select item from Split(@Contracts, ',')))
			or (CharIndex(',', @Contracts) = 0 and G.guO1 like @Contracts))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by Year(S.saProcD), Month(S.saProcD), S.sals

-- si se debe basar en la fecha de llegada
else

	insert @Table

	-- Ventas con huesped
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on L.lsID = S.sals
	where
		-- Con huesped
		S.sagu is not null
		-- Fecha de llegada
		and G.guCheckInD between @DateFrom and @DateTo
		-- Procesable
		and S.saProc = 1
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Lead Sources
		and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
		-- Mercado
		and (@Market = 'ALL' or G.gumk = @Market)
		-- Numero de noches
		and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
		-- Agencia
		and (@Agency = 'ALL' or G.guag like @Agency)
		-- Contrato
		and (@Contracts = 'ALL' or (CharIndex(',', @Contracts) > 0 and G.guO1 in (select item from Split(@Contracts, ',')))
			or (CharIndex(',', @Contracts) = 0 and G.guO1 like @Contracts))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by Year(G.guCheckInD), Month(G.guCheckInD), S.sals

	-- Ventas sin huesped
	union all
	select
		Year(S.saProcD),
		Month(S.saProcD),
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on L.lsID = S.sals
	where
		-- Sin huesped
		S.sagu is null
		-- Fecha de procesable
		and S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Lead Sources
		and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
		-- Mercado
		and (@Market = 'ALL' or G.gumk = @Market)
		-- Numero de noches
		and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
		-- Agencia
		and (@Agency = 'ALL' or G.guag like @Agency)
		-- Contrato
		and (@Contracts = 'ALL' or (CharIndex(',', @Contracts) > 0 and G.guO1 in (select item from Split(@Contracts, ',')))
			or (CharIndex(',', @Contracts) = 0 and G.guO1 like @Contracts))
		-- Invitaciones externas
		and (@External = 0
			or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
			or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
	group by Year(S.saProcD), Month(S.saProcD), S.sals

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

