using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public static class BRInvitsGifts
  {

    #region GetGiftsInvitationWithoutReceipt
    /// <summary>
    /// Obtiene los regalos de una invitacion                                                       
    /// </summary>
    /// <param name="guestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 28/Abril/2016 Created
    /// [vipacheco] 17/Junio/2016 Modified --> Se agrego sincronia y se modifico el tipo de lista retornada
    /// </history>
    public static async Task<List<GiftsReceiptDetail>> GetGiftsInvitationWithoutReceipt(int? guestID, bool? package = false)
    {
      List<GiftsReceiptDetail> lstResult = new List<GiftsReceiptDetail>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          // Obtenemos los datos del stored
          List<GiftInvitationWithoutReceipt> lstShort = dbContext.USP_OR_GetGiftsInvitationWithoutReceipt(guestID, package).ToList();

          if (lstShort.Count > 0)
          {
            // Contruimos la lista nueva a retornar!
            lstResult = lstShort.Select(x => new GiftsReceiptDetail
            {
              geAdults = x.geAdults,
              geAsPromotionOpera = (bool)x.geAsPromotionOpera,
              geCancelElectronicPurse = (bool)x.geCancelElectronicPurse,
              geCancelPVPPromo = (bool)x.geCancelPVPPromo,
              geCharge = (decimal)x.geCharge,
              geComments = x.geComments,
              geConsecutiveElectronicPurse = string.IsNullOrEmpty(x.geConsecutiveElectronicPurse) ? 0 : Convert.ToInt32(x.geConsecutiveElectronicPurse),
              gect = x.gect,
              gecxc = x.geCxC,
              geExtraAdults = x.geExtraAdults,
              geFolios = x.geFolios,
              gegi = x.gegi,
              gegr = string.IsNullOrEmpty(x.gegr) ? 0 : Convert.ToInt32(x.gegr),
              geInElectronicPurse = (bool)x.geInElectronicPurse,
              geInOpera = (bool)x.geInOpera,
              geInPVPPromo = (bool)x.geInPVPPromo,
              geMinors = x.geMinors,
              gePriceA = (decimal)x.gePriceA,
              gePriceAdult = (decimal)x.gePriceAdult,
              gePriceExtraAdult = (decimal)x.gePriceExtraAdult,
              gePriceM = (decimal)x.gePriceM,
              gePriceMinor = (decimal)x.gePriceMinor,
              geQty = x.geQty,
              geSale = (bool)x.geSale
            }).ToList();
          }
        }
      });

      return lstResult;
    }
    #endregion

    #region GetInvitGift
    /// <summary>
    /// Obtienen un Invitation de un Gift en especifico
    /// </summary>
    /// <param name="guestID"></param>
    /// <param name="giftID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 11/Mayo/2016 Created
    /// [aalcocer]  03/08/2016 Modified. Se agregó asincronía
    /// </history>
    public static async Task<InvitationGift> GetInvitGift(int guestID, string giftID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.InvitationsGifts.SingleOrDefault(x => x.iggu == guestID && x.iggi == giftID);
        }
      });
    }
    #endregion

    #region GetInvitGift
    /// <summary>
    /// Obtienen el InvitationGift de un Guest especifico
    /// </summary>
    /// <param name="guestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 13/Mayo/2016 Created
    /// [aalcocer]  03/08/2016 Modified. Se agregó asincronía
    /// </history>
    public static async Task<List<InvitationGift>> GetInvitsGiftsByGuestID(int guestID)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.InvitationsGifts.Where(x => x.iggu == guestID).ToList();
        }
      });
    }
    #endregion GetInvitGift

  }
}
