using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Model;

namespace IM.BusinessRules.BR
{
  public class BRSalesRooms
  {
    /// <summary>
    /// Obtiene el catalogo de SalesRooms
    /// </summary>
    /// <param name="status"> 0- Sin filtro, 1-Activos, 2. Inactivos </param>
    /// <returns>List<Model.GetSalesRooms></returns>
    public static List<Model.GetSalesRooms> GetSalesRooms(int status)
    {
      try
      {
        using (var model = new IMEntities())
        {
          return model.USP_OR_GetSalesRooms(Convert.ToByte(status)).ToList();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }
    /// <summary>
    /// Obtiene la lista de SalesRooms
    /// </summary>
    /// <param name="user">Usuario o default('ALL')</param>
    /// <param name="regions">Region o default('ALL')</param>
    /// <returns></returns>
    public static List<Model.GetSalesRoomsByUser> GetSalesRoomsByUser(string user, string regions)
    {
      try
      {
        using (var model = new IMEntities())
        {
          return model.USP_OR_GetSalesRoomsByUser(user,regions).ToList();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }

    /// <summary>
    /// Obtiene los datos de una sala de ventas en específico.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="regions"></param>
    /// <returns></returns>
    ///  <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// </history>
    public static GetSalesRoom GetSalesRoom(string salesRoom)
    {
      try
      {
        using (var model = new IMEntities())
        {
          return model.USP_OR_GetSalesRoom(salesRoom).FirstOrDefault();
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }

  }
}
