if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetContractAgencySalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetContractAgencySalesAmount]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por contrato y agencia
** 
** [galcocer]	04/Feb/2012 Created
** [wtorres]	19/Nov/2013 Modified. Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
create function [dbo].[UFN_OR_GetContractAgencySalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',	-- Claves de mercados
	@Agencies varchar(max) = 'ALL',	-- Claves de agencias
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Contract varchar(20),
	Agency varchar(35),
	SalesAmount money
)
as
begin

insert @Table
select
	G.guO1,
	G.guag,
	Sum(saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
where
	-- Fecha de procesable
	((((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Lead Source
	and S.sals in (select item from split(@LeadSources, ','))
	-- Mercado
	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
	-- Agencia
	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))
group by G.guO1, G.guag

return
end