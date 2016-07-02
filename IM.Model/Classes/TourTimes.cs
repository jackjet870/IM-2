using System.Collections.Generic;

namespace IM.Model.Classes
{
  public class TourTimes
  {
    public List<TourTime> TourTimeByLeadSourceSalesRoom { get; set; }
    public List<TourTimeByDay> TourTimeByLeadSourceSalesRoomWeekDay { get; set; }
    public List<TourTimeBySalesRoomWeekDay> TourTimeBySalesRoomWeekDay { get; set; }
  }
}
