if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ObtenerMembresiasPorSalaFecha]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ObtenerMembresiasPorSalaFecha]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las membresias que se vendieron en una sala en un rango de fechas determinado
** 
** [wtorres]	21/Nov/2008 Creado
**
*/
create procedure [dbo].[USP_OR_ObtenerMembresiasPorSalaFecha]
	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final
	@SalesRoom varchar(10)	-- Clave de la sala
as
set nocount on

declare @Table table(
	MembershipNum varchar(10)
)
-- Manifiesto
insert into @Table
select
	saMembershipNum
	from Guests
	left outer join Sales on guID = sagu
	where guShowD between @DateFrom and @DateTo and gusr = @SalesRoom
		and saProc = 1
		-- No se toman en cuenta las ventas pendientes
		and (guShowD = saProcD or (guShowD <> saProcD and saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas Bump ni Regen
		and sast <> 'BUMP' and sast <> 'REGEN'
		-- No se toman en cuenta las ventas canceladas
		and saCancel <> 1

-- Ventas con show de otro dia
insert into @Table
select
	saMembershipNum
	from Guests
	left outer join Sales on guID = sagu
	where 
		-- Ventas que no tienen Show o que su show es de otro dia
		( (guShowD is null or not guShowD between @DateFrom and @DateTo)
		-- Ventas del dia  y ventas procesables del dia (no se toman en cuenta las canceladas)
		and (saD between @DateFrom and @DateTo or saProcD between @DateFrom and @DateTo)
		-- Bumps, Regen y las ventas de otra sala
		or ( (sast = 'BUMP' or sast = 'REGEN' or gusr <> sasr) and saD between @DateFrom and @DateTo) )
		-- Ventas de la sala
		and sasr = @SalesRoom
		-- No se toman en cuenta los Deposit Before
		and not (sast = 'NS' and guDepSale > 0 and not (saProc = 1 and saD <> saProcD and saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas pendientes
		and not (saProcD > @DateTo)

-- devolvemos la tabla 
select * from @Table

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

