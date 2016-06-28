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
    #region GetSalesSalesmen

    /// <summary>
    /// devuelve un boleando indicando si exite un registro de SaleAmountWith o SaleAmountOwn por venta y vendedor
    /// </summary>   
    /// <history>
    /// [jorcanche] created 09/06/2016
    /// </history>
    public async static Task<SalesSalesman> GetSalesSalesmen(SalesSalesman salesSalesmen = null)
    {
      List<SalesSalesman> result = null;
      await Task.Run(() =>
      {       
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
            if (!string.IsNullOrWhiteSpace(salesSalesmen.smpe))//Filtro por descripcion
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
      });
      return result.FirstOrDefault();
    }
    #endregion

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
            if (!string.IsNullOrWhiteSpace(salesSalesmen.smpe))//Filtro por descripcion
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
    public static List<SalesmenChanges> GetSalesmenChanges(int sale)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_GetSalesmenChanges(sale).ToList();
      }
    }
    #endregion

    /// <summary>
    /// Elimina uno o mas registros que contengan el Id Sale en la table SalesSalesmen
    /// </summary>    
    ///<history>
    ///[jorcanche] created 22/06/2016
    ///</history>
    public static int DeleteSalesmenbySale(int sale)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var sales = dbContext.SalesSalesmen.Where(x => x.smsa == sale).ToList();
        dbContext.SalesSalesmen.RemoveRange(sales);
        return dbContext.SaveChanges();
      }
    }
  }
}
