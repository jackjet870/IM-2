if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_SaveLoginLog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_SaveLoginLog]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- =============================================
-- Procedimiento:	Guardar histórico de logins
-- Descripción:		Agrega un registro en el histórico de logins
-- Histórico:		[wtorres] 01/Jun/2010 Creado
-- =============================================
create procedure [dbo].[USP_OR_SaveLoginLog]
	@Location varchar(10),		-- Clave de la locación
	@User varchar(10),			-- Clave del usuario
	@ComputerName varchar(10)	-- Nombre de la computadora
as
set nocount on

insert into LoginsLog 
(lllo, llpe, llPCName) 
values 
(@Location, @User , @ComputerName)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

