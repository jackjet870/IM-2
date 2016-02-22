USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los datos para imprimir un cupon de comida
** 
** [lchairez]		07/Feb/2014 Created
** [lormartinez]	17/Sep/2014 Modified. Se agregan campos de adultos, menores, guid y salesroom
** [lormartinez]	10/Ago/2015 Modified. Se agregan columnas de reference y reference name para los datos de representative y collaborator
*/
CREATE procedure [dbo].[USP_OR_GetMealTicketByFolio]
	@meID AS integer,
	@Folio AS integer,
	@Authorized varchar(50)
as

select
	M.meType,
	MT.myN, 
	M.megu, 
	dbo.UFN_OR_GetFullName(G.guFirstName1, G.guLastName1) as Name, 
	M.meD,
	M.mepe, 
	@Authorized Authorized,
	@Folio as meFolio,
	G.gula,
	M.meAdults,
	M.meMinors,
	IsNull(G.guID,0) as guID,
	M.mesr as SalesRoomID,
	S.srN as SalesRoomName,
	case when r.raID = 1 then '' else R.raN end [raN],
	case when r.raID in (2, 3) then M.mepe 
		when r.raID = 4 then a.agID + ' - ' + a.agN
		end as REFERENCE,
	case when r.raID in (2, 3) then P.peN +' - ' + IsNull(P.peCollaboratorID, '')
		when r.raID = 4 then M.merep
		end as REFERENCENAME
from MealTickets M
	left join MealTicketTypes MT on MT.myID = M.meType
	left join Guests G on G.guID = M.megu
	left join Personnel P on P.peID = M.mepe
	left join Salesrooms S on S.srID = M.mesr
	left join RateTypes R on R.raID = M.mera
	left join Agencies A on A.agID = M.meag
where M.meID = @meID


GO