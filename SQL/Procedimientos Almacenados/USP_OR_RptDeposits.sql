if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptDeposits]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptDeposits]
GO

set QUOTED_IDENTIFIER on 
GO
set ANSI_NULLS on 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de depositos
**		1. Recibos de regalos
**		2. Monedas
**		3. Tipos de pago
** 
** [wtorres]	07/May/2009 Ahora se pasa la lista de salas como un solo parametro
**							Ahora se devuelve la descripcion de las monedas
** [wtorres]	10/Jul/2013 Renombrado de sprptPreManifDepsBySRSimple a USP_OR_RptDeposits
** [alesanchez]	03/Oct/2013 Agregue el manejo de tipos de pago
** [lchairez]	08/Nov/2013 Agregue la columna de show
*/
create procedure [dbo].[USP_OR_RptDeposits]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@SalesRooms varchar(8000)	-- Claves de las salas de ventas
as
set nocount on

-- Recibos de regalos
-- =============================================
select 
	grID,
	grNum,
	guOutInvitNum,
	grgu, 
	grD,
	guBookD,
	guShow,
	grGuest, 
	grHotel, 
	grls, 
	grsr,
	grpe, 
	peN,
	grpt,
	grcu, 
	grDepositTwisted,
	grDeposit, 
	IsNull(guDepositReceived, 0) as guDepositReceived, 
	grDeposit - IsNull(guDepositReceived, 0) as grDepositCxC,
	grCxCPRDeposit,
	grcuCxCPRDeposit
into #GiftsReceipts
from GiftsReceipts
	left join Guests on grgu = guID
	left join Personnel on grpe = peID 
where 
	-- Sala de ventas
	grsr in (select item from split(@SalesRooms, ','))
	-- Fecha del recibo
	and grD between @DateFrom and @DateTo 
	-- Con deposito
	and (grDeposit > 0 or grDepositTwisted > 0)
	-- No cancelados
	and grCancel = 0
order by grID, grgu

-- actualizamos los depositos
-- =============================================
declare
	@grgu int,						-- Clave del huesped
	@grID int,						-- Clave del recibo de regalos
	@grcu varchar(10),				-- Clave de la moneda
	@grCxCPRDeposit money,			-- Monto de deposito de la CxC del PR
	@grcuCxCPRDeposit varchar(10),	-- Clave de la moneda del deposito de la CxC del PR
	@GiftReceiptID int,				-- Clave del recibo de regalos (auxiliar)
	@GuestID int,					-- Clave de la invitacion (auxiliar)
	@bdcu varchar(10),				-- Clave de la moneda del deposito
	@bdAmount money,				-- Monto del deposito
	@bdReceived money,				-- Monto del deposito recibido
	@grpt varchar(10)				-- Forma de pago en un recibo recibo de regalos 

-- cursor de recibos de regalos
declare csGiftsReceipts cursor for
select grgu, grID, grcu, grCxCPRDeposit, grcuCxCPRDeposit,grpt from #GiftsReceipts

-- abrimos el cursor de recibos de regalos
open csGiftsReceipts

-- buscamos el primer recibo de regalos
fetch next from csGiftsReceipts into @grgu, @grID, @grcu, @grCxCPRDeposit, @grcuCxCPRDeposit, @grpt

-- mientras haya mas recibos de regalos
while @@fetch_status = 0 begin
	set @bdReceived = 0

	-- obtenemos el monto del deposito recibido
	select @bdReceived = bdReceived from BookingDeposits where bdgu = @grgu and bdcu = @grcu and bdpt = @grpt 
	
	-- actualizamos el deposito recibido y el deposito de CxC
	if @bdReceived > 0
		update #GiftsReceipts set guDepositReceived = @bdReceived, grDepositCxC = 0 where current of csGiftsReceipts
	else
		update #GiftsReceipts set guDepositReceived = 0, grDepositCxC = 0 where current of csGiftsReceipts				

	-- actualizamos el deposito de CxC y el deposito de la CxC del PR, siempre que las monedas de depósito del recibo y CXC del PR coincidan
	if @grcu = @grcuCxCPRDeposit
		update #GiftsReceipts set grDepositCxC = @grCxCPRDeposit, grCxCPRDeposit = 0 where current of csGiftsReceipts				
	
	-- buscamos el siguiente recibo de regalos
	fetch next from csGiftsReceipts into @grgu, @grID, @grcu, @grCxCPRDeposit, @grcuCxCPRDeposit, @grpt
end

-- cerramos y liberamos el cursor de recibos de regalos
close csGiftsReceipts
deallocate csGiftsReceipts

-- se anexan los registros faltantes de otras monedas y formas de pago
-- =============================================
insert into #GiftsReceipts
select grID, grNum, guOutInvitNum, grgu, grD, guBookD, guShow, grGuest, grHotel,
	   grls, grsr, grpe, peN, bdpt, bdcu, 0, 0, bdReceived, 0, 0, 'US'
from #GiftsReceipts
	inner join BookingDeposits on grgu = bdgu 
where (grcu <> bdcu) and (grpt <> bdpt) and (bdReceived > 0)
order by grID, grgu

-- se anexan los registros faltantes de la misma moneda y diferente Forma de Pago
-- =============================================
insert into #GiftsReceipts
select grID, grNum, guOutInvitNum, grgu, grD, guBookD, guShow, grGuest, grHotel,
	   grls, grsr, grpe, peN, bdpt, grcu, 0, 0, bdReceived, 0, 0, 'US'
from #GiftsReceipts
	inner join BookingDeposits on grgu = bdgu 
where (grcu = bdcu) and (grpt <> bdpt) and (bdReceived > 0)
order by grID, grgu

-- se anexan registros de CxC que esten en otra moneda a la Moneda de Depósitos de la Host
-- =============================================
insert into #GiftsReceipts
select grID, grNum, guOutInvitNum, grgu, grD, guBookD, guShow, grGuest, grHotel,
	   grls, grsr, grpe, peN, grpt, grcuCxCPRDeposit, 0, 0, 0, grCxCPRDeposit, 0, grcuCxCPRDeposit
from #GiftsReceipts 
where (grcu <> grcuCxCPRDeposit) and (grCxCPRDeposit > 0)
order by grID, grgu

-- actualizamos los depositos recibidos
-- =============================================
set @GiftReceiptID = 0
set @GuestID = 0

-- cursor de recibos de regalos
declare csGiftsReceipts cursor for
select grgu, grID from #GiftsReceipts

-- abrimos el cursor de recibos de regalos
open  csGiftsReceipts

-- buscamos el primer recibo de regalos
fetch next from csGiftsReceipts into @grgu, @grID

-- mientras haya mas recibos de regalos
while @@fetch_status = 0 begin

	-- si es un nuevo recibo de regalos
	if @GiftReceiptID <> @grID
	begin
		set @GiftReceiptID = @grID

		-- si no es una nueva invitacion
		if @GuestID = @grgu
			set @GuestID = 0
		else
			set @GuestID = @grgu
	end

	-- si no es una nueva invitacion
	-- evitamos que se repita lo recibido en otro registro, cuando es el mismo guest
	if @GuestID = 0
		update #GiftsReceipts set guDepositReceived = 0 where current of csGiftsReceipts

	-- buscamos el siguiente recibo de regalos
	fetch next from csGiftsReceipts into @grgu, @grID
end

-- cerramos y liberamos el cursor de recibos de regalos
close csGiftsReceipts
deallocate csGiftsReceipts

-- 1. Recibos de regalos
-- =============================================
select 
	grID,
	grNum,
	guOutInvitNum,
	grgu, 
	grD,
	guBookD,
	guShow ,
	grGuest, 
	grHotel, 
	grls, 
	grsr,
	grpe, 
	peN,
	grpt,	
	grcu, 
	grDepositTwisted,
	grDeposit, 
	guDepositReceived, 
	grDepositCxC
from #GiftsReceipts
order by grID, grgu

-- 2. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #GiftsReceipts R
	left join Currencies C on C.cuID = R.grcu

-- 3. Tipos de pago
-- =============================================
select distinct P.ptID, P.ptN
from #GiftsReceipts R
	left join PaymentTypes P on P.ptID = R.grpt

GO
set QUOTED_IDENTIFIER OFF 
GO
set ANSI_NULLS on 
GO

