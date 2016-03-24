using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRReportsBySalesRoom
  {
    #region GetRptBookingsBySalesRoomProgramTime
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de venta y Programa
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramTime> </returns>
    /// <history>
    /// [edgrodriguez] 12/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramTime> GetRptBookingsBySalesRoomProgramTime(DateTime dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramTime(dtmStart, salesRooms).ToList();
      }
    } 
    #endregion

    #region GetRptBookingsBySalesRoomProgramLeadSourceTime
    /// <summary>
    /// Obtiene el reporte de Bookings por Sala de Venta,Programa y Locaciones.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="SalesRooms">Salas de Venta</param>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptBookingsBySalesRoomProgramLeadSourceTime> GetRptBookingsBySalesRoomProgramLeadSourceTime(DateTime dtmStart, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptBookingsBySalesRoomProgramLeadSourceTime(dtmStart, salesRooms).ToList();
      }
    } 
    #endregion

    #region GetRptCxC
    /// <summary>
    /// Obtiene el reporte de CxC por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart">Fecha Inicial</param>
    /// <param name="dtmEnd">Fecha Final</param>
    /// <param name="salesRooms">Sala de Venta</param>
    /// <returns> List<RptCxC> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<RptCxC> GetRptCxC(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptCxC(dtmStart, dtmEnd, salesRooms).ToList();
      }
    } 
    #endregion

    #region GetRptCxCDeposits
    /// <summary>
    /// Obtiene el reporte de CxCDeposits por Sala de ventas.
    /// </summary>
    /// <param name="dtmStart"></param>
    /// <param name="dtmEnd"></param>
    /// <param name="salesRooms"></param>
    /// <returns> List<RptCxCDeposits> </returns>
    /// <history>
    /// [edgrodriguez] 22/Mar/2016 Created
    /// </history>
    public static List<RptCxCDeposits> GetRptCxCDeposits(DateTime? dtmStart, DateTime? dtmEnd, string salesRooms)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptCxCDeposits(dtmStart, dtmEnd, salesRooms).ToList();
      }
    } 
    #endregion
  }
}
