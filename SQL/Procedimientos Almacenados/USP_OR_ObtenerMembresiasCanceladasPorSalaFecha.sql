if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_ObtenerMembresiasCanceladasPorSalaFecha]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_ObtenerMembresiasCanceladasPorSalaFecha]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Autor:			William Jesús Torres Flota
-- Procedimiento:	Obtener membresías canceladas por sala y fecha
-- Fecha:			27/Nov/2008
-- Descripción:		Devuelve las membresías que se vendieron en una sala en un rango de
--					fechas determinado
-- =============================================
CREATE PROCEDURE [dbo].[USP_OR_ObtenerMembresiasCanceladasPorSalaFecha]
	@SalesRoom varchar(10),	-- Clave de la sala
	@DateFrom datetime,		-- Fecha inicial
	@DateTo	datetime		-- Fecha final
AS
	SET NOCOUNT ON;

select
	saMembershipNum as MembershipNum
	from Guests
	left outer join Sales on guID = sagu
	where
		-- Canceladas
		saCancelD between @DateFrom and @DateTo
		-- Ventas de la sala
		and sasr = @SalesRoom

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

