if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptAssignment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptAssignment]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de huespedes asignados
** 
** [wtorres]	10/Nov/2011 Creado
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
**
*/
create procedure [dbo].[USP_OR_RptAssignment]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@LeadSource varchar(10),	-- Clave del Lead Source
	@Markets varchar(8000)		-- Claves de mercados
as
set nocount on

select
	G.guID,
	G.guCheckInD,
	G.guCheckIn,
	G.guRoomNum,
	G.guLastName1,
	G.guFirstName1,
	G.guPRAssign,
	P.peN,
	G.guag,
	A.agN,
	G.guMemberShipNum,
	'' as Gross,
	'' as PR,
	'' as Liner,
	'' as Closer
from Guests G
	inner join Personnel P on G.guPRAssign = P.peID
	left join Agencies A on G.guag = A.agID
where
	-- Lead Source
	G.guls = @LeadSource
	-- Fecha de llegada
	and G.guCheckInD between @DateFrom and @DateTo
	-- Asignados
	and (G.guPRAssign is not null or G.guPRAssign <> '')
	-- Mercados
	and G.gumk in (select item from Split(@Markets, ','))
 order by G.guCheckInD, G.guPRInvit1, G.guLastName1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

