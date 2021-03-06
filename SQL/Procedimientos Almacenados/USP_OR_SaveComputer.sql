if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveComputer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveComputer]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*** Palace Resorts** Grupo de Desarrollo Palace**** Agrega una computadora si no existe en el catalogo de computadoras** ** [wtorres]	03/Jun/201 Created** [wtorres]	04/Dic/2014 Modified. Aumente el ancho del parametro @ComputerName para soportar los nombres de computadoras Mac***/
create procedure [dbo].[USP_OR_SaveComputer]
	@ComputerName varchar(63),	-- Nombre de la computadora
	@IPAddress varchar(15)		-- Dirección IP de la computadora
as
set nocount on

-- si no existe la computadora
if (select Count(*) from Computers where cpID = @ComputerName) = 0

	-- agregamos la computadora
    insert into Computers (cpID, cpN, cpIPAddress)
	values (@ComputerName, @ComputerName, @IPAddress)

-- si existe la computadora
else

	-- actualizamos la direccion IP de la computadora
	update Computers
	set cpIPAddress = @IPAddress
	where cpID = @ComputerName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

