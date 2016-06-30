using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRSalesSalesmen
  {
    #region GetSalesSalesmens

    /// <summary>
    /// devuelve un boleando indicando si exite un registro de SaleAmountWith o SaleAmountOwn por venta y vendedor
    /// </summary>   
    /// <history>
    /// [jorcanche] created 09/06/2016
    /// </history>
    public static SalesSalesman GetSalesSalesmens (SalesSalesman salesSalesmen = null)
    {
      List<SalesSalesman> result = null;

        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from ss in dbContext.SalesSalesmen
                      select ss;
          if (salesSalesmen != null)
          {
            if (salesSalesmen.smsa != 0)//filtro por ID
            {
              query = query.Where(ss => ss.smsa == salesSalesmen.smsa);
            }
            if (!String.IsNullOrWhiteSpace(salesSalesmen.smpe))//Filtro por descripcion
            {
              query = query.Where(ss => ss.smpe == salesSalesmen.smpe);
            }
            if (salesSalesmen.smSale)//Filtro por descripcion
            {
              query = query.Where(ss => ss.smSale == true);
            }
            if (salesSalesmen.smSaleAmountOwn != 0)//Filtro por descripcion
            {
              query = query.Where(ss => ss.smSaleAmountOwn == salesSalesmen.smSaleAmountOwn);
            }
            if (salesSalesmen.smSaleAmountWith != 0)//Filtro por descripcion
            {
              query = query.Where(ss => ss.smSaleAmountWith == salesSalesmen.smSaleAmountWith);
            }
          }
          result = query.OrderBy(pt => pt.smsa).ToList();
        }
      return result.FirstOrDefault();
    }
    #endregion

    #region GetSalesmenChanges
    /// <summary>
    /// Obtiene una lista de SalesMenChanges segun el Sale
    /// </summary>
    /// <history>
    /// [jorcanche] created 22/06/2016
    /// </history>
    public static async Task<List<SalesmenChanges>> GetSalesmenChanges(int sale)
    {
      var res = new List<SalesmenChanges>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          res = dbContext.USP_OR_GetSalesmenChanges(sale).ToList();
        }
      });
      return res;
    }
    #endregion

    #region DeleteSalesSalesmenbySaleId

    /// <summary>
    /// Elimina uno o mas registros que contengan el Id Sale en la tabla salesSalesman
    /// </summary>   
    /// <param name="saleId">Idetificador de Sale</param>
    /// <history>
    /// [jorcanche]  created 24062016
    /// </history>
    public static async Task<int> DeleteSalesSalesmenbySaleId(int saleId)
    {
      var res = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var lstsalesSalesman = dbContext.SalesSalesmen.Where(salesSalesman => salesSalesman.smsa == saleId);
          dbContext.SalesSalesmen.RemoveRange(lstsalesSalesman);
          res = dbContext.SaveChanges();
        }
      });
      return res;
    }

    #endregion

    #region UpdateGuestSalesmen

    /// <summary>
    /// Actualiza un GuestSalesMen 
    /// </summary>
    /// <param name="guId">Id del Guest</param>
    /// <param name="saleId">Id de la venta</param>
    /// <history>
    /// [jorcanche] created 30062016
    /// </history>
    public static async Task<int> UpdateGuestSalesmen(int? guId, int saleId)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          return dbContext.USP_OR_UpdateGuestSalesmen(guId, saleId);
        }
      });
    }

    #endregion

  
  }
}
