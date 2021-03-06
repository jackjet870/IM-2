USE [OrigosVCPalace]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USP_IM_RptAccountingCodes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USP_IM_RptAccountingCodes]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
**
**
**Palace Resorts
**Desarrollo de sistemas
** Obtiene los registros para generar el reporte de Accounting Codes.
**
**[edgrodriguez] 14/Oct/2016 Created.
*/
CREATE PROCEDURE [dbo].[USP_IM_RptAccountingCodes](
@DateFrom datetime,
@DateTo datetime,
@SalesRooms varchar(MAX) ='ALL'
)
AS 
BEGIN
  SET NOCOUNT ON
    
  SELECT 
        '2270' soccecoid,
         g.guID,
         g.guBookD,
         s.srN,
         a.actN,
         ag.acgCode ceco,
         p.pgN,         
         m.mkN,
         ls.lsN,
         asg.asgSegment,
         sl.saMembershipNum
  FROM dbo.Guests g
  INNER JOIN dbo.SalesRooms s ON s.srID= g.gusr  
  INNER JOIN dbo.LeadSources ls ON ls.lsID = g.gulsOriginal  
  INNER JOIN dbo.Programs p ON p.pgID = ls.lspg
  INNER JOIN dbo.AccountingGuests ag ON ag.acggu= g.guID
  INNER JOIN dbo.Activities a ON a.actID = ag.acgact 
  INNER JOIN dbo.AccountingSegments asg ON asg.asgpg = p.pgID
  CROSS APPLY dbo.UFN_OR_GetAccountingSegment(g.guID,p.pgID) seg -- ON seg.seg = asg.asgSegment   
  LEFT  JOIN dbo.Markets m ON m.mkID = asg.asgmk
  LEFT JOIN dbo.Sales sl ON sl.sagu = g.guID
  WHERE 
	g.guBookD between @DateFrom and @DateTo --Fecha de booking
	AND (@SalesRooms='ALL' OR (s.srID IN (SELECT item FROM dbo.Split(@SalesRooms, ',')))) --Salas de venta
	AND a.actA= 1  
	AND asg.asgA=1
	AND ( 
    (p.pgID='IH' AND asg.asgmk = g.gumk)
    OR 
    (p.pgID='OUT' AND (
	(seg.lsID <> '' AND ls.lsID= seg.lsID AND asg.asgls = ls.lsID)
    OR
    (seg.lsID = '' AND asg.asgls is null AND seg.seg = asg.asgSegment ))))
  group by g.guID, g.guBookD, s.srN, a.actN, ag.acgCode, p.pgN, m.mkN, ls.lsN, asg.asgSegment, sl.saMembershipNum
  ORDER BY p.pgN, g.guID, sl.saMembershipNum, a.actN
  
  
  END