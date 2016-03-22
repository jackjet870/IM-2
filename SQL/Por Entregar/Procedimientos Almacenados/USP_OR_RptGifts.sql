USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_OR_RptGifts]    Script Date: 03/16/2016 09:58:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene informacion de gifts para el reporte.
** 
** [edgrodriguez] 16/Mar/2016 Created. 
**
*/
ALTER PROCEDURE [dbo].[USP_OR_RptGifts]	
as
set nocount on

	Select giID, giN, giShortN, giO, giPrice1, giPrice2, giPrice3, giPrice4, giPack, gcN, giInven, giWFolio, giWPax, giA 
	from Gifts inner join GiftsCategs on gigc = gcID