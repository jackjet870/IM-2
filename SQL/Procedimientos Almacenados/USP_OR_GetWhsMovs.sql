if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetWhsMovs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetWhsMovs]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Script para obtener los  de un almacén 
** que ocurrieron en un día en específico.
** 
** [edgrodriguez]	22/feb/2016 Created
**
*/
CREATE procedure [dbo].[USP_OR_GetWhsMovs]
@wmwh varchar(10),	--Clave del almacén
@wmD datetime		--Fecha
as
Begin
	SELECT wmD, wmpe, peN, wmQty, giN, wmComments, wmwh FROM WhsMovs 
	INNER JOIN Personnel ON wmpe = peID 
	INNER JOIN Gifts ON wmgi=giID 
	WHERE wmwh = @wmwh AND wmD = @wmD
End
