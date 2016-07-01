using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRCxC
  {
    #region GetCxC
    /// <summary>
    /// Obtiene las CxC cargadas a los PR
    /// </summary>
    /// <param name="authorized">Indica si se desean las CxC autorizadas</param>
    /// <param name="salesRoom">Clave de la sala de ventas</param>
    /// <param name="user">Clave de usuario</param>
    /// <param name="dateFrom">Fecha desde</param>
    /// <param name="dateTo">Fecha hasta</param>
    /// <param name="leadSource">Clave del Lead Source</param>
    /// <param name="personnel">Clave del PR</param>
    /// <returns>CxC cargadas a los PR</returns>
    /// <history>
    /// [michan] 01/Junio/2016 Created
    /// </history>
    public async static Task<List<CxCData>> GetCxC(bool authorized, string salesRoom, string user, DateTime? dateFrom, DateTime? dateTo, string leadSource = null, string personnel = null)
    {
      List<CxCData> cxcData = new List<CxCData>();
      await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          cxcData = dbContext.USP_OR_GetCxC(authorized, salesRoom, user, dateFrom.Value, dateTo.Value, leadSource, personnel).ToList();
        }
      });
      return cxcData;
    }
    #endregion

    #region GetCxCPayments
    /// <summary>
    /// Obtiene los pagos de CxC de un recibo de regalos
    /// </summary>
    /// <param name="giftReceiptID"></param>
    /// <returns></returns>
    /// <history>
    /// [michan] 14/Junio/2016 Created
    /// </history>
    public async static Task<List<CxCPaymentShort>> GetCxCPayments(int giftReceiptID)
    {
      List<CxCPaymentShort> cxcData = new List<CxCPaymentShort>();
      await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          cxcData = dbContext.USP_OR_GetCxCPayments(giftReceiptID.ToString()).ToList();
        }
      });
      return cxcData;
    }
    #endregion
  }
}
