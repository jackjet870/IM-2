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
** Genera el Refund y actualiza los datos de los bookingdeposits
** 
[LorMartinez] 13/Ene/2015, Devuelve la fecha en modo MM/YY
*/
CREATE FUNCTION dbo.FormatExpDate(@ExpD datetime)
RETURNS varchar(5)
AS
BEGIN 
 
 DECLARE @strRes varchar(10)
 SELECT @strRes=''
 
 IF @ExpD IS NULL
   select @strRes=''
 ELSE IF YEAR(@ExpD) = 1900
   SELECT @STRRES=''
 ELSE
    SELECT @strRes= CONVERT(VARCHAR(2),MONTH(@ExpD)) + '/' + 
          substring(CONVERT(VARCHAR(4),YEAR(@ExpD)),3,2)
       
 return @strRes
 
END
GO