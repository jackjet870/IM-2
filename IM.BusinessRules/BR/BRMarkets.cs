using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRMarkets
  {
    /// <summary>
    /// Obtiene el catalogo de Markets
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns>List<Model.GetMarkets></returns>
    public static List<Model.GetMarkets> GetMarkets(int status)
    {
      try
      {
        using (var model = new IM.Model.IMEntities())
        {
          return model.USP_OR_GetMarkets(Convert.ToByte(status)).ToList();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }
  }
}
