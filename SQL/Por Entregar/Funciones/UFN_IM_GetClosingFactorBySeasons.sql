/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el Closing Factor (Porcentaje de cierre) de una temporada entre 2 fechas
** 
** [ecanul]	12/08/2016 Created
**
*/

CREATE FUNCTION [dbo].[UFN_IM_GetClosingFactorBySeasons](
	@DatesFrom datetime,	-- Fecha Inicio
	@DatesTo datetime		-- Fecha Fin
)
RETURNS money
AS
BEGIN
	declare @tbTemp table(
		ssID varchar(10),
		sdStartD datetime,
		sdEndD datetime,
		ssClosingFactor money,
		count int
	);
	insert INTO @tbTemp
	SELECT 
		ss.ssID, sd.sdStartD, sd.sdEndD, ss.ssClosingFactor, 0 Count
	FROM 
		dbo.SeasonsDates sd
		LEFT JOIN dbo.Seasons ss ON sd.sdss = ss.ssID
	where Year(sd.sdStartD) >= YEAR(@DatesFrom) and Year(sd.sdStartD) <= YEAR(@DatesTo)
	order by sd.sdStartD

	WHILE @DatesFrom <= @DatesTo
	BEGIN
		UPDATE @tbTemp 
		SET [count] = ([count] + (SELECT COUNT( ssID) FROM @tbTemp WHERE @DatesFrom BETWEEN sdStartD AND sdEndD))
		WHERE @DatesFrom BETWEEN sdStartD AND sdEndD;
		SET @DatesFrom = DATEADD(DAY,1,@DatesFrom);
	END
	DECLARE @Result MONEY
	SET @Result =(SELECT TOP 1 ssClosingFactor FROM @tbTemp ORDER BY Count DESC)
RETURN @Result
END