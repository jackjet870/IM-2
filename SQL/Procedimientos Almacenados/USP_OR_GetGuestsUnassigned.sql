if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestsUnassigned]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestsUnassigned]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el los huespedes no asignados
** 
** [wtorres]	10/Nov/2011 Creado
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
**
*/
create procedure [dbo].[USP_OR_GetGuestsUnassigned] 
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSource varchar(10),	-- Clave del Lead Source
	@Markets varchar(8000),		-- Claves de mercados
	@OnlyAvailables bit = 0		-- Indica si solo se desean los huespedes disponibles
as
set nocount on

select
	G.guID,
	G.guCheckInD,
	G.guCheckIn,
	G.guRoomNum,
	G.guLastName1,
	G.guag,
	A.agN,
	G.guMemberShipNum,
	G.guAvail,
	G.guComments
from Guests G
	left join Agencies A on G.guag = A.agID
where
	-- Fecha de llegada
	G.guCheckInD between @DateFrom and @DateTo
	-- Lead Source
	and G.guls = @LeadSource
	-- No asignados
	and (G.guPRAssign is null or G.guPRAssign = '')
	-- Disponibles
	and (@OnlyAvailables = 0 or G.guum = 0)
	-- Mercados
	and G.gumk in (select item from Split(@Markets, ','))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

