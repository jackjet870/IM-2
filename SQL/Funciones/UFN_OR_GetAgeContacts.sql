if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetAgeContacts]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetAgeContacts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de contactos por edad
** 
** [wtorres]	18/Oct/2010 Creado
** [wtorres]	18/Oct/2010 Agregue el parametro @BasedOnArrival
** [wtorres]	20/Oct/2011 Ahora no se cuentan los rebooks
**
*/
create function [dbo].[UFN_OR_GetAgeContacts](
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSources varchar(8000),	-- Claves de Lead Sources
	@BasedOnArrival bit = 0		-- Indica si se debe basar en la fecha de llegada
)
returns @Table table (
	Age tinyint,
	Contacts int
)
as
begin

insert @Table
select
	guAge1,
	Count(*)
from Guests
where
	-- Fecha de contacto
	((@BasedOnArrival = 0 and guInfoD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and guCheckInD between @DateFrom and @DateTo
	-- Contactado
	and guInfo = 1))
	-- No Rebook
	and guRef is null
	-- No Antes In & Out
	and guAntesIO = 0
	-- Lead Source
	and guls in (select item from split(@LeadSources, ','))
group by guAge1

return
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
