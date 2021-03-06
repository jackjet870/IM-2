if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSalesRoomLocationSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSalesRoomLocationSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por locacion y sales room
** 
** [caduran]	29/Sep/2014 Creado, basado en UFN_OR_GetLocationSales del 25/07/2014
**
*/
create function [dbo].[UFN_OR_GetSalesRoomLocationSales](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@SalesRoom varchar(8000)= 'ALL',					-- Clave de la sala de ventas
	@MembershipGroups varchar(8000)= 'ALL'	-- Claves de grupos de tipos de membresia
)
returns @Table table (
	SalesRoom varchar(10),
	Location varchar(10),	
	Sales int
)
as
begin

insert @Table
select	
	S.sasr,
	S.salo,
	Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join MembershipTypes M on M.mtID = S.samt
where
	-- Sala de ventas
	(@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
	-- Fecha de procesable
	and S.saProcD between @DateFrom and @DateTo
	-- No downgrades
	and ST.ststc <> 'DG'
	-- No canceladas o canceladas fuera del periodo
	and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo)
	-- Grupos de tipos de membresia
	and (@MembershipGroups = 'ALL' or M.mtGroup in (select * from split(@MembershipGroups, ',')))
group by S.sasr, S.salo

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

