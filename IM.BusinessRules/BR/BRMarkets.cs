using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetMarkets(Convert.ToByte(status)).ToList();
      }
    }
    #endregion

    #region GetMarkets

    /// <summary>
    ///Devuelve una lista de markets con todos sus datos incluyendo el status
    /// </summary>
    /// <param name="market">Objeto para filtros adicionales</param>
    /// <param name="nStatus">-1. Todos los registros | 0. Registros inactivos | 1. Registros Activos</param>
    /// <returns>Lista de markets</returns>
    /// <history>
    /// [emoguel]created 09/03/2016
    /// [emoguel] modified 17/03/2016--->Se agregó la validacion null del objeto y se cambió el filtro por descripcion a "contains"
    /// </history>
    public static List<Market> GetMarkets(Market market=null,int nStatus=-1)    
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from mkt in dbContext.Markets
                    select mkt;

        if(nStatus!=-1)//Filtro por status
        {
          query = query.Where(mkt => mkt.mkA == market.mkA);
        }

        if (market != null)//Valida si se tiene un objeto
        {
          if (!string.IsNullOrWhiteSpace(market.mkID))//Filtro por ID
          {
            query = query.Where(mkt => mkt.mkID == market.mkID);
          }

          if (!string.IsNullOrWhiteSpace(market.mkN))//Filtro por Nombre(Descripcion)
          {
            query = query.Where(mkt => mkt.mkN.Contains(market.mkN));
          }
        }
        return query.ToList();
      }
    }

    #endregion
  }
}
