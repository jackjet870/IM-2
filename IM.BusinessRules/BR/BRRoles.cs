using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    public static List<Role> GetRoles(int nStatus = -1, Role role = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
    }
    #endregion

     #region SaveRole
    /// <summary>
    /// Actualiza|Agrega un registro en el catalogo Roles
    /// </summary>
    /// <param name="role">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>0.no se guardó | 1. Se guardó | -1. Existe un registro con el mismo ID</returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// </history>
    public static int SaveRole(Role role,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Update
        if (blnUpdate)//Actualizar
        {
          dbContext.Entry(role).State = System.Data.Entity.EntityState.Modified;
        } 
        #endregion
        #region Add
        else
        {
          Role roleVal = dbContext.Roles.Where(ro => ro.roID == role.roID).FirstOrDefault();
          if(roleVal!=null)//Verificamos si existe un registro con el mismo ID
          {
            return -1;
          }
          else//Agregamos
          {
            dbContext.Roles.Add(role);
          }
        } 
        #endregion

        return dbContext.SaveChanges();
      }
    }
    #endregion
  }
}
