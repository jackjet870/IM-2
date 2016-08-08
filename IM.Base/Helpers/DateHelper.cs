using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using IM.Model.Enums;
using IM.BusinessRules.BR;
using System.Windows.Controls;
using System.Windows;
using Xceed.Wpf.Toolkit;
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
      System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
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
    /// [wtorres] 25/Jul/2016 Modified. Correccion de error en el formato de meses incompletos
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
      //Si son diferentes meses
      else if (dateFrom.Month != dateTo.Month)
      {
        //Año completo
        if (dateFrom.Day ==1 && dateFrom.Month==1 && dateTo.Day==31 && dateTo.Month==12)
        {
          dateRange = string.Format("{0:yyyy}", dateFrom);
        }
        //Meses completos
        else if (dateFrom.Day==1 && dateTo.Day== DateTime.DaysInMonth(dateTo.Year,dateTo.Month))
        {
          dateRange = string.Format("{0:yyyy-MM} to {1:MM}", dateFrom, dateTo);
        }
        //Meses incompletos
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd} to {1:MM-dd}", dateFrom, dateTo);
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
        //Mes incompleto
        else
        {
          dateRange = string.Format("{0:yyyy-MM-dd} to {1:dd}", dateFrom,dateTo);
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
      if(date == null) date = DateTime.Now;
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
      int olds = 0;
      if (BirthDate != null)
      {
        DateTime today = DateTime.Now;
        //si no es la fecha default
        if ((IsDefaultDate(BirthDate.Value) != null) && (BirthDate.Value < today))
        {
          olds = today.Year - BirthDate.Value.Year;
          //Obtengo la fecha de cumpleaños de este año.
          BirthDate = BirthDate.Value.AddYears(olds);
          //Le resto un año si la fecha actual es anterior  al día de nacimiento.
          if (today < BirthDate.Value) olds--;
        }
      }
      return (byte)((olds < 0 || olds > 100) ? 0 : olds);
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
    public static DateTime? IsDefaultDate(DateTime? BirthDate = null)
    {
      //si no es la fecha default (01/01/1900)
      DateTime? date = null;
      if (BirthDate != null)
      {
        if(BirthDate.Value.Year > 1900) date = BirthDate;
      }      
      return date;
    }
    #endregion

    #region GetDateRange
    /// <summary>
    /// Devuelve una fecha inicial y una fecha final
    /// dependiendo del EnumPredefinedDate seleccionado
    /// </summary>
    /// <param name="dateSelected"></param>
    /// <returns></returns>
    public static Tuple<DateTime, DateTime> GetDateRange(EnumPredefinedDate dateSelected)
    {
      DateTime today = BRHelpers.GetServerDate();
      DateTime dtmStart;
      DateTime dtmEnd;
      switch (dateSelected)
      {
        case EnumPredefinedDate.Today:
          dtmStart = dtmEnd = today;
          break;

        case EnumPredefinedDate.Yesterday:
          dtmStart = dtmEnd = today.Date.AddDays(-1);
          break;

        case EnumPredefinedDate.ThisWeek:
          dtmStart = today.AddDays((DayOfWeek.Monday - today.DayOfWeek));
          dtmEnd = today.AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.PreviousWeek:
          dtmStart = today.AddDays(-7).AddDays(DayOfWeek.Monday - today.DayOfWeek);
          dtmEnd = today.AddDays(-7).AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.TwoWeeksAgo:
          dtmStart = today.AddDays(-14).AddDays(DayOfWeek.Monday - today.DayOfWeek);
          dtmEnd = today.AddDays(-14).AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.ThreeWeeksAgo:
          dtmStart = today.AddDays(-21).AddDays(DayOfWeek.Monday - today.DayOfWeek);
          dtmEnd = today.AddDays(-21).AddDays((DayOfWeek.Sunday - today.DayOfWeek) + 7);
          break;

        case EnumPredefinedDate.ThisHalf:
          if (today.Day <= 15)
          {
            dtmStart = new DateTime(today.Year, today.Month, 1);
            dtmEnd = new DateTime(today.Year, today.Month, 15);
          }
          else
          {
            dtmStart = new DateTime(today.Year, today.Month, 16);
            dtmEnd = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
          }
          break;

        case EnumPredefinedDate.PreviousHalf:

          if (today.Day <= 15)
          {
            if (today.Month > 1)
            {
              dtmStart = new DateTime(today.Year, today.Month - 1, 16);
              dtmEnd = new DateTime(today.Year, today.Month - 1, DateTime.DaysInMonth(today.Year, today.Month - 1));
            }
            else
            {
              dtmStart = new DateTime(today.Year - 1, 12, 16);
              dtmEnd = new DateTime(today.Year - 1, 12, 31);
            }
          }
          else
          {
            dtmStart = new DateTime(today.Year, today.Month, 1);
            dtmEnd = new DateTime(today.Year, today.Month, 15);
          }
          break;

        case EnumPredefinedDate.ThisMonth:
          dtmStart = new DateTime(today.Year, today.Month, 1);
          dtmEnd = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
          break;

        case EnumPredefinedDate.PreviousMonth:
          dtmStart = new DateTime(today.Year, today.Month - 1, 1);
          dtmEnd = new DateTime(today.Year, today.Month - 1, DateTime.DaysInMonth(today.Year, today.Month - 1));
          break;

        case EnumPredefinedDate.TwoMonthsAgo:
          dtmStart = new DateTime(today.Year, today.Month - 2, 1);
          dtmEnd = new DateTime(today.Year, today.Month - 2, DateTime.DaysInMonth(today.Year, today.Month - 2));
          break;

        case EnumPredefinedDate.ThreeMonthsAgo:
          dtmStart = new DateTime(today.Year, today.Month - 3, 1);
          dtmEnd = new DateTime(today.Year, today.Month - 3, DateTime.DaysInMonth(today.Year, today.Month - 3));
          break;

        case EnumPredefinedDate.ThisYear:
          dtmStart = new DateTime(today.Year, 1, 1);
          dtmEnd = new DateTime(today.Year, 12, 31);
          break;

        case EnumPredefinedDate.PreviousYear:
          dtmStart = new DateTime(today.Year - 1, 1, 1);
          dtmEnd = new DateTime(today.Year - 1, 12, 31);
          break;

        default:
          dtmStart = dtmEnd = today;
          break;
      }

      return Tuple.Create(dtmStart, dtmEnd);
    }
    #endregion

    #region ValidateValueDate

    /// <summary>
    /// Valida que sea correcta la fecha proporcionada
    /// Sobrecarga del metodo que usa solo el DatePicker comun de wpf
    /// </summary>
    /// <param name="sender">Objeto de tipo DataTimePicker</param>
    /// <history>
    ///   [ecanul] 28/07/2016 Created
    /// </history>
    public static void ValidateValueDate(DateTimePicker sender)
    {
      if(!sender.Value.HasValue)
      {
        //Cuando el usuario ingresa una fecha invalida
        UIHelper.ShowMessage("Invalid date", MessageBoxImage.Exclamation, "Specify the Date");
        //Y le asignamos la fecha del servidor (la actual hora actual)
        sender.Value = BRHelpers.GetServerDate();
      }
    }
    /// <summary>
    /// Sirve para validar la fecha de los controles DATETIMEPICKER
    /// Valida valores Nulos, y Rangos de fecha en el orden correcto.
    /// </summary>
    /// <param name="dtFrom">Fecha From</param>
    /// <param name="dtTo">Fecha To</param>
    /// <returns>True si pasó la validacion  || False en caso contrario</returns>
    /// <history>
    /// [erosado] Created.  20/07/2016
    /// </history>
    public static bool ValidateValueDate(DateTimePicker dtFrom, DateTimePicker dtTo)
    {
      bool validatePass = true;
      if (dtFrom == null && dtTo == null)
      {
        validatePass = false;
        UIHelper.ShowMessage("We can't get your selected date, please try again.", MessageBoxImage.Error, "Intelligence Marketing");
      }
      else if (dtFrom.Value.HasValue == false || dtTo.Value.HasValue == false)
      {
        validatePass = false;
        UIHelper.ShowMessage("Sorry we can't accept empty dates", MessageBoxImage.Error, "Intelligence Marketing");
      }
      else if (!(dtFrom?.Value.Value <= dtTo?.Value.Value))
      {
        validatePass = false;
        UIHelper.ShowMessage("End date must be greater than start date.", MessageBoxImage.Error, "Intelligence Marketing");
      }
      return validatePass;
    }
    #endregion
  }
}
