using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

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

    /// <summary>
    /// Función para obtener los tipos de cambio del dia seleccionado
    /// </summary>
    /// <param name="date"></param>
    /// <returns> Lista de tipo GetExchangeRatesWithPesosByDate </returns>
    /// <history>
    /// [vipacheco] 04/03/2016 Created
    /// </history>
    public static List<ExchangeRateData> getGetExchangeRatesWithPesosByDate(DateTime? dateSelected)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetExchangeRatesWithPesosByDate(dateSelected).ToList();
      }
    }

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
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetExchangeRateLog(currency).ToList();
      }
    }

    /// <summary>
    /// Función para actualizar los tipos de cambio de monedas hasta la fecha actual
    /// </summary>
    /// <param name="serverDate"></param>
    /// <history>
    /// [vipacheco] 07/03/2016 Created
    /// </history>
    public static void InsertExchangeRate(DateTime serverDate)
    {
      using (var dbContext = new IMEntities())
      {
        dbContext.USP_OR_InsertExchangeRate(serverDate);
      }
    }

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
      using (var dbContext = new IMEntities())
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
      using (var dbContext = new IMEntities())
      {
        dbContext.USP_OR_SaveExchangeRateLog(currency, date, HoursDif, ChangedBy);
      }
    }
  }
}
