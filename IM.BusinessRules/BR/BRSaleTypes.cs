using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRSaleTypes
  {
    #region GetSalesTypes
    /// <summary>
    /// Obtiene registros del catalogo SaleTypes
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Iniactivos  | 1. Activos</param>
    /// <param name="saleType">Objeto con  filtros adicionales</param>
    /// <returns>Lista de tipo SaleType></returns>
    /// <history>
    /// [emoguel] created 19/04/2016
    /// [emoguel] modifies 28/06/2016---> se volvió async
    /// </history>
    public async static Task<List<SaleType>> GetSalesTypes(int nStatus = -1, SaleType saleType = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from st in dbContext.SaleTypes
                      select st;

          if (nStatus != -1)//Filtro por estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(st => st.stA == blnStatus);
          }

          if (saleType != null)
          {
            if (!string.IsNullOrWhiteSpace(saleType.stID))//Filtro por ID
            {
              query = query.Where(st => st.stID == saleType.stID);
            }

            if (!string.IsNullOrWhiteSpace(saleType.stN))//Filtro por descripción
            {
              query = query.Where(st => st.stN.Contains(saleType.stN));
            }

            if (!string.IsNullOrWhiteSpace(saleType.ststc))//Filtro por categoria
            {
              query = query.Where(st => st.ststc == saleType.ststc);
            }
          }

          return query.OrderBy(st => st.stID).ToList();
        }
      });
    }
    #endregion

    #region GetStstcOfSaleTypeById
    /// <summary>
    /// Trae un tipo de Sale por su ID
    /// </summary>
    /// <param name="stId">Ide del sale</param>
    /// <param name="nStatus">Estatus, pueden ser: -1. Todos | 0. Iniactivos  | 1. Activos</param>
    ///<history>
    ///[jorcanche] created 29/06/2016
    ///</history> 
    public static async Task<string> GetStstcOfSaleTypeById(string stId, int nStatus = -1)
    {
      string res = string.Empty;
      await Task.Run(() =>
       {
         using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
         {
           var status = Convert.ToBoolean(nStatus);
           res = (from st in dbContext.SaleTypes
                  where st.stID == stId && (nStatus == -1 || st.stA == status)
                  select st.ststc).FirstOrDefault();
         }
       });
      return res;
    }
    #endregion

    #region GetstUpdateOfSaleTypeById
    /// <summary>
    /// Trae un boleano indicando si el Sale se esta actualizando 
    /// </summary>
    /// <param name="stId">Ide del sale</param>
    /// <param name="nStatus">Estatus, pueden ser: -1. Todos | 0. Iniactivos  | 1. Activos</param>
    ///<history>
    ///[jorcanche] created 29/06/2016
    ///</history> 
    public static async Task<bool> GetstUpdateOfSaleTypeById(string stId, int nStatus = -1)
    {
      var res = false;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var status = Convert.ToBoolean(nStatus);
          res = (from st in dbContext.SaleTypes
                 where st.stID == stId && (nStatus == -1 || st.stA == status)
                 select st.stUpdate).FirstOrDefault();
        }
      });
      return res;
    }
    #endregion
  }
}
