using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRProducts
  {
    #region GetProducsts
    /// <summary>
    /// Obtiene registros del catalogo Product
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1.Activos</param>
    /// <param name="product">Objeto con filtros adicionales</param>
    /// <returns>Lista de tipo product</returns>
    /// <history>
    /// [emoguel] created 20/05/2016
    /// </history>
    public async static Task<List<Product>> GetProducts(int nStatus = -1, Product product = null)
    {
      List<Product> lstProducts = new List<Product>();
      await Task.Run(() => {
        
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        var query = from pr in dbContext.Products
                    select pr;

        if (nStatus != -1)//Filtro por status
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(pr => pr.prA == blnStatus);
        }

        if (product != null)
        {
          if (!string.IsNullOrWhiteSpace(product.prID))//Filtro por ID
          {
            query = query.Where(pr => pr.prID == product.prID);
          }

          if (!string.IsNullOrWhiteSpace(product.prN))//Filtro por descripción
          {
            query = query.Where(pr => pr.prN.Contains(product.prN));
          }
        }
        
       lstProducts=  query.OrderBy(p => p.prN).ToList();
      }        
      });
      return lstProducts;
    }
    #endregion

    #region SaveProduct
    /// <summary>
    /// Agrega|Actualiza un product
    /// Asigna|desasigna gifts
    /// Actualiza|Agrega un productLegend
    /// </summary>
    /// <param name="product">OBjeto a guardar en product</param>
    /// <param name="blnUpdate">True. Actualiza | False. Agrega</param>
    /// <param name="productLegend">Product legend a guardar</param>
    /// <param name="lstAdd">Gifts a asignar</param>
    /// <param name="lstDel">gift a desasignar</param>
    /// <returns>-1. Existe un product con el mismo ID | 0. No se guardó | >0. Se guardó correctamente</returns>
    /// <history>
    /// [emoguel] created 24/05/2016
    /// </history>
    public async static Task<int> SaveProduct(Product product, bool blnUpdate, ProductLegend productLegend, List<Gift> lstAdd, List<Gift> lstDel)
    {
      int nRes = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          using (var transacction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              #region Update
              if (blnUpdate)
              {
                dbContext.Entry(product).State = EntityState.Modified;
              }
              #endregion
              #region Add
              else
              {
                if (dbContext.Products.Where(pr => pr.prID == product.prID).FirstOrDefault() != null)
                {
                  return -1;
                }
                else
                {
                  dbContext.Products.Add(product);
                }
              }
              #endregion
              #region ProductLegend
              if (!string.IsNullOrWhiteSpace(productLegend.pxText))
              {
                ProductLegend prodLegenVal = dbContext.ProductsLegends.Where(px => px.pxla == productLegend.pxla && px.pxpr == productLegend.pxpr).FirstOrDefault();
                if (prodLegenVal != null)
                {
                  prodLegenVal.pxText = productLegend.pxText;
                }
                else
                {
                  productLegend.pxpr = product.prID;
                  dbContext.ProductsLegends.Add(productLegend);
                }
              }
              #endregion
              #region Gifts
              dbContext.Gifts.AsEnumerable().Where(gi => lstAdd.Any(gii => gii.giID == gi.giID)).ToList().ForEach(gi =>//Add
              {
                gi.gipr = product.prID;
              });

              dbContext.Gifts.AsEnumerable().Where(gi => lstDel.Any(gii => gii.giID == gi.giID)).ToList().ForEach(gi =>//Del
              {
                gi.gipr = null;
              });
              #endregion
              nRes = dbContext.SaveChanges();
              transacction.Commit();
              return nRes;
            }
            catch
            {
              transacction.Rollback();
              throw;
            }
          }
        }
      });
      return nRes;
    } 
    #endregion
  }
}
