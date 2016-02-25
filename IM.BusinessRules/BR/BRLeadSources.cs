using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRLeadSources
  {
    /// <summary>
    /// Obtiene el catalogo de LeadSources
    /// </summary>
    /// <param name="user">Usuario </param>
    /// <param name="programs"> Programa o default('ALL') </param>
    /// <param name="regions">Region o default('ALL') </param>
    /// <returns>List<Model.GetLeadSourcesByUser></returns>
    public static List<Model.GetLeadSourcesByUser> GetLeadSourcesByUser(string user,string programs, string regions )
    {
      try
      {
        using (var model = new IM.Model.IMEntities())
        {
          return model.USP_OR_GetLeadSourcesByUser(user,programs,regions).ToList();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }
  }
}
