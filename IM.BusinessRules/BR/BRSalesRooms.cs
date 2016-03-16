using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;

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
    /// <history>
    /// [edgrodriguez] 29/Feb/2016 Created
    /// [edgrodriguez] 04/Mar/2016 Modified //Se agregarón los valores default.
    /// </history>
    public static List<SalesRoomByUser> GetSalesRoomsByUser(string user = "ALL", string regions = "ALL")
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

    /// <summary>
    /// Funcion para cerrar una entidad de la sala de ventas
    /// </summary>
    /// <param name="salesRoomType"> Tipo de entidad </param>
    /// <history>
    /// [vipacheco] 01/03/2016  Createad
    /// </history>
    public static void SetCloseSalesRoom(EnumSalesRoomType salesRoomType, string salesRoom, DateTime? dateClose)
    {
      using (var model = new IMEntities())
      {
        switch (salesRoomType)
        {
          case EnumSalesRoomType.Shows:
            model.USP_OR_CloseShows(salesRoom, dateClose);
            break;
          case EnumSalesRoomType.MealTickets:
            model.USP_OR_CloseMealTickets(salesRoom, dateClose);
            break;
          case EnumSalesRoomType.Sales:
            model.USP_OR_CloseSales(salesRoom, dateClose);
            break;
          case EnumSalesRoomType.GiftsReceipts:
            model.USP_OR_CloseGiftsReceipts(salesRoom, dateClose);
            break;
        }
      }
    }

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
      using (var model = new IMEntities())
      {
        model.USP_OR_SaveSalesRoomLog(SalesRoomID, HoursDif, ChangedBy);
      }
    }

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
      using (var model = new IMEntities())
      {
        return model.USP_OR_GetSalesRoomLog(salesRoom).ToList();
      }
    }

    /// <summary>
    /// Función para obtener el manifestHost
    /// </summary>
    /// <param name="currentDate"></param>
    /// <param name="salesRoomID"></param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 22/02/2016 Created
    /// </history>
    public static List<GuestPremanifestHost> GetPremanifestHost(DateTime? currentDate, string salesRoomID)
    {
      using (var model = new IMEntities())
      {
        return model.USP_OR_GetPremanifestHost(currentDate, salesRoomID).ToList();
      }
    }
  }
}
