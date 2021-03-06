if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMonthAgencyLeadSourceSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMonthAgencyLeadSourceSales]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por mes, agencia y Lead Source
** 
** [wtorres]	22/Oct/2009 Created
** [wtorres]	24/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetMonthAgencyLeadSourceSales](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Year] int,
	[Month] int,
	Agency varchar(35),
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
		G.guag,
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Fecha de procesable
		S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(S.saProcD), Month(S.saProcD), G.guag, S.sals

-- si se debe basar en la fecha de llegada
else

	insert @Table

	-- Ventas con huesped
	select
		Year(G.guCheckInD),
		Month(G.guCheckInD),
		G.guag,
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
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
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(G.guCheckInD), Month(G.guCheckInD), G.guag, S.sals

	-- Ventas sin huesped
	union all
	select
		Year(S.saProcD),
		Month(S.saProcD),
		G.guag,
		S.sals,
		Count(*)
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		left join LeadSources L on S.sals = L.lsID
	where
		-- Sin huesped
		S.sagu is null
		-- Fecha de procesable
		and S.saProcD between @DateFrom and @DateTo
		-- No downgrades
		and ST.ststc <> 'DG'
		-- No canceladas o canceladas fuera del periodo
		and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
		-- Agencias
		and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
		-- Programa
		and L.lspg = 'IH'
		-- Originalmente disponibles
		and (G.guOriginAvail = 1 or G.guInvit = 1)
	group by Year(S.saProcD), Month(S.saProcD), G.guag, S.sals

return
end