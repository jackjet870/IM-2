using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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
    public async static Task InsertExchangeRate(DateTime serverDate)
    {
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContext.USP_OR_InsertExchangeRate(serverDate);
        }
      });
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
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

    #region GetExchangeRateByID
    /// <summary>
    /// Obtiene un ExchangeRate por medio de su ID
    /// </summary>
    /// <param name="exchangeID"> ID del Exchange Rate</param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 13/abril/2016 Created
    /// </history>
    public static ExchangeRate GetExchangeRateByID(string exchangeID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.ExchangeRates.Where(x => x.excu == exchangeID).OrderBy(o => o.exD).FirstOrDefault();
      }
    }
    #endregion

    #region GetExchangeRatesByDate
    /// <summary>
    /// Obtiene los tipos de cambio dada una fecha
    /// </summary>
    /// <param name="_date"></param>
    /// <history>
    /// [vipacheco] 13/Abril/2016 Created
    /// </history>
    public static List<ExchangeRateShort> GetExchangeRatesByDate(DateTime? _date, string currency = "")
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetExchangeRatesByDate(_date, currency).ToList();
      }
    }
    #endregion

    #region UpdateExchangeRate
    /// <summary>
    /// Actualiza el tipo de cambio al mismo de la intranet
    /// </summary>
    /// <param name="date">Fecha</param>
    /// <param name="currency">Moneda</param>
    /// <param name="exchangeRate">Tipo de cambio</param>
    /// <returns></returns>
    /// <history>
    ///   [michan] 06/04/2016 Created
    /// </history>
    public async static Task UpdateExchangeRate(DateTime? date, string currency, decimal? exchangeRate)
    {

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              dbContext.USP_OR_UpdateExchangeRate(date, currency, exchangeRate);
            }
            catch
            {
              transaction.Rollback();
            }
          }
        }
      });
    }

    #endregion
  }
}
