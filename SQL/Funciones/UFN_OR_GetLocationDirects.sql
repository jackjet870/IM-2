if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetLocationDirects]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetLocationDirects]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Obtener directas de locación
-- Descripción:		Devuelve el número de directas por locación
-- Histórico:		[wtorres] 28/Sep/2009 Creado
-- =============================================
create function [dbo].[UFN_OR_GetLocationDirects](
	@DateFrom datetime,		-- Fecha desde
	@DateTo datetime,		-- Fecha hasta
	@SalesRoom varchar(10)	-- Clave de la sala de ventas
)
returns @Table table (
	Location varchar(10),
	Directs int
)
as
begin

insert @Table
select
	guloInvit as Location,
	Count(*) as Directs
from Guests
where
	-- Sala de ventas
	gusr = @SalesRoom
	-- Fecha de reservación
	and guBookD between @DateFrom and @DateTo
	-- Directas
	and guDirect = 1
group by guloInvit

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

