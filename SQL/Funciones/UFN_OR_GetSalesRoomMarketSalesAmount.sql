if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomMarketSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomMarketSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por sala de ventas y mercado
** 
** [wtorres]	11/Jun/2011 Creado
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetSalesRoomMarketSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	SalesRoom varchar(10),
	Market varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	S.sasr,
	G.gumk,
	Sum(S.saGrossAmount)
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
	-- Fecha de cancelacion
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ',')))
group by S.sasr, G.gumk

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

