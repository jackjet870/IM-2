using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Clase para el manejo de las consultas a la tabla GiftsPacks
  /// </summary>
  /// <history>
  /// [vipacheco] 06/Julio/2016
  /// </history>
  public class BRGiftsPacks
  {

    #region GetGiftsPacks
    /// <summary>
    /// Obtiene los paquetes de gifts
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 07/Julio/2016 Created
    /// </history>
    public async static Task<List<GiftPackageItem>> GetGiftsPacks()
    {
      List<GiftPackageItem> lstResult = new List<GiftPackageItem>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          lstResult = dbContext.GiftsPackagesItems.ToList();
        }
      });

      return lstResult;
    } 
    #endregion


  }
}
