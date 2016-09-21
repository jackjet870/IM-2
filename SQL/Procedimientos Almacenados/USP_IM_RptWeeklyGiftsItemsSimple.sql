/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene la cantidad de regalos por semana por Sala de ventas.
** 
** [edgrodriguez]	08/Abr/2016 Creado
**
*/
CREATE procedure [dbo].[USP_IM_RptWeeklyGiftsItemsSimple]
	@StartDate as datetime,
	@EndDate as datetime,
	@SalesRooms as varchar(8000)= 'ALL'
	as
	set nocount on
	select 
		grD,
		giN	as Gift,
		giShortN as ShortN,
		sum(geQty) as Qty
	from GiftsReceipts inner join GiftsReceiptsC on grID=gegr
		inner join Gifts on gegi = giID
	where
		(@SalesRooms = 'ALL' or grsr in (select item from split(@SalesRooms, ',')))
		and grD between @StartDate and @EndDate
		and gigc='ITEMS'
		and grCancel=0
		and (grct<> 'PR'
		and grct<> 'LINER'
		and grct<> 'CLOSER')
	group by grD, giN,giShortN
	order by giN
	
	select 
		cast(case when srGiftsRcptCloseD >= @EndDate then 1 else 0 end as bit) as GiftsClosed  
	from 
		SalesRooms 
	where
	(@SalesRooms = 'ALL' or srID in (select item from split(@SalesRooms, ',')))
	