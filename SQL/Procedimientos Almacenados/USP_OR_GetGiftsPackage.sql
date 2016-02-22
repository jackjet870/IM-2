if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGiftsPackage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGiftsPackage]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Consulta los items de un paquete de regalos
** 
** [wtorres]	15/Sep/2014 Created
**
*/
create procedure [dbo].[USP_OR_GetGiftsPackage]
	@Package varchar(10)	-- Clave del paquete de regalos
as
set nocount on

select P.gpPack, P.gpQty, P.gpgi
from GiftsPacks P
	inner join Gifts G on G.giID = P.gpgi
where
	-- Paquete
	P.gpPack = @Package
	-- Items activos
	and G.giA = 1
order by G.giN
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

