using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;
using System.Dynamic;
using System.Linq.Expressions;

namespace IM.BusinessRules.BR
{
  public class BRReportsBySalesRoom
  {
    public static List<RptBookingsBySalesRoomProgramTime> getRptBookingsBySalesRoomProgramTime(DateTime dtmStart,string SalesRooms)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramTime(dtmStart, SalesRooms).ToList();
      }
    }   
  }
}
