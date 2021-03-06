if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgencyMarketSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgencyMarketSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por agencia y mercado
** 
** [wtorres]	23/Oct/2009 Created
** [wtorres]	26/Oct/2009 Modified. Ahora toma el mercado del campo gumk de la tabla Guests
** [wtorres]	15/Oct/2010 Modified. Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [wtorres]	04/Jun/2015 Modified. Agregue el parametro @External
**
*/
create function [dbo].[UFN_OR_GetAgencyMarketSales](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@ConsiderNights bit = 0,	-- Indica si se debe considerar el numero de noches
	@NightsFrom int = 0,		-- Numero de noches desde
	@NightsTo int = 0,			-- Numero de noches hasta
	@OnlyQuinellas bit = 0,		-- Indica si se desean solo las quinielas
	@External int = 0,			-- Filtro de invitaciones externas
								--		0. Sin filtro
								--		1. Excluir invitaciones externas
								--		2. Solo invitaciones externas
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Agency varchar(35),
	Market varchar(10),
	Sales int
)
as
begin

insert @Table
select
	G.guag,
	G.gumk,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on G.guID = S.sagu
	left join LeadSources L on L.lsID = S.sals
where
	-- Fecha de procesable
	(((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Numero de noches
	and (@ConsiderNights = 0 or DateDiff(Day, G.guCheckInD, G.guCheckOutD) between @NightsFrom and @NightsTo)
	-- Quinielas
	and (@OnlyQuinellas = 0 or G.guQuinella = 1)
	-- Invitaciones externas
	and (@External = 0
		or (@External = 1 and (L.lspg = 'OUT' or (L.lspg = 'IH' and G.guHReservID is not null)))
		or (@External = 2 and L.lspg = 'IH' and G.guHReservID is null))
group by G.guag, G.gumk

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

