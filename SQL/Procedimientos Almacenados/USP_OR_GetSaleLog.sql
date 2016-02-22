if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSaleLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSaleLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el registro historico de una venta
** 
** [wtorres]	17/Ago/2011 Created
** [wtorres]	23/Mar/2015 Modified. Agregue los nombres del personal, tipo de venta y tipo de membresia
** [wtorres]	18/Abr/2015 Modified. Agregue el nombre del huesped
**
*/
create procedure [dbo].[USP_OR_GetSaleLog] 
	@Sale int -- Clave de la venta
as
set nocount on;

select 
	L.slChangedBy,
	C.peN as ChangedByN,
	L.slID,
	L.slgu,
	dbo.UFN_OR_GetFullName(G.guFirstName1, G.guLastName1) as GuestName,
	L.slD,
	L.slProcD,
	L.slCancelD,
	L.slMembershipNum,
	ST.stN,
	L.slReference,
	M.mtN,
	L.slOriginalAmount,
	L.slNewAmount,
	L.slGrossAmount,
	L.sllo,
	L.slls,
	L.slsr,
	L.slSelfGen,
	-- Vendedores
	L.slPR1,
	P1.peN as PR1N,
	L.slPR2,
	P2.peN as PR2N,
	L.slPR3,
	P3.peN as PR3N,
	L.slLiner1Type,
	L.slLiner1,
	L1.peN as Liner1N,
	L.slLiner2,
	L2.peN as Liner2N,
	L.slCloser1,
	C1.peN as Closer1N,
	L.slCloser2,
	C2.peN as Closer2N,
	L.slCloser3,
	C3.peN as Closer3N,
	L.slExit1,
	E1.peN as Exit1N,
	L.slExit2,
	E2.peN as Exit2N,
	L.slPodium,
	PO.peN as PodiumN,
	L.slVLO,
	VLO.peN as VLON,
	L.slPRCaptain1,
	PC1.peN as PR1CaptainN,
	L.slPRCaptain2,
	PC2.peN as PR2CaptainN,
	L.slLinerCaptain1,
	LC.peN as LinerCaptainN,
	L.slCloserCaptain1,
	CC.peN as CloserCaptainN,
	L.slCloser1P,
	L.slCloser2P,
	L.slCloser3P,
	L.slExit1P,
	L.slExit2P,
	L.slClosingCost,
	L.slOverPack
from SalesLog L
	left join Guests G on G.guID = L.slgu
	left join SaleTypes ST on ST.stID = slst
	left join MembershipTypes M on M.mtID = slmt
	left join Personnel C on L.slChangedBy = C.peID
	left join Personnel P1 on P1.peID = L.slPR1
	left join Personnel P2 on P2.peID = L.slPR2
	left join Personnel P3 on P3.peID = L.slPR3
	left join Personnel L1 on L1.peID = L.slLiner1
	left join Personnel L2 on L2.peID = L.slLiner2
	left join Personnel C1 on C1.peID = L.slCloser1
	left join Personnel C2 on C2.peID = L.slCloser2
	left join Personnel C3 on C3.peID = L.slCloser3
	left join Personnel E1 on E1.peID = L.slExit1
	left join Personnel E2 on E2.peID = L.slExit2
	left join Personnel PO on PO.peID = L.slPodium
	left join Personnel VLO on VLO.peID = L.slVLO
	left join Personnel PC1 on PC1.peID = L.slPRCaptain1
	left join Personnel PC2 on PC2.peID = L.slPRCaptain2
	left join Personnel LC on LC.peID = L.slLinerCaptain1
	left join Personnel CC on CC.peID = L.slCloserCaptain1
where L.slsa = @Sale
order by L.slID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

