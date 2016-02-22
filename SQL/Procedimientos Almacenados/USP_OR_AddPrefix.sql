if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_OR_AddPrefix]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_OR_AddPrefix]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
** Palace Resorts
** Grupo de Desarrollo Palace
[LorMartinez] 20/10/2015 Created, Agrega el prefixo a la lista de prefijos usados
[LorMartinez] 29/10/2015 Modified, Se aquita campo initial
*/
CREATE PROCEDURE dbo.USP_OR_AddPrefix(@prxID varchar(4),
@TableName varchar(50),
@Description varchar(80)
)
AS
BEGIN
 SET NOCOUNT ON
 
 IF EXISTS(SELECT p.prxID
           FROM dbo.Prefixes p
           where p.prxID = @prxID
          )
  BEGIN          
   SELECT 'The prefix is already in use, please use another one.'
   RETURN
  END
  
  INSERT INTO dbo.Prefixes( prxID,  prxTable, prxN)
  VALUES(@prxID, @TableName,@Description)
    
  
  SELECT * FROM dbo.Prefixes p where p.prxID=@prxID
  Select 'Process Completed Successfuly'
  
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

