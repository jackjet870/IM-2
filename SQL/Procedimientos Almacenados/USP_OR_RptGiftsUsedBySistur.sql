if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptGiftsUsedBySistur]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptGiftsUsedBySistur]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de regalos usados como promociones en Sistur
** 
** [wtorres]	07/Nov/2015 Created
** [wtorres]	13/Nov/2015 Modified. Agregue los campos de costo de los regalos y diferencia entre el costo de Origos vs el precio de Sistur
** [wtorres]	20/Nov/2015 Modified. Agregue el campo de CeCos
** [emoguel]    10/10/2016 Modified. Se agregó el campo SisturPrice
*/
create procedure [dbo].[USP_OR_RptGiftsUsedBySistur]
	@DateFrom DateTime,					-- Fecha desde
	@DateTo DateTime,					-- Fecha hasta
	@SalesRooms varchar(8000) = 'ALL',	-- Claves de las salas de ventas
	@Programs varchar(8000) = 'ALL',	-- Claves de los programas
	@LeadSources varchar(8000) = 'ALL',	-- Claves de los Lead Sources
	@DateBasedOn int=0					-- tipo de filtro por fecha, 0.-Fecha del recibo | 1.-Fecha del cupon de sistur
as
set nocount on
SET FMTONLY OFF
SELECT tb.* into #GiftsUsedBySistur FROM
	(select S.srN,
		PG.pgN,
		L.lsN,
		R.grID,
		R.grD,
		R.grCancel,
		R.grCancelD,
		R.grExchange,
		G.guID,
		G.guHReservID,
		G.guOutInvitNum,
		G.guLastName1,
		G.guFirstName1,
		-- Datos del regalo
		D.geQty,
		GI.giQty,
		GI.giID,
		GI.giN,
		IsNull(GI.giPVPPromotion, D.gePVPPromotion) as Promotion,
		D.geQty * GI.giQty as Granted,
		D.geCancelPVPPromo,
		-- Datos del cupon de Sistur
		P.gspUsed,
		P.gspCoupon,
		P.gspD,
		P.gspPax,
		P.gspAmount,
		D.gePriceA + D.gePriceM as Cost,
		0 as [Difference],
		'' as Cecos,
		--Sistur Price
		CASE
			---Si es tipo descuento
			WHEN GI.giDiscount=1 THEN 0
			--Si es tipo Monetary
			WHEN GI.giMonetary=1 THEN (P.gspAmount/GI.giAmount) * GI.giPrice1
			--Si no es monetario
			ELSE ISNULL(P.gspUsed,0) * ((gePriceA + D.gePriceM)/GI.giQty) end		
		as sisturCost
			
	from GiftsReceipts R
	left join SalesRooms S on S.srID = R.grsr
	left join LeadSources L on L.lsID = R.grls
	left join Programs PG on PG.pgID = L.lspg
	left join Guests G on G.guID = R.grgu
	left join LeadSources LG on LG.lsID = G.guls
	left join SalesRooms SG on SG.srID = G.gusr
	inner join GiftsReceiptsC D on D.gegr = R.grID
	left join Gifts GI on GI.giID = D.gegi
	left join GuestsSisturPromotions P on
		P.gspHotel = case
			-- Outhouse
			when LG.lspg = 'OUT' then SG.srPropertyOpera
			-- Externos
			when G.guHReservID is null then LG.lsPropertyOpera
			-- Inhouse
			else G.gulsOriginal end
		and P.gspFolio = case
			-- Outhouse y Externos
			when LG.lspg = 'OUT' or G.guHReservID is null then R.grID
			-- Inhouse
			else G.guHreservID end
		and P.gspPromotion = IsNull(GI.giPVPPromotion, D.gePVPPromotion)
	where
		
		-- Fecha
		--R.grD between @DateFrom and @DateTo
		--Filtro por fecha del recibo
		(( @DateBasedOn=0 and R.grD between @DateFrom and @DateTo)
		--Filtro por fecha del cupon de sistur
		or (@DateBasedOn=1 and P.gsPD between @DateFrom and @DateTo))
				
		-- Regalo dado en Sistur
		and D.geInPVPPromo = 1
		-- Sala de ventas
		and (@SalesRooms = 'ALL' or R.grsr in (select item from Split(@SalesRooms, ',')))
		-- Programas
		and (@Programs = 'ALL' or L.lspg in (select item from Split(@Programs, ',')))
		-- Lead Sources
		and (@LeadSources = 'ALL' or R.grls in (select item from Split(@LeadSources, ','))))
	
	as tb
	--Declaramos las variables del cursor
	DECLARE @giID VARCHAR(100),
			@grID INT,
			@gspCoupon INT,
			@oldgiID VARCHAR(100),
			@oldgrID INT
			
	--Declaramos el cursor
	DECLARE cursorGifts CURSOR 
	For			
	select tb.giID,tb.grID,tb.gspCoupon from #GiftsUsedBySistur tb 
	WHERE tb.grID IN (
       -- Claves de recibos
       SELECT DISTINCT RC.grID FROM (
             -- Recibos que tienen regalos consumidos por mas de un cupon de Sistur
             select tb.grID, tb.giID, Count(*) as c
             from #GiftsUsedBySistur tb
             GROUP BY tb.grID, tb.giID
             HAVING Count(*) > 1 ) AS RC)
	order by tb.srN, tb.pgN, tb.lsN, tb.grD, tb.grID


	--Abrimos el cursor
	open cursorGifts
	
	--Leemos la primera fila
	FETCH FROM cursorGifts
	INTO @giID,@grID,@gspCoupon
	
	WHILE (@@fetch_status=0)
	BEGIN
		IF(@giID=@oldgiID and @grID=@oldgrID)
		UPDATE #GiftsUsedBySistur SET Cost=0 WHERE grID=@grID and giID=@giID and gspCoupon=@gspCoupon
					
					
		SET @oldgiID=@giID
		SET @oldgrID=@grID
		
		FETCH FROM cursorGifts
		INTO @giID,@grID,@gspCoupon
	END
	
	--Cerramos el cursor
	CLOSE cursorGifts
	--Liberamos recursos
	DEALLOCATE cursorGifts
	--Actualizamos la diferencia en la tabla 
	UPDATE #GiftsUsedBySistur SET [Difference]=ISNULL(SisturCost,0)-ISNULL(Cost,0)
	
select tb.* from #GiftsUsedBySistur tb order by tb.srN, tb.pgN, tb.lsN, tb.grD, tb.grID



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO