if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGroupSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGroupSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por grupo
** 
** [wtorres]	21/Jul/2010 Creado
** [wtorres]	17/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetGroupSales](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',	-- Claves de Lead Sources
	@ConsiderSelfGen bit = 0,			-- Indica si se debe considerar Self Gen
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	[Group] int,
	Sales int
)
as
begin

insert @Table
select
	I.gjgx,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	inner join Guests G on S.sagu = G.guID
	inner join GuestsGroupsIntegrants I on I.gjgu = G.guID
where
	-- Fecha de procesable
	(((@BasedOnArrival = 0 or (@BasedOnArrival = 1 and S.sagu is null)) and S.saProcD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1	and S.sagu is not null and G.guCheckInD between @DateFrom and @DateTo
	-- Procesable
	and S.saProc = 1))
	-- No downgrades
	and ST.ststc <> 'DG'
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
group by I.gjgx

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

