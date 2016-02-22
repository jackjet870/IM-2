USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Inserta o modifica el último folio
**
** [lchairez]		12/Feb/2014	Created
** [lormartinez]	11/Ago/2015 Modified. Se agrega parametro @RateType, para uso de ratetype y se agregan validaciones con ratetype
**
*/
CREATE procedure [dbo].[USP_OR_UpdateMealTicketFolio]
	@SalesRoom varchar(10),
	@MealTicketType varchar(10),
	@Folio varchar(30),
	@RateType int
as

-- si existe el folio, lo actualizamos
if exists(select top 1 null from MealTicketFolios where mfsr = @SalesRoom and mfmy = @MealTicketType and mfra = @RateType)
	update MealTicketFolios 
	SET mfFolio = @Folio 
	WHERE mfsr = @SalesRoom and mfmy = @MealTicketType and mfra = @RateType

-- si no existe el folio, lo agregamos
else
	insert into MealTicketFolios (mfsr, mfmy, mffolio, mfra)
	values(@SalesRoom, @MealTicketType, @Folio, @RateType)
	

GO