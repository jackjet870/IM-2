if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveSaleLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveSaleLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Guardar histórico de venta
-- Descripción:		Agrega un registro en el histórico de una venta si su información relevante cambió
-- Histórico:		[wtorres] 01/Jun/2010 Optimización
-- =============================================
create procedure [dbo].[USP_OR_SaveSaleLog]
	@Sale int,				-- Clave de la venta
	@HoursDif smallint,		-- Horas de diferencia
	@ChangedBy varchar(10)	-- Clave del usuario que está haciendo el cambio
as
set nocount on

declare @Count int

-- Determina si cambió algún campo relevante
select @Count = Count(*)
from SalesLog 
	inner join Sales on slsa = saID
where
	slsa = saID
	and (slMemberShipNum = saMemberShipNum or (slMemberShipNum is null and saMemberShipNum is null))
	and (slgu = sagu or (slgu is null and sagu is null))
	and (slst = sast or (slst is null and sast is null))
	and (slReference = saReference or (slReference is null and saReference is null))
	and (slmt = samt or (slmt is null and samt is null))
	and (slOriginalAmount = saOriginalAmount or (slOriginalAmount is null and saOriginalAmount is null))
	and (slNewAmount = saNewAmount or (slNewAmount is null and saNewAmount is null))
	and (slGrossAmount = saGrossAmount or (slGrossAmount is null and saGrossAmount is null))
	and (slD = saD or (slD is null and saD is null))
	and (slProcD = saProcD or (slProcD is null and saProcD is null))
	and (slCancelD = saCancelD or (slCancelD is null and saCancelD is null))
	and (sllo = salo or (sllo is null and salo is null))
	and (slls = sals or (slls is null and sals is null))
	and (slsr = sasr or (slsr is null and sasr is null))
	and (slPR1 = saPR1 or (slPR1 is null and saPR1 is null))
	and (slSelfGen = saSelfGen or (slSelfGen is null and saSelfGen is null))
	and (slPR2 = saPR2 or (slPR2 is null and saPR2 is null))
	and (slPR3 = saPR3 or (slPR3 is null and saPR3 is null))
	and (slPRCaptain1 = saPRCaptain1 or (slPRCaptain1 is null and saPRCaptain1 is null))
	and (slPRCaptain2 = saPRCaptain2 or (slPRCaptain2 is null and saPRCaptain2 is null))
	and (slLiner1Type = saLiner1Type or (slLiner1Type is null and saLiner1Type is null))
	and (slLiner1 = saLiner1 or (slLiner1 is null and saLiner1 is null))
	and (slLiner2 = saLiner2 or (slLiner2 is null and saLiner2 is null))
	and (slLinerCaptain1 = saLinerCaptain1 or (slLinerCaptain1 is null and saLinerCaptain1 is null))
	and (slCloser1 = saCloser1 or (slCloser1 is null and saCloser1 is null))
	and (slCloser2 = saCloser2 or (slCloser2 is null and saCloser2 is null))
	and (slCloser3 = saCloser3 or (slCloser3 is null and saCloser3 is null))
	and (slExit1 = saExit1 or (slExit1 is null and saExit1 is null))
	and (slExit2 = saExit2 or (slExit2 is null and saExit2 is null))
	and (slCloserCaptain1 = saCloserCaptain1 or (slCloserCaptain1 is null and saCloserCaptain1 is null))
	and (slPodium = saPodium or (slPodium is null and saPodium is null))
	and (slVLO = saVLO or (slVLO is null and saVLO is null))
	and (slCloser1P = saCloser1P or (slCloser1P is null and saCloser1P is null))
	and (slCloser2P = saCloser2P or (slCloser2P is null and saCloser2P is null))
	and (slCloser3P = saCloser3P or (slCloser3P is null and saCloser3P is null))
	and (slExit1P = saExit1P or (slExit1P is null and saExit1P is null))
	and (slExit2P = saExit2P or (slExit2P is null and saExit2P is null))
	and (slClosingCost = saClosingCost or (slClosingCost is null and saClosingCost is null))
	and (slOverPack = saOverPack or (slOverPack is null and saOverPack is null))
	and slID in (select Max(slID) from SalesLog where slsa = @Sale)

-- Agrega un registro en el histórico, si cambió algún campo relevante
insert into SalesLog
select
	DateAdd(hh, @HoursDif, GetDate()),
	saID,
	saMemberShipNum,
	sagu,
	sast,
	saReference,
	samt,
	saOriginalAmount,
	saNewAmount,
	saGrossAmount,
	saD,
	saProcD,
	saCancelD,
	salo,
	sals,
	sasr,
	saPR1,
	saSelfGen,
	saPR2,
	saPR3,
	saPRCaptain1,
	saPRCaptain2,
	saLiner1Type,
	saLiner1,
	saLiner2,
	saLinerCaptain1,
	saCloser1,
	saCloser2,
	saCloser3,
	saExit1,
	saExit2,
	saCloserCaptain1,
	saPodium,
	saVLO,
	saCloser1P,
	saCloser2P,
	saCloser3P,
	saExit1P,
	saExit2P,
	saClosingCost,
	saOverPack,
	@ChangedBy
from Sales
where saID = @Sale and @Count = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

