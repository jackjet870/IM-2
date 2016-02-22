SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ObtenerHotelesPorEstado]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ObtenerHotelesPorEstado]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
**[cgutierrez]	24/07/2008	Created	Obtenemos un listado de hoteles dado un estado especificado
**
*/

CREATE PROCEDURE [dbo].[USP_OR_ObtenerHotelesPorEstado]	(
	@Estado	[int])
AS 
	if ( @Estado = 2 )
		SELECT [hoID],[hoGroup],[hoar],[hoA] 
	 FROM [dbo].[Hotels] 
	else
 	SELECT [hoID],[hoGroup],[hoar],[hoA] 
	 FROM [dbo].[Hotels]  where   [hoA]  = @Estado
 
	
	 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
