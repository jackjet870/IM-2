using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{

  /// <summary>
  /// Clase manejador de reglas de tipo ExchengeRate
  /// </summary>
  /// <history>
  /// [vipacheco] 04/03/2016 Created
  /// </history>
  public class BRExchangeRate
  {
    #region GetGetExchangeRatesWithPesosByDate
    /// <summary>
    /// Función para obtener los tipos de cambio del dia seleccionado
    /// </summary>
    /// <param name="date"></param>
    /// <returns> Lista de tipo GetExchangeRatesWithPesosByDate </returns>
    /// <history>
    /// [vipacheco] 04/03/2016 Created
    /// </history>
    public static List<ExchangeRateData> GetGetExchangeRatesWithPesosByDate(DateTime? dateSelected)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetExchangeRatesWithPesosByDate(dateSelected).ToList();
      }
    }
    #endregion

    #region InsertExchangeRate
    /// <summary>
    /// Función para actualizar los tipos de cambio de monedas hasta la fecha actual
    /// </summary>
    /// <param name="serverDate"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    public static void InsertExchangeRate(DateTime serverDate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        #region Transaction
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            dbContext.USP_OR_InsertExchangeRate(serverDate);
          }
          catch
          {
            transaction.Rollback();
          }
        }
        #endregion
      }
    }
    #endregion

    #region SaveExchangeRate
    /// <summary>
    /// Función para guardar un nuevo Exchange Rate agregado
    /// </summary>
    /// <param name="bUpd"> true - Insertar | false - Update </param>
    /// <param name="exchangeRateData"></param>
    /// <history>
    /// [vipacheco] 14/03/2016 Created
    /// </history>
    public static void SaveExchangeRate(bool bUpd, ExchangeRate exchangeRate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        if (bUpd)
        {
          dbContext.ExchangeRates.Add(exchangeRate);
          dbContext.SaveChanges();
        }
        else
        {
          dbContext.Entry(exchangeRate).State = System.Data.Entity.EntityState.Modified;
          dbContext.SaveChanges();
        }
      }
    }
        #endregion

    #region UpdateExchangeRate

    /// <summary>
    /// Función para actualizar un Exchange Rate
    /// </summary>
    /// <param name="date">Fecha</param>
    /// <param name="currency">Moneda</param>
    /// <param name="exchangeRate">Tipo de cambio</param>
    /// <returns></returns>
    /// <history>
    ///   [michan] 06/04/2016 Created
    /// </history>
    public static void UpdateExchangeRate(DateTime? date, string currency, decimal? exchangeRate)
    {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
            dbContext.USP_OR_UpdateExchangeRate(date, currency, exchangeRate);
        }
    }

    #endregion

  }
}
