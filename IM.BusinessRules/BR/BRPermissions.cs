using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRPermissions
  {

    #region GetPermissions
    /// <summary>
    /// Obtiene registros del catalogo Permissions
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1.Activos</param>
    /// <param name="permision">objeto con filtros adicionales</param>
    /// <returns>Lista de tipo Permision</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    public static List<Permission> GetPermissions(int nStatus = -1, Permission permision = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from pm in dbContext.Permissions
                    select pm;

        if (nStatus != -1)//Filtro por estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(pm => pm.pmA == blnStatus);
        }

        if (permision != null)
        {
          if (!string.IsNullOrWhiteSpace(permision.pmID))//filtro por ID
          {
            query = query.Where(pm => pm.pmID == permision.pmID);
          }

          if (!string.IsNullOrWhiteSpace(permision.pmN))//Filtro por Descripción
          {
            query = query.Where(pm => pm.pmN.Contains(permision.pmN));
          }
        }

        return query.OrderBy(pm => pm.pmN).ToList();
      }
    }
    #endregion

    #region SavePermission
    /// <summary>
    /// Actualiza|Agrega un registro en el catalogo Permissions
    /// </summary>
    /// <param name="permission">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | false. Agrega</param>
    /// <returns>0. No se guardó | 1. Se guardó  | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    public static int SavePermission(Permission permission,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//SI es actualiza
        {
          dbContext.Entry(permission).State = System.Data.Entity.EntityState.Modified;
        }
        #endregion
        #region Add
        else
        {
          Permission permissionVal = dbContext.Permissions.Where(pm => pm.pmID == permission.pmID).FirstOrDefault();
          if(permissionVal!=null)//Verificamos si existe un registro con el mismo ID
          {
            return -1;
          }
          else
          {
            dbContext.Permissions.Add(permission);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
