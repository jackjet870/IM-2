using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRSourcePayments
  {

    #region GetSourcesPayments
    /// <summary>
    /// Obtiene la lista de sources payments de acuerdo al estatus
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 15/Abril/2016 Created
    /// </history>
    public static List<SourcePayment> GetSourcesPayments(int status = -1)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool _status = Convert.ToBoolean(status);

        return dbContext.SourcePayments.Where(x => x.sbA == _status).ToList();
      }
    } 
    #endregion

  }
}
