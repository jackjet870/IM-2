if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptWhsMovs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptWhsMovs]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE procedure [dbo].[sprptWhsMovs]
	@StartD datetime,
	@endD datetime,
	@WH0 varchar(10)
as
set nocount on

select 
    wmD,
    wmQty, 
    giN,
	wmpe,
	peN,
    wmComments
from 
    WhsMovs inner join Gifts on wmgi = giID
	left outer join Personnel on wmpe = peID
where 
    wmwh = @WH0
    and wmD between @StartD and @endD
order by 
    wmd, giN



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

