if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestsAssigned]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestsAssigned]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los huespedes asignados
** 
** [wtorres]	10/Nov/2011 Creado
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
**
*/
create procedure [dbo].[USP_OR_GetGuestsAssigned] 
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSource varchar(10),	-- Clave del Lead Source
	@PRs varchar(8000),			-- Claves de PRs
	@Markets varchar(8000)		-- Claves de mercados
as
set nocount on

select
	G.guID,
	G.guRoomNum,
	G.guLastName1,
	G.guag,
	A.agN,
	G.guMemberShipNum,
	G.guAvail
from Guests G
	left join Agencies A on G.guag = A.agID
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Lead Source
	and G.guls = @LeadSource
	-- PRs
	and G.guPRAssign in (select item from Split(@PRs, ','))
	-- Mercados
	and G.gumk in (select item from Split(@Markets, ','))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

