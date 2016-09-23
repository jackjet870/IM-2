USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetClosingFactorBySeasons]    Script Date: 09/22/2016 19:17:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetClosingFactorBySeasons]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetClosingFactorBySeasons]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetPersonnelAssistancesByWeek]    Script Date: 09/22/2016 19:17:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetPersonnelAssistancesByWeek]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetPersonnelAssistancesByWeek]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetPersonnelPostIDByDate]    Script Date: 09/22/2016 19:17:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetPersonnelPostIDByDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetPersonnelPostIDByDate]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesAmount]    Script Date: 09/22/2016 19:17:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesmanTypeBySegments]    Script Date: 09/22/2016 19:17:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetSalesmanTypeBySegments]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetSalesmanTypeBySegments]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesmanTypesCloser]    Script Date: 09/22/2016 19:17:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetSalesmanTypesCloser]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetSalesmanTypesCloser]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesmanTypesFTB]    Script Date: 09/22/2016 19:17:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetSalesmanTypesFTB]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetSalesmanTypesFTB]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSaleType]    Script Date: 09/22/2016 19:17:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetSaleType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetSaleType]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSelfGenType]    Script Date: 09/22/2016 19:17:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_GetSelfGenType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_GetSelfGenType]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_IsSelfGen]    Script Date: 09/22/2016 19:17:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_IsSelfGen]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_IsSelfGen]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_StringComparer]    Script Date: 09/22/2016 19:17:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_IM_StringComparer]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_IM_StringComparer]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetCnxSalesAmount]    Script Date: 09/22/2016 19:17:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetCnxSalesAmount]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetCnxSalesAmount]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetSalesDownPayment]    Script Date: 09/22/2016 19:17:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFN_OR_GetSalesDownPayment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[UFN_OR_GetSalesDownPayment]
GO

USE [OrigosVCPalace]
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetClosingFactorBySeasons]    Script Date: 09/22/2016 19:17:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetPersonnelAssistancesByWeek]    Script Date: 09/22/2016 19:17:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el número de asistencias de un personal en una semana.
** Si no tiene definido las asistencias devuelve -1
**
** 
** [ecanul]  13/08/2016 Created
**
*/
CREATE FUNCTION [dbo].[UFN_IM_GetPersonnelAssistancesByWeek](
	@SalesRoom varchar(10),	-- Clave de la sala de ventas
	@Personnel varchar(10),	-- Clave del personal
	@DateFrom datetime,		-- Fecha de inicio de la semana
	@DateTo datetime		-- Fecha de fin de la semana
)
RETURNS int
AS
BEGIN
	declare
	@Count int,
	@NumAssistance int

	-- Determina si hay asistencias
	set @Count = (select count(aspe) from Assistance
		where asPlaceID = @SalesRoom and aspe = @Personnel and asStartD = @DateFrom and asEndD = @DateTo)
	-- Si hay asistencias
	if @Count = 1
	begin
		select @NumAssistance = asNum
		from Assistance
		where asPlaceID = @SalesRoom and aspe = @Personnel and asStartD = @DateFrom and asEndD = @DateTo
	end
	-- Si NO hay asistencias
	ELSE
		set @NumAssistance = -1
RETURN @NumAssistance
END
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetPersonnelPostIDByDate]    Script Date: 09/22/2016 19:17:51 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesAmount]    Script Date: 09/22/2016 19:17:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el Sales Amount del monto de venta enviado 
** 
** [ecanul]	18/jul/2016 Created
**
*/
CREATE FUNCTION [dbo].[UFN_IM_GetSalesAmount](
	@GrossAmount money,			-- Gross Amount
	@Salesman1 varchar(10),		-- Clave del vendedor 1
	@Salesman2 varchar(10),		-- Clave del vendedor 2
	@Salesman3 varchar(10),		-- Clave del vendedor 3
	@Salesman4 varchar(10),		-- Clave del vendedor 4
	@Salesman5 varchar(10)		-- Clave del vendedor 5
)
RETURNS money
AS
BEGIN
SET @GrossAmount = @GrossAmount * dbo.UFN_OR_GetPercentageSalesman(@Salesman1,@Salesman2,@Salesman3,@Salesman4,@Salesman5);
RETURN @GrossAmount
END
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesmanTypeBySegments]    Script Date: 09/22/2016 19:17:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el tipo de un vendedor en el reporte de estadisticas por segmentos
** 
** [aalcocer]	17/Jun/2016 Creado
**
*/

create function [dbo].[UFN_IM_GetSalesmanTypeBySegments](
	@SalesmanID varchar(10),	-- Clave de un vendedor
	@PostID varchar(10),		-- Clave del puesto	
	@Role varchar (50),			-- Clave de rol
	@SelfGen bit				-- marcado como Self Gen
)
returns varchar(50)
as
BEGIN
	DECLARE @Result varchar(50)
	
	--si no tiene puesto
	IF (@PostID = 'NP') SET @Result = 'NO POST'
	ELSE BEGIN	
		IF(dbo.UFN_IM_IsSelfGen(@SalesmanID,@Role,@SelfGen) = 1) SET @Result = 'Front To Middle'
		ELSE BEGIN
			DECLARE @PostName varchar(50)
			SELECT @PostName = poN FROM Posts where poID = @PostID
			-- si es un regen
			IF (@PostID = 'REGEN') SET @Result = @PostName
			ELSE BEGIN			
				SET @Result = CASE @Role
					WHEN 'Liner' THEN CASE @PostID					
						WHEN 'LINER' THEN @PostName
						WHEN 'FTM' THEN @PostName
						WHEN 'CLOSER' THEN @PostName + ' As Front To Back'
						WHEN 'EXIT' THEN @PostName + ' As Front To Back'
						ELSE 'Front To Back'
					END
					WHEN 'Closer' THEN CASE @PostID					
						WHEN 'CLOSER' THEN @PostName
						WHEN 'EXIT' THEN @PostName
						ELSE @postName + ' As Closer'						
					END
					WHEN 'Exit' THEN CASE @PostID					
						WHEN 'EXIT' THEN @PostName					
						ELSE @PostName + ' As Exit Closer'
					END
				END
			END
		END
	END
	return @Result
END


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesmanTypesCloser]    Script Date: 09/22/2016 19:17:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina los tipos de un vendedor Closer (Own, As Front To Back, With Junior)
** 
** [aalcocer]	12/Jul/2016 Creado
**
*/

create function [dbo].[UFN_IM_GetSalesmanTypesCloser](
	@Role varchar (10),			-- Clave de rol
	@Sold bit,				-- Indica si vendio	
	@Own bit					-- Indica si trabajo solo
)
returns varchar(5)
as
BEGIN
	DECLARE @Result varchar(50)
	
	SET @Result = CASE @Role
					WHEN 'Liner' THEN CASE 					
						WHEN @own=1 THEN 'AS'
						WHEN @sold=1 THEN 'WITH'						
						ELSE 'AS'
					END
					WHEN 'Closer' THEN 'OWN'						
					WHEN 'Exit' THEN CASE					
						WHEN @sold=1 THEN 'OWN'
					END
				END
	return @Result
END


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSalesmanTypesFTB]    Script Date: 09/22/2016 19:18:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina los tipos de un vendedor FTB (Own, With Closer o As Closer)
** 
** [michan]	21/Jul/2016 Creado
**
*/

create function [dbo].[UFN_IM_GetSalesmanTypesFTB](
	@idsalesmen varchar(max),
	@Role varchar (10),			-- Clave de rol
	@Sold bit,				-- Indica si vendio	
	
	@salesmenid1 varchar(max),
	@salesmenpost1 varchar(max),
	@salesmenrol1 varchar(max),
	@salesmenid2 varchar(max),
	@salesmenpost2 varchar(max),
	@salesmenrol2 varchar(max),	
	@salesmenid3 varchar(max),
	@salesmenpost3 varchar(max),
	@salesmenrol3 varchar(max),
	@salesmenid4 varchar(max),
	@salesmenpost4 varchar(max),
	@salesmenrol4 varchar(max),
	@salesmenid5 varchar(max),
	@salesmenpost5 varchar(max),
	@salesmenrol5 varchar(max),
	@salesmenid6 varchar(max),
	@salesmenpost6 varchar(max),
	@salesmenrol6 varchar(max)	
)
returns varchar(5)
as
BEGIN
	DECLARE @Result varchar(50)
	
	DECLARE @IsWorkAlone bit
	
	DECLARE @salesmen TABLE(id varchar (max) DEFAULT NULL, post varchar (max)DEFAULT NULL, rol varchar (max)DEFAULT NULL ) -- Tabla que contiene todos los vendedores
	
	INSERT INTO @salesmen (id)
		SELECT ID
		FROM
		(
			SELECT @salesmenid1 AS ID
			UNION SELECT @salesmenid2 
			UNION SELECT @salesmenid3 
			UNION SELECT @salesmenid4 
			UNION SELECT @salesmenid5 
			UNION SELECT @salesmenid6
			
		) AS tblI
		
	
	UPDATE @salesmen
	SET post = T2.Post
	FROM @salesmen t1, 
	(SELECT Post
    FROM
	(
		SELECT @salesmenpost1 AS Post
		UNION SELECT @salesmenpost2 
		UNION SELECT @salesmenpost3 
		UNION SELECT @salesmenpost4 
		UNION SELECT @salesmenpost5 
		UNION SELECT @salesmenpost6
		
	) AS tblP
	) AS T2
	
	UPDATE @salesmen
	SET rol = T3.Rol
	FROM @salesmen t1, 
	(SELECT Rol
    FROM
	(
		SELECT @salesmenrol1 AS Rol
		UNION SELECT @salesmenrol2 
		UNION SELECT @salesmenrol3 
		UNION SELECT @salesmenrol4 
		UNION SELECT @salesmenrol5 
		UNION SELECT @salesmenrol6
		
	) AS tblP
	) AS T3
	DECLARE @workalone BIT 
	SET @workalone = 
	-- Determina si un vendedor trabajo solo 
	CASE WHEN dbo.UFN_IM_StringComparer(@idsalesmen, @salesmenid1,@salesmenid2,@salesmenid3,@salesmenid4,@salesmenid5,@salesmenid6) = 1 THEN 
		1
	ELSE -- si trabajo con un Regen y si el Regen trabajo como Closer o como Exit
		CASE WHEN (SELECT COUNT(id) FROM @salesmen WHERE post = 'REGEN' AND rol IN ('Closer', 'Exit') AND id <> @idsalesmen) >= 1 THEN 
			0
		ELSE --si no trabajo con un Regen
			CASE WHEN (SELECT COUNT(id) FROM @salesmen WHERE id <> @idsalesmen AND rol IN ('Closer', 'Exit')) >= 1 THEN 
				0
			ELSE
				1
			END
		END
	END
	
	SET @Result = CASE @Role
					WHEN 'Liner' THEN CASE 					
						WHEN @workalone = 1 AND @Sold = 1 THEN 'OWN'
						WHEN @Sold = 1 THEN 'WITH'
						ELSE 'WITH'	
					END
					WHEN 'Closer' THEN 'AS'						
					WHEN 'Exit' THEN 'AS'
				END
	return @Result
END


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSaleType]    Script Date: 09/22/2016 19:18:08 ******/
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

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetSelfGenType]    Script Date: 09/22/2016 19:18:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el tipo de SelfGen de un Personnel (para el reporte SelfGen & SelfGen Team) 
**						Retutn 1.- Selfgen	0.- Selfgen Team
** 
** [ecanul]	15/jul/2016 Created
**
*/
CREATE FUNCTION [dbo].[UFN_IM_GetSelfGenType](
	@PersonnelID varchar(10),	-- Clave del Personal
	@PR1 varchar(10) = NULL,	-- Clave del PR1
	@PR2 varchar(10) = NULL,	-- Clave del PR2
	@PR3 varchar(10) = NULL		-- Clave del P3
)
RETURNS int
AS
BEGIN
DECLARE @Result int
-- Si no se envio ningun PR
IF @PR1 IS NULL AND @PR2 IS NULL AND @PR3 IS NULL
	-- Se determina si es un Front To Middle en general
	SELECT
		@Result = count(peID)
	FROM Personnel
	WHERE peLinerID = @PersonnelID
ELSE
	-- Si se envio la clave del PR1
	IF @PR1 IS NOT NULL
	BEGIN
		BEGIN
			SELECT @Result = Count(peID)
			FROM dbo.Personnel
			WHERE peLinerID = @PersonnelID AND peID = @PR1
			-- Si no se encontro la clave del FTM y se envio PR2
			IF @Result = 0 AND @PR2 IS NOT NULL
				BEGIN
					SELECT @Result = COUNT(peID)
					FROM dbo.Personnel
					WHERE peLinerID = @PersonnelID AND peID = @PR2
					-- Si no se encontro el FTM y se mando PR3
					IF @Result = 0 AND @PR3 IS NOT NULL
						SELECT @Result = COUNT(peID)
						FROM dbo.Personnel
						WHERE peLinerID = @PersonnelID AND peID = @PR3
				END
			END
		END
RETURN @Result
END
GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_IsSelfGen]    Script Date: 09/22/2016 19:18:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina si un personal es Self Gen
** 
** [aalcocer]	25/Jun/2016 Creado
**
*/
create function [dbo].[UFN_IM_IsSelfGen](
	@SalesmanID varchar(10),	-- Clave de un vendedor
	@Role varchar (50),			-- Clave de rol
	@SelfGen bit				-- marcado como Self Gen	
)
returns bit
AS
BEGIN
	DECLARE @IsSelfGen bit 
	-- si desempeña el rol de liner
	IF(@Role = 'Liner') BEGIN
		-- si la Hostess lo marco como Self Gen
		IF(@SelfGen = 1) SET @IsSelfGen = 1	
		-- si es un Front To Middle			
		ELSE SELECT @IsSelfGen = CAST(count(peID) AS bit) FROM Personnel where peLinerID = @SalesmanID 
	END	
RETURN @IsSelfGen
END


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_IM_StringComparer]    Script Date: 09/22/2016 19:18:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Determina si las cadenas son el mismo valor (omitiendo los valores nulos)
** 
** [aalcocer]	29/Jun/2016 Creado
**
*/
CREATE FUNCTION [dbo].[UFN_IM_StringComparer](
	@Value1 varchar(max),
	@Value2 varchar(max),
	@Value3 varchar(max),
	@Value4 varchar(max),
	@Value5 varchar(max),
	@Value6 varchar(max),
	@Value7 varchar(max))
RETURNS bit
AS
BEGIN
	DECLARE @IsSameValue bit 
	SELECT @IsSameValue = CAST(CASE
         WHEN COUNT(DISTINCT Value) > 1 THEN 0 
         ELSE 1
    END AS BIT)
    FROM
	(
		SELECT @Value1 AS Value
		UNION SELECT @Value2 
		UNION SELECT @Value3 
		UNION SELECT @Value4 
		UNION SELECT @Value5 
		UNION SELECT @Value6
		UNION SELECT @Value7
	) AS tbl
	Where tbl.Value is NOT NULL
	RETURN @IsSameValue
END


GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetCnxSalesAmount]    Script Date: 09/22/2016 19:18:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el monto de de ventas canceladas
** [Lormartinez] 19/Ene/2016, Created
*/
CREATE FUNCTION [dbo].[UFN_OR_GetCnxSalesAmount](
@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final
	@SalesRoom varchar(MAX)= 'ALL'	-- Clave de la sala
) RETURNS MONEY
AS
BEGIN
 DECLARE @RES money
 SELECT @RES= 0
 
  select 
	    @res = sum(SALE)
  FROM dbo.Sales s  
  INNER JOIN hotel.analista_h.CLMEMBER m ON m.[APPLICATION] = s.saMembershipNum
  WHERE s.saProcD between @DateFrom AND @DateTo  
  --Salas de venta
  and (@SalesRoom = 'ALL' or S.sasr in (select item from split(@SalesRoom, ','))) 
  -- Ventas canceladas por código de venta caída
  and CANCEL_SALE_DT <> '17530101' and cnxcod_id = 'VC'  
  -- Agrupadas por día
  group by  s.saProcD
  
  RETURN ISNULL(@RES,0)

END

GO

/****** Object:  UserDefinedFunction [dbo].[UFN_OR_GetSalesDownPayment]    Script Date: 09/22/2016 19:18:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las membresias que se vendieron en una sala en un rango de fechas determinado
** 
** [LorMartinez] 18/Ene/2016 Modificado, Se agrega funcionalidad para mas de un saleroom
*/
CREATE FUNCTION [dbo].[UFN_OR_GetSalesDownPayment]
(	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime,		-- Fecha final  
	@SalesRoom varchar(MAX)='ALL',
  @Collected bit =0)
RETURNS MONEY
  -- Clave de la sala
as
BEGIN
--set nocount on

DECLARE @Res money
select @Res=0

;with cte as
(
select 	s.saDownPayment,s.saDownPaymentPaid
	from Guests g
	left outer join Sales s on g.guID = s.sagu
  OUTER APPLY (SELECT TOP 1 s2.saID SaleID
               FROM dbo.Sales s2
               WHERE s2.saMembershipNum= s.saMembershipNum
               ORDER By s2.saID DESC
              )LastSale
	where g.guShowD between @DateFrom and @DateTo 
    --Sales room
    and (@SalesRoom='ALL' OR (@SalesRoom <> 'ALL' AND g.gusr in (select item from dbo.split(@SalesRoom,','))))
    --and gusr = @SalesRoom
		-- Procesado
    and s.saProc = 1
		-- No se toman en cuenta las ventas pendientes
		and (g.guShowD = s.saProcD or (g.guShowD <> s.saProcD and s.saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas Bump ni Regen
		and s.sast <> 'BUMP' and s.sast <> 'REGEN'
		-- No se toman en cuenta las ventas canceladas
		and s.saCancel <> 1
UNION ALL    
select
	s.saDownPayment,s.saDownPaymentPaid
	from Guests g
	left outer join Sales s on guID = sagu
  OUTER APPLY (SELECT TOP 1 s2.saID SaleID
               FROM dbo.Sales s2
               WHERE s2.saMembershipNum= s.saMembershipNum
               ORDER By s2.saID DESC
              )LastSale
	where 
		-- Ventas que no tienen Show o que su show es de otro dia
		( (g.guShowD is null or not g.guShowD between @DateFrom and @DateTo)
		-- Ventas del dia  y ventas procesables del dia (no se toman en cuenta las canceladas)
		and (s.saD between @DateFrom and @DateTo or s.saProcD between @DateFrom and @DateTo)
		-- Bumps, Regen y las ventas de otra sala
		or ( (s.sast = 'BUMP' or s.sast = 'REGEN' or g.gusr <> s.sasr) and s.saD between @DateFrom and @DateTo) )
		-- Ventas de la sala  and sasr = @SalesRoom
		and (@SalesRoom='ALL' OR (@SalesRoom <> 'ALL' AND gusr in (select item from dbo.split(@SalesRoom,','))))
		-- No se toman en cuenta los Deposit Before
		and not (s.sast = 'NS' and g.guDepSale > 0 and not (s.saProc = 1 and s.saD <> s.saProcD and s.saProcD between @DateFrom and @DateTo))
		-- No se toman en cuenta las ventas pendientes
		and not (s.saProcD > @DateTo)
    AND Lastsale.SaleID = s.saID
)
SELECT @Res = CASE 
      WHEN @Collected = 0 THEN sum(isnull(c.saDownPayment,0))
      ELSE sum(isnull(c.saDownPaymentPaid,0))
      END
FROM cte c

RETURN ISNULL(@Res,0)

END

GO


