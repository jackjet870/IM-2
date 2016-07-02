using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace IM.Model.Enums
{
  /// <summary>
  ///   Enumerado para el manejo de los diferentes esquemas de horarios de tour
  /// </summary>
  /// <history>
  ///   [vku] 20/Jun/2016 Created
  /// </history>
  public enum EnumTourTimesSchema
  {
    [Description("TourTime")]
    ttsByLeadSourceSalesRoom = 1,
    [Description("TourTimesByDay")]
    ttsByLeadSourceSalesRoomWeekDay,
    [Description("TourTimesBySalesRoomWeekDay")]
    ttsBySalesRoomWeekDay
  }
}
