if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_EsFrontToMiddle]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_EsFrontToMiddle]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Es Front To Middle
-- Fecha:			15/10/2008
-- Descripción:		Determina si un personal es Front To Middle
--					Un Front To Middle es un Guest Service que tiene clave de liner para poder tourear.
--					Si se envía alguna clave de PR se determina si es Front To Middle pero de esos determinados
--					PR's.
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_EsFrontToMiddle]	
	@PersonnelID varchar(10),	-- Clave del personal
	@PR1 varchar(10) = NULL,	-- Clave del PR 1 (Opcional)
	@PR2 varchar(10) = NULL,	-- Clave del PR 2 (Opcional)
	@PR3 varchar(10) = NULL		-- Clave del PR 3 (Opcional)
AS
	SET NOCOUNT ON;

declare @Count int			-- Número de registros encontrados

-- Si NO se enviaron las claves de PR
if @PR1 is null and @PR2 is null and @PR3 is null
	-- Se determina si es un Front To Middle en general
	select
		@Count = count(peID)
	from Personnel
	where peLinerID = @PersonnelID

-- Si se envío alguna clave de PR
else
	-- Si se envío la clave del PR 1
	if @PR1 is not null
		begin
			select
				@Count = count(peID)
			from Personnel
			where peLinerID = @PersonnelID and peID = @PR1
			
			-- Si no encontró la clave de FTM y se envío la clave del PR 2
			if @Count = 0 and @PR2 is not null
				begin
					select
						@Count = count(peID)
					from Personnel
					where peLinerID = @PersonnelID and peID = @PR2

					-- Si no encontró la clave de FTM y se envío la clave del PR 3
					if @Count = 0 and @PR3 is not null
						select
							@Count = count(peID)
						from Personnel
						where peLinerID = @PersonnelID and peID = @PR3
				end
		end

--Devuelve el resultado
select @Count as [Count]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

