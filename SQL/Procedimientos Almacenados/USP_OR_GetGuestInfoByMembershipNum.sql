if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_GetGuestInfoByMembershipNum]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_GetGuestInfoByMembershipNum]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Reporte de estadísticas por locación
-- Descripción:		Obtiene los datos del reporte
-- Histórico:		[wtorres] 25/Sep/2009 Creado
--					[wtorres] 22/Ene/2010 Agregué el parámetro @ByLocationsCategories
-- =============================================
create procedure [dbo].[USP_OR_GetGuestInfoByMembershipNum]
	@MembershipNum varchar(10),		-- Número de membresía
	@BySegmentsCategories bit = 0,	-- Indica si es por categorías de segmentos
	@ByLocationsCategories bit = 0	-- Indica si es por categorías de locaciones
as
set nocount on

select
	guID,
	guWalkOut,
	guTour,
	guSelfGen,
	guts,
	-- Segmento
	IsNull(case when @BySegmentsCategories = 0 then (case when lspg = 'IH' then agse else lsso end)
		else (case when lspg = 'IH' then sesc else sosc end) end, 'NS') as Segment,
	IsNull(case when @BySegmentsCategories = 0 then (case when lspg = 'IH' then seN else soN end)
		else (case when lspg = 'IH' then SCA.scN else SCL.scN end) end, 'NO SEGMENT') as SegmentN,
	-- Locación
	case when @ByLocationsCategories = 0 then loN else IsNull(lcN, 'NO LOCATION CATEGORY') end as loN,
	guOverflow
from Guests
	left join Sales on sagu = guID
	left join Agencies on agID = guag
	left join SegmentsByAgency on seID = agse
	left join Locations on loID = guloInvit
	left join LocationsCategories on lolc = lcID
	left join LeadSources on sals = lsID
	left join SegmentsByLeadSource on lsso = soID
	left join SegmentsCategories SCA on sesc = SCA.scID
	left join SegmentsCategories SCL on sosc = SCL.scID
where saMembershipNum = @MembershipNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

