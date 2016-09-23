if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_GetSalesmanTypesFTB]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_GetSalesmanTypesFTB]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

	
	
	
	