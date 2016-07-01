using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRWholesalers
  {
    #region GetWholesalers
    /// <summary>
    /// Obtiene los Wholesalers a traves del SP "USP_OR_GetWholesalers"
    /// </summary>
    /// <returns>Lista de tipo WholesalersData</returns>
    /// <hitory>
    /// [emoguel] created 07/06/2016
    /// </hitory>
    public async static Task<List<WholesalerData>> GetWholesalers(WholesalerData wholesalerData=null)
    {
      List<WholesalerData> lstWholesalers = await Task.Run(() =>
        {
          using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
          {
            var query = from ws in dbContext.USP_OR_GetWholesalers()
                        select ws;

            if(wholesalerData!=null)
            {
              if (wholesalerData.wscl>0)//Filtro por Club
              {
                query = query.Where(ws => ws.wscl == wholesalerData.wscl);
              }

              if(!string.IsNullOrWhiteSpace(wholesalerData.wsApplication))//Filtro por Application
              {
                query = query.Where(ws => ws.wsApplication == wholesalerData.wsApplication);
              }

              if(!string.IsNullOrWhiteSpace(wholesalerData.Name))//Filtro por Name
              {
                query = query.Where(ws => ws.Name.Contains(wholesalerData.Name));
              }
            }
            return query.OrderByDescending(ws=>ws.Name).ToList();
          }
        });

      return lstWholesalers;
    }
    #endregion

    #region SaveWholesaler
    /// <summary>
    /// Guarda un wholesaler
    /// </summary>
    /// <param name="wholesaler">Objeto a guardar</param>
    /// <returns>-1. Existe un registro con el mismo ID| 0. No se guardó | 1. Se guardó</returns>
    /// <history>
    /// [emoguel] created 07/06/2016
    /// </history>
    public async static Task<int> SaveWholesaler(Wholesaler wholesaler)
    {
      int nRes = await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          if (dbContext.Wholesalers.Where(ws => ws.wsApplication == wholesaler.wsApplication && ws.wscl == wholesaler.wscl && ws.wsCompany == wholesaler.wsCompany).FirstOrDefault() != null)
          {
            return -1;
          }
          else
          {
            dbContext.Wholesalers.Add(wholesaler);
          }
          return dbContext.SaveChanges();
        }
      });

      return nRes;
    } 
    #endregion
  }
}
