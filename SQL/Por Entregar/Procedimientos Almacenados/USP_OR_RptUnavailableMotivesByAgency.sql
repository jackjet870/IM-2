if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptUnavailableMotivesByAgency]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptUnavailableMotivesByAgency]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Unavailable Motives by Angency
** 
** [lchairez]	05/Nov/2013 Creado
** [wtorres]	08/Nov/2013 Cambie el tipo de datos del porcentaje de llegadas
** [aalcocer]	10/Jun/2016 Se modifica el tipo de dato del campo clave de agencias a varchar(max)
**
*/
CREATE PROCEDURE [dbo].[USP_OR_RptUnavailableMotivesByAgency]
	@DateFrom AS DATETIME,				-- Fecha desde
	@DateTo AS DATETIME,				-- Fecha hasta
	@LeadSources AS VARCHAR(8000),		-- Claves de Lead Sources
	@Markets varchar(8000) = 'ALL',		-- Claves de mercados
	@Agencies varchar(max) = 'ALL'		-- Claves de agencias
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ArrivalsTotal AS INT
	
	-- SACAMOS LOS DATOS QUE NECESITAMOS PARA EL REPORTE
	SELECT  um.umID, um.umN UnavailMot, mk.mkID, mk.mkN Market, ag.agID, ag.agN Agency
		,COUNT(*) Arrivals, CAST(0 AS DECIMAL(12,6)) as ArrivalsPercentage, SUM(CASE WHEN guAvail <> guAvailBySystem THEN 1 ELSE 0 END) ByUser
	INTO #Data
	FROM Guests gu
	LEFT JOIN dbo.UnavailMots um on gu.guum = um.umID
	LEFT JOIN Agencies ag on gu.guAg = ag.agID
	LEFT JOIN Markets mk on ag.agmk = mk.mkID
	WHERE 
	-- Fecha de llegada
	 gu.guCheckInD between @DateFrom and @DateTo
	 -- Con Check In
	 AND gu.guCheckIn = 1
	 -- No Rebook
	 AND gu.guRef is null
	 -- Lead Source
	 AND gu.guls in (select item from split(@LeadSources, ','))
	 -- No disponible
	 AND gu.guAvail = 0 and gu.guum > 0
	 -- Markets
	 AND (@Markets = 'ALL' or mk.mkID in (select item from Split(@Markets, ',')))
	 -- Agencies
	 AND (@Agencies = 'ALL' or gu.guAg in (select item from Split(@Agencies, ',')))
	GROUP BY um.umID, um.umN, mk.mkID, mk.mkN, ag.agID, ag.agN
	ORDER BY um.umN, mk.mkN, ag.agN
	
	-- OBTENEMOS LA CANTIDAD DE MERCADOS DE ACUERDO A SU MOTIVO
	SET @ArrivalsTotal = (SELECT SUM(Arrivals) FROM #Data)
	
	-- CALCULAMOS EL PORCENTAJE DE LAS LLEGADAS
	UPDATE #Data SET ArrivalsPercentage = CAST(Arrivals AS DECIMAL(12,6)) / @ArrivalsTotal

	-- DELVOLVEMOS LOS DATOS DEL REPORTE
	SELECT * FROM #Data
	
DROP TABLE #Data
	
END
GO
