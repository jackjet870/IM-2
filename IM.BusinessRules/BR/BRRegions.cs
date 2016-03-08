using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRRegions
  {

    #region GetRegions
    /// <summary>
    /// Devuelve el catalogo de Regiones
    /// </summary>
    /// <param name="nStatus"><-1. Sin filtro| 0. Inactivos| 1. Activos/param>
    /// <returns>Lista de regiones</returns>
    /// /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// </history>
    public static List<Region> GetRegions(int nStatus=-1)
    {
      List<Region> lstRegion = new List<Region>();
      using (var dbContext = new IMEntities())
      {
        if(nStatus==-1)//Devuelve toda la lista de regiones
        {
          lstRegion = dbContext.Regions.ToList();
        }
        else//devuelve la lista por status activo o inactivo
        {
          bool bStat = Convert.ToBoolean(nStatus);
          lstRegion = dbContext.Regions.Where(c=>c.rgA==bStat).ToList();
        }
      }
      return lstRegion;
    }
    #endregion
  }
}
