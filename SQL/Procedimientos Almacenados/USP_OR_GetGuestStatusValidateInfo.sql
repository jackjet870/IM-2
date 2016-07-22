USE [OrigosVCPalace];
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
/*
Devuelve la informacion de GuesStatus de un Guest e informaicon de cuantos permite
[LorMartinez] 9/3/2016 Created
*/
CREATE PROCEDURE dbo.USP_OR_GetGuestStatusValidateInfo(
@Guest integer,
@GiftReceipt integer = 0)
as
begin

set nocount on

--Si no hay GuestID, se usa el Receipt
if @GiftReceipt > 0 AND @Guest=0 
 begin
   SELECT @Guest = gr.grgu  FROM GiftsReceipts gr where gr.grID= @GiftReceipt
 end

SELECT gs.gsID, 
       gs.gsN, 
       gs.gsA, 
       gs.gsAgeStart, 
       gs.gsAgeEnd, 
       gs.gsMaxAuthGifts, 
       gs.gsMaxQtyTours, 
       gs.gsAllowTourDisc,
       g.guPax,
       isnull(tours.DiscUsed,0) [DiscUsed],
       isnull(tours.TourUsed,0) [TourUsed],
       ls.lspg
FROM dbo.Guests g
INNER JOIN dbo.GuestStatus gs ON g.guGStatus = gs.gsID
INNER JOIN dbo.LeadSources ls ON ls.lsID= g.gulsOriginal
OUTER APPLY(SELECT SUM( 
              CASE WHEN gi.giDiscount= 1 THEN
                 (gi.giQty * grc.geQty)
                ELSE 0
              END )[DiscUsed],  
              SUM(CASE WHEN gi.giDiscount= 0 THEN
                 (gi.giQty * grc.geQty)
                ELSE 0
              END) [TourUsed]
              FROM dbo.GiftsReceipts gr
              INNER JOIN dbo.GiftsReceiptsC grc ON grc.gegr = gr.grID
              INNER JOIN dbo.Gifts gi ON gi.giID = grc.gegi
              INNER JOIN dbo.GiftsCategs gc ON gc.gcID = gi.gigc
              WHERE gr.grgu = @Guest         
              AND gc.gcID='TOURS'
              AND gr.grCancel = 0
              AND ((@GiftReceipt= 0) OR ( @GiftReceipt > 0 AND @GiftReceipt <> gr.grID))
            ) tours
where g.guID= @Guest
AND ls.lspg = 'IH'

end
GO