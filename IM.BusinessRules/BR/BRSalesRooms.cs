using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;
using System.Threading.Tasks;

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
    public async static Task<List<SalesRoomShort>> GetSalesRooms(int status)
    {
      return await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.USP_OR_GetSalesRooms(Convert.ToByte(status)).ToList();
        }
      });
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
    /// [edgrodriguez] 04/Mar/2016 Modified Se agregarón los valores default.
    /// [edgrodriguez] 21/May/2016 Modified El método se volvio asincrónico.
    /// </history>
    public async static Task<List<SalesRoomByUser>> GetSalesRoomsByUser(string user = "ALL", string regions = "ALL")
    {
      List<SalesRoomByUser> result = new List<SalesRoomByUser>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          result = dbContext.USP_OR_GetSalesRoomsByUser(user, regions).ToList();
        }
      });

      return result;
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.USP_OR_GetSalesRoom(salesRoom).FirstOrDefault();
      }
    }

    #endregion

    #region SetCloseSalesRoom
    /// <summary>
    /// Funcion para cerrar una entidad de la sala de ventas
    /// </summary>
    /// <param name="salesRoomType"> Tipo de entidad </param>
    /// <history>
    /// [vipacheco] 01/03/2016  Createad
    /// </history>
    public static void SetCloseSalesRoom(EnumEntities salesRoomType, string salesRoom, DateTime? dateClose)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        switch (salesRoomType)
        {
          case EnumEntities.Shows:
            dbContext.USP_OR_CloseShows(salesRoom, dateClose);
            break;
          case EnumEntities.MealTickets:
            dbContext.USP_OR_CloseMealTickets(salesRoom, dateClose);
            break;
          case EnumEntities.Sales:
            dbContext.USP_OR_CloseSales(salesRoom, dateClose);
            break;
          case EnumEntities.GiftsReceipts:
            dbContext.USP_OR_CloseGiftsReceipts(salesRoom, dateClose);
            break;
        }
      }
    }
    #endregion

    #region GetSalesRoom
    /// <summary>
    /// Obtiene registros del catalogo Sales Rooms
    /// </summary>
    /// <param name="nStatus">-1. Todos | 0. Inactivos | 1. Activos</param>
    /// <param name="nAppointment">-1. Todos | 0. Is Not Apooinment | 1. Is Appoinment</param>
    /// <param name="salesRoom">Objeto con filtros adicionales</param>
    /// <param name="blnTeamLog">Sirve para utilizar la consulta para TeamsLog</param>
    /// <returns>Lista de tipo sales Room</returns>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// [emoguel] modified 27/04/2016--->Se agregó el parametro blnTeamLog
    /// [emoguel] modified 30/05/2016 Se volvió async
    /// </history>
    public async static Task<List<SalesRoom>> GetSalesRooms(int nStatus = -1, int nAppointment = -1, SalesRoom salesRoom = null, bool blnTeamLog = false)
    {
      List<SalesRoom> lstSalesRooms = new List<SalesRoom>();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = from sr in dbContext.SalesRooms
                      select sr;

          if (blnTeamLog)
          {
            query = from ts in dbContext.TeamsSalesmen
                    from sr in dbContext.SalesRooms.Where(sr => sr.srID == ts.tssr).DefaultIfEmpty().Distinct()
                    select sr;

          }

          if (nStatus != -1)//Filtro por Estatus
          {
            bool blnStatus = Convert.ToBoolean(nStatus);
            query = query.Where(sr => sr.srA == blnStatus);
          }
          if (nAppointment != -1)//Filtro por ApointMent
          {
            bool blnAppointment = Convert.ToBoolean(nAppointment);
            query = query.Where(sr => sr.srAppointment == blnAppointment);
          }

          #region FIltros adicionales
          if (salesRoom != null)
          {
            if (!string.IsNullOrWhiteSpace(salesRoom.srID))//Filtro por ID
            {
              query = query.Where(sr => sr.srID == salesRoom.srID);
            }
            if (!string.IsNullOrWhiteSpace(salesRoom.srN))//Filtro por descripción
            {
              query = query.Where(sr => sr.srN.Contains(salesRoom.srN));
            }
            if (!string.IsNullOrWhiteSpace(salesRoom.srar))
            {
              query = query.Where(sr => sr.srar == salesRoom.srar);
            }
            if (!string.IsNullOrWhiteSpace(salesRoom.srcu))
            {
              query = query.Where(sr => sr.srcu == salesRoom.srcu);
            }
          }
          #endregion
          lstSalesRooms = query.OrderBy(sr => sr.srN).ToList();

        }
      });
      return lstSalesRooms;
    }
    #endregion

    #region SaveSalesRoom
    /// <summary>
    /// Guarda|Actualiza un registro en el catalogo
    /// </summary>
    /// <param name="salesRoom">Objeto a guardar</param>
    /// <param name="blnUpdate">True. Actualiza | False. Inserta</param>
    /// <returns>-1 Existe un registro con el mismo ID | 0. No se guardó | 1. Se guardó</returns>
    /// <history>
    /// [emoguel] created 22/04/2016
    /// </history>
    public static int SaveSalesRoom(SalesRoom salesRoom, bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        int nRes = 0;
        #region Update
        if (blnUpdate)
        {
          dbContext.Entry(salesRoom).State = System.Data.Entity.EntityState.Modified;
          return dbContext.SaveChanges();
        }
        #endregion
        #region Add
        else
        {
          using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
          {
            try
            {
              SalesRoom salesRoomVal = dbContext.SalesRooms.Where(sr => sr.srID == salesRoom.srID).FirstOrDefault();
              if (salesRoomVal != null)
              {
                return -1;
              }
              else
              {
                var GetDate = BRHelpers.GetServerDate();
                salesRoom.srGiftsRcptCloseD = GetDate;
                salesRoom.srCxCCloseD = GetDate;
                salesRoom.srShowsCloseD = GetDate;
                salesRoom.srMealTicketsCloseD = GetDate;
                salesRoom.srSalesCloseD = GetDate;

                dbContext.SalesRooms.Add(salesRoom);
                if (dbContext.SaveChanges() > 0)
                {
                  dbContext.USP_OR_AddAccessAdministrator("SR");
                  dbContext.SaveChanges();
                  nRes = 1;
                  transaction.Commit();
                }
                else
                {
                  transaction.Rollback();
                  return 0;
                }
              }
            }
            catch
            {
              transaction.Rollback();
              nRes = 0;
            }
          }
        }
        #endregion
        return nRes;

      }
    }
    #endregion

    #region GetSalesRoomByID
    /// <summary>
    /// Obtiene los registros de un Sales Room por su ID
    /// </summary>
    /// <param name="srID"> Identificador de Sales Room </param>
    /// <returns></returns>
    /// <history>
    /// [vipacheco] 14/Abril/2016 Created
    /// </history>
    public static SalesRoom GetSalesRoomByID(string srID)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        return dbContext.SalesRooms.Where(x => x.srID == srID).FirstOrDefault();








      }
    }
    #endregion

    #region GetCloseSalesRoom
    /// <summary>
    /// Obtiene la fecha de cierre de algun Sales Room
    /// </summary>
    /// <param name="salesRoomType"></param>
    /// <param name="salesRoom"></param>
    /// <returns> DateTime </returns>
    /// <history>
    /// [vipacheco] 19/Abril/2016 Created
    /// [michan]  07/Junio/2016 Modified Se agregó la fecha de cierre de CxC
    /// </history>
    public static DateTime? GetCloseSalesRoom(EnumEntities salesRoomType, string salesRoom)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
      {
        SalesRoom _salesRoom;
        switch (salesRoomType)
        {
          case EnumEntities.Shows:
            _salesRoom = dbContext.SalesRooms.Where(x => x.srID == salesRoom).SingleOrDefault();
            return _salesRoom.srShowsCloseD;
          case EnumEntities.MealTickets:
            _salesRoom = dbContext.SalesRooms.Where(x => x.srID == salesRoom).SingleOrDefault();
            return _salesRoom.srMealTicketsCloseD;
          case EnumEntities.Sales:
            _salesRoom = dbContext.SalesRooms.Where(x => x.srID == salesRoom).SingleOrDefault();
            return _salesRoom.srSalesCloseD;
          case EnumEntities.GiftsReceipts:
            _salesRoom = dbContext.SalesRooms.Where(x => x.srID == salesRoom).SingleOrDefault();
            return _salesRoom.srGiftsRcptCloseD;
          case EnumEntities.CxC:
            _salesRoom = dbContext.SalesRooms.Where(x => x.srID == salesRoom).SingleOrDefault();
            return _salesRoom.srCxCCloseD;
          default:
            return null;
        }
      }
    }
    #endregion

    #region GetSalesRoomsByIDs
    /// <summary>
    /// Obtiene salesRoom a traves de su ID
    /// </summary>
    /// <param name="lstSalesRoomID"></param>
    /// <returns>Devuelve lista de SalesRoomShort</returns>
    /// <history>
    /// [emoguel] created 16/06/2016
    /// </history>
    public static async Task<List<SalesRoomShort>> GetSalesRoomsByIDs(List<string>lstSalesRoomID)
    {
      List<SalesRoomShort> lstSalesRoom = await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          return dbContext.SalesRooms.AsEnumerable().Where(sr => lstSalesRoomID.Contains(sr.srID)).Select(sr=>new SalesRoomShort { srID=sr.srID,srN=sr.srN}).ToList();
        }
      });
      return lstSalesRoom;
    }
    #endregion

    #region getLocationByTeamGuestSevice
    /// <summary>
    /// Obtiene los SalesRoom relacionados a TeamsSalesMen
    /// </summary>
    /// <returns>Lista tipo Object</returns>
    /// <history>
    /// [emoguel] created 18/06/2016
    /// </history>
    public static async Task<List<object>> GetSalesRoombyTeamSalesMen()
    {
      List<object> lstObject = await Task.Run(() => {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          var query = (from sr in dbContext.SalesRooms
                       from ts in dbContext.TeamsSalesmen.Distinct()
                       where sr.srID == ts.tssr
                       select new { srID = sr.srID, srN = sr.srN }).Distinct();
          return query.ToList<object>();
        }
      });
      return lstObject;
    }
    #endregion
  }
}
