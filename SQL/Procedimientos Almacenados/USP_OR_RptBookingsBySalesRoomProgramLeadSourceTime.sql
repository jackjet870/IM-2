if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el numero de bookings por sala de ventas, programa, Lead Source y hora
** 
** [lormartinez]	26/Ago/2014 Creado
**
*/
create procedure [dbo].[USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime]
	@Date datetime,						-- Fecha desde
	@SalesRooms varchar(8000) = 'ALL'	-- Claves de salas de ventas
as
set nocount on

-- Bookings
-- =============================================
select
	S.srN as SalesRoom,
	P.pgN as Program,
	L.lsN [LeadSource],
	case when G.guRef is null then 'Bookings' else 'Rebookings' end as BookType,
	IsNull(Cast(Convert(varchar(5), G.guBookT, 114) as varchar(7)), 'No Time') as [Time],
	Count(*) as Books
into #tblData
from Guests G 
	inner join SalesRooms S on G.gusr = S.srID
	inner join LeadSources L on G.guls = L.lsID
	inner join Programs P on L.lspg = P.pgID
where
	-- Fecha de booking
	G.guBookD = @Date
	-- Sala de ventas
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Booking no cancelado
	and G.guBookCanc = 0
group by S.srN, P.pgN, G.guBookT, G.guRef

-- Reschedules
-- =============================================
union all
select
	S.srN,
	P.pgN,
	L.lsN,
	'Reschedules',
	IsNull(Cast(Convert(varchar(5), G.guReschT, 114) as varchar(7)), 'No Time'),
	Count(*)
from Guests G
	inner join SalesRooms S on G.gusr = S.srID
	inner join LeadSources L on G.guls = L.lsID
	inner join Programs P on L.lspg = P.pgID
where
	-- Fecha de reschedule
	G.guReschD = @Date
	-- Sala de ventas
	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))
	-- Booking no cancelado
	and G.guBookCanc = 0
group by S.srN, P.pgN, G.guReschT
order by SalesRoom, Program, BookType, [Time]

-- Datos del reporte
select * from #tblData

-- Horarios
select distinct [Time] from #tblData order by [Time]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

