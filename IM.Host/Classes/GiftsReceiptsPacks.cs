using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Host.Classes
{
  public class GiftsReceiptsPacks
  {

    #region PrepareToSave
    /// <summary>
    ///  Prepara los regalos de paquetes para ser guardados
    /// </summary>
    /// <param name="ReceiptID"></param>
    /// <param name="Cancel"></param>
    /// <history>
    /// [vipacheco] 17/Junio/2016 Created
    /// </history>
    public static List<GiftsReceiptPackageItem> PrepareToSave(int ReceiptID, ref bool MustSave)
    {
      List<GiftsReceiptPackageItem> lstResult = new List<GiftsReceiptPackageItem>();
       MustSave = false;

      // Obtenemos la lista de Gifts de paquete
      lstResult = BRGiftsReceiptsPacks.GetGiftsReceiptPackageByID(ReceiptID);
     
      return lstResult;
    }
    #endregion


    public static void Update(int receiptID, string package, EnumMode Action)
    {

    }

  }
}
