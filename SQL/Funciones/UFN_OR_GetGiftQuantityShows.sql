if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftQuantityShows]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftQuantityShows]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de shows por regalo y cantidad
** 
** [wtorres]	26/Nov/2009 Creado
** [wtorres]	02/Dic/2009 Elimine los campos de cantidad y monto. No se deben considerar los recibos cancelados ni con depositos quemados
** [wtorres]	27/Abr/2010 Agregue el parametro @ConsiderDirectsAntesInOut
** [wtorres]	17/Nov/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	19/Oct/2011 Agregue el parametro @InOut
**
*/
create function [dbo].[UFN_OR_GetGiftQuantityShows](
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Clave de los Lead Sources
	@GiftsQuantitys varchar(8000),		-- Lista de cantidades y regalos
	@ConsiderQuinellas bit = 0,			-- Indica si se debe considerar quinielas
	@InOut int = -1,					-- Filtro de In & Outs:
										--		-1. Sin filtro
										--		 0. No In & Outs
										--		 1. In & Outs
	@ConsiderDirectsAntesInOut bit = 0,	-- Indica si se debe considerar directas Antes In & Out
	@BasedOnArrival bit = 0				-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Gift varchar(10),
	PR varchar(10),
	Shows int
)
as
begin

insert @Table
select
	D.gegi,
	R.grpe,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guShowsQty end)
from Guests G
	inner join (select item from split(@LeadSources, ',')) LS on LS.item = G.guls
	inner join GiftsReceipts R on G.guID = R.grgu
	inner join GiftsReceiptsC D on R.grID = D.gegr
	inner join (select Gift, Quantity from UFN_OR_SplitGiftsQuantitys(@GiftsQuantitys)) Q on D.gegi = Q.Gift and D.geQty >= Q.Quantity
where
	-- Fecha de show
	((@BasedOnArrival = 0 and G.guShowD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Con show
	and G.guShow = 1))
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No recibos cancelados
	and R.grCancel = 0
	-- Sin depositos quemados
	and R.grDepositTwisted = 0
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No Directas no Antes In & Out
	and (@ConsiderDirectsAntesInOut = 0 or not (guDirect = 1 and guAntesIO = 0))
group by D.gegi, R.grpe

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

