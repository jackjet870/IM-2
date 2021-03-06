if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGuestAccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGuestAccount]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Actualizar cuenta de invitación
-- Descripción:		Actualiza la cuenta y el número de monederos electrónicos de una invitación
-- Histórico:		[wtorres] 28/Jul/2009 Creado
-- =============================================
create procedure [dbo].[USP_OR_UpdateGuestAccount]
	@GuestID int,				-- Clave de la invitación
	@Account varchar(16),		-- Cuenta
	@QtyElectronicPurses int	-- Cantidad de monederos elctrónicos asociados a la cuenta
as
set nocount on

update Guests
set
	guAccountGiftsCard = @Account,
	guQtyGiftsCard = @QtyElectronicPurses
where guID = @GuestID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

