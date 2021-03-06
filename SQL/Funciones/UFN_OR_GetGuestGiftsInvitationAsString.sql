if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestGiftsInvitationAsString]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestGiftsInvitationAsString]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los regalos prometidos de la invitacion de un huesped como una cadena
** 
** [wtorres]	04/Jun/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetGuestGiftsInvitationAsString](
	@GuestID int	-- Clave del huesped
)
returns varchar(8000)
as
begin

declare
	@Gifts varchar(8000),	-- Lista de regalos
	@Gift varchar(8000)		-- Regalo

-- declaramos el cursor
declare curGifts cursor for
select Cast(Sum(igQty) as varchar(4)) + ' - ' + Gifts.giN
from (

	-- Regalos del huésped principal
	select iggi, igQty
	from InvitsGifts
	where iggu = @GuestID
		and igQty > 0 and (iggi is not null and iggi <> '')

	-- Regalos de los huéspedes adicionales
	union all
	select iggi, igQty
	from InvitsGifts
		inner join Gifts on iggi = giID
		inner join GuestsAdditional on iggu = gaAdditional
	where iggu in (select gaAdditional from GuestsAdditional where gagu = @GuestID)
		and igQty > 0 and (iggi is not null and iggi <> '')
) as D
	inner join Gifts on iggi = giID
group by giN
for read only

-- abrimos el cursor
open curGifts

-- buscamos el primer registro
fetch next from curGifts into @Gift

-- mientras haya mas registros
while @@fetch_status = 0
begin
	set @Gifts = dbo.AddString(@Gifts, @Gift, ', ')

	-- buscamos el siguiente registro
	fetch next from curGifts into @Gift
end

-- cerramos y liberamos el cursor
close curGifts
deallocate curGifts

return @Gifts
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

