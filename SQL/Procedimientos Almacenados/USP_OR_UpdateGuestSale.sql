if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGuestSale]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGuestSale]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Marca o desmarca a un huesped como si tuviera venta
**
** [wtorres]	07/Ago/2015	Created
**
*/
create procedure [dbo].[USP_OR_UpdateGuestSale]
    @GuestID int,
    @Sale bit
as

update Guests set guSale = @Sale where guID = @GuestID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

