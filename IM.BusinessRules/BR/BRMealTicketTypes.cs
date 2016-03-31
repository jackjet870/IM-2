using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.BusinessRules.BR
{
  public class BRMealTicketTypes
  {
    #region GetMealTicketType
    /// <summary>
    /// Función para obtener los tipo de MealTicket
    /// </summary>
    /// <returns> Lista de tipo MealTicketType </returns>
    /// <history>
    /// [vipacheco] 22/03/2016 Created
    /// </history>
    public static List<MealTicketType> GetMealTicketType()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.MealTicketTypes.OrderBy(x => x.myN).ToList();
      }
    }
    #endregion
  }
}
