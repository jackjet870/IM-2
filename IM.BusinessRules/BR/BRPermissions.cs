using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

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
    public async static Task<List<Permission>> GetPermissions(int nStatus = -1, Permission permision = null)
    {
      List<Permission> lstPermission = await Task.Run(() =>
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
        });
      return lstPermission;
    }
    #endregion

    #region GetPersonnelPermision
    /// <summary>
    /// Develve los permisos del usuario
    /// </summary>
    /// <param name="idUser">id del usuario a buscar PersonnelPermision</param>
    /// <returns>Lista de permision</returns>
    /// <history>
    /// [emoguel] created 16/06/2016
    /// </history>
    public async static Task<List<PersonnelPermission>> GetPersonnelPermission(string idUser)
    {
      List<PersonnelPermission> lstPersonnelPermission = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.PersonnelPermissions.Where(pp => pp.pppe == idUser).ToList();
        }
      });
      return lstPersonnelPermission;
    } 
    #endregion
  }
}
