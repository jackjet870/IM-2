using IM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRInvitationBase
  {
    /// <summary>
    /// Método para obtener el lenguaje en le formulario Invitatio Base
    /// </summary>
    public static IEnumerable<GetLanguages> GetLanguage()
    {
      try{
        using (var dbContext = new IMEntities())
        {

          var languajes = dbContext.USP_OR_GetLanguages(0).ToList();
          return languajes;
        }
      }
      catch (Exception ex)
      {
        return null;
      }
    }

  }
}
