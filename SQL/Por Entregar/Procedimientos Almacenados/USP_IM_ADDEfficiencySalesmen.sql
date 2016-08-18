/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Agrega a la tabla de EfficiencySalesmen
**
** [ecanul] 18/08/2016 Created
**
*/

CREATE PROCEDURE [dbo].[USP_IM_ADDEfficiencySalesmen]
	@EfficiencyID int,		--ID de la eficiencya
	@SalemanID varchar(10)	-- ID del Personal a agregar
AS
INSERT INTO dbo.EfficiencySalesmen VALUES(@EfficiencyID,@SalemanID)