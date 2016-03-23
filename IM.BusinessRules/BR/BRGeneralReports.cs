using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  public class BRGeneralReports
  {
    #region GetRptPersonnel
    /// <summary>
    /// Obtiene el reporte de personal.
    /// </summary>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptPersonnel> GetRptPersonnel()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptPersonnel().ToList();
      }
    } 
    #endregion

    #region GetRptGifts
    /// <summary>
    /// Obtiene el reporte de regalos.
    /// </summary>
    /// <returns> List<RptBookingsBySalesRoomProgramLeadSourceTime> </returns>
    /// <history>
    /// [edgrodriguez] 16/Mar/2016 Created
    /// </history>
    public static List<RptGifts> GetRptGifts()
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        return dbContext.USP_OR_RptGifts().ToList();
      }
    } 
    #endregion
  }
}
