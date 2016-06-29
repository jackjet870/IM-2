using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRPlaceTypes
  {
    #region GetPlaceTypes

    /// <summary>
    /// Obtiene registros del catalogo PlaceTypes
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="placeType">objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Playce Type</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    public async static Task<List<PlaceType>> GetPlaceTypes(int nStatus = -1, PlaceType placeType = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from py in dbContext.PlaceTypes
                      select py;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(py => py.pyA == blnStatus);
          }

          if (placeType != null)//verificamos si se tiene un objeto
          {
            if (!string.IsNullOrWhiteSpace(placeType.pyID))//filtro por ID
            {
              query = query.Where(py => py.pyID == placeType.pyID);
            }

            if (!string.IsNullOrWhiteSpace(placeType.pyN))//Filtro por descripción
            {
              query = query.Where(py => py.pyN.Contains(placeType.pyN));
            }
          }

          return query.OrderBy(py => py.pyN).ToList();
        }
      });
    }
    #endregion
  }
}
