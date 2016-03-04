IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[USP_OR_GetMailOutTextsByLeadSource]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[USP_OR_GetMailOutTextsByLeadSource]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los mensajes de emails de un Lead Source 
** 
** [aalcocer]	22/feb/2016 Creado
**
*/

CREATE PROCEDURE [dbo].[USP_OR_GetMailOutTextsByLeadSource]
	@mtls VARCHAR(10),  --Clave del Lead Source
	@mtA BIT			--Activo
AS
BEGIN
SELECT mtmoCode, mtla, mtRTF, laMrMrs, laRoom 
FROM MailOutTexts 
INNER JOIN Languages ON mtla = laID 
WHERE mtls = @mtls AND mtA = @mtA 
ORDER BY mtmoCode, mtla
END
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO