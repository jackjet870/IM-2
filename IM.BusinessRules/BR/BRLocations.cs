using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System;

namespace IM.BusinessRules.BR
{
  public class BRLocations
  {
    #region GetLocationsByUser

    /// <summary>
    /// Obtiene las locaciones de un usuario
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="programs"> Programa o default('ALL') </param>
    /// <history>
    /// [wtorres]  07/Mar/2016 Created
    /// </history>
    public static List<LocationByUser> GetLocationsByUser(string user, string programs)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetLocationsByUser(user, programs).ToList();
      }
    }

    #endregion

    #region GetLocations
    /// <summary>
    /// Obtiene registros del catalogo Locations
    /// </summary>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1.Registros Activos</param>
    /// <param name="location">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Location</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    public static List<Location> GetLocations(int nStatus=-1,Location location=null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from lo in dbContext.Locations
                    select lo;

        if(nStatus!=-1)//Filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(lo => lo.loA == blnEstatus);
        }
        #region Filtros adicionales

        if (location != null)//verificar si se tiene un objeto
        {
          if (!string.IsNullOrWhiteSpace(location.loID))//filtro por ID
          {
            query = query.Where(lo => lo.loID == location.loID);
          }

          if (!string.IsNullOrWhiteSpace(location.loN))//Filtro por Descripcion
          {
            query = query.Where(lo => lo.loN.Contains(location.loN));
          }

          if (!string.IsNullOrWhiteSpace(location.losr))//Filtro por sales room
          {
            query = query.Where(lo => lo.losr == location.losr);
          }

          if (!string.IsNullOrWhiteSpace(location.lolc))//Filtro por categoria
          {
            query = query.Where(lo => lo.lolc == location.lolc);
          }
        }

        #endregion
        return query.OrderBy(lo => lo.loN).ToList();
      }
    }

    #endregion

    #region SaveLocation
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo Locations
    /// </summary>
    /// <param name="location">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó correctamente | 2. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 01/04/2016
    /// </history>
    public static int SaveLocation(Location location,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(location).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insert
        else
        {
          Location locationVal = dbContext.Locations.Where(lo => lo.loID == location.loID).FirstOrDefault();
          if(locationVal!=null)//Existe un registro con el mismo ID
          {
            return 2;
          }
          else//Insertar el registro
          {
            dbContext.Locations.Add(location);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
