using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;
using System.Threading.Tasks;

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
    /// [emoguel] modified 28/06/2016 --- Se volvió async
    /// </history>
    public async static Task<List<LocationCategory>> GetLocationsCategories(int nStatus=-1,LocationCategory locationCategory=null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from lc in dbContext.LocationsCategories
                      select lc;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnEstatus = Convert.ToBoolean(nStatus);
            query = query.Where(lc => lc.lcA == blnEstatus);
          }

          if (locationCategory != null)//Verificamos si se tiene un objeto
          {
            if (!string.IsNullOrWhiteSpace(locationCategory.lcID))//Filtro por ID
            {
              query = query.Where(lc => lc.lcID == locationCategory.lcID);
            }

            if (!string.IsNullOrWhiteSpace(locationCategory.lcN))//Filtro por descripcion
            {
              query = query.Where(lc => lc.lcN.Contains(locationCategory.lcN));
            }
          }
          return query.OrderBy(lc => lc.lcN).ToList();
        }
      });
    }
    #endregion

    #region SaveLocationCategories
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo LocationsCategories
    /// Asigna|desasigna locations de loa¿cationscategories
    /// </summary>
    /// <param name="locationCategory">Objeto a guardar</param>
    /// <param name="lstAdd">Locations a asignar</param>
    /// <param name="lstDel">Locations a desasignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>-1. Existe un registro con el mismo ID | 0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 18/05/2016
    /// [emoguel] modified 29/07/2016 se volvió async
    /// </history>
    public static async Task<int> SaveLocationCategories(LocationCategory locationCategory, List<Location> lstAdd, List<Location> lstDel, bool blnUpdate)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              if (blnUpdate)
              {
                dbContext.Entry(locationCategory).State = EntityState.Modified;
              }
              else
              {
                var locationVal = dbContext.LocationsCategories.Where(lc => lc.lcID == locationCategory.lcID).FirstOrDefault();
                if (locationVal != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.LocationsCategories.Add(locationCategory);
                }
              }

              #region Locations
              dbContext.Locations.AsEnumerable().Where(lo => lstAdd.Any(loo => loo.loID == lo.loID)).ToList().ForEach(lo =>
                   {//Agresignar locaciones
                     lo.lolc = locationCategory.lcID;
                   });

              dbContext.Locations.AsEnumerable().Where(lo => lstDel.Any(loo => loo.loID == lo.loID)).ToList().ForEach(lo =>
              {//Agresignar locaciones
                lo.lolc = null;
              });
              #endregion

              int nRes = dbContext.SaveChanges();
              transacction.Commit();
              return nRes;
            }
            catch
            {
              transacction.Rollback();
              return 0;
            }
          }
        }
      });
    } 
    #endregion
  }
}
