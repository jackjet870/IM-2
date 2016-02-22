if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetNotices]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetNotices]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el catalogo de avisos
** 
** [wtorres]	05/Sep/2011 Creado
**
*/
create procedure [dbo].[USP_OR_GetNotices] 
	@LeadSource varchar(10),	-- Clave del Lead Source
	@Date datetime				-- Fecha
as
set nocount on;

select N.noTitle, N.noText
from Notices N
	inner join NoticesByLeadSource L on L.nlno = N.noID
where
	-- Lead Source
	L.nlls = @LeadSource
	-- Fecha
	and @Date between N.noStartD and N.noEndD
	-- Activos
	and noA = 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

