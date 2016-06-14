using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRCurrencies
  {
    #region GetCurrencies
    /// <summary>
    /// devuelve datos del catalogo currencies
    /// </summary>
    /// <param name="currency">objeto con filtros adicionales</param>
    /// <param name="nStatus">-1. todos los registros | 0. registros inactivos | 1. registros activos</param>
    /// <param name="exceptCurrencyID"> lista de currencies ID a excluir, incluido US</param>
    /// <returns>lista de tipo currency</returns>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// [vipacheco] 10/03/2016 Modified --> Se agregó validacion de Object Currency vacía.
    /// [vipacheco] 11/03/2016 Modified --> Se agregó parametro nuevo para excluir alguna lista currencies en especifico por su ID
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </history>
    public async static Task<List<Currency>> GetCurrencies(Currency currency = null, int nStatus = -1, List<string> exceptCurrencyID = null)
    {
      List<Currency> Result = null;

      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from c in dbContext.Currencies
                      select c;

          if (nStatus != -1)//filtro por estatus
          {
            bool blnEstatus = Convert.ToBoolean(nStatus);

            if (exceptCurrencyID != null) // Verifica si se desea excluir alguna currency en especifico
            {
              query = query.Where(c => !exceptCurrencyID.Contains(c.cuID) && c.cuID != "US" && c.cuA == blnEstatus);
            }
            else
            {
              query = query.Where(c => c.cuA == blnEstatus);
            }
          }
          if (currency != null)
          {
            if (!string.IsNullOrWhiteSpace(currency.cuID))//Filtro por ID
            {
              query = query.Where(c => c.cuID == currency.cuID);
            }

            if (!string.IsNullOrWhiteSpace(currency.cuN))//filtro por nombre
            {
              query = query.Where(c => c.cuN.Contains(currency.cuN));
            }
          }
          Result = query.OrderBy(c => c.cuN).ToList();
        }
      });

      return Result;
    }
    #endregion

    #region GetCurrencyId
    /// <summary>
    /// Obtiene una moneda en específico
    /// </summary>
    /// <param name="currencyId">Identificador de la moneda</param>
    /// <returns>Currency</returns>
    /// <history>
    /// [lchairez] 23/03/2016 Created.
    /// </history>
    public static Currency GetCurrencyId(string currencyId)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.Currencies.Where(c => c.cuID == currencyId).SingleOrDefault();
      }
    }
    #endregion
  }
}
