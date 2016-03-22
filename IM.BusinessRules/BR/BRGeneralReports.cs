using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{  
  public class BRGeneralReports
  {
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de Venta,Programa y Locaciones.
    /// </summary>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptPersonnel> getRptPersonnel()
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptPersonnel().ToList();
      }
    }

    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de Venta,Programa y Locaciones.
    /// </summary>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptGifts> getRptGifts()
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_RptGifts().ToList();
      }
    }
  }
}
