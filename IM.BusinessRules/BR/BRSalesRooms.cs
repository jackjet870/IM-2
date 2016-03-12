using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRSalesRooms
  {
    #region GetSalesRooms

    /// <summary>
    /// Obtiene el catalogo de SalesRooms
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns>List<Model.GetSalesRooms></returns>
    public static List<SalesRoomShort> GetSalesRooms(int status)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetSalesRooms(Convert.ToByte(status)).ToList();
      }
    }

    #endregion

    #region GetSalesRoomsByUser

    /// <summary>
    /// Obtiene la lista de SalesRooms
    /// </summary>
    /// <param name="user">Usuario o default('ALL')</param>
    /// <param name="regions">Region o default('ALL')</param>
    /// <returns>List<SalesRoomByUser></returns>
    /// <hystory>
    /// [erosado] 08/03/2016  created
    /// </hystory>
    public static List<SalesRoomByUser> GetSalesRoomsByUser(string user, string regions)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetSalesRoomsByUser(user, regions).ToList();
      }
    }

    #endregion

    #region GetSalesRoom

    /// <summary>
    /// Obtiene los datos de una sala de ventas en específico.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="regions"></param>
    /// <returns></returns>
    ///  <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// </history>
    public static SalesRoomCloseDates GetSalesRoom(string salesRoom)
    {
      using (var dbContext = new IMEntities())
      {
        return dbContext.USP_OR_GetSalesRoom(salesRoom).FirstOrDefault();
      }
    }

    #endregion
  }
}
