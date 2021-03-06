if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sprptLoginsLogCombos]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptLoginsLogCombos]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE [dbo].[sprptLoginsLogCombos]
AS

SET NOCOUNT ON;

--SELECT loID, loN FROM Locations INNER JOIN LeadSources ON lols=lsID WHERE loA=1 AND lspg='IH'

SELECT loID, loN INTO #t1 FROM Locations INNER JOIN LeadSources ON lols=lsID WHERE loA=1 AND lspg='IH'
INSERT INTO  #t1 VALUES ('','')
SELECT * FROM #t1 ORDER BY loN

SELECT peID, peN INTO #t2 FROM Personnel WHERE peA=1 AND peID IN(SELECT plpe FROM PersLSSR INNER JOIN LeadSources 
											  ON plLSSRID = lsID WHERE plLSSR = 'LS' AND lspg='IH' AND lsA=1)
INSERT INTO  #t2 VALUES ('','')
SELECT * FROM #t2 ORDER BY peN


SELECT DISTINCT llPCName INTO #t3 FROM LoginsLog
INSERT INTO  #t3 VALUES ('')
SELECT * FROM #t3 ORDER BY llPCName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

