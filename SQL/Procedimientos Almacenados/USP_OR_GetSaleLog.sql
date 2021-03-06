USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_OR_GetSaleLog]    Script Date: 10/13/2016 12:30:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
** [erosado]	13/10/2016	Modified. Se agregaron las nuevas posiciones Liner3, FTM 1 y 2, FTB 1 y 2, Closer4, ExitCloser3
**
*/
ALTER procedure [dbo].[USP_OR_GetSaleLog] 
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
	L.slLiner3,
	L3.peN as Liner3N,
	L.slFTM1,
	FTM1.peN as FTM1N,
	L.slFTM2,
	FTM2.peN as FTM2N,
	L.slFTB1,
	FTB1.peN as FTB1N,
	L.slFTB2,
	FTB2.peN as FTB2N,
	L.slCloser1,
	C1.peN as Closer1N,
	L.slCloser2,
	C2.peN as Closer2N,
	L.slCloser3,
	C3.peN as Closer3N,
	L.slCloser4,
	C4.peN as Closer4N,
	L.slExit1,
	E1.peN as Exit1N,
	L.slExit2,
	E2.peN as Exit2N,
	L.slExit3,
	E3.peN as Exit3N,
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
	left join Personnel L3 on L3.peID = L.slLiner3
	left join Personnel FTM1 on FTM1.peID = L.slFTM1
	left join Personnel FTM2 on FTM2.peID = L.slFTM2
	left join Personnel FTB1 on FTB1.peID = L.slFTB1
	left join Personnel FTB2 on FTB2.peID = L.slFTB2
	left join Personnel C4 on C4.peID = L.slCloser4
	left join Personnel E3 on E3.peID = L.slExit3
	
where L.slsa = @Sale
order by L.slID


