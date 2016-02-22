if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta las ventas
** 
** [wtorres]	23/Mar/2015 Created
**
*/
create procedure [dbo].[USP_OR_GetSales]
	@Guest int = 0,						-- Clave del huesped
	@Sale int = 0,						-- Clave de la venta
	@Membership varchar(10) = 'ALL',	-- Clave de la membresia
	@Name varchar(40) = 'ALL',			-- Nombre
	@LeadSource varchar(10) = 'ALL',	-- Clave del Lead Source
	@SalesRoom varchar(10) = 'ALL',		-- Clave de la sala de ventas
	@DateFrom datetime = null,			-- Fecha desde
	@DateTo datetime = null				-- Fecha hasta
as
set nocount on

select S.saID, S.saMembershipNum, S.saD, ST.stN, S.saCancel
from Sales S
	left join SaleTypes ST on ST.stID = S.sast
where
	-- Huesped
	(@Guest = 0 or S.sagu in (
		select @Guest
		union all select gaAdditional from GuestsAdditional where gagu = @Guest))
	-- Venta
	and (@Sale = 0 or S.saID = @Sale)
	-- Membresia
	and (@Membership = 'ALL' or S.saMembershipNum like '%'+ @Membership + '%')
	-- Nombre
	and (@Name = 'ALL' or (S.saLastName1 like '%' + @Name + '%' or S.saFirstName1 like '%' + @Name + '%'
		or S.saLastName2 like '%' + @Name + '%' or S.saFirstName2 like '%' + @Name + '%'))
	-- Sala de ventas
	and (@SalesRoom = 'ALL' or S.sasr = @SalesRoom)
	-- Lead Source
	and (@LeadSource = 'ALL' or S.sals = @LeadSource)
	-- Fecha
	and (@DateFrom is null or S.saD between @DateFrom and @DateTo)
order by S.saD, S.saMembershipNum, S.saID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

