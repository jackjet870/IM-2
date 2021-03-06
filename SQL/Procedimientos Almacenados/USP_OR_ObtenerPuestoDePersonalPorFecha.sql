if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ObtenerPuestoDePersonalPorFecha]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ObtenerPuestoDePersonalPorFecha]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:			William Jesús Torres Flota
-- Procedimiento:	Obtener puesto de personal por fecha
-- Fecha:			05/Feb/2009
-- Descripción:		Devuelve el puesto de un empleado en una fecha determinada
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_ObtenerPuestoDePersonalPorFecha]	
	@PersonnelID varchar(10),	-- Clave del personal
	@Date datetime				-- Fecha
AS
	SET NOCOUNT ON;

declare @PostsLogCount int			-- Número de registros encontrados en el histórico de puestos
declare @PersonnelCount int			-- Número de registros encontrados en el catálogo de personal

-- Consulta el histórico de puestos
select
	@PostsLogCount = count(ppDT)
from PostsLog
where pppe = @PersonnelID and datediff(day, ppDT, @Date) >= 0

-- Si se encontró registros en el histórico de puestos
if @PostsLogCount > 0
	select
		pppo as Post,
		poN as PostN
	from PostsLog
	left join Posts on pppo = poID
	where pppe = @PersonnelID and ppDT = (
		select max(ppDT)
			from PostsLog
			where pppe = @PersonnelID and datediff(day, ppDT, @Date) >= 0
	)

-- Si NO se encontró registros en el histórico de puestos
else
begin
	-- Consulta el catálogo de personal
	select
		@PersonnelCount = count(peID)
	from Personnel
	where peID = @PersonnelID

	-- Si el personal existe
	if @PersonnelCount > 0
		select
			pepo as Post,
			poN as PostN
		from Personnel
		left join Posts on pepo = poID
		where peID = @PersonnelID
	
	-- Si el personal NO existe
	else
		select
			'' as Post,
			'' as PostN
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

