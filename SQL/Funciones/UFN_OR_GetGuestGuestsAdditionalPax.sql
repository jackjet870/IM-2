if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestGuestsAdditionalPax]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestGuestsAdditionalPax]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los nombres de los huespedes adicionales de la invitacion de un huesped como una cadena
** 
** [wtorres]	04/Jun/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetGuestGuestsAdditionalPax](
	@GuestID int	-- Clave del huesped
)
returns decimal(4, 1)
as
begin

declare
	@TotalPax decimal(4, 1),	-- Número de personas de todos los huéspedes adicionales de un huésped
	@Pax decimal(4, 1)			-- Número de personas de cada huésped adicional de un huésped

set @TotalPax = 0

-- declaramos el cursor
declare curGuests cursor for
select guPax
from GuestsAdditional
	inner join Guests on gaAdditional = guID
where gagu = @GuestID
for read only

-- abrimos el cursor
open curGuests

-- buscamos el primer registro
fetch next from curGuests into @Pax

-- mientras haya mas registros
while @@fetch_status = 0
begin
	set @TotalPax = @TotalPax + @Pax

	-- buscamos el siguiente registro
	fetch next from curGuests into @Pax
end

-- cerramos y liberamos el cursor
close curGuests
deallocate curGuests

return @TotalPax
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

