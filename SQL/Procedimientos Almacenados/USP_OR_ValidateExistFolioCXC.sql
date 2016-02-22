GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/* 
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que exista el folio en la tabla de folios y que no exista el folio en el BookingDeposits
** 
** [bcc]	14/01/2015 Created
** 
*/
CrEATE PROCEDURE [dbo].[USP_OR_FolioValidCXC]
	@Number INTEGER,		-- Numero folio
	@GuestID INTEGER,		-- Id del huesped
	@Active BIT,			-- Especifica si esta activo el rango de folios
	@Action INTEGER			-- Especifica la accion a realizar Insert(0) o Update(1)	
AS
SET NOCOUNT ON

DECLARE @Result VARCHAR(100)
	
-- ponemos el resultado como verdadero
SET @Result = ''

-- validamos que el rango dado no exista en la tabla
IF EXISTS(
	SELECT *
	FROM FolioCXC
	WHERE fiA = @Active
		AND (@Number between fiFrom AND fiTo))
		
	SET @Result = 'VALIDO'
ELSE
	SET @Result = 'Folio Invalid'

IF @action = 0
BEGIN
 IF EXISTS(SELECT bdfoliocxc FROM BookingDeposits WHERE bdfoliocxc = @Number)
	SET @Result = 'Folio already exists in other Deposits'
END	
ELSE
BEGIN	
 IF EXISTS(SELECT bdfoliocxc FROM BookingDeposits WHERE bdfoliocxc = @Number AND bdgu <> @GuestID )
	SET @Result = 'Folio already exists in other Deposits'
END	

SELECT @Result

GO
