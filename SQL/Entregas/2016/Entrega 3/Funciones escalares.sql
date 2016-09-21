USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetPersonnelPostIDByDate]    Script Date: 09/21/2016 17:04:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetPersonnelPostIDByDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetPersonnelPostIDByDate]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSaleType]    Script Date: 09/21/2016 17:04:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetSaleType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetSaleType]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetPersonnelPostIDByDate]    Script Date: 09/21/2016 17:04:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el puesto el puesto que tenia el personal entre las fechas indicadas
** 
** [ecanul]	27/May/2016 Created
** [ecanul] 14/06/2016 Modified. Cambiado nombre de UFN_IM_GetPerssonelPostsIDByDate a UFN_IM_GetPersonnelPostIDByDate
**
*/
CREATE FUNCTION [dbo].[UFN_IM_GetPersonnelPostIDByDate](
	@date Datetime,
	@PersonnelID varchar(50)
)
Returns Varchar(50)
AS
BEGIN
	DECLARE @postLCount int;
	DECLARE @POSTS VARCHAR(50);
	SET @postLCount = (SELECT COUNT(ppDT) FROM PostsLog WHERE pppe = @PersonnelID AND CONVERT(VARCHAR,ppdt,112)<=@date);
	IF @PostLCount > 0
		SET @POSTS = (SELECT TOP 1 pppo from PostsLog WHERE pppe = @PersonnelID AND CONVERT(VARCHAR,ppdt,112)<=@date ORDER BY ppDT DESC);
	ELSE
		SET @POSTS = (SELECT pepo FROM Personnel WHERE peID = @PersonnelID);
	IF @POSTS IS NULL
		SET @POSTS = 'NP'
	RETURN @POSTS;
END

GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSaleType]    Script Date: 09/21/2016 17:05:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



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



GO


