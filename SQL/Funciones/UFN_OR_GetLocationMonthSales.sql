if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationMonthSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationMonthSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por locacion y mes
** 
** [wtorres]	22/Oct/2009 Creado
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetLocationMonthSales](
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
)
returns @Table table (
	Location varchar(10),
	[Year] int,
	[Month] int,
	Sales int
)
as
begin

insert @Table
select
	S.salo,
	Year(S.saProcD),
	Month(S.saProcD),
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Sala de ventas
	S.sasr = @SalesRoom
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
group by S.salo, Year(S.saProcD), Month(S.saProcD)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

