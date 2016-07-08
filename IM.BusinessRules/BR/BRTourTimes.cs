using System;
using System.Linq;
using System.Threading.Tasks;
using IM.Model;
using IM.Model.Helpers;
using IM.Model.Enums;
using IM.Model.Classes;
namespace IM.BusinessRules.BR
{
  public class BRTourTimes
  {
    #region GetTourTimes
    /// <summary>
    ///   Obtiene los horarios de tour
    /// </summary>
    /// <param name="_enumTourTimes">Enumerado de los esquemas de horarios</param>
    /// <param name="ttls">Clave de Lead Source</param>
    /// <param name="ttsr">Clave de Sales Room</param>
    /// <param name="ttday">Dia de la semana</param>
    /// <history>
    ///   [vku] 21/Jun/2016 Created
    /// </history>
    public async static Task<TourTimes> GetTourTimes(EnumTourTimesSchema _enumTourTimes, string ttls, string ttsr, int ttday)
    {
      TourTimes lstTts = new TourTimes();
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          switch (_enumTourTimes)
          {
            case EnumTourTimesSchema.ttsByLeadSourceSalesRoom:
              lstTts.TourTimeByLeadSourceSalesRoom = dbContext.TourTimes.Where(x => (x.ttls == ttls) && (x.ttsr == ttsr)).ToList();
              break;
            case EnumTourTimesSchema.ttsByLeadSourceSalesRoomWeekDay:
              lstTts.TourTimeByLeadSourceSalesRoomWeekDay = dbContext.TourTimesByDay.Where(x => (x.ttls == ttls) && (x.ttsr == ttsr) && (x.ttDay == ttday)).ToList();
              break;
            case EnumTourTimesSchema.ttsBySalesRoomWeekDay:
              lstTts.TourTimeBySalesRoomWeekDay = dbContext.TourTimesBySalesRoomWeekDay.Where(x => (x.ttsr == ttsr) && (x.ttDay == ttday)).ToList();
              break;
          }      
        }
      });
      return lstTts;
    }
    #endregion

    #region CopyTourTimesByLeadSourceSalesRoomToLeadSource
    /// <summary>
    ///   Copia los horarios de tour por Lead Source y sala de ventas de un Lead Source a otro
    /// </summary>
    /// <param name="leadSourceFrom">Clave del Lead Source de donde se van a copiar los horarios</param>
    /// <param name="leadSourceTo">Clave del Lead Source al que se le van a agregar los horarios</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesByLeadSourceSalesRoomToLeadSource(string leadSourceFrom, string leadSourceTo)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSource(leadSourceFrom, leadSourceTo);
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSource
    /// <summary>
    ///   Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de un Lead Source a otro
    /// </summary>
    /// <param name="leadSourceFrom">Clave del Lead Source de donde se van a copiar los horarios</param>
    /// <param name="leadSourceTo">Clave del Lead Source al que se le van a agregar los horarios</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSource(string leadSourceFrom, string leadSourceTo)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSource(leadSourceFrom, leadSourceTo);
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesByLeadSourceSalesRoomToSalesRoom
    /// <summary>
    ///   Copia los horarios de tour por Lead Source y sala de ventas de una sala de ventas a otra
    /// </summary>
    /// <param name="salesRoomFrom">Clave de la sala de donde se van a copiar los horarios</param>
    /// <param name="salesRoomTo">Clave de la sala a la que se le van a agregar los horarios</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesByLeadSourceSalesRoomToSalesRoom(string salesRoomFrom, string salesRoomTo)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesByLeadSourceSalesRoomToSalesRoom(salesRoomFrom, salesRoomTo);
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoom
    /// <summary>
    ///   Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de una sala de ventas a otra
    /// </summary>
    /// <param name="salesRoomFrom"></param>
    /// <param name="salesRoomTo"></param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoom(string salesRoomFrom, string salesRoomTo)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoom(salesRoomFrom, salesRoomTo);
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesBySalesRoomWeekDayToSalesRoom
    /// <summary>
    ///   Copia los horarios de tour por sala de ventas y dia de la semana de una sala de ventas a otra
    /// </summary>
    /// <param name="salesRoomFrom">Clave de la sala de donde se van a copiar los horarios</param>
    /// <param name="salesRoomTo">Clave de la sala a la que se le van a agregar los horarios</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesBySalesRoomWeekDayToSalesRoom(string salesRoomFrom, string salesRoomTo)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesBySalesRoomWeekDayToSalesRoom(salesRoomFrom, salesRoomTo);
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoomsWeekDaysOfLeadSource
    /// <summary>
    ///    Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de un Lead Source, sala de ventas y dia a todas las salas de     ///    ventas y dias de la semana del mismo Lead Source
    /// </summary>
    /// <param name="leadSource">Clave del Lead Source</param>
    /// <param name="salesRoom">Clave de la sala de ventas</param>
    /// <param name="weekDay">Dia de la semana</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoomsWeekDaysOfLeadSource(string leadSource, string salesRoom, string weekDay)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToSalesRoomsWeekDaysOfLeadSource(leadSource, salesRoom, Convert.ToByte(weekDay));
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesByLeadSourceSalesRoomToLeadSourcesOfProgram
    /// <summary>
    ///   Copia los horarios de tour por Lead Source y sala de ventas de un Lead Source a todos los Lead Sources del mismo programa
    /// </summary>
    /// <param name="leadSource">Clave del Lead Source</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesByLeadSourceSalesRoomToLeadSourcesOfProgram(string leadSource)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using (var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesByLeadSourceSalesRoomToLeadSourcesOfProgram(leadSource);
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSourcesOfProgram
    /// <summary>
    ///   Copia los horarios de tour por Lead Source, sala de ventas y dia de la semana de un Lead Source a todos los Lead Sources del mismo programa
    /// </summary>
    /// <param name="leadSource">Clave de Lead Source</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSourcesOfProgram(string leadSource)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using(var dbContext = new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesByLeadSourceSalesRoomWeekDayToLeadSourcesOfProgram(leadSource);
        }
      });
      return response;
    }
    #endregion

    #region CopyTourTimesBySalesRoomWeekDayToWeekDaysOfSalesRoom
    /// <summary>
    ///   Copia los horarios de tour por sala de ventas y dia de la semana de una sala de ventas y dia a todos los dias de la semana de la misma sala de ventas
    /// </summary>
    /// <param name="salesRoom">Clave de la sala de ventas</param>
    /// <param name="weekDay">Dia de la semana</param>
    /// <history>
    ///   [vku] 04/Jul/2016 Created
    /// </history>
    public async static Task<int> CopyTourTimesBySalesRoomWeekDayToWeekDaysOfSalesRoom(string salesRoom, string weekDay)
    {
      int response = 0;
      await Task.Run(() =>
      {
        using(var dbContext =new IMEntities(ConnectionHelper.ConnectionString()))
        {
          response = dbContext.USP_OR_CopyTourTimesBySalesRoomWeekDayToWeekDaysOfSalesRoom(salesRoom, Convert.ToByte(weekDay));
        }
      });
      return response;
    }
    #endregion
  }
}
