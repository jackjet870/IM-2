if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AgregarTarjetaCreditoInvitacion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AgregarTarjetaCreditoInvitacion]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:			William Jesús Torres Flota
-- Procedimiento:	Agregar tarjeta de crédito a invitación
-- Fecha:			30/Dic/2008
-- Descripción:		Agrega una tarjeta de crédito a una invitación
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_AgregarTarjetaCreditoInvitacion]
	@GuestID int,			-- Clave de la invitación
	@Quantity tinyint,		-- Cantidad de tarjetas de cédito
	@CreditCard varchar(10)	-- Tarjeta de crédito (Clave o descripción)
AS
	SET NOCOUNT ON;

declare @CreditCardID varchar(10)	-- Clave de la tarjeta de crédito
declare @Exists bit					-- Indica si la tarjeta de crédito ya existe en la invitación

-- Obtiene la clave de la tarjeta de crédito
select @CreditCardID =
	case
		-- AMERICAN EXPRESS
		when Substring(@CreditCard, 1, 1) = 'A' then 'AEX'
		-- DEBIT MASTERCARD
		when @CreditCard = 'CM' then 'CM'
		-- DEBIT VISA
		when @CreditCard = 'CV' then 'CV'
		-- DINNERS
		when Substring(@CreditCard, 1, 3) = 'DIN' then 'DIN'
		-- DISCOVER
		when Substring(@CreditCard, 1, 3) = 'DIS' then 'DIS'
		-- MBNA
		when Substring(@CreditCard, 1, 2) = 'MB' then 'MB'
		-- MASTERCARD
		when CharIndex('MC', @CreditCard) > 0 then 'MC'
		when CharIndex('MAS', @CreditCard) > 0 then 'MC'
		-- VISA
		when Substring(@CreditCard, 1, 1) = 'V' then 'V'
		else ''
	end
-- Si se debe agregar
if @CreditCardID <> ''
begin
	if @Quantity < 1
		set @Quantity = 1
	select @Exists = count(gdcc) from GuestsCreditCards where gdgu = @GuestID and gdcc = @CreditCardID
	-- Si ya existe la tarjeta de crédito en la invitación
	if @Exists > 0
		update GuestsCreditCards set gdQuantity = gdQuantity + @Quantity
		where gdgu = @GuestID and gdcc = @CreditCardID
	-- Si no existe la tarjeta de crédito en la invitación
	else
		-- Agrega la tarjeta de crédito
		insert into GuestsCreditCards (gdgu, gdQuantity, gdcc)
		select @GuestID, @Quantity, @CreditCardID
end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

