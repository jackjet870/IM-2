using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.BusinessRules.BR;
using IM.Model;
using IM.Model.Helpers;

namespace IM.Host.Classes
{
  /// <summary>
  /// Clase para el manejo de los SalesSalesmen
  /// </summary>
  /// <history>
  /// [jorcanche] created 01072016
  /// </history>
 public class SalesSalesmen
  {
    /// <summary>
    /// Guarda los movimientos de los SalesSalesmen
    /// </summary>
    /// <param name="salesSalesmans">Listado de los SalesSalesmen</param>
    /// <param name="saId">Id de la venta</param>
    /// <param name="amount">monto de la venta </param>
    /// <param name="saleAmountOriginal">monto de la venta original</param>
    /// <history>
    /// [jorcanche]  created 01072016
    /// </history>
    public static async void SaveSalesSalesmen(List<SalesSalesman> salesSalesmans, int saId, decimal amount, decimal saleAmountOriginal)
    {
      //1.-Extraemos el Listado de los SaleMan que se modificaron 
      //2.-Se elimina la propiedad virtual Persaonel para que no marque error de repeticion de llaves,
      // ya que personel tiene la llave de peID y la llave de la tabla SalesSaleman tiene igual la llave smpe y marcan conflicto
      // var lstModSalesSaleman = lstSaleman.Where(sa => !_oldList.Any(agg => agg.smpe == sa.smpe && ObjectHelper.IsEquals(sa, agg))).ToList();
      var lstModSalesSaleman = new List<SalesSalesman>();
      salesSalesmans.Where(ss => !ss.smSale || ss.smSaleAmountOwn != amount || ss.smSaleAmountWith != amount).
        ToList().ForEach(x =>
        {
          var ss = new SalesSalesman();
          ObjectHelper.CopyProperties(ss, x);
          lstModSalesSaleman.Add(ss);
        });
      //Eliminamos todos los registros de la tabla SalesSalesmen que sean de este sale
      await BRSalesSalesmen.DeleteSalesSalesmenbySaleId(saId);
      //Se guardan los SalesSaleman que se modificaron       
      foreach (var salessalesmen in lstModSalesSaleman)
      {
        await BREntities.OperationEntity(salessalesmen, Model.Enums.EnumMode.add);
      }
    }
  }
}
