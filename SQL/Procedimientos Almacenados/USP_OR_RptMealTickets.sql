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
** Devuelve los datos para el reporte de cupones de comida
** 
** [wtorres]	16/Dic/2010 Modified. Ahora se pasa la lista de salas como un sólo parámetro y agregué el parámetro @Cancelled
** [wtorres]	05/Feb/2015 Modified. Dividi el campo Pax en adultos y menores
** [LorMartinez] 27/Nov/2015 Modified, Se agregan columnas de agencia y representante
                                      Se agregan filtros para ratetypes
*/
CREATE procedure [dbo].[USP_OR_RptMealTickets]
	@DateFrom datetime,			-- Fecha desde
	@DateTo datetime,			-- Fecha hasta
	@SalesRooms varchar(8000),	-- Claves de salas
	@Cancelled bit = 0,			-- Indica si se desean los recibos cancelados
  @RateTypes varchar(max) = 'ALL' -- Claves de RateTypes
as
set nocount  on

select
	M.meID,
	M.meD,
	M.megu, 
	M.meQty,
	T.myN,
	M.meAdults,
	M.meMinors,
	M.meTAdults + M.meTMinors as Total,
	M.meFolios,
	M.meComments,
	G.guLastName1,
	G.guloInfo,
	G.guEntryHost,
	G.guShow,
	H.peN as guEntryHostN,
	G.guPRInvit1,
	P.peN as guPRInvit1N,
	G.guLiner1,
	L.peN as guLiner1N,
	M.mera,
	R.raN as RateTypeN,
	M.mepe,
	C.peCollaboratorID,
	C.peN,
  a.agN,
  M.merep
from MealTickets M
	inner join MealTicketTypes T on T.myID = M.meType
	left join Guests G on G.guID = M.megu
	left join Personnel P on G.guPRInvit1 = P.peID
	left join Personnel H on G.guEntryHost = H.peID
	left join Personnel L on G.guLiner1 = L.peID
	left join Personnel C on M.mepe = C.peID
	left join RateTypes R on M.mera = R.raID
  left join dbo.Agencies a ON agID = M.meag
where
	-- Fecha de cupón
	M.meD between @DateFrom and @DateTo	
	-- Salas
	and M.mesr in (select item from Split(@SalesRooms, ','))	
	-- Cupones cancelados
	and M.meCanc = @Cancelled
  --RateTypes
  and (@RateTypes='ALL' OR (@RateTypes <> 'ALL' AND  M.mera in (select item from split(@RateTypes,','))))
order by M.meD, M.meFolios
GO