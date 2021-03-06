if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftQuantitySalesAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftQuantitySalesAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de ventas por regalo y cantidad
** 
** [wtorres]	26/Nov/2009 Creado
** [wtorres]	02/Dic/2009 Elimine los campos de cantidad y monto. No se deben considerar los recibos cancelados ni con depositos quemados
** [wtorres]	17/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Nov/2013 Ahora los downgrades se cuentan en base a la categoria de tipo de venta
**
*/
create function [dbo].[UFN_OR_GetGiftQuantitySalesAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@GiftsQuantitys varchar(8000),	-- Lista de cantidades y regalos
	@ConsiderSelfGen bit = 0,		-- Indica si se debe considerar Self Gen
	@ConsiderOutOfPending bit = 0,	-- Indica si se debe considerar Out Of Pending
	@ConsiderCancel bit = 0,		-- Indica si se debe considerar canceladas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Gift varchar(10),
	PR varchar(10),
	SalesAmount money
)
as
begin

insert @Table
select
	D.gegi,
	R.grpe,
	Sum(S.saGrossAmount)
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
	left join Guests G on S.sagu = G.guID
	inner join (select item from split(@LeadSources, ',')) LS on LS.item = S.sals
	inner join GiftsReceipts R on S.sagu = R.grgu
	inner join GiftsReceiptsC D on R.grID = D.gegr
	inner join (select Gift, Quantity from UFN_OR_SplitGiftsQuantitys(@GiftsQuantitys)) Q on D.gegi = Q.Gift and D.geQty >= Q.Quantity
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
	or (@ConsiderCancel = 0 and ST.ststc = 'DG' and S.saProcRD between @DateFrom and @DateTo)))
	-- Fecha de cancelacion
	and ((@ConsiderCancel = 0 and (S.saCancel = 0 or S.saCancelD not between @DateFrom and @DateTo))
		or (@ConsiderCancel = 1 and S.saCancelD between @DateFrom and @DateTo))
	-- Self Gen
	and (@ConsiderSelfGen = 0 or S.saSelfGen = 1)
	-- Out Of Pending
	and (@ConsiderOutOfPending = 0 or S.saD <> S.saProcD)
	-- No recibos cancelados
	and R.grCancel = 0
	-- Sin depositos quemados
	and R.grDepositTwisted = 0
group by D.gegi, R.grpe

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

