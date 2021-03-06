if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetSales]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de ventas
** 
** [wtorres]	23/Nov/2009 Creado
** [wtorres]	24/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	16/Nov/2013 Agregue los parametros @SalesRooms, @MembershipGroups, @ConsiderOutOfPending y @ConsiderCancel
** 							Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetSales](
	@DateFrom datetime,						-- Fecha desde
	@DateTo datetime,						-- Fecha hasta
	@LeadSources varchar(8000) = 'ALL',		-- Claves de Lead Sources
	@SalesRooms varchar(8000) = 'ALL',		-- Claves de salas de ventas
	@MembershipGroups varchar(8000)= 'ALL',	-- Claves de grupos de tipos de membresia
	@ConsiderOutOfPending bit = 0,			-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,				-- Indica si se debe considerar canceladas
	@BasedOnArrival bit = 0					-- Indica si se debe basar en la fecha de llegada
)
returns int
as
begin

declare @Result int

select @Result = Count(*)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on G.guID = S.sagu
	left join MembershipTypes M on M.mtID = S.samt
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
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- Lead Sources
	and (@LeadSources = 'ALL' or S.sals in (select item from split(@LeadSources, ','))) 
	-- Salas de ventas
	and (@SalesRooms = 'ALL' or S.sasr in (select item from split(@SalesRooms, ','))) 
	-- Grupos de tipos de membresia
	and (@MembershipGroups = 'ALL' or M.mtGroup in (select * from split(@MembershipGroups, ',')))

return @Result
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

