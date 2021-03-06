if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptTaxisIn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptTaxisIn]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos de reporte de taxis de llegada
** 
** [lchairez]	24/Oct/2013 Documentado y optimizado. Agregue el campo de comentarios de hostess
** [lchairez]   30/Oct/2013 Agregue el campo de tipo de show
** [wtorres]	16/Dic/2013 Cambie la descripcion de los tours regulares
*/
CREATE procedure [dbo].[USP_OR_RptTaxisIn]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL'	-- Claves de salas
as
set nocount on

select
	case
		when guTour = 1 then 'Regular Tour'
		when guCTour = 1 then 'Courtesy Tour'
		when guSaveProgram = 1 then 'Save Tour'
		when guInOut = 1 then 'In & Out'
		when guWalkOut = 1 then 'Walk Out'
		when guWithQuinella = 1 then 'With Quinella'
	end as ShowType,
	guID,
	guloInvit,
	guShowD,
	dbo.UFN_OR_GetFullName(guLastName1, guFirstName1) as guGuest,
	guPax,
	Floor(guPax) as Adults,
	Cast((guPax - Floor(guPax)) * 10 as int) as Minors,
	guHotel,
	guPRInvit1,
	guTaxiIn,
	guEntryHost,
	guWComments
from Guests
where
	-- Con monto de taxi de llegada
	guTaxiIn > 0
	-- Fecha de show
	and guShowD between @DateFrom and @DateTo
	-- Sala de ventas
	and (@SalesRooms = 'ALL' or gusr in (select item from Split(@SalesRooms, ',')))
order by ShowType, guShowD, guID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

