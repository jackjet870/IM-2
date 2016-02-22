if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetPRNotesByBossDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetPRNotesByBossDate]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene las notas de PR dado un patron y a partir de determinada fecha
** 
** [wtorres]	02/Dic/2011 Creado
**
*/
create procedure [dbo].[USP_OR_GetPRNotesByBossDate] 
	@Boss varchar(10),	-- Clave del patron
	@Date datetime		-- Fecha
as
set nocount on

select * from PRNotes
where pngu in (
	select N.pngu from PRNotes N
		inner join Guests G on N.pngu = G.guID
		inner join LeadSources L on G.guls = L.lsID
	where
		(L.lsBoss is null or L.lsBoss = @Boss)
		and (G.guCheckInD >= @Date or G.guBookD >= @Date)
	union
	select N2.pngu from PRNotes N2
		inner join GuestsAdditional A on A.gaAdditional = N2.pngu
		inner join Guests G2 on A.gagu = G2.guID
		inner join LeadSources L2 on G2.guls = L2.lsID
	where
		(L2.lsBoss is null or L2.lsBoss = @Boss)
		and (G2.guCheckInD >= @Date or G2.guBookD >= @Date)
)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

