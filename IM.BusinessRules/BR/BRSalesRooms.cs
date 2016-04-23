using System;
using System.Collections.Generic;
using System.Linq;
using IM.Model;
using IM.Model.Enums;
using IM.Model.Helpers;

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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
    public static void SetCloseSalesRoom(EnumSalesRoomType salesRoomType, string salesRoom, DateTime? dateClose)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        switch (salesRoomType)
        {
          case EnumSalesRoomType.Shows:
            dbContext.USP_OR_CloseShows(salesRoom, dateClose);
            break;
          case EnumSalesRoomType.MealTickets:
            dbContext.USP_OR_CloseMealTickets(salesRoom, dateClose);
            break;
          case EnumSalesRoomType.Sales:
            dbContext.USP_OR_CloseSales(salesRoom, dateClose);
            break;
          case EnumSalesRoomType.GiftsReceipts:
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
    /// <returns>Lista de tipo sales Room</returns>
    /// <history>
    /// [emoguel] created 21/04/2016
    /// </history>
    public static List<SalesRoom> GetSalesRooms(int nStatus=-1,int nAppointment=-1, SalesRoom salesRoom=null)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
      {
        var query = from sr in dbContext.SalesRooms
                    select sr;        
        
        if(nStatus!=-1)//Filtro por Estatus
        {
          bool blnStatus = Convert.ToBoolean(nStatus);
          query = query.Where(sr => sr.srA == blnStatus);
        }
        if(nAppointment!=-1)//Filtro por ApointMent
        {
          bool blnAppointment = Convert.ToBoolean(nAppointment);
          query = query.Where(sr => sr.srAppointment == blnAppointment);
        }

        if(salesRoom!=null)
        {
          if(!string.IsNullOrWhiteSpace(salesRoom.srID))//Filtro por ID
          {
            query = query.Where(sr => sr.srID == salesRoom.srID);
          }
          if(!string.IsNullOrWhiteSpace(salesRoom.srN))//Filtro por descripción
          {
            query = query.Where(sr => sr.srN.Contains(salesRoom.srN));
          }
          if(!string.IsNullOrWhiteSpace(salesRoom.srar))
          {
            query = query.Where(sr => sr.srar == salesRoom.srar);
          }
          if(!string.IsNullOrWhiteSpace(salesRoom.srcu))
          {
            query = query.Where(sr => sr.srcu == salesRoom.srcu);
          }
        }
        return query.OrderBy(sr => sr.srN).ToList();

      }
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
    public static int SaveSalesRoom(SalesRoom salesRoom,bool blnUpdate)
    {
      using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString))
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
              SalesRoom salesRoomVal = dbContext.SalesRooms.Where(sr => sr.srCEBEID == salesRoom.srID).FirstOrDefault();
              if(salesRoomVal!=null)
              {
                return -1;
              }
              else
              {
                var GetDate= BRHelpers.GetServerDate();
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
              nRes= 0;
            }
          }
        }
        #endregion
        return nRes;

      }
    }
    #endregion
  }
}
