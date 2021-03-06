if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsReceivedBySR]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsReceivedBySR]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el reporte de regalos recibidos por sala de ventas
**
** [wtorres]	30/Jun/2009 Created
** [wtorres]	07/Jul/2009 Modified. Agregue el campo de clave del regalo
** [wtorres]	17/Sep/2009 Modified. Agregue el filtro por cargar a
** [wtorres]	03/Ago/2011 Modified. Desglose el campo de Pax en Adults y Minors y agregue los campos de Quantity y Couples
** [aalcocer]	10/Jun/2016 Modified. Se modifica el tipo de dato del campo clave del regalo a varchar(max)
**
*/
create procedure [dbo].[USP_OR_RptGiftsReceivedBySR]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@ChargeTo varchar(8000) = 'ALL',	-- Claves de cargar a
	@Gifts varchar(max) = 'ALL'			-- Claves de regalos
as
set nocount on

select
	S.srN as SalesRoom,
	G.giID as Gift,
	G.giN as GiftN,
	R.grcu as Currency,
	Sum(D.geQty) as Quantity,
	Sum(IsNull(GU.guShowsQty, 0)) as Couples,
	Sum(IsNull(Floor(R.grPax), 0)) as Adults,
	Sum(IsNull((R.grPax - Floor(R.grPax)) * 10, 0)) as Minors,
	Sum(IsNull(D.gePriceA + D.gePriceM, 0)) as Amount
into #tblData
from GiftsReceipts R
	inner join GiftsReceiptsC D on R.grID = D.gegr
	inner join SalesRooms S on R.grsr = S.srID
	inner join Gifts G on D.gegi = G.giID
	left join Guests GU on R.grgu = GU.guID
where
	-- Fecha del recibo
	R.grD between @DateFrom and @DateTo
	-- Lead Source
	and R.grls in (select item from split(@LeadSources, ','))
	-- Cargar a
	and (@ChargeTo = 'ALL' or R.grct in (select item from split(@ChargeTo, ',')))
	-- Regalo
	and (@Gifts = 'ALL' or D.gegi in (select item from split(@Gifts, ',')))
	-- No considerar los recibos cancelados
	and R.grCancel = 0
group by S.srN, G.giID, G.giN, R.grcu
order by S.srN, G.giID, R.grcu


-- 1. Regalos recibidos por sala
-- =============================================
select * from #tblData

-- 2. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #tblData D
	left join Currencies C on D.Currency = C.cuID