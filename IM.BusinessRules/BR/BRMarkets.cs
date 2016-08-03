using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRMarkets
  {
    #region GetMarkets

    /// <summary>
    /// Obtiene el catalogo de mercados
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns><list type="MarketShort"></list></returns>
    /// <hystory>
    /// [erosado] 08/03/2016  created
    /// [aalcocer] 25/05/2016  Modified. Se agregó asincronía
    /// </hystory>
    public static async Task<List<MarketShort>> GetMarkets(int status)
    {
      var result = new List<MarketShort>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.USP_OR_GetMarkets(Convert.ToByte(status)).ToList();
        }
      });
      return result;
    }

    #endregion GetMarkets

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
    public async static Task<List<Market>> GetMarkets(Market market = null, int nStatus = -1)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from mkt in dbContext.Markets
                      select mkt;

          if (nStatus != -1)//Filtro por status
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(mkt => mkt.mkA == blnStatus);
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
      });
    }

    #endregion GetMarkets

    #region SaveMarket
    /// <summary>
    /// Agrega|Actualiza un registro en el catalogo market
    /// Asigna|Desasigna agencies al market
    /// </summary>
    /// <param name="market">Objeto a guardar</param>
    /// <param name="lstAdd">Agencias a asignar</param>
    /// <param name="lstDel">Agencias a desasignar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns></returns>
    /// <history>
    /// [emoguel] modified 29/07/2016-->Se volvió async
    /// </history>
    public static async Task<int> SaveMarket(Market market, List<Agency> lstAdd, bool blnUpdate)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region UpdateMarket
              if (blnUpdate)
              {
                dbContext.Entry(market).State = EntityState.Modified;
              }
              #endregion
              #region Add market
              else
              {
                if (dbContext.Markets.Where(mk => mk.mkID == market.mkID).FirstOrDefault() != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.Markets.Add(market);
                }
              }
              #endregion

              #region Agencies
              //Add markets
              dbContext.Agencies.AsEnumerable().Where(ag => lstAdd.Any(agg => agg.agID == ag.agID)).ToList().ForEach(ag =>
              {
                ag.agmk = market.mkID;
              });
              #endregion
              int nRes = dbContext.SaveChanges();
              transacction.Commit();
              return nRes;
            }
            catch
            {
              transacction.Rollback();
              return 0;
            }
          }
        }
      });
    } 
    #endregion
  }
}