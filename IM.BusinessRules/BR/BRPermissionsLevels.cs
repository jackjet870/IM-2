using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPermissionsLevels
  {
    #region GetPermissionsLevels
    /// <summary>
    /// Obtiene registros del catalogo PermissionsLevels
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="permissionsLevels">Contiene filtros adicionales</param>
    /// <returns>Lista de tipo PermissionsLevel</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    public static List<PermissionLevel> GetPermissionsLevels(int nStatus = -1, PermissionLevel permissionsLevels = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pl in dbContext.PermissionsLevels
                    select pl;

        if (nStatus != -1)//filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(pl => pl.plA == blnStatus);
        }

        if (permissionsLevels != null)//Verificamos que se tenga un objeto
        {
          if (permissionsLevels.plID > 0)//Filtro por ID
          {
            query = query.Where(pl => pl.plID == permissionsLevels.plID);
          }

          if (!string.IsNullOrWhiteSpace(permissionsLevels.plN))//Filtro por Descripción
          {
            query = query.Where(pl => pl.plN.Contains(permissionsLevels.plN));
          }
        }

        return query.OrderBy(pl => pl.plN).ToList();
      }
    }
    #endregion

    #region SavePermissions
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo PermissionsLevels
    /// </summary>
    /// <param name="permissionLevel">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <returns>0. No se guardó | 1. Si se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 11/04/2016
    /// </history>
    public static int SavePermissions(PermissionLevel permissionLevel, bool blnUpdate)
    {
      using (var dbContex = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)
        {
          dbContex.Entry(permissionLevel).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Insert
        else
        {
          PermissionLevel PermissionVal = dbContex.PermissionsLevels.Where(pl => pl.plID == permissionLevel.plID).FirstOrDefault();
          if (PermissionVal != null)//Verificamos si existe un objeto con el mismo ID
          {
            return -1;
          }
          else
          {
            dbContex.PermissionsLevels.Add(permissionLevel);
          }
        }
        #endregion

        return dbContex.SaveChanges();
      }
    } 
    #endregion
  }
}
