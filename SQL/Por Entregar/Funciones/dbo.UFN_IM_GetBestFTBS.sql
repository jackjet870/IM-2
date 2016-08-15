/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los mejores FTBs entre las fechas seleccionadas
**
** [ecanul] 08/08/2016 Created
**
*/

CREATE FUNCTION [dbo].[UFN_IM_GetBestFTBS](
	@SalesRoom varchar(10),
	@DateFrom datetime,
	@DateTo datetime
)

RETURNS @Table table(
	espe varchar(10),
	peN varchar(40)
)
as
begin
insert @Table
	SELECT es.espe, p.peN
	FROM dbo.Efficiency ef
		INNER JOIN dbo.EfficiencySalesmen es ON es.esef = ef.efID
		INNER JOIN dbo.Personnel p on es.espe = p.peID
	WHERE ef.efsr =  @SalesRoom AND ef.efDateFrom = @DateFrom AND ef.efDateTo = @DateTo
		AND ef.efpd = 'W' AND ef.efet = 'BEST_FTBS'
RETURN
END