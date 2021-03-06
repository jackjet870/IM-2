if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_DeleteGuestsGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_DeleteGuestsGroups]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Eliminar grupos de huéspedes
-- Descripción:		Elimina grupos de huéspedes y sus registros asociados
-- Histórico:		[wtorres] 22/Jul/2010 Creado
-- =============================================
create procedure [dbo].[USP_OR_DeleteGuestsGroups]
    @Groups varchar(8000)	-- Lista de claves de grupos de huéspedes
as
set nocount on

-- Marcar los huéspedes del grupo como que no pertenecen a ningún grupo
update Guests set guGroup = 0 where guID in (
	select gjgu from GuestsGroupsIntegrants where gjgx in (select Item from split(@Groups, ','))
)

-- Integrantes del grupo de huéspedes
delete from GuestsGroupsIntegrants where gjgx in (select Item from split(@Groups, ','))

-- Grupo de huéspedes
delete from GuestsGroups where gxID in (select Item from split(@Groups, ','))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

