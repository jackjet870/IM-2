USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_IM_RptLoginLog]    Script Date: 04/26/2016 16:08:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Log de Login
** 
** [edgrodriguez]	26/04/2016 Created
*/
CREATE PROCEDURE [dbo].[USP_IM_RptLoginLog]
@DateFrom datetime,
@DateTo datetime,
@Location varchar(8000)='ALL',
@PCName varchar(8000)='ALL',
@Personnel varchar(8000)='ALL'
AS
BEGIN
	SELECT 
		LG.llID,L.loN,LG.llpe,P.peN,LG.llPCName
	FROM LoginsLog LG
	INNER JOIN Locations L ON LG.lllo=L.loID
	INNER JOIN Personnel P ON LG.llpe=P.peID
	WHERE 
		LG.llID BETWEEN @DateFrom AND @DateTo
		and (@Location = 'ALL' or L.loID=@Location)
		and (@PCName ='ALL' or LG.llPCName=@PCName)
		and (@Personnel = 'ALL' or P.peID=@Personnel)
	ORDER BY LG.llID
END