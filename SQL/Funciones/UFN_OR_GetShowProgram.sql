if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetShowProgram]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetShowProgram]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el programa de show de un huesped
** 
** [wtorres]	08/Ago/2011 Creado
** [wtorres]	12/Oct/2011 Ajustes en la clave de tour normal (N). Ahora es regular (R).
**
*/
create function [dbo].[UFN_OR_GetShowProgram](
	@SaveProgram bit,	-- Indica si es del programa de rescate
	@CourtesyTour bit,	-- Indica si es un tour de cortesia
	@InOut bit,			-- Indica si es un In & Out
	@Appointment bit	-- Indica si es una cita
)
returns varchar(10)
as
begin
declare @ShowProgram varchar(10)

select
	@ShowProgram = case
		when @SaveProgram = 1 then 'S'
		when @CourtesyTour = 1 then 'CT'
		when @InOut = 1 then 'IO'
		when @Appointment = 1 then 'A'
		else 'R' end

return @ShowProgram
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

