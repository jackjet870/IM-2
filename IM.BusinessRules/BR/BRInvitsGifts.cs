using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRInvitsGifts
  {

    #region GetGiftsInvitationWithoutReceipt
    /// <summary>
    /// Obtiene los regalos de una invitacion                                                       
    /// </summary>
    /// <param name="guestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 28/Abril/2016
    /// </history>
    public static List<GiftInvitationWithoutReceipt> GetGiftsInvitationWithoutReceipt(int? guestID, bool? package = false)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsInvitationWithoutReceipt(guestID, package).ToList();
      }
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
    /// </history>
    public static InvitationGift GetInvitGift(int guestID, string giftID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.InvitationsGifts.Where(x => x.iggu == guestID && x.iggi == giftID).SingleOrDefault();
      }
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
    /// </history>
    public static List<InvitationGift> GetInvitsGiftsByGuestID(int guestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.InvitationsGifts.Where(x => x.iggu == guestID).ToList();
      }
    }
    #endregion

  }
}
