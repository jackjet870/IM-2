using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRLocationsCategories
  {
    #region GetLocationsCategories
    /// <summary>
    /// Obtiene registros de LocationCategories
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. registros inactivos | 1. Registros activos</param>
    /// <param name="locationCategory">objeto con los filtros adicionales</param>
    /// <returns>Lista tipo Location category</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    public static List<LocationCategory> GetLocationsCategories(int nStatus=-1,LocationCategory locationCategory=null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from lc in dbContext.LocationsCategories
                    select lc;

        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(lc => lc.lcA == blnEstatus);
        }

        if(locationCategory!=null)//Verificamos si se tiene un objeto
        {
          if(!string.IsNullOrWhiteSpace(locationCategory.lcID))//Filtro por ID
          {
            query = query.Where(lc => lc.lcID == locationCategory.lcID);
          }

          if(!string.IsNullOrWhiteSpace(locationCategory.lcN))//Filtro por descripcion
          {
            query = query.Where(lc => lc.lcN.Contains(locationCategory.lcN));
          }
        }

        return query.OrderBy(lc => lc.lcN).ToList();
      }
    }
    #endregion
  }
}
