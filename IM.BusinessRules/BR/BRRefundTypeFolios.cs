using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRRefundTypeFolios
  {

    #region GetRefundFolio
    /// <summary>
    /// Calcula el Folio
    /// </summary>
    /// <param name="pRefundType"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 21/Julio/2016 Created
    /// </history>
    public async static Task<int> GetRefundFolio(string pRefundType)
    {
      int folioResult = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var result = dbContext.RefundTypesFolios.Where(x => x.rtfrf == pRefundType).Select(s => s.rtfFolio).SingleOrDefault();

          if (result != 0)
          {
            folioResult = result + 1;
          }
          else
          {
            folioResult = 1;
          }
        }
      });

      return folioResult;
    }
    #endregion

    #region UpdateRefundFolio
    /// <summary>
    /// nserta o modifica el último folio de tipo de refund
    /// </summary>
    /// <param name="pRefundTypeID"></param>
    /// <param name="pFolio"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 21/Julio/2016 Created
    /// </history>
    public async static Task UpdateRefundFolio(string pRefundTypeID, int pFolio)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.USP_OR_UpdateRefundFolio(pRefundTypeID, pFolio);
        }
      });
    } 
    #endregion

  }
}
