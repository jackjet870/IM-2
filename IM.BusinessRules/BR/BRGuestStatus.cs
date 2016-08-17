using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGuestStatus
  {

    /// <summary>
    /// Obtiene el GuestsStatus de acuerdo al guestID ingresado
    /// </summary>
    /// <param name="guestID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 11/Abril/2016 Created
    /// </history>
    public static GuestStatus GetGuestsStatus(int guestID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.GuestsStatus.Join(dbContext.GuestsStatusTypes, x => x.gtgs, y => y.gsID, (x, y) => x).SingleOrDefault();
      }
    }

    #region GetStatusValidateInfo
    /// <summary>
    /// Devuelve la informacion de GuesStatus de un Guest e informaicon de cuantos permite
    /// </summary>
    /// <param name="guestID"></param>
    /// <param name="receiptID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// </history>
    public static GuestStatusValidateData GetGuestStatusInfo(int guestID, int receiptID = 0)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetGuestStatusValidateInfo(guestID, receiptID).SingleOrDefault();
      }
    } 
    #endregion

  }
}
