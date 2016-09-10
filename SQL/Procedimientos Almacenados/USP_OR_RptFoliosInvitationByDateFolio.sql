if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptFoliosInvitationByDateFolio]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de Folios Invitations Outhouse)
**
** [lormartinez]	28/Ago/2014 Created
** [wtorres]		08/May/2015 Modified. Agregue la fecha de booking
** [lormartinez]	08/Ene/2015 Modified.Agregue los parametros @LeadSources y @PRs
**
*/
CREATE procedure [dbo].[USP_OR_RptFoliosInvitationByDateFolio]
	@DateFrom datetime = null,	-- Fecha desde
	@DateTo datetime = null,	-- Fecha hasta
	@Serie varchar(5) = 'ALL',	-- Serie
	@FolioFrom integer = 0,		-- Folio desde
	@FolioTo integer = 0,		-- Folio hasta,
	@LeadSources varchar(MAX) ='ALL', --Lista de LeadSources
	@PRs varchar(MAX) ='ALL' --Lista de PRs
as
set nocount on
 
select
	G.guOutInvitNum,
	G.guPRInvit1 as PR,
	P.peN as PRN, 
	G.guLastName1,
	G.guBookD,
	L.lsN
from Guests G
	left join LeadSources L on G.guls = L.lsID
	left join Personnel P on G.guPRInvit1 = P.peID
	outer apply (select Substring(G.guOutInvitNum, CharIndex('-', G.guOutInvitNum) + 1, Len(G.guOutInvitNum) - CharIndex('-', G.guOutInvitNum)) as Folio ) F
where
	-- Programa Outhouse
	L.lspg = 'OUT'
	-- Serie
	and (@Serie = 'ALL' or G.guOutInvitNum like @Serie + '-%')
	-- Rango de folios
	and ((@FolioFrom = 0 and @FolioTo = 0) 
		or (@FolioFrom = 0 and (@FolioTo > 0 and F.Folio <= @FolioTo))
		or (@FolioTo = 0 and  (@FolioFrom > 0 and F.Folio >= @FolioFrom))
		or (@FolioTo > 0 and @FolioFrom > 0 and (F.Folio between @FolioFrom and @FolioTo)  
	))
	-- Fecha de booking
	and(@DateFrom is null or G.guBookD Between @DateFrom and @DateTo)
	-- Lead Sources
	AND (@LeadSources = 'ALL' OR L.lsID IN (select item from split(@LeadSources, ',')))
	--PRs
	AND (@PRs ='ALL' OR P.peID IN (select item from split(@PRs, ',')))
order by G.guOutInvitNum