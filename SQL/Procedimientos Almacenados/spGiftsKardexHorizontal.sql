if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGiftsKardexHorizontal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGiftsKardexHorizontal]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

create procedure dbo.spGiftsKardexHorizontal
	@StartDate datetime,
	@EndDate datetime,
	@WH varchar(10)
	as
	set nocount 
	on
	declare
	@TStartDate datetime, /*sirve para ir al primer dia del mes de la fecha inicial*/
	@TEndDate as datetime /*sirve para ir al un dia antes del dia inicial*/
	/*******************movimientos en el rango*******************/
	--Entradas
	select 
		wmD as MovD,
		wmgi as MovGi,
		giN,
		sum(wmQty) as MovQty
	into #TMovs
	from WhsMovs inner join Gifts on wmgi = giID
	where wmD between @StartDate and @EndDate 
		and wmwh=@WH
		and giInven = 1
		and wmQty > 0 
	group by wmD, wmgi, giN
	--order by wmD
	--salidas
	insert into #TMovs(MovD,MovGi,giN,MovQty)
	(select 
		wmD,
		wmgi,
		giN,
		sum(wmQty)
	from WhsMovs inner join Gifts on wmgi = giID
	where wmD between @StartDate and @EndDate 
		and wmwh=@WH
		and giInven = 1
		and wmQty < 0 
	group by wmD, wmgi, giN)
	/* *******************recibos de regalos en el rango**********/
	--Salidas
	select 
		grD as RecD,  
		gegi as RecGi,
		giN,
		sum(geQty) as RecQty 
	into #TRecs 
	from GiftsReceipts inner join GiftsReceiptsC on grID=gegr 
		inner join Gifts on gegi=giID
	where grD between @StartDate and @EndDate
		and grWh=@WH
		and giInven = 1
		and grCancel=0
		and geQty > 0
	group by grD,gegi,giN 
	--Devoluciones
	insert into #TRecs(RecD,RecGi,giN,RecQty)
	(select 
		grD,  
		gegi,
		giN,
		sum(geQty)  
	from GiftsReceipts inner join GiftsReceiptsC on grID=gegr 
		inner join Gifts on gegi=giID
	where grD between @StartDate and @EndDate
		and grWh=@WH
		and giInven = 1	and grCancel=0
		and geQty < 0
	group by grD,gegi,giN )
	/********************************************************************************************************/
	
	/*select distinct glgi as Invgi,giN, cast (0 as int) as InvQty into #TInv
	from GiftsByLoc inner join Gifts on glgi = giID
	where giInven = 1 and giA = 1 and gllo = @WH*/
	
	select distinct giID as Invgi,giN, cast (0 as int) as InvQty into #TInv
	from Gifts where giInven = 1 and giA = 1 

	if (day(@StartDate)) > 1 
		/*procedimiento para sacar el inventario a hasta un dia antes de la fecha inicial.*/
		begin
			set @TStartDate = dateadd( day, day(@StartDate) * -1 + 1, @StartDate)
			set @TEndDate = dateadd( day, -1 , @StartDate)
	/**********************************************************************************************************/
			select 	gvgi as Invgi , gvQty as InvQty	into	#t1
			from	GiftsInventory
			where	gvD = @TStartDate and  gvwh = @WH
			
			select	 wmgi, sum(wmQty) as wmSumQty 	into	#t2
			from 	WhsMovs
			where	wmD between @TStartDate and @TEndDate and wmwh=@WH
			group by wmgi
			
			update	#t1 --actualiza el inventario de los regalos existentes, sumadole movimientos
			set	InvQty = InvQty + wmSumQty
			from	#t1 inner join #t2 on Invgi = wmgi
	
			insert into #t1(Invgi, InvQty) --Inserta los movimientos de los regalos inexistentes en el inventario
			select 	wmgi,
				case when (wmSumQty) > 0 then (wmSumQty) else (wmSumQty * -1)end 
			from #t2 where not exists (select Invgi from #t1)
			
			select	gegi, sum(geQty) as grSumQty 	into	#t3
			from 	GiftsReceipts inner join GiftsReceiptsC on grID = gegr
			where	grD between @TStartDate and @TEndDate and grwh = @WH and grCancel = 0
			group by gegi
			
			update	#t1--actualiza el inventario de los regalos existentes, restandole recibos
			set	InvQty = InvQty - grSumQty
			from 	#t1 inner join #t3 on Invgi = gegi
	
			insert into #t1(Invgi, InvQty)--Inserta los recibos de los regalos inexistentes en el inventario
			select 	gegi, 
				case when (grSumQty) > 0 then (grSumQty * -1 ) else (grSumQty)end   
			from #t3 where not exists(select Invgi from #t1)
	
			update #TInv set #TInv.InvQty = #t1.InvQty 
			from #TInv inner join #t1 on #TInv.Invgi = #t1.Invgi
	
			drop table #t1
			drop table #t2
			drop table #t3
	/*********************************************************************************************************/
		end
	else
		begin	
			update #TInv set InvQty = gvQty 
			from #TInv Left outer join GiftsInventory on gvgi = Invgi
			where gvwh = @WH and gvD = @StartDate
		end
	
	select 	cast(case when srGiftsRcptCloseD >= @EndDate then 1 else 0 end as bit) as GiftsClosed  
	from SalesRooms where srID = @WH
	select * from #TInv
	select * from #TMovs order by MovGi,MovD 
	select * from #TRecs order by RecGi, RecD

	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

