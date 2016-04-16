using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    /// </history>
    public static List<Region> GetRegions(int nStatus=-1, Region region = null)
    {      
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from rg in dbContext.Regions
                    select rg;
        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(rg=>rg.rgA==blnEstatus);          
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

        return query.OrderBy(rg=>rg.rgN).ToList();
      }
      
    }
    #endregion
    #region SaveRegion
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo Regions
    /// </summary>
    /// <param name="region">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | false. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 14/04/2016
    /// </history>
    public static int SaveRegion(Region region,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(region).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insert
        else
        {
          Region regionVal = dbContext.Regions.Where(rg => rg.rgID == region.rgID).FirstOrDefault();
          if(regionVal!=null)//Validamos que no exista un registro con el mismo ID
          {
            return 2;
          }
          else//Agregar
          {
            dbContext.Regions.Add(region);
          }
        } 
        #endregion
        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
