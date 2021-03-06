if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por locacion
** 
** [wtorres]	25/Sep/2009 Creado
** [wtorres]	28/Oct/2009 Agregue el parametro @ConsiderOutOfPenfing
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [gmaya]		25/07/2014 Se modifico el parametro @SalesRoom a varchar(8000)  = 'ALL'
** [gmaya]		25/07/2014 Se agrego(@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
**
*/
create function [dbo].[UFN_OR_GetLocationSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRoom varchar(8000)= 'ALL',			-- Clave de la sala de ventas
	@ConsiderOutOfPending bit = 0	-- Indica si se debe considerar Out Of Pending
)
returns @Table table (
	Location varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	S.salo as Location,
	Sum(S.saGrossAmount) as SalesAmount
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Sala de ventas
	(@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- No downgrades
	and (ST.ststc <> 'DG'
	-- Downgrades cuya membresia de referencia esta dentro del periodo
	or (ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo))
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
group by S.salo

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

