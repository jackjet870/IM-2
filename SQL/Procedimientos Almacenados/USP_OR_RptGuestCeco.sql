/*
**[LorMartinez] 26/Nov/2015 Created, devuelve informacion de reporte GuestCeco
**
*/
CREATE PROCEDURE dbo.USP_OR_RptGuestCeco(
@DateFrom datetime,
@DateTo datetime,
@SalesRooms varchar(MAX)
)
AS 
BEGIN

  SET NOCOUNT ON
    
  SELECT sc.soccecoid,
         g.guID,
         g.guBookD,
         s.srN,
         ac.acn,
         cec.ceco,
         ms.mksN
  FROM dbo.Guests g
  INNER JOIN dbo.SalesRooms s ON s.srID= g.gusr
  INNER JOIN dbo.Societies sc WITH(NOLOCK) ON sc.socID='1001' 
  INNER JOIN dbo.LeadSources ls WITH(NOLOCK) ON ls.lsID = g.gulsOriginal  
  INNER JOIN dbo.CECOCEBETypes c WITH(NOLOCK) ON c.ceID  IN ('MARKET','SALES')
  INNER JOIN dbo.Activities ac WITH(NOLOCK) ON ac.acSource = ls.lspg AND ac.acce = c.ceID
  CROSS APPLY dbo.UFN_OR_GETCECO_tbt(g.guID, c.ceID) cec 
  CROSS APPLY dbo.UFN_OR_GetMarketSegment_Tbt(g.guID) mks 
  LEFT JOIN dbo.MarketSegments ms ON ms.mksid = mks.mktseg
  WHERE g.guBookD between @DateFrom and @DateTo --Fecha de booking
  AND s.srID IN (SELECT item FROM dbo.Split(@SalesRooms, ',')  ) --Salas de venta

END
