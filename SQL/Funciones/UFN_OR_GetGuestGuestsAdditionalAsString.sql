if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestGuestsAdditionalAsString]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestGuestsAdditionalAsString]
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
create function [dbo].[UFN_OR_GetGuestGuestsAdditionalAsString](
	@GuestID int	-- Clave del huesped
)
returns varchar(8000)
as
begin

declare
	@Guests varchar(8000),	-- Lista de huespedes adicionales
	@Guest varchar(8000)	-- Huesped adicional

-- declaramos el cursor
declare curGuests cursor for
select dbo.AddString(dbo.UFN_OR_GetFullName(guLastName1, guFirstName1), dbo.UFN_OR_GetFullName(guLastName2, guFirstName2), ' / ')
from GuestsAdditional
	inner join Guests on gaAdditional = guID
where gagu = @GuestID
for read only

-- abrimos el cursor
open curGuests

-- buscamos el primer registro
fetch next from curGuests into @Guest

-- mientras haya mas registros
while @@fetch_status = 0
begin
	set @Guests = dbo.AddString(@Guests, @Guest, ', ')

	-- buscamos el siguiente registro
	fetch next from curGuests into @Guest
end

-- cerramos y liberamos el cursor
close curGuests
deallocate curGuests

return @Guests
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

