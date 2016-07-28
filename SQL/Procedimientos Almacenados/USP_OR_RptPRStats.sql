if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptPRStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptPRStats]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las estadisticasdel modulo PRStats
** 
** [erosado]	01/Mar/2016 Created
** [lchairez]	18/Abr/2016 Modified. Agregué el parámetro @BasedOnPrLocation en la función UFN_OR_GetPRSales
** [lchairez]	18/Abr/2016 Modified. Agregué el parámetro @BasedOnPrLocation en la función UFN_OR_GetPRSalesAmount
**
*/
create procedure [dbo].[USP_OR_RptPRStats] 
	@DateFrom datetime,					-- Fecha desde
	@DateTo datetime,					-- Fecha hasta
	@LeadSources varchar(max) = 'ALL',	-- Clave de los Lead Sources
	@SalesRooms varchar(max) = 'ALL',	-- Clave de las salas de ventas
	@Countries varchar(max) = 'ALL',	-- Clave de los paises
	@Agencies varchar(max) = 'ALL',		-- Clave de las agencias
	@Markets varchar(max) = 'ALL'		-- Clave de los mercados
as
set nocount on;

SELECT
	-- PR ID
	D.PR AS 'PR_ID',
	-- PR Names
	P.peN AS 'PR_NAME',
	-- Assigns
	Sum(Assigns) AS Assign,
	-- Contacts
	SUM(D.Contacts) AS Conts,
	-- Contacts Factor (Contacts / Assigns)
	dbo.UFN_OR_SecureDivision(SUM(D.Contacts),SUM(D.Assigns)) AS 'C_Factor',
	-- Availables
	SUM(D.Availables)AS Avails,
	-- Availables Factor (Availables / Contacts)
	dbo.UFN_OR_SecureDivision(SUM(D.Availables), SUM(D.Contacts))  AS 'A_Factor',
	-- Bookings Netos (Sin Directas)
	SUM(D.GrossBooks) AS Bk,
	-- Bookings Factor (Books / Availables)
	dbo.UFN_OR_SecureDivision(SUM(D.Books), SUM(D.Availables)) AS 'Bk_Factor',
	-- Deposits (Bookings)
	SUM(D.Deposits) AS Dep,
	-- 	Directs (Bookings)
	SUM(D.Directs) AS Dir,
	-- Shows Netos (Shows WithOut Directs Without In & Outs)
	SUM(D.GrossShows) AS Sh,
	--	In & Outs (Shows)
	SUM(D.InOuts) AS 'IO',
	-- Shows Factor (Shows / Bookings Netos)
	dbo.UFN_OR_SecureDivision(SUM(D.Shows), SUM(D.GrossBooks)) AS 'Sh_Factor',
	-- Total Shows
	SUM(D.Shows) AS 'TSh',
	-- Self Gen Tours (Guests)
	SUM(D.SelfGenShows) AS SG,
	-- Processable Number
	SUM(D.ProcessableNumber) AS	'Proc_Number',	
	-- Processable Amount
	SUM(D.ProcessableAmount) - SUM(D.OutPendingAmount) AS Processable,
	-- Out Pending Number 
	SUM(D.OutPendingNumber) AS	'OutP_Number',
	-- Out Pending Amount 
	SUM(D.OutPendingAmount) AS	'Out_Pending',
	-- Cancelled Number 
	SUM(D.CancelledNumber) AS	'C_Number',
	-- Cancelled Amount 
	SUM(D.CancelledAmount) AS	'Cancelled',
	-- Total Number
	SUM(D.ProcessableNumber) AS 'Total_Number',
	-- Total Amount
	SUM(D.ProcessableAmount) AS Total,
	-- Proc PR Number
	SUM(D.ProcessableNumber) - SUM(D.SelfGenNumber) AS 'Proc_PR_Number',
	-- Proc PR Amount
	SUM(D.ProcessableAmount) - SUM(D.SelfGenAmount) AS 'Proc_PR',
	-- Proc SG Number(ConsidererSelfGen=1)	
	SUM(D.SelfGenNumber)AS 'Proc_SG_Number',
	-- Proc SG Amount (ConsidererSelfgen=1)
	SUM(D.SelfGenAmount)AS 'Proc_SG',
	-- Efficient
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableAmount),SUM(D.Shows)) AS Eff,
	-- Clossing Factor
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableNumber),SUM(D.Shows)) AS 'Cl_Factor',
	-- Canceladas Factor
	dbo.UFN_OR_SecureDivision(SUM(D.CancelledAmount),SUM(D.ProcessableAmount)) AS 'Ca_Factor',
	-- Avg Sale
	dbo.UFN_OR_SecureDivision(SUM(D.ProcessableAmount),SUM(D.ProcessableNumber)) AS 'Avg_Sale'
FROM(
	-- Asignaciones
	SELECT 
	PR							/*1*/
	--	PR Name Join Personnel
	,Assigns					/*2*/
	,0 AS Contacts				/*3*/
	-- Contacts Factor	
	,0 AS Availables			/*4*/
	-- Availables Factor
	,0 AS GrossBooks			/*5*/
	-- Bookings Factor
	,0 AS Books					/*6*/
	,0 AS Deposits				/*7*/
	-- Shows Factor
	,0 AS Directs				/*8*/
	,0 AS GrossShows			/*9*/
	,0 AS InOuts				/*10*/
	,0 AS Shows					/*11*/
	,0 AS SelfGenShows			/*12*/
	,0 AS ProcessableNumber		/*13*/
	,0 AS ProcessableAmount		/*14*/
	,0 AS OutPendingNumber		/*15*/
	,0 AS OutPendingAmount		/*16*/
	,0 AS CancelledNumber		/*17*/
	,0 AS CancelledAmount		/*18*/
	-- Total Number
	-- Total Amount 	
	,0 AS SelfGenNumber			/*19*/
	,0 AS SelfGenAmount			/*20*/
	-- Efficient Factor
	-- Closing Factor
	-- Cancelled Factor
	-- Avg Sales
	
	FROM dbo.UFN_OR_GetPRAssigns(@DateFrom, @DateTo, @LeadSources, @SalesRooms, @Countries, @Agencies, @Markets)
	-- Contacts
	UNION ALL
	SELECT PR,0,Contacts,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRContacts(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Availables
	UNION ALL
	SELECT PR,0,0,Availables,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRAvailables(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Bookings Netos (Sin Directas)
	UNION ALL
	SELECT PR,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT, DEFAULT,0
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Bookings
	UNION ALL
	SELECT PR,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT, DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Depositos
	UNION ALL
	SELECT PR,0,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,1,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Directos
	UNION ALL
	SELECT PR,0,0,0,0,0,0,Books,0,0,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRBookings(@DateFrom, @DateTo, @LeadSources, DEFAULT, DEFAULT, DEFAULT,DEFAULT,1
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Shows Netos
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,0
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- In & Outs
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0,0
	FROM UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Shows
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Self Gen Shows
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,Shows,0,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRShows(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Processable 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Amount Processable 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT,DEFAULT, DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Number Out Of Pending 
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)	
	-- Amount Out Of Pending	
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Cancelled
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0,0,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Amount Cancelled
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount,0,0
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Number Self Gen
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,Sales,0
	FROM dbo.UFN_OR_GetPRSales(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	-- Amount Self Gen
	UNION ALL
	SELECT PR,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,SalesAmount
	FROM dbo.UFN_OR_GetPRSalesAmount(@DateFrom, @DateTo, @LeadSources,DEFAULT,DEFAULT,1,DEFAULT,DEFAULT,DEFAULT,DEFAULT
	,DEFAULT,DEFAULT,DEFAULT,@SalesRooms,@Countries,@Agencies,@Markets)
	
)AS D
	LEFT JOIN Personnel P ON D.PR = P.peID
GROUP BY PR, P.peN
ORDER BY P.peN
