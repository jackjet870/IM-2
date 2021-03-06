if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptAssignmentArrivals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptAssignmentArrivals]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de llegadas y su asignacion
** 
** [wtorres]	10/Nov/2011 Creado
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
**
*/
create procedure [dbo].[USP_OR_RptAssignmentArrivals]
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
	left join Personnel P on G.guPRAssign = P.peID
	left join Agencies A on G.guag = A.agID
where
	-- Disponibles
	G.guum = 0
	-- Lead Source
	and G.guls = @LeadSource
	-- Fecha de llegada
	and G.guCheckInD between @DateFrom and @DateTo
	-- Mercados
	and G.gumk in (select item from Split(@Markets, ','))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

