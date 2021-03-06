if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptPremanifestOutside]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptPremanifestOutside]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de premanifiesto del modulo Outside
** 
** [wtorres]	22/Jul/2011 Creado
** [wtorres]	28/Ene/2012 Agregue el campo descripcion del pais
**
*/
create procedure  [dbo].[USP_OR_RptPremanifestOutside]
	@Date datetime,			-- Fecha
	@LeadSource varchar(10)	-- Clave del Lead Source
as
set nocount on

select
	S.srN,
	G.guID,
	G.guOutInvitNum,
	Cast(case when G.guDeposit > 0 or G.guDepositTwisted > 0 then 'DEPOSIT' else 'NO DEPOSIT' end as varchar) as Deposit,
	G.guHotel,
	G.guRoomNum,
	G.guLastName1,
	G.guFirstName1,
	G.guco,
	C.coN,
	G.guBookD,
	G.guBookT,
	G.guPRInvit1,
	G.guShow,
	G.guSale,
	dbo.AddStringLabel(dbo.AddStringLabel(dbo.AddStringLabel('', dbo.UFN_OR_GetGuestDepositsAsString(G.guID), '', 'Deposits: '), 
		dbo.UFN_OR_FormatDeposit(G.guDepositTwisted, G.gucu), ', ', 'Burned: '),
		G.guComments, '  |||  ', 'Comments: ') as guComments
from Guests G
	inner join SalesRooms S on G.gusr = S.srID
	left join Countries C on G.guco = C.coID
where
	-- Fecha de booking
	G.guBookD = @Date
	-- Lead Source
	and G.guls = @LeadSource
order by G.gusr, Deposit, G.guBookT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

