using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
    /// </history>
    public static List<Currency> GetCurrencies(Currency currency, int nStatus=-1, List<string> exceptCurrencyID = null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from c in dbContext.Currencies
                    select c;

        if(nStatus!=-1)//filtro por estatus
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

        if(currency != null)
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
        
        return query.OrderBy(c=>c.cuN).ToList();
      }
    }
        
    #endregion

    #region SaveCurrency
    /// <summary>
    /// Actualiza|agrega un registro al catalogo de currencies
    /// </summary>
    /// <param name="currency">objeto a guardar en la BD</param>
    /// <param name="blnUpd">true. para actualizar | false. para agregar</param>
    /// <returns>0. No se pudo guardar el registro | 1. El registro se guardo | 2.- Existe un registro con el mismo ID</returns>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    public static int saveCurrency(Currency currency,bool blnUpd)
    {
      int nRes = 0;
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        if(blnUpd)//Actualizar
        {
          dbContext.Entry(currency).State = System.Data.Entity.EntityState.Modified;
          nRes=dbContext.SaveChanges();
        }
        else//Insertar
        {
          Currency currencyVal = dbContext.Currencies.Where(c=>c.cuID==currency.cuID).FirstOrDefault();
          if(currencyVal!=null)//Existe un registro con el mismo ID
          {
            nRes = 2;
          }
          else//NO existe un registro con el mismo ID
          {
            dbContext.Currencies.Add(currency);
            nRes = dbContext.SaveChanges();
          }
        }
      }
      return nRes;
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
