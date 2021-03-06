if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AgregarVendedor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AgregarVendedor]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Agregar Vendedor
-- Fecha:			21/10/2008
-- Descripción:		Agrega un vendedor
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_AgregarVendedor]	
	@SalesRoomID varchar(10),	-- Clave de la sala
	@TeamID varchar(10),		-- Clave del equipo
	@PostID varchar(10),		-- Clave del puesto
	@PersonnelID varchar(10)	-- Clave del personal	
AS
	SET NOCOUNT ON;

UPDATE Personnel SET
	peTeamType = 'SA',
	pePlaceID = @SalesRoomID,
	peTeam = @TeamID,
	pepo = @PostID
WHERE peID = @PersonnelID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

