if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetShowProgramSalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetShowProgramSalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por programa de show
** 
** [wtorres]	05/Ago/2011 Creado
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetShowProgramSalesAmount](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de salas de ventas
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	ShowProgram varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	D.ShowProgram,
	Sum(D.SalesAmount)
from (
	select
		dbo.UFN_OR_GetShowProgram(G.guSaveProgram, G.guCTour, G.guInOut, SR.srAppointment) as ShowProgram,
		S.saGrossAmount as SalesAmount
	from Sales S
		left join SaleTypes ST on ST.stID = S.sast
		left join Guests G on S.sagu = G.guID
		inner join SalesRooms SR on S.sasr = SR.srID
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
) as D
group by D.ShowProgram

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

