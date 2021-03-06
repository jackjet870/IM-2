if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGiftQuantityBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGiftQuantityBookings]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el número de bookings por regalo y cantidad
** 
** [wtorres]	26/Nov/2009 Creado
** [wtorres]	02/Dic/2009 Eliminé los campos de cantidad y monto. No se deben considerar los recibos cancelados ni con depósitos quemados
** [wtorres]	27/Abr/2010 Agregué el parámetro @ConsiderDirects
** [wtorres]	17/Nov/2010 Agregué el parámetro @BasedOnArrival y reemplacé el parámetro @ConsiderDirects por @Direct
** [wtorres]	26/Nov/2010 Ahora no se cuentan los bookings cancelados
**
*/
create function [dbo].[UFN_OR_GetGiftQuantityBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(8000),		-- Clave de los Lead Sources
	@GiftsQuantitys varchar(8000),	-- Lista de cantidades y regalos
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@BasedOnArrival bit = 0			-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Gift varchar(10),
	PR varchar(10),
	Books int
)
as
begin

insert @Table
select
	D.gegi,
	R.grpe,
	Sum(case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join (select item from split(@LeadSources, ',')) LS on LS.item = G.guls
	inner join GiftsReceipts R on G.guID = R.grgu
	inner join GiftsReceiptsC D on R.grID = D.gegr
	inner join (select Gift, Quantity from UFN_OR_SplitGiftsQuantitys(@GiftsQuantitys)) Q on D.gegi = Q.Gift and D.geQty >= Q.Quantity
where
	-- Fechas de booking
	((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- No recibos cancelados
	and R.grCancel = 0
	-- Sin depósitos quemados
	and R.grDepositTwisted = 0
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- No bookings cancelados
	and G.guBookCanc = 0
group by D.gegi, R.grpe

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

