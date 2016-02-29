using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRGifts
  {
    /// <summary>
    /// Método para obtener una lista de Regalos por lugar y/o estatus.
    /// </summary>
    /// <param name="location">Lugar o Almacén</param>
    /// <param name="Status">0. Sin filtro.
    /// 1. Activos.
    /// 2. Inactivos.</param>
    /// <returns> List<GetGifts> </returns>
    /// <history>
    /// [edgrodriguez] 24/Feb/2016 Created
    /// </history>
    public static List<GetGifts> getGifts(string location = "ALL",int Status = 0)
    {
      List<GetGifts> lstgetGifs = new List<GetGifts>();
      using (var dbContext = new IMEntities())
      {       
        lstgetGifs = dbContext.USP_OR_GetGifts(location, Convert.ToByte(Status)).ToList();
      }
      return lstgetGifs;
    }
  }
}
