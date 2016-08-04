using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRMaritalStatus
  {
    #region GetMaritalStratus
    /// <summary>
    /// Obtiene la lista de estados civiles
    /// </summary>
    /// <param name="status">-1 Todos los registros | 0. Registros inactivos | 1. Registros activos</param>
    /// <returns></returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// [emoguel] modified 01/04/2016--->Se agregaron filtros opcionales
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// [erosado] 04/08/2016 Modified. Se estandarizó el valor que retorna.
    /// </history>
    public async static Task<List<MaritalStatus>> GetMaritalStatus(int status = -1, MaritalStatus maritaStatus = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from ms in dbContext.MaritalStatusList
                      select ms;
          if (status != -1)//Filtro por Estatus
          {
            bool blnStatus = Convert.ToBoolean(status);
            query = query.Where(ms => ms.msA == blnStatus);
          }
          #region Filtros adicionales
          if (maritaStatus != null)//Se verifica que se tenga un objeto
          {
            if (!string.IsNullOrWhiteSpace(maritaStatus.msID))//filtro por ID
            {
              query = query.Where(ms => ms.msID == maritaStatus.msID);
            }
            if (!string.IsNullOrWhiteSpace(maritaStatus.msN))//Filtro por Descripcion
            {
              query = query.Where(ms => ms.msN.Contains(maritaStatus.msN));
            }
          }
          #endregion

          return query.OrderBy(ms => ms.msN).ToList();
        }
      });
    }
    #endregion
  }
}
