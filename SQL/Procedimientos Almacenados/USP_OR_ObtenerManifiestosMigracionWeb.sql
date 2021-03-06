if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ObtenerManifiestosMigracionWeb]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ObtenerManifiestosMigracionWeb]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Obtenemos los datos necesarios para migrar los manifiestos a Web
**
** [cgutierrez] 28/01/2008 Created
**				22/01/2009 Contemplamos los manifiestos de Punta Cana
** [jsansores]	25/04/2011 Se agrego la obtencion de los manifiestos de aventura cove palace para la locacion A.
** [jsansores]	31/05/2011 Se agregaron los manifiestos de leblanc con la locacion cancun.
** [jsansores]	31/05/2011 Se agregaron los manifiestos cozumel palace para la sala de Aventura Palace
**						   Se agregaron los manifiestos de in house islas mujeres, in house sun palace para la la sala de Cancun Palace.
** [jsansores]	10/11/2011 Se quito la validación de que el campo G.guEntryHost "hostes que captura" sea diferente de vacio.
** [jsansores]	23/12/2011 Se agregaron las validaciones para las nuevas salas "Locaciones" B(Beach Palace) y L(PlayaCar Palace).
** [jsansores]	19/08/2014 Se agregaron las validaciones para las nuevas salas "Locaciones" J(Moon Palace Jamaica Grande).
** [jsansores]	17/02/2015 Se agrega validacion para que solo Shows sin ventas o si tiene ventas, que sean del dia
** [jsansores]	23/02/2015 Se agrega validacion para que se tomen los vendedores reales y no solo los principales.
** [wtorres]	02/03/2015 Si fue venta se devuelve el personal de la venta, de lo contrario se devuelve el personal del show.
**						   La segunda y tercer consultas no deben de incluir a los shows del dia
** [wtorres]	17/03/2015 Si fue venta se devuelve el nombre y apellido de la venta, de lo contrario se devuelve el nombre y apellido de la
**						   invitacion.
** [wtorres]	23/03/2015 Se corrige validacion para que tome solo Shows sin ventas o si tiene ventas, que sean del dia
**
*/
create procedure [dbo].[USP_OR_ObtenerManifiestosMigracionWeb]
	@Locacion varchar(10),
	@Fecha datetime,
	@Excluir varchar(4000)
as
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON

-- Shows del dia
SELECT distinct G.guls as lsID, G.guID,
	case when S.sagu is null then G.guLastName1 else S.saLastName1 end as guLastName1,
	case when S.sagu is null then G.guFirstName1 else S.saFirstName1 end as guFirstName1,
	G.guco, G.guHotel,
	G.guag, IsNull(G.guTimeInT, '') as guTimeInT, IsNull(G.guTimeOutT, '') as guTimeOutT, 
	G.guQ, IsNull( G.guDepSale, 0) as guDepSale, G.guBookD, IsNull(G.guDepositSaleNum, '') as guDepositSaleNum,
	IsNull(G.guMembershipNum, '') as guMembershipNum,
	S.sast, IsNull(S.saMembershipNum, '') as saMembershipNum, IsNull(S.saNewAmount, 0) as saNewAmount, S.saD, S.saProcD, S.saProc,
	IsNull(S.saRefMember, '') as saRefMember,
	Replace(IsNull(case when S.sagu is null then G.guPRInvit1 else S.saPR1 end, ''), '*', '') as guPRInfo,
	Replace(IsNull(case when S.sagu is null then G.guLiner1 else S.saLiner1 end, ''), '*', '') as guLiner1,  
	Replace(IsNull(case when S.sagu is null then G.guCloser1 else S.saCloser1 end, ''), '*', '') as guCloser1,
	Replace(IsNull(case when S.sagu is null then G.guExit1 else S.saExit1 end, ''), '*', '') as guExit1
FROM Guests G
	LEFT JOIN Sales as S ON G.guID = S.sagu
		-- Fecha de venta y fecha de venta procesable
		and (S.saD = Convert(varchar, @Fecha, 112) or S.saProcD = Convert(varchar, @Fecha, 112))
		-- Ventas no canceladas
		and S.saCancel = 0
where
	-- Sala de ventas
	(G.gusr = case @Locacion
		when 'P' then 'CP'
		when 'M' then 'MP'
		when 'S' then 'MPS'
		when 'A' then 'SPA'
		when 'G' then 'GMP'
		when 'C' then 'CZ'
		when 'V' then 'VP'
		when 'Z' then 'PC'
		when 'B' then 'BP'
		when 'L' then 'PL'
		when 'J' then 'ZCJG'
	end
	or G.gusr = case @Locacion when 'P' then 'LBC' when 'A' then 'ACP' end
	or G.gusr = case @Locacion when 'A' then 'CZ' end
	or G.gusr = case @Locacion when 'P' then 'IM' end
	or G.gusr = case @Locacion when 'P' then 'SP' end)
	-- Fecha de show
	and g.guShowD =  Convert(varchar, @Fecha, 112)
	-- Guest ID's a excluir
	and G.guID not in ( select * from  dbo.split(@Excluir, ',') ) 
                    
-- Ventas del dia
union
SELECT distinct G.guls as lsID, G.guID, S.saLastName1, S.saFirstName1,
	case when A.gaAdditional is null then G.guco else GA.guco end as guco,
	case when A.gaAdditional is null then G.guHotel else GA.guHotel end as guHotel,
	G.guag,
	IsNull(case when A.gaAdditional is null then G.guTimeInT else GA.guTimeInT end, '') as guTimeInT,
	IsNull(case when A.gaAdditional is null then G.guTimeOutT else GA.guTimeOutT end, '') as guTimeOutT,
	G.guQ,
	IsNull(case when A.gaAdditional is null then G.guDepSale else GA.guDepSale end, 0) as guDepSale,
	IsNull(case when A.gaAdditional is null then G.guBookD else GA.guBookD end, '') as guBookD,
	IsNull(case when A.gaAdditional is null then G.guDepositSaleNum else GA.guDepositSaleNum end, '') as guDepositSaleNum,
	IsNull(G.guMembershipNum, '') as guMembershipNum,
	S.sast, IsNull(S.saMembershipNum, '') as saMembershipNum, IsNull(S.saNewAmount, 0) as saNewAmount, S.saD, S.saProcD, S.saProc,
	IsNull(S.saRefMember, '') as saRefMember,
	Replace(IsNull(S.saPR1, ''), '*', '') as guPRInfo, Replace(IsNull(S.saLiner1, ''), '*', '') as guLiner1,  
	Replace(IsNull(S.saCloser1, ''), '*', '') as guCloser1, Replace(IsNull(S.saExit1, ''), '*', '') as guExit1
FROM Guests G
	LEFT JOIN Sales S ON G.guID = S.sagu
	LEFT JOIN GuestsAdditional A ON A.gaAdditional = G.guID
	LEFT JOIN Guests GA ON GA.guID = A.gagu
where
	-- Sala de ventas
	(G.gusr = case @Locacion
		when 'P' then 'CP'
		when 'M' then 'MP'
		when 'S' then 'MPS'
		when 'A' then 'SPA'
		when 'G' then 'GMP'
		when 'C' then 'CZ'
		when 'V' then 'VP'
		when 'Z' then 'PC'
		when 'B' then 'BP'
		when 'L' then 'PL'
		when 'J' then 'ZCJG'
	end
	or G.gusr = case @Locacion when 'P' then 'LBC' when 'A' then 'ACP' end
	or G.gusr = case @Locacion when 'A' then 'CZ' end
	or G.gusr = case @Locacion when 'P' then 'IM' end
	or G.gusr = case @Locacion when 'P' then 'SP' end)
	-- Shows de otro dia
	and G.guShowD <> Convert(varchar, @Fecha, 112)
	-- Fecha de venta
	and S.saD =  Convert(varchar, @Fecha, 112)
	-- Ventas no canceladas
	and S.saCancel = 0
	-- Guest ID's a excluir
	and G.guID not in ( select * from  dbo.split(@Excluir, ',') )

-- Ventas out of pending
union
SELECT distinct G.guls as lsID, G.guID, S.saLastName1, S.saFirstName1,
	case when A.gaAdditional is null then G.guco else GA.guco end as guco,
	case when A.gaAdditional is null then G.guHotel else GA.guHotel end as guHotel,
	G.guag,
	IsNull(case when A.gaAdditional is null then G.guTimeInT else GA.guTimeInT end, '') as guTimeInT,
	IsNull(case when A.gaAdditional is null then G.guTimeOutT else GA.guTimeOutT end, '') as guTimeOutT,
	G.guQ,
	IsNull(case when A.gaAdditional is null then G.guDepSale else GA.guDepSale end, 0) as guDepSale,
	IsNull(case when A.gaAdditional is null then G.guBookD else GA.guBookD end, '') as guBookD,
	IsNull(case when A.gaAdditional is null then G.guDepositSaleNum else GA.guDepositSaleNum end, '') as guDepositSaleNum,
	IsNull(G.guMembershipNum, '') as guMembershipNum,
	S.sast, IsNull(S.saMembershipNum, '') as saMembershipNum, IsNull(S.saNewAmount, 0) as saNewAmount, S.saD, S.saProcD, S.saProc,
	IsNull(S.saRefMember, '') as saRefMember,
	Replace(IsNull(S.saPR1, ''), '*', '') as guPRInfo, Replace(IsNull(S.saLiner1, ''), '*', '') as guLiner1,
	Replace(IsNull(S.saCloser1, ''), '*', '') as guCloser1, Replace(IsNull(S.saExit1,''), '*', '') as guExit1
FROM Guests G
	LEFT OUTER JOIN Sales as S ON G.guID = S.sagu
	LEFT JOIN GuestsAdditional A ON A.gaAdditional = G.guID
	LEFT JOIN Guests GA ON GA.guID = A.gagu
where
	-- Sala de ventas
	(G.gusr = case @Locacion
		when 'P' then 'CP'
		when 'M' then 'MP'
		when 'S' then 'MPS'
		when 'A' then 'SPA'
		when 'G' then 'GMP'
		when 'C' then 'CZ'
		when 'V' then 'VP'
		when 'Z' then 'PC'
		when 'B' then 'BP'
		when 'L' then 'PL'
		when 'J' then 'ZCJG'
	end
	or G.gusr = case @Locacion when 'P' then 'LBC' when 'A' then 'ACP' end
	or G.gusr = case @Locacion when 'A' then 'CZ' end
	or G.gusr = case @Locacion when 'P' then 'IM' end
	or G.gusr = case @Locacion when 'P' then 'SP' end)
	-- Shows de otro dia
	and G.guShowD <> Convert(varchar, @Fecha, 112)
	-- Fecha de venta procesable
	and S.saProcD = Convert(varchar, @Fecha, 112)
	-- Out of pending
	and S.saProcD <> S.saD
	-- Ventas no canceladas
	and S.saCancel = 0
	-- Guest ID's a excluir
	and G.guID not in ( select * from  dbo.split(@Excluir, ',') )

order by G.guID, saMembershipNum

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

