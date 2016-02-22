if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetNoticesByBoss]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetNoticesByBoss]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene los avisos dado un patron
** 
** [wtorres]	01/Dic/2011 Creado
**
*/
create procedure [dbo].[USP_OR_GetNoticesByBoss] 
	@Boss varchar(10)	-- Clave del patron
as
set nocount on

select * from Notices
where noID in (
	select N.noID from Notices N
		inner join NoticesByLeadSource NL on NL.nlno = N.noID
		inner join LeadSources L on NL.nlls = L.lsID
	where L.lsBoss is null or L.lsBoss = @Boss
)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

