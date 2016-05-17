using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRSegmentsByLeadSource
  {
    #region GetSegmentsByLeadSource
    /// <summary>
    /// Obtiene registros del catalogo SegmentByLeadSource
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="segment">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo SegmentByLeadSource</returns>
    /// <history>
    /// [emoguel] created 16/05/2016
    /// </history>
    public static List<SegmentByLeadSource> GetSegmentsByLeadSource(int nStatus = -1, SegmentByLeadSource segment = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from so in dbContext.SegmentsByLeadSources
                    select so;

        if (nStatus != -1)//Filtro por Estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(so => so.soA == blnStatus);
        }

        if (segment != null)
        {
          if (!string.IsNullOrWhiteSpace(segment.soID))//Filtro por ID
          {
            query = query.Where(so => so.soID == segment.soID);
          }

          if (!string.IsNullOrWhiteSpace(segment.soN))//Filtro por descripción
          {
            query = query.Where(so => so.soN.Contains(segment.soN));
          }
        }

        return query.OrderBy(so => so.soN).ToList();
      }
    } 
    #endregion
  }
}
