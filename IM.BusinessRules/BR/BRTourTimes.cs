using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using IM.Model.Classes;
using System.Collections.Generic;
namespace IM.BusinessRules.BR
{
  public class BRTourTimes
  {
    #region GetTourTimes
    /// <summary>
    ///   Obtiene los horarios de tour
    /// </summary>
    /// <param name="_enumTourTimes">Enumerado de los esquemas de horarios</param>
    /// <param name="ttls">Clave de Lead Source</param>
    /// <param name="ttsr">Clave de Sales Room</param>
    /// <param name="ttday">Dia de la semana</param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    public async static Task<TourTimes> GetTourTimes(EnumTourTimesSchema _enumTourTimes, string ttls, string ttsr, int ttday)
    {
      TourTimes lstTts = new TourTimes();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          switch (_enumTourTimes)
          {
            case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
              lstTts.TourTimeByLeadSourceSalesRoom = dbContext.TourTimes.Where(x => (x.ttls == ttls) && (x.ttsr == ttsr)).ToList();
              break;
            case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
              lstTts.TourTimeByLeadSourceSalesRoomWeekDay = dbContext.TourTimesByDay.Where(x => (x.ttls == ttls) && (x.ttsr == ttsr) && (x.ttDay == ttday)).ToList();
              break;
            case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
              lstTts.TourTimeBySalesRoomWeekDay = dbContext.TourTimesBySalesRoomWeekDay.Where(x => (x.ttsr == ttsr) && (x.ttDay == ttday)).ToList();
              break;
          }      
        }
      });
      return lstTts;
    }
    #endregion
  }
}
