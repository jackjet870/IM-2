using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Reglas de negocio del historico de tipos de cambio
  /// </summary>
  /// <history>
  /// [wtorres]  23/Mar/2016 Created
  /// </history>
  public class BRExchangeRatesLogs
  {
    #region GetExchangeRateLog
    /// <summary>
    /// Función para obtener el log de exchenge rate de acuerdo a un currency especificado.
    /// </summary>
    /// <param name="currency"></param>
    /// <returns> Lista de tipo GetExchangeRateLog </returns>
    /// <history>
    /// [vipacheco] 05/03/2016 Created
    /// </history>
    public static List<ExchangeRateLogData> GetExchangeRateLog(string currency)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetExchangeRateLog(currency).ToList();
      }
    }
    #endregion

    #region SaveExchangeRateLog
    /// <summary>
    /// Función que guarda el historico de un cambio en Exchange Rate
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="date"></param>
    /// <param name="HoursDif"></param>
    /// <param name="ChangedBy"></param>
    /// <history>
    /// [vipacheco] 14/03/2016 Created
    /// </history>
    public static void SaveExchangeRateLog(string currency, DateTime date, Int16 HoursDif, string ChangedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.USP_OR_SaveExchangeRateLog(currency, date, HoursDif, ChangedBy);
      }
    } 
    #endregion
  }
}
