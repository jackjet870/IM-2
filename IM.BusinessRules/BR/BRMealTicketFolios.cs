using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRMealTicketFolios
  {

    #region GetMaxMealTicketFolio
    /// <summary>
    /// Funcion para obtener el ultimo folio agregado
    /// </summary>
    /// <param name="mfsr"></param>
    /// <param name="mfmy"></param>
    /// <param name="mfra"></param>
    /// <returns>int</returns>
    /// <history>
    /// [vipacheco] 30/03/2016 Created
    /// </history>
    public static int GetMaxMealTicketFolio(string mfsr, string mfmy, int mfra)
    {
      string result = "";
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        result = dbContext.MealTicketsFolios.Where(x => x.mfsr == mfsr && x.mfmy == mfmy && x.mfra == mfra).Select(s => s.mfFolio).Max();
        return 1;
      }
    } 
    #endregion

    #region UpdateMealTicketFolio
    /// <summary>
    /// Funcion que actualiza en la BD del Meal ticket creado
    /// </summary>
    /// <param name="SR"></param>
    /// <param name="MType"></param>
    /// <param name="RType"></param>
    /// <param name="strNewFolio"></param>
    /// <history>
    /// [vipacheco] 31/03/2016 Created
    /// </history>
    public static void UpdateMealTicketFolio(string SR, string MType, int RType, string strNewFolio)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_UpdateMealTicketFolio(SR, MType, strNewFolio, RType);
      }
    }
    #endregion

  }
}
