using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGiftsReceiptsPayments
  {

    #region GetGiftsReceiptPaymentsShort
    /// <summary>
    /// Consulta los pagos de un recibo de regalos
    /// </summary>
    /// <param name="receipt"> Clave del recibo de regalos </param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 08/04/2016 Created
    /// </history>
    public async static Task<List<GiftsReceiptPaymentShort>> GetGiftsReceiptPaymentsShort(int receipt)
    {
      List<GiftsReceiptPaymentShort> lstResult = new List<GiftsReceiptPaymentShort>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          lstResult = dbContext.USP_OR_GetGiftsReceiptPayments(receipt).ToList();
        }
      });

      return lstResult;
    }
    #endregion

    #region GetGiftsReceiptPayments
    /// <summary>
    /// Consulta los pagos de un recibo de regalos
    /// </summary>
    /// <param name="receipt"> Clave del recibo de regalos </param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 08/04/2016 Created
    /// </history>
    public static List<GiftsReceiptPayment> GetGiftsReceiptPayments(int GiftsReceipt)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.GiftsReceiptsPayments.Where(x => x.gygr == GiftsReceipt).ToList();
      }
    }
    #endregion

    #region GetGiftReceiptPayment
    /// <summary>
    /// Obtiene un GiftsReceiptPayment de acuerdo a los criterios ingresados
    /// </summary>
    /// <param name="Receipt"></param>
    /// <param name="GiftPaymentID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 07/Mayo/2016 Created
    /// </history>
    public static GiftsReceiptPayment GetGiftReceiptPayment(int Receipt, int GiftPaymentID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.GiftsReceiptsPayments.Where(x => x.gygr == Receipt && x.gyID == GiftPaymentID).SingleOrDefault();
      }
    }
    #endregion

    #region AddGiftReceiptPayment
    /// <summary>
    /// 
    /// </summary>
    /// <param name="GiftReceiptID"></param>
    /// <param name="ReceivedBy"></param>
    /// <param name="ReceivedDate"></param>
    /// <param name="USDAmount"></param>
    /// <param name="RateAmount"></param>
    /// <param name="MXNAmount"></param>
    /// <returns></returns>
    public async static Task<List<AddGiftReceiptPayment>> AddGiftReceiptPayment(int GiftReceiptID, string ReceivedBy, DateTime ReceivedDate, decimal USDAmount, decimal RateAmount, decimal MXNAmount)
    {
      List<AddGiftReceiptPayment> _AddGiftReceiptPayment = new List<AddGiftReceiptPayment>();
      await Task.Run(() => { 
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          _AddGiftReceiptPayment = dbContext.USP_OR_AddGiftReceiptPayment(GiftReceiptID, ReceivedBy, ReceivedDate, USDAmount, RateAmount, MXNAmount).ToList();
        }
      });
      return _AddGiftReceiptPayment;
    }
    #endregion
  }
}
