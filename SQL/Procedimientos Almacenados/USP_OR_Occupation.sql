if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_Occupation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_Occupation]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Ocupación
-- Descripción:		Devuelve el porcentaje de ocupación de un Lead Source en una fecha determinada
-- Histórico:		[wtorres] 28/Abr/2009 Agregue una variable tipo tabla en vez de una tabla temporal
--					[wtorres] 22/Sep/2009 Eliminé la variable tipo tabla
-- =============================================
create procedure [dbo].[USP_OR_Occupation] 
	@Date datetime,	-- Fecha
	@LS varchar(10)	-- Clave del Lead Source
as
set nocount on 

declare
@NGuests money,	-- Número de huéspedes
@NRooms money	-- Número de habitaciones

-- Obtiene el número de huéspedes
select @NGuests = Count(*)
from (
	select distinct guRoomNum
	from Guests
	where
		guls = @LS	
		and guCheckIn = 1
		and guCheckInD <= @Date 
		and guCheckOutD > @Date
		and guRoomNum is not null 
		and guRoomNum <> ''
		and guHReservID is not null
		and guHReservID <> ''
) as Guests

-- Obtiene el número de habitaciones
set @NRooms = IsNull((select lsRooms from LeadSources where lsID = @LS), 0)

-- Calcula el porcentaje de ocupación
select Occupation = Cast([dbo].UFN_OR_SecureDivision(@NGuests * 100, @NRooms) as varchar) + '%'

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

