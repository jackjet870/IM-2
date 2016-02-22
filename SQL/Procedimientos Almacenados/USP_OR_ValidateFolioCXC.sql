
/* 
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que el rango de folios de invitaciones CXC que se desea agregar no exista en el catalogo
** 
** [bcc]	06/01/2015 Created
** 
*/
CREATE PROCEDURE [dbo].[USP_OR_ValidateFolioCXC]
	@NumberFrom INTEGER,	-- Numero inicial
	@NumberTo INTEGER,		-- Numero final
	@Active BIT,			-- Especifica si esta activo el rango de folios
	@Action INTEGER			-- Especifica la accion a realizar Insert(0) o Update(1)
AS
SET NOCOUNT ON

DECLARE @Result BIT
	
-- ponemos el resultado como verdadero
SET @Result = 1

-- si es un insert solo validamos que los numeros se encuentren en algun rango ya almacenado
IF @Action = 0 BEGIN

	-- validamos que el rango dado no exista en la tabla
	IF NOT EXISTS(
		SELECT *
		FROM FolioCXC
		WHERE fiA = @Active
			AND (@NumberFrom between fiFrom AND fiTo OR @NumberTo between fiFrom AND fiTo))
		
		SET @Result = 1
	ELSE
		SET @Result = 0
END

-- si es una actualizacion debemos realizar varias validaciones para que no se repitan folios
ELSE BEGIN

	-- validamos que el numero inicial no se encuentre en mas de 1 rango, si es asi no es posible realizar el update
	IF(SELECT Count(*) FROM FolioCXC WHERE fiA = @Active
		AND (fiFrom < @NumberFrom AND fiTo > @NumberFrom)) > 1
		
		SET @Result = 0
	
	-- validamos que el numero final no se encuentre en mas de 1 rango, si es asi no es posible realizar el update
	IF(SELECT Count(*) FROM FolioCXC WHERE fiA = @Active
		AND (fiFrom < @NumberTo AND fiTo > @NumberTo)) > 1
			
		SET @Result = 0
			
	-- validamos que los numeros no se encuentren en mas de 1 rango, si es asi no es posible realizar el update
	IF(SELECT Count(*) FROM FolioCXC WHERE fiA = @Active
		AND ((@NumberFrom between fiFrom AND fiTo OR @NumberTo between fiFrom AND fiTo)
		OR (fiFrom >= @NumberFrom AND fiTo <= @NumberTo))) > 1
			
		SET @Result = 0
END

SELECT @Result



GO