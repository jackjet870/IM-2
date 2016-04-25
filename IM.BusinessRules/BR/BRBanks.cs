using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRBanks
  {

    #region GetBanks
    /// <summary>
    /// Obtiene la lista de bancos activos
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// </history>
    public static List<Bank> GetBanks(int status = -1)
    {
      using (var dbcontext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool _status = Convert.ToBoolean(status);

        return dbcontext.Banks.Where(x => x.bkA == _status).ToList();
      }
    } 
    #endregion

  }
}
