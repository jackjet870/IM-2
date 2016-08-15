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
    /// <param name="exchangeRate"> Entidad ExchangeRateDate</param>
    /// <param name="currencyID">Clave de la moneda</param>
    /// <param name="date">Fecha del servidor </param>
    /// <param name="HoursDif"> Diferencia de horas del SalesRoom</param>
    /// <param name="ChangedBy"> Clave de la persona que realiza la accion </param>
    /// <history>
    /// [vipacheco] 14/03/2016 Created
    /// [vipacheco] 10/Agosto/2016 Modified se agregó asincronia y se agrego la transaccion
    /// </history>
    public static void SaveExchangeRate(bool bUpd, ExchangeRate exchangeRate, string currencyID, DateTime date, short HoursDif, string ChangedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
        {
          try
          {
            // Insertamos un nuevo Exchange
            if (bUpd) { dbContext.Entry(exchangeRate).State = System.Data.Entity.EntityState.Added; }
            // Actualizamos un Exchange existente
            else { dbContext.Entry(exchangeRate).State = System.Data.Entity.EntityState.Modified; }

            // Guadarmos el Log del cambio.
            dbContext.USP_OR_SaveExchangeRateLog(currencyID, date, HoursDif, ChangedBy);

            // Guardamos los cambios
            dbContext.SaveChanges();
            transaction.Commit();
          }
          catch
          {
            transaction.Rollback();
            throw;
          }
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
