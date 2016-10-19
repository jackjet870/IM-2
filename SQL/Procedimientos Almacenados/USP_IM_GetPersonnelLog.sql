if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_GetPersonnelLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_GetPersonnelLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtiene el registro historico de un Personal
** [emoguel]		17/Oct/2016 Created
*/
create procedure [dbo].[USP_IM_GetPersonnelLog] 
	@Personnel varchar(10)=NULL,-- Clave del personal
	@DateFrom datetime = NULL,  --Fecha desde
	@DateTo datetime =NULL		--Fecha hasta
as
set nocount on;

select 
plg.plgpe,--ID del personal
pe.peN ,--Nombre del personal
plg.plgDT,--Fecha en que se realizó el cambio
plg.plgChangedBy,--Id del personnal que realizó el cambio
pc.peN ChangedByN,--Nombre de la persona que realizó el cambio
de.deN,--Nombre del departamento del personal
po.poN,--Nombre del puesto del personal
sr.srN,--Nombre del sales Room del personal
lo.loN,--Nombre de la locacion del personal 
(SELECT STUFF(( 
    select ','+ r.roN
    from Roles r 
    WHERE r.roID in (select prl.prlro from PersonnelRolesLog prl WHERE prl.prlplg=plg.plgID)
    FOR XML PATH('') 
    ) 
    ,1,1,'')) Roles,--Roles del Personal
(SELECT STUFF(( 
    select ','+ sr.srN
    from SalesRooms sr 
    WHERE sr.srID in (select psl.pslsr from PersonnelSalesRoomsLog psl WHERE psl.pslplg=plg.plgID)
    FOR XML PATH('') 
    ) 
    ,1,1,'')) SalesRooms,--Salas de venta del Personal
(SELECT STUFF(( 
    select ','+ ls.lsN
    from LeadSources ls 
    WHERE ls.lsID in (select pll.pllls from PersonnelLeadSourcesLog pll WHERE pll.pllplg=plg.plgID)
    FOR XML PATH('') 
    ) 
    ,1,1,'')) LeadSources--Lead sources del Personal
from PersonnelLog plg
LEFT JOIN Personnel pe on pe.peID=plg.plgpe
LEFT JOIN Personnel pc on pc.peID=plg.plgChangedBy
LEFT JOIN Depts de on de.deID=plg.plgde
LEFT JOIN Posts po on po.poID=plg.plgpo
LEFT JOIN SalesRooms sr on sr.srID=plg.plgsr
LEFT JOIN Locations lo on lo.loID=plg.plglo
where (@personnel=NULL or plg.plgpe=@Personnel)
OR (@DateFrom IS NULL or plg.plgDT BETWEEN @DateFrom and ISNULL(@DateTo,@DateFrom) )

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO