if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptDailyGiftSimple]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptDailyGiftSimple]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

create procedure dbo.sprptDailyGiftSimple
	@Date as datetime,
	@SR as varchar(10),
	@SR1 as varchar(10)='',
	@SR2 as varchar(10)='',
	@SR3 as varchar(10)='',
	@SR4 as varchar(10)='',
	@SR5 as varchar(10)='',
	@SR6 as varchar(10)='',
	@SR7 as varchar(10)='',
	@SR8 as varchar(10)='',
	@SR9 as varchar(10)=''
	as
	set nocount on
	select 
		cast(case when srGiftsRcptCloseD >= @Date then 1 else 0 end as bit) as GiftsClosed  
	from SalesRooms 
	where srID = @SR
	select 
		grCancel,
		grID,
		gegi,
		giShortN,
		geQty,
		grlo
	from
		GiftsReceipts inner join GiftsReceiptsC on grID=gegr
		inner join Gifts on gegi = giID
	where
		(grsr=@SR
		or grsr=@SR1
		or grsr=@SR2
		or grsr=@SR3
		or grsr=@SR4
		or grsr=@SR5
		or grsr=@SR6
		or grsr=@SR7
		or grsr=@SR8
		or grsr=@SR9)
		and grD=@Date
		and grCancel=0
		and (grct<> 'PR'
		and grct<> 'LINER'
		and grct<> 'CLOSER')
	order by
		gegi
	
	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

