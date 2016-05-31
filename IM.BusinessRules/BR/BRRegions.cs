using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

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
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// [emoguel] Modified 30/05/2016 Se volvio async
    /// </history>
    public async static Task<List<Region>> GetRegions(int nStatus = -1, Region region = null)
    {
      List<Region> lstRegions = new List<Region>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from rg in dbContext.Regions
                      select rg;
          if (nStatus != -1)//Filtro por estatus
          {
            bool blnEstatus = Convert.ToBoolean(nStatus);
            query = query.Where(rg => rg.rgA == blnEstatus);
          }

          if (region != null)//Valida si se tiene objeto
          {
            if (!string.IsNullOrWhiteSpace(region.rgID))//Filtro por ID
            {
              query = query.Where(rg => rg.rgID == region.rgID);
            }

            if (!string.IsNullOrWhiteSpace(region.rgN))//Filtro por nombre(Descripcion)
            {
              query = query.Where(rg => rg.rgN.Contains(region.rgN));
            }
          }

          lstRegions = query.OrderBy(rg => rg.rgN).ToList();
        }
      });

      return lstRegions;
    }
    #endregion
  }
}
