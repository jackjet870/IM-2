using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Clase que se utiliza para el log del Login
  /// </summary>
  public class BRLoginLogs
  {
    #region SaveGuestLog
    /// <summary>
    /// Guarda los cambios del log del Login
    /// </summary>
    ///<param name="location">Holtel del cual se logueo el usuario</param>
    /// <param name="user">Usuario logueado</param>
    /// <param name="computerName">Nombre de la computadora en donde logueo</param>
    /// <history>[jorcanche] 09/03/2016</history>
    public static void SaveGuestLog(string location, string user, string computerName)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        dbContext.USP_OR_SaveLoginLog(location, user, computerName);
      }
    }
    #endregion

    #region GetLoginsLogPCName
    /// <summary>
    /// Obtiene la lista de Nombres de Pc del historico de accesos.
    /// </summary>
    /// <history>[edgrodriguez] 27/04/2016</history>
    public static List<string> GetLoginsLogPCName()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.LoginsLogs.Select(c => c.llPCName).OrderBy(c=>c).Distinct().ToList();
      }
    }
    #endregion
  }
}
