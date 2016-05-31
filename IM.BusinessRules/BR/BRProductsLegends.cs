using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using System.Data.Entity;

namespace IM.BusinessRules.BR
{
  public class BRProductsLegends
  {

    #region GetProductLegend
    /// <summary>
    /// Regresa un registro de 
    /// </summary>
    /// <param name="productLegend">Objeto con los filtros</param>
    /// <returns>Product legend</returns>
    /// <history>
    /// [emoguel] created 23/05/2016
    /// </history>
    public async static Task<ProductLegend> GetProductLegend(ProductLegend productLegend)
    {
      ProductLegend productL = null;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
        {
          var query = from px in dbContext.ProductsLegends
                      where px.pxla == productLegend.pxla && px.pxpr == productLegend.pxpr
                      select px;
          productL=query.FirstOrDefault();

        }
      });
      return productL;
      #endregion
    }
  }
}
