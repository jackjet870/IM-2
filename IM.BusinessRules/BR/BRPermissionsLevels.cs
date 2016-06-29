using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

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
    public async static Task<List<PermissionLevel>> GetPermissionsLevels(int nStatus = -1, PermissionLevel permissionsLevels = null)
    {
      List<PermissionLevel> lstPermission = await Task.Run(() =>
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
        });
      return lstPermission;
    }
    #endregion
  }
}
