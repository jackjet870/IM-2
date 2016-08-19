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
    /// <returns><Dictionary>(string, int)giftId,cantidad></Dictionary></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    internal static Dictionary<string, int> ProductionByGiftQuantity => Settings.Default.ProductionByGiftQuantity.Cast<string>().
      Select(y => y.Split('-')).ToDictionary(z => z[1].Trim(), z => Convert.ToInt32(z[0].Trim()));

    /// <summary>
    /// Agencias del reporte de produccion mensual por agencia
    /// </summary>
    /// <returns><list type="string"></list></returns>
    /// <history>
    /// [aalcocer] 23/03/2016 Created
    /// </history>
    internal static List<string> ProductionByAgencyMonthly => Settings.Default.ProductionByAgencyMonthly.Cast<string>().ToList();

    internal static string StrAll => Settings.Default.strALL;

    internal static string StrLeadSources => Settings.Default.strLeadSources;

    internal static string StrChargeTo => Settings.Default.strChargeTo;

    internal static string StrMarkets => Settings.Default.strMarkets;

    internal static string StrAgencies => Settings.Default.strAgencies;

    internal static string StrPR => Settings.Default.strPR;

    internal static string StrGifts => Settings.Default.strGifts;
  }
}