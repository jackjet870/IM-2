if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetReportText]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetReportText]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene un texto de un reporte
** 
** [wtorres]	19/Jun/2014 Created
**
*/
create function [dbo].[UFN_OR_GetReportText](
	@Report varchar(50),	-- Clave del reporte
	@TextId varchar(50),	-- Clave del texto
	@Language varchar(2)	-- Clave del idioma
)
returns varchar(max)
as
begin

declare @Text varchar(max)

select @Text = reText
from ReportsTexts
where
	reReport = @Report
	and reTextId = @TextId
	and rela = @Language

return @Text
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

