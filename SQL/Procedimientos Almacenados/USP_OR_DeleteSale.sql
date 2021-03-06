if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_DeleteSale]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_DeleteSale]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina una venta
** 
** [wtorres]	28/Ene/2012 Creado
**
*/
create procedure [dbo].[USP_OR_DeleteSale]
    @Sale int	-- Clave de la venta
as
set nocount on

declare
	@Guest int,	-- Clave del huesped
	@Sales int	-- Numero de ventas

-- obtenemos la clave del huesped
select @Guest = sagu from Sales where saID = @Sale

-- Pagos
delete from Payments where pasa = @Sale

-- Ventas especiales de los vendedores
delete from SalesSalesmen where smsa = @Sale

-- Historico de la venta
delete from SalesLog where slsa = @Sale

-- Venta
delete from Sales where saID = @Sale

-- obtenemos el numero de ventas que le quedaron
select @Sales = Count(*) from Sales where sagu = @Guest

-- si ya no le quedan mas ventas
if @Sales = 0

	-- indicamos que el huesped ya no tiene mas ventas
	update Guests set guSale = 0 where guID = @Guest

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

