using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

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
    /// </history>
    public static List<SegmentByAgency> GetSegMentsByAgency(SegmentByAgency segmentByAgency, int nStatus = -1)
    {
      using (var dbContext = new IMEntities())
      {
        var query = from sba in dbContext.SegmentsByAgencies
                    select sba;

        if (nStatus != -1)//Filtro por Estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sba => sba.seA == blnStatus);
        }

        if (!string.IsNullOrWhiteSpace(segmentByAgency.seID))//Filtro por ID
        {
          query = query.Where(sba => sba.seID == segmentByAgency.seID);
        }

        if (!string.IsNullOrWhiteSpace(segmentByAgency.seN))//Fitro por nombre(Descripcion)
        {
          query = query.Where(sba => sba.seN == segmentByAgency.seN);
        }
        
        return query.OrderBy(sba=>sba.seN).ToList();
      }
    } 
    #endregion
  }
}
