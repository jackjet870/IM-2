if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSourcesOfProgram]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSourcesOfProgram]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Copia los horarios de tour por Lead Source y sala de ventas de un Lead Source a todos los Lead Sources del mismo programa
** 
** [wtorres]	31/May/2011 Creado
**
*/
create procedure [dbo].[USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSourcesOfProgram]
	@LeadSource varchar(10)	-- Clave del Lead Source
as
set nocount on

declare @Program varchar(10)	-- Clave del programa

-- obtenemos el programa del Lead Source
select @Program = lspg from LeadSources where lsID = @LeadSource

-- eliminamos los horarios de tour de todos los Lead Sources del mismo programa menos los del Lead Source de donde se van a copiar los horarios de tour
delete from TourTimes
where ttls in (
	select lsID
	from LeadSources
	where lspg = @Program
		and lsID <> @LeadSource
)

-- copiamos los horarios de tour del Lead Source a todos los Lead Sources del mismo programa
insert into TourTimes (ttls, ttsr, ttT, ttPickUpT, ttMaxBooks)
select L.lsID, T.ttsr, T.ttT, T.ttPickUpT, T.ttMaxBooks
from TourTimes T, LeadSources L
where
	-- Horarios de tour del Lead Source
	T.ttls = @LeadSource
	-- Lead Sources del mismo programa
	and L.lspg = @Program
	and L.lsID <> @LeadSource
	and L.lsA = 1
order by L.lsID, T.ttsr, T.ttT

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

