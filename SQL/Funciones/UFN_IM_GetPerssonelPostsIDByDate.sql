USE [OrigosVCPalace]
GO
/****** Object:  UserDefinedFunction [dbo].[UFN_IM_GetPerssonelPostsIDByDate]    Script Date: 06/03/2016 11:54:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
** Palace Resorts
** Grupo de Desarrollo Palace
**
** Devuelve el puesto el puesto que tenia el personal entre las fechas indicadas
** 
** [ecanul]	27/May/2016 Created
**
*/
CREATE FUNCTION [dbo].[UFN_IM_GetPerssonelPostsIDByDate](
	@date Datetime,
	@PersonnelID varchar(50)
)
Returns Varchar(50)
AS
BEGIN
	DECLARE @postLCount int;
	DECLARE @POSTS VARCHAR(50);
	SET @postLCount = (SELECT COUNT(ppDT) FROM PostsLog WHERE pppe = @PersonnelID AND CONVERT(VARCHAR,ppdt,112)<=@date);
	IF @PostLCount > 0
		SET @POSTS = (SELECT TOP 1 pppo from PostsLog WHERE pppe = @PersonnelID AND CONVERT(VARCHAR,ppdt,112)<=@date ORDER BY ppDT DESC);
	ELSE
		SET @POSTS = (SELECT pepo FROM Personnel WHERE peID = @PersonnelID);
	IF @POSTS IS NULL
		SET @POSTS = 'NP'
	RETURN @POSTS;
END
