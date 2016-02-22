if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DateSerial]') and xtype in (N'FN', N'if', N'TF'))
drop function [dbo].[DateSerial]
GO

set QUOTED_IDENTIFIER ON 
GO
set ANSI_NULLS ON 
GO

-- =============================================
-- Función:		Generar fecha
-- Descripción:	Genera una fecha
-- Histórico:	[wtorres] 04/Feb/2010 Creado
-- =============================================
create function [dbo].[DateSerial](
	@Year smallint,	-- Año
	@Month tinyint,	-- Mes
	@Day tinyint	-- Día
) 
returns datetime
as 
begin

declare	@Result datetime

set @Result = Cast( dbo.Pad(@Year, '0', 4, 0) + dbo.Pad(@Month, '0', 2, 0) + dbo.Pad(@Day, '0', 2, 0) as datetime)
return @Result
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO