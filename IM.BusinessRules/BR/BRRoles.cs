using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRRoles
  {
    #region GetRoles
    /// <summary>
    /// Obtiene registros del catalogo Roles
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1.Activos</param>
    /// <param name="role">objeto con filtros adicionales</param>
    /// <returns>Lista tipo Roles</returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    public async static Task<List<Role>> GetRoles(int nStatus = -1, Role role = null)
    {
      List<Role> lstRoles = await Task.Run(() =>
       {
         using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
         {
           var query = from ro in dbContext.Roles
                       select ro;
           if (nStatus != -1)
           {
             bool blnStatus = Convert.ToBoolean(nStatus);
             query = query.Where(ro => ro.roA == blnStatus);
           }

           if (role != null)
           {
             if (!string.IsNullOrWhiteSpace(role.roID))//Filtro por ID
            {
               query = query.Where(ro => ro.roID == role.roID);
             }

             if (!string.IsNullOrWhiteSpace(role.roN))//Filtro por Descripcion
            {
               query = query.Where(ro => ro.roN.Contains(role.roN));
             }
           }

           return query.OrderBy(ro => ro.roN).ToList();
         }
       });
      return lstRoles;
    }
    #endregion

    #region GetRolesByUser
    /// <summary>
    /// Devuelve los roles de un usuario
    /// </summary>
    /// <param name="idUser">id del usuario a devolver roles</param>
    /// <returns>Lista de roles</returns>
    /// <history>
    /// [emoguel] created 16/06/2016
    /// </history>
    public static async Task<List<Role>> GetRolesByUser(string idUser)
    {
      List<Role> lstRoles = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var person = dbContext.Personnels.Where(pe => pe.peID == idUser).Include("Roles").FirstOrDefault();

            return person.Roles.ToList();
          }
        });

      return lstRoles;
    }
    #endregion
  }
}
