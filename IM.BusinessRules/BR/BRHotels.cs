using IM.Model;
using IM.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.BusinessRules.BR
{
  public class BRHotels
  {
    #region GetHotels
    /// <summary>
    /// Obtiene la lista de hoteles
    /// </summary>
    /// <param name="status">Indica el estado del hotel</param>
    /// <returns>Lista Hotels</returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// </history>
    public static List<Hotel> GetHotels(int status)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        bool statusHotel = Convert.ToBoolean(status);
        return dbContext.Hotels.Where(h => h.hoA == statusHotel).ToList();
      }
    }
    #endregion
  }
}
