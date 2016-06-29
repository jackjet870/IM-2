using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

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
    /// [erosado] 19/05/2016  Modified. Se agregó asincronía
    /// </hystory>
    public async static Task<List<CountryShort>> GetCountries(int status)
    {
      List<CountryShort> result = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          result = dbContext.USP_OR_GetCountries(Convert.ToByte(status)).ToList();
        }
      });
      return result;
    }
    #endregion

    #region GetCountries
    /// <summary>
    /// Devuelve una lista de tipo country con todos sus datos
    /// </summary>
    /// <param name="country">Objeto con los filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. registros inactivos | 1.Registros Activos</param>
    /// <returns>Devuelve una lista de tipo country</returns>
    /// <history>
    /// [emoguel] created 14/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// [emoguel] modified 06/06/2016--->Se volvió async
    /// </history>
    public async static Task<List<Country>> GetCountries(Country country = null, int nStatus = -1)
    {
      List<Country> lstCountries = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
          {
            var query = from ct in dbContext.Countries
                        select ct;

            if (nStatus != -1)//Filtro por status
          {
              bool blnStatus = Convert.ToBoolean(nStatus);
              query = query.Where(ct => ct.coA == blnStatus);
            }

            if (country != null)//Valida si se tiene un objeto
          {
              if (!string.IsNullOrWhiteSpace(country.coID))//Filtro por ID
            {
                query = query.Where(ct => ct.coID == country.coID);
              }

              if (!string.IsNullOrWhiteSpace(country.coN))//Filtro por descripcion(Nombre)
            {
                query = query.Where(ct => ct.coN.Contains(country.coN));
              }
            }
            return query.OrderBy(ct => ct.coN).ToList();
          }
        });
      return lstCountries;
    }
    #endregion

    #region TransferAddCountries

    /// <summary>
    /// Agrega las paises en el proceso de transferencia
    /// </summary>
    /// <hystory>
    /// [michan] 13/04/2016  created
    /// </hystory>

    public static int TransferAddCountries()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_TransferAddCountries();
      }
    }
    #endregion

  }
}
