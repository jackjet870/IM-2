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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_SaveGuestPromotion(Receipt, Gift, Promotion, Guest, Quantity, Date);
      }
    } 
    #endregion

  }
}
