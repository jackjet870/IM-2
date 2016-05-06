using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace IM.Base.Helpers
{
  public class DateHelper
  {

    #region DateRange for Report
    /// <summary>
    /// Obtiene un rango de fecha formateado para utilizar en el Reporte
    /// </summary>
    /// <param name="dateFrom">Fecha inicio</param>
    /// <param name="dateTo">Fecha Fin</param>
    /// <returns>Rango de fecha formateada (string) </returns>
    /// <history>
    /// [erosado] 15/Mar/2016 Created
    /// </history>
    public static string DateRange(DateTime dateFrom, DateTime dateTo)
    {
      string dateRange = string.Empty;
      //Comparar si es la misma fecha
      if (dateFrom.Date == dateTo.Date)
      {
        dateRange = string.Format("{0:MMMM d, yyyy}", dateFrom);
      }
      // Si son diferentes años
      else if (dateFrom.Year != dateTo.Year)
      {
        dateRange = string.Format("{0:MMMM d, yyyy} - {1:MMMM d, yyyy}", dateFrom, dateTo);
      }
      //Si son diferentes Meses
      else if (dateFrom.Month != dateTo.Month)
      {
        dateRange = string.Format("{0:MMMM d} - {1:MMMM d, yyyy}", dateFrom, dateTo);
      }
      // Mismo mes y mismo año
      else
      {
        dateRange = string.Format("{0:MMMM d} - {1:d, yyyy}", dateFrom, dateTo);
      }

      return dateRange;
    }
    #endregion

    #region DateRange for Suggest FileName Report
    /// <summary>
    /// Obtiene un rango de fecha formateado para utilizar al sugerir Nombre del Reporte
    /// </summary>
    /// <param name="dateFrom">Fecha inicio</param>
    /// <param name="dateTo">Fecha fin</param>
    /// <returns>Rango de fecha formateada (string) </returns>
    /// <history>
    /// [erosado] 24/Mar/2016 Created
    /// [wtorres] 16/Abr/2016 Modified. Correccion de error en el formato de mes en rangos de 1 dia
    /// </history>
    public static string DateRangeFileName(DateTime dateFrom, DateTime dateTo)
    {
      string dateRange = string.Empty;
      //Comparar si es la misma fecha
      if (dateFrom.Date == dateTo.Date)
      {
        dateRange = string.Format("{0:yyyy-MM-dd}", dateFrom);
      }
      // Si son diferentes años
      else if (dateFrom.Year != dateTo.Year)
      {
        //Años completos
        if (dateFrom.Day == 1 && dateFrom.Month == 1 && dateTo.Day==31 && dateTo.Month==12)
        {
          dateRange= string.Format("{0:yyyy} to {1:yyyy}", dateFrom, dateTo);
        }
        //Años incompletos meses completos
        else if (dateFrom.Day == 1 && dateTo.Day ==DateTime.DaysInMonth(dateTo.Year,dateTo.Month))
        {
          dateRange = string.Format("{0:yyyy-MM} to {1:yyyy-MM}", dateFrom, dateTo);
        }
        //Años incompletos, meses incompletos
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd} to {1:yyyy-MM-dd}", dateFrom, dateTo);
        }
       
      }
      //Si son diferentes Meses
      else if (dateFrom.Month != dateTo.Month)
      {
        //Año completo
        if (dateFrom.Day ==1 && dateFrom.Month==1 && dateTo.Day==31 && dateTo.Month==12)
        {
          dateRange = string.Format("{0:yyyy}", dateFrom);
        }
        //Meses Completo
        else if (dateFrom.Day==1 && dateTo.Day== DateTime.DaysInMonth(dateTo.Year,dateTo.Month))
        {
          dateRange = string.Format("{0:yyyy-MM} to {1:MM}", dateFrom, dateTo);
        }
        //Meses Incompletos
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd to} - {1:MM-dd}", dateFrom, dateTo);
        }
      }
      // Mismo mes y mismo año
      else
      {
        //Mes completo
        if (dateFrom.Day==1 && dateTo.Day== DateTime.DaysInMonth(dateTo.Year,dateTo.Month))
        {
          dateRange = string.Format("{0:yyyy-MM}", dateFrom);
        }
        //Mes Incompleto
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd to}{1:dd}", dateFrom,dateTo);
        }
      }
      return dateRange;
    }

    #endregion

    #region GetStartWeek
    /// <summary>
    ///   Devuelve el primer dia de la semana dada un fecha
    /// </summary>
    /// <param name="date"></param>
    /// <history>
    ///   [vku] 05/Mar/2016 Created
    /// </history>
    public static System.DateTime GetStartWeek(DateTime date)
    {
      System.DayOfWeek dmon = System.DayOfWeek.Monday;
      int span = date.DayOfWeek - dmon;
      date = date.AddDays(-span);
      return date;
    }
    #endregion

    #region IsRangeHours
    ///<summary>Metodo que valida que una hora este en un rango</summary>
    ///<param name="stratTime">Hora donde se inicia la validacion</param>
    ///<param name="endTime">Hora donde finaliza la validacion</param>
    ///<param name="currentTime">Hora que se va validar</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static bool IsRangeHours(TimeSpan currentTime, TimeSpan stratTime, TimeSpan endTime)
    {
      bool _response = false;
      Console.WriteLine(currentTime.CompareTo(stratTime) + " " + currentTime.CompareTo(endTime));
      if ((currentTime.CompareTo(stratTime) > -1) && (currentTime.CompareTo(endTime) < 1))
      {
        _response = true;
      }
      return _response;
    }
    #endregion

    #region IsRangeTime
    ///<summary>Metodo que valida que dos horas sean iguales</summary>
    ///<param name="currentTime">dia que contiene la hora actual</param>
    ///<param name="compareTime">Dia y hora con que se va comparar</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static bool IsRangeTime(DateTime currentTime, DateTime compareTime)
    {
      bool _response = false;
      if ((currentTime.Hour == compareTime.Hour) && (currentTime.Minute == compareTime.Minute))
      {
        _response = true;
      }
      return _response;
    }
    #endregion

    #region isDateEquals
    ///<summary>Metodo que valida dos fechas sean iguales</summary>
    ///<param name="dateToday">Fecha que se desea comparar</param>
    ///<param name="dateCompare">Fecha con que se va comparar</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static bool isDateEquals(DateTime dateToday, DateTime dateCompare)
    {
      bool status = false;
      if (dateToday.ToString("d") == dateCompare.ToString("d"))
      {
        status = true;
      }
      return status;
    }
    #endregion

    #region AddTimeDate
    ///<summary>Metodo que agrega tiempo a hora actual</summary>
    ///<param name="addTime">Horas que se desean aumentar al dia</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static DateTime AddTimeDate(TimeSpan addTime)
    {
      DateTime dateAfter = DateTime.Now.AddHours(addTime.Hours).AddMinutes(addTime.Minutes).AddSeconds(addTime.Seconds);
      return dateAfter;

    }
    #endregion

    #region timeDuration
    ///<summary>Metodo que retorna el tiempo transcurrido entre dos Datetime</summary>
    ///<param name="dateFirst">Tiempo de inicio</param>
    ///<param name="dateEnd">Tiempo donde termina</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static TimeSpan timeDuration(DateTime dateFirst, DateTime dateEnd)
    {
      return dateEnd.Subtract(dateFirst).Duration();
    }
    #endregion

    #region DaysBeforeOrAfter
    ///<summary>Metodo que agrega o resta días a una fecha</summary>
    ///<param name="date">Fecha que se agregara los dias</param>
    ///<param name="days">Cantidad de dias ha agregar</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static DateTime DaysBeforeOrAfter(int days, DateTime? date = null)
    {
      if (date == null)
      {
        date = DateTime.Now;
      }
      DateTime dateBefore = date.Value.AddDays(days);
      return dateBefore;
    }
    #endregion

    #region GetMonthName
    ///<summary>Metodo retorna el nombre del mes</summary>
    ///<param name="monthNumber">Numero de mes</param>
    ///<history>
    ///[michan] 14/04/2016 Created
    ///</history>
    public static string GetMonthName(int monthNumber)
    {
      try
      {
        DateTimeFormatInfo dateFormat = CultureInfo.CurrentCulture.DateTimeFormat;
        string MonthName = dateFormat.GetMonthName(monthNumber);
        return MonthName;
      }
      catch
      {
        return "Desconocido";
      }
    }
    #endregion

    #region GetAge
    /// <summary>
    /// Metodo que calcula la edad de un huesped
    /// </summary>
    /// <param name="BirthDate"></param>
    /// <returns>Byte</returns>
    public static byte GetAge(DateTime? BirthDate = null)
    {
      //Obtengo la diferencia en años.
      byte age = 0;
      DateTime today = DateTime.Now;
      if (BirthDate != null && BirthDate < today)
      {
        int year = today.Year - BirthDate.Value.Year;
        //Obtengo la fecha de cumpleaños de este año.
        BirthDate = BirthDate.Value.AddYears(year);
        //Le resto un año si la fecha actual es anterior 
        //al día de nacimiento.
        if (today.CompareTo(BirthDate) > 0)
        {
          year--;
        }
        age = (byte)year;
      }
      return age;
    }
    #endregion

    #region IsDefaultDate 
    /// <summary>
    /// Indica si una fecha es la fecha default
    /// </summary>
    /// <param name="BirthDate"></param>
    /// <returns>bool</returns>
    /// <history>
    /// [michan]  20/04/2016 created
    /// </history>
    public static DateTime? IsDefaultDate(DateTime BirthDate)
    {
      //si no es la fecha default (01/01/1900)
      DateTime? date = null;      
      if (BirthDate.Year > 1900)
      {
        date = BirthDate;
      }
      return date;
    }
    #endregion

    #region StringDate
    /// <summary>
    /// Devuelve una etiqueta de rango de fechas.
    /// </summary>
    /// <param name="dtmStart">Fecha de inicio</param>
    /// <param name="dtmEnd">Fecha de fin</param>
    /// <returns>Retorna la etiqueta en un string </returns>
    /// <history>
    /// [michan]  22/04/2016 created
    /// </history>
    public static string StringDate(DateTime dtmStart, DateTime? dtmEnd = null)
    {
      string stringdate = String.Format("{0:MMM d, yyyy}", dtmStart); ;
      if (dtmEnd != null)
      {
        int result = dtmStart.CompareTo(dtmEnd.Value);
        if (result < 0)
        {
          stringdate = StringYear(dtmStart, dtmEnd.Value);
        }
        else if (result > 0)
        {
          stringdate = StringYear(dtmStart, dtmEnd.Value);
        }
      }
      return stringdate;
    }
    #endregion

    #region StringYear
    /// <summary>
    /// Retorna una etiqueta de un rango diferenciando el año.
    /// </summary>
    /// <param name="dtmStart">Fecha de inicio</param>
    /// <param name="dtmEnd">Fecha de fin</param>
    /// <returns>Etiqueta en string de la fechas</returns>
    /// <history>
    /// [michan]  22/04/2016 created
    /// </history>
    public static string StringYear(DateTime dtmStart, DateTime dtmEnd)
    {
      int year = int.Parse(dtmStart.Year.ToString()).CompareTo(int.Parse(dtmEnd.Year.ToString()));
      if (year < 0)
      {
        return String.Format("{0:MMM d, yyyy}", dtmStart) + " to " + String.Format("{0:MMM d, yyyy}", dtmEnd);
      }
      else if (year == 0)
      {
        return StringMonth(dtmStart, dtmEnd);
      }
      else
      {
        return StringMonth(dtmStart, dtmEnd);
      }
    }
    #endregion

    #region StringMonth
    /// <summary>
    /// Etiqueta de un rango de fechas diferenciando el mes.
    /// </summary>
    /// <param name="dtmStart">Fecha de inicio</param>
    /// <param name="dtmEnd">Fecha de fin</param>
    /// <returns>Retorna la equiqueta en string</returns>
    /// <history>
    /// [michan]  22/04/2016 created
    /// </history>
    public static string StringMonth(DateTime dtmStart, DateTime dtmEnd)
    {
      int month = int.Parse(dtmStart.Month.ToString()).CompareTo(int.Parse(dtmEnd.Month.ToString()));
      
      if (month < 0)
      {
        return String.Format("{0:MMM d}", dtmStart) + " to " + String.Format("{0:MMM d, yyyy}", dtmEnd);
      }
      else if (month > 0)
      {
        return String.Format("{0:d}", dtmEnd) + " to " + String.Format("{0:MMM d, yyyy}", dtmStart);
      }
      else
      {
        return String.Format("{0:d}", dtmStart) + " to " + String.Format("{0:MMM d, yyyy}", dtmEnd);

      }
    }
    #endregion
  }
}
