if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UFN_OR_FormatDeposit]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[UFN_OR_FormatDeposit]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Formatea un deposito con su moneda
** 
** [wtorres]	03/Jun/2011 Creado
**
*/
create function [dbo].[UFN_OR_FormatDeposit](
	@Deposit money,			-- Deposito
	@Currency varchar(10)	-- Moneda
)
returns varchar(8000)
as
begin

declare	@DepositFormatted varchar(8000) -- Deposito formateado

if @Deposit > 0
	set @DepositFormatted = dbo.FormatNumber(@Deposit, 0) + ' ' +  @Currency

return @DepositFormatted
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

