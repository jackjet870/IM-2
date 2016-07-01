using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{

  public class BRGuestsPromotions
  {

    #region SaveGuestPromotion
    /// <summary>
    /// Guarda una promocion de Opera
    /// </summary>
    /// <param name="Receipt"> Clave del recibo de regalos </param>
    /// <param name="Gift"> Clave del regalo </param>
    /// <param name="Promotion"> Clave de la promocion de Opera </param>
    /// <param name="Guest"> Clave del huesped </param>
    /// <param name="Quantity"> Cantidad </param>
    /// <param name="Date"> Fecha </param>
    /// <history>
    /// [vipacheco] 01/Junio/2016 Created
    /// </history>
    public static void SaveGuestPromotion(int Receipt, string Gift, string Promotion, int? Guest, int Quantity, DateTime Date)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.USP_OR_SaveGuestPromotion(Receipt, Gift, Promotion, Guest, Quantity, Date);
      }
    }
    #endregion

    #region GetIsUsedGuestPromotion
    /// <summary>
    /// Determina si un regalo ha sido usado como promocion de Opera
    /// </summary>
    /// <param name="receiptID"></param>
    /// <param name="gift"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 24/Junio/2016 Created
    /// </history>
    public static bool GetIsUsedGuestPromotion(int receiptID, string gift)
    {
      bool result = false;

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        result = Convert.ToBoolean(dbContext.USP_OR_IsUsedGuestPromotion(receiptID, gift));
      }

      return result;
    }
    #endregion

    #region GetGuestPromotion
    /// <summary>
    ///  Consulta una promocion de Opera
    /// </summary>
    /// <param name="pReceiptID"></param>
    /// <param name="pGift"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 01/Julio/2016 Created
    /// </history>
    public async static Task<GuestPromotion> GetGuestPromotion(int pReceiptID, string pGift)
    {
      GuestPromotion guest = new GuestPromotion();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          guest = dbContext.GuestsPromotions.Where(x => x.gpgr == pReceiptID && x.gpgi == pGift).SingleOrDefault();
        }
      });

      return guest;
    } 
    #endregion

  }
}
