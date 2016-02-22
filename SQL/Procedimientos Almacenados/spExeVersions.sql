if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spExeVersions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spExeVersions]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE procedure spExeVersions
	@LauncherName varchar(20),
	@EXEName varchar(20)
as
set nocount on
Select 	evDepFile, evMajor, evMinor, evRevision, evDate from EXEVersions
where	evEXEName = @LauncherName
Select 	evDepFile, evMajor, evMinor, evRevision, evDate from EXEVersions
where	evEXEName = @EXEName





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

