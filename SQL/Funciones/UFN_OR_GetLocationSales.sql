if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas por locacion
** 
** [wtorres]	28/Sep/2009 Creado
** [wtorres]	16/Nov/2013 Agregue el parametro @MembershipGroups
** 							Ahora los downgrades se cuentan en base a la categoria de tipo de venta
** [gmaya]		25/07/2014 Se modifico el parametro @SalesRoom a varchar(8000)  = 'ALL'
** [gmaya]		25/07/2014 Se agrego (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ',')))
**
*/
create function [dbo].[UFN_OR_GetLocationSales](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@SalesRoom varchar(8000)= 'ALL',					-- Clave de la sala de ventas
	@MembershipGroups varchar(8000)= 'ALL'	-- Claves de grupos de tipos de membresia
)
returns @Table table (
	Location varchar(10),
	Sales int
)
as
begin

insert @Table
select
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
group by S.salo

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

