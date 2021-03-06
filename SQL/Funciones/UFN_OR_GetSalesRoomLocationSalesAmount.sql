if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomLocationSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomLocationSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por locacion y sales room
** 
** [caduran]	29/Sep/2014 Creado, basado en UFN_OR_GetLocationSalesAmount del 25/07/2014
**
*/
create function [dbo].[UFN_OR_GetSalesRoomLocationSalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@SalesRoom varchar(8000)= 'ALL',			-- Clave de la sala de ventas
	@ConsiderOutOfPending bit = 0	-- Indica si se debe considerar Out Of Pending
)
returns @Table table (
	SalesRoom varchar(10),
	Location varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	S.sasr as SalesRoom,
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
group by S.sasr, S.salo

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

