if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGuestsAdditional]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGuestsAdditional]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar huéspedes adicionales
-- Descripción:		Actualiza los huéspedes adicionales de un huésped
--						1. Los marca como no disponibles
--						2. Los marca como contactados
--						3. Los marca como que se les haya dado seguimiento
-- Histórico:		[wtorres] 24/Jun/2010 Creado
--					[wtorres] 29/Jul/2010 Ahora no marca como que se le dió seguimiento si el huésped principal ya estaba invitado
-- =============================================
create procedure [dbo].[USP_OR_UpdateGuestsAdditional]
	@Guest int,				-- Clave del huésped principal	
	@PR varchar(10),		-- Clave del PR
	@Date datetime,			-- Fecha de contacto
	@Location varchar(10),	-- Clave de la locación de contacto
	@Invited bit			-- Indica si el huésped principal está invitado
as
set nocount on

declare
	@GuestAdditional int,	-- Clave del huésped adicional
	@Available bit,			-- Indica si el huésped adicional está disponible
	@Contact bit,			-- Indica si el huésped adicional está contactado
	@Follow bit				-- Indica si el huésped adicional se le ha dado seguimiento

-- Declara el cursor
declare csGuestsAdditional cursor for
select A.gaAdditional, G.guAvail, G.guInfo, G.guFollow
from GuestsAdditional A
	inner join Guests G on A.gaAdditional = G.guID
where A.gagu = @Guest

-- Abre el cursor
open csGuestsAdditional

-- Busca el primer registro
fetch next from csGuestsAdditional into @GuestAdditional, @Available, @Contact, @Follow

-- Mietras haya más registros
while @@fetch_status = 0
begin

	-- Si está disponible
	if @Available = 1

		-- Marca el huésped adicional como no disponible
		update Guests
		set guAvail = 0,
			guPRAvail = @PR,
			guum = 28 -- SHOW
		where guID = @GuestAdditional

	-- Si no está contactado
	if @Contact = 0

		-- Marca el huésped adicional como contactado
		update Guests
		set guInfo = 1,
			guInfoD = @Date,
			guPRInfo = @PR,
			guloInfo = @Location		
		where guID = @GuestAdditional

	-- Si no se le ha dado seguimiento, está contactado y el huésped principal no estaba invitado
	if @Follow = 0 and @Contact = 1 and @Invited = 0

		-- Marca el huésped adicional como que se le haya dado seguimiento
		update Guests
		set guFollow = 1,
			guFollowD = @Date,
			guPRFollow = @PR
		where guID = @GuestAdditional

	-- Busca el siguiente registro
	fetch next from csGuestsAdditional into @GuestAdditional, @Available, @Contact, @Follow
end

-- Cierra y libera el cursor
close csGuestsAdditional
deallocate csGuestsAdditional

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

