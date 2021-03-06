if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetGuestDepositsAsString]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetGuestDepositsAsString]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los depositos de un huesped como una cadena
** 
** [wtorres]	03/Jun/2011 Creado
**
*/
create function [dbo].[UFN_OR_GetGuestDepositsAsString](
	@GuestID int	-- Clave del huesped
)
returns varchar(8000)
as
begin

declare
	@Deposits varchar(8000),	-- Lista de depositos
	@Deposit varchar(8000)		-- Deposito

-- declaramos el cursor
declare curDeposits cursor for
select dbo.FormatNumber(bdAmount, 0) + '/' + dbo.FormatNumber(bdReceived, 0) + ' ' +  bdcu
from BookingDeposits
where bdgu = @GuestID
for read only

-- abrimos el cursor
open curDeposits

-- buscamos el primer registro
fetch next from curDeposits into @Deposit

-- mientras haya mas registros
while @@fetch_status = 0
begin
	set @Deposits = dbo.AddString(@Deposits, @Deposit, ', ')

	-- buscamos el siguiente registro
	fetch next from curDeposits into @Deposit
end

-- cerramos y liberamos el cursor
close curDeposits
deallocate curDeposits

return @Deposits
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

