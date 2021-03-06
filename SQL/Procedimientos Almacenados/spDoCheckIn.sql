if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDoCheckIn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDoCheckIn]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

--Si marca error, se deberá ejecutar el comando DROP en caso de que ya exista el procedimiento.

create procedure dbo.spDoCheckIn
	@guls			varchar(10),
	@guHReservID	varchar(15),
	@guLastName1	varchar(25) = null,
	@guFirstName1	varchar(25) = null,
	@guRoomNum		varchar(15) = null,
	@guCCType		varchar(30) = null,
	@guBirthDate1	datetime = null,
	@guBirthDate2	datetime = null,
	@guBirthDate3	datetime = null,
	@guBirthDate4	datetime = null
AS
SET NOCOUNT ON

declare
	@guID		int,
	@guCheckIn	bit,
	@guAge1		tinyint,
	@guAge2		tinyint,
	@Year1		smallint,
	@Month1		tinyint,
	@Day1		tinyint,
	@Year2		smallint,
	@Month2		tinyint,
	@Day2		tinyint,
	@guAvail	bit,
	@guum		tinyint

select	@guID = isnull(guID, 0), 
		@guCheckIn = isnull(guCheckIn, 0),
		@guAvail = guAvail,
		@guum = guum 
from	OrigosVCPalace.dbo.Guests
Where	gulsOriginal = @guls and guHReservID = @guHReservID

if @guID > 0
begin
	if @guCheckIn = 0
		set @guAvail = case when (@guum = 0) then 1 else 0 end

	set @Year2 = year(getdate())
	set @Month2 = month(getdate())
	set @Day2 = day(getdate())

	if @guBirthDate1 is not null
	begin
		set @Year1 = year(@guBirthDate1)
		set @Month1 = month(@guBirthDate1)
		set @Day1 = day(@guBirthDate1)
		if @Month1 > @Month2 or (@Month1 = @Month2 and @Day1 > @Day2)
			set @guAge1 = @Year2 - @Year1 - 1
		else
			set @guAge1 = @Year2 - @Year1
	end

	if @guBirthDate2 is not null
	begin
		set @Year1 = year(@guBirthDate2)
		set @Month1 = month(@guBirthDate2)
		set @Day1 = day(@guBirthDate2)
		if @Month1 > @Month2 or (@Month1 = @Month2 and @Day1 > @Day2)
			set @guAge2 = @Year2 - @Year1 - 1
		else
			set @guAge2 = @Year2 - @Year1
	end

	update	OrigosVCPalace.dbo.Guests
	set		guCheckIn = 1,
			guAvail = @guAvail,
			guLastName1 = case when (@guLastName1 is null) then guLastName1 else @guLastName1 end,
			guFirstName1 = case when (@guFirstName1 is null) then guFirstName1 else @guFirstName1 end,
			guRoomNum = case when (@guRoomNum is null) then guRoomNum else @guRoomNum end,
			guCCType = case when (@guCCType is null) then guCCType else @guCCType end,
			guBirthDate1 = case when (@guBirthDate1 is null) then guBirthDate1 else @guBirthDate1 end, 
			guBirthDate2 = case when (@guBirthDate2 is null) then guBirthDate2 else @guBirthDate2 end, 
			guBirthDate3 = case when (@guBirthDate3 is null) then guBirthDate3 else @guBirthDate3 end, 
			guBirthDate4 = case when (@guBirthDate4 is null) then guBirthDate4 else @guBirthDate4 end, 
			guAge1 = case when (@guAge1 is null) then guAge1 else @guAge1 end, 
			guAge2 = case when (@guAge2 is null) then guAge2 else @guAge2 end 
	from	OrigosVCPalace.dbo.Guests
	where	guID = @guID
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

