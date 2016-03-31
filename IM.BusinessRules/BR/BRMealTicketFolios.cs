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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return Convert.ToInt32(dbContext.MealTicketsFolios.Where(x => x.mfsr == mfsr && x.mfmy == mfmy && x.mfra == mfra).Select(s => s.mfFolio).Max());
      }
    }

  }
}
