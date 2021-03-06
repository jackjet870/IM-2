if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftQuantityQuantitysAmount]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftQuantityQuantitysAmount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el total de cantidades y monto por regalo y cantidad
** 
** [wtorres]	02/Dic/2009 Creado
**
*/
create function [dbo].[UFN_OR_GetGiftQuantityQuantitysAmount](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@GiftsQuantitys varchar(8000)	-- Lista de cantidades y regalos
)
returns @Table table (
	Gift varchar(10),
	PR varchar(10),
	Quantity int,
	Amount money
)
as
begin

insert @Table
select
	D.gegi,
	R.grpe,
	Sum(D.geQty),
	Sum(IsNull(D.gePriceA + D.gePriceM, 0))
from Guests G
	inner join GiftsReceipts R on G.guID = R.grgu
	inner join GiftsReceiptsC D on R.grID = D.gegr
	inner join (select Gift, Quantity from UFN_OR_SplitGiftsQuantitys(@GiftsQuantitys)) Q on D.gegi = Q.Gift and D.geQty >= Q.Quantity
where
	-- Fecha del recibo
	R.grD between @DateFrom and @DateTo
	-- Lead Source
	and R.grls in (select item from split(@LeadSources, ','))
	-- No recibos cancelados
	and R.grCancel = 0
	-- Sin depósitos quemados
	and R.grDepositTwisted = 0
group by D.gegi, R.grpe

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

