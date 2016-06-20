USE [OrigosVCPalace]
GO
/****** Object:  StoredProcedure [dbo].[USP_IM_RptDailyGiftSimple]    Script Date: 04/07/2016 16:35:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la cantidad de regalos por Sala de ventas.
** 
** [edgrodriguez]	07/Abr/2016 Creado
**
*/
ALTER PROCEDURE [dbo].[USP_IM_RptDailyGiftSimple]
@Date as datetime,
@SalesRooms varchar(8000) = 'ALL'
as
Begin
set nocount on
select 
		grCancel,
		grID,
		giN,
		giShortN,
		geQty,
		grlo
	from
		GiftsReceipts 
		inner join GiftsReceiptsC on grID=gegr
		inner join Gifts on gegi = giID
	where
		(@SalesRooms = 'ALL' or grsr in (select item from split(@SalesRooms, ',')))
		and grD=@Date
		and grCancel=0
		and (grct<> 'PR'
		and grct<> 'LINER'
		and grct<> 'CLOSER')
	order by
		gegi
	
	--select 
	--	cast(case when srGiftsRcptCloseD >= @Date then 1 else 0 end as bit) as GiftsClosed  
	--from SalesRooms
	--where
	-- (@SalesRooms = 'ALL' or srID in (select item from split(@SalesRooms, ',')))
End