if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptTaxisOut]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptTaxisOut]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos de reporte de taxis de salida
** 
** [lchairez]	28/Oct/2013 Documentado y optimizado. Agregue el campo de comentarios de hostess
**
*/

CREATE procedure [dbo].[USP_OR_RptTaxisOut]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL'	-- Claves de salas
as
set nocount on

select 
	R.grID,
	R.grGuest, 
	R.grlo, 
	R.grHotel,
	R.grPax, 
	R.grD, 
	R.grpe, 
	R.grTaxiOut, 
	R.grTaxiOutDiff,
	R.grNum,
	R.grHost,
	G.guWComments
from GiftsReceipts R
	left join Guests G on G.guID = R.grgu
where 
	-- No cancelados
	R.grCancel = 0
	-- Con taxi de salida
	and (R.grTaxiOut > 0 or R.grTaxiOutDiff > 0)
	-- Fecha de recibo de regalos
	and R.grD between @DateFrom and @DateTo
	-- Salas de venta
	and (@SalesRooms = 'ALL' or R.grsr in (select item from Split(@SalesRooms, ',')))
order by grID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

