if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsReceipts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsReceipts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los recibos de regalos
** 
** [wtorres]	23/Mar/2015 Created
**
*/
create procedure [dbo].[USP_OR_GetGiftsReceipts]
	@Guest int = 0,						-- Clave del huesped
	@SalesRoom varchar(10) = 'ALL',		-- Clave de la sala de ventas
	@Receipt int = 0,					-- Clave del recibo
	@Folio varchar(10) = 'ALL',			-- Folio de Palace Resorts
	@DateFrom datetime = null,			-- Fecha desde
	@DateTo datetime = null,			-- Fecha hasta
	@Name varchar(20) = 'ALL',			-- Nombre
	@Reservation varchar(15) = 'ALL'	-- Folio de reservacion
as
set nocount on

select R.grID, R.grNum, R.grExchange
from GiftsReceipts R
	left join Guests G on G.guID = R.grgu
where
	-- Huesped
	(@Guest = 0 or R.grgu in (
		select @Guest
		union all select gaAdditional from GuestsAdditional where gagu = @Guest))
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or R.grsr = @SalesRoom)
	-- Recibo
	and (@Receipt = 0 or R.grID = @Receipt)
	-- Folio de Palace Resorts
	and (@Folio = 'ALL' or R.grNum = @Folio)
	-- Fecha
	and (@DateFrom is null or R.grD between @DateFrom and @DateTo)
	-- Nombre
	and (@Name = 'ALL' or R.grGuest like '%' + @Name + '%')
	-- Folio de reservacion
	and (@Reservation = 'ALL' or G.guHReservID = @Reservation)
order by R.grD, R.grID, R.grNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

