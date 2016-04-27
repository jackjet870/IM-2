using IM.ProcessorInhouse.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.ProcessorInhouse.Classes
{
  internal static class GetSettings
  {
    /// <summary>
    /// Cantidades y regalos para el reporte de produccion por regalo y cantidad
    /// </summary>
    /// <returns>Dictionary<string, int>giftId,cantidad</returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    public static Dictionary<string, int> ProductionByGiftQuantity()
    {
      return Settings.Default.ProductionByGiftQuantity.Cast<string>().
        Select(y => y.Split('-')).
        ToDictionary(z => z[1].Trim(), z => Convert.ToInt32(z[0].Trim()));
    }

    /// <summary>
    /// Agencias del reporte de produccion mensual por agencia
    /// </summary>
    /// <returns>List<string></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    public static List<string> ProductionByAgencyMonthly()
    {
      return Settings.Default.ProductionByAgencyMonthly.Cast<string>().ToList();
    }
  }
}