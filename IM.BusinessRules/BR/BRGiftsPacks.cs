using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

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
    /// [emoguel] modified 18/07/2016
    /// </history>
    public async static Task<List<GiftPackageItem>> GetGiftsPacks(GiftPackageItem giftPackageItem = null, bool blnIncludeGift = false)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from gp in dbContext.GiftsPackagesItems
                      select gp;

          if (blnIncludeGift)
          {
            query = query.Include(gp => gp.GiftItem);
          }
          if (giftPackageItem != null)
          {
            if (!string.IsNullOrWhiteSpace(giftPackageItem.gpPack))
            {
              query = query.Where(gp => gp.gpPack == giftPackageItem.gpPack);
            }
          }

          return query.OrderBy(gp => gp.gpgi).ToList();

        }
      });
    } 
    #endregion


  }
}
