if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsByCategoryProgram]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsByCategoryProgram]
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de regalos por categoria
**		1. Si la sala de ventas esta cerrada
**		2. Detalle de recibos de regalos
**		3. Dias
** 
** [axperez]	29/Oct/2013 Creado
** [axperez]	30/Oct/2013 Modificado: agregue record set de dias
**
*/
CREATE PROCEDURE [dbo].[USP_OR_RptGiftsByCategoryProgram]
	@DateFrom as DateTime,			-- Fecha desde
	@DateTo as DateTime,			-- Fecha hasta
	@SalesRooms AS VARCHAR(8000)	-- Claves de las Salas de ventas
AS 
SET NOCOUNT ON

-- Detalle de recibos de regalos
-- ===================================
SELECT 
	G.giN AS gegi, 
	DAY(R.grD) AS Day, 
	SUM(case when G.gigc = 'TOURS' then D.geAdults + D.geMinors else D.geQty end) AS Quantity,
	SUM(D.gePriceA + D.gePriceM) AS Price,
	G.gigc, 
	C.gcN,
	G.giPrice1 AS UnitCost,
	(SUM(case when G.gigc = 'TOURS' then D.geAdults + D.geMinors else D.geQty end) * G.giPrice1) AS TotalCost,
	P.pgN AS Program
INTO #GiftsReceiptsDetail
FROM GiftsReceiptsC D
	INNER JOIN GiftsReceipts R ON D.gegr = R.grID
	INNER JOIN Gifts G ON D.gegi = G.giID
	LEFT JOIN GiftsCategs C ON G.gigc = C.gcID 
	INNER JOIN Guests Gu ON R.grgu = Gu.guID 
	INNER JOIN LeadSources LD ON Gu.guls = LD.lsID 
	INNER JOIN Programs P ON LD.lspg = P.pgID
WHERE
	-- Salas de ventas
	(R.grsr IN (SELECT item FROM split(@SalesRooms, ','))) AND
	-- Fecha del recibo
	R.grD BETWEEN @DateFrom AND @DateTo AND
	-- No cancelados
	R.grCancel = 0 AND
	-- No cargados a vendedores
	R.grct NOT IN ('PR', 'LINER', 'CLOSER')
GROUP BY R.grD, DAY(R.grD), G.giN, G.gigc, C.gcN, G.giPrice1, P.pgN
ORDER BY P.pgN, C.gcN desc, G.giN, R.grD

-- 1. Cierre de salas de ventas
-- ===================================
SELECT 
	CAST(CASE WHEN srGiftsRcptCloseD >= @DateTo THEN 1 ELSE 0 END AS BIT) AS GiftsClosed  
FROM 
	SalesRooms 
WHERE srID IN (SELECT item FROM split(@SalesRooms, ','))

-- 2. Detalle de recibos de regalos
-- ===================================
SELECT * FROM #GiftsReceiptsDetail

-- 3. Dias
-- ===================================
SELECT DISTINCT Day
FROM #GiftsReceiptsDetail

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO