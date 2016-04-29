using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRTourTimesSchemas
  {
    #region GetTourTimesSchemas
    /// <summary>
    /// Obtiene registros del catalogo TourTimesSchemas
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="tourTimeSchema">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo TourTimesSchema</returns>
    /// <history>
    /// [emoguel] created 28/04/2016
    /// </history>
    public static List<TourTimesSchema> GetTourTimesSchemas(int nStatus = -1, TourTimesSchema tourTimeSchema = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from tc in dbContext.TourTimesSchemas
                    select tc;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(tc => tc.tcA == blnStatus);
        }

        if (tourTimeSchema != null)
        {
          if (tourTimeSchema.tcID > 0)//filtro por ID
          {
            query = query.Where(tc => tc.tcID == tourTimeSchema.tcID);
          }

          if (!string.IsNullOrWhiteSpace(tourTimeSchema.tcN))//Filtro por Descripción
          {
            query = query.Where(tc => tc.tcN.Contains(tourTimeSchema.tcN));
          }
        }

        return query.OrderBy(tc => tc.tcN).ToList();
      }
    } 
    #endregion
  }
}
