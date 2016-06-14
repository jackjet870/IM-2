USE [OrigosVCPalace]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el grid de GiftsReceiptsAdditional
** 
** [VIPACHECO]	12/Mayo/2016 Created
*/

CREATE PROCEDURE [dbo].[USP_IM_GetGiftsReceiptsAdditional]
				 @GuestID integer  --Indentificador del guest
AS 
BEGIN
	 SELECT gagu,
			Cast(1 as bit) as Generate,
			gaAdditional,
			guLastName1,
			guFirstName1,
			'' as grID
	FROM [dbo].[GuestsAdditional]
	INNER JOIN [dbo].[Guests]
	ON gaAdditional = guID
	WHERE gagu = @GuestID
	ORDER BY Generate
END
