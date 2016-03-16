using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRCountries
  {
    #region GetCountries

    /// <summary>
    /// Obtiene el catalogo de paises 
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns>List<CountryShort></returns>
    /// <hystory>
    /// [erosado] 08/03/2016  created
    /// </hystory>

    public static List<CountryShort> GetCountries(int status)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetCountries(Convert.ToByte(status)).ToList();
      }
    }


    /// <summary>
    /// Devuelve una lista de tipo country con todos sus datos
    /// </summary>
    /// <param name="country">Objeto con los filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. registros inactivos | 1.Registros Activos</param>
    /// <returns>Devuelve una lista de tipo country</returns>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// </history>
    public static List<Country> GetCountries(Country country,int nStatus=-1)
    {
      using (var dbContext = new IMEntities())
      {
        var query = from ct in dbContext.Countries
                    select ct;

        if(nStatus!=-1)//Filtro por status
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(ct=>ct.coA==blnStatus);
        }

        if(!string.IsNullOrWhiteSpace(country.coID))//Filtro por ID
        {
          query = query.Where(ct => ct.coID == country.coID);
        }

        if(!string.IsNullOrWhiteSpace(country.coN))//Filtro por descripcion(Nombre)
        {
          query.Where(ct=>ct.coN==country.coN);
        }

        return query.OrderBy(ct => ct.coN).ToList();
      }
    }
    #endregion

    #region saveCountry

    public static int saveCountry(Country country,bool blnUpdate)
    {
      int nRes = 0;
      using (var dbContext = new IMEntities())
      {
        if(blnUpdate)//Si es actualizar
        {
          dbContext.Entry(country).State = System.Data.Entity.EntityState.Modified;
          nRes = dbContext.SaveChanges();
        }
        else//Si es insertar
        {
          Country countryValid = dbContext.Countries.Where(co => co.coID == country.coID).FirstOrDefault<Country>();
          if (countryValid != null)//Existe un registro con el mismo ID
          {
            nRes = 2;
          }
          else
          {
            dbContext.Countries.Add(country);
            nRes = dbContext.SaveChanges();
          }
        }
      }

      return nRes;
    }
    #endregion
  }
}
