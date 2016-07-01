using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGiftsReceiptLog
  {

    #region SaveGiftsReceiptsLog
    /// <summary>
    /// Guarda el historico de recibos de regalos
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="ChangeBy"></param>
    /// <history>
    /// [vipacheco] 09/Mayo/2016 Created
    /// </history>
    public async static Task SaveGiftsReceiptsLog(int ReceiptID, string ChangeBy, int HoursDiff = 0)
    {
      await Task.Run(() => { 
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.USP_OR_SaveGiftsReceiptLog(ReceiptID, Convert.ToInt16(HoursDiff), ChangeBy);
        }
      });
    }
    #endregion

    #region GetGiftsReceiptLog
    /// <summary>
    /// Devuelve el historico de Receipt en especifico
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 07/Junio/2016 Created
    /// </history>
    public async static Task<List<GiftsReceiptLogData>> GetGiftsReceiptLog(int ReceiptID)
    {
      List<GiftsReceiptLogData> lstResult = new List<GiftsReceiptLogData>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          lstResult = dbContext.USP_OR_GetGiftsReceiptLog(ReceiptID).ToList(); ;
        }
      });

      return lstResult;
    } 
    #endregion

  }
}
