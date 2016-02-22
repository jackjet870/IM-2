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
[LorMartinez] 5/Enero/2015 Created, Valida que los folios no se sobreescriban
*/
CREATE PROCEDURE dbo.USP_OR_ValidateFolioInvOutPR(
@PR varchar(10),
@Serie varchar(10),
@From int,
@To int,
@isCancel bit =0
)
AS
BEGIN 
 
 DECLARE @bRes bit
 DECLARE @sPR varchar(150)
 SELECT @bRes = 0,@SPR = ''
 
 SET NOCOUNT ON
 --Si no es de cancelacion solo se valida en FolioInvitationsOutsidePR
 IF @isCancel = 0
 BEGIN
   
    SELECT @sPR = por.fippe
    FROM dbo.FolioInvitationsOutsidePR por
    WHERE por.fippe <> @PR
    AND por.fipSerie = @Serie
    AND por.fipFrom between @From AND @To
    
   IF ISNULL(@sPR,'') <> ''
       SELECT @sPR = p.peID + ' - ' + p.peN,
              @bRes= 1
       FROM dbo.Personnel p
       WHERE p.peID = @spr
       --SELECT @bRes= 1
    ELSE
       SELECT @bRes = 0,@sPR =''   
 END
 ELSE
 BEGIN
   
     SELECT @sPR = fc.ficpe
     FROM dbo.FolioInvitationsOutsidePRCancellation fc
     WHERE fc.ficpe <> @PR
     AND fc.ficSerie =@Serie
     and fc.ficFrom between @From and @To
     
    IF ISNULL(@SPR,'') <> ''
       SELECT @sPR = p.peID + ' - ' + p.peN,
              @bRes= 1
       FROM dbo.Personnel p
       WHERE p.peID = @spr
      --SELECT @bRes = 1
    ELSE
      SELECT @bRes = 0,@sPR=''
 END
 
 select @bRes [Result],@sPR [PR]

END
GO