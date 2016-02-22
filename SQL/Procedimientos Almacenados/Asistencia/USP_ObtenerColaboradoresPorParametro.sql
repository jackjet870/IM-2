if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_ObtenerColaboradoresPorParametro]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_ObtenerColaboradoresPorParametro]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo CEDIS
**
** Obtiene colaboradores por parametros de busqueda
**
** [gmaya]	30/07/2014 Created
**
*/
CREATE PROCEDURE [dbo].[USP_ObtenerColaboradoresPorParametro]
	@Id varchar(11) = 'ALL',		-- Numero de colaborador
	@Name varchar(50)= 'ALL',		-- Nombre
	@Puesto varchar(50) = 'ALL',	-- Puesto
	@Locacion varchar(50) = 'ALL',	-- Locacion
	@Hotel varchar(50) = 'ALL' 	    -- Hotel


as
set nocount on

SELECT E.EMPLID AS EmpID, E.NOMBRE AS NombreCompleto, E.JOBDESCR AS Puesto, L.DESCRIPCION AS Locacion, H.NOMBRE AS Hotel
FROM  RHEMPDEP E
	  LEFT JOIN	LOCACION L ON L.LOCACION LIKE E.LOCATION
	  LEFT JOIN	HOTEL H ON H.NUMERO = E.HOTEL	
WHERE
	-- Numero de colaborador
	(@Id = 'ALL' or E.EMPLID LIKE '%' + @Id + '%') AND
	-- Nombre
	(@Name = 'ALL' or  E.NOMBRE LIKE '%' + @Name + '%') AND
	-- Puesto
	(@Puesto = 'ALL' or E.JOBDESCR LIKE '%' + @Puesto + '%') AND
	-- Locacion
	(@Locacion = 'ALL' or L.DESCRIPCION LIKE '%' + @Locacion + '%') AND
	-- Hotel
	(@Hotel = 'ALL' or H.NOMBRE LIKE '%' + @Hotel + '%')
ORDER BY E.NOMBRE

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO