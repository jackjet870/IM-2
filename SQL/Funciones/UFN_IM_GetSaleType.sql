

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el tipo de venta
** 
** [edgrodriguez]	27/May/2016 Creado
**
*/
CREATE function [dbo].[UFN_IM_GetSaleType](
	@dateFrom datetime,
	@dateTo datetime,
	@sast varchar(8000),
	@ststc varchar(8000),
	@guDepSale money,
	@saD datetime,
	@saProcD datetime,
	@saCancelD datetime,
	@gusr varchar(8000),
	@sasr varchar(8000),
	@saByPhone bit	
)
returns int
as
begin

declare @Result int

SELECT	
	@Result=
		CASE @sast
			WHEN 'BUMP' THEN 8
			WHEN 'REGEN' THEN 9
			WHEN 'UA' THEN 14
			
			ELSE
			CASE @ststc
				WHEN 'N' THEN
					CASE 
						WHEN @guDepSale > 0 THEN 10
						WHEN (@saD BETWEEN @dateFrom AND @dateTo)
						AND @gusr <> @sasr THEN 13
					ELSE 3
					END
				WHEN 'UG' THEN 4
				WHEN 'DG' THEN 5
			END
		END
	
IF(@saByPhone = 1)	
BEGIN
	SET @Result = 11
END

IF(@saProcD IS NOT NULL)
BEGIN
	IF(@saD <> @saProcD AND (@saProcD BETWEEN @dateFrom AND @dateTo))
	BEGIN	
		SET @Result = 6
	END
	IF(@saCancelD BETWEEN @dateFrom AND @dateTo)
	BEGIN 
		SET @Result = 7
	END
END
ELSE IF((@saCancelD BETWEEN @dateFrom AND @dateTo) AND (@saD NOT BETWEEN @dateFrom AND @dateTo))
BEGIN
	SET @Result = 7
END

return @Result
END


