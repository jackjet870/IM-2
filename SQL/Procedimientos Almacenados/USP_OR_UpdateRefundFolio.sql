USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Inserta o modifica el último folio de tipo de refund
**
** [lormartinez]	Created.
*/
CREATE PROCEDURE dbo.USP_OR_UpdateRefundFolio(
@RefundTypeID varchar(2),
@Folio integer)
AS
BEGIN
  SET NOCOUNT ON
  
  --Si ya existe el folio se actualiza
  IF EXISTS(SELECT rtf.rtfrf
            FROM dbo.RefundTypeFolios rtf
            where rtf.rtfrf = @RefundTypeID
            )
   BEGIN
      UPDATE dbo.RefundTypeFolios
      SET rtfFolio= @Folio
      WHERE rtfrf = @RefundTypeID  
   END
  ELSE
   BEGIN
      INSERT INTO dbo.RefundTypeFolios(rtfrf, rtfFolio)
      VALUES(@RefundTypeID,@Folio)
   END
  
  
END
GO