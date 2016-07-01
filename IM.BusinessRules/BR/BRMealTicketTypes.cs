using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRMealTicketTypes
  {
    #region GetMealTicketType
    /// <summary>
    /// Función para obtener los tipo de MealTicket
    /// </summary>
    /// <param name="mealTicketType">Objeto con filtros adicionales</param>
    /// <param name="nWPax">filtro de WPax</param>
    /// <returns> Lista de tipo MealTicketType </returns>
    /// <history>
    /// [vipacheco] 22/03/2016 Created
    /// [emoguel] 04/04/2016 Modified se agregaron filtros de busqueda
    /// [emoguel] modified28/06/2016 ---> Se volvió async
    /// </history>
    public async static Task<List<MealTicketType>> GetMealTicketType(MealTicketType mealTicketType=null,int nWPax=-1)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from my in dbContext.MealTicketTypes
                      select my;

          if (nWPax != -1)//filtro por WPAX
          {
            bool blnWPax = Convert.ToBoolean(nWPax);
            query = query.Where(my => my.myWPax == blnWPax);
          }

          #region Filtros adicionales
          if (mealTicketType != null)//Validamos si tenemos objeto
          {
            if (!string.IsNullOrWhiteSpace(mealTicketType.myID))//Filtro por ID
            {
              query = query.Where(my => my.myID == mealTicketType.myID);
            }

            if (!string.IsNullOrWhiteSpace(mealTicketType.myN))//Filtro por descripcion
            {
              query = query.Where(my => my.myN.Contains(mealTicketType.myN));
            }
          }
          #endregion

          return query.OrderBy(x => x.myN).ToList();
        }
      });
    }
    #endregion
  }
}
