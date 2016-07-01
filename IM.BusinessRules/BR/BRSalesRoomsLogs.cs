using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Helpers;

namespace IM.BusinessRules.BR
{
  /// <summary>
  /// Reglas de negocio del historico de salas de ventas
  /// </summary>
  /// <history>
  /// [wtorres]  23/Mar/2016 Created
  /// </history>
  public class BRSalesRoomsLogs
  {
    #region SaveSalesRoomLog
    /// <summary>
    /// Función que se encarga de guarda el historico de cambios realizados
    /// </summary>
    /// <param name="SalesRoomID"> Clave de la sala de ventas </param>
    /// <param name="HoursDif"> Horas de diferencia </param>
    /// <param name="ChangedBy"> Clave del usuario que esta haciendo el cambio </param>
    /// <history>
    /// [vipacheco] 02/03/2016 Created
    /// </history>
    public static void SaveSalesRoomLog(string SalesRoomID, Int16 HoursDif, string ChangedBy)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        dbContext.USP_OR_SaveSalesRoomLog(SalesRoomID, HoursDif, ChangedBy);
      }
    }
    #endregion

    #region GetSalesRoomLog
    /// <summary>
    /// Función para obtener el log de Sales Room
    /// </summary>
    /// <param name="salesRoom"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 22/02/2016 Created
    /// </history>
    public static List<SalesRoomLogData> GetSalesRoomLog(string salesRoom)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetSalesRoomLog(salesRoom).ToList();
      }
    }
    #endregion
  }
}
