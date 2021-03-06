if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_UpdateGuestSalesmen]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_UpdateGuestSalesmen]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Actualiza los vendedores de un huesped en base a los vendedores de una venta
**
** [wtorres]	07/Ago/2015	Modified. Ahora ya no marca al huesped como que tiene ventas. Lo separe en otro procedimiento almacenado
** [wtorres]	20/Oct/2015 Modified. Ahora solo actualiza a los Liners si el huesped no tiene Liner 1.
**							Tambien solo actualiza los Closers y Exit Closers si el huesped no tiene Closer 1.
**
*/
create procedure [dbo].[USP_OR_UpdateGuestSalesmen]
    @GuestID int,
    @SaleID int
as

declare
    @Liner1 varchar(10),
    @Liner2 varchar(10),
    @Closer1 varchar(10),
    @Closer2 varchar(10),
    @Closer3 varchar(10),
    @Exit1 varchar(10),
    @Exit2 varchar(10)

-- obtenemos los vendedores de la venta
select
    @Liner1 = saLiner1,
    @Liner2 = saLiner2,
    @Closer1 = saCloser1,
    @Closer2 = saCloser2,
    @Closer3 = saCloser3,
    @Exit1 = saExit1,
    @Exit2 = saExit2
from Sales where saID = @SaleID

-- Liners
update Guests
set guLiner1 = @Liner1,
	guLiner2 = @Liner2
where guID = @GuestID and guLiner1 is null and @Liner1 is not null

-- Closers y Exit Closers
update Guests
set guCloser1 = @Closer1,
	guCloser2 = @Closer2,
	guCloser3 = @Closer3,
	guExit1 = @Exit1,
	guExit2 = @Exit2
where guID = @GuestID and guCloser1 is null and @Closer1 is not null

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

