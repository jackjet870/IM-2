if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptDailyGiftSimple]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptDailyGiftSimple]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la cantidad de regalos por Sala de ventas.
** 
** [edgrodriguez]	07/Abr/2016 Created
** [wtorres]		05/Ago/2016 Modified. Optimizacion del WHERE de "Charge To"
**
*/
CREATE PROCEDURE [dbo].[USP_IM_RptDailyGiftSimple]
	@Date as datetime,
	@SalesRooms varchar(8000) = 'ALL'
as
set nocount on

SELECT 
	R.grCancel,
	R.grID,
	G.giN,
	G.giShortN,
	D.geQty,
	R.grlo
FROM GiftsReceipts R
	INNER JOIN GiftsReceiptsC D ON D.gegr = R.grID
	INNER JOIN Gifts G ON G.giID = D.gegi
WHERE
	-- Sala de ventas
	(@SalesRooms = 'ALL' or R.grsr in (select item from split(@SalesRooms, ',')))
	-- Fecha del recibo
	AND R.grD = @Date
	-- No cancelados
	AND R.grCancel = 0
	-- No cargados a vendedores
	AND R.grct NOT IN ('PR', 'LINER', 'CLOSER')
ORDER BY D.gegi