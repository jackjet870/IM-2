using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRTourTimesAvailables
  {
    #region GetTourTimesAvailables
    /// <summary>
    /// Obtiene los horarios disponibles para booking
    /// </summary>
    /// <param name="leadSource"></param>
    /// <param name="salesRoom">Sala de ventas</param>
    /// <param name="selectedDate">día selecccionado</param>
    /// <param name="originalDate">fecha original</param>
    /// <param name="originalTime"> hora original</param>
    /// <param name="currentDate">Fecha de hoy</param>
    /// <returns>Lista de TourTimeAvailable</returns>
    /// <history>
    /// [lchairez] 10/03/2016 Created.
    /// [erosado] 22/08/2016  Modified. se agrego async y await se optimizo.
    /// </history>
    public static async Task<List<TourTimeAvailable>> GetTourTimesAvailables(string leadSource, string salesRoom, DateTime selectedDate
                                                                  , DateTime? originalDate = null, DateTime? originalTime = null
                                                                  ,DateTime? currentDate = null)
    {
      return await Task.Run(() =>
      {
        using (var dbContex = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          dbContex.Database.CommandTimeout = 60;
          return dbContex.USP_OR_GetTourTimesAvailables(leadSource, salesRoom, selectedDate, originalDate, originalTime, currentDate ?? DateTime.Today).ToList();
        }
      });
    }
    #endregion
  }
}
