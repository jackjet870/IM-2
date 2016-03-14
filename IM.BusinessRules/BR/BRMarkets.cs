using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRMarkets
  {
    #region GetMarkets

    /// <summary>
    /// Obtiene el catalogo de mercados
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns>List<MarketShort></returns>
    /// <hystory>
    /// [erosado] 08/03/2016  created
    /// </hystory>

    public static List<MarketShort> GetMarkets(int status)
    {
      using (var dbContext = new IM.Model.IMEntities())
      {
        return dbContext.USP_OR_GetMarkets(Convert.ToByte(status)).ToList();
      }
    }

    /// <summary>
    ///Devuelve una lista de markets con todos sus datos incluyendo el status
    /// </summary>
    /// <param name="market">Objeto para filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. Registros Activos</param>
    /// <returns>Lista de markets</returns>
    /// <history>
    /// [emoguel]created 09/03/2016
    /// </history>
    public static List<Market> GetMarkets(Market market,int nStatus=-1)    
    {
      using (var dbContext = new IMEntities())
      {
        var query = from mkt in dbContext.Markets
                    select mkt;

        if(nStatus!=-1)//Filtro por status
        {
          query = query.Where(mkt => mkt.mkA == market.mkA);
        }

        if(!string.IsNullOrWhiteSpace(market.mkID))//Filtro por ID
        {
          query = query.Where(mkt=>mkt.mkID==market.mkID);
        }

        if(!string.IsNullOrWhiteSpace(market.mkN))//Filtro por Nombre(Descripcion)
        {
          query = query.Where(mkt=>mkt.mkN==market.mkN);
        }
        return query.ToList();
      }
    }

    #endregion
  }
}
