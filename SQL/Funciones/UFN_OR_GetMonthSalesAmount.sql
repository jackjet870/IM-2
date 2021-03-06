if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetMonthSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetMonthSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por mes
** 
** [wtorres]	07/Oct/2009 Created
** [wtorres]	26/Oct/2009 Modified. Ahora toma el mercado del campo gumk de la tabla Guests
** [wtorres]	10/May/2010 Modified. Renombre la funcion. Ahora acepta varios Lead Sources
** [wtorres]	23/Nov/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Modified. Agregue el parametro @Contracts. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [wtorres] 	19/Feb/2015 Modified. Ahora para el parametro @Contracts se hace siempre la busqueda like
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetMonthSalesAmount](
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
	SalesAmount int
)
as
begin

declare	@Contract varchar(20)

-- declaramos el cursor
declare curContracts cursor for
select item from split(@Contracts, ',')

-- abrimos el cursor
open curContracts

-- buscamos el primer registro
fetch next from curContracts into @Contract

-- mientras haya mas registros
while @@fetch_status = 0 begin

	-- agregamos los registros del contrato
	
	-- si no se debe basar en la fecha de llegada
	if @BasedOnArrival = 0

		insert @Table
		select
			Year(S.saProcD),
			Month(S.saProcD),
			Sum(S.saGrossAmount)
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
			and (saCancel = 0 or saCancelD not between @DateFrom and @DateTo)
			-- Lead Sources
			and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
			-- Mercado
			and (@Market = 'ALL' or G.gumk = @Market)
			-- Numero de noches
			and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
			-- Agencia
			and (@Agency = 'ALL' or G.guag like @Agency)
			-- Contrato
			and (@Contracts = 'ALL' or guO1 like @Contract)
			-- Invitaciones externas
			and (@External = 0
				or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
				or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
		group by Year(S.saProcD), Month(S.saProcD)

	-- si se debe basar en la fecha de llegada
	else

		insert @Table

		-- Ventas con huesped
		select
			Year(G.guCheckInD),
			Month(G.guCheckInD),
			Sum(S.saGrossAmount)
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
			and (saCancel = 0 or saCancelD not between @DateFrom and @DateTo)
			-- Lead Sources
			and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
			-- Mercado
			and (@Market = 'ALL' or G.gumk = @Market)
			-- Numero de noches
			and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
			-- Agencia
			and (@Agency = 'ALL' or G.guag like @Agency)
			-- Contrato
			and (@Contracts = 'ALL' or guO1 like @Contract)
			-- Invitaciones externas
			and (@External = 0
				or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
				or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
		group by Year(G.guCheckInD), Month(G.guCheckInD)

		-- Ventas sin huesped
		union all
		select
			Year(S.saProcD),
			Month(S.saProcD),
			Sum(S.saGrossAmount)
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
			and (saCancel = 0 or saCancelD not between @DateFrom and @DateTo)
			-- Lead Sources
			and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ',')))
			-- Mercado
			and (@Market = 'ALL' or G.gumk = @Market)
			-- Numero de noches
			and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
			-- Agencia
			and (@Agency = 'ALL' or G.guag like @Agency)
			-- Contrato
			and (@Contracts = 'ALL' or guO1 like @Contract)
			-- Invitaciones externas
			and (@External = 0
				or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
				or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
		group by Year(S.saProcD), Month(S.saProcD)

	-- buscamos el siguiente registro
	fetch next from curContracts into @Contract
end

-- cerramos y liberamos el cursor
close curContracts
deallocate curContracts

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

