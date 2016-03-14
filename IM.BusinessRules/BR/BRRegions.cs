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
    /// <param name="region">Objeto con filtros adicionales</param>
    /// <param name="nStatus"><-1. Sin filtro| 0. Inactivos| 1. Activos/param>
    /// <returns>Lista de regiones</returns>
    /// <history>
    /// [emoguel] 27/Feb/2016 Created
    /// [emoguel] 11/03/2016 Se le agregaron los filtros ID y Descripcion
    /// </history>
    public static List<Region> GetRegions(Region region,int nStatus=-1)
    {      
      using (var dbContext = new IMEntities())
      {
        var query = from rg in dbContext.Regions
                    select rg;
        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(rg=>rg.rgA==blnEstatus);          
        }

        if(!string.IsNullOrWhiteSpace(region.rgID))//Filtro por ID
        {
          query = query.Where(rg=>rg.rgID==region.rgID);
        }

        if(!string.IsNullOrWhiteSpace(region.rgN))//Filtro por nombre(Descripcion)
        {
          query = query.Where(rg=>rg.rgN==region.rgN);
        }

        return query.OrderBy(rg=>rg.rgN).ToList();
      }
      
    }
    #endregion
  }
}
