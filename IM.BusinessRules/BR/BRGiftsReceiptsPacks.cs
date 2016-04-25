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

    /// <summary>
    /// Consulta los items de un paquete de un recibo
    /// </summary>
    /// <param name="receipt"> Clave del recibo de regalos </param>
    /// <param name="package"> Clave del paquete de regalos </param>
    /// <returns></returns>
    public static List<GiftsReceiptPackage> GetGiftsReceiptPackage(int receipt, string package)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetGiftsReceiptPackage(receipt, package).ToList();
      }
    }

  }
}
