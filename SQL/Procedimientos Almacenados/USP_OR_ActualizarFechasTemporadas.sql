if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ActualizarFechasTemporadas]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ActualizarFechasTemporadas]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:			William Jesús Torres Flota
-- Procedimiento:	Actualizar fechas de temporadas
-- Fecha:			17/Feb/2009
-- Descripción:		Actualiza los rangos de fechas de las temporadas hasta el año actual
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_ActualizarFechasTemporadas]
	@CurrentYear int	-- Año actual
AS
	SET NOCOUNT ON;

declare
@Count int,
@Year int,
@LastYear int,
@i int

-- Determina si el año actual ha sido actualizado
set @Count = (select count(*) from SeasonsDates where Year(sdStartD) = @CurrentYear)
-- Si el año actual no ha si actualizado
if @Count = 0 
begin
	-- Obtiene el último año actualizado
	set @LastYear = (Select top 1 Year(sdStartD) from SeasonsDates order by sdStartD desc) 
	set @Year = @LastYear + 1
	set @i = 1
	-- Actualiza todos los años anteriores al año actual e inclusive al año acutl que no han sido
	-- actualizados
	while @Year <= @CurrentYear
	begin
		insert into SeasonsDates(sdss, sdStartD, sdEndD)
		select  sdss, DateAdd(Year, @i, sdStartD), DateAdd(Year, @i, sdEndD)
		from SeasonsDates where Year(sdStartD) = @LastYear 	
		set @Year = @Year + 1
		set @i = @i + 1
	end
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

