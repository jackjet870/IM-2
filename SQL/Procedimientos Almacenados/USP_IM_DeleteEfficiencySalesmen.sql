/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Elimina los vendedores que tengan la eficiencia seleccionada
**
** [ecanul] 18/08/2016 Created
**
*/
CREATE PROCEDURE [dbo].[USP_IM_DeleteEfficiencySalesmen]
	@EfficiencyID int		--ID de la eficiencia a eliminar
AS
DELETE FROM dbo.EfficiencySalesmen 
WHERE esef = @EfficiencyID