if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_GetPRBookings]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_GetPRBookings]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el numero de bookings por PR
** 
** [wtorres]		18/Sep/2009 Created
** [wtorres]		23/Sep/2009 Modified. Convertido a funcion. Agregue los parametros @LeadSources, @ConsiderAntesInOut, @ConsiderQuinellasSplit
** [wtorres]		27/Oct/2009 Modified. Agregue los parametros @ConsiderQuinellas y @FilterDeposit y elimine los parametros @ConsiderRoomsQty,
**								@ConsiderAntesInOut y @ConsiderQuinellasSplit
** [wtorres]		02/Ene/2010 Modified. Agregue los parametros @ConsiderDirects y @ConsiderInOuts
** [wtorres]		11/Mar/2010 Modified. Ahora ya no se restan los In & Outs que no estan en el rango de fechas definido
** [wtorres]		16/Abr/2010 Modified. Agregue los parametros @PRs y @Program
** [wtorres]		18/Nov/2010 Modified. Agregue el parametro @BasedOnArrival y reemplace el parametro @ConsiderDirects por @Direct
** [wtorres]		26/Nov/2010 Modified. Ahora no se cuentan los bookings cancelados
** [wtorres]		26/Nov/2013 Modified. Reemplace el parametro @ConsiderInOuts por @InOut para que el filtro de In & Outs sea mas simple
** [gmaya]			11/Ago/2014 Modified. Agregue el parametro de formas de pago
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [wtorres]		15/Jul/2015 Modified. Elimine el parametro @BasedOnBooking porque no es necesario
** [lormartinez]	15/Dic/2015 Modified. Se reimplementa filtro por tipos de pago 
** [erosado]		04/Mar/2016 Modified. Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets
** [edgrodriguez]	05/May/2016 Modified. Se agregaron parametros a la funcion UFN_OR_GetPercentageSalesman
**
*/
create function [dbo].[UFN_OR_GetPRBookings](
	@DateFrom datetime,				-- Fecha desde
	@DateTo datetime,				-- Fecha hasta
	@LeadSources varchar(max)='ALL',-- Clave de los Lead Sources
	@PRs varchar(8000) = 'ALL',		-- Claves de PRs
	@Program varchar(10) = 'ALL',	-- Clave de programa
	@ConsiderQuinellas bit = 0,		-- Indica si se debe considerar quinielas
	@FilterDeposit tinyint = 0,		-- Filtro de depositos:
									--		0. Sin filtro
									--		1. Con deposito (Deposits)
									--		2. Sin deposito (Flyers)
									--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
	@Direct int = -1,				-- Filtro de directas:
									--		-1. Sin filtro
									--		 0. No directas
									--		 1. Directas
	@InOut int = -1,				-- Filtro de In & Outs:
									--		-1. Sin filtro
									--		 0. No In & Outs
									--		 1. In & Outs
	@BasedOnArrival bit = 0,		-- Indica si se debe basar en la fecha de llegada,
	@PaymentTypes varchar(MAX) = 'ALL', --Filtro para tipos de pagos
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
  
)
returns @Table table (
	PR varchar(10),
	Books money
)
as
begin

declare @tmpPaymentTypes table(item varchar(20))

--Si hay PaymentTypes se llena la lista
IF @PaymentTypes <> 'ALL'
BEGIN
 INSERT INTO @tmpPaymentTypes
 SELECT ITEM FROM dbo.Split(@PaymentTypes,',')
END

insert @Table

-- Bookings (PR 1)
-- =============================================
select
	G.guPRInvit1,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 1
	G.guPRInvit1 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
	-- Filtro para tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount, 0) > 0 ))
	-- Sales Rooms	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))	-- Countries	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))	-- Agencies	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))	-- Markets	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit1
union all

-- Bookings (PR 2)
-- =============================================
select
	G.guPRInvit2,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 2
	G.guPRInvit2 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
	-- Filtro para tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount, 0) > 0 ))
	-- Sales Rooms	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))	-- Countries	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))	-- Agencies	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))	-- Markets	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit2
union all

-- Bookings (PR 3)
-- =============================================
select
	G.guPRInvit3,
	Sum(dbo.UFN_OR_GetPercentageSalesman(G.guPRInvit1, G.guPRInvit2, G.guPRInvit3, DEFAULT, DEFAULT) * case when @ConsiderQuinellas = 0 then 1 else G.guRoomsQty end)
from Guests G
	inner join LeadSources L on L.lsID = G.guls
	OUTER APPLY (
		SELECT count(bd.bdID) bdCount
		FROM BookingDeposits bd
		WHERE bd.bdgu = g.guid
			and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and bd.bdpt IN (select item from @tmpPaymentTypes)) )
	) bk
where
	-- PR 3
	G.guPRInvit3 is not null
	-- Fecha de booking
	and ((@BasedOnArrival = 0 and G.guBookD between @DateFrom and @DateTo)
	-- Fecha de llegada
	or (@BasedOnArrival = 1 and G.guCheckInD between @DateFrom and @DateTo
	-- Invitado
	and G.guInvit = 1))
	-- No Antes In & Out
	and G.guAntesIO = 0
	-- No Quiniela Split
	and (@ConsiderQuinellas = 0 or G.guQuinellaSplit = 0)
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls in (select item from split(@LeadSources, ','))) 
	-- PRs
	and (@PRs = 'ALL' or (G.guPRInvit1 in (select item from split(@PRs, ','))
		or G.guPRInvit2 in (select item from split(@PRs, ','))
		or G.guPRInvit3 in (select item from split(@PRs, ','))))
	-- Programa
	and (@Program = 'ALL' or L.lspg = @Program)
	-- Filtro de depositos
	and (@FilterDeposit = 0 or (@FilterDeposit = 1 and G.guDeposit > 0) or (@FilterDeposit = 2 and G.guDeposit = 0)
		or (@FilterDeposit = 3 and (G.guDeposit > 0 or (G.guShow = 1 and G.guDeposit = 0))))
	-- Directas
	and (@Direct = -1 or G.guDirect = @Direct)
	-- In & Outs
	and (@InOut = -1 or G.guInOut = @InOut)
	-- No bookings cancelados
	and G.guBookCanc = 0
	-- Filtro para tipos de pago
	and (@PaymentTypes ='ALL' or (@PaymentTypes <> 'ALL' and ISNULL(bk.bdCount, 0) > 0 ))
	-- Sales Rooms	and (@SalesRooms = 'ALL' or G.gusr in (select item from split(@SalesRooms, ',')))	-- Countries	and (@Countries = 'ALL' or G.guco in (select item from split(@Countries, ',')))	-- Agencies	and (@Agencies = 'ALL' or G.guag in (select item from split(@Agencies, ',')))	-- Markets	and (@Markets = 'ALL' or G.gumk in (select item from split(@Markets, ',')))
group by G.guPRInvit3

return
end