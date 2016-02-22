if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_RptAgencies]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_RptAgencies]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve los datos para el reporte de agencias
** 
** [wtorres]	17/Sep/2009 Creado
** [wtorres]	28/Dic/2011 Agregue el campo descripcion de la agencia
**
*/
create procedure [dbo].[USP_OR_RptAgencies]
as
set nocount on

select
	A.agID,
	A.agN,
	U.umN,
	A.agmk,
	A.agShowPay,
	A.agSalePay,
	A.agrp,
	A.agA
from Agencies A
	left join UnavailMots U on A.agum = U.umID
order by A.agA desc, A.agID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

