if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationMonthSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationMonthSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por locacion y mes
** 
** [wtorres]	22/Oct/2009 Creado
** [wtorres]	24/Nov/2009 Agregue el parametro @ConsiderCancel
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetLocationMonthSalesAmount](
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10),	-- Clave de la sala de ventas
	@ConsiderCancel bit = 0	-- Indica si se debe considerar canceladas
)
returns @Table table (
	Location varchar(10),
	[Year] int,
	[Month] int,
	SalesAmount money
)
as
begin

insert @Table
select
	S.salo as Location,
	Year(S.saProcD),
	Month(S.saProcD),
	Sum(S.saGrossAmount) as SalesAmount
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Sala de ventas
	S.sasr = @SalesRoom
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
group by S.salo, Year(S.saProcD), Month(S.saProcD)

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

