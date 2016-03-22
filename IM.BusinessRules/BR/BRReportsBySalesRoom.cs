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
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de venta y Programa
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramTime> </returns>
    /// <history>
    /// [edgrodriguez] 12/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramTime> getRptBookingsBySalesRoomProgramTime(DateTime dtmStart,string SalesRooms)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramTime(dtmStart, SalesRooms).ToList();
      }
    }

    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de Venta,Programa y Locaciones.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramLeadSourceTime> getRptBookingsBySalesRoomProgramLeadSourceTime(DateTime dtmStart, string SalesRooms)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime(dtmStart, SalesRooms).ToList();
      }
    }
  }
}
