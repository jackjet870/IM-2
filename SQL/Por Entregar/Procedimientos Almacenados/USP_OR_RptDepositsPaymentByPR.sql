USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_OR_RptDepositsPaymentByPR]    Script Date: 09/27/2016 10:51:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de pago de depositos a los PRs (Comisiones de PRs de Outhouse)
**
** [alesanchez]		19/Nov/2013 Created
** [gmaya]			11/Ago/2014 Modified. Agregue el parametro de formas de pago
** [lormartinez]	22/Sep/2014 Modified. Se agrega parametro para filtro basado en fecha de booking
** [lormartinez]	15/Dic/2015 Modified. Se modifica parametro de formas de pago.
**								Se envia forma de pago en funciones para filtrado
** [erosado]		04/Mar/2016 Modified. Se agrego parametro @SelfGen en la funcion UFN_OR_GetPRShows
**								Agregue los parametros @SalesRooms, @Countries, @Agencies y @Markets a las funciones UFN_OR_GetPRContacts,
**								UFN_OR_GetPRAvailables, UFN_OR_GetPRBookings, UFN_OR_GetPRShows, UFN_OR_GetPRSales y UFN_OR_GetPRSalesAmount
** [lchairez]		18/Abr/2016 Modified. Agregue el parametro @BasesOnPRLocation en la función UFN_OR_GetPRSalesAmount
** [wtorres]		12/May/2016 Modified. Correccion de error. No estaba ordenando correctamente los datos del reporte. Elimine la consulta de "Lugares de Pago"
**								Elimine la columna tpShow
** [erosado]		27/09/2016	Modified. Se cambio el nombre del campo ToPay -> Cxc y se corrigió el error de la formula cuando se trata de pago diferente a CC
**										  
*/
ALTER procedure [dbo].[USP_OR_RptDepositsPaymentByPR]
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(8000),			-- Claves de Lead Sources
	@PRs varchar(8000) = 'ALL',			-- Claves de PRs
	@Program varchar(10) = 'ALL',		-- Clave de programa
	@PaymentTypes varchar(MAX) = 'ALL',	-- Claves de formas de pago
	@FilterDeposit tinyint = 0			-- Filtro de depositos:
										--		0. Sin filtro
										--		1. Con deposito (Deposits)
										--		2. Sin deposito (Flyers)
										--		3. Con deposito y shows sin deposito (Deposits & Flyers Show)
as
set nocount on

declare	@FilterDepositAlternate tinyint	-- Filtro de depositos alterno (Aplica para bookings)

set @FilterDepositAlternate = @FilterDeposit
-- si se desea el reporte con deposito y shows sin deposito (Deposits & Flyers Show)
if @FilterDeposit = 3
	set @FilterDeposit = 0	-- Sin filtro

select
	'TO RETAIN' AS Category,
	-- Esquema de pago
	Cast('' as varchar(50)) as PaymentSchema,
	-- Porcentaje de show del esquema de pago
	Cast(0 as money) as PaymentSchemaFactor,
	-- Clave del PR
	T.PR,
	-- Nombre del PR
	IsNull(P.peN, 'CODE NOT FOUND!') as PRN,
	-- Bookings
	Sum(T.Books) as Books,
	-- In & Outs
	Sum(T.InOuts) as InOuts,
	-- Bookings netos
	Sum(T.GrossBooks) as GrossBooks,
	-- Porcentaje de show factor
	[dbo].UFN_OR_SecureDivision(Sum(T.GrossShows), Sum(T.GrossBooks)) as ShowsFactor, 
	-- Shows netos
	Sum(T.GrossShows) as GrossShows,
	-- Sales Amount
	Sum(T.SalesAmount) SalesAmount,
	-- Eficiency
	[dbo].UFN_OR_SecureDivision(Sum(T.SalesAmount), Sum(T.GrossBooks)) as Efficiency
into #PRs 
from (
	-- Bookings
	select PR, Books, 0 as GrossBooks, 0 as InOuts, 0 as GrossShows, 0 as SalesAmount
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, default, 0,@PaymentTypes, default, default, default, default)
	-- Bookings netos
	union all
	select PR, 0, Books, 0, 0, 0
	from UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDepositAlternate, 0, 0, 0, @PaymentTypes, default, default, default, default)
	-- In & Outs
	union all
	select PR, 0, 0, Shows, 0, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 1, default, default, default, default,
		default, 1, @PaymentTypes, default, default, default, default, default)
	-- Shows netos
	union all
	select PR, 0, 0, 0, Shows, 0
	from UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, @FilterDeposit, 0, 0, default, default, default, default,
		default, 1, @PaymentTypes, default, default, default, default, default)
	-- Monto de ventas
	union all
	select PR, 0, 0, 0, 0, SalesAmount
	from dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources, @PRs, @Program, default, default, default, @FilterDeposit, default, default, 1, default, default, default, default, default)
) AS T 
	left join Personnel P on T.PR = P.peID
group by T.PR, P.peN
order by ShowsFactor desc, GrossShows desc, Books desc

-- actualizamos la categoria (TO RETAIN o TO PAY) y el esquema de pago
;WITH cte AS
(SELECT P.*,
	IsNull(PAS.pasShowFactor, 0) as pasShowFactor, 
	IsNull(Pas.pasN, '') as pasN
FROM #PRs P
	OUTER APPLY (
		SELECT pasShowFactor / 100 as pasShowFactor, pasN
		FROM PaymentSchemas
		WHERE P.GrossShows between pasCoupleFrom AND pasCoupleTo
	) Pas
)
update cte
set PaymentSchemaFactor = pasShowFactor,
    PaymentSchema = pasN,
    Category = CASE WHEN ShowsFactor >= pasShowFactor THEN 'TO PAY' ELSE Category END

-- Depositos
select
	P.Category,
	C.pcN,
	P.PaymentSchema,
	P.PaymentSchemaFactor,
	P.PR, 
	P.PRN,
	G.guID,
	dbo.UFN_OR_GetFullName( G.guLastName1, g.gufirstname1) [guName],
	G.guAge1, 
	G.guBookD, 
	G.guShowD,
	G.guOutInvitNum,
	G.guHotel,
	G.guls, 
	G.gusr, 
	D.bdpt, 
	D.bdcu,
	D.bdAmount,
	D.bdReceived,
	case 
	when D.bdpt = 'CC' and P.Category = 'TO PAY' 
	then dbo.UFN_OR_GetTotalReceivedRound(D.bdAmount - D.bdReceived, 3.33)
	else D.bdAmount - D.bdReceived
	end as CxC
into #Deposits
from #PRs P
	inner join Guests G on G.guPRInvit1 = P.PR
	inner join BookingDeposits D on D.bdgu = G.guID 
	left join PaymentPlaces C on C.pcID = D.bdpc
where
	-- Fecha de booking
	G.guBookD between @DateFrom and @DateTo
	-- No asisten al show
	and G.guShow = 0
	-- Lead Sources
	and (@LeadSources = 'ALL' or G.guls  in (select item from split(@LeadSources, ','))) 
	-- Formas de pago
	and (@PaymentTypes = 'ALL' or D.bdpt  in (select item from split(@PaymentTypes, ',')))

-- 1. PRs
-- =============================================
select
	P.Category,
	P.PaymentSchema,
	P.PaymentSchemaFactor,
	P.PR,
	P.PRN,
	P.Books,
	P.InOuts,
	P.GrossBooks,
	P.ShowsFactor, 	     
	P.GrossShows,
	P.SalesAmount,
	P.Efficiency
from #PRs P
	inner join #Deposits D ON D.PR = P.PR
group by P.Category, P.PaymentSchema, P.PaymentSchemaFactor, P.PR, P.PRN, P.books, P.InOuts, P.GrossBooks,P.ShowsFactor, P.GrossShows, P.SalesAmount, P.Efficiency
order by P.Category, P.PaymentSchemaFactor, P.PRN

-- 2. Depositos
-- =============================================
select * from #Deposits
order by Category, pcN, PaymentSchemaFactor, PRN

-- 3. Monedas
-- =============================================
select distinct C.cuID, C.cuN
from #Deposits D
	left join Currencies C on D.bdcu = C.cuID

-- 4. Formas de Pago
-- =============================================
select distinct T.ptID, T.ptN 
from #Deposits D
	left join PaymentTypes T on D.bdpt = T.ptID