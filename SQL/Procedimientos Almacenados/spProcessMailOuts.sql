if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spProcessMailOuts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spProcessMailOuts]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE procedure [dbo].[spProcessMailOuts]
 @LeadSource varchar(5),
 @Date datetime = '19800101'
as
set nocount on
declare
 @moCode varchar(10),
 @moCCheckIn bit,
 @moCCheckInDFrom smallint, 
 @moCCheckInDTo smallint, 
 @moCCheckOutDFrom smallint, 
 @moCCheckOutDTo smallint, 
 @moCBookDFrom smallint, 
 @moCBookDTo smallint, 
 @CheckInDFrom datetime,  
 @CheckInDTo datetime,  
 @CheckOutDFrom datetime,
 @CheckOutDTo datetime,
 @BookDFrom datetime,
 @BookDTo datetime,
 @moCRoomNumFrom varchar(6),
 @moCRoomNumTo varchar(6),
 @moCInfo1 tinyint,
 @moCInfo2 tinyint,
 @moCInvit1 tinyint,
 @moCInvit2 tinyint,
 @moCBookCanc1 tinyint,
 @moCBookCanc2 tinyint,
 @moCShow bit,
 @moCSale bit,
 @moCOnGroup1 tinyint,
 @moCOnGroup2 tinyint,
 @UnavailMot1 tinyint,
 @UnavailMot2 tinyint,
 @moCMarket varchar(10),
-- @moCMarket2 varchar(10),
 @moCAgency varchar(35),
 @moCCountry varchar(25),
 @sql varchar(2000)
 --@where varchar(500)
declare csMailOuts insensitive cursor
for
 	select
 		moCode, 
 		moCCheckIn, 
 		moCCheckInDFrom, 
 		moCCheckInDTo, 
 		moCCheckOutDFrom, 
 		moCCheckOutDTo, 
 		moCBookDFrom, 
 		moCBookDTo, 
 		moCRoomNumFrom, 
 		moCRoomNumTo, 
 		moCInfo, 
 		moCInvit, 
 		moCBookCanc, 
 		moCShow, 
 		moCSale, 
 		moCOnGroup, 
 		moCMarket,
 		moCAgency,
		moCCountry
 	from 
		MailOuts
 	where
		mols = @LeadSource
  		and moA = 1
 	order by moOrder
for read only
if @Date = '19800101'
 	set @Date =  cast(convert(varchar, getdate(), 112) as datetime)
update 	Guests 
set 	gumo = Null
where 	gumo is not null and guls = @LeadSource
open csMailOuts
fetch next from csMailOuts into
  @moCode,
  @moCCheckIn, 
  @moCCheckInDFrom, 
  @moCCheckInDTo, 
  @moCCheckOutDFrom, 
  @moCCheckOutDTo, 
  @moCBookDFrom, 
  @moCBookDTo, 
  @moCRoomNumFrom,
  @moCRoomNumTo,
  @moCInfo1,
  @moCInvit1,
  @moCBookCanc1,
  @moCShow,
  @moCSale,
  @moCOnGroup1,
  @moCMarket,
  @moCAgency,
  @moCCountry
while @@fetch_status = 0
begin
 	if @moCInfo1 = 0
  		set @moCInfo2 = 0
 	else
  		if @moCInfo1 = 1
   			set @moCInfo2 = 1
  		else
   			select @moCInfo1 = 0, @moCInfo2 = 1
 	
	if @moCInvit1 = 0
  		set @moCInvit2 = 0
 	else
  		if @moCInvit1 = 1
   			set @moCInvit2 = 1
  		else
   			select @moCInvit1 = 0, @moCInvit2 = 1
 	
	if @moCBookCanc1 = 0
  		set @moCBookCanc2 = 0
 	else
  		if @moCBookCanc1 = 1
   			set @moCBookCanc2 = 1
  		else
   			select @moCBookCanc1 = 0, @moCBookCanc2 = 1
	
 	if @moCOnGroup1 = 0
  	begin
  		set @moCOnGroup2 = 0
  		set @UnavailMot1 = 0
  		set @UnavailMot2 = 0
  	end
 	else
  		if @moCOnGroup1 = 1
  		begin
   			set @moCOnGroup2 = 1
   			set @UnavailMot1 = 2
   			set @UnavailMot2 = 2
  		end
  		else
   			select @moCOnGroup1 = 0, @moCOnGroup2 = 1, @UnavailMot1 = 0, @UnavailMot2 = 2
	
 	set @CheckInDFrom = dateadd(d, @moCCheckInDFrom, @Date)
 	set @CheckInDTo = dateadd(d, @moCCheckInDTo, @Date)
 	set @CheckOutDFrom = dateadd(d, @moCCheckOutDFrom, @Date)
 	set @CheckOutDTo = dateadd(d, @moCCheckOutDTo, @Date)
	
	if @moCBookDFrom is not null
		select @BookDFrom = dateadd(d, @moCBookDFrom, @Date), @BookDTo = dateadd(d, @moCBookDTo, @Date)
	
	set @sql = 'update Guests set gumo = ' + char(39) + @moCode + char(39) + ' '
	set @sql = @sql + 'from Guests inner join Agencies on guag = agID '
	set @sql = @sql + 'where guCheckInD between ' + char(39) + convert(varchar, @CheckInDFrom, 112) + char(39)
	set @sql = @sql + ' and ' + char(39) + convert(varchar, @CheckInDTo, 112) + char(39) + ' '
	set @sql = @sql + 'and guCheckOutD between ' + char(39) + convert(varchar, @CheckOutDFrom, 112) + char(39)
	set @sql = @sql + ' and ' + char(39) + convert(varchar, @CheckOutDTo, 112) + char(39) + ' '
	set @sql = @sql + 'and guls = ' + char(39) + @LeadSource + char(39) + ' '
	set @sql = @sql + 'and guCheckIn = ' + cast(@moCCheckIn as varchar) + ' '
	set @sql = @sql + 'and (guInfo = ' + cast(@moCInfo1 as varchar) + ' or guInfo = ' + cast(@moCInfo2 as varchar) + ') '
	set @sql = @sql + 'and (guum = ' + cast(@UnavailMot1 as varchar) + ' or guum = ' + cast(@UnavailMot2 as varchar) + ') '
	set @sql = @sql + 'and guHReservID is NOT NULL '
	set @sql = @sql + 'and guHReservID <> ' + char(39) + char(39) + ' '
	set @sql = @sql + 'and guRoomNum between ' + char(39) + @moCRoomNumFrom + char(39) 
	set @sql = @sql + ' and ' + char(39) + @moCRoomNumTo + char(39) + ' '
	set @sql = @sql + 'and ((guAvail = 1 or guum = ' + cast(@UnavailMot1 as varchar) + ' or guum = ' + cast(@UnavailMot2 as varchar) + ') or (guCheckInD = ' 
	set @sql = @sql + char(39) + convert(varchar, dateadd(d, 1, @Date), 112) + char(39) + ')) '

	if @moCMarket <> 'ANY ONE'
		set @sql = @sql + 'and gumk = ' + char(39) + @moCMarket + char(39) + ' '
	
	if @moCAgency <> 'ANY ONE'
		set @sql = @sql + 'and guag = ' + char(39) + @moCAgency + char(39) + ' '
	
	if @moCCountry <> 'ANY ONE'
		set @sql = @sql + 'and guco = ' + char(39) + @moCCountry + char(39) + ' '
	
	if @moCBookDFrom is null
		set @sql = @sql + 'and guInvit = 0 '
	else
	begin
		--20071114 HC Evitar enviar Reschedule o reminder cuando sea un reschedule
		set @sql = @sql + 'and guResch = 0 '
		--20071114 HC Evitar enviar Reschedule o reminder cuando sea un reschedule
		set @sql = @sql + 'and (guBookCanc = ' + char(39) + convert(varchar, @moCBookCanc1, 112) + char(39) 
		set @sql = @sql + ' or guBookCanc = ' + char(39) + convert(varchar, @moCBookCanc2, 112) + char(39) + ') '
		set @sql = @sql + 'and guShow = ' + cast(@moCShow as varchar) + ' '
		set @sql = @sql + 'and guSale = ' + cast(@moCSale as varchar) + ' '
		if @moCInvit1 = 1 and @moCInvit2 = 1
		begin
			set @sql = @sql + 'and guBookD between ' + char(39) + convert(varchar, @BookDFrom, 112) + char(39)
			set @sql = @sql + ' and ' + char(39) + convert(varchar, @BookDTo, 112) + char(39) + ' '
			--set @sql = @sql + 'and (guBookCanc = ' + @moCBookCanc1 + ' or guBookCanc = ' + @moCBookCanc2 + ') '
		end
		else
		begin
			set @sql = @sql + 'and ((guInvit = 1 and guBookD between ' + char(39) + convert(varchar, @BookDFrom, 112) + char(39)
			set @sql = @sql + ' and ' + char(39) + convert(varchar, @BookDTo, 112) + char(39) + ') or guInvit = 0) '
			--set @sql = @sql + 'and (guBookCanc = ' + @moCBookCanc1 + ' or guBookCanc = ' + @moCBookCanc2 + ') '
		end
	end
	--print @sql
	execute (@sql)
fetch next from csMailOuts into
  @moCode,
  @moCCheckIn, 
  @moCCheckInDFrom, 
  @moCCheckInDTo, 
  @moCCheckOutDFrom, 
  @moCCheckOutDTo, 
  @moCBookDFrom, 
  @moCBookDTo, 
  @moCRoomNumFrom,
  @moCRoomNumTo,
  @moCInfo1,
  @moCInvit1,
  @moCBookCanc1,
  @moCShow,
  @moCSale,
  @moCOnGroup1,
  @moCMarket,
  @moCAgency,
  @moCCountry
end
close csMailOuts
deallocate csMailOuts




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

