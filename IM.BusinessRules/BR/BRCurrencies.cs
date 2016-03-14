using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

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
    /// <returns>lista de tipo currency</returns>
    /// <history>
    /// [Emoguel] created 07/03/2016
    /// </history>
    public static List<Currency> GetCurrencies(Currency currency,int nStatus=-1)
    {
      using (var dbContext = new IMEntities())
      {
        var query = from c in dbContext.Currencies
                    select c;

        if(nStatus!=-1)//filtro por estatus
        {
          bool blnEstatus = Convert.ToBoolean(nStatus);
          query = query.Where(c=>c.cuA==blnEstatus);
        }

        if(!string.IsNullOrWhiteSpace(currency.cuID))//Filtro por ID
        {
          query = query.Where(c=>c.cuID==currency.cuID);
        }

        if(!string.IsNullOrWhiteSpace(currency.cuN))//filtro por nombre
        {
          query = query.Where(c=>c.cuN==currency.cuN);
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
      using (var dbContext = new IMEntities())
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
  }
}
