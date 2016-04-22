using IM.ProcessorOuthouse.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace IM.ProcessorOuthouse.Classes
{
  internal class GetSettings
  {
    #region PRPaymentCommissions
    /// <summary>
    /// Lead Sources para el pago de comisiones de PR
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vku] 20/Abr/2016 Created
    /// </history>
    public static List<string> PRPaymentCommissions()
    {
      return Settings.Default.PRPaymentCommissions.Cast<string>().ToList();
    }
    #endregion

    #region ProductionByGift
    /// <summary>
    /// Lead Sources para el pago de comisiones de PR
    /// </summary>
    /// <returns></returns>
    /// <history>
    /// [vku] 21/Abr/2016 Created
    /// </history>
    public static List<string> ProductionByGift()
    {
      return Settings.Default.ProductionByGift.Cast<string>().ToList();
    }
    #endregion
  }
}
