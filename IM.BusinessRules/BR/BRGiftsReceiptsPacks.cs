using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRGiftsReceiptsPacks
  {

    #region GetGiftsReceiptPackage
    /// <summary>
    /// Consulta los items de un paquete de un recibo
    /// </summary>
    /// <param name="receipt"> Clave del recibo de regalos </param>
    /// <param name="package"> Clave del paquete de regalos </param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 25/Abril/2016 Created
    /// [vipachecp] 21/Junio/2016 Modified --> Se elimino la consulta al Store USP_OR_GetGiftsReceiptPackage y se agrego la consulta por LinQ para trabajar con la entidad pura
    /// </history>
    public async static Task<List<GiftsReceiptPackageItem>> GetGiftsReceiptPackage(int receipt, string package)
    {
      List<GiftsReceiptPackageItem> lstResult = new List<GiftsReceiptPackageItem>();

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from packs in dbContext.GiftsReceiptsPackagesItems
                      join gifts in dbContext.Gifts on packs.gkgi equals gifts.giID into result
                      from b in result.DefaultIfEmpty()
                      where b.giA == true
                      select packs;

          lstResult = query.Where(x => x.gkgr == receipt && x.gkPack == package).ToList();
        }
      }); 

      return lstResult;
    }
    #endregion

    #region GetGiftsReceiptPackageByID
    /// <summary>
    /// Obtiene los Gifts Receipt de paquetes por un GiftsReceipt ID en especifico
    /// </summary>
    /// <param name="receiptID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 18/Junio/2016 Created
    /// </history>
    public static List<GiftsReceiptPackageItem> GetGiftsReceiptPackageByID(int receiptID)
    {
      List<GiftsReceiptPackageItem> lstResult = new List<GiftsReceiptPackageItem>();

      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        lstResult = dbContext.GiftsReceiptsPackagesItems.Where(x => x.gkgr == receiptID).ToList();
      }

      return lstResult;
    } 
    #endregion

  }
}
