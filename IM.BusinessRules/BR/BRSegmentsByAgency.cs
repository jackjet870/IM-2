using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRSegmentsByAgency
  {
    #region GetSegmentsByAgecy
    /// <summary>
    /// Devuelve la lista de SegmentByAgcy
    /// </summary>
    /// <param name="segmentByAgency">Objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0.Registros Inactivos | 1.Registros Activos</param>
    /// <returns></returns>
    /// <history>
    /// [emoguel] created 11/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<SegmentByAgency> GetSegMentsByAgency(SegmentByAgency segmentByAgency=null, int nStatus = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from sba in dbContext.SegmentsByAgencies
                    select sba;

        if (nStatus != -1)//Filtro por Estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sba => sba.seA == blnStatus);
        }

        if (segmentByAgency != null)//Valida si se tiene un objeto
        {
          if (!string.IsNullOrWhiteSpace(segmentByAgency.seID))//Filtro por ID
          {
            query = query.Where(sba => sba.seID == segmentByAgency.seID);
          }

          if (!string.IsNullOrWhiteSpace(segmentByAgency.seN))//Fitro por nombre(Descripcion)
          {
            query = query.Where(sba => sba.seN.Contains(segmentByAgency.seN));
          }
        }
        
        return query.OrderBy(sba=>sba.seN).ToList();
      }
    } 
    #endregion
  }
}
