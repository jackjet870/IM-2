if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ValidateInvitation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ValidateInvitation]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Valida que los datos de una invitacion existan
** 
** [wtorres]	23/Ago/2011 Optimizado
** [wtorres]	04/Jul/2013 Segunda optimizacion
**
*/
create procedure [dbo].[USP_OR_ValidateInvitation]
	@ChangedBy varchar(10),		-- Clave del usuario que esta haciendo el cambio
	@PR varchar(10),			-- Clave del PR
	@Location varchar(10),		-- Clave de la locacion
	@LeadSource varchar(10),	-- Clave del Lead Source
	@SalesRoom varchar(10),		-- Clave de la sala de ventas
	@Agency varchar(35),		-- Clave de la agencia
	@Country varchar(25)		-- Clave del pais
as
set nocount on

declare
	@Focus varchar(20),		-- Control que tendra el foco en caso de error
	@Message varchar(100)	-- Mensaje de error

-- validamos que el usuario que esta haciendo el cambio tenga permiso
select @Focus = case when Focus = 'ID' then 'ChangedBy' else Focus end, @Message = Message
from UFN_OR_ValidateUser(@ChangedBy, NULL, 'LS', @LeadSource, NULL, 0)

-- si no hubo error
if @Focus = '' begin
	
	-- si el que esta haciendo el cambio no es el PR
	if @ChangedBy <> @PR

		-- validamos que el PR tenga permiso
		select @Focus = case when Focus = 'ID' then 'PR' else Focus end, @Message = Message
		from UFN_OR_ValidateUser(@PR, NULL, 'LS', @LeadSource, 'PR', 0)

	-- si no hubo error
	if @Focus = '' begin

		-- validamos la locacion
		if (select Count(*) from Locations where loID = @Location and lols = @LeadSource) = 0
			select @Focus = 'Location', @Message = 'Location does not exist or does not belong to this Lead Source'

		-- validamos la sala de ventas	
		else if (select Count(*) from SalesRooms where srID = @SalesRoom) = 0
			select @Focus = 'SalesRoom', @Message = 'Sales Room does not exist'

		-- validamos la agencia
		else if (select  Count(*) from Agencies where agID = @Agency) = 0
			select @Focus = 'Agency', @Message = 'Agency does not exist'

		-- validamos el pais
		else if (select Count(*) from Countries where coID = @Country) = 0
			select @Focus = 'Country', @Message = 'Country does not exist'
	end
end

select @Focus as Focus, @Message as Message

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

