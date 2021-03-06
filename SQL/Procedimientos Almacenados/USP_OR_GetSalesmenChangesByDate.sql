USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSalesmenChangesByDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSalesmenChangesByDate]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los cambios de vendedores de un rango de fechas
** 
** [wtorres]		19/Ago/2015 Created
** [edgrodriguez]	05/Oct/2016 Modified. Se agregó el filtro de tipo de movimiento.
**
*/
CREATE procedure [dbo].[USP_OR_GetSalesmenChangesByDate]
	@DateFrom datetime,	-- Fecha desde
	@DateTo datetime,	-- Fecha hasta
	@MovementType varchar(10)='SL' --Tipo de Movimiento.
as
set nocount on

IF @MovementType='SL'
Begin
	select
		-- Fecha y hora
		C.schDT as [DateTime],
		
		-- Venta
		C.schsa as SaleID,
		S.saMembershipNum as Membership,
		S.saD as SaleDate,
		SR.srN as SalesRoom,
		
		-- Autorizado por
		C.schAuthorizedBy as AuthorizedBy,
		A.peN as AuthorizedByName,
		
		-- Hecho por
		C.schMadeBy as MadeBy,
		M.peN as MadeByName,
		
		-- Rol y posicion
		R.roN as [Role],
		C.schPosition as Position,
		
		-- Antiguo vendedor
		C.schOldSalesman as OldSalesman,
		O.peN as OldSalesmanName,
		
		-- Nuevo vendedor
		C.schNewSalesman as NewSalesman,
		N.peN as NewSalesmanName
	from SalesmenChanges C
		left join Sales S on S.saID = C.schsa
		--left join Guests G
		left join SalesRooms SR on SR.srID = S.sasr
		--left join SalesRooms SR on SR.srID = G.sasr
		left join Roles R on R.roID = C.schro
		left join Personnel A on A.peID = C.schAuthorizedBy
		left join Personnel M on M.peID = C.schMadeBy
		left join Personnel O on O.peID = C.schOldSalesman
		left join Personnel N on N.peID = C.schNewSalesman
	where
		-- Fecha del cambio
		Convert(varchar, C.schDT, 112) between @DateFrom and @DateTo
		And C.schmt ='SL'
	order by SR.srN, S.saMembershipNum, C.schDT
END
ELSE
BEGIN
	select
		-- Fecha y hora
		C.schDT as [DateTime],
		
		--Show		
		G.guID,
		G.guLastName1,
		G.guFirstName1,
		G.guShow,
		G.guShowD,
		SR.srN,		
		
		-- Autorizado por
		C.schAuthorizedBy as AuthorizedBy,
		A.peN as AuthorizedByName,
		
		-- Hecho por
		C.schMadeBy as MadeBy,
		M.peN as MadeByName,
		
		-- Rol y posicion
		R.roN as [Role],
		C.schPosition as Position,
		
		-- Antiguo vendedor
		C.schOldSalesman as OldSalesman,
		O.peN as OldSalesmanName,
		
		-- Nuevo vendedor
		C.schNewSalesman as NewSalesman,
		N.peN as NewSalesmanName
	from SalesmenChanges C
		left join Guests G on C.schgu = G.guID
		left join SalesRooms SR on SR.srID = G.gusr
		left join Roles R on R.roID = C.schro
		left join Personnel A on A.peID = C.schAuthorizedBy
		left join Personnel M on M.peID = C.schMadeBy
		left join Personnel O on O.peID = C.schOldSalesman
		left join Personnel N on N.peID = C.schNewSalesman
	where
		-- Fecha del cambio
		(Convert(varchar, C.schDT, 112) between @DateFrom and @DateTo)
		-- Si es Show
		And C.schmt ='SH'
	order by SR.srN, G.guID, C.schDT
END