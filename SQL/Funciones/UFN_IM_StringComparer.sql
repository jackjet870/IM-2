if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_IM_StringComparer]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_IM_StringComparer]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




